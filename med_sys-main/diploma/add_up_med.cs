using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace diploma
{
    public partial class add_up_med : Form
    {
        public NpgsqlConnection con;
        int id;
        string name;
        string producer, release_form, descr;

         DataTable dt = new DataTable();
        DataSet ds = new DataSet();

        private void enterButton_Click(object sender, EventArgs e)
        {
            if (id == -1)
            {
                string sql;
                sql = "insert into Medication(name,producer,release_form,descr) values (:name,:producer,:release_form,:descr);";
                NpgsqlCommand com = new NpgsqlCommand(sql, con);
                com = new NpgsqlCommand(sql, con);
                com.Parameters.AddWithValue("name", nameTextBox.Text);
                com.Parameters.AddWithValue("producer", prodTextBox.Text);
                com.Parameters.AddWithValue("release_form", relTextBox.Text);
                com.Parameters.AddWithValue("descr", descRichTextBox.Text);
                com.ExecuteNonQuery();
                Close();
            }
        }

        private void add_up_med_Load(object sender, EventArgs e)
        {

        }

        public add_up_med(NpgsqlConnection con, int id, string name, string producer,string release_form,string descr)
        {
            InitializeComponent();
            this.con = con;
            this.id = id;
            if (id != -1)
            {
                this.name = name;
                this.producer = producer;
                this.release_form = release_form;   
                this.descr = descr; 

            }
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
