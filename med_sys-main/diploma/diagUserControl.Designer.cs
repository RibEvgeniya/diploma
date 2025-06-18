namespace diploma
{
    partial class diagUserControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.fioLabel = new System.Windows.Forms.Label();
            this.dateLabel = new System.Windows.Forms.Label();
            this.procLabel = new System.Windows.Forms.Label();
            this.costLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // fioLabel
            // 
            this.fioLabel.AutoSize = true;
            this.fioLabel.Location = new System.Drawing.Point(103, 22);
            this.fioLabel.Name = "fioLabel";
            this.fioLabel.Size = new System.Drawing.Size(38, 16);
            this.fioLabel.TabIndex = 0;
            this.fioLabel.Text = "ФИО";
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Location = new System.Drawing.Point(20, 22);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(43, 16);
            this.dateLabel.TabIndex = 1;
            this.dateLabel.Text = "ДАТА";
            // 
            // procLabel
            // 
            this.procLabel.AutoSize = true;
            this.procLabel.Location = new System.Drawing.Point(135, 79);
            this.procLabel.Name = "procLabel";
            this.procLabel.Size = new System.Drawing.Size(92, 16);
            this.procLabel.TabIndex = 2;
            this.procLabel.Text = "ПРОЦЕДУРА";
            // 
            // costLabel
            // 
            this.costLabel.AutoSize = true;
            this.costLabel.Location = new System.Drawing.Point(135, 108);
            this.costLabel.Name = "costLabel";
            this.costLabel.Size = new System.Drawing.Size(93, 16);
            this.costLabel.TabIndex = 3;
            this.costLabel.Text = "СТОИМОСТЬ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "ПРОЦЕДУРА:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "СТОИМОСТЬ:";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.AntiqueWhite;
            this.richTextBox1.Location = new System.Drawing.Point(294, 22);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(323, 127);
            this.richTextBox1.TabIndex = 7;
            this.richTextBox1.Text = "";
            // 
            // diagUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AntiqueWhite;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.costLabel);
            this.Controls.Add(this.procLabel);
            this.Controls.Add(this.dateLabel);
            this.Controls.Add(this.fioLabel);
            this.Name = "diagUserControl";
            this.Size = new System.Drawing.Size(620, 171);
            this.Load += new System.EventHandler(this.diagUserControl_Load);
            this.DoubleClick += new System.EventHandler(this.diagUserControl_DoubleClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.diagUserControl_MouseDoubleClick);
            this.MouseLeave += new System.EventHandler(this.diagUserControl_MouseLeave);
            this.MouseHover += new System.EventHandler(this.diagUserControl_MouseHover);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label fioLabel;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.Label procLabel;
        private System.Windows.Forms.Label costLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}
