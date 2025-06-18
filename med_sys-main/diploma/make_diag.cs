using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;
using System.Xml.Linq;
using System.IO;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Guna.UI2.WinForms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Cryptography;

using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
using System.Diagnostics;


namespace diploma
{
    public partial class make_diag : Form
    {
        public NpgsqlConnection con;
        int id_pat;
        int id_emp;
        int id_a;
        System.Data.DataTable dt = new System.Data.DataTable();
        DataSet ds = new DataSet();
        string emp;
        DataTable med;
        DataTable dis;
        string type_acc;
        DataTable dt2 = new DataTable();
        DataSet ds2 = new DataSet();
        public make_diag(NpgsqlConnection con, int id_pat, int id_emp, int id_a, string type_acc)
        {
            InitializeComponent();
            this.con = con;
            this.id_pat = id_pat;
            this.id_emp = id_emp;
            this.id_a = id_a;
            this.type_acc = type_acc;
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }



        private void upd()
        {
            dateLabel.Text = DateTime.Today.ToShortDateString();
            string sql = "Select emp.first_name ||' '|| emp.last_name ||' '|| emp.patronymic as fio1, pat.first_name ||' '|| pat.last_name ||' '|| pat.patronymic as fio2, pat.birthdate,p.name,a.date_app from Employee emp,Post p,Patient pat,appointment a where p.id=emp.id_post and pat.id=:id_pat and emp.id=:id_emp and a.id_spec=pat.id and a.id_emp=emp.id and a.id=:id_a;";

            NpgsqlDataAdapter cmd = new NpgsqlDataAdapter(sql, con);
            cmd.SelectCommand.Parameters.AddWithValue("id_pat", this.id_pat);
            cmd.SelectCommand.Parameters.AddWithValue("id_emp", this.id_emp);
            cmd.SelectCommand.Parameters.AddWithValue("id_a", this.id_a);
            DataTable dt1 = new DataTable();
            cmd.Fill(dt1);
            DataRow dr = dt1.Select()[0];
            fioEmpLabel.Text = dr.Field<string>(0);
            fioPatLabel.Text = dr.Field<string>(1);
            birthLabel.Text = (dr.Field<DateTime>(2)).ToShortDateString();
            postEmpLabel.Text = dr.Field<string>(3);
            dateLabel.Text = dr.Field<DateTime>(4).Date.ToShortDateString();



        }

        private void upd_look()
        {


            string sql = "Select diag.attendance,a.date_app from appointment a,diagnoses diag where diag.id_app=a.id and a.id=:id_a;";

            NpgsqlDataAdapter cmd1 = new NpgsqlDataAdapter(sql, con);

            cmd1.SelectCommand.Parameters.AddWithValue("id_a", this.id_a);
            DataTable dt1 = new DataTable();
            cmd1.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {
                DataRow dr = dt1.Select()[0];
                attenNumericUpDown.Value = dr.Field<int>(0);
                dateLabel.Text = dr.Field<DateTime>(1).Date.ToShortDateString();
            }












            sql = "Select med.name, dm.dosage, dm.duration from appointment a,medication med,diagnoses diag, diagnos_med dm where a.id=:id_a and dm.id_diag=diag.id and diag.id_app=a.id and med.id=dm.id_med;";

            NpgsqlDataAdapter cmd = new NpgsqlDataAdapter(sql, con);

            cmd.SelectCommand.Parameters.AddWithValue("id_a", this.id_a);
            DataTable dt_med = new DataTable();
            cmd.Fill(dt_med);
            dt_med.Columns["name"].ColumnName = "Название";
            dt_med.Columns["dosage"].ColumnName = "Дозирование";
            dt_med.Columns["duration"].ColumnName = "Длительность";


            this.med = dt_med;
            upd_med2();





            sql = "Select dis.name, dd.descr from appointment a,diseases dis,diagnoses diag, diagnos_dis dd where a.id=:id_a and dd.id_diag=diag.id and diag.id_app=a.id and dis.id=dd.id_dis;";

            cmd = new NpgsqlDataAdapter(sql, con);

            cmd.SelectCommand.Parameters.AddWithValue("id_a", this.id_a);
            DataTable dt_dis = new DataTable();
            cmd.Fill(dt_dis);
            dt_dis.Columns["name"].ColumnName = "Название";
            dt_dis.Columns["descr"].ColumnName = "Дополнительно";




            this.dis = dt_dis;
            upd_dis2();



        }

