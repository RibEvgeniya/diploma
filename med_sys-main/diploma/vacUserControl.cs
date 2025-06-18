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
    public partial class vacUserControl : UserControl
    {
        NpgsqlConnection con;
        int id_pat;
        Size s;
        Color c;
        string name,name_dis;
        string manuf;
        Decimal cost;
        DateTime date_get,date_exp;
        Color col;
        int num;
        string fio;

        private void fioLabel_Click(object sender, EventArgs e)
        {

        }

        public vacUserControl(NpgsqlConnection con, int id_pat, Size s, string name, string name_dis,string manuf,Decimal cost, DateTime date_get, DateTime date_exp, Color col, int num,string fio)
        {
            this.con = con;
            this.s = s;
            InitializeComponent();
            this.id_pat = id_pat; this.name = name;
            this.name_dis = name_dis; this.manuf = manuf;
            this.cost = cost;
            this.date_get = date_get; this.date_exp = date_exp;
            this.col = col;
            this.num = num;
            this.fio = fio;
        }

        private void vacUserControl_Load(object sender, EventArgs e)
        {
            this.Width = s.Width;
            this.BackColor = col;
            numLabel.Text = num.ToString()+"). ";
            dateExpLabel.Text = date_exp.Date.ToShortDateString();
            dateGetLabel.Text = date_get.Date.ToShortDateString();
            nameLabel.Text = name;
            manufLabel.Text = manuf.ToString();
            costLabel.Text = cost.ToString();
            fioLabel.Text = fio.ToString();
            disLabel.Text=name_dis.ToString();
        }
    }
}
