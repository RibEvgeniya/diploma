using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace diploma
{
    public partial class proc_db : Form
    {
        string type_acc;
        public NpgsqlConnection con;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        DataTable dt1 = new DataTable();
        DataSet ds1 = new DataSet();
        string name;
        int id;
        public proc_db(NpgsqlConnection con, string type_acc)
        {
            this.con = con;
            this.type_acc = type_acc;
            InitializeComponent();
        }



        private void update_proc()
        {

            string sql = "Select proc.id,proc.name,proc.descr, proc.duration from med_procedure proc";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            procDataGridView.DataSource = dt;
            procDataGridView.Columns[0].HeaderText = "ID";
            procDataGridView.Columns[1].HeaderText = "Название процедуры";
            procDataGridView.Columns[2].HeaderText = "Описание";
            procDataGridView.Columns[3].HeaderText = "Длительность";

            this.StartPosition = FormStartPosition.CenterScreen;
            procDataGridView.Columns[0].Width = 20;


        }


        private void proc_db_Load(object sender, EventArgs e)
        {
            if (type_acc == "look")
                specMenuStrip.Visible = false;

            procDataGridView.ColumnHeadersHeight = 30;
            empDataGridView.ColumnHeadersHeight = 30;
            обновитьToolStripMenuItem.Visible = false;
            update_proc();
        }


        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }




        private void update_emp(int id_proc)
        {

            string sql = "Select distinct emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio,post.name from Employee emp,Specialisation spec,spec_proc,spec_emp,post, med_procedure proc where emp.id=spec_emp.id_emp and spec_emp.id_spec=spec.id and post.id=emp.id_post and proc.id=:id and spec_proc.id_proc=proc.id and spec_proc.id_spec=spec.id;";
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sql, con);
            da1.SelectCommand.Parameters.AddWithValue("id", id_proc);
            ds1.Reset();
            da1.Fill(ds1);
            dt1 = ds1.Tables[0];
            empDataGridView.DataSource = dt1;
            empDataGridView.Columns[0].HeaderText = "ФИО работника";
            empDataGridView.Columns[1].HeaderText = "Должность";
        }

        private void procDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            int id_proc;
            try
            {
                if (procDataGridView.CurrentRow != null)
                    id_proc = (int)procDataGridView.CurrentRow.Cells["ID"].Value;
                else id_proc = procDataGridView.RowCount;
                update_emp(id_proc);
            }
            catch
            {
            }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            var result = guna2MessageDialog1.Show("Вы точно хотите удалить данную запись?", "Удаление");
            if (result == DialogResult.Yes)
            {
                string sql = "Delete from med_procedure where ID=:id";
                int id = (int)procDataGridView.CurrentRow.Cells["ID"].Value;
                NpgsqlCommand com = new NpgsqlCommand(sql, con);
                com.Parameters.AddWithValue("id", id);
                com.ExecuteNonQuery();
            }
            update_proc();
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add_up_proc f = new add_up_proc(con, -1);
            this.Hide();
            f.ShowDialog();
            update_proc();
            this.Show();
        }


        public int get_id()
        {
            return this.id;
        }
        public string get_name()
        {
            return this.name;
        }

        private void procDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (procDataGridView.CurrentRow.Cells[0].Value != null)
            {
                int id = (int)procDataGridView.CurrentRow.Cells[0].Value;
                string name = (string)procDataGridView.CurrentRow.Cells[1].Value;

                this.name = name;
                this.id = id;
                Close();
            }
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
                procDataGridView.DataSource = null;
                procDataGridView.DataSource = bsEmp;
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
        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            search();
        }

        private void назначитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add_up_assign_proc f = new add_up_assign_proc(con, -1);
            this.Hide();
            f.ShowDialog();
            update_proc();
            this.Show();
        }
    }
}
