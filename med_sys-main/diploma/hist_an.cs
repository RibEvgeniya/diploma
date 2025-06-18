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
    public partial class hist_an : Form
    {
        public NpgsqlConnection con;
        int id_emp;
        int id_pat;
        bool prev_info = false;
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        public hist_an(NpgsqlConnection con, int id_emp, int id_pat)
        {
            this.con = con;
            InitializeComponent();
            this.id_emp = id_emp;
            this.id_pat = id_pat;

        }




        private void upd()
        {
            anLayoutPanel.Controls.Clear();
            string sql;
            NpgsqlDataAdapter cmd4;

            string mes_units, result, norm, fio,type_analysis,dev;
            DateTime date_get;
            //Decimal cost;
            Color col = Color.BlanchedAlmond;
            if (anComboBox.Items.Count > 0)
            {
                Console.WriteLine(anComboBox.SelectedValue.ToString());
                sql = "select a.id,a.type_analysis,a.results,a.mes_units,a.norm,a.deviation,a.date_get,emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio from analysis a, employee emp where a.id_emp=emp.id and a.id_pat=@id_pat and a.type_analysis=@name;";
                cmd4 = new NpgsqlDataAdapter(sql, con);
                cmd4.SelectCommand.Parameters.AddWithValue("@id_pat", id_pat);
                cmd4.SelectCommand.Parameters.AddWithValue("@name", anComboBox.SelectedValue.ToString());
                DataTable dt4 = new DataTable();
                cmd4.Fill(dt4);
                DataRow dr4;
                if (dt4.Rows.Count > 0)
                {
                    for (int i = 0; i < dt4.Rows.Count; i++)
                    {
                        dr4 = dt4.Rows[i];
                        type_analysis = dr4.Field<string>(1);
                        result = dr4.Field<string>(2);
                        mes_units = dr4.Field<string>(3);
                        norm = dr4.Field<string>(4);
                        dev = dr4.Field<string>(5);
                        date_get = dr4.Field<DateTime>(6);
                        fio = dr4.Field<string>(7);
                        Size s = new Size(Convert.ToInt32(anLayoutPanel.Width), anLayoutPanel.Height / 5);
                        anUserControl an = new anUserControl(con, id_pat, s, type_analysis, result, mes_units, norm, dev, date_get, col, i + 1, fio);
                        anLayoutPanel.Controls.Add(an);


                    }
                }
            }

        }


        private void hist_an_Load(object sender, EventArgs e)
        {
            string sql = "Select  distinct type_analysis,id from analysis where id_pat=:id;";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            da.SelectCommand.Parameters.AddWithValue("id", id_pat);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            anComboBox.DataSource = dt;
            anComboBox.DisplayMember = "type_analysis";
            anComboBox.ValueMember = "type_analysis";

            upd();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void anComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            upd();
        }
    }
}
