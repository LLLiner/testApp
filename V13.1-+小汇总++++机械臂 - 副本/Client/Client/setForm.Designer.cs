namespace Client
{
    partial class setForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(setForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.cardB1 = new System.Windows.Forms.Button();
            this.imglistMS = new System.Windows.Forms.ImageList(this.components);
            this.cardB2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.slotTxtBox = new System.Windows.Forms.TextBox();
            this.gateTxtBox = new System.Windows.Forms.TextBox();
            this.setBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(163, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "门控列表：";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(163, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "时间槽：";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.cardB2, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.cardB1, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(290, 219);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(304, 57);
            this.tableLayoutPanel3.TabIndex = 8;
            // 
            // cardB1
            // 
            this.cardB1.BackColor = System.Drawing.SystemColors.Control;
            this.cardB1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cardB1.ImageIndex = 0;
            this.cardB1.ImageList = this.imglistMS;
            this.cardB1.Location = new System.Drawing.Point(4, 4);
            this.cardB1.Margin = new System.Windows.Forms.Padding(4);
            this.cardB1.Name = "cardB1";
            this.cardB1.Size = new System.Drawing.Size(133, 48);
            this.cardB1.TabIndex = 44;
            this.cardB1.Tag = "1";
            this.cardB1.Text = "板1";
            this.cardB1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cardB1.UseVisualStyleBackColor = false;
            this.cardB1.Click += new System.EventHandler(this.select_Click);
            // 
            // imglistMS
            // 
            this.imglistMS.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglistMS.ImageStream")));
            this.imglistMS.TransparentColor = System.Drawing.Color.Transparent;
            this.imglistMS.Images.SetKeyName(0, "slave.png");
            this.imglistMS.Images.SetKeyName(1, "master.png");
            // 
            // cardB2
            // 
            this.cardB2.BackColor = System.Drawing.SystemColors.Control;
            this.cardB2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cardB2.ImageIndex = 0;
            this.cardB2.ImageList = this.imglistMS;
            this.cardB2.Location = new System.Drawing.Point(156, 4);
            this.cardB2.Margin = new System.Windows.Forms.Padding(4);
            this.cardB2.Name = "cardB2";
            this.cardB2.Size = new System.Drawing.Size(144, 48);
            this.cardB2.TabIndex = 45;
            this.cardB2.Tag = "2";
            this.cardB2.Text = "板2";
            this.cardB2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cardB2.UseVisualStyleBackColor = false;
            this.cardB2.Click += new System.EventHandler(this.select_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(163, 234);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "主从时钟：";
            // 
            // slotTxtBox
            // 
            this.slotTxtBox.Location = new System.Drawing.Point(290, 150);
            this.slotTxtBox.Name = "slotTxtBox";
            this.slotTxtBox.Size = new System.Drawing.Size(100, 25);
            this.slotTxtBox.TabIndex = 10;
            this.slotTxtBox.Text = "8";
            // 
            // gateTxtBox
            // 
            this.gateTxtBox.Location = new System.Drawing.Point(290, 64);
            this.gateTxtBox.Name = "gateTxtBox";
            this.gateTxtBox.Size = new System.Drawing.Size(300, 25);
            this.gateTxtBox.TabIndex = 11;
            this.gateTxtBox.Text = "fc,fa,f9,f8,fc,fa,f9,f8";
            // 
            // setBtn
            // 
            this.setBtn.Location = new System.Drawing.Point(235, 326);
            this.setBtn.Name = "setBtn";
            this.setBtn.Size = new System.Drawing.Size(88, 36);
            this.setBtn.TabIndex = 12;
            this.setBtn.Text = "设置";
            this.setBtn.UseVisualStyleBackColor = true;
            this.setBtn.Click += new System.EventHandler(this.setBtn_Click);
            // 
            // setForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.setBtn);
            this.Controls.Add(this.gateTxtBox);
            this.Controls.Add(this.slotTxtBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "setForm";
            this.Text = "setForm";
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        public System.Windows.Forms.Button cardB2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ImageList imglistMS;
        public System.Windows.Forms.Button cardB1;
        private System.Windows.Forms.TextBox slotTxtBox;
        private System.Windows.Forms.TextBox gateTxtBox;
        private System.Windows.Forms.Button setBtn;
    }
}