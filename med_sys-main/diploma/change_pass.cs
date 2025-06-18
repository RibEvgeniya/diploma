using Guna.UI2.WinForms;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Configuration;

namespace diploma
{
    public partial class change_pass : Form
    {
        public NpgsqlConnection con;
        string email;
        int code;
        bool flag = true;
        public change_pass(NpgsqlConnection con,string email)
        {
            InitializeComponent();
            this.con = con;
            this.email = email;
        }





        private void Send_em()
        {
            try
            {

                string emailUserName = ConfigurationManager.AppSettings["EmailUserName"];
                string emailPassword = ConfigurationManager.AppSettings["EmailPassword"];
                string smtpHost = ConfigurationManager.AppSettings["SmtpHost"];
                int smtpPort;
                if (!int.TryParse(ConfigurationManager.AppSettings["SmtpPort"], out smtpPort))
                {
                    
                    statusLabel.Text = "Ошибка конфигурации";
                    return;
                }

                if (string.IsNullOrEmpty(emailUserName) || string.IsNullOrEmpty(emailPassword) ||
                    string.IsNullOrEmpty(smtpHost))
                {
                    
                    statusLabel.Text = "Ошибка конфигурации";
                    return;
                }


                Random rnd = new Random();
                this.code = rnd.Next(10000, 99999);

             
                NetworkCredential cred = new NetworkCredential(emailUserName, emailPassword);

                MailMessage Msg = new MailMessage();
            
                Msg.From = new MailAddress(emailUserName);

                Msg.To.Add(new MailAddress(email)); 

                Msg.BodyEncoding = Encoding.UTF8;
                Msg.SubjectEncoding = Encoding.UTF8;
                Msg.Subject = "Код для подтверждения изменений";
                Msg.Body = "Ваш код подтверждения для изменения пароля:'" + code + "'";
                Msg.IsBodyHtml = true;

                SmtpClient client = new SmtpClient(smtpHost);
                client.Port = smtpPort;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = cred;
                client.Send(Msg);
                statusLabel.Text = "Сообщение было отправлено";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отправке сообщения: {ex.Message}");
                statusLabel.Text = "Ошибка";
            }
        }

      

        private void chpass_load()
        {
            this.Size = new Size(600, 400);
            if (this.flag)
            {
                firstGroupBox.Top = this.Height/2 - firstGroupBox.Height/2;
                firstGroupBox.Left = this.Width/2 - firstGroupBox.Width / 2;
                firstGroupBox.Visible = true;
                secondGroupBox.Visible = false;
                Send_em();
            }
            else
            {
                secondGroupBox.Top = this.Height / 2 - secondGroupBox.Height / 2;
                secondGroupBox.Left = this.Width / 2 - secondGroupBox.Width / 2;
                secondGroupBox.Location = new Point(24, 63);
                firstGroupBox.Visible = false;
                secondGroupBox.Visible = true;
                pass1TextBox.PasswordChar = '*';
                pass2TextBox.PasswordChar = '*';


            }


        }



        private void change_pass_Load(object sender, EventArgs e)
        {
            chpass_load();
        }


        private void back2Button_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(this.code) == codeTextBox.Text)
            {
                this.flag = false;
                chpass_load();

            }
            else
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Show("Код введен неверно!", "Повторите попытку");

            }
        }


        static byte[] Create_for_pass()
        {
            const int SaltLength = 64;
            byte[] salt = new byte[SaltLength];
            var rngRand = new RNGCryptoServiceProvider();
            rngRand.GetBytes(salt);
            return salt;
        }
        static byte[] GenerateMD5Hash(string password, byte[] salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltedPassword = new byte[salt.Length + passwordBytes.Length];
            var hash = new MD5CryptoServiceProvider();
            return hash.ComputeHash(saltedPassword);
        }

        private void enter2Button_Click(object sender, EventArgs e)
        {
            if (pass1TextBox.Text == pass2TextBox.Text)
            {
                string sql;
                sql = "Update patient set h_password=:h_password where email=:email;";
                NpgsqlCommand com = new NpgsqlCommand(sql, con);
                com.Parameters.AddWithValue("email", this.email);
                string pass = pass1TextBox.Text;
                byte[] for_hash = Create_for_pass();
                byte[] hashed_pass = GenerateMD5Hash(pass, for_hash);
                string h_password = Convert.ToBase64String(hashed_pass);
                com.Parameters.AddWithValue("h_password", h_password);

                com.ExecuteNonQuery();
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Show("Изменение пароля прошло успешно!", "Успех");
                Close();

            }
            else
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Show("Пароли не совпадают!", "Повторите попытку");

            }
        }

        private void againButton_Click(object sender, EventArgs e)
        {
            Send_em();
        }

        private void firstGroupBox_Click(object sender, EventArgs e)
        {

        }
    }
}
