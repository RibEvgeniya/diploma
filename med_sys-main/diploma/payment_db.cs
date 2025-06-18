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

namespace diploma
{
    public partial class payment_db : Form
    {
        string type_acc;
        public NpgsqlConnection con;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        DataTable dt1 = new DataTable();
        DataSet ds1 = new DataSet();
        string name;
        int id;
        public payment_db(NpgsqlConnection con, string type_acc)
        {
            this.con = con;
            this.type_acc = type_acc;
            InitializeComponent();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
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
                paymentDataGridView.DataSource = null;
                paymentDataGridView.DataSource = bsEmp;
                paymentDataGridView.Columns[0].HeaderText = "ID";
                paymentDataGridView.Columns[1].HeaderText = "ФИО сотрудника";
                paymentDataGridView.Columns[2].HeaderText = "Должность";
                paymentDataGridView.Columns[3].HeaderText = "Процедура";
                paymentDataGridView.Columns[4].HeaderText = "Стоимость";
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


        public void update_payment()
        {
            string sql = "Select emp_proc_cost.id,emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio, post.name,proc.name, emp_proc_cost.cost from Employee emp,emp_proc_cost, Post,med_procedure proc where emp_proc_cost.id_emp=emp.id and emp_proc_cost.id_proc=proc.id and emp.id_post=post.id";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            paymentDataGridView.DataSource = dt;
            paymentDataGridView.Columns[0].HeaderText = "ID";
            paymentDataGridView.Columns[1].HeaderText = "ФИО сотрудника";
            paymentDataGridView.Columns[2].HeaderText = "Должность";
            paymentDataGridView.Columns[3].HeaderText = "Процедура";
            paymentDataGridView.Columns[4].HeaderText = "Стоимость";

            paymentDataGridView.Columns[0].Width = 20;
            this.StartPosition = FormStartPosition.CenterScreen;

        }
        public void update_payment_proc(string name)
        {
            string sql = "Select proc.name,proc.descr, proc.duration from med_procedure proc where proc.name=:name";
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sql, con);
            da1.SelectCommand.Parameters.AddWithValue("name", name);
            ds1.Reset();
            da1.Fill(ds1);
            dt1 = ds1.Tables[0];
            procDataGridView.DataSource = dt1;
            procDataGridView.Columns[0].HeaderText = "Название процедуры";
            procDataGridView.Columns[1].HeaderText = "Описание";
            procDataGridView.Columns[2].HeaderText = "Длительность(минуты)";

            this.StartPosition = FormStartPosition.CenterScreen;

        }
        private void payment_db_Load(object sender, EventArgs e)
        {
            update_payment();
            paymentDataGridView.ColumnHeadersHeight = 30;
            procDataGridView.ColumnHeadersHeight = 30;
            обновитьToolStripMenuItem.Visible = false;
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            search();
        }




        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            var result = guna2MessageDialog1.Show("Вы точно хотите удалить данную запись?", "Удаление");
            if (result == DialogResult.Yes)
            {
                string sql = "Delete from emp_proc_cost where ID=:id";
                int id = (int)paymentDataGridView.CurrentRow.Cells["ID"].Value;
                NpgsqlCommand com = new NpgsqlCommand(sql, con);
                com.Parameters.AddWithValue("id", id);
                com.ExecuteNonQuery();
            }
            update_payment();
        }

        private void paymentDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            string name_proc;
            try
            {
                if (paymentDataGridView.CurrentRow != null)
                    name_proc = (string)paymentDataGridView.CurrentRow.Cells[3].Value;
                else name_proc = "";
                update_payment_proc(name_proc);
            }
            catch
            {
            }
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add_up_payment f = new add_up_payment(con, -1);
            this.Hide();
            f.ShowDialog();
            update_payment();
            this.Show();
        }
    }
}
