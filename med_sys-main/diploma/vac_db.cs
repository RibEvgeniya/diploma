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
using System.Xml.Linq;

namespace diploma
{
    public partial class vac_db : Form
    {
        string type_acc;
        public NpgsqlConnection con;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        DataTable dt1 = new DataTable();
        DataSet ds1 = new DataSet();
        string name;
        int id;

        public vac_db(NpgsqlConnection con, string type_acc)
        {
            this.con = con;
            this.type_acc = type_acc;
            InitializeComponent();
        }


    
        private void update_vac()
        {

            string sql = "Select v.id,v.name,v.descr, v.term,d.name,v.manufacturer,v.cost from vaccine v, diseases d where v.id_dis=d.id";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            vacDataGridView.DataSource = dt;
            vacDataGridView.Columns[0].HeaderText = "ID";
            vacDataGridView.Columns[1].HeaderText = "Название";
            vacDataGridView.Columns[2].HeaderText = "Описание";
            vacDataGridView.Columns[3].HeaderText = "Количество дней";
            vacDataGridView.Columns[4].HeaderText = "Заболевание";
            vacDataGridView.Columns[5].HeaderText = "Изготовитель";
            vacDataGridView.Columns[6].HeaderText = "Цена проставления";

            this.StartPosition = FormStartPosition.CenterScreen;
            vacDataGridView.Columns[0].Width = 20;

        }



        private void vac_db_Load(object sender, EventArgs e)
        {
            if (type_acc == "look")
                specMenuStrip.Visible = false;
            обновитьToolStripMenuItem.Visible = false;  
            vacDataGridView.ColumnHeadersHeight = 30;
            update_vac();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            var result = guna2MessageDialog1.Show("Вы точно хотите удалить данную запись?", "Удаление");
            if (result == DialogResult.Yes)
            {
                string sql = "Delete from vaccine where ID=:id";
                int id = (int)vacDataGridView.CurrentRow.Cells["ID"].Value;
                NpgsqlCommand com = new NpgsqlCommand(sql, con);
                com.Parameters.AddWithValue("id", id);
                com.ExecuteNonQuery();
            }
            update_vac();
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add_up_vac f = new add_up_vac(con, -1);
            this.Hide();
            f.ShowDialog();
            update_vac();
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

                vacDataGridView.DataSource = null;
                vacDataGridView.DataSource = bsEmp;
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

        private void searchButton_TextChanged(object sender, EventArgs e)
        {
            search();
        }
      
        private void vacDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (vacDataGridView.CurrentRow.Cells[0].Value != null)
            {
                int id = (int)vacDataGridView.CurrentRow.Cells[0].Value;
                string name = (string)vacDataGridView.CurrentRow.Cells[1].Value;

                this.name = name;
                this.id = id;
                Close();
            }
        }

        private void specMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
