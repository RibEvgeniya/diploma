using Guna.UI2.WinForms;
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
    public partial class work_hours_db : Form
    {
        //string type_acc;
        public NpgsqlConnection con;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        public work_hours_db(NpgsqlConnection con)
        {
            this.con = con;
            //this.type_acc = type_acc;
            InitializeComponent();
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

                whDataGridView.DataSource = null;
                whDataGridView.DataSource = bsEmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при фильтрации: " + ex.Message);
            }
        }



        private void searchButton_Click(object sender, EventArgs e)
        {
            search();
        }

        private void searchTextBox_Enter(object sender, EventArgs e)
        {
            search();
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            search();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void add_or_upd(int id)
        {
            add_up_wh f = new add_up_wh(con, id);
            this.Hide();
            f.ShowDialog();
            this.Show();
            upd();
        }



        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add_or_upd(-1);
        }



        private void upd()
        {

            
            string
                sql = "Select wh.id,wh.day_week,wh.time_start,wh.time_end,emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio from work_days wh, employee emp where  emp.id=wh.id_emp";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();

            da.Fill(ds);
            dt = ds.Tables[0];
            whDataGridView.DataSource = dt;
            whDataGridView.Columns[0].HeaderText = "ID";
            whDataGridView.Columns[1].HeaderText = "День недели";
            whDataGridView.Columns[2].HeaderText = "Время начала";
            whDataGridView.Columns[3].HeaderText = "Время окончания";
            whDataGridView.Columns[4].HeaderText = "ФИО сотрудника";

            whDataGridView.Columns[0].Width = 20;
            this.StartPosition = FormStartPosition.CenterScreen;


        }

        private void work_hours_db_Load(object sender, EventArgs e)
        {
            whDataGridView.ColumnHeadersHeight = 30;
            обновитьToolStripMenuItem.Visible = false;
            upd();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            var result = guna2MessageDialog1.Show("Вы точно хотите удалить данную запись?", "Удаление");
            if (result == DialogResult.Yes)
            {
                string sql = "Delete from work_days where ID=:id";
                int id = (int)whDataGridView.CurrentRow.Cells["ID"].Value;
                NpgsqlCommand com = new NpgsqlCommand(sql, con);
                com.Parameters.AddWithValue("id", id);
                com.ExecuteNonQuery();
            }
            upd();
        }
    }
}
