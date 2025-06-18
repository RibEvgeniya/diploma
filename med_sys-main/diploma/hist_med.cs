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
    public partial class hist_med : Form
    {
        public NpgsqlConnection con;
        int id_emp;
        int id_pat;
        bool prev_info = false;

        public hist_med(NpgsqlConnection con, int id_emp, int id_pat)
        {
            this.con = con;
            InitializeComponent();
            this.id_emp = id_emp;
            this.id_pat = id_pat;

        }



        public void fill_panel()
        {
            Size s = new Size(Convert.ToInt32(medicLayoutPanel.Width), medicLayoutPanel.Height / 3);


            medicLayoutPanel.Controls.Clear();
            List<int> idDiagList = new List<int>();
            //string query = "select distinct app.id from patient pat, employee emp, appointment app  where pat.first_name ||' '|| pat.last_name ||' '|| pat.patronymic=@fio and app.id_spec=pat.id and app.id_emp=" + this.id+ " and app.date_app<'"+DateTime.Today.Date+"'";
            string query = "select distinct diag.id,diag.id_app,app.id,app.date_app,emp.first_name || ' ' || emp.last_name || ' ' || emp.patronymic as fio from patient pat,employee emp, appointment app, diagnoses diag  where diag.id_app = app.id and app.id_emp = emp.id and app.id_spec ="+this.id_pat+" order by app.date_app DESC; ";
            using (IDbCommand command = con.CreateCommand())
            {
                command.CommandText = query;


                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id_diag = reader.GetInt32(0);
                        idDiagList.Add(reader.GetInt32(0));
                    }
                }
            }


            //string sql = "Select distinct app.id, emp.first_name || ' ' || emp.last_name || ' ' || emp.patronymic as fio, proc.name, emp_proc_cost.cost, app.date_app, emp.id from employee emp, emp_proc_cost, appointment app, med_procedure proc where app.id_emp = emp.id and app.id_proc = proc.id and emp_proc_cost.id_proc = proc.id and emp_proc_cost.id_emp = app.id_emp and app.id=:id_app order by app.date_app";
            DataTable med = new DataTable();
           // med.Columns.Add("Название");
            //med.Columns.Add("Дозирование");
            //med.Columns.Add("Длительность");
            foreach (int id_diag in idDiagList)
            {
                using (IDbCommand secondCommand = con.CreateCommand())
                {

                    List<string> listdis = new List<string>();
                    string thirdQuery = "SELECT dis.name from Diseases dis, diagnoses diag, appointment app, diagnos_dis dd  where diag.id_app=app.id and dd.id_diag=diag.id and dd.id_dis=dis.id and diag.id = :id_diag order by app.date_app";
                    using (IDbCommand thirdCommand = con.CreateCommand())
                    {
                        IDbDataParameter parameter1 = thirdCommand.CreateParameter();
                        parameter1.ParameterName = ":id_diag";
                        parameter1.Value = id_diag;
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

                    
                    thirdQuery = "SELECT med.name, dm.dosage,dm.duration from medication med, diagnoses diag, appointment app, diagnos_med dm  where diag.id_app=app.id and dm.id_diag=diag.id and dm.id_med=med.id and diag.id = :id_diag order by app.date_app";
                    NpgsqlDataAdapter da2 = new NpgsqlDataAdapter(thirdQuery, con);
                    da2.SelectCommand.Parameters.AddWithValue("id_diag", id_diag);
                    da2.Fill(med);
                    med.Columns[0].ColumnName = "Название";
                    med.Columns[1].ColumnName = "Дозирование";
                    med.Columns[2].ColumnName = "Длительность";




                    secondCommand.CommandText = query;
                    using (IDataReader secondReader = secondCommand.ExecuteReader())
                    {
                        while (secondReader.Read())
                        {


                            string fio1 = secondReader.GetString(4);
                            DateTime date = secondReader.GetDateTime(3);
                            List<string> dis = listdis;

                            medUserControl uc = new medUserControl(con, id_diag, id_emp, s, fio1, dis, med, date, id_pat);
                            uc.Tag = id_diag;
                            medicLayoutPanel.Controls.Add(uc);

                        }
                    }




                }


              











            }



            medicLayoutPanel.AutoScroll = true;

        }




        private void hist_med_Load(object sender, EventArgs e)
        {
            fill_panel();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
