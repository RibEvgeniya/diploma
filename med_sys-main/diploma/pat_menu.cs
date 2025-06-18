using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Npgsql;

namespace diploma
{
    public partial class pat_menu : Form
    {
        public NpgsqlConnection con;
        int id_pat;
        int id_emp;
        int id_ap;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        DataTable dt1 = new DataTable();
        DataSet ds1 = new DataSet();
        string type_acc;
        public pat_menu(NpgsqlConnection con, int id_pat, string type_acc, int id_emp, int id_ap)
        {
            InitializeComponent();
            this.con = con;
            this.id_pat = id_pat;
            this.type_acc = type_acc;
            this.id_emp = id_emp;
            this.id_ap = id_ap;
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void photoButton_Click(object sender, EventArgs e)
        {

        }



        private void upd() 
        {
            if (id_emp == -1)
            {
                string sql;
                if (futRadioButton.Checked)
                    sql = "Select sh.date_app,sh.time_app, emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio2,proc.name,emp_proc_cost.cost, sh.status from Appointment sh, patient pat, employee emp,med_procedure proc,emp_proc_cost where pat.id=sh.id_spec and emp_proc_cost.id_emp=emp.id and emp.id=sh.id_emp and sh.id_proc=proc.id and pat.id=:id and sh.date_app>=:cur_date;";
                else
                    sql = "Select sh.date_app,sh.time_app, emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio2,proc.name,emp_proc_cost.cost, sh.status from Appointment sh, patient pat, employee emp,med_procedure proc,emp_proc_cost where pat.id=sh.id_spec and emp_proc_cost.id_emp=emp.id and emp.id=sh.id_emp and sh.id_proc=proc.id and pat.id=:id and  sh.date_app<:cur_date;";
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
                da.SelectCommand.Parameters.AddWithValue("id", id_pat);
                da.SelectCommand.Parameters.AddWithValue("cur_date", DateTime.Today);
                ds.Reset();

                da.Fill(ds);
                dt = ds.Tables[0];
                appDataGridView.DataSource = dt;
                appDataGridView.Columns[0].HeaderText = "Дата";
                appDataGridView.Columns[1].HeaderText = "Время начала";
                appDataGridView.Columns[2].HeaderText = "ФИО сотрудника";
                appDataGridView.Columns[3].HeaderText = "Процедура";
                appDataGridView.Columns[4].HeaderText = "Стоимость ";
                appDataGridView.Columns[5].HeaderText = "Статус";

                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else 
            {
                string sql;
                if (futRadioButton.Checked)
                    sql = "Select sh.date_app,sh.time_app, emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio2,proc.name,emp_proc_cost.cost, sh.status from Appointment sh, patient pat, employee emp,med_procedure proc,emp_proc_cost where pat.id=sh.id_spec and emp_proc_cost.id_emp=emp.id and emp.id=sh.id_emp and sh.id_proc=proc.id and pat.id=:id and sh.date_app>=:cur_date and emp.id=:id_emp;";
                else
                    sql = "Select sh.date_app,sh.time_app, emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio2,proc.name,emp_proc_cost.cost, sh.status from Appointment sh, patient pat, employee emp,med_procedure proc,emp_proc_cost where pat.id=sh.id_spec and emp_proc_cost.id_emp=emp.id and emp.id=sh.id_emp and sh.id_proc=proc.id and pat.id=:id and  sh.date_app<:cur_date and emp.id=:id_emp;";
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
                da.SelectCommand.Parameters.AddWithValue("id", id_pat);
                da.SelectCommand.Parameters.AddWithValue("id_emp", id_emp);
                da.SelectCommand.Parameters.AddWithValue("cur_date", DateTime.Today);
                ds.Reset();

                da.Fill(ds);
                dt = ds.Tables[0];
                appDataGridView.DataSource = dt;
                appDataGridView.Columns[0].HeaderText = "Дата";
                appDataGridView.Columns[1].HeaderText = "Время начала";
                appDataGridView.Columns[2].HeaderText = "ФИО сотрудника";
                appDataGridView.Columns[3].HeaderText = "Процедура";
                appDataGridView.Columns[4].HeaderText = "Стоимость ";
                appDataGridView.Columns[5].HeaderText = "Статус";

                this.StartPosition = FormStartPosition.CenterScreen;


            }
            upd_vac();

        }



        private void upd_vac()
        {
            string sql;
            
            sql = "Select distinct vac.name, v.date_get, v.date_exp,dis.name,vac.cost,emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio2, v.status_vac from vaccination v,vaccine vac, diseases dis, Employee emp, Patient pat where vac.id_dis=dis.id and v.id_vac=vac.id and v.id_pat=:id and v.id_emp=emp.id;";

            Console.WriteLine("fsddsdsdsd" + id_pat.ToString());    
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sql, con);
            da1.SelectCommand.Parameters.AddWithValue("id", id_pat);
            da1.SelectCommand.Parameters.AddWithValue("cur_date", DateTime.Today);
            ds1.Reset();

            da1.Fill(ds1);
            dt1 = ds1.Tables[0];
            vacDataGridView.DataSource = dt1;
            vacDataGridView.Columns[0].HeaderText = "Название";
            vacDataGridView.Columns[1].HeaderText = "Дата получения";
            vacDataGridView.Columns[2].HeaderText = "Дата истечения срока";
            vacDataGridView.Columns[3].HeaderText = "Болезнь";
            vacDataGridView.Columns[4].HeaderText = "Стоимость";
            vacDataGridView.Columns[5].HeaderText = "ФИО сотрудника";
            vacDataGridView.Columns[6].HeaderText = "Статус";
            //vacDataGridView.Columns[6].HeaderText = "ID";

            this.StartPosition = FormStartPosition.CenterScreen;



        }




        private void upd_menu()
        {
            try
            {
                string sql = "Select emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio,emp.login,emp.email,emp.phone,emp.snils, emp.birthdate,emp.gender,emp.polis, emp.seria,emp.numb, emp.passport_issued,emp.passport_issued_in,emp.id,emp.register_date from Patient emp where emp.id=:id";

                NpgsqlDataAdapter cmd = new NpgsqlDataAdapter(sql, con);
                cmd.SelectCommand.Parameters.AddWithValue("id", this.id_pat);
                DataTable dt1 = new DataTable();
                cmd.Fill(dt1);
                DataRow dr = dt1.Select()[0];
                //String name = dr.Field<string>(1) + " " + dr.Field<string>(0) + " " + dr.Field<string>(2);
                actualFioLabel.Text = dr.Field<string>(0);
                actualLoginLabel.Text = dr.Field<string>(1);
                actualEmailLabel.Text = dr.Field<string>(2);
                actualPhoneLabel.Text = dr.Field<string>(3);
                actualSnilsLabel.Text = dr.Field<string>(4);
                actualBirthLabel.Text = dr.Field<DateTime>(5).ToLongDateString();
                actualGenderLabel.Text = dr.Field<string>(6);
                actualPolisLabel.Text = dr.Field<string>(7);
                actualIssuedLabel.Text = dr.Field<DateTime>(11).ToLongDateString();
                actualIssuedByLabel.Text = dr.Field<string>(10);
                actualSeriaLabel.Text = dr.Field<string>(8);
                actualNumLabel.Text = dr.Field<string>(9);
                if (!dr.IsNull(13)) 
                {
                    actualDateRegLabel.Text = dr.Field<DateTime>(13).ToLongDateString();
                }
                else
                {
                    actualDateRegLabel.Text = "нет данных";
                }
                //actualDateRegLabel.Text = dr.Field<string>(13);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }












        private void pat_menu_Load(object sender, EventArgs e)
        {
            
            date.Text = DateTime.Today.ToShortDateString();



            anaButton.Visible = false;


            appDataGridView.ColumnHeadersHeight = 30;
            vacDataGridView.ColumnHeadersHeight = 30;


            if (type_acc == "look" || type_acc == "choose")
            {
                possabGroupBox.Visible = false;
                actInfoGroupBox.Location = new Point(actInfoGroupBox.Location.X, infoGroupBox.Location.Y);
                photoButton.Visible = false;

                diagButton.Location = new Point(actInfoGroupBox.Location.X, diagButton.Location.Y);
                addAnaInfoButton.Location= new Point(actInfoGroupBox.Location.X, addAnaInfoButton.Location.Y);
                vac2Button.Width= addAnaInfoButton.Size.Width;
                vac2Button.Location = new Point(addAnaInfoButton.Location.X + addAnaInfoButton.Size.Width + 2, vac2Button.Location.Y);
                medic2Button.Location=new Point(addAnaInfoButton.Location.X +addAnaInfoButton.Size.Width + 2, medic2Button.Location.Y);

                passportGroupBox.Visible = false;
                vacInfo2Button.Size= new Size(vac2Button.Width, vac2Button.Height);
                vacInfo2Button.Location=new Point(vacInfo2Button.Location.X, vacInfo2Button.Location.Y-vacInfo2Button.Size.Height-2);
                diagHisButton.Visible = false;

            }
            else {
                diagButton.Visible = false; 
                addAnaInfoButton.Visible = false;
                med2Button.Visible = false;
                medic2Button.Visible=false;
                vac2Button.Visible= false;
                vacInfo2Button.Location = new Point(vacInfo2Button.Location.X + addAnaInfoButton.Size.Width+2, vacInfo2Button.Location.Y);
                //vacInfo2Button.Visible= false;
            }

            string sql = "Select image from images_pat where id_pat=:id;";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            da.SelectCommand.Parameters.AddWithValue("id", id_pat);
            DataSet ds = new DataSet();
            da.Fill(ds);
            byte[] aray;
            if (ds.Tables[0].Rows.Count != 0)
            {
                aray = (byte[])ds.Tables[0].Rows[0][0];
                MemoryStream ms = new MemoryStream(aray);
                guna2PictureBox1.Image = Image.FromStream(ms);
            }

            upd();
            upd_vac();
            upd_menu();

            if(id_emp!=-1)
                notif.Visible = false;



        }

        private void changePassButton_Click(object sender, EventArgs e)
        {
            change_pass f = new change_pass(con, actualEmailLabel.Text);
            this.Hide();
            f.ShowDialog();
            this.Show();
            upd();
        }

        private void appButton_Click(object sender, EventArgs e)
        {
            make_app f = new make_app(con,id_pat);
            this.Hide();
            f.ShowDialog();
            this.Show();
            upd();
        }

        private void futRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            upd();
        }

        private void pastRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            upd();
        }

        private void vacDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
        }

