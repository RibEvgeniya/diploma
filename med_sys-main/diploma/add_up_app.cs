using Guna.UI2.WinForms;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace diploma
{
    public partial class add_up_app : Form
    {
        public NpgsqlConnection con;
        int id;
        public add_up_app(NpgsqlConnection con, int id)
        {
            this.con = con;
            InitializeComponent();
            this.id = id;
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            string sql;
            string status;
            if (this.id == -1)
            {
                try
                {
                    sql = "insert into appointment(id_emp,date_app,time_app,id_proc,id_spec,status) values (:id_emp,:date_app,:time_app,:id_proc,:id_spec,:status);";
                    NpgsqlCommand com = new NpgsqlCommand(sql, con);

                    com.Parameters.AddWithValue("id_emp", empComboBox.SelectedValue);
                    com.Parameters.AddWithValue("id_spec", patComboBox.SelectedValue);
                    com.Parameters.AddWithValue("id_proc", procComboBox.SelectedValue);
                    com.Parameters.AddWithValue("time_app", timeDateTimePicker.Value.TimeOfDay);
                    com.Parameters.AddWithValue("date_app", dateDateTimePicker.Value);
                    if (finRadioButton.Checked)
                        status = finRadioButton.Text;
                    else
                        status = inProcRadioButton.Text;
                    com.Parameters.AddWithValue("status", status);
                    com.ExecuteNonQuery();
                    guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog1.Show("Запись добавлена!", "Операция выполнена");
                    Close();

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

                    
                }
                catch
                {
                }
            }
        }

        private void add_up_app_Load(object sender, EventArgs e)
        {
            DateTime time = timeDateTimePicker.Value;
            dateDateTimePicker.ShowUpDown = false;
            timeDateTimePicker.ShowUpDown = true;
            //guna2DateTimePicker1.ShowUpDown = true;
            //time.AddHours(1);
            //guna2DateTimePicker2.Value = time;
            string sql;
            sql = "select id,last_name||' '||first_name||' '||patronymic as fio from patient;";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            DataTable dt1 = new DataTable();
            DataSet ds1 = new DataSet();
            ds1.Reset();
            da.Fill(ds1);
            dt1 = ds1.Tables[0];
            patComboBox.DataSource = dt1;
            patComboBox.DisplayMember = "fio";
            patComboBox.ValueMember = "id";

            sql = "select id,last_name||' '||first_name||' '||patronymic as fio from employee;";
            da = new NpgsqlDataAdapter(sql, con);
            DataTable dt2 = new DataTable();
            DataSet ds2 = new DataSet();
            ds2.Reset();
            da.Fill(ds2);
            dt2 = ds2.Tables[0];
            empComboBox.DataSource = dt2;
            empComboBox.DisplayMember = "fio";
            empComboBox.ValueMember = "id";

            sql = "select id,name from med_procedure;";
            da = new NpgsqlDataAdapter(sql, con);
            DataTable dt3 = new DataTable();
            DataSet ds3 = new DataSet();
            ds3.Reset();
            da.Fill(ds3);
            dt3 = ds3.Tables[0];
            procComboBox.DataSource = dt3;
            procComboBox.DisplayMember = "name";
            procComboBox.ValueMember = "id";
        }

        private void more2Button_Click(object sender, EventArgs e)
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

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            proc_db f = new proc_db(con, "look");
            this.Hide();
            f.ShowDialog();
            if (f.get_id() != -1)
            {
                procComboBox.SelectedValue = f.get_id();
            }
            this.Show();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            pat_db f = new pat_db(con, "choose");
            this.Hide();
            f.ShowDialog();
            if (f.get_id() != -1)
            {
                procComboBox.SelectedValue = f.get_id();
            }
            this.Show();
        }
    }
}
