using Guna.UI2.WinForms;
using Npgsql;
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

namespace diploma
{
    public partial class emp_menu : Form
    {
        public NpgsqlConnection con;
        int id;
        string type_acc;
        DataTable dt1 = new DataTable();
        DataSet ds1 = new DataSet();
        DataTable dt2 = new DataTable();
        DataSet ds2 = new DataSet();
        public emp_menu(NpgsqlConnection con, int id, string type_acc)
        {
            InitializeComponent();
            this.con = con;
            this.id = id;
            this.type_acc = type_acc;
        }

        private void upd_menu() 
        {
            try
            {
                string sql = "Select emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio,emp.login,emp.email,emp.phone,emp.education, emp.birthdate,emp.gender,post.name, emp.seria,emp.numb, emp.passport_issued,emp.passport_issued_in,emp.experience,emp.date_of_emp,emp.id from Employee emp,Post post where emp.id_post=post.id and emp.id=:id";

                NpgsqlDataAdapter cmd = new NpgsqlDataAdapter(sql, con);
                cmd.SelectCommand.Parameters.AddWithValue("id", this.id);
                DataTable dt1 = new DataTable();
                cmd.Fill(dt1);
                DataRow dr = dt1.Select()[0];
                //String name = dr.Field<string>(1) + " " + dr.Field<string>(0) + " " + dr.Field<string>(2);
                actualFioLabel.Text = dr.Field<string>(0);
                actualLoginLabel.Text = dr.Field<string>(1);
                actualEmailLabel.Text = dr.Field<string>(2);
                actualPhoneLabel.Text = dr.Field<string>(3);
                actualEduLabel.Text = dr.Field<string>(4);
                actualBirthLabel.Text = dr.Field<DateTime>(5).ToLongDateString();
                actualGenderLabel.Text = dr.Field<string>(6);
                actualPostLabel.Text = dr.Field<string>(7);
                actualIssuedLabel.Text = dr.Field<DateTime>(11).ToLongDateString();
                actualIssuedByLabel.Text = dr.Field<string>(10);
                actualSeriaLabel.Text = dr.Field<string>(8);
                actualNumLabel.Text = dr.Field<string>(9);
                actualExpLabel.Text = dr.Field<string>(12);
                if (!dr.IsNull(13)) 
                {
                    actualDateEmpLabel.Text = dr.Field<DateTime>(13).ToLongDateString();
                }
                else
                {
                    actualDateEmpLabel.Text = "нет данных";
                }
                //actualDateEmpLabel.Text = dr.Field<DateTime>(13).ToLongDateString();



                string sql1 = "Select spec.name from Employee emp,Specialisation spec,spec_emp where emp.id=:id and emp.id=spec_emp.id_emp and spec_emp.id_spec=spec.id";
                NpgsqlDataAdapter cmd4 = new NpgsqlDataAdapter(sql1, con);
                cmd4.SelectCommand.Parameters.AddWithValue("id", this.id);
                DataTable dt3 = new DataTable();
                cmd4.Fill(dt3);

                if (dt3.Rows.Count > 0)
                {
                    List<string> spec = new List<string>();

                    foreach (DataRow dr3 in dt3.Rows)
                    {
                        if (!dr3.IsNull(0))
                        {
                            spec.Add(dr3[0].ToString());
                        }
                    }

                    actualSpecLabel.Text = string.Join(", ", spec);
                }
                else
                {
                    actualSpecLabel.Text = "нет данных";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }



        private void up_app() 
        {

            string
                    sql = "Select sh.date_app,sh.time_app, pat.first_name ||' '|| pat.last_name ||' '|| pat.patronymic as fio2,proc.name, sh.status,emp_proc_cost.cost, pat.id,sh.id from emp_proc_cost,Appointment sh, patient pat, employee emp,med_procedure proc where pat.id=sh.id_spec and emp.id=sh.id_emp and sh.id_proc=proc.id and emp.id=:id and sh.date_app >= :fut_date and emp_proc_cost.id_emp=emp.id and emp_proc_cost.id_proc=proc.id;";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            da.SelectCommand.Parameters.AddWithValue("id", id);
            da.SelectCommand.Parameters.AddWithValue("fut_date", DateTime.Today);
            ds1.Reset();

            da.Fill(ds1);
            dt1 = ds1.Tables[0];
            futAppDataGridView.DataSource = dt1;
            futAppDataGridView.Columns[0].HeaderText = "Дата";
            futAppDataGridView.Columns[1].HeaderText = "Время начала";
            futAppDataGridView.Columns[2].HeaderText = "ФИО пациента";
            futAppDataGridView.Columns[3].HeaderText = "Процедура";
            futAppDataGridView.Columns[4].HeaderText = "Статус";
            futAppDataGridView.Columns[5].HeaderText = "Стоимость";
            futAppDataGridView.Columns[6].HeaderText = "ID";

            this.StartPosition = FormStartPosition.CenterScreen;


            sql = "Select sh.date_app,sh.time_app, pat.first_name ||' '|| pat.last_name ||' '|| pat.patronymic as fio2,proc.name, sh.status,emp_proc_cost.cost, pat.id,sh.id from emp_proc_cost, Appointment sh, patient pat, employee emp,med_procedure proc where pat.id=sh.id_spec and emp.id=sh.id_emp and sh.id_proc=proc.id and emp.id=:id and sh.date_app < :past_date and emp_proc_cost.id_emp=emp.id and emp_proc_cost.id_proc=proc.id;";
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sql, con);
            da1.SelectCommand.Parameters.AddWithValue("id", id);
            da1.SelectCommand.Parameters.AddWithValue("past_date", DateTime.Today);
            ds2.Reset();

            da1.Fill(ds2);
            dt2 = ds2.Tables[0];
            pastAppDataGridView.DataSource = dt2;
            pastAppDataGridView.Columns[0].HeaderText = "Дата";
            pastAppDataGridView.Columns[1].HeaderText = "Время начала";
            pastAppDataGridView.Columns[2].HeaderText = "ФИО пациента";
            pastAppDataGridView.Columns[3].HeaderText = "Процедура";
            pastAppDataGridView.Columns[4].HeaderText = "Статус";
            pastAppDataGridView.Columns[5].HeaderText = "Стоимость";
            pastAppDataGridView.Columns[6].HeaderText = "ID пат";
            pastAppDataGridView.Columns[7].HeaderText = "ID прием";
            pastAppDataGridView.Columns[7].Width = 0;
            pastAppDataGridView.Columns[6].Width = 0;



        }




        private void emp_menu_Load(object sender, EventArgs e)
        {
            date.Text = DateTime.Today.ToShortDateString();

       

            futAppDataGridView.ColumnHeadersHeight = 30;
            pastAppDataGridView.ColumnHeadersHeight = 30;
            if (type_acc == "look") 
            {
                possabGroupBox.Visible = false;
                actInfoGroupBox.Location = new Point(actInfoGroupBox.Location.X, infoGroupBox.Location.Y);
                photoButton.Visible = false;
                notif.Visible = false;
                
            }




            string sql = "Select image from images_emp where id_emp=:id;";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            da.SelectCommand.Parameters.AddWithValue("id", id);
            DataSet ds = new DataSet();
            da.Fill(ds);
            byte[] aray;
            if (ds.Tables[0].Rows.Count != 0)
            {
                aray = (byte[])ds.Tables[0].Rows[0][0];
                MemoryStream ms = new MemoryStream(aray);
                guna2PictureBox1.Image = Image.FromStream(ms);
            }
            upd_menu();
            up_app();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void calendarButton_Click(object sender, EventArgs e)
        {
            sheduler f = new sheduler(con, this.id);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void photoButton_Click(object sender, EventArgs e)
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
                                string sql = "select *from images_emp where id_emp=:id;";
                                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
                                da.SelectCommand.Parameters.AddWithValue("id", this.id);
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    guna2PictureBox1.Load(filename);
                                    sql = "update images_emp set image=:image where id_emp=:id;";
                                    NpgsqlCommand com = new NpgsqlCommand(sql, con);
                                    com.Parameters.AddWithValue("id", this.id);
                                    Image img = guna2PictureBox1.Image;
                                    ImageConverter converter = new ImageConverter();
                                    byte[] arr = (byte[])converter.ConvertTo(img, typeof(byte[]));
                                    com.Parameters.AddWithValue("image", arr);
                                    com.ExecuteNonQuery();
                                }
                                else
                                {
                                    guna2PictureBox1.Load(filename);
                                    sql = "Insert into images_emp(name,id_emp, image) values ('Фотография',:id,:image);";
                                    NpgsqlCommand com = new NpgsqlCommand(sql, con);
                                    com.Parameters.AddWithValue("id", this.id);
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

        private void changePassButton_Click(object sender, EventArgs e)
        {
            change_pass f = new change_pass(con, actualEmailLabel.Text);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }



        private void futAppDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //
        }

        private void futAppDataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = (int)futAppDataGridView.CurrentRow.Cells["ID"].Value;
            int id_ap = (int)futAppDataGridView.CurrentRow.Cells[7].Value;
            pat_menu f = new pat_menu(con, id, "look",this.id,id_ap);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void pastAppDataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
        
            int id_ap = (int)pastAppDataGridView.CurrentRow.Cells[7].Value;
            int id_pat = (int)pastAppDataGridView.CurrentRow.Cells[6].Value;
            make_diag f = new make_diag(con, id_pat, id, id_ap, "look");
            this.Hide();
            f.ShowDialog();
            this.Show();
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
            sql = "Select count(*) from appointment a where a.date_app=:start and a.id_emp=:id and a.time_app>:time ";
            //sql = "Select id from receptions";
            cmd = new NpgsqlDataAdapter(sql, con);
            cmd.SelectCommand.Parameters.AddWithValue("start", DateTime.Now.Date);
            cmd.SelectCommand.Parameters.AddWithValue("id",id);
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

        private void notifRichTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            sheduler f = new sheduler(con, this.id);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            add_up_emp f = new add_up_emp(con, id);
            this.Hide();
            f.ShowDialog();
            upd_menu();
            
            this.Show();
        }

        private void appButton_Click(object sender, EventArgs e)
        {
            emp_app f = new emp_app(con, id);
            this.Hide();
            f.ShowDialog();
            upd_menu();

            this.Show();
        }

        private void pastAppDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void futAppDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
