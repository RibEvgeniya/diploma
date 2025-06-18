using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Npgsql;
using System.Security.Cryptography;

namespace diploma
{
    public partial class sign_in : Form
    {
        public NpgsqlConnection con;
        public string type_user;
        public sign_in(NpgsqlConnection con, string type_user)
        {
            this.con = con;
            InitializeComponent();
            this.type_user = type_user;
        }


        private void upd() 
        {
            if (type_user == "emp" || type_user == "admin")
            {
                if (admCheckBox.Checked)
                {

                    loginTextBox.Text = "Admin";
                    passTextBox.Text = "adminpass";
                    passTextBox.Text = "12345678";
                }
                else 
                {
                    loginTextBox.Text = "emp2";
                    passTextBox.Text = "12345678";
                }
            }
            else
            {
                loginTextBox.Text = "pat1";
                passTextBox.Text = "12345678";


            }
        }


        private void sign_in_Load(object sender, EventArgs e)
        {
            passTextBox.PasswordChar = '*';
            if(type_user!="emp")
                admCheckBox.Visible = false;
            upd();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
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

        public static string Get_passw(string pass)
        {
            byte[] for_hash = Create_for_pass();
            byte[] hashed_pass = GenerateMD5Hash(pass, for_hash);
            string hashed_pass_str = Convert.ToBase64String(hashed_pass);
            return hashed_pass_str;
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
        
            int id;
            
            if (type_user == "emp" || type_user == "admin")
            {
                try
                {
                    string sql = "Select id from employee where login=:login and h_password=:h_password";
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
                    da.SelectCommand.Parameters.AddWithValue("login", loginTextBox.Text);
                    string pass = passTextBox.Text;
                   
                    string hashed_pass_str = Get_passw(pass); 
                   
                    da.SelectCommand.Parameters.AddWithValue("h_password", hashed_pass_str);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count == 1)
                    {
                        NpgsqlDataAdapter cmd = new NpgsqlDataAdapter(sql, con);
                        cmd.SelectCommand.Parameters.AddWithValue("login", loginTextBox.Text);
                        cmd.SelectCommand.Parameters.AddWithValue("h_password", hashed_pass_str);
                        DataTable dt1 = new DataTable();
                        cmd.Fill(dt1);
                        DataRow dr = dt1.Select()[0];
                        id = dr.Field<int>(0);
                      
                        sql = "Select * from employee,post where employee.login=:login and employee.id_post=post.id and post.name='Администратор баз данных';";
                        da = new NpgsqlDataAdapter(sql, con);
                        da.SelectCommand.Parameters.AddWithValue("login", loginTextBox.Text);
                        DataTable dt2 = new DataTable();
                        da.Fill(dt2);
                    
                        if (dt2.Rows.Count == 1)
                        {
                            type_user = "admin";
                            admin_main f = new admin_main(con, type_user, id);
                            this.Hide();
                            f.ShowDialog();
                            this.Show();
                           
                        }
                        else 
                        {
                            type_user = "emp";
                            emp_menu f = new emp_menu(con, id,"menu");
                            this.Hide();
                            f.ShowDialog();
                            this.Show();
                       
                        }


                    }
                    else
                    {
                        guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                        guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                      
                        guna2MessageDialog1.Show("Неправильный логин или пароль! Обратитесь за подробной информацией к специалисту!", "Ошибка");
                     
                    }


                }
                catch (Exception ex)
                {
                    
                    MessageBox.Show($"Ошибка: {ex.Message}\n\nStackTrace:\n{ex.StackTrace}");
                }

            }
            else
            {
                try
                {
                    string sql = "Select id from patient where login=:login and h_password=:h_password";
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
                    da.SelectCommand.Parameters.AddWithValue("login", loginTextBox.Text);
                    string pass = passTextBox.Text;
                    //Console.WriteLine(loginTextBox.Text);
                    string hashed_pass_str = Get_passw(pass); 
                    //Console.WriteLine(hashed_pass_str);
                    da.SelectCommand.Parameters.AddWithValue("h_password", hashed_pass_str);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count == 1)
                    {
                        NpgsqlDataAdapter cmd = new NpgsqlDataAdapter(sql, con);
                        cmd.SelectCommand.Parameters.AddWithValue("login", loginTextBox.Text);
                        cmd.SelectCommand.Parameters.AddWithValue("h_password", hashed_pass_str);
                        DataTable dt1 = new DataTable();
                        cmd.Fill(dt1);
                        DataRow dr = dt1.Select()[0];
                        id = dr.Field<int>(0);
                        Console.WriteLine($"\nid пользователч: {id}");
                        pat_menu f = new pat_menu(con,id,"main",-1,-1);
                         this.Hide();
                        f.ShowDialog();
                        this.Show();


                    }
                    else
                    {
                        guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                        guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                        //guna2MessageDialog1.p
                        guna2MessageDialog1.Show("Неправильный логин или пароль! Обратитесь за подробной информацией к специалисту!", "Ошибка");
                        //enter = false;
                    }


                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Ошибка: {ex.Message}\n\nStackTrace:\n{ex.StackTrace}");
                }
  
            }
        }

        private void admCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            upd();
        }
    }
}
