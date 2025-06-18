using Npgsql;
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

namespace diploma
{
    public partial class add_up_pat : Form
    {
        public NpgsqlConnection con;
        int id;

        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        DataTable dt1 = new DataTable();
        DataSet ds1 = new DataSet();
        DataTable dt2 = new DataTable();
        DataSet ds2 = new DataSet();



        decimal height, weight;
        string act, blood, smok, alch, medAl, al, chrDis, other;


        public add_up_pat(NpgsqlConnection con, int id)
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
                    string sql = "Select * from patient where login=:login";
                    NpgsqlDataAdapter da3 = new NpgsqlDataAdapter(sql, con);
                    da3.SelectCommand.Parameters.AddWithValue("login", loginTextBox.Text);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    if (dt3.Rows.Count == 0)
                    {

                        string gender;
                        sql = "insert into Patient(polis,login,first_name,patronymic,last_name,email,phone,h_password,birthdate,passport_issued,passport_issued_in,seria,numb, gender,register_date,snils) values (:polis,:login,:first_name,:patronymic,:last_name,:email,:phone,:h_password,:birthdate,:passport_issued,:passport_issued_in,:seria,:numb, :gender,:register_date,:snils);";
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
                        com.Parameters.AddWithValue("polis", polisTextBox.Text);
                        com.Parameters.AddWithValue("seria", seriaTextBox.Text);
                        com.Parameters.AddWithValue("numb", numTextBox.Text);
                        com.Parameters.AddWithValue("passport_issued", issuedTextBox.Text);
                        com.Parameters.AddWithValue("passport_issued_in", issuedDateTimePicker.Value.Date);
                        com.Parameters.AddWithValue("snils", snilsTextBox.Text);
                        com.Parameters.AddWithValue("register_date", DateTime.Today.Date);
                        com.ExecuteNonQuery();





                        sql = "Select id from patient where login=:login and h_password=:h_password";
                        NpgsqlDataAdapter cmd = new NpgsqlDataAdapter(sql, con);
                        cmd.SelectCommand.Parameters.AddWithValue("login", loginTextBox.Text);
                        cmd.SelectCommand.Parameters.AddWithValue("h_password", hashed_pass_str);
                        dt2 = new DataTable();
                        cmd.Fill(dt2);
                        DataRow dr = dt2.Select()[0];
                        int id_pat = dr.Field<int>(0);
                       // Console.WriteLine($"\nациент: {id_pat}");





                        //add_photo();

                        sql = "Insert into images_pat(name,id_pat, image) values ('Фотография',:id,:image);";
                        NpgsqlCommand com1 = new NpgsqlCommand(sql, con);
                        com1.Parameters.AddWithValue("id", id_pat);

                        System.Drawing.Image img = Properties.Resources.icon;
                        ImageConverter converter = new ImageConverter();
                        byte[] arr = (byte[])converter.ConvertTo(img, typeof(byte[]));
                        com1.Parameters.AddWithValue("image", arr);
                        com1.ExecuteNonQuery();


                        if (medCheckBox.Checked)
                        {

                            sql = "insert into Patient_add_info(id_pat,height,weight,phys_activ, bloodType,smoking_status,alcohol,med_allergy,allergies,chronic_dis,other) values (:id_pat,:height,:weight,:phys_activ, :bloodType,:smoking_status,:alcohol,:med_allergy,:allergies,:chronic_dis,:other);";
                            com = new NpgsqlCommand(sql, con);
                            com.Parameters.AddWithValue("id_pat", id_pat);
                            com.Parameters.AddWithValue("height", height);
                            com.Parameters.AddWithValue("weight", weight);
                            com.Parameters.AddWithValue("phys_activ",act);
                            com.Parameters.AddWithValue("bloodType", blood);

                            com.Parameters.AddWithValue("smoking_status", smok);
                            com.Parameters.AddWithValue("alcohol", alch);
                            com.Parameters.AddWithValue("med_allergy", medAl);
                            com.Parameters.AddWithValue("allergies", al);
                            com.Parameters.AddWithValue("chronic_dis", chrDis);
                            com.Parameters.AddWithValue("other", other);
                            com.ExecuteNonQuery();


                        }






                        guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                        guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                        guna2MessageDialog1.Show("Пациент добавлен!", "Операция выполнена");
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
                    string sql = "Select * from patient where login=:login and id!=:id";
                    NpgsqlDataAdapter da3 = new NpgsqlDataAdapter(sql, con);
                    da3.SelectCommand.Parameters.AddWithValue("login", loginTextBox.Text);
                    da3.SelectCommand.Parameters.AddWithValue("id", this.id);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    if (dt3.Rows.Count == 0)
                    {

                        string gender;
                        sql = "UPDATE patient set polis=:polis,login=:login,first_name=:first_name,patronymic=:patronymic,last_name=:last_name,email=:email,phone=:phone, birthdate=:birthdate,passport_issued=:passport_issued,passport_issued_in=:passport_issued_in,seria=:seria,numb=:numb, gender=:gender,snils=:snils where (id=:id);";
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
                        com.Parameters.AddWithValue("polis", polisTextBox.Text);
                        com.Parameters.AddWithValue("seria", seriaTextBox.Text);
                        com.Parameters.AddWithValue("numb", numTextBox.Text);
                        com.Parameters.AddWithValue("passport_issued", issuedTextBox.Text);
                        com.Parameters.AddWithValue("passport_issued_in", issuedDateTimePicker.Value.Date);
                        com.Parameters.AddWithValue("snils", snilsTextBox.Text);
                        com.ExecuteNonQuery();


                        if (medCheckBox.Checked)
                        {
                            sql = "Select * from Patient_add_info where id_pat=:id ";
                            NpgsqlDataAdapter da4 = new NpgsqlDataAdapter(sql, con);
                            da4.SelectCommand.Parameters.AddWithValue("id", this.id);
                            DataTable dt4 = new DataTable();
                            da4.Fill(dt4);
                            if (dt4.Rows.Count != 0)
                            {

                             
                                sql = "update Patient_add_info set height=:height,weight=:weight,phys_activ=:phys_activ, bloodType=:bloodType,smoking_status=:smoking_status,alcohol=:alcohol,med_allergy=:med_allergy,allergies=:allergies,chronic_dis=:chronic_dis,other=:other where (id_pat=:id_pat);";
                                com = new NpgsqlCommand(sql, con);
                                com.Parameters.AddWithValue("id_pat", id);
                                com.Parameters.AddWithValue("height", height);
                                com.Parameters.AddWithValue("weight", weight);
                                com.Parameters.AddWithValue("phys_activ", act);
                                com.Parameters.AddWithValue("bloodType", blood);

                                com.Parameters.AddWithValue("smoking_status", smok);
                                com.Parameters.AddWithValue("alcohol", alch);
                                com.Parameters.AddWithValue("med_allergy", medAl);
                                com.Parameters.AddWithValue("allergies", al);
                                com.Parameters.AddWithValue("chronic_dis", chrDis);
                                com.Parameters.AddWithValue("other", other);
                                com.ExecuteNonQuery();
                            }
                            else {
                            
                                sql = "insert into Patient_add_info(id_pat,height,weight,phys_activ, bloodType,smoking_status,alcohol,med_allergy,allergies,chronic_dis,other) values (:id_pat,:height,:weight,:phys_activ, :bloodType,:smoking_status,:alcohol,:med_allergy,:allergies,:chronic_dis,:other);";
                                com = new NpgsqlCommand(sql, con);
                                com.Parameters.AddWithValue("id_pat", id);
                                com.Parameters.AddWithValue("height", height);
                                com.Parameters.AddWithValue("weight", weight);
                                com.Parameters.AddWithValue("phys_activ", act);
                                com.Parameters.AddWithValue("bloodType", blood);

                                com.Parameters.AddWithValue("smoking_status", smok);
                                com.Parameters.AddWithValue("alcohol", alch);
                                com.Parameters.AddWithValue("med_allergy", medAl);
                                com.Parameters.AddWithValue("allergies", al);
                                com.Parameters.AddWithValue("chronic_dis", chrDis);
                                com.Parameters.AddWithValue("other", other);
                                com.ExecuteNonQuery();

                            }

                        }











                        guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                        guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                        guna2MessageDialog1.Show("Успешное обновление данных!", "Операция выполнена");
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

        private void medButton_Click(object sender, EventArgs e)
        {
            add_up_more_info f = new add_up_more_info(con, id, -1, "pat");
            this.Hide();
            f.ShowDialog();
            if (f.get_compl())
            {

                medCheckBox.Checked = true;
                this.height=f.get_height();
                this.weight=f.get_weight();
                this.al=f.get_al();
                this.medAl=f.get_med_al();
                this.other=f.get_other();
                this.alch=f.get_alch();
                this.smok = f.get_smok();
                this.chrDis=f.get_chrDis();
                this.act = f.get_act();
                this.blood=f.get_blood();
                
            }
            this.Show();
            
        }


        private void for_upd(int id)
        {


            string sql = "Select emp.id,emp.first_name, emp.last_name, emp.patronymic,emp.login,emp.email,emp.phone,emp.snils, emp.birthdate,emp.gender," +
                " emp.polis from patient emp where emp.id=:id";
            NpgsqlDataAdapter cmd = new NpgsqlDataAdapter(sql, con);
            cmd.SelectCommand.Parameters.AddWithValue("id", this.id);
            DataTable dt1 = new DataTable();
            cmd.Fill(dt1);
            DataRow dr = dt1.Select()[0];
            nameTextBox.Text = dr.Field<string>(1);
            lastNameTextBox.Text = dr.Field<string>(2);
            middleNameTextBox.Text = dr.Field<string>(3);
            loginTextBox.Text = dr.Field<string>(4);
            //this.login = dr.Field<string>(4);

            emailTextBox.Text = dr.Field<string>(5);
            phoneTextBox.Text = dr.Field<string>(6);
            snilsTextBox.Text = dr.Field<string>(7);
            birthDateTimePicker.Value = dr.Field<DateTime>(8);
            if (dr.Field<string>(9) == femaleRadioButton.Text)
                femaleRadioButton.Checked = true;
            else maleRadioButton.Checked = true;
            polisTextBox.Text = dr.Field<string>(10);




            sql = "Select emp.seria ,emp.numb,emp.passport_issued ,emp.passport_issued_in from Patient emp where emp.id=:id";
            cmd = new NpgsqlDataAdapter(sql, con);
            cmd.SelectCommand.Parameters.AddWithValue("id", this.id);
            dt1 = new DataTable();
            cmd.Fill(dt1);
            dr = dt1.Select()[0];
            seriaTextBox.Text = dr.Field<string>(0);
            numTextBox.Text = dr.Field<string>(1);
            issuedTextBox.Text = dr.Field<string>(2);
            issuedDateTimePicker.Value = dr.Field<DateTime>(3);




        }
        private void add_up_pat_Load(object sender, EventArgs e)
        {
            if (this.id != -1)
            {
                passwordTextBox.Visible = false;
                label2.Visible = false;
               
                for_upd(this.id);


            }
        }
    }
}
