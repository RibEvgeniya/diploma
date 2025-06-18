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
    public partial class add_up_spec : Form
    {
        public NpgsqlConnection con;
        int id;
        public add_up_spec(NpgsqlConnection con, int id)
        {
            this.con = con;
            InitializeComponent();
            this.id = id;
        }
      

        private void add_up_spec_Load(object sender, EventArgs e)
        {

        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            string sql;
            sql = "insert into specialisation(name,descr) values (:name,:descr);";
            NpgsqlCommand com = new NpgsqlCommand(sql, con);
            com = new NpgsqlCommand(sql, con);
            com.Parameters.AddWithValue("name", nameTextBox.Text);
            com.Parameters.AddWithValue("descr", descRichTextBox.Text);
            com.ExecuteNonQuery();
            Close();
        }

        private void backPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