        private void make_diag_Load(object sender, EventArgs e)
        {
            onOwn2CheckBox.Visible = false;
            onOwnCheckBox.Visible = false;

            upd();
            disRichTextBox.Enabled = false;
            medRichTextBox.Enabled = false;
            if (type_acc == "look")
            {
                more1Button.Visible = false;
                more2Button.Visible = false;
                attenNumericUpDown.Enabled = false;
                onOwn2CheckBox.Visible = false;
                onOwnCheckBox.Visible = false;
                enterButton.Visible = false;

                upd_look();
            }

        }

        private void lookButton_Click(object sender, EventArgs e)
        {

            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                string targetFolderPath = Path.Combine(desktopPath, "Диагнозы");

                if (!Directory.Exists(targetFolderPath))
                {
                    Directory.CreateDirectory(targetFolderPath);
                }

                string fileName = $"diag_{fioPatLabel.Text}_{id_a}.docx";
                string filePath = Path.Combine(targetFolderPath, fileName);

                ExportDataToWord(filePath);
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Произошла непредвиденная ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




        }



        private void AddHorizontalLine(Word.Document doc, object missing)
        {
            Word.Paragraph paraLine = doc.Paragraphs.Add(ref missing);
  
            paraLine.Format.SpaceBefore = 6;
            paraLine.Format.SpaceAfter = 6; 

            Word.Border bottomBorder = paraLine.Borders[Word.WdBorderType.wdBorderBottom];
            bottomBorder.LineStyle = Word.WdLineStyle.wdLineStyleSingle; 
            bottomBorder.LineWidth = Word.WdLineWidth.wdLineWidth050pt; 
            bottomBorder.Color = Word.WdColor.wdColorAutomatic; 


          
        }


