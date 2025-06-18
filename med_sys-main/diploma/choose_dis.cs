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
    public partial class choose_dis : Form
    {
        public NpgsqlConnection con;
        int id_pat;
        int id_emp;
        DataTable dt;
        DataTable dt1 = new DataTable();
        DataSet ds1 = new DataSet();
        DataTable dis = new DataTable();
        bool full;
        public choose_dis(NpgsqlConnection con, int id_pat, int id_emp)
        {
            InitializeComponent();
            this.con = con;
            this.id_pat = id_pat;
            this.id_emp = id_emp;
            full = false;

        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            full = true;
            Close();
        }

        private void medButton_Click(object sender, EventArgs e)
        {
            dis_db f = new dis_db(con, "look");
            this.Hide();
            f.ShowDialog();
            this.Show();
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
                allDisDataGridView.DataSource = null;
                allDisDataGridView.DataSource = bsEmp;
                allDisDataGridView.Columns[0].Width = 0;
                allDisDataGridView.Columns[0].HeaderText = "ID";
                allDisDataGridView.Columns[1].HeaderText = "Название";

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




        private void upd()
        {
            string sql = "Select med.id,med.name from Diseases med;";
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sql, con);
            ds1.Reset();
            da1.Fill(ds1);
            dt1 = ds1.Tables[0];

            allDisDataGridView.DataSource = dt1;
            allDisDataGridView.Columns[0].Width = 0;
            allDisDataGridView.Columns[0].HeaderText = "ID";
            allDisDataGridView.Columns[1].HeaderText = "Название";



         
            dis.Columns.Add("Название");
            dis.Columns.Add("Дополнительно");

            selectDisDataGridView.DataSource = dis;

        }
        private void choose_dis_Load(object sender, EventArgs e)
        {
            allDisDataGridView.ColumnHeadersHeight = 30;
            selectDisDataGridView.ColumnHeadersHeight = 30;

            upd();
            //selectMedDataGridView.Columns[0].HeaderText = "ID";
            selectDisDataGridView.Columns[0].HeaderText = "Название";
            selectDisDataGridView.Columns[1].HeaderText = "Дополнительно";
        }

        private void leftButton_Click(object sender, EventArgs e)
        {
            if (selectDisDataGridView.SelectedRows.Count > 0)
            {
             
                foreach (DataGridViewRow row in selectDisDataGridView.SelectedRows)
                {
                    dis.Rows[row.Index].Delete();
                }
            }
        }
        public bool get_full()
        {
            return full;
        }
        public DataTable get_dis()
        {
            return dis;
        }
        private void rightButton_Click(object sender, EventArgs e)
        {
            int id_dis = (int)allDisDataGridView.CurrentRow.Cells[0].Value;
            string name_dis = (string)allDisDataGridView.CurrentRow.Cells[1].Value;



            DataRow newRow = dis.NewRow();
         
            newRow[0] = name_dis;
            newRow[1] = notesRichTextBox.Text;
            dis.Rows.Add(newRow);

            notesRichTextBox.Clear();
            
        }
    }
}
