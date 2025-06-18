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
    public partial class docUserControl : UserControl
    {
        public NpgsqlConnection con;
        int id_pat;
        int id_emp;
        Image photo;
        string fio;
        
        string exp;
        string spec;
        decimal price;



        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        public docUserControl(NpgsqlConnection con, int id_pat, int id_emp, Image photo, string fio,string spec, string exp, decimal price)
        {
            this.con = con;
            InitializeComponent();
            this.id_pat = id_pat;
            this.id_emp = id_emp;
            this.photo = photo;
            this.fio = fio;
            this.exp = exp;
            this.spec = spec;
            this.price = price;
        }

        private void docUserControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            set_date_app f = new set_date_app(con, id_pat,id_emp,price);
            //this.Hide();
            f.ShowDialog();
            this.Show();
            
        }

        private void docUserControl_Load(object sender, EventArgs e)
        {
            actualFioLabel.Text =fio;
            actualExpLabel.Text =exp;
            actualSpecLabel.Text = spec;
            actualCostLabel.Text = price.ToString();
            photoPictureBox.Image = photo;
            

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void actualFioLabel_Click(object sender, EventArgs e)
        {

        }

        private void docUserControl_MouseHover(object sender, EventArgs e)
        {
            this.BackColor = Color.SandyBrown;
        }

        private void docUserControl_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.BlanchedAlmond;
        }

        private void actualSpecLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
