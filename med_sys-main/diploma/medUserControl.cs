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
using System.Xml.Linq;
using System.Xml;
using System.Runtime.InteropServices.ComTypes;

namespace diploma
{
    public partial class medUserControl : UserControl
    {
        NpgsqlConnection con;
        int id_diag;
        int id_emp;
        int id_pat;

        Size s;
        Color c;

        string fio_emp,fio_pat;
        DataTable med;
        List<string> dis;
        DateTime date;

        public medUserControl(NpgsqlConnection con, int id_diag, int id_emp, Size s, string fio_emp, List<String> dis, DataTable med, DateTime date, int id_pat)
        {
            this.con = con;
            this.id_diag= id_diag;
            this.id_emp = id_emp;
            this.s = s;
            this.dis = dis;
            InitializeComponent();
           this.fio_emp = fio_emp;
            this.date = date;
            this.id_pat = id_pat;
            this.med = med;
            //this.med.Columns["name"].ColumnName = "Название";
            //this.med.Columns["dosage"].ColumnName = "Дозирование";
            //this.med.Columns["duration"].ColumnName = "Длительность";
        }

        private void medUserControl_Load(object sender, EventArgs e)
        {
            this.Width = s.Width;
            //this.BackColor = col;
            //numLabel.Text = num.ToString() + "). ";
            dateLabel.Text = date.Date.ToShortDateString();
            for (int i = 0; i < dis.Count; i++)
                disRichTextBox.Text += (i + 1) + ")" + dis[i] + "\n";
            fioLabel.Text = fio_emp.ToString();




            medRichTextBox.Font = new Font("Consolas", 10);
            medRichTextBox.Clear();
            if (med != null && med.Columns.Count > 0)
            {
                int numColumns = med.Columns.Count;
                int[] columnWidths = new int[numColumns];
                string columnSeparator = " | "; 
                int rowNumberingWidth = (med.Rows.Count.ToString().Length + 2); 

             
                for (int j = 0; j < numColumns; j++)
                {
                    columnWidths[j] = med.Columns[j].ColumnName.Length;
                }

                foreach (DataRow row in med.Rows)
                {
                    for (int j = 0; j < numColumns; j++)
                    {
                
                        string cellValue = row[j]?.ToString() ?? "";
                        if (cellValue.Length > columnWidths[j])
                        {
                            columnWidths[j] = cellValue.Length;
                        }
                    }
                }

                
                StringBuilder headerBuilder = new StringBuilder();
                headerBuilder.Append("".PadRight(rowNumberingWidth)); 

                for (int j = 0; j < numColumns; j++)
                {
                    headerBuilder.Append(med.Columns[j].ColumnName.PadRight(columnWidths[j]));
                    if (j < numColumns - 1) 
                    {
                        headerBuilder.Append(columnSeparator);
                    }
                }
                medRichTextBox.AppendText(headerBuilder.ToString() + Environment.NewLine);

                
                StringBuilder lineBuilder = new StringBuilder();
                lineBuilder.Append("".PadRight(rowNumberingWidth)); 

                for (int j = 0; j < numColumns; j++)
                {
                    lineBuilder.Append(new string('-', columnWidths[j]));
                    if (j < numColumns - 1)
                    {
                        lineBuilder.Append(new string('-', columnSeparator.Length));
                    }
                }
                medRichTextBox.AppendText(lineBuilder.ToString() + Environment.NewLine);


                
                int i = 1;
                foreach (DataRow row in med.Rows)
                {
                    StringBuilder rowBuilder = new StringBuilder();
                
                    rowBuilder.Append((i + ")").PadRight(rowNumberingWidth));

                    for (int j = 0; j < numColumns; j++)
                    {
                        string cellValue = row[j]?.ToString() ?? ""; 
                        rowBuilder.Append(cellValue.PadRight(columnWidths[j]));
                        if (j < numColumns - 1) 
                        {
                            rowBuilder.Append(columnSeparator);
                        }
                    }
                    medRichTextBox.AppendText(rowBuilder.ToString() + Environment.NewLine);
                    i++;
                }
            }
        }
    }
}
