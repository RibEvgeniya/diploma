namespace diploma
{
    partial class medUserControl
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
            this.medRichTextBox = new System.Windows.Forms.RichTextBox();
            this.dateLabel = new System.Windows.Forms.Label();
            this.fioLabel = new System.Windows.Forms.Label();
            this.disRichTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // medRichTextBox
            // 
            this.medRichTextBox.BackColor = System.Drawing.Color.AntiqueWhite;
            this.medRichTextBox.Location = new System.Drawing.Point(539, 30);
            this.medRichTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.medRichTextBox.Name = "medRichTextBox";
            this.medRichTextBox.ReadOnly = true;
            this.medRichTextBox.Size = new System.Drawing.Size(785, 196);
            this.medRichTextBox.TabIndex = 14;
            this.medRichTextBox.Text = "";
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Location = new System.Drawing.Point(-7, 18);
            this.dateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(120, 25);
            this.dateLabel.TabIndex = 9;
            this.dateLabel.Text = "12.12.1111";
            // 
            // fioLabel
            // 
            this.fioLabel.Location = new System.Drawing.Point(131, 36);
            this.fioLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.fioLabel.Name = "fioLabel";
            this.fioLabel.Size = new System.Drawing.Size(318, 53);
            this.fioLabel.TabIndex = 8;
            this.fioLabel.Text = "ФИО";
            // 
            // disRichTextBox
            // 
            this.disRichTextBox.BackColor = System.Drawing.Color.AntiqueWhite;
            this.disRichTextBox.Location = new System.Drawing.Point(20, 106);
            this.disRichTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.disRichTextBox.Name = "disRichTextBox";
            this.disRichTextBox.ReadOnly = true;
            this.disRichTextBox.Size = new System.Drawing.Size(511, 120);
            this.disRichTextBox.TabIndex = 15;
            this.disRichTextBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(131, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 25);
            this.label1.TabIndex = 16;
            this.label1.Text = "Назначил:";
            // 
            // medUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AntiqueWhite;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.disRichTextBox);
            this.Controls.Add(this.medRichTextBox);
            this.Controls.Add(this.dateLabel);
            this.Controls.Add(this.fioLabel);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "medUserControl";
            this.Size = new System.Drawing.Size(1354, 250);
            this.Load += new System.EventHandler(this.medUserControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox medRichTextBox;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.Label fioLabel;
        private System.Windows.Forms.RichTextBox disRichTextBox;
        private System.Windows.Forms.Label label1;
    }
}
