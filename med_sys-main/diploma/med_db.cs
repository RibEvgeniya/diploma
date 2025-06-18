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
    public partial class med_db : Form
    {
        public NpgsqlConnection con;
        public string type_acc;
        int id;
        string name;
        DataTable dt1 = new DataTable();
        DataSet ds1 = new DataSet();
        DataTable dt2 = new DataTable();
        DataSet ds2 = new DataSet();
        DataTable dt3 = new DataTable();
        DataSet ds3 = new DataSet();
        public med_db(NpgsqlConnection con, string type_user)
        {
            InitializeComponent();
            this.con = con;
            this.type_acc = type_user;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }



        public void update_med()
        {
            string sql = "Select med.id,med.name,med.producer,med.release_form,med.descr from Medication med;";
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sql, con);
            ds1.Reset();
            da1.Fill(ds1);
            dt1 = ds1.Tables[0];
            medDataGridView.DataSource = dt1;
            medDataGridView.Columns[0].HeaderText = "ID";
            medDataGridView.Columns[1].HeaderText = "Название";
            medDataGridView.Columns[3].HeaderText = "Форма выпуска";
            medDataGridView.Columns[2].HeaderText = "Производитель";
            medDataGridView.Columns[4].HeaderText = "Описание";
            medDataGridView.Columns[0].Width = 20;

            this.StartPosition = FormStartPosition.CenterScreen;

        }


        private void med_db_Load(object sender, EventArgs e)
        {
            if (type_acc == "look" || type_acc == "choose")
                empMenuStrip.Visible = false;
            medDataGridView.ColumnHeadersHeight = 30;
            обновитьToolStripMenuItem.Visible = false;
            update_med();
        }


        public void update_med_descr(int id)
        {

                
                string sql = "Select med.descr,med.id from Medication med where med.id=:id;";

                NpgsqlDataAdapter cmd2 = new NpgsqlDataAdapter(sql, con);
                cmd2.SelectCommand.Parameters.AddWithValue("id", id);
                DataTable dt2 = new DataTable();
                cmd2.Fill(dt2);
           
                DataRow dr = dt2.Select()[0];
                descrRichTextBox.Text = dr.Field<string>(0);
             
            
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
                medDataGridView.DataSource = null; 
                medDataGridView.DataSource = bsEmp;
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

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            var result = guna2MessageDialog1.Show("Вы точно хотите удалить данную запись?", "Удаление");
            if (result == DialogResult.Yes)
            {
                string sql = "Delete from Medication where ID=:id";
                int id = (int)medDataGridView.CurrentRow.Cells["ID"].Value;
                NpgsqlCommand com = new NpgsqlCommand(sql, con);
                com.Parameters.AddWithValue("id", id);
                com.ExecuteNonQuery();
            }
            update_med();
        }


        private void add_or_upd(int id, string name, string producer, string release_form, string descr)
        {
            add_up_med f = new add_up_med(con, -1, name, producer, release_form, descr);
            this.Hide();
            f.ShowDialog();
            update_med();
            this.Show();

        }


        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add_or_upd(-1, "", "", "", "");
        }

        private void medDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void medDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            int id_m;
            try
            {
                if (medDataGridView.CurrentRow != null)
                    id_m = (int)medDataGridView.CurrentRow.Cells["ID"].Value;
                else id_m = medDataGridView.RowCount;
                Console.WriteLine(id_m);
                update_med_descr(id_m);
            }
            catch
            {
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
        private void medDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (medDataGridView.CurrentRow.Cells[0].Value != null)
            {
                int id = (int)medDataGridView.CurrentRow.Cells[0].Value;
                string name = (string)medDataGridView.CurrentRow.Cells[1].Value;

                this.id = id;
                this.name = name;
                Close();
            }
        }
    }
}
