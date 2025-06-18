using Guna.UI2.WinForms;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace diploma
{
    public partial class dayUserControl : UserControl
    {
        NpgsqlConnection con;
        int id;
        DateTime start;
        string count;
        Size s;
        int rec_count = 0;
        DateTimeFormatInfo dtfi = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat;
        Color past;
        Size start_size;
        string stat = "close";
        public dayUserControl(NpgsqlConnection con, int id, DateTime start,Size start_size)
        {
            this.con = con;
            this.id = id;
            this.start = start;
            InitializeComponent();
            s = start_size;
            this.start_size = start_size;
        }


        private void update()
        {
            todayLabel.Visible = false;
            String sql;
            NpgsqlDataAdapter cmd;
            sql = "Select count(*) from appointment where date_app=:start and id_emp=:id";
            //sql = "Select id from receptions";
            cmd = new NpgsqlDataAdapter(sql, con);
            cmd.SelectCommand.Parameters.AddWithValue("start", this.start);
            cmd.SelectCommand.Parameters.AddWithValue("id", this.id);
            DataTable dt1 = new DataTable();
            cmd.Fill(dt1);
            DataRow dr1 = dt1.Select()[0];
            count = dr1[0].ToString();
            if (count != "0")
            {
                countLabel.Text = count;
                rec_count = int.Parse(count);     
                //Console.WriteLine("dddsdsda" + rec_count);
            }
            else
            {
                countLabel.Visible = false;
                label1.Visible = false;
                countLabel.Visible = false;
                listLayoutPanel.Enabled= false;

            }

          
            try
            {
                sql = "Select wh.day_week,wh.time_start,wh.time_end from employee emp, work_days wh where emp.id=wh.id_emp and emp.id=:id";
                cmd = new NpgsqlDataAdapter(sql, con);
                cmd.SelectCommand.Parameters.AddWithValue("id", this.id);
                DataTable dt3 = new DataTable();
                cmd.Fill(dt3);

                string current_us = start.ToString("dddd", new CultureInfo("ru-RU"));

                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    DataRow dr = dt3.Select()[i];
                    String week_day = dr.Field<string>(0);
                    if (current_us == week_day.ToLower())
                    {
                        this.BackColor = Color.GreenYellow;
                        past = this.BackColor;
                    }
                }
                if (this.BackColor != Color.GreenYellow)
                {
                    past = Color.AntiqueWhite;
                }
                if (this.start == DateTime.Today) {
                    todayLabel.Visible = true;
                    this.BackColor = Color.Thistle;
                    past = this.BackColor;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }










        public void days(int numday)
        {
            dateLabel.Text = numday + "";

        }













        private void update_list()
        {

        }

        private void dayUserControl_Load(object sender, EventArgs e)
        {
            this.Size = start_size;
            doneCheckBox.Visible = false;
            update();
            update_list();
            

        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void dayUserControl_MouseLeave(object sender, EventArgs e)
        {
            // listLayoutPanel.Controls.Clear();
            //this.Size = s;
            if (this.Size == s) 
                this.BackColor = past;
    
        }

        private void dayUserControl_MouseHover(object sender, EventArgs e)
        {
            if (this.BackColor == Color.GreenYellow)
            {
             
                this.BackColor = Color.PaleTurquoise;
                past = Color.GreenYellow;
            }
            else if (this.BackColor == Color.AntiqueWhite)
            {
                this.BackColor = Color.Coral;
                past = Color.AntiqueWhite;
            }
            else if ( this.start==DateTime.Today) {
                this.BackColor = Color.Aquamarine;
                past = Color.Thistle;
            }

        }

        private void listLayoutPanel_MouseHover(object sender, EventArgs e)
        {
            if (count != "0")
            {
                listLayoutPanel.Enabled = true;
                //this.Size = new Size(s.Width * 2, s.Height * 3);
            }
        }

        private void listLayoutPanel_MouseLeave(object sender, EventArgs e)
        {
            //this.Size = s;
        }

        private void dayUserControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.stat == "close")
            {
                this.stat = "open";

                if (this.BackColor == Color.GreenYellow)
                {
                    
                    this.BackColor = Color.PaleTurquoise;
                    past = Color.GreenYellow;
                }
                else if (this.BackColor == Color.AntiqueWhite)
                {
                    this.BackColor = Color.Coral;
                    past = Color.AntiqueWhite;
                }
                if (count != "0")
                {
                    listLayoutPanel.Enabled = true;
                    this.Size = new Size(s.Width * 2+10, s.Height * 3);
                }
                countLabel.Visible = false;

                if (count != "0")
                {
                    //this.Size = new Size(150, 170);
                    try
                    {
                        String sql;
                        NpgsqlDataAdapter cmd;
                        sql = "Select pat.last_name || ' ' || pat.first_name ||' ' ||pat.patronymic, app.time_app, app.status,pat.id,app.id from appointment app,patient pat where app.date_app=:start and pat.id=app.id_spec and app.id_emp=:id order by app.time_app";


                        cmd = new NpgsqlDataAdapter(sql, con);
                        cmd.SelectCommand.Parameters.AddWithValue("id", this.id);
                        cmd.SelectCommand.Parameters.AddWithValue("start", this.start);
                        DataTable dt1 = new DataTable();
                        cmd.Fill(dt1);
                        
                        label1.Visible = true;
                        label1.Text = "";
                        listLayoutPanel.Controls.Clear();//////
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {

                            Guna2CustomCheckBox cb = new Guna2CustomCheckBox();
                            cb.Size=doneCheckBox.Size;
                            cb.UncheckedState.FillColor=doneCheckBox.UncheckedState.FillColor;
                            cb.CheckedState.FillColor = doneCheckBox.CheckedState.FillColor;

                            DataRow dr = dt1.Select()[i];
                            String name = dr.Field<string>(0) + ".";
                            string status = dr.Field<string>(2);
                            int id_pat= dr.Field<int>(3);
                            int id_ap = dr.Field<int>(4);
                            TimeSpan tim = dr.Field<TimeSpan>(1);
                            LinkLabel lbl = new LinkLabel();
                            lbl.Text += Convert.ToString(i + 1) + ") " + name + " " + string.Format("{0:hh\\:mm}", (dr.Field<TimeSpan>(1))) + "\n\n";
                            lbl.AutoSize=true;
                            lbl.LinkColor=Color.Black;
                            listLayoutPanel.Controls.Add(lbl);
                            listLayoutPanel.Controls.Add(cb);
                            lbl.Tag = new List<int> { id_pat,id_ap };
                            lbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lbl_LinkClicked);
                            if (start < DateTime.Today.Date || (start == DateTime.Today.Date && tim<DateTime.Now.TimeOfDay)) 
                            {
                                lbl.LinkColor = Color.DarkBlue;
                                cb.Location = new Point(lbl.Location.X + lbl.Width,lbl.Location.Y-100);
                                cb.Visible = true;
                                cb.Checked = true;
                                cb.Enabled = false;

                            }
                            else
                            {
                                
                                cb.Location = new Point(lbl.Location.X + lbl.Width, lbl.Location.Y-100);
                                cb.Visible = true;
                                cb.Checked = false;
                            }
                            
                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }




            }
            else 
            {
                stat = "close";
                listLayoutPanel.Controls.Clear();
                this.Size = s;
                this.BackColor = past;
                if (count != "0")
                {
                    label1.Text = "Приемов";
                    countLabel.Visible = true;
                    listLayoutPanel.Enabled = false;
                }

            }
        }

        private void lbl_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            //this.Text = "sdfsfsfsf ";
            var list = ((LinkLabel)sender).Tag as List<int>;
            // int id_pat = (int)((LinkLabel)sender).Tag;
            int id_pat = list[0];
            int id_ap = list[1];
            Console.WriteLine(this.id);
            pat_menu f = new pat_menu(con, id_pat, "look", this.id,id_ap);
            this.Hide();
            f.ShowDialog();
            this.Show();
            
        }
    }
}
