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
    public partial class add_up_assign_proc : Form
    {
        public NpgsqlConnection con;
        int id;
        public add_up_assign_proc(NpgsqlConnection con, int id)
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
            sql = "insert into spec_proc(id_proc,id_spec) values (:id_proc,:id_spec);";
            NpgsqlCommand com = new NpgsqlCommand(sql, con);
            com = new NpgsqlCommand(sql, con);
            com.Parameters.AddWithValue("id_proc", procComboBox.SelectedValue);
            com.Parameters.AddWithValue("id_spec", specComboBox.SelectedValue);
            com.ExecuteNonQuery();
            //Close();
        }

        private void add_up_assign_proc_Load(object sender, EventArgs e)
        {
            string sql;


            sql = "select id,name from med_procedure;";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            DataTable dt2 = new DataTable();
            DataSet ds2 = new DataSet();
            ds2.Reset();
            da.Fill(ds2);
            dt2 = ds2.Tables[0];
            procComboBox.DataSource = dt2;
            procComboBox.DisplayMember = "name";
            procComboBox.ValueMember = "id";

            sql = "select id,name from specialisation;";
            da = new NpgsqlDataAdapter(sql, con);
            DataTable dt3 = new DataTable();
            DataSet ds3 = new DataSet();
            ds3.Reset();
            da.Fill(ds3);
            dt3 = ds3.Tables[0];
            specComboBox.DataSource = dt3;
            specComboBox.DisplayMember = "name";
            specComboBox.ValueMember = "id";
        }

        private void moreButton_Click(object sender, EventArgs e)
        {
            spec_db f = new spec_db(con, "look");
            this.Hide();
            f.ShowDialog();
            if (f.get_id() != -1)
            {
                specComboBox.SelectedValue = f.get_id();
            }
            this.Show();
        }

        private void more2Button_Click(object sender, EventArgs e)
        {
            proc_db f = new proc_db(con, "look");
            this.Hide();
            f.ShowDialog();
            if (f.get_id() != -1)
            {
                procComboBox.SelectedValue = f.get_id();
            }
            this.Show();
        }
    }
}
