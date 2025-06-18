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
    public partial class blankUserControl : UserControl
    {
        Size s;
        public blankUserControl(Size s)
        {
            this.s = s;
            InitializeComponent();
        }

        private void blankUserControl_Load(object sender, EventArgs e)
        {
            this.Size = s;
        }
    }
}
