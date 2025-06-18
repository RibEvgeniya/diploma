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
    public partial class diagUserControl : UserControl
    {
        NpgsqlConnection con;
        int id_app;
        int id_emp;
        int id_pat;

        Size s;
        Color c;
        string fio;
        string proc;
        Decimal cost;
        List<String> dis;
        DateTime date;

        public diagUserControl(NpgsqlConnection con, int id_app, int id_emp, Size s, string fio,string proc, Decimal cost, List<String> dis, DateTime date,int id_pat)
        {
            this.con = con;
            this.id_app = id_app;
            this.id_emp = id_emp;
            this.s = s;
            InitializeComponent();
            this.fio = fio;
            this.cost = cost;
            this.dis = dis;
            this.proc = proc;
            this.date = date;
            this.id_pat = id_pat;
        }



        public void update()
        {

            dateLabel.Text = date.ToShortDateString();
            fioLabel.Text = fio;
            procLabel.Text = proc;
            costLabel.Text = cost.ToString();
            //richTextBox1.Text = dis;
            for (int i = 0; i < dis.Count; i++)
                richTextBox1.Text +=(i+1)+")"+ dis[i]+"\n";


        }

        private void diagUserControl_Load(object sender, EventArgs e)
        {
            this.Size = s;
            c= this.BackColor;
            update();
        }

        private void diagUserControl_MouseHover(object sender, EventArgs e)
        {
            this.BackColor = Color.Coral;
        }

        private void diagUserControl_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = c;
        }

        private void diagUserControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           
        }

        private void diagUserControl_DoubleClick(object sender, EventArgs e)
        {

            
            make_diag f = new make_diag(con, id_pat, id_emp, id_app, "look");
            this.Hide();
            f.ShowDialog();
            this.Show();
        }
    }
}
