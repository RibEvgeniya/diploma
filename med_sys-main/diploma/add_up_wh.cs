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

namespace diploma
{
    public partial class add_up_wh : Form
    {
        NpgsqlConnection con;
        int id;
        public List<string> Names { get; set; }
        public add_up_wh(NpgsqlConnection con, int id)
        {
            this.id = id;
            this.con = con;
            InitializeComponent();
        }

        private void update()
        {
            startDateTimePicker.Format = DateTimePickerFormat.Time;
            endDateTimePicker.Format = DateTimePickerFormat.Time;
            startDateTimePicker.ShowUpDown = true;
            endDateTimePicker.ShowUpDown = true;    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            string sql;
            sql = "select id,last_name||' '||first_name||' '||patronymic as fio from employee;";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            da = new NpgsqlDataAdapter(sql, con);
            DataTable dt2 = new DataTable();
            DataSet ds2 = new DataSet();
            ds2.Reset();
            da.Fill(ds2);
            dt2 = ds2.Tables[0];
            empComboBox.DataSource = dt2;
            empComboBox.DisplayMember = "fio";
            empComboBox.ValueMember = "id";


        }
        private void add_up_wh_Load(object sender, EventArgs e)
        {
            update();
            Names = new List<string>() { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };
            //weekComboBox.DataSource = Names;
            weekListBox.DataSource = Names;
        }


     

        private void enterButton_Click(object sender, EventArgs e)
        {
            if (this.id == -1)
            {
                try
                {
    

                    string sql;
                    foreach (string Id in weekListBox.SelectedItems)
                    {
                        sql = "insert into work_days(id_emp,day_week,time_start,time_end) values (:id_emp,:day_week,:time_start,:time_end);";
                        NpgsqlCommand com = new NpgsqlCommand(sql, con);
                        com = new NpgsqlCommand(sql, con);
                        com.Parameters.AddWithValue("id_emp", empComboBox.SelectedValue);
                        com.Parameters.AddWithValue("day_week", Id);
                        com.Parameters.AddWithValue("time_start", startDateTimePicker.Value);
                        com.Parameters.AddWithValue("time_end", endDateTimePicker.Value);
                        com.ExecuteNonQuery();
                    }

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

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void more2Button_Click(object sender, EventArgs e)
        {

        }
    }
}
