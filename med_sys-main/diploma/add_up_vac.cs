using Microsoft.Vbe.Interop;
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
using System.Xml.Linq;

namespace diploma
{
    public partial class add_up_vac : Form
    {
        public NpgsqlConnection con;
        int id;
        public add_up_vac(NpgsqlConnection con, int id)
        {
            this.con = con;
            InitializeComponent();
            this.id = id;
        }


   

        private void add_up_vac_Load(object sender, EventArgs e)
        {
            string sql;


            sql = "select id,name from diseases;";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            DataTable dt2 = new DataTable();
            DataSet ds2 = new DataSet();
            ds2.Reset();
            da.Fill(ds2);
            dt2 = ds2.Tables[0];
            disComboBox.DataSource = dt2;
            disComboBox.DisplayMember = "name";
            disComboBox.ValueMember = "id";
        }



 

        private void enterButton_Click(object sender, EventArgs e)
        {
            string sql;
            sql = "insert into vaccine(name,descr,term,manufacturer,id_dis,cost) values (:name,:descr,:duration,:manufacturer,:id_dis,:cost);";
            NpgsqlCommand com = new NpgsqlCommand(sql, con);
            com = new NpgsqlCommand(sql, con);
            com.Parameters.AddWithValue("name", nameTextBox.Text);
            com.Parameters.AddWithValue("manufacturer",prodTextBox.Text);
            com.Parameters.AddWithValue("descr", descRichTextBox.Text);
            com.Parameters.AddWithValue("duration", termNumericUpDown.Value);
            com.Parameters.AddWithValue("id_dis",disComboBox.SelectedValue);
            com.Parameters.AddWithValue("cost", costNumericUpDown.Value);
            com.ExecuteNonQuery();
            Close();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void moreButton_Click(object sender, EventArgs e)
        {
            dis_db f = new dis_db(con, "look");
            this.Hide();
            f.ShowDialog();
            if (f.get_name() != "")
            {
                disComboBox.SelectedValue = f.get_id();
            }
            this.Show();
        }

        private void backPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
