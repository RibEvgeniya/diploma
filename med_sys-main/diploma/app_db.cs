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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace diploma
{
    public partial class app_db : Form
    {
        public NpgsqlConnection con;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        public app_db(NpgsqlConnection con)
        {
            this.con = con;
            InitializeComponent();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }








        private void update_app()
        {

            string
                sql = "Select sh.id,sh.date_app,sh.time_app, pat.first_name ||' '|| pat.last_name ||' '|| pat.patronymic as fio1,emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio2,proc.name, sh.status from Appointment sh, patient pat, employee emp,med_procedure proc where pat.id=sh.id_spec and emp.id=sh.id_emp and sh.id_proc=proc.id";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();

            da.Fill(ds);
            dt = ds.Tables[0];
            appDataGridView.DataSource = dt;
            appDataGridView.Columns[0].HeaderText = "ID";
            appDataGridView.Columns[1].HeaderText = "Дата";
            appDataGridView.Columns[2].HeaderText = "Время начала";
            appDataGridView.Columns[3].HeaderText = "ФИО пациента";
            appDataGridView.Columns[4].HeaderText = "ФИО сотрудника";
            appDataGridView.Columns[5].HeaderText = "Процедура";
            appDataGridView.Columns[6].HeaderText = "Статус";

            this.StartPosition = FormStartPosition.CenterScreen;

            appDataGridView.Columns[0].Width = 20;
        }




        private void app_db_Load(object sender, EventArgs e)
        {
            appDataGridView.ColumnHeadersHeight = 50;
            обновитьToolStripMenuItem.Visible = false;
            update_app();
        }


        private void add_or_upd(int id)
        {
            add_up_app f = new add_up_app(con, id);
            this.Hide();
            f.ShowDialog();
            this.Show();
            update_app();
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add_or_upd(-1);
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            var result = guna2MessageDialog1.Show("Вы точно хотите удалить данную запись?", "Удаление");
            if (result == DialogResult.Yes)
            {
                string sql = "Delete from appointment where ID=:id";
                int id = (int)appDataGridView.CurrentRow.Cells["ID"].Value;
                NpgsqlCommand com = new NpgsqlCommand(sql, con);
                com.Parameters.AddWithValue("id", id);
                com.ExecuteNonQuery();
            }
            update_app();
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

                appDataGridView.DataSource = null;
                appDataGridView.DataSource = bsEmp;
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
    }
}
