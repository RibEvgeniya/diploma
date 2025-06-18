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
    public partial class schema_db : Form
    {
        public NpgsqlConnection con;
        public schema_db(NpgsqlConnection con)
        {
            this.con = con;
            InitializeComponent();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void schema_db_Load(object sender, EventArgs e)
        {

        }
    }
}