        private void ExportDataToWord(string filePath)
        {
            Word.Application wordApp = null;
            Word.Document wordDoc = null;
            object missing = Missing.Value;

            try
            {
                wordApp = new Word.Application();
                wordDoc = wordApp.Documents.Add(ref missing, ref missing, ref missing, ref missing);

               
                wordDoc.Content.Font.Name = "Consolas";
                wordDoc.Content.Font.Size = 14;



                Word.Range currentRange;

                // --- Раздел 1: Заголовок "Прием:" и дата ---
         
                Word.Paragraph paraReceptionText = wordDoc.Paragraphs.Add(ref missing);
                currentRange = paraReceptionText.Range;
                currentRange.Text = "Прием за: " + dateLabel.Text; ;
                currentRange.Font.Name = "Consolas";
                currentRange.Font.Size = 14;
                currentRange.Font.Bold = 1; 
                currentRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                currentRange.InsertParagraphAfter(); 

                // --- Разделитель ---
                AddHorizontalLine(wordDoc, missing);

                // --- Раздел 2: Информация о враче ---

                Word.Paragraph paraDoctor = wordDoc.Paragraphs.Last;
                currentRange = paraDoctor.Range;
                currentRange.Text = $"{empGroupBox.Text}";
                currentRange.Font.Name = "Consolas";
                currentRange.Font.Size = 14;
                currentRange.Font.Bold = 1;
                currentRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                                                                                                            
                currentRange.InsertParagraphAfter();




                Word.Paragraph paraDoctorInfo = wordDoc.Paragraphs.Last;
                currentRange = paraDoctorInfo.Range;
                currentRange.Text = $"{label3.Text} {fioEmpLabel.Text}{Environment.NewLine}" +
                                    $"{label5.Text} {postEmpLabel.Text}";
                currentRange.Font.Name = "Consolas";
                currentRange.Font.Size = 12;
                currentRange.Font.Bold = 0; 
                currentRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify; 
                                                                                                           
                currentRange.InsertParagraphAfter();

                // --- Разделитель ---
                AddHorizontalLine(wordDoc, missing);



                // --- Раздел 3: Информация о пациенте ---


                Word.Paragraph paraPatien = wordDoc.Paragraphs.Last;
                currentRange = paraPatien.Range;
                currentRange.Text = $"{patGroupBox.Text}";
                currentRange.Font.Name = "Consolas";
                currentRange.Font.Size = 14;
                currentRange.Font.Bold = 1; 
                currentRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter; 
                                                                                                            
                currentRange.InsertParagraphAfter();




                Word.Paragraph paraPatientInfo = wordDoc.Paragraphs.Last;
                currentRange = paraPatientInfo.Range;
                currentRange.Text = $"{label9.Text} {fioPatLabel.Text}{Environment.NewLine}" +
                                    $"{birthLabel1.Text} {birthLabel.Text}";
                currentRange.Font.Name = "Consolas";
                currentRange.Font.Size = 12;
                currentRange.Font.Bold = 0; 
                currentRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify; 
                                                                                                            
                currentRange.InsertParagraphAfter();


                AddHorizontalLine(wordDoc, missing);






                // --- Раздел 4: Информация о болезнях ---


                Word.Paragraph paraDis = wordDoc.Paragraphs.Last;
                currentRange = paraDis.Range;
                currentRange.Text = $"{disGroupBox.Text}";
                currentRange.Font.Name = "Consolas";
                currentRange.Font.Size = 14;
                currentRange.Font.Bold = 1; 
                currentRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter; 
                                                                                                            
                currentRange.InsertParagraphAfter();

                disRichTextBox.Enabled = true;

                Word.Paragraph paraDisInfo = wordDoc.Paragraphs.Last; 
                currentRange = paraDisInfo.Range;
                currentRange.Text = disRichTextBox.Text; 
                                                         
                                                        
                currentRange.Font.Name = "Consolas";
                currentRange.Font.Size = 12;
                currentRange.Font.Bold = 0; 
                currentRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify;
                                                                                                            
                currentRange.InsertParagraphAfter(); 


                disRichTextBox.Enabled = false;
                AddHorizontalLine(wordDoc, missing);






                // --- Раздел 5: Информация о лекарствах ---


                Word.Paragraph paraMed = wordDoc.Paragraphs.Last;
                currentRange = paraMed.Range;
                currentRange.Text = $"{medGroupBox.Text}";
                currentRange.Font.Name = "Consolas";
                currentRange.Font.Size = 14;
                currentRange.Font.Bold = 1; 
                currentRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter; 
                                                                                                           
                currentRange.InsertParagraphAfter();




                disRichTextBox.Enabled = true;

                Word.Paragraph paraMedInfo = wordDoc.Paragraphs.Last; 
                currentRange = paraMedInfo.Range;
                currentRange.Text = medRichTextBox.Text; 

                currentRange.Font.Name = "Consolas";
                currentRange.Font.Size = 12;
                currentRange.Font.Bold = 0; 
                currentRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphJustify; 
                                                                                                         
                currentRange.InsertParagraphAfter(); 


                disRichTextBox.Enabled = false;
                AddHorizontalLine(wordDoc, missing);

                // --- Раздел 6: Повтор и роспись ---

                Word.Paragraph atten = wordDoc.Paragraphs.Add(ref missing);
                currentRange = atten.Range;
                currentRange.Text = "Явка: через " + attenNumericUpDown.Value.ToString()+" дней/дня"; ;
                currentRange.Font.Name = "Consolas";
                currentRange.Font.Size = 12;
                currentRange.Font.Bold = 0; 
                currentRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                currentRange.InsertParagraphAfter();



                Word.Paragraph sig = wordDoc.Paragraphs.Add(ref missing);
                currentRange = atten.Range;
                currentRange.Text = "Подпись врача"+"_______________________" +"("+fioEmpLabel.Text+")"; ;
                currentRange.Font.Name = "Consolas";
                currentRange.Font.Size = 12;
                currentRange.Font.Bold = 0;
                currentRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                currentRange.InsertParagraphAfter(); 







                wordDoc.SaveAs2(filePath, ref missing, ref missing, ref missing, ref missing, ref missing,
                                ref missing, ref missing, ref missing, ref missing, ref missing,
                                ref missing, ref missing, ref missing, ref missing, ref missing);





                MessageBox.Show($"Документ успешно сохранен как: {filePath}", "Экспорт завершен", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при экспорте в Word: " + ex.Message, "Ошибка экспорта", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
               Process.Start(filePath);
                
                if (wordDoc != null)
                {
                    wordDoc.Close(false, ref missing, ref missing); 

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wordDoc);
                    wordDoc = null;
                }
                if (wordApp != null)
                {
                    wordApp.Quit(false, ref missing, ref missing);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);
                    wordApp = null;
                }
                
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }



