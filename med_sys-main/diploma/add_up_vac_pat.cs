using Microsoft.Vbe.Interop;
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
    public partial class add_up_vac_pat : Form
    {
        public NpgsqlConnection con;
        int id;
        int id_emp;
        int id_pat;

        public add_up_vac_pat(NpgsqlConnection con, int id,int id_emp, int id_pat)
        {
            this.con = con;
            InitializeComponent();
            this.id = id;
            this.id_emp = id_emp;
            this.id_pat = id_pat;

        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void add_up_vac_pat_Load(object sender, EventArgs e)
        {
            string sql;
            dateDateTimePicker.Value = DateTime.Now;

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

            if (id_emp!=-1)
                { empComboBox.SelectedIndex = id_emp; empComboBox.Enabled = false; }

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

            if (id_pat!=-1)
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
        }

        private void backPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void more1Button_Click(object sender, EventArgs e)
        {
            emp_db f = new emp_db(con, "choose");
            this.Hide();
            f.ShowDialog();
            if (f.get_id() != -1)
            {
                empComboBox.SelectedValue = f.get_id();
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
            com.Parameters.AddWithValue("status_vac"," Действительна");
            com.ExecuteNonQuery();
            Close();
        }

        private void more2Button_Click(object sender, EventArgs e)
        {
            //pat_db f = new pat_db(con, "choose");
            //this.Hide();
            //f.ShowDialog();
            //if (f.get_id() != -1)
            //{
            //    patComboBox.SelectedValue = f.get_id();
            //}
            //this.Show();
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
    }
}
