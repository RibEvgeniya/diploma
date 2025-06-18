using Microsoft.Vbe.Interop;
using Npgsql;
using Spire.Doc.Documents;
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
    public partial class vac_pat_db : Form
    {
        string type_acc;
        public NpgsqlConnection con;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        DataTable dt1 = new DataTable();
        DataSet ds1 = new DataSet();
        string name;
        int id;

        public vac_pat_db(NpgsqlConnection con, string type_acc)
        {
            this.con = con;
            this.type_acc = type_acc;
            InitializeComponent();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }




        private void update_vac_pat()
        {

            string sql = "Select v.id,pat.first_name ||' '|| pat.last_name ||' '|| pat.patronymic as fio1,vac.name, v.date_get, v.date_exp,emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio2, v.status_vac from vaccination v,vaccine vac, Employee emp, Patient pat where v.id_vac=vac.id and v.id_pat=pat.id and v.id_emp=emp.id;";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            vacPatDataGridView.DataSource = dt;
            vacPatDataGridView.Columns[0].HeaderText = "ID";
            vacPatDataGridView.Columns[1].HeaderText = "ФИО пациента";
            vacPatDataGridView.Columns[2].HeaderText = "Название вакцины";
            vacPatDataGridView.Columns[3].HeaderText = "Дата получения";
            vacPatDataGridView.Columns[4].HeaderText = "Дата истечения";
            vacPatDataGridView.Columns[5].HeaderText = "ФИО сотрудника";
            vacPatDataGridView.Columns[6].HeaderText = "Статус вакцинации";


            this.StartPosition = FormStartPosition.CenterScreen;

        }


        private void vac_pat_db_Load(object sender, EventArgs e)
        {
            if (type_acc == "look")
                specMenuStrip.Visible = false;

            vacDataGridView.ColumnHeadersHeight = 30;
            vacPatDataGridView.ColumnHeadersHeight = 30;
            update_vac_pat();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            var result = guna2MessageDialog1.Show("Вы точно хотите удалить данную запись?", "Удаление");
            if (result == DialogResult.Yes)
            {
                string sql = "Delete from vaccination where ID=:id";
                int id = (int)vacPatDataGridView.CurrentRow.Cells["ID"].Value;
                NpgsqlCommand com = new NpgsqlCommand(sql, con);
                com.Parameters.AddWithValue("id", id);
                com.ExecuteNonQuery();
            }
            update_vac_pat();
        }



        private void update_vac(int id_vac)
        {

            string sql = "Select v.id,v.name,v.descr, v.term,d.name,v.manufacturer from vaccine v,vaccination vac, diseases d where v.id_dis=d.id and vac.id=:id_vac and vac.id_vac=v.id;";
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sql, con);
            da1.SelectCommand.Parameters.AddWithValue("id_vac", id_vac);
            ds1.Reset();
            da1.Fill(ds1);
            dt1 = ds1.Tables[0];
            vacDataGridView.DataSource = dt1;
            vacDataGridView.Columns[0].HeaderText = "ID";
            vacDataGridView.Columns[1].HeaderText = "Название";
            vacDataGridView.Columns[2].HeaderText = "Описание";
            vacDataGridView.Columns[3].HeaderText = "Количество дней";
            vacDataGridView.Columns[4].HeaderText = "Заболевание";
            vacDataGridView.Columns[5].HeaderText = "Изготовитель";

        }

        private void vacPatDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            int id_vac;
            try
            {
                if (vacPatDataGridView.CurrentRow != null)
                    id_vac = (int)vacPatDataGridView.CurrentRow.Cells["ID"].Value;
                else id_vac = vacPatDataGridView.RowCount;
                update_vac(id_vac);
            }
            catch
            {
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

                vacPatDataGridView.DataSource = null;
                vacPatDataGridView.DataSource = bsEmp;
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

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add_up_vac_pat f = new add_up_vac_pat(con, -1,-1,-1);
            this.Hide();
            f.ShowDialog();
            update_vac_pat();
            this.Show();
        }

        private void updButton_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in vacPatDataGridView.Rows)
                {
                    if (row.IsNewRow) continue;


                    DateTime date_get, date_exp;

                    if (!DateTime.TryParse(row.Cells[3].Value?.ToString(), out date_get) ||
                        !DateTime.TryParse(row.Cells[4].Value?.ToString(), out date_exp))
                    {
                        
                        continue; 
                    }

                    if (date_exp>=DateTime.Today)
                    {
                        string sql = "UPDATE vaccination SET status_vac =:status_vac WHERE id=:id;";
                        NpgsqlCommand com = new NpgsqlCommand(sql, con);
                        com = new NpgsqlCommand(sql, con);
                        com.Parameters.AddWithValue("id", row.Cells[0].Value);
                        com.Parameters.AddWithValue("status_vac", " Недействительна");
                        com.ExecuteNonQuery();
                    }

                }
            }


            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
