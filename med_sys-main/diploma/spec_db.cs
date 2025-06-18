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
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace diploma
{
    public partial class spec_db : Form
    {
        string type_acc;
        public NpgsqlConnection con;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        DataTable dt1 = new DataTable();
        DataSet ds1 = new DataSet();
        string name;
        int id;
        public spec_db(NpgsqlConnection con, string type_acc)
        {
            this.con = con;
            this.type_acc = type_acc;
            InitializeComponent();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void update_spec()
        {

            string sql = "Select spec.id,spec.name,spec.descr from Specialisation spec";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            specDataGridView.DataSource = dt;
            specDataGridView.Columns[0].HeaderText = "ID";
            specDataGridView.Columns[1].HeaderText = "Название специализации";
            specDataGridView.Columns[2].HeaderText = "Описание";

            this.StartPosition = FormStartPosition.CenterScreen;
            specDataGridView.Columns[0].Width = 20;

        }

        private void update_emp(int id_spec)
        {

            string sql = "Select emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio,post.name from Employee emp,Specialisation spec,spec_emp,post where emp.id=spec_emp.id_emp and spec_emp.id_spec=spec.id and spec.id=:id and post.id=emp.id_post;";
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sql, con);
            da1.SelectCommand.Parameters.AddWithValue("id", id_spec);
            ds1.Reset();
            da1.Fill(ds1);
            dt1 = ds1.Tables[0];
            empDataGridView.DataSource = dt1;
            empDataGridView.Columns[0].HeaderText = "ФИО работника";
            empDataGridView.Columns[1].HeaderText = "Должность";
        }


        private void spec_db_Load(object sender, EventArgs e)
        {
            if(type_acc=="look")
                specMenuStrip.Visible = false;


            specDataGridView.ColumnHeadersHeight = 30;
            empDataGridView.ColumnHeadersHeight = 30;
            обновитьToolStripMenuItem.Visible = false;
            update_spec();
            
        }

        private void specDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            


        }

        private void specDataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (specDataGridView.CurrentRow.Cells[0].Value != null)
            {
                int id = (int)specDataGridView.CurrentRow.Cells[0].Value;
                string name = (string)specDataGridView.CurrentRow.Cells[1].Value;

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

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
                var result = guna2MessageDialog1.Show("Вы точно хотите удалить данную запись?", "Удаление");
                if (result == DialogResult.Yes)
                {
                    string sql = "Delete from Specialisation where ID=:id";
                    int id = (int)specDataGridView.CurrentRow.Cells["ID"].Value;
                    NpgsqlCommand com = new NpgsqlCommand(sql, con);
                    com.Parameters.AddWithValue("id", id);
                    com.ExecuteNonQuery();
                }
                update_spec();
            
        }


        private void search() {
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

                specDataGridView.DataSource = null; 
                specDataGridView.DataSource = bsEmp;
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

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void specDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add_up_spec f = new add_up_spec(con, -1);
            this.Hide();
            f.ShowDialog();
            update_spec();
            this.Show();
        }

        private void назначитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add_up_assign_spec f = new add_up_assign_spec(con, -1);
            this.Hide();
            f.ShowDialog();
            update_spec();
            this.Show();
        }

        private void specDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            int id_spec;
            try
            {
                if (specDataGridView.CurrentRow != null)
                    id_spec = (int)specDataGridView.CurrentRow.Cells["ID"].Value;
                else id_spec = specDataGridView.RowCount;
                update_emp(id_spec);
            }
            catch
            {
            }
        }

        private void backPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