        private void diagButton_Click(object sender, EventArgs e)
        {
            hist_diag f = new hist_diag(con, id_pat);
            this.Hide();
            f.ShowDialog();
            this.Show();
            upd();
        }

        private void diagButton_Click_1(object sender, EventArgs e)
        {
            make_diag f = new make_diag(con, id_pat,id_emp,id_ap,"add");
            this.Hide();
            f.ShowDialog();
            this.Show();
            upd();
        }

        private void natifControlBox_Click(object sender, EventArgs e)
        {
            notif.Visible = false;



        }

        private void notif_Paint(object sender, PaintEventArgs e)
        {
            notifRichTextBox.SelectionAlignment = HorizontalAlignment.Center;
            //notifRichTextBox.Enabled = false;




            String sql;
            NpgsqlDataAdapter cmd;
            sql = "Select count(*) from appointment a where a.date_app=:start and a.id_spec=:id and a.time_app>:time ";
            //sql = "Select id from receptions";
            cmd = new NpgsqlDataAdapter(sql, con);
            cmd.SelectCommand.Parameters.AddWithValue("start", DateTime.Now.Date);
            cmd.SelectCommand.Parameters.AddWithValue("id", id_pat);
            cmd.SelectCommand.Parameters.AddWithValue("time", DateTime.Today.TimeOfDay);
            DataTable dt3 = new DataTable();
            cmd.Fill(dt3);
            DataRow dr3 = dt3.Select()[0];
            string count = dr3[0].ToString();
            if (count != "0")
            {
                notifRichTextBox.Text = "\n\nСегодня планируется еще " + count + " прием(ов/а)!";
            }
            else 
            {
                notifRichTextBox.Text = "\n\nСегодня планируется еще " + count + " прием(ов/а)!";
                notif.Visible = false;

            }
            notifRichTextBox.ReadOnly = true;

        }

