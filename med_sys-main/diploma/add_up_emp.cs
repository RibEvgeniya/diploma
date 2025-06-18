using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static TheArtOfDevHtmlRenderer.Adapters.RGraphicsPath;

namespace diploma
{
    public partial class add_up_emp : Form
    {
        public NpgsqlConnection con;
        int id;
        string fio;
        string login;
        string email;
        string phone;
        List<string> spec;
        string post;
        string edu;
        DateTime emp_date;
        DateTime birthdate;
        string passport_issued;
        DateTime passport_issued_in;
        string seria;
        string numb;
        string gender;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        DataTable dt1 = new DataTable();
        DataSet ds1 = new DataSet();
        DataTable dt2 = new DataTable();
        DataSet ds2 = new DataSet();
        public add_up_emp(NpgsqlConnection con,int id)
        {
            InitializeComponent();
            this.con = con;
            this.id = id;
            if (id != -1) 
            {
              
            
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

        public static string Get_passw(string pass)
        {
            byte[] for_hash = Create_for_pass();
            byte[] hashed_pass = GenerateMD5Hash(pass, for_hash);
            string hashed_pass_str = Convert.ToBase64String(hashed_pass);
            return hashed_pass_str;
        }
        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }




        


        private void enterButton_Click(object sender, EventArgs e)
        {
            if (this.id == -1)
            {
                try
                {
                    string sql = "Select * from employee where login=:login";
                    NpgsqlDataAdapter da3 = new NpgsqlDataAdapter(sql, con);
                    da3.SelectCommand.Parameters.AddWithValue("login", loginTextBox.Text);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    if (dt3.Rows.Count == 0)
                    {
                        
                        string gender;
                        sql = "insert into Employee(id_post,login,first_name,patronymic,last_name,email,phone,h_password,birthdate,passport_issued,passport_issued_in,seria,numb, gender,date_of_emp,education,experience) values (:id_post,:login,:first_name,:patronymic,:last_name,:email,:phone,:h_password,:birthdate,:passport_issued,:passport_issued_in,:seria,:numb, :gender,:date_of_emp,:education,:experience);";
                        NpgsqlCommand com = new NpgsqlCommand(sql, con);
                        com.Parameters.AddWithValue("login", loginTextBox.Text);
                        if (femaleRadioButton.Checked)
                            gender = femaleRadioButton.Text;
                        else
                            gender = maleRadioButton.Text;
                        com.Parameters.AddWithValue("gender", gender);
                        com.Parameters.AddWithValue("first_name", nameTextBox.Text);
                        com.Parameters.AddWithValue("patronymic", middleNameTextBox.Text);
                        com.Parameters.AddWithValue("last_name", lastNameTextBox.Text);
                        com.Parameters.AddWithValue("email", emailTextBox.Text);
                        com.Parameters.AddWithValue("phone", phoneTextBox.Text);
                        string password = passwordTextBox.Text;
                        string hashed_pass_str = Get_passw(password);
                        com.Parameters.AddWithValue("h_password", hashed_pass_str);
                        com.Parameters.AddWithValue("birthdate", birthDateTimePicker.Value.Date);
                        com.Parameters.AddWithValue("id_post", postComboBox.SelectedValue);
                        com.Parameters.AddWithValue("seria", seriaTextBox.Text);
                        com.Parameters.AddWithValue("numb", numTextBox.Text);
                        com.Parameters.AddWithValue("passport_issued", issuedTextBox.Text);
                        com.Parameters.AddWithValue("passport_issued_in", issuedDateTimePicker.Value.Date);
                        com.Parameters.AddWithValue("education", eduRichTextBox.Text);
                        com.Parameters.AddWithValue("date_of_emp", empDateTimePicker.Value.Date);
                        com.Parameters.AddWithValue("experience", expTextBox.Text);
                        com.ExecuteNonQuery();

                       

                        
                        List<int> selectedIds = new List<int>();
                        foreach (object item in specListBox.SelectedItems)
                        {
                           

                            if (item is DataRowView rowView && rowView["id"] != DBNull.Value)
                            {
                                selectedIds.Add((int)rowView["id"]);
                            }
                            else
                            {
                                Console.WriteLine("Ошибка получения ID из выбранного элемента.");
                            }
                        }
                        foreach (int id in selectedIds)
                        {
                            Console.WriteLine(id);
                          
                        }
                        sql = "Select id from employee where login=:login and h_password=:h_password";
                        NpgsqlDataAdapter cmd = new NpgsqlDataAdapter(sql, con);
                        cmd.SelectCommand.Parameters.AddWithValue("login", loginTextBox.Text);
                        cmd.SelectCommand.Parameters.AddWithValue("h_password", hashed_pass_str);
                        dt2 = new DataTable();
                        cmd.Fill(dt2);
                        DataRow dr = dt2.Select()[0];
                        int id_emp = dr.Field<int>(0);
                   
                        foreach (int Id in selectedIds)
                        {
                            sql = "insert into spec_emp(id_emp,id_spec) values (:id_emp,:id_spec);";
                            com = new NpgsqlCommand(sql, con);
                            com.Parameters.AddWithValue("id_emp", id_emp);
                            com.Parameters.AddWithValue("id_spec", Id);
                      
                            com.ExecuteNonQuery();
                        }







                        //add_photo();

                        sql = "Insert into images_emp(name,id_emp, image) values ('Фотография',:id,:image);";
                        NpgsqlCommand com1 = new NpgsqlCommand(sql, con);
                        com1.Parameters.AddWithValue("id", id_emp);

                        System.Drawing.Image img = Properties.Resources.icon;
                        ImageConverter converter = new ImageConverter();
                        byte[] arr = (byte[])converter.ConvertTo(img, typeof(byte[]));
                        com1.Parameters.AddWithValue("image", arr);
                        com1.ExecuteNonQuery();











                        guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                        guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                        guna2MessageDialog1.Show("Сотрудник добавлен!", "Операция выполнена");
                        Close();
                    }
                    else
                    {
                        guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                        guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                        guna2MessageDialog1.Show("Данный логин уже существует!", "Ошибка");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }
            else
            {
                try
                {
                    string sql = "Select * from employee where login=:login and id!=:id";
                    NpgsqlDataAdapter da3 = new NpgsqlDataAdapter(sql, con);
                    da3.SelectCommand.Parameters.AddWithValue("login", loginTextBox.Text);
                    da3.SelectCommand.Parameters.AddWithValue("id", this.id);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    if (dt3.Rows.Count == 0)
                    {

                        string gender;
                        sql = "UPDATE employee set id_post=:id_post,login=:login,first_name=:first_name,patronymic=:patronymic,last_name=:last_name,email=:email,phone=:phone, birthdate=:birthdate,passport_issued=:passport_issued,passport_issued_in=:passport_issued_in,seria=:seria,numb=:numb, gender=:gender,date_of_emp=:date_of_emp,education=:education,experience=:experience where (id=:id);";
                        NpgsqlCommand com = new NpgsqlCommand(sql, con);
                        com.Parameters.AddWithValue("id", this.id);
                        com.Parameters.AddWithValue("login", loginTextBox.Text);
                        if (femaleRadioButton.Checked)
                            gender = femaleRadioButton.Text;
                        else
                            gender = maleRadioButton.Text;
                        com.Parameters.AddWithValue("gender", gender);
                        com.Parameters.AddWithValue("first_name", nameTextBox.Text);
                        com.Parameters.AddWithValue("patronymic", middleNameTextBox.Text);
                        com.Parameters.AddWithValue("last_name", lastNameTextBox.Text);
                        com.Parameters.AddWithValue("email", emailTextBox.Text);
                        com.Parameters.AddWithValue("phone", phoneTextBox.Text);
                      
                        com.Parameters.AddWithValue("birthdate", birthDateTimePicker.Value.Date);
                        com.Parameters.AddWithValue("id_post", postComboBox.SelectedValue);
                        com.Parameters.AddWithValue("seria", seriaTextBox.Text);
                        com.Parameters.AddWithValue("numb", numTextBox.Text);
                        com.Parameters.AddWithValue("passport_issued", issuedTextBox.Text);
                        com.Parameters.AddWithValue("passport_issued_in", issuedDateTimePicker.Value.Date);
                        com.Parameters.AddWithValue("education", eduRichTextBox.Text);
                        com.Parameters.AddWithValue("date_of_emp", empDateTimePicker.Value.Date);
                        com.Parameters.AddWithValue("experience", expTextBox.Text);
                        com.ExecuteNonQuery();




                        List<int> selectedIds = new List<int>();
                        foreach (object item in specListBox.SelectedItems)
                        {
                           

                            if (item is DataRowView rowView && rowView["id"] != DBNull.Value)
                            {
                                selectedIds.Add((int)rowView["id"]);
                            }
                            else
                            {
                                Console.WriteLine("Ошибка получения ID из выбранного элемента.");
                            }
                        }
                        foreach (int id in selectedIds)
                        {
                            Console.WriteLine(id);
                           
                        }


                        sql = "delete from spec_emp where id_emp=:id_emp;";
                        com = new NpgsqlCommand(sql, con);
                        com.Parameters.AddWithValue("id_emp", this.id);

                        com.ExecuteNonQuery();

                        foreach (int Id in selectedIds)
                        {
                            sql = "insert into spec_emp(id_emp,id_spec) values (:id_emp,:id_spec);";
                            com = new NpgsqlCommand(sql, con);
                            com.Parameters.AddWithValue("id_emp", this.id);
                            com.Parameters.AddWithValue("id_spec", Id);
                           
                            com.ExecuteNonQuery();
                        }




                        guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                        guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                        guna2MessageDialog1.Show("Сотрудник обновлен!", "Операция выполнена");
                        Close();
                    }
                    else
                    {
                        guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                        guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                        guna2MessageDialog1.Show("Данный логин уже существует!", "Ошибка");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void profGroupBox_Click(object sender, EventArgs e)
        {

        }





        private void up_post() 
        {
            string sql = "Select * from Post;";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            postComboBox.DataSource = dt;
            postComboBox.DisplayMember = "name";
            postComboBox.ValueMember = "id";

        }



        private void for_upd(int id) 
        {


            string sql = "Select emp.id,emp.first_name, emp.last_name, emp.patronymic,emp.login,emp.email,emp.phone,emp.education, emp.birthdate,emp.gender, post.id,emp.date_of_emp,emp.experience from Employee emp,Post post where emp.id_post=post.id and emp.id=:id";
            NpgsqlDataAdapter cmd = new NpgsqlDataAdapter(sql, con);
            cmd.SelectCommand.Parameters.AddWithValue("id", this.id);
            DataTable dt1 = new DataTable();
            cmd.Fill(dt1);
            DataRow dr = dt1.Select()[0];
            nameTextBox.Text = dr.Field<string>(1);
            lastNameTextBox.Text = dr.Field<string>(2);
            middleNameTextBox.Text = dr.Field<string>(3);
            loginTextBox.Text = dr.Field<string>(4);
            this.login= dr.Field<string>(4);

            emailTextBox.Text = dr.Field<string>(5);
            phoneTextBox.Text = dr.Field<string>(6);
            eduRichTextBox.Text = dr.Field<string>(7);
            birthDateTimePicker.Value = dr.Field<DateTime>(8);
            if (dr.Field<string>(9) == femaleRadioButton.Text)
                femaleRadioButton.Checked = true;
            else maleRadioButton.Checked = true;
            postComboBox.SelectedValue = dr.Field<int>(10);
            if (!dr.IsNull(11)) 
            {
                empDateTimePicker.Value = dr.Field<DateTime>(11);
            }
            expTextBox.Text = dr.Field<string>(12);





            sql = "Select emp.seria ,emp.numb,emp.passport_issued ,emp.passport_issued_in from Employee emp where emp.id=:id";
            cmd = new NpgsqlDataAdapter(sql, con);
            cmd.SelectCommand.Parameters.AddWithValue("id", this.id);
            dt1 = new DataTable();
            cmd.Fill(dt1);
            dr = dt1.Select()[0];
            seriaTextBox.Text = dr.Field<string>(0);
            numTextBox.Text = dr.Field<string>(1);
            issuedTextBox.Text = dr.Field<string>(2);
            issuedDateTimePicker.Value= dr.Field<DateTime>(3);




        }


        private void add_up_emp_Load(object sender, EventArgs e)
        {
            
            passwordTextBox.PasswordChar = '*';
            string sql = "Select * from Post;";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            postComboBox.DataSource = dt;
            postComboBox.DisplayMember = "name";
           postComboBox.ValueMember = "id";

            sql = "Select * from Specialisation;";
            da = new NpgsqlDataAdapter(sql, con);
            ds1.Reset();
            da.Fill(ds1);
            dt1 = ds1.Tables[0];

            specListBox.DataSource = dt1;
            specListBox.DisplayMember = "name";
            specListBox.ValueMember = "id";



            if (this.id != -1) 
            {
                passwordTextBox.Visible = false;
                label2.Visible = false;
              
                for_upd(this.id);
                
            
            }
        }

        private void more2Button_Click(object sender, EventArgs e)
        {
            spec_db f = new spec_db(con, "look");
            this.Hide();
            f.ShowDialog();
            this.Show();
        }



        public void update_post(int id_s)
        {
            try
            {
                postComboBox.SelectedValue = id_s;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }



        private void moreButton_Click(object sender, EventArgs e)
        {
            post_db f = new post_db(con, "look");
            this.Hide();
            f.ShowDialog();
            if (f.get_name()!= "")
            {
                update_post(f.get_id());
            }
            this.Show();
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void postComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
