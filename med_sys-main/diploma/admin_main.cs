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
    public partial class admin_main : Form
    {
        public NpgsqlConnection con;
        public string type_user;
        int id;
        public admin_main(NpgsqlConnection con, string type_user, int id)
        {
            this.con = con;
            InitializeComponent();
            this.type_user = type_user;
            this.id = id;
        }

        private void cabImageButton_Click(object sender, EventArgs e)
        {
            emp_menu f = new emp_menu(con, id, "menu");
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void admin_main_Load(object sender, EventArgs e)
        {

        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void empButton_Click(object sender, EventArgs e)
        {
            emp_db f = new emp_db(con, type_user);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void specButton_Click(object sender, EventArgs e)
        {
            spec_db f = new spec_db(con, "admin");
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void appButton_Click(object sender, EventArgs e)
        {
            app_db f = new app_db(con);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void workHourButton_Click(object sender, EventArgs e)
        {
            work_hours_db f = new work_hours_db(con);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void postButton_Click(object sender, EventArgs e)
        {
            post_db f = new post_db(con, "admin");
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void illButton_Click(object sender, EventArgs e)
        {
            dis_db f = new dis_db(con, "admin");
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void schemaButton_Click(object sender, EventArgs e)
        {
            schema_db f = new schema_db(con);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void appCostButton_Click(object sender, EventArgs e)
        {
            payment_db f = new payment_db(con, "admin");
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void procButton_Click(object sender, EventArgs e)
        {
            proc_db f = new proc_db(con, "admin");
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void medButton_Click(object sender, EventArgs e)
        {
            med_db f = new med_db(con, "admin");
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void vacButton_Click(object sender, EventArgs e)
        {
            
            vac_db f = new vac_db(con, "admin");
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void vacPeopButton_Click(object sender, EventArgs e)
        {
            vac_pat_db f = new vac_pat_db(con, "admin");
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void patButton_Click(object sender, EventArgs e)
        {
            pat_db f = new pat_db(con, type_user);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }
    }
}
