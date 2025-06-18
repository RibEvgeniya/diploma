using Microsoft.Vbe.Interop;
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
    public partial class add_up_more_info : Form
    {
        public NpgsqlConnection con;
        int id_pat,id_emp;
        string type_acc;
        bool compl;
 decimal height,weight;
     string act, blood, smok, alch, medAl, al,chrDis,other;

        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        public add_up_more_info(NpgsqlConnection con, int id_pat,int id_emp,string type_acc)
        {
            InitializeComponent();
            this.con = con;
            this.id_pat = id_pat;
            this.id_emp = id_emp;
            this.compl = false;
            this.type_acc = type_acc;
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }








        private void enterButton_Click(object sender, EventArgs e)
        {
            if (type_acc == "look") { Close(); }

            this.compl = true;

           
            this.height= heightNumericUpDown.Value;
            this.weight =weightNumericUpDown.Value;
            if (lowRadioButton.Checked)
                this.act =lowRadioButton.Text;
            else if (highRadioButton.Checked)
                this.act =highRadioButton.Text;
            else
                this.act =middleRadioButton.Text;
            blood= bloodComboBox.SelectedValue.ToString();
            if (smokRadioButton.Checked)
                this.smok = smokRadioButton.Text;
            else
                this.smok = noSmokRadioButton.Text;

            if (alcRadioButton.Checked)
                this.alch = alcRadioButton.Text;
            else
                this.alch = noAlcRadioButton.Text;
            medAl=medAlTextBox.Text;
            al=alTextBox.Text;
            chrDis=chrDisTextBox.Text;
            other=otherTextBox.Text;








            Close();
        }

        public bool get_compl() 
        {
            return this.compl;
        }

        public decimal get_height()
        {
            return this.height;
        }
        public decimal get_weight()
        {
            return this.weight;
        }
        public string get_blood()
        {
            return this.blood;
        }
        public string get_alch()
        {
            return this.alch;
        }
        public string get_al()
        {
            return this.al;
        }
        public string get_med_al()
        {
            return this.medAl;
        }
        public string get_chrDis()
        {
            return this.chrDis;
        }
        public string get_other()
        {
            return this.other;
        }
        public string get_smok()
        {
            return this.smok;
        }
        public string get_act()
        {
            return this.act;
        }

        private void addGroupBox_Click(object sender, EventArgs e)
        {



        }



        private void for_upd(int id)
        {


            string sql = "Select pat.id,pat.height,pat.weight,pat.phys_activ, pat.bloodType,pat.smoking_status,pat.alcohol,pat.med_allergy,pat.allergies,pat.chronic_dis,pat.other from patient_add_info pat where pat.id_pat=:id";
            NpgsqlDataAdapter cmd = new NpgsqlDataAdapter(sql, con);
            cmd.SelectCommand.Parameters.AddWithValue("id", id);
            DataTable dt1 = new DataTable();
            cmd.Fill(dt1);
            if (dt1.Rows.Count >0)
            {
                DataRow dr = dt1.Select()[0];

                heightNumericUpDown.Value = dr.Field<int>(1);
                weightNumericUpDown.Value = dr.Field<Decimal>(2);
               

                if (dr.Field<string>(3) == lowRadioButton.Text)
                    lowRadioButton.Checked = true;
                else if (dr.Field<string>(3) == middleRadioButton.Text)
                    middleRadioButton.Checked = true;
                else {highRadioButton.Checked = true;}


                bloodComboBox.Text = dr.Field<string>(4);
                Console.WriteLine("'dr.Field<string>(5)'");
                Console.WriteLine(dr.Field<string>(5));
                Console.WriteLine(smokRadioButton.Text);
                if (dr.Field<string>(5) == smokRadioButton.Text)
                {
                    smokRadioButton.Checked = true;
                    noSmokRadioButton.Checked = false;
                }
                else { noSmokRadioButton.Checked = true; }
                if (dr.Field<string>(6) == alcRadioButton.Text)
                    alcRadioButton.Checked = true;
                else { noAlcRadioButton.Checked = true; }

                medAlTextBox.Text = dr.Field<string>(7);
                alTextBox.Text = dr.Field<string>(8);
                chrDisTextBox.Text = dr.Field<string>(9);
                otherTextBox.Text= dr.Field<string>(10);





            }



        }



        private void add_up_more_info_Load(object sender, EventArgs e)
        {
            List<string> bl = new List<string>() {  "O(I) Rh+ — первая положительная","O(I) Rh− — первая отрицательная",
"A(II) Rh+ — вторая положительная","A(II) Rh− — вторая отрицательная","B (III) Rh+ — третья положительная",
"B (III) Rh− — третья отрицательная","AB (IV) Rh+ — четвёртая положительная" };
            //weekComboBox.DataSource = Names;
            bloodComboBox.DataSource = bl;



            if (type_acc == "look")
            {
             

          
                for_upd(this.id_pat);
                heightNumericUpDown.Enabled = false;
                weightNumericUpDown.Enabled = false;
                lowRadioButton.Enabled = false;
                highRadioButton.Enabled = false;
                middleRadioButton.Enabled = false;
                smokRadioButton.Enabled = false;
                noAlcRadioButton.Enabled = false;
                noSmokRadioButton.Enabled = false;
                alcRadioButton.Enabled = false;
                alTextBox.Enabled = false;
                medAlTextBox.Enabled = false;
                bloodComboBox.Enabled = false;
                chrDisTextBox.Enabled = false;
                otherTextBox.Enabled = false;
                //for_upd(this.id_pat);

            }
            if (id_pat != -1) 
            {
                for_upd(this.id_pat);
                compl = true;
            }


        }
    }
}
