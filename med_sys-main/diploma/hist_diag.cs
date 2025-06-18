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
    public partial class hist_diag : Form
    {
        public NpgsqlConnection con;
        int id_pat;
        bool prev_info = false;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        public hist_diag(NpgsqlConnection con, int id_pat)
        {
            this.con = con;
            InitializeComponent();
            this.id_pat = id_pat;

        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }



        private void update_emp()
        {

            string sql = "Select DISTINCT emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio from Patient pat, employee emp, appointment app,diagnoses diag where diag.id_app=app.id and app.id_emp=emp.id and app.id_spec=:id";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            da.SelectCommand.Parameters.AddWithValue("id", id_pat);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            empDataGridView.DataSource = dt;
            empDataGridView.Columns[0].HeaderText = "ФИО врача";

            this.StartPosition = FormStartPosition.CenterScreen;


        }


        public void fill_panel(string fio)
        {
            Size s = new Size(Convert.ToInt32(diagLayoutPanel.Width), diagLayoutPanel.Height / 3);

            Console.WriteLine(fio);

            diagLayoutPanel.Controls.Clear();
            List<int> idAppList = new List<int>();
            //string query = "select distinct app.id from patient pat, employee emp, appointment app  where pat.first_name ||' '|| pat.last_name ||' '|| pat.patronymic=@fio and app.id_spec=pat.id and app.id_emp=" + this.id+ " and app.date_app<'"+DateTime.Today.Date+"'";
            string query = "select distinct app.id,app.date_app from patient pat, employee emp, appointment app  where emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic=@fio and app.id_emp=emp.id and app.id_spec=" + this.id_pat + " order by app.date_app DESC;";
            using (IDbCommand command = con.CreateCommand())
            {
                
                IDbDataParameter parameter = command.CreateParameter();
                parameter.ParameterName = "@fio";
                parameter.Value = fio;
                command.Parameters.Add(parameter);
                command.CommandText = query;




                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id_app = reader.GetInt32(0);
                        Console.WriteLine(id_app);
                        idAppList.Add(reader.GetInt32(0));


                    }
                }

            }


            string sql = "Select distinct app.id, emp.first_name || ' ' || emp.last_name || ' ' || emp.patronymic as fio, proc.name, emp_proc_cost.cost, app.date_app, emp.id from employee emp, emp_proc_cost, appointment app, diagnoses diag, med_procedure proc where diag.id_app=app.id and app.id_emp = emp.id and app.id_proc = proc.id and emp_proc_cost.id_proc = proc.id and emp_proc_cost.id_emp = app.id_emp and app.id=:id_app order by app.date_app";

            foreach (int id_app in idAppList)
            {
                using (IDbCommand secondCommand = con.CreateCommand())
                {



                    List<List<string>> listlist = new List<List<string>>(); 
                    List<string> listdis = new List<string>();
                    string thirdQuery = "SELECT dis.name from Diseases dis, diagnoses diag, appointment app, diagnos_dis dd  where diag.id_app=app.id and dd.id_diag=diag.id and dd.id_dis=dis.id and app.id = :id_app order by app.date_app";
                    using (IDbCommand thirdCommand = con.CreateCommand())
                    {
                        IDbDataParameter parameter1 = thirdCommand.CreateParameter();
                        parameter1.ParameterName = ":id_app";
                        parameter1.Value = id_app;
                        thirdCommand.Parameters.Add(parameter1);
                        thirdCommand.CommandText = thirdQuery;

                        using (IDataReader thirdReader = thirdCommand.ExecuteReader())
                        {
                            while (thirdReader.Read())
                            {
                                listdis.Add(thirdReader.GetString(0));
                            }
                        }
                    }
                  
                    foreach (string illName in listdis)
                    {
                        listlist.Add(listdis);
                        Console.WriteLine(illName);
                    }
                    Console.WriteLine();







                    //----------------------------------





                    IDbDataParameter parameter = secondCommand.CreateParameter();
                    parameter.ParameterName = ":id_app";
                    parameter.Value = id_app;
                    secondCommand.Parameters.Add(parameter);
                    secondCommand.CommandText = sql;

                    using (IDataReader secondReader = secondCommand.ExecuteReader())
                    {
                        while (secondReader.Read())
                        {


                            string fio1 = secondReader.GetString(1);
                            string proc_name = secondReader.GetString(2);
                            Decimal cost = secondReader.GetDecimal(3);
                            DateTime date = secondReader.GetDateTime(4);
                            int id_emp = secondReader.GetInt16(5);
                            List<string> spec = listdis;
                            diagUserControl uc = new diagUserControl(con, id_app, id_emp, s, fio1, proc_name, cost, spec, date, id_pat);
                            uc.Tag = id_app;
                            diagLayoutPanel.Controls.Add(uc);

                        }
                    }
                }














            }



            diagLayoutPanel.AutoScroll = true;

        }
        private void hist_diag_Load(object sender, EventArgs e)
        {
            empDataGridView.ColumnHeadersHeight = 30;
            update_emp();
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

                empDataGridView.DataSource = null;

                empDataGridView.DataSource = bsEmp;
                empDataGridView.Columns[0].HeaderText = "ФИО врача";
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

        private void empDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            string fio = (string)empDataGridView.CurrentRow.Cells[0].Value;
            fill_panel(fio);
        }
    }
}
