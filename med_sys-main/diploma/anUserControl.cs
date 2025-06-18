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
using System.Xml.Linq;
using System.Xml;

namespace diploma
{
    public partial class anUserControl : UserControl
    {



        NpgsqlConnection con;
        int id_pat;
        Size s;
        Color c;
        string mes_units, result, norm, fio, type_analysis, dev;

        private void divLabel_Click(object sender, EventArgs e)
        {

        }

        DateTime date_get;
        Color col;
        int num;

      

        public anUserControl(NpgsqlConnection con, int id_pat, Size s, string type_analysis, string result, string mes_units, string norm, string dev, DateTime date_get, Color col, int num, string fio)
        {
            this.con = con;
            this.s = s;
            InitializeComponent();
            this.id_pat = id_pat;
            this.type_analysis = type_analysis;
            this.result = result;
            this.mes_units = mes_units;
            this.norm = norm;
            this.dev = dev;
            this.col = col;
            this.num = num;
            this.fio = fio;
        }

        private void anUserControl_Load(object sender, EventArgs e)
        {
            this.Width = s.Width;
            this.BackColor = col;
            numLabel.Text = num.ToString() + "). ";
            dateGetLabel.Text = date_get.Date.ToShortDateString();
            resultLabel.Text = result;
            normLabel.Text = norm;
            divLabel.Text = dev;
            fioLabel.Text = fio;
            typeLabel.Text = type_analysis;
            fioLabel.Text = fio.ToString();
        }
    }
}
