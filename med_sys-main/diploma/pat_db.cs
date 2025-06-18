using Guna.UI2.WinForms;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace diploma
{
    public partial class pat_db : Form
    {
        public NpgsqlConnection con;
        public string type_acc;
        int id;
        DataTable dt1 = new DataTable();
        DataSet ds1 = new DataSet();
        DataTable dt2 = new DataTable();
        DataSet ds2 = new DataSet();
        DataTable dt3 = new DataTable();
        DataSet ds3 = new DataSet();
        public pat_db(NpgsqlConnection con, string type_user)
        {
            InitializeComponent();
            this.con = con;
            this.type_acc = type_user; 
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }



 





        public void update_pat()
        {
            string sql = "Select emp.id,emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio,emp.login,emp.h_password,emp.email,emp.phone,emp.snils, emp.birthdate,emp.gender, emp.polis,emp.register_date from Patient emp";
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sql, con);
            ds1.Reset();
            da1.Fill(ds1);
            dt1 = ds1.Tables[0];
            patDataGridView.DataSource = dt1;
            patDataGridView.Columns[0].HeaderText = "ID";
            patDataGridView.Columns[1].HeaderText = "ФИО";
            patDataGridView.Columns[2].HeaderText = "Логин";
            patDataGridView.Columns[3].HeaderText = "Хэшированный пароль";
            patDataGridView.Columns[4].HeaderText = "Почта";
            patDataGridView.Columns[5].HeaderText = "Телефон";
            patDataGridView.Columns[6].HeaderText = "Снилс";
            patDataGridView.Columns[7].HeaderText = "Дата рождения";
            patDataGridView.Columns[8].HeaderText = "Пол";
            patDataGridView.Columns[9].HeaderText = "Полис";
            patDataGridView.Columns[10].HeaderText = "Дата регистрации";

            this.StartPosition = FormStartPosition.CenterScreen;
            patDataGridView.Columns[0].Width = 20;
            patDataGridView.Columns[3].Width = 0;
            patDataGridView.Columns[2].Width = 0;
        }
        public void update_emp_pas(int id)
        {
            string
            sql = "Select emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio,emp.seria ,emp.numb,emp.passport_issued ,emp.passport_issued_in from Patient emp where emp.id=:id";
            NpgsqlDataAdapter da2 = new NpgsqlDataAdapter(sql, con);
            da2.SelectCommand.Parameters.AddWithValue("id", id);
            ds2.Reset();
            da2.Fill(ds2);
            dt2 = ds2.Tables[0];
            passpDataGridView.DataSource = dt2;
            passpDataGridView.Columns[0].HeaderText = "ФИО";
            passpDataGridView.Columns[1].HeaderText = "Серия";
            passpDataGridView.Columns[2].HeaderText = "Номер";
            passpDataGridView.Columns[3].HeaderText = "Выдан";
            passpDataGridView.Columns[4].HeaderText = "Дата выдачи";

        }



        private void pat_db_Load(object sender, EventArgs e)
        {
            if (type_acc == "look" || type_acc == "choose")
                empMenuStrip.Visible = false;
            patDataGridView.ColumnHeadersHeight = 30;
            passpDataGridView.ColumnHeadersHeight = 30;
            update_pat();
        }

        private void empDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            int id_pat;
            try
            {
                if (patDataGridView.CurrentRow != null)
                    id_pat = (int)patDataGridView.CurrentRow.Cells["ID"].Value;
                else id_pat = patDataGridView.RowCount;
                update_emp_pas(id_pat);
            }
            catch
            {
            }
        }








        private void search()
        {
            string searchTerm = searchTextBox.Text.ToLower();
            bsEmp.DataSource = dt1;

            if (string.IsNullOrEmpty(searchTerm))
            {
                bsEmp.Filter = null;
                return;
            }
            try
            {
                DataTable filteredTable = dt1.Clone();

                foreach (DataRow row in dt1.Rows)
                {
                    bool rowMatches = false;
                    foreach (DataColumn column in dt1.Columns)
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
                patDataGridView.DataSource = null;
               patDataGridView.DataSource = bsEmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при фильтрации: " + ex.Message);
            }
        }


        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            search();
        }

        private void searchButton_Click(object sender, EventArgs e)
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
                string sql = "Delete from Patient where ID=:id";
                int id = (int)patDataGridView.CurrentRow.Cells["ID"].Value;
                NpgsqlCommand com = new NpgsqlCommand(sql, con);
                com.Parameters.AddWithValue("id", id);
                com.ExecuteNonQuery();
            }
            update_pat();
        }



        private void add_or_upd(int id)
        {
            add_up_pat f = new add_up_pat(con, -1);
            this.Hide();
            f.ShowDialog();
            update_pat();
            this.Show();

        }
        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add_or_upd(-1);
        }



        public int get_id()
        {
            return this.id;
        }

        private void patDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (type_acc == "choose")
            {
                if (patDataGridView.CurrentRow.Cells[0].Value != null)
                {
                    int id = (int)patDataGridView.CurrentRow.Cells[0].Value;
                    string name = (string)patDataGridView.CurrentRow.Cells[1].Value;
                    this.id = id;
                    Close();
                }


            }
            else
            {
                int id = (int)patDataGridView.CurrentRow.Cells["ID"].Value;
                pat_menu f = new pat_menu(con, id, "look",-1,-1);
                this.Hide();
                f.ShowDialog();
                this.Show();


            }
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = (int)patDataGridView.CurrentRow.Cells["ID"].Value;
            add_up_pat f = new add_up_pat(con, id);
            this.Hide();
            f.ShowDialog();
            update_pat();
            this.Show();
        }
    }
}
