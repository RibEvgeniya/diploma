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
using Guna.UI2.WinForms;
using Npgsql;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;


namespace diploma
{
    public partial class emp_db : Form
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
        public emp_db(NpgsqlConnection con, string type_user)
        {
            InitializeComponent();
            this.con = con; 
            this.type_acc = type_user;
        }

        public void update_emp()
        {
            string sql = "Select emp.id,emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio,emp.login,emp.h_password,emp.email,emp.phone,emp.education, emp.birthdate,emp.gender, post.name,emp.date_of_emp from Employee emp,Post post where emp.id_post=post.id";
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sql, con);
            ds1.Reset();
            da1.Fill(ds1);
            dt1 = ds1.Tables[0];
            empDataGridView.DataSource = dt1;
            empDataGridView.Columns[0].HeaderText = "ID";
            empDataGridView.Columns[1].HeaderText = "ФИО";
            empDataGridView.Columns[2].HeaderText = "Логин";
            empDataGridView.Columns[3].HeaderText = "Хэшированный пароль";
            empDataGridView.Columns[4].HeaderText = "Почта";
            empDataGridView.Columns[5].HeaderText = "Телефон";
            empDataGridView.Columns[6].HeaderText = "Образование";
            empDataGridView.Columns[7].HeaderText = "Дата рождения";
            empDataGridView.Columns[8].HeaderText = "Пол";
            empDataGridView.Columns[9].HeaderText = "Должность";
            empDataGridView.Columns[10].HeaderText = "Дата принятия на работу";
            
            this.StartPosition = FormStartPosition.CenterScreen;
            empDataGridView.Columns[0].Width = 20;
            empDataGridView.Columns[3].Width = 0;
            empDataGridView.Columns[2].Width = 0;
        }
        public void update_emp_pass(int id)
        {
            string
            sql = "Select emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio,emp.seria ,emp.numb,emp.passport_issued ,emp.passport_issued_in from Employee emp where emp.id=:id";
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

        public void update_emp_spec(int id)
        {
            string
            sql = "Select emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio,spec.name,spec.descr from Employee emp,Specialisation spec,spec_emp where emp.id=:id and emp.id=spec_emp.id_emp and spec_emp.id_spec=spec.id";
            NpgsqlDataAdapter da3 = new NpgsqlDataAdapter(sql, con);
            da3.SelectCommand.Parameters.AddWithValue("id", id);
            ds3.Reset();
            da3.Fill(ds3);
            dt3 = ds3.Tables[0];
            specDataGridView.DataSource = dt3;
            specDataGridView.Columns[0].HeaderText = "ФИО";
            specDataGridView.Columns[1].HeaderText = "Название";
            specDataGridView.Columns[2].HeaderText = "Описание";

        }

        private void emp_db_Load(object sender, EventArgs e)
        {
            if (type_acc == "look" || type_acc == "choose")
                empMenuStrip.Visible = false;
            empDataGridView.ColumnHeadersHeight = 30;
            passpDataGridView.ColumnHeadersHeight = 30;
            specDataGridView.ColumnHeadersHeight = 30;
            update_emp();
            //bsEmp.DataSource = dt1;
            update_emp();
            
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void add_or_upd(int id)
        {
            add_up_emp f = new add_up_emp(con, id);
            this.Hide();
            f.ShowDialog();
            update_emp();
            this.Show();

        }
        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            add_or_upd(-1);
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = (int)empDataGridView.CurrentRow.Cells["ID"].Value;
            add_or_upd(id);
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            var result = guna2MessageDialog1.Show("Вы точно хотите удалить данную запись?", "Удаление");
            if (result == DialogResult.Yes)
            {
                string sql = "Delete from Employee where ID=:id";
                int id = (int)empDataGridView.CurrentRow.Cells["ID"].Value;
                NpgsqlCommand com = new NpgsqlCommand(sql, con);
                com.Parameters.AddWithValue("id", id);
                com.ExecuteNonQuery();
            }
            update_emp();
        }

        private void backPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void empDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            int id;
            try
            {
                if (empDataGridView.CurrentRow != null)
                    id = (int)empDataGridView.CurrentRow.Cells["ID"].Value;
                else id = empDataGridView.RowCount;
                update_emp_pass(id);
                update_emp_spec(id);
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

             
                empDataGridView.DataSource = null; 
                empDataGridView.DataSource = bsEmp;
                empDataGridView.Columns[0].HeaderText = "ID";
                empDataGridView.Columns[1].HeaderText = "ФИО";
                empDataGridView.Columns[2].HeaderText = "Логин";
                empDataGridView.Columns[3].HeaderText = "Хэшированный пароль";
                empDataGridView.Columns[4].HeaderText = "Почта";
                empDataGridView.Columns[5].HeaderText = "Телефон";
                empDataGridView.Columns[6].HeaderText = "Образование";
                empDataGridView.Columns[7].HeaderText = "Дата рождения";
                empDataGridView.Columns[8].HeaderText = "Пол";
                empDataGridView.Columns[9].HeaderText = "Должность";
                empDataGridView.Columns[10].HeaderText = "Дата принятия на работу";

                this.StartPosition = FormStartPosition.CenterScreen;
                empDataGridView.Columns[0].Width = 20;
                empDataGridView.Columns[3].Width = 0;
                empDataGridView.Columns[2].Width = 0;
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







        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            search();
        }

        private void empDataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {


            if (type_acc == "choose")
            {
                if (empDataGridView.CurrentRow.Cells[0].Value != null)
                {
                    int id = (int)empDataGridView.CurrentRow.Cells[0].Value;
                    string name = (string)empDataGridView.CurrentRow.Cells[1].Value;
                    this.id = id;
                    Close();
                }


            }
            else 
            {
                int id = (int)empDataGridView.CurrentRow.Cells["ID"].Value;
                emp_menu f = new emp_menu(con, id, "look");
                this.Hide();
                f.ShowDialog();
                this.Show();


            }



        }


        public int get_id()
        {
            return this.id;
        }

        private void назначитьЗарплатуToolStripMenuItem_Click(object sender, EventArgs e)
        {

           


        }
    }
}
