using Microsoft.Vbe.Interop;
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
    public partial class add_up_payment : Form
    {
        public NpgsqlConnection con;
        int id;
        public add_up_payment(NpgsqlConnection con, int id)
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
            if (id == -1)
            {
                string sql;
                sql = "insert into emp_proc_cost(id_emp,id_proc,cost) values (:id_emp,:id_proc,:cost);";
                NpgsqlCommand com = new NpgsqlCommand(sql, con);
                com = new NpgsqlCommand(sql, con);
                com.Parameters.AddWithValue("id_emp", empComboBox.SelectedValue);
                com.Parameters.AddWithValue("id_proc", procComboBox.SelectedValue);
                com.Parameters.AddWithValue("cost", costNumericUpDown.Value);
                com.ExecuteNonQuery();
               // Close();
            }
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

        private void more1Button_Click(object sender, EventArgs e)
        {
            emp_db f = new emp_db(con, "choose");
            this.Hide();
            f.ShowDialog();
            if (f.get_id() != -1)
            {
                empComboBox.SelectedValue = f.get_id();
            }
            this.Show();
        }

        private void add_up_payment_Load(object sender, EventArgs e)
        {
            string sql;


            sql = "select id,last_name||' '||first_name||' '||patronymic as fio from employee;";
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sql, con);
            DataTable dt1 = new DataTable();
            DataSet ds1 = new DataSet();
            ds1.Reset();
            da1.Fill(ds1);
            dt1 = ds1.Tables[0];
            empComboBox.DataSource = dt1;
            empComboBox.DisplayMember = "fio";
            empComboBox.ValueMember = "id";

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
        }
    }
}
