﻿namespace diploma
{
    partial class add_up_spec
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.guna2MessageDialog1 = new Guna.UI2.WinForms.Guna2MessageDialog();
            this.backPanel = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            this.enterButton = new Guna.UI2.WinForms.Guna2Button();
            this.addGroupBox = new Guna.UI2.WinForms.Guna2GroupBox();
            this.descRichTextBox = new System.Windows.Forms.RichTextBox();
            this.nameTextBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.backButton = new Guna.UI2.WinForms.Guna2Button();
            this.guna2ControlBox3 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.guna2ControlBox2 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.guna2ControlBox1 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.backPanel.SuspendLayout();
            this.addGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2BorderlessForm1
            // 
            this.guna2BorderlessForm1.ContainerControl = this;
            this.guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // guna2MessageDialog1
            // 
            this.guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
            this.guna2MessageDialog1.Caption = null;
            this.guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.None;
            this.guna2MessageDialog1.Parent = null;
            this.guna2MessageDialog1.Style = Guna.UI2.WinForms.MessageDialogStyle.Default;
            this.guna2MessageDialog1.Text = null;
            // 
            // backPanel
            // 
            this.backPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.backPanel.BackColor = System.Drawing.Color.Transparent;
            this.backPanel.BorderColor = System.Drawing.Color.Black;
            this.backPanel.BorderRadius = 1;
            this.backPanel.BorderThickness = 1;
            this.backPanel.Controls.Add(this.enterButton);
            this.backPanel.Controls.Add(this.addGroupBox);
            this.backPanel.Controls.Add(this.backButton);
            this.backPanel.Controls.Add(this.guna2ControlBox3);
            this.backPanel.Controls.Add(this.guna2ControlBox2);
            this.backPanel.Controls.Add(this.guna2ControlBox1);
            this.backPanel.CustomBorderColor = System.Drawing.Color.Black;
            this.backPanel.FillColor = System.Drawing.Color.AntiqueWhite;
            this.backPanel.FillColor2 = System.Drawing.Color.Beige;
            this.backPanel.FillColor3 = System.Drawing.Color.AntiqueWhite;
            this.backPanel.FillColor4 = System.Drawing.Color.Tan;
            this.backPanel.Location = new System.Drawing.Point(2, 10);
            this.backPanel.Name = "backPanel";
            this.backPanel.Size = new System.Drawing.Size(640, 325);
            this.backPanel.TabIndex = 8;
            this.backPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.backPanel_Paint);
            // 
            // enterButton
            // 
            this.enterButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.enterButton.Animated = true;
            this.enterButton.BackColor = System.Drawing.Color.Transparent;
            this.enterButton.BorderColor = System.Drawing.Color.Olive;
            this.enterButton.BorderThickness = 1;
            this.enterButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.enterButton.DisabledState.BorderColor = System.Drawing.Color.Black;
            this.enterButton.DisabledState.CustomBorderColor = System.Drawing.Color.Transparent;
            this.enterButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.enterButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.enterButton.FillColor = System.Drawing.Color.Beige;
            this.enterButton.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.enterButton.ForeColor = System.Drawing.Color.Black;
            this.enterButton.HoverState.FillColor = System.Drawing.Color.PeachPuff;
            this.enterButton.Location = new System.Drawing.Point(13, 263);
            this.enterButton.Name = "enterButton";
            this.enterButton.Size = new System.Drawing.Size(163, 42);
            this.enterButton.TabIndex = 23;
            this.enterButton.Text = "Подтвердить";
            this.enterButton.Click += new System.EventHandler(this.enterButton_Click);
            // 
            // addGroupBox
            // 
            this.addGroupBox.BorderColor = System.Drawing.Color.Tan;
            this.addGroupBox.Controls.Add(this.descRichTextBox);
            this.addGroupBox.Controls.Add(this.nameTextBox);
            this.addGroupBox.Controls.Add(this.label2);
            this.addGroupBox.Controls.Add(this.label1);
            this.addGroupBox.CustomBorderColor = System.Drawing.Color.Beige;
            this.addGroupBox.FillColor = System.Drawing.Color.WhiteSmoke;
            this.addGroupBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.addGroupBox.ForeColor = System.Drawing.Color.Black;
            this.addGroupBox.Location = new System.Drawing.Point(23, 27);
            this.addGroupBox.Name = "addGroupBox";
            this.addGroupBox.Size = new System.Drawing.Size(596, 218);
            this.addGroupBox.TabIndex = 22;
            this.addGroupBox.Text = "Добавление новой записи";
            this.addGroupBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // descRichTextBox
            // 
            this.descRichTextBox.Location = new System.Drawing.Point(111, 128);
            this.descRichTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.descRichTextBox.Name = "descRichTextBox";
            this.descRichTextBox.Size = new System.Drawing.Size(465, 77);
            this.descRichTextBox.TabIndex = 72;
            this.descRichTextBox.Text = "";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.nameTextBox.DefaultText = "";
            this.nameTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.nameTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.nameTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.nameTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.nameTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.nameTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nameTextBox.ForeColor = System.Drawing.Color.Black;
            this.nameTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.nameTextBox.Location = new System.Drawing.Point(115, 86);
            this.nameTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.PasswordChar = '\0';
            this.nameTextBox.PlaceholderText = "";
            this.nameTextBox.SelectedText = "";
            this.nameTextBox.Size = new System.Drawing.Size(279, 27);
            this.nameTextBox.TabIndex = 38;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 20);
            this.label2.TabIndex = 47;
            this.label2.Text = "Описание";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 20);
            this.label1.TabIndex = 46;
            this.label1.Text = "Название";
            // 
            // backButton
            // 
            this.backButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.backButton.Animated = true;
            this.backButton.BackColor = System.Drawing.Color.Transparent;
            this.backButton.BorderColor = System.Drawing.Color.Olive;
            this.backButton.BorderThickness = 1;
            this.backButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.backButton.DisabledState.BorderColor = System.Drawing.Color.Black;
            this.backButton.DisabledState.CustomBorderColor = System.Drawing.Color.Transparent;
            this.backButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.backButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.backButton.FillColor = System.Drawing.Color.Beige;
            this.backButton.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.backButton.ForeColor = System.Drawing.Color.Black;
            this.backButton.HoverState.FillColor = System.Drawing.Color.PeachPuff;
            this.backButton.Location = new System.Drawing.Point(482, 263);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(137, 42);
            this.backButton.TabIndex = 6;
            this.backButton.Text = "Назад";
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // guna2ControlBox3
            // 
            this.guna2ControlBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox3.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            this.guna2ControlBox3.FillColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.guna2ControlBox3.IconColor = System.Drawing.Color.Black;
            this.guna2ControlBox3.Location = new System.Drawing.Point(529, 3);
            this.guna2ControlBox3.Name = "guna2ControlBox3";
            this.guna2ControlBox3.Size = new System.Drawing.Size(35, 29);
            this.guna2ControlBox3.TabIndex = 4;
            // 
            // guna2ControlBox2
            // 
            this.guna2ControlBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox2.BackColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox2.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MaximizeBox;
            this.guna2ControlBox2.FillColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox2.IconColor = System.Drawing.Color.Black;
            this.guna2ControlBox2.Location = new System.Drawing.Point(569, 3);
            this.guna2ControlBox2.Name = "guna2ControlBox2";
            this.guna2ControlBox2.Size = new System.Drawing.Size(30, 29);
            this.guna2ControlBox2.TabIndex = 3;
            // 
            // guna2ControlBox1
            // 
            this.guna2ControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox1.FillColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox1.IconColor = System.Drawing.Color.Black;
            this.guna2ControlBox1.Location = new System.Drawing.Point(605, 3);
            this.guna2ControlBox1.Name = "guna2ControlBox1";
            this.guna2ControlBox1.Size = new System.Drawing.Size(30, 29);
            this.guna2ControlBox1.TabIndex = 2;
            // 
            // add_up_spec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 344);
            this.Controls.Add(this.backPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "add_up_spec";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "add_up_spec";
            this.Load += new System.EventHandler(this.add_up_spec_Load);
            this.backPanel.ResumeLayout(false);
            this.addGroupBox.ResumeLayout(false);
            this.addGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private Guna.UI2.WinForms.Guna2MessageDialog guna2MessageDialog1;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel backPanel;
        private Guna.UI2.WinForms.Guna2Button enterButton;
        private Guna.UI2.WinForms.Guna2GroupBox addGroupBox;
        private System.Windows.Forms.RichTextBox descRichTextBox;
        private Guna.UI2.WinForms.Guna2TextBox nameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Button backButton;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox3;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox2;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox1;
    }
}