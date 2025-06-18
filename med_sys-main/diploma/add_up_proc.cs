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
    public partial class add_up_proc : Form
    {
        public NpgsqlConnection con;
        int id;
        public add_up_proc(NpgsqlConnection con, int id)
        {
            this.con = con;
            InitializeComponent();
            this.id = id;
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            string sql;
            sql = "insert into med_procedure(name,descr,duration) values (:name,:descr,:duration);";
            NpgsqlCommand com = new NpgsqlCommand(sql, con);
            com = new NpgsqlCommand(sql, con);
            com.Parameters.AddWithValue("name", nameTextBox.Text);
            com.Parameters.AddWithValue("descr", descRichTextBox.Text);
            com.Parameters.AddWithValue("duration", timeNumericUpDown.Value);
            com.ExecuteNonQuery();
            Close();
        }

        private void add_up_proc_Load(object sender, EventArgs e)
        {

        }
    }
}