        private void upd_dis2()
        {

            disRichTextBox.Font = new Font("Consolas", 10);
            disRichTextBox.Clear();
            if (dis != null && dis.Columns.Count > 0)
            {
                int numColumns = dis.Columns.Count;
                int[] columnWidths = new int[numColumns];
                string columnSeparator = " | ";
                int rowNumberingWidth = (dis.Rows.Count.ToString().Length + 2);

                for (int j = 0; j < numColumns; j++)
                {
                    columnWidths[j] = dis.Columns[j].ColumnName.Length;
                }

                foreach (DataRow row in dis.Rows)
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
                    headerBuilder.Append(dis.Columns[j].ColumnName.PadRight(columnWidths[j]));
                    if (j < numColumns - 1)
                    {
                        headerBuilder.Append(columnSeparator);
                    }
                }
                disRichTextBox.AppendText(headerBuilder.ToString() + Environment.NewLine);


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
                disRichTextBox.AppendText(lineBuilder.ToString() + Environment.NewLine);


                int i = 1;
                foreach (DataRow row in dis.Rows)
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
                    disRichTextBox.AppendText(rowBuilder.ToString() + Environment.NewLine);
                    i++;
                }
            }

        }
        private void upd_med2() {

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

        private void upd_med() 
        {
            medRichTextBox.Clear();
            if (med!=null)
            {

                foreach (DataColumn column in med.Columns)
                {
                    medRichTextBox.AppendText(column.ColumnName + " | \t");
                }

                medRichTextBox.AppendText(Environment.NewLine);
                int i = 1;
                foreach (DataRow row in med.Rows)
                {
                    medRichTextBox.AppendText(i + ")");
                    foreach (var item in row.ItemArray)
                    {
                        medRichTextBox.AppendText(item.ToString() + " |\t");
                    }
                    medRichTextBox.AppendText(Environment.NewLine);
                    i++;
                }
            }
        }

        private void upd_dis()
        {
            disRichTextBox.Clear();
            if (dis!=null)
            {
                foreach (DataColumn column in dis.Columns)
                {
                    disRichTextBox.AppendText(column.ColumnName + " | \t");
                }

                disRichTextBox.AppendText(Environment.NewLine);
                int i = 1;
                foreach (DataRow row in dis.Rows)
                {
                    disRichTextBox.AppendText(i + ")");
                    foreach (var item in row.ItemArray)
                    {
                        disRichTextBox.AppendText(item.ToString() + " |\t");
                    }
                    disRichTextBox.AppendText(Environment.NewLine);
                    i++;
                }
            }
        }
        private void more1Button_Click(object sender, EventArgs e)
        {
           choose_med f = new choose_med(con, id_pat,id_emp);
            this.Hide();
            f.ShowDialog();
            if(f.get_full())
                med=f.get_med();
            upd_med2();
            this.Show();
        }

        private void onOwnCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (onOwnCheckBox.Checked)
                disRichTextBox.Enabled = true;
            else disRichTextBox.Enabled = false;
        }

        private void onOwn2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (onOwn2CheckBox.Checked)
                medRichTextBox.Enabled = true;
            else medRichTextBox.Enabled = false;
        }

        private void more2Button_Click(object sender, EventArgs e)
        {
            choose_dis f = new choose_dis(con, id_pat, id_emp);
            this.Hide();
            f.ShowDialog();
            if (f.get_full())
                dis = f.get_dis();
            upd_dis2();
            this.Show();
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            if (type_acc != "look")
            {
                string sql;
                sql = "insert into diagnoses(id_app,attendance) values (:id_app,:attendance);";
                NpgsqlCommand com = new NpgsqlCommand(sql, con);
                com.Parameters.AddWithValue("id_app", id_a);
                com.Parameters.AddWithValue("attendance", attenNumericUpDown.Value);
                com.ExecuteNonQuery();

                sql = "Select id from diagnoses where id_app=:id_app;";
                NpgsqlDataAdapter cmd = new NpgsqlDataAdapter(sql, con);
                cmd.SelectCommand.Parameters.AddWithValue("id_app", id_a);
                dt2 = new DataTable();
                cmd.Fill(dt2);
                DataRow dr1 = dt2.Select()[0];
                int id_diag = dr1.Field<int>(0);
                int id_med;


                DataRow dr4;

                if (med!=null)
                {
                    for (int i = 0; i < med.Rows.Count; i++)
                    {
                        dr4 = med.Rows[i];

                        sql = "Select id from medication where name=:name;";
                        cmd = new NpgsqlDataAdapter(sql, con);
                        cmd.SelectCommand.Parameters.AddWithValue("name", dr4.Field<string>(0));
                        dt2 = new DataTable();
                        cmd.Fill(dt2);
                        dr1 = dt2.Select()[0];
                        id_med = dr1.Field<int>(0);



                        sql = "insert into diagnos_med(id_diag,id_med,dosage,duration) values (:id_diag,:id_med,:dosage,:duration);";
                        com = new NpgsqlCommand(sql, con);
                        com.Parameters.AddWithValue("id_diag", id_diag);
                        com.Parameters.AddWithValue("id_med", id_med);
                        com.Parameters.AddWithValue("dosage", dr4.Field<string>(1));
                        com.Parameters.AddWithValue("duration", dr4.Field<string>(2));
                        com.ExecuteNonQuery();

                    }
                }

                int id_dis;


                DataRow dr5;

                if (dis!=null)
                {
                    for (int i = 0; i < dis.Rows.Count; i++)
                    {
                        dr5 = dis.Rows[i];

                        sql = "Select id from diseases where name=:name;";
                        cmd = new NpgsqlDataAdapter(sql, con);
                        cmd.SelectCommand.Parameters.AddWithValue("name", dr5.Field<string>(0));
                        dt2 = new DataTable();
                        cmd.Fill(dt2);
                        dr1 = dt2.Select()[0];
                        id_dis = dr1.Field<int>(0);



                        sql = "insert into diagnos_dis(id_diag,id_dis,descr) values (:id_diag,:id_dis,:descr);";
                        com = new NpgsqlCommand(sql, con);
                        com.Parameters.AddWithValue("id_diag", id_diag);
                        com.Parameters.AddWithValue("id_dis", id_dis);
                        com.Parameters.AddWithValue("descr", dr5.Field<string>(1));
                        com.ExecuteNonQuery();

                    }
                }





            }
            else {
            
            
            
            
            }
            Close();





        }
    }
}
