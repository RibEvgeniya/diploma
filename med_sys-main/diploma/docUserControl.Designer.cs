namespace diploma
{
    partial class docUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(docUserControl));
            this.actualFioLabel = new System.Windows.Forms.Label();
            this.actualSpecLabel = new System.Windows.Forms.Label();
            this.actualExpLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.actualCostLabel = new System.Windows.Forms.Label();
            this.photoPictureBox = new Guna.UI2.WinForms.Guna2PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.photoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // actualFioLabel
            // 
            this.actualFioLabel.AutoSize = true;
            this.actualFioLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.actualFioLabel.Location = new System.Drawing.Point(23, 282);
            this.actualFioLabel.Name = "actualFioLabel";
            this.actualFioLabel.Size = new System.Drawing.Size(70, 29);
            this.actualFioLabel.TabIndex = 87;
            this.actualFioLabel.Text = "ФИО";
            this.actualFioLabel.Click += new System.EventHandler(this.actualFioLabel_Click);
            // 
            // actualSpecLabel
            // 
            this.actualSpecLabel.Location = new System.Drawing.Point(14, 311);
            this.actualSpecLabel.Name = "actualSpecLabel";
            this.actualSpecLabel.Size = new System.Drawing.Size(332, 67);
            this.actualSpecLabel.TabIndex = 86;
            this.actualSpecLabel.Text = "Специализация";
            this.actualSpecLabel.Click += new System.EventHandler(this.actualSpecLabel_Click);
            // 
            // actualExpLabel
            // 
            this.actualExpLabel.AutoSize = true;
            this.actualExpLabel.Location = new System.Drawing.Point(136, 378);
            this.actualExpLabel.Name = "actualExpLabel";
            this.actualExpLabel.Size = new System.Drawing.Size(64, 25);
            this.actualExpLabel.TabIndex = 88;
            this.actualExpLabel.Text = "Стаж";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(68, 416);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 25);
            this.label1.TabIndex = 89;
            this.label1.Text = "Прием";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(68, 378);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 25);
            this.label2.TabIndex = 90;
            this.label2.Text = "Стаж";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // actualCostLabel
            // 
            this.actualCostLabel.AutoSize = true;
            this.actualCostLabel.Location = new System.Drawing.Point(152, 416);
            this.actualCostLabel.Name = "actualCostLabel";
            this.actualCostLabel.Size = new System.Drawing.Size(120, 25);
            this.actualCostLabel.TabIndex = 91;
            this.actualCostLabel.Text = "Стоимость";
            // 
            // photoPictureBox
            // 
            this.photoPictureBox.FillColor = System.Drawing.Color.DimGray;
            this.photoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("photoPictureBox.Image")));
            this.photoPictureBox.ImageLocation = "";
            this.photoPictureBox.ImageRotate = 0F;
            this.photoPictureBox.InitialImage = null;
            this.photoPictureBox.Location = new System.Drawing.Point(38, 6);
            this.photoPictureBox.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.photoPictureBox.Name = "photoPictureBox";
            this.photoPictureBox.Size = new System.Drawing.Size(244, 245);
            this.photoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.photoPictureBox.TabIndex = 68;
            this.photoPictureBox.TabStop = false;
            // 
            // docUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.BlanchedAlmond;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.actualCostLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.actualExpLabel);
            this.Controls.Add(this.actualFioLabel);
            this.Controls.Add(this.actualSpecLabel);
            this.Controls.Add(this.photoPictureBox);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "docUserControl";
            this.Size = new System.Drawing.Size(342, 473);
            this.Load += new System.EventHandler(this.docUserControl_Load);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.docUserControl_MouseDoubleClick);
            this.MouseLeave += new System.EventHandler(this.docUserControl_MouseLeave);
            this.MouseHover += new System.EventHandler(this.docUserControl_MouseHover);
            ((System.ComponentModel.ISupportInitialize)(this.photoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2PictureBox photoPictureBox;
        private System.Windows.Forms.Label actualFioLabel;
        private System.Windows.Forms.Label actualSpecLabel;
        private System.Windows.Forms.Label actualExpLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label actualCostLabel;
    }
}
