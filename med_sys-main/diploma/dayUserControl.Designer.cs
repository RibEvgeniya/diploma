namespace diploma
{
    partial class dayUserControl
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
            this.listLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.countLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateLabel = new System.Windows.Forms.Label();
            this.todayLabel = new System.Windows.Forms.Label();
            this.doneCheckBox = new Guna.UI2.WinForms.Guna2CustomCheckBox();
            this.SuspendLayout();
            // 
            // listLayoutPanel
            // 
            this.listLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listLayoutPanel.Location = new System.Drawing.Point(3, 69);
            this.listLayoutPanel.Name = "listLayoutPanel";
            this.listLayoutPanel.Size = new System.Drawing.Size(152, 53);
            this.listLayoutPanel.TabIndex = 13;
            this.listLayoutPanel.MouseLeave += new System.EventHandler(this.listLayoutPanel_MouseLeave);
            this.listLayoutPanel.MouseHover += new System.EventHandler(this.listLayoutPanel_MouseHover);
            // 
            // countLabel
            // 
            this.countLabel.AutoSize = true;
            this.countLabel.Location = new System.Drawing.Point(78, 44);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(78, 16);
            this.countLabel.TabIndex = 14;
            this.countLabel.Text = "К. приемов";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 16);
            this.label1.TabIndex = 15;
            this.label1.Text = "Приемов:";
            // 
            // dateLabel
            // 
            this.dateLabel.AutoSize = true;
            this.dateLabel.Location = new System.Drawing.Point(3, 14);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(21, 16);
            this.dateLabel.TabIndex = 16;
            this.dateLabel.Text = "00";
            // 
            // todayLabel
            // 
            this.todayLabel.AutoSize = true;
            this.todayLabel.Location = new System.Drawing.Point(30, 14);
            this.todayLabel.Name = "todayLabel";
            this.todayLabel.Size = new System.Drawing.Size(73, 16);
            this.todayLabel.TabIndex = 17;
            this.todayLabel.Text = "СЕГОДНЯ";
            // 
            // doneCheckBox
            // 
            this.doneCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.doneCheckBox.CheckedState.BorderColor = System.Drawing.Color.LawnGreen;
            this.doneCheckBox.CheckedState.BorderRadius = 2;
            this.doneCheckBox.CheckedState.BorderThickness = 1;
            this.doneCheckBox.CheckedState.FillColor = System.Drawing.Color.Gray;
            this.doneCheckBox.Location = new System.Drawing.Point(138, 14);
            this.doneCheckBox.Name = "doneCheckBox";
            this.doneCheckBox.Size = new System.Drawing.Size(17, 18);
            this.doneCheckBox.TabIndex = 29;
            this.doneCheckBox.Text = "guna2CustomCheckBox1";
            this.doneCheckBox.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.doneCheckBox.UncheckedState.BorderRadius = 2;
            this.doneCheckBox.UncheckedState.BorderThickness = 0;
            this.doneCheckBox.UncheckedState.FillColor = System.Drawing.Color.OliveDrab;
            // 
            // dayUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AntiqueWhite;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.doneCheckBox);
            this.Controls.Add(this.todayLabel);
            this.Controls.Add(this.dateLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.countLabel);
            this.Controls.Add(this.listLayoutPanel);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "dayUserControl";
            this.Size = new System.Drawing.Size(160, 132);
            this.Load += new System.EventHandler(this.dayUserControl_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dayUserControl_MouseClick);
            this.MouseLeave += new System.EventHandler(this.dayUserControl_MouseLeave);
            this.MouseHover += new System.EventHandler(this.dayUserControl_MouseHover);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel listLayoutPanel;
        private System.Windows.Forms.Label countLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.Label todayLabel;
        private Guna.UI2.WinForms.Guna2CustomCheckBox doneCheckBox;
    }
}
