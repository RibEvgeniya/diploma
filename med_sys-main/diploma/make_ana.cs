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
    public partial class make_ana : Form
    {
        public NpgsqlConnection con;
        int id_emp;
        int id_pat;
        bool prev_info = false;

        public make_ana(NpgsqlConnection con, int id_emp, int id_pat)
        {
            this.con = con;
            InitializeComponent();
            this.id_emp = id_emp;
            this.id_pat = id_pat;

        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }



        private void for_upd2() 
        {
            string sql = "Select v.id,v.type_analysis,v.date_get, v.status_an,v.results,v.mes_units,v.norm,v.deviation,v.descr from analysis v where v.id_pat=@id_pat and v.id_emp=@id_emp and v.date_get=@date ";

            NpgsqlDataAdapter cmd4 = new NpgsqlDataAdapter(sql, con);
            cmd4.SelectCommand.Parameters.AddWithValue("@id_pat", id_pat);
            cmd4.SelectCommand.Parameters.AddWithValue("@id_emp", id_emp);
            cmd4.SelectCommand.Parameters.AddWithValue("@date", dateDateTimePicker.Value.Date);

            DataTable dt4 = new DataTable();
            cmd4.Fill(dt4);

            if (dt4.Rows.Count > 0)
            {
                clear();
                DataRow dr4 = dt4.Rows[0];
                typeTextBox.Text = dr4.Field<string>(1);
                dateDateTimePicker.Value = dr4.Field<DateTime>(2);
                statComboBox.Text = dr4.Field<string>(3);
                resTextBox.Text = dr4.Field<string>(4);
                unitTextBox.Text = dr4.Field<string>(5);
                normTextBox.Text = dr4.Field<string>(6);
                devRichTextBox.Text = dr4.Field<string>(7);
                descrTextBox.Text = dr4.Field<string>(8);

             





                prev_info = true;
                dateDateTimePicker.Value = DateTime.Today;
            }
            else { }

        }
        private void make_ana_Load(object sender, EventArgs e)
        {
            List<string> stat = new List<string>() {  "В ожидании","Отменено","Выполнено"};
            //weekComboBox.DataSource = Names;
            statComboBox.DataSource = stat;


            string sql = "select id,last_name||' '||first_name||' '||patronymic as fio from employee;";
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(sql, con);
            DataTable dt1 = new DataTable();
            DataSet ds1 = new DataSet();
            ds1.Reset();
            da1.Fill(ds1);
            dt1 = ds1.Tables[0];
            empComboBox.DataSource = dt1;
            empComboBox.DisplayMember = "fio";
            empComboBox.ValueMember = "id";

            if (id_emp != -1)
            { empComboBox.SelectedItem = id_emp; empComboBox.Enabled = false; }

            sql = "select id,last_name||' '||first_name||' '||patronymic as fio from patient;";
            NpgsqlDataAdapter da2 = new NpgsqlDataAdapter(sql, con);
            DataTable dt2 = new DataTable();
            DataSet ds2 = new DataSet();
            ds2.Reset();
            da2.Fill(ds2);
            dt2 = ds2.Tables[0];
            patComboBox.DataSource = dt2;
            patComboBox.DisplayMember = "fio";
            patComboBox.ValueMember = "id";

            if (id_pat != -1)
            { patComboBox.SelectedIndex = id_pat; patComboBox.Enabled = false; }




            for_upd();
            for_upd2();

            empComboBox.Enabled = false;
        }


        private void for_upd()
        {
            string sql;
            if (id_emp != -1)
            {
                sql = "select id,last_name||' '||first_name||' '||patronymic as fio from employee where id=:id";
                NpgsqlDataAdapter cmd = new NpgsqlDataAdapter(sql, con);
                cmd.SelectCommand.Parameters.AddWithValue("id", this.id_emp);
                DataTable dt1 = new DataTable();
                cmd.Fill(dt1);
                DataRow dr = dt1.Select()[0];
                empComboBox.SelectedValue = dr.Field<int>(0);
            }
            if (id_pat != -1)
            {
                sql = "select id,last_name||' '||first_name||' '||patronymic as fio from patient where id=:id";
                NpgsqlDataAdapter cmd1 = new NpgsqlDataAdapter(sql, con);
                cmd1.SelectCommand.Parameters.AddWithValue("id", this.id_pat);
                DataTable dt2 = new DataTable();
                cmd1.Fill(dt2);
                DataRow dr1 = dt2.Select()[0];
                patComboBox.SelectedValue = dr1.Field<int>(0);
            }
        }

        private void dateDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            for_upd2();
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            if (prev_info)
            {
                string sql;
                sql = "UPDATE patient set polis=:polis,login=:login,first_name=:first_name,patronymic=:patronymic,last_name=:last_name,email=:email,phone=:phone,h_password=:h_password, birthdate=:birthdate,passport_issued=:passport_issued,passport_issued_in=:passport_issued_in,seria=:seria,numb=:numb, gender=:gender,snils=:snils where (id=:id);";
                sql = "UPDATE analysis set date_get=:date_get,type_analysis=:type_analysis,status_an=:status_ana,results=:results,mes_units=:mes_units,norm=:norm,deviation=:deviation,descr=:descr where (id_pat=:id_pat and id_emp=:id_emp);";
                NpgsqlCommand com = new NpgsqlCommand(sql, con);
                com = new NpgsqlCommand(sql, con);
                com.Parameters.AddWithValue("date_get", dateDateTimePicker.Value);
                com.Parameters.AddWithValue("id_pat", patComboBox.SelectedValue);
                com.Parameters.AddWithValue("id_emp", empComboBox.SelectedValue);
                com.Parameters.AddWithValue("type_analysis", typeTextBox.Text);
                com.Parameters.AddWithValue("results", resTextBox.Text);
                com.Parameters.AddWithValue("mes_units", unitTextBox.Text);
                com.Parameters.AddWithValue("norm", normTextBox.Text);
                com.Parameters.AddWithValue("deviation", devRichTextBox.Text);
                com.Parameters.AddWithValue("status_ana", statComboBox.SelectedValue);
                com.Parameters.AddWithValue("descr", descrTextBox.Text);
                com.ExecuteNonQuery();
                Close();
            }
            else
            {
                string sql;
                sql = "insert into analysis(date_get,type_analysis,id_emp,id_pat,status_an,results,mes_units,norm,deviation) values (:date_get,:type_analysis,:id_emp,:id_pat,:status_ana,:results,:mes_units,:norm,:deviation);";
                NpgsqlCommand com = new NpgsqlCommand(sql, con);
                com = new NpgsqlCommand(sql, con);
                com.Parameters.AddWithValue("date_get", dateDateTimePicker.Value);
                com.Parameters.AddWithValue("id_pat", patComboBox.SelectedValue);
                com.Parameters.AddWithValue("id_emp", empComboBox.SelectedValue);
                com.Parameters.AddWithValue("type_analysis", typeTextBox.Text);
                com.Parameters.AddWithValue("results",resTextBox.Text);
                com.Parameters.AddWithValue("mes_units", unitTextBox.Text);
                com.Parameters.AddWithValue("norm",normTextBox.Text);
                com.Parameters.AddWithValue("deviation",devRichTextBox.Text);
                com.Parameters.AddWithValue("status_ana", statComboBox.SelectedValue);
                com.ExecuteNonQuery();
                Close();
            }
        }

        private void more2Button_Click(object sender, EventArgs e)
        {
            pat_db f = new pat_db(con, "choose");
            this.Hide();
            f.ShowDialog();
            if (f.get_id() != -1)
            {
                patComboBox.SelectedValue = f.get_id();
            }
            this.Show();
        }

        private void patComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            for_upd2();
        }

        private void clear() 
        {

            typeTextBox.Text = "";
            dateDateTimePicker.Value = DateTime.Today;

            resTextBox.Text = "";
            unitTextBox.Text = "";
            normTextBox.Text = "";
            devRichTextBox.Text = "";
            descrTextBox.Text = "";
        }



        private void clearButton_Click(object sender, EventArgs e)
        {
            
            dateDateTimePicker.Value = DateTime.Today;
            clear();

        }
    }
}
