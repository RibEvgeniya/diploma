using Guna.UI2.WinForms;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace diploma
{
    public partial class set_date_app : Form
    {
        public NpgsqlConnection con;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        int id_pat;
        int id_emp;
        CultureInfo ruRU = new CultureInfo("ru-RU");
        decimal cost;
        public set_date_app(NpgsqlConnection con, int id_pat, int id_emp, decimal cost)
        {
            this.con = con;
            InitializeComponent();
            this.id_pat = id_pat;
            this.id_emp = id_emp;
            this.cost = cost;
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void enterButton_Click(object sender, EventArgs e)
        {




        }

        private void moreButton_Click(object sender, EventArgs e)
        {

        }

        private void TimeButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button button = (System.Windows.Forms.Button)sender;
            DateTime clickedTime = (DateTime)button.Tag; 


            MessageBox.Show("Выбранное время: " + TimeSpan.Parse(clickedTime.ToString("HH:mm:ss")));





            string sql = "insert into appointment(id_emp,date_app,time_app,id_proc,id_spec,status) values (:id_emp,:date_app,:time_app,:id_proc,:id_spec,:status);";
            NpgsqlCommand com = new NpgsqlCommand(sql, con);

            com.Parameters.AddWithValue("id_emp", id_emp);
            com.Parameters.AddWithValue("id_spec", id_pat);
            com.Parameters.AddWithValue("id_proc", procComboBox.SelectedValue);
            com.Parameters.AddWithValue("time_app", TimeSpan.Parse(clickedTime.ToString("HH:mm:ss")));
            com.Parameters.AddWithValue("date_app", dateDateTimePicker.Value);
            com.Parameters.AddWithValue("status", "В процессе");
            com.ExecuteNonQuery();
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
            guna2MessageDialog1.Show("Успешная запись на прием!", "Операция выполнена");



            this.Close();
        }

        private void CreateTimeButtons(DateTime startTime, DateTime endTime)
        {
            timeLayoutPanel.Controls.Clear(); 
            if (DateTime.Today.Date == dateDateTimePicker.Value.Date) { 
                int mayb_start= DateTime.Now.Hour+1;
                if (mayb_start > startTime.Hour)
                {
                    startTime = DateTime.Now.AddHours(2);
                    startTime = startTime.AddMinutes(-startTime.Minute);
                    startTime = startTime.AddSeconds(-startTime.Second);

                }
               
            }

            for (DateTime currentTime = startTime; currentTime <= endTime; currentTime = currentTime.AddHours(1))
            {
                System.Windows.Forms.Button timeButton = new System.Windows.Forms.Button();
                timeButton.Text = currentTime.ToString("HH:mm:ss"); 
                timeButton.Tag = currentTime; 
                timeButton.Size = new Size(70, 30); 
                timeButton.Click += TimeButton_Click;
                if (!hour_ex(currentTime)) {
                    
                    timeLayoutPanel.Controls.Add(timeButton); }
              
            }
        }







        private bool hour_ex(DateTime hour) 
        {
           
            string count;
            String sql;
            NpgsqlDataAdapter cmd;
            sql = "Select count(*) from appointment a where (EXTRACT(HOUR FROM a.time_app))::integer =:time  and a.id_emp=:id and a.date_app=:date";
            //sql = "Select id from receptions";
            cmd = new NpgsqlDataAdapter(sql, con);
            Console.WriteLine(hour.Hour);
          
            cmd.SelectCommand.Parameters.AddWithValue("time", hour.Hour);
            cmd.SelectCommand.Parameters.AddWithValue("id",id_emp);
            cmd.SelectCommand.Parameters.AddWithValue("date",dateDateTimePicker.Value.Date);
            DataTable dt1 = new DataTable();
            cmd.Fill(dt1);
            DataRow dr1 = dt1.Select()[0];
            count = dr1[0].ToString();
            if (count != "0")
                return true;
            return false;
        }




        private void set_date_app_Load(object sender, EventArgs e)
        {
            costLabel.Text =cost.ToString();
            dateDateTimePicker.Value = DateTime.Now;

         
            enterButton.Visible = false;



            string sql = "Select proc.id,proc.name from Med_procedure proc, emp_proc_cost epc where epc.id_proc=proc.id and epc.id_emp=:id;";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            da.SelectCommand.Parameters.AddWithValue("id", this.id_emp);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            procComboBox.DataSource = dt;
            procComboBox.DisplayMember = "name";
            procComboBox.ValueMember = "id";





        }

        private void dateDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            timeLayoutPanel.Controls.Clear();
            if (DateTime.Now.Date <= dateDateTimePicker.Value.Date)
            {
                
                string sql = "Select wh.time_start,wh.time_end from employee emp, work_days wh where emp.id=wh.id_emp and emp.id=:id and wh.day_week=:day";
                NpgsqlDataAdapter cmd; cmd = new NpgsqlDataAdapter(sql, con);
                cmd.SelectCommand.Parameters.AddWithValue("id", this.id_emp);


                String dat = dateDateTimePicker.Value.ToString("dddd", new CultureInfo("ru-RU"));
                char first = char.ToUpper(dat[0]);
                string rest = dat.Substring(1);
                dat = first + rest;

         



                cmd.SelectCommand.Parameters.AddWithValue("day", dat.ToString());
                DataTable dt3 = new DataTable();
                cmd.Fill(dt3);

                int count = dt3.Rows.Count;
                if (count != 0)
                {
                    DataRow dr3 = dt3.Select()[0];
                    TimeSpan startTime = (TimeSpan)dr3[0];
                    TimeSpan endTime = (TimeSpan)dr3[1];

                   
                    DateTime dayS = DateTime.Today.Add(startTime);
                    DateTime dayE = DateTime.Today.Add(endTime);
                    //CreateTimeButtons(startTime, endTime);
                    CreateTimeButtons(dayS, dayE);
                }
                else
                {
                    Label lbl = new Label();
                    lbl.Text = "Нет возможности записаться на этот день";
                    lbl.AutoSize = true;
                    timeLayoutPanel.Controls.Add(lbl);
                }
            }
            else
            {
                Label lbl = new Label();
                lbl.Text = "Нельзя оформить запись на прошедший день";
                lbl.AutoSize = true;
                timeLayoutPanel.Controls.Add(lbl);
            }

        }

        private void backPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void procComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void procComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            string sql = "Select emc.cost  from employee emp, emp_proc_cost emc, med_procedure proc  where emp.id=emc.id_emp and emp.id=:id and emc.id_proc=proc.id and proc.name=:proc";
            NpgsqlDataAdapter cmd; cmd = new NpgsqlDataAdapter(sql, con);
            cmd.SelectCommand.Parameters.AddWithValue("id", this.id_emp);
            Console.WriteLine(procComboBox.SelectedValue);
            cmd.SelectCommand.Parameters.AddWithValue("proc", procComboBox.SelectedText);


            DataTable dt3 = new DataTable();
            cmd.Fill(dt3);

       
        }
    }
}
