using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Npgsql;
using System.Configuration;

namespace diploma
{
    public partial class start : Form
    {
        public NpgsqlConnection con;
        public start()
        {
            InitializeComponent();
        }
        public void InitializeDatabaseConnection()
        {

            string initialConnectionString = ConfigurationManager.ConnectionStrings["InitialPostgresConnection"]?.ConnectionString;

         
            string dbCreateFileName = ConfigurationManager.AppSettings["DbCreateSqlFileName"];
            string tablesCreateFileName = ConfigurationManager.AppSettings["TablesCreateSqlFileName"];
            string newDatabaseName = ConfigurationManager.AppSettings["NewDatabaseName"];

          
            if (string.IsNullOrEmpty(initialConnectionString) ||
                string.IsNullOrEmpty(dbCreateFileName) ||
                string.IsNullOrEmpty(tablesCreateFileName) ||
                string.IsNullOrEmpty(newDatabaseName))
            {
                MessageBox.Show("Ошибка: Не все параметры конфигурации для базы данных найдены в App.config. Проверьте секции connectionStrings и appSettings.", "Ошибка конфигурации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string dbCreateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbCreateFileName);
            string tablesCreateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, tablesCreateFileName);

        
            string mainAppConnectionString;

            try
            {
              
                if (!CheckDatabaseExists(initialConnectionString, newDatabaseName))
                {
                 
                    CreateDatabase(initialConnectionString, dbCreateFilePath);
                }

             
                NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder(initialConnectionString);
                builder.Database = newDatabaseName;
                mainAppConnectionString = builder.ToString();

         
                using (NpgsqlConnection newConn = new NpgsqlConnection(mainAppConnectionString))
                {
                    newConn.Open();

                    string tableCreateScript = File.ReadAllText(tablesCreateFilePath).Trim();

                    if (string.IsNullOrEmpty(tableCreateScript))
                    {
                        throw new Exception($"Файл с запросом для создания таблиц '{tablesCreateFileName}' пустой или не найден.");
                    }


                    using (NpgsqlTransaction transaction = newConn.BeginTransaction())
                    {
                        try
                        {
                            using (NpgsqlCommand cmd = new NpgsqlCommand(tableCreateScript, newConn, transaction))
                            {
                                cmd.ExecuteNonQuery();
                            }
                            transaction.Commit();
                          
                        }
                        catch (NpgsqlException pgEx)
                        {
                           
                                MessageBox.Show($"Ошибка при создании таблиц: {pgEx.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Непредвиденная ошибка при создании таблиц: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                con = new NpgsqlConnection(mainAppConnectionString);
                con.Open();
         

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критическая ошибка при инициализации базы данных: {ex.Message}\nПроверьте настройки в App.config и доступность сервера PostgreSQL.", "Ошибка инициализации БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //con = new NpgsqlConnection("Server=localhost;Port=5432;UserID=postgres;Password=postpass; Database=med_system_db");

            InitializeDatabaseConnection();


            NpgsqlDataAdapter cmd;
            string sql = "Select count(*) from employee ";
            //sql = "Select id from receptions";
            cmd = new NpgsqlDataAdapter(sql, con);
            DataTable dt3 = new DataTable();
            cmd.Fill(dt3);
            DataRow dr3 = dt3.Select()[0];
            string count = dr3[0].ToString();
            if (count == "0")
            {
                sql = "Select count(*) from post ";
                //sql = "Select id from receptions";
                cmd = new NpgsqlDataAdapter(sql, con);
                DataTable dt4 = new DataTable();
                cmd.Fill(dt4);
                DataRow dr4 = dt4.Select()[0];
                string count1 = dr4[0].ToString();
                if (count1 == "0")
                {
                    sql = "insert into post(name,descr) values (:name,:descr);";
                    NpgsqlCommand com = new NpgsqlCommand(sql, con);
                    com = new NpgsqlCommand(sql, con);
                    com.Parameters.AddWithValue("name", "Администратор баз данных");
                    com.Parameters.AddWithValue("descr", "Имеет доступ ко всем данным");
                    com.ExecuteNonQuery();

                




                }
                NpgsqlCommand com1;
                sql = "insert into Employee(id_post,login,h_password,first_name,last_name,patronymic,email,phone,birthdate,passport_issued_in) values (:id_post,:login,:pass,:first_name,:last_name,:patronymic,:email,:phone,:bd,:passport);";
                com1 = new NpgsqlCommand(sql, con);
                com1.Parameters.AddWithValue("login", "Admin");
                com1.Parameters.AddWithValue("pass", "+merkYT41XTO982OCy8aeA==");
                com1.Parameters.AddWithValue("id_post", 1);
                com1.Parameters.AddWithValue("first_name", " ");
                com1.Parameters.AddWithValue("last_name", " ");
                com1.Parameters.AddWithValue("patronymic", " ");
                com1.Parameters.AddWithValue("email", " ");
                com1.Parameters.AddWithValue("phone", " ");
                com1.Parameters.AddWithValue("bd", DateTime.Today.Date);
                com1.Parameters.AddWithValue("passport", DateTime.Today.Date);
                com1.ExecuteNonQuery();

            }



        }

        private bool CheckDatabaseExists(string connectionString, string databaseName)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand($"SELECT 1 FROM pg_database WHERE datname = '{databaseName}'", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
        }

        private void CreateDatabase(string connectionString, string dbCreateFilePath)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                string[] dbCreateCommands = File.ReadAllLines(dbCreateFilePath);
                foreach (string command in dbCreateCommands)
                {
                    using (var cmd = new NpgsqlCommand(command, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

       

        private void empButton_Click(object sender, EventArgs e)
        {

            sign_in f = new sign_in(con,"emp");
            this.Hide();
            f.ShowDialog();
            this.Show();

        }

        private void patButton_Click(object sender, EventArgs e)
        {
            sign_in f = new sign_in(con,"pat");
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void regButton_Click(object sender, EventArgs e)
        {
            add_up_pat f = new add_up_pat(con, -1);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }
    }
}
