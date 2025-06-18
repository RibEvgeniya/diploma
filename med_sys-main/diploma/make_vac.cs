using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace diploma
{
    public partial class make_vac : Form
    {
        public NpgsqlConnection con;
        int id_emp;
        int id_pat;

        public make_vac(NpgsqlConnection con, int id_emp, int id_pat)
        {
            this.con = con;
            InitializeComponent();
            this.id_emp = id_emp;
            this.id_pat = id_pat;

        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void make_vac_Load(object sender, EventArgs e)
        {
            dateDateTimePicker.Value = DateTime.Today;
            
            
            string sql;

            sql = "select id,last_name||' '||first_name||' '||patronymic as fio from employee;";
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sql, con);
            DataTable dt1 = new DataTable();
            DataSet ds1 = new DataSet();
            ds1.Reset();
            da1.Fill(ds1);
            dt1 = ds1.Tables[0];
            empComboBox.DataSource = dt1;
            empComboBox.DisplayMember = "fio";
            empComboBox.ValueMember = "id";

            if (id_emp != -1)
            { empComboBox.SelectedItem = id_emp; empComboBox.Enabled = false; }

            sql = "select id,last_name||' '||first_name||' '||patronymic as fio from patient;";
            NpgsqlDataAdapter da2 = new NpgsqlDataAdapter(sql, con);
            DataTable dt2 = new DataTable();
            DataSet ds2 = new DataSet();
            ds2.Reset();
            da2.Fill(ds2);
            dt2 = ds2.Tables[0];
            patComboBox.DataSource = dt2;
            patComboBox.DisplayMember = "fio";
            patComboBox.ValueMember = "id";

            if (id_pat != -1)
            { patComboBox.SelectedIndex = id_pat; patComboBox.Enabled = false; }

            sql = "select id,name from vaccine;";
            NpgsqlDataAdapter da3 = new NpgsqlDataAdapter(sql, con);
            DataTable dt3 = new DataTable();
            DataSet ds3 = new DataSet();
            ds3.Reset();
            da3.Fill(ds3);
            dt3 = ds3.Tables[0];
            vacComboBox.DataSource = dt3;
            vacComboBox.DisplayMember = "name";
            vacComboBox.ValueMember = "id";



            for_upd();

            empComboBox.Enabled = false;


        }



        private void for_upd()
        {
            string sql;
            if (id_emp != -1)
            {
                sql = "select id,last_name||' '||first_name||' '||patronymic as fio from employee where id=:id";
                NpgsqlDataAdapter cmd = new NpgsqlDataAdapter(sql, con);
                cmd.SelectCommand.Parameters.AddWithValue("id", this.id_emp);
                DataTable dt1 = new DataTable();
                cmd.Fill(dt1);
                DataRow dr = dt1.Select()[0];
                empComboBox.SelectedValue = dr.Field<int>(0);
            }
            if (id_pat != -1)
            {
                sql = "select id,last_name||' '||first_name||' '||patronymic as fio from patient where id=:id";
                NpgsqlDataAdapter cmd1 = new NpgsqlDataAdapter(sql, con);
                cmd1.SelectCommand.Parameters.AddWithValue("id", this.id_pat);
                DataTable dt2 = new DataTable();
                cmd1.Fill(dt2);
                DataRow dr1 = dt2.Select()[0];
                patComboBox.SelectedValue = dr1.Field<int>(0);
            }
        }








        private void more2Button_Click(object sender, EventArgs e)
        {
            pat_db f = new pat_db(con, "choose");
            this.Hide();
            f.ShowDialog();
            if (f.get_id() != -1)
            {
                patComboBox.SelectedValue = f.get_id();
            }
            this.Show();
        }

        private void more3Button_Click(object sender, EventArgs e)
        {
            vac_db f = new vac_db(con, "look");
            this.Hide();
            f.ShowDialog();
            if (f.get_id() != -1)
            {
                vacComboBox.SelectedValue = f.get_id();
            }
            this.Show();
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            string sql;
            sql = "insert into vaccination(date_get,id_vac,id_emp,id_pat,status_vac) values (:date_get,:id_vac,:id_emp,:id_pat,:status_vac);";
            NpgsqlCommand com = new NpgsqlCommand(sql, con);
            com = new NpgsqlCommand(sql, con);
            com.Parameters.AddWithValue("date_get", dateDateTimePicker.Value);
            com.Parameters.AddWithValue("id_vac", vacComboBox.SelectedValue);
            com.Parameters.AddWithValue("id_pat", patComboBox.SelectedValue);
            com.Parameters.AddWithValue("id_emp", empComboBox.SelectedValue);
            com.Parameters.AddWithValue("status_vac", " Действительна");
            com.ExecuteNonQuery();
            Close();
        }

        private void vacComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            vacTextBox.Text = "";


            string sql = "Select v.id,v.name,v.descr, v.term,d.name,v.manufacturer,v.cost from vaccine v, diseases d where v.id_dis=d.id and v.id=@id"; // @id вместо :id

            NpgsqlDataAdapter cmd4 = new NpgsqlDataAdapter(sql, con);

            if (vacComboBox.SelectedValue != null && int.TryParse(vacComboBox.SelectedValue.ToString(), out int idValue))
            {
                cmd4.SelectCommand.Parameters.AddWithValue("@id", idValue); 


                DataTable dt4 = new DataTable();
                cmd4.Fill(dt4);

                if (dt4.Rows.Count > 0) 
                {
                    DataRow dr4 = dt4.Rows[0]; 
                    vacTextBox.Text += "Название: " + dr4.Field<string>(1)+ "\n\n"; 
                    vacTextBox.Text += "Описание: " + dr4.Field<string>(2) + "\n\n";
                    vacTextBox.Text += "Изготовитель: " + dr4.Field<string>(5) + "\n\n";
                    vacTextBox.Text += "Болезнь: " + dr4.Field<string>(4) + "\n\n";
                    vacTextBox.Text += "Действие(в месяцах): " + dr4.Field<int>(3) + "\n\n";
                   // vacTextBox.Text += "Стоимость: " + dr4.Field<Decimal>(6).ToString() + "\n\n";
                    DateTime exp_date = dateDateTimePicker.Value.AddMonths(dr4.Field<int>(3));
                    vacTextBox.Text += "Действие до: " + exp_date.ToShortDateString() + "\n";
                }
                else
                {
                    vacTextBox.Text += "Данные не найдены.";
                }
            }




        }

        private void dateDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            vacTextBox.Text = "";


            string sql = "Select v.id,v.name,v.descr, v.term,d.name,v.manufacturer,v.cost from vaccine v, diseases d where v.id_dis=d.id and v.id=@id"; // @id вместо :id

            NpgsqlDataAdapter cmd4 = new NpgsqlDataAdapter(sql, con);

            if (vacComboBox.SelectedValue != null && int.TryParse(vacComboBox.SelectedValue.ToString(), out int idValue))
            {
                cmd4.SelectCommand.Parameters.AddWithValue("@id", idValue);


                DataTable dt4 = new DataTable();
                cmd4.Fill(dt4);

                if (dt4.Rows.Count > 0)
                {
                    DataRow dr4 = dt4.Rows[0];
                    vacTextBox.Text += "Название: " + dr4.Field<string>(1) + "\n\n";
                    vacTextBox.Text += "Описание: " + dr4.Field<string>(2) + "\n\n";
                    vacTextBox.Text += "Изготовитель: " + dr4.Field<string>(5) + "\n\n";
                    vacTextBox.Text += "Болезнь: " + dr4.Field<string>(4) + "\n\n";
                    vacTextBox.Text += "Действие(в месяцах): " + dr4.Field<int>(3) + "\n\n";
                    vacTextBox.Text += "Стоимость: " + dr4.Field<Decimal>(6).ToString() + "\n\n";
                    DateTime exp_date = dateDateTimePicker.Value.AddMonths(dr4.Field<int>(3));
                    vacTextBox.Text += "Действие до: " + exp_date.ToShortDateString() + "\n";
                }
                else
                {
                    vacTextBox.Text += "Данные не найдены.";
                }
            }

        }
    }
}
