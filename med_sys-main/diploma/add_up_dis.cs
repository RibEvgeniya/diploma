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
    public partial class add_up_dis : Form
    {
        public NpgsqlConnection con;
        int id;
        public add_up_dis(NpgsqlConnection con, int id)
        {
            this.con = con;
            InitializeComponent();
            this.id = id;
        }

        private void add_up_dis_Load(object sender, EventArgs e)
        {

        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            string sql;
            sql = "insert into Diseases(name,descr,mkb_code,symptoms,treatment) values (:name,:descr,:mkb,:sym,:treat);";
            NpgsqlCommand com = new NpgsqlCommand(sql, con);
            com = new NpgsqlCommand(sql, con);
            com.Parameters.AddWithValue("name", nameTextBox.Text);
            com.Parameters.AddWithValue("descr", descRichTextBox.Text);
            com.Parameters.AddWithValue("mkb", codeTextBox.Text);
            com.Parameters.AddWithValue("sym", symRichTextBox.Text);
            com.Parameters.AddWithValue("treat", treatRichTextBox.Text);
            com.ExecuteNonQuery();
            Close();
        }
    }
}