        private void notifControlBox_Click(object sender, EventArgs e)
        {
            notif.Visible=false;    
        }

        private void medDataButton_Click(object sender, EventArgs e)
        {
            add_up_more_info f = new add_up_more_info(con, id_pat, id_emp,"look");
            this.Hide();
            f.ShowDialog();
            this.Show();
            upd();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            add_up_pat f = new add_up_pat(con, id_pat);
            this.Hide();
            f.ShowDialog();
            this.Show();
            upd_menu();
        }

        private void photoButton_Click_1(object sender, EventArgs e)
        {
            try
            {
                Stream myStream = null;
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "jpeg|*.jpg|bmp|*.bmp|all files|*.*";
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        if ((myStream = ofd.OpenFile()) != null)
                        {
                            string filename = ofd.FileName;
                            if (myStream.Length > 512000)
                            {
                                MessageBox.Show("Слишком большой файл");
                            }
                            else
                            {
                                string sql = "select *from images_pat where id_pat=:id;";
                                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
                                da.SelectCommand.Parameters.AddWithValue("id", this.id_pat);
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    guna2PictureBox1.Load(filename);
                                    sql = "update images_pat set image=:image where id_pat=:id;";
                                    NpgsqlCommand com = new NpgsqlCommand(sql, con);
                                    com.Parameters.AddWithValue("id", this.id_pat);
                                    Image img = guna2PictureBox1.Image;
                                    ImageConverter converter = new ImageConverter();
                                    byte[] arr = (byte[])converter.ConvertTo(img, typeof(byte[]));
                                    com.Parameters.AddWithValue("image", arr);
                                    com.ExecuteNonQuery();
                                }
                                else
                                {
                                    guna2PictureBox1.Load(filename);
                                    sql = "Insert into images_pat(name,id_pat, image) values ('Фотография',:id,:image);";
                                    NpgsqlCommand com = new NpgsqlCommand(sql, con);
                                    com.Parameters.AddWithValue("id", this.id_pat);
                                    Image img = guna2PictureBox1.Image;
                                    ImageConverter converter = new ImageConverter();
                                    byte[] arr = (byte[])converter.ConvertTo(img, typeof(byte[]));
                                    com.Parameters.AddWithValue("image", arr);
                                    com.ExecuteNonQuery();


                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addAnaInfoButton_Click(object sender, EventArgs e)
        {
            
            make_ana f = new make_ana(con, id_emp, id_pat);
            this.Hide();
            f.ShowDialog();
            this.Show();
            upd();
        }

        private void med2Button_Click(object sender, EventArgs e)
        {
            add_up_more_info f = new add_up_more_info(con, id_pat, id_emp, "look");
            this.Hide();
            f.ShowDialog();
            this.Show();
            upd();
        }

        private void medic2Button_Click(object sender, EventArgs e)
        {
            
            hist_med f = new hist_med(con, id_emp, id_pat);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void medButton_Click(object sender, EventArgs e)
        {
            hist_med f = new hist_med(con, -1, id_pat);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void vac2Button_Click(object sender, EventArgs e)
        {
        }

        private void vac2Button_Click_1(object sender, EventArgs e)
        {
            
            make_vac f = new make_vac(con, id_emp, id_pat);
            this.Hide();
            f.ShowDialog();
            this.Show();
            upd();

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
                vacDataGridView.DataSource = null;
                vacDataGridView.DataSource = bsEmp;

                vacDataGridView.Columns[0].HeaderText = "Название";
                vacDataGridView.Columns[1].HeaderText = "Дата получения";
                vacDataGridView.Columns[2].HeaderText = "Дата истечения срока";
                vacDataGridView.Columns[3].HeaderText = "Болезнь";
                vacDataGridView.Columns[4].HeaderText = "Стоимость";
                vacDataGridView.Columns[5].HeaderText = "ФИО сотрудника";
                vacDataGridView.Columns[6].HeaderText = "Статус";
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

        private void vacInfo2Button_Click(object sender, EventArgs e)
        {
            
            hist_vac f = new hist_vac(con, id_emp,id_pat);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void infAnaButton_Click(object sender, EventArgs e)
        {
            hist_an f = new hist_an(con, id_emp, id_pat);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }
    }
}
