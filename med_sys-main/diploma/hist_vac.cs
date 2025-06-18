using HarfBuzzSharp;
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
    public partial class hist_vac : Form
    {
        public NpgsqlConnection con;
        int id_emp;
        int id_pat;
        bool prev_info = false;

        public hist_vac(NpgsqlConnection con, int id_emp, int id_pat)
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

        private void upd() 
        {
            vacLayoutPanel.Controls.Clear();
            string sql;
            NpgsqlDataAdapter cmd4;

            string name, name_dis, manuf,fio;
            DateTime date_get, date_exp;
            Decimal cost;
            Color col=Color.BlanchedAlmond;
            if (notExpCheckBox.Checked)
            {
                sql = "select v.id,v.name,d.name,v.manufacturer,v.cost,vac.date_get,vac.date_exp,emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio from vaccine v, vaccination vac,diseases d,employee emp where vac.id_emp=emp.id and vac.id_vac=v.id and vac.id_pat=@id_pat and v.id_dis=d.id and vac.date_exp>@date;";
                cmd4 = new NpgsqlDataAdapter(sql, con);
                cmd4.SelectCommand.Parameters.AddWithValue("@id_pat", id_pat);
                cmd4.SelectCommand.Parameters.AddWithValue("@date", DateTime.Today.Date);

            }
            else
            {
                sql = "select v.id,v.name,d.name,v.manufacturer,v.cost,vac.date_get,vac.date_exp,emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio from vaccine v, vaccination vac,diseases d,employee emp where vac.id_emp=emp.id and vac.id_vac=v.id and vac.id_pat=@id_pat and v.id_dis=d.id;";
                cmd4 = new NpgsqlDataAdapter(sql, con);
                cmd4.SelectCommand.Parameters.AddWithValue("@id_pat", id_pat);
            }
            DataTable dt4 = new DataTable();
            cmd4.Fill(dt4);
            DataRow dr4;
            if (dt4.Rows.Count > 0)
            {
                for (int i = 0; i < dt4.Rows.Count; i++)
                {
                    dr4 = dt4.Rows[i];
                    name = dr4.Field<string>(1);
                    name_dis = dr4.Field<string>(2);
                    manuf = dr4.Field<string>(3);
                    cost = dr4.Field<Decimal>(4);
                    date_get = dr4.Field<DateTime>(5);
                    date_exp = dr4.Field<DateTime>(6);
                    fio = dr4.Field<string>(7);

                    if (date_exp > DateTime.Today.Date) 
                    {
                        col = Color.Aquamarine;
                    
                    
                    }
                    Size s = new Size(Convert.ToInt32(vacLayoutPanel.Width),vacLayoutPanel.Height / 5);
                    vacUserControl vac = new vacUserControl(con,id_pat,s,name,name_dis,manuf,cost,date_get,date_exp,col,i+1,fio);
                    vacLayoutPanel.Controls.Add(vac);


                }

            }

        }

        private void hist_vac_Load(object sender, EventArgs e)
        {


            upd();

        }

        private void vacButton_Click(object sender, EventArgs e)
        {
            vac_db f = new vac_db(con, "look");
            this.Hide();
            f.ShowDialog();
            this.Show();
        }




        private void notExpCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            upd();
        }
    }
}
