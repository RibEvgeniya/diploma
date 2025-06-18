using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace diploma
{
    public partial class dis_db : Form
    {
        string type_acc;
        public NpgsqlConnection con;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        string name;
        int id;

        public dis_db(NpgsqlConnection con, string type_acc)
        {
            this.con = con;
            this.type_acc = type_acc;
            InitializeComponent();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void update_dis()
        {

            string sql = "Select dis.id,dis.mkb_code,dis.name,dis.descr, dis.symptoms,dis.treatment from Diseases dis;";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            disDataGridView.DataSource = dt;
            disDataGridView.Columns[0].HeaderText = "ID";
            disDataGridView.Columns[1].HeaderText = "МКБ код";
            disDataGridView.Columns[2].HeaderText = "Название заболевания";
            disDataGridView.Columns[3].HeaderText = "Описание";
            disDataGridView.Columns[4].HeaderText = "Симптомы";
            disDataGridView.Columns[5].HeaderText = "Общее лечение";

            this.StartPosition = FormStartPosition.CenterScreen;
            disDataGridView.Columns[0].Width = 20;

        }
        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add_up_dis f = new add_up_dis(con, -1);
            this.Hide();
            f.ShowDialog();
            update_dis();
            this.Show();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (disDataGridView.CurrentRow.Cells[0].Value != null)    
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
                var result = guna2MessageDialog1.Show("Вы точно хотите удалить данную запись?", "Удаление");
                if (result == DialogResult.Yes)
                {
                    string sql = "Delete from Diseases where ID=:id";
                    int id = (int)disDataGridView.CurrentRow.Cells["ID"].Value;
                    NpgsqlCommand com = new NpgsqlCommand(sql, con);
                    com.Parameters.AddWithValue("id", id);
                    com.ExecuteNonQuery();
                }
                update_dis();
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

                disDataGridView.DataSource = null;
                disDataGridView.DataSource = bsEmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при фильтрации: " + ex.Message);
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

        private void dis_db_Load(object sender, EventArgs e)
        {
            if (type_acc == "look")
                specMenuStrip.Visible = false;

            disDataGridView.ColumnHeadersHeight = 30;
            обновитьToolStripMenuItem.Visible = false;

            update_dis();
        }

        private void searchButton_Click_1(object sender, EventArgs e)
        {
            search();
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            search();
        }

        private void disDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (disDataGridView.CurrentRow.Cells[0].Value != null)
            {
                int id = (int)disDataGridView.CurrentRow.Cells[0].Value;
                string name = (string)disDataGridView.CurrentRow.Cells[1].Value;

                this.name = name;
                this.id = id;
                Close();
            }
        }
    }
}
