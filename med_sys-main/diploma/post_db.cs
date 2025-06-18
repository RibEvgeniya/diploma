using Guna.UI2.WinForms;
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
    public partial class post_db : Form
    {
        string type_acc;
        public NpgsqlConnection con;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        DataTable dt1 = new DataTable();
        DataSet ds1 = new DataSet();
        string name;
        int id;

        public post_db(NpgsqlConnection con, string type_acc)
        {
            this.con = con;
            this.type_acc = type_acc;
            InitializeComponent();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void update_post()
        {

            string sql = "Select post.id,post.name,post.descr from Post post";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            postDataGridView.DataSource = dt;
            postDataGridView.Columns[0].HeaderText = "ID";
            postDataGridView.Columns[1].HeaderText = "Название должности";
            postDataGridView.Columns[2].HeaderText = "Описание";

            this.StartPosition = FormStartPosition.CenterScreen;
            postDataGridView.Columns[0].Width = 20;

        }

      

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            var result = guna2MessageDialog1.Show("Вы точно хотите удалить данную запись?", "Удаление");
            if (result == DialogResult.Yes)
            {
                string sql = "Delete from Post where ID=:id";
                int id = (int)postDataGridView.CurrentRow.Cells["ID"].Value;
                NpgsqlCommand com = new NpgsqlCommand(sql, con);
                com.Parameters.AddWithValue("id", id);
                com.ExecuteNonQuery();
            }
            update_post();

        }


        private void search()
        {
            string searchTerm = searchTextBox.Text.ToLower();
            bsEmp.DataSource = dt;


            if (string.IsNullOrEmpty(searchTerm))
            {
                bsEmp.Filter = null;
                return;
            }

            try
            {
                DataTable filteredTable = dt.Clone();

                foreach (DataRow row in dt.Rows)
                {
                    bool rowMatches = false;

                    foreach (DataColumn column in dt.Columns)
                    {
                        object cellValue = row[column];

                        if (cellValue != DBNull.Value)
                        {
                            if (column.DataType == typeof(string))
                            {
                                if (cellValue.ToString().ToLower().Contains(searchTerm))
                                {
                                    rowMatches = true;
                                    break;
                                }
                            }
                            else if (column.DataType == typeof(DateTime))
                            {
                                DateTime dateValue = (DateTime)cellValue;
                                string dateString = dateValue.ToString("dd.MM.yyyy");
                                if (dateString.Contains(searchTerm))
                                {
                                    rowMatches = true;
                                    break;
                                }
                            }
                            else if (column.DataType == typeof(int) || column.DataType == typeof(long))
                            {
                                if (cellValue.ToString().Contains(searchTerm))
                                {
                                    rowMatches = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (rowMatches)
                    {

                        filteredTable.ImportRow(row);
                    }
                }
                bsEmp.DataSource = filteredTable;
                postDataGridView.DataSource = null;
                postDataGridView.DataSource = bsEmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при фильтрации: " + ex.Message);
            }
        }



        

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add_up_post f = new add_up_post(con, -1);
            this.Hide();
            f.ShowDialog();
            update_post();
            this.Show();
        }

        private void post_db_Load(object sender, EventArgs e)
        {
            if (type_acc == "look")
                specMenuStrip.Visible = false;

            postDataGridView.ColumnHeadersHeight = 30;
            empDataGridView.ColumnHeadersHeight = 30;
            обновитьToolStripMenuItem.Visible = false;


            update_post();
        }

        private void searchTextBox_TextChanged_1(object sender, EventArgs e)
        {
            search();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            search();
        }

        private void postDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (postDataGridView.CurrentRow.Cells[0].Value != null)
            {
                int id = (int)postDataGridView.CurrentRow.Cells[0].Value;
                string name = (string)postDataGridView.CurrentRow.Cells[1].Value;

                this.name = name;
                this.id = id;
                Close();
            }
        }

        public int get_id() 
        {
            return this.id;
        }
        public string get_name()
        {
            return this.name;
        }


        private void update_emp(int id_post)
        {

            string sql = "Select post.name,emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio,emp.login from Employee emp,post where post.id=:id and post.id=emp.id_post;";
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sql, con);
            da1.SelectCommand.Parameters.AddWithValue("id", id_post);
            ds1.Reset();
            da1.Fill(ds1);
            dt1 = ds1.Tables[0];
            empDataGridView.DataSource = dt1;
            empDataGridView.Columns[0].HeaderText = "Должность";
            empDataGridView.Columns[1].HeaderText = "ФИО работника";
            empDataGridView.Columns[2].HeaderText = "Логин";
        }

        private void postDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            int id_post;
            try
            {
                if (postDataGridView.CurrentRow != null)
                    id_post = (int)postDataGridView.CurrentRow.Cells["ID"].Value;
                else id_post = postDataGridView.RowCount;
                update_emp(id_post);
            }
            catch
            {
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
