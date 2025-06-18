using Guna.UI2.WinForms;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;

namespace diploma
{
    public partial class make_app : Form
    {
        public NpgsqlConnection con;
        int id;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        string emp;
        public make_app(NpgsqlConnection con, int id)
        {
            InitializeComponent();
            this.con = con;
            this.id = id;
        }



        private void update_spec()
        {

            string sql = "Select spec.name from Specialisation spec where not(spec.name='Администратор PostgreSQL')";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            specDataGridView.DataSource = dt;
            specDataGridView.Columns[0].HeaderText = "Название специализации";

            this.StartPosition = FormStartPosition.CenterScreen;


        }

        

        private void make_app_Load(object sender, EventArgs e)
        {
            specDataGridView.ColumnHeadersHeight = 30;
            update_spec();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void specButton_Click(object sender, EventArgs e)
        {
            spec_db f = new spec_db(con, "look");
            this.Hide();
            f.ShowDialog();
            this.Show();
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

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            search();
            specDataGridView.Columns[0].HeaderText = "Название специализации";
        }




        private void fill_panel(string spec)
        {
            Console.WriteLine(spec);

            empLayoutPanel.Controls.Clear();

            //string query = "select emp.id,ie.image,emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio,emp.experience,emp.seria from images_emp ie,employee emp,specialisation spec,spec_emp se  where ie.id_emp=emp.id and spec.name=@spec and se.id_emp=emp.id and spec.id=se.id_spec";
            string query = "select emp.id,ie.image,emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio,emp.experience,epc.cost from images_emp ie,employee emp,specialisation spec,spec_emp se,emp_proc_cost epc, med_procedure proc  where ie.id_emp=emp.id and spec.name=@spec and se.id_emp=emp.id and spec.id=se.id_spec and epc.id_emp=emp.id and epc.id_proc=proc.id";
            using (IDbCommand command = con.CreateCommand())
            {
               
                IDbDataParameter parameter = command.CreateParameter();
                parameter.ParameterName = "@spec";
                parameter.Value = spec;
                command.Parameters.Add(parameter);
               
                command.CommandText = query;




                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id_emp = reader.GetInt32(0);
                        string fio = reader.GetString(2);
                        string exp = reader.GetString(3);
                        decimal price = reader.GetDecimal(4);
                        
                        Image photo = null;
                        if (!reader.IsDBNull(1)) 
                        {
                            byte[] imageData = (byte[])reader.GetValue(1);
                            using (MemoryStream ms = new MemoryStream(imageData))
                            {
                                photo = Image.FromStream(ms);
                            }
                        }

                        docUserControl uc = new docUserControl(con,id,id_emp,photo, fio,spec,exp,price);
                        uc.Tag = id_emp;
                        //uc.Size = new Size(empLayoutPanel.Width - 20, 100);
                        empLayoutPanel.Controls.Add(uc);


                    }
                }

            }

          
            empLayoutPanel.AutoScroll = true;




        }
  
        private void specDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (specDataGridView.CurrentRow != null && specDataGridView.CurrentRow.Cells[0] != null &&
    specDataGridView.CurrentRow.Cells[0].Value != DBNull.Value && specDataGridView.CurrentRow.Cells[0].Value != null)
            {
                string spec = (string)specDataGridView.CurrentRow.Cells[0].Value;
                fill_panel(spec);
            }
        }
    }
}
