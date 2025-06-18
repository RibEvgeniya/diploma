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
using Guna.UI2.WinForms;
using Npgsql;

namespace diploma
{
    public partial class sheduler : Form
    {
        int month, year;
        public static int static_month, static_year;
        NpgsqlConnection con;
        int id;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        DateTimeFormatInfo dtfi = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat;
        public sheduler(NpgsqlConnection con,int id)
        {
            InitializeComponent();
            this.con = con;
            this.id = id;
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void sheduler_Load(object sender, EventArgs e)
        {
            
          
            displayDays();
           
        }




        private void displayDays()
        {
            DateTime now = DateTime.Now;
            month = now.Month;
            year = now.Year;
            static_month = month;
            static_year = year;
            string monthname = dtfi.GetMonthName(month);
            monthLabel.Text = monthname + " " + year;


            updater();

        }


        private void updater()
        {

            static_month = month;
            static_year = year;
            DateTime now = DateTime.Now;
            DateTime startofthemonth = new DateTime(year, month, 1);
            int days = DateTime.DaysInMonth(year, month);
            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d"));

            Size s = new Size(Convert.ToInt32(shedLayoutPanel.Width / 7.5), shedLayoutPanel.Height / 5);
    
            for (int i = 1; i < dayoftheweek; i++)
            {
                blankUserControl ucblank = new blankUserControl(s);
                shedLayoutPanel.Controls.Add(ucblank);

            }
            for (int i = 1; i <= days; i++)
            {
                DateTime start = new DateTime(year, month, i);
                dayUserControl ucdays = new dayUserControl(con, id, start,s);
                ucdays.days(i);
                shedLayoutPanel.Controls.Add(ucdays);
            }

        }



        private void leftButton_Click(object sender, EventArgs e)
        {
            shedLayoutPanel.Controls.Clear();
            month--;
            if (month == 0) { month = 12; year--; }
            string monthname = dtfi.GetMonthName(month);
            monthLabel.Text = monthname + " " + year;
            updater();
        }

        private void rightButton_Click(object sender, EventArgs e)
        {
            shedLayoutPanel.Controls.Clear();
            month++;
            if (month == 13) { month = 1; year++; }
            string monthname = dtfi.GetMonthName(month);
            monthLabel.Text = monthname + " " + year;
            updater();
        }
    }
}
