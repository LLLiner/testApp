namespace Client
{
    partial class MainWindows
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindows));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnopen = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnDO4 = new System.Windows.Forms.Button();
            this.imglistDO = new System.Windows.Forms.ImageList(this.components);
            this.btnDO1 = new System.Windows.Forms.Button();
            this.btnDO2 = new System.Windows.Forms.Button();
            this.btnDO3 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnPreview = new System.Windows.Forms.Button();
            this.RealPlayWnd = new System.Windows.Forms.PictureBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RealPlayWnd)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(465, 457);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "网络系统架构";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Client.Properties.Resources.structure4;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(6, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(453, 427);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click_1);
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(483, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(791, 457);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "网络时延分析";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnopen);
            this.groupBox5.Controls.Add(this.tableLayoutPanel1);
            this.groupBox5.Location = new System.Drawing.Point(16, 38);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(235, 288);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "灯";
            // 
            // btnopen
            // 
            this.btnopen.Location = new System.Drawing.Point(15, 34);
            this.btnopen.Margin = new System.Windows.Forms.Padding(4);
            this.btnopen.Name = "btnopen";
            this.btnopen.Size = new System.Drawing.Size(100, 29);
            this.btnopen.TabIndex = 37;
            this.btnopen.Text = "打开连接";
            this.btnopen.UseVisualStyleBackColor = true;
            this.btnopen.Click += new System.EventHandler(this.btnopen_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnDO4, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnDO1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDO2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDO3, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(15, 92);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnDO4
            // 
            this.btnDO4.BackColor = System.Drawing.SystemColors.Control;
            this.btnDO4.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDO4.ImageIndex = 0;
            this.btnDO4.ImageList = this.imglistDO;
            this.btnDO4.Location = new System.Drawing.Point(104, 54);
            this.btnDO4.Margin = new System.Windows.Forms.Padding(4);
            this.btnDO4.Name = "btnDO4";
            this.btnDO4.Size = new System.Drawing.Size(92, 40);
            this.btnDO4.TabIndex = 47;
            this.btnDO4.Tag = "4";
            this.btnDO4.Text = "  DO4";
            this.btnDO4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDO4.UseVisualStyleBackColor = false;
            this.btnDO4.Click += new System.EventHandler(this.btnDO_Click);
            // 
            // imglistDO
            // 
            this.imglistDO.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglistDO.ImageStream")));
            this.imglistDO.TransparentColor = System.Drawing.Color.Transparent;
            this.imglistDO.Images.SetKeyName(0, "comclosed.ico");
            this.imglistDO.Images.SetKeyName(1, "comopened.ico");
            this.imglistDO.Images.SetKeyName(2, "24_Help.ico");
            // 
            // btnDO1
            // 
            this.btnDO1.BackColor = System.Drawing.SystemColors.Control;
            this.btnDO1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDO1.ImageIndex = 0;
            this.btnDO1.ImageList = this.imglistDO;
            this.btnDO1.Location = new System.Drawing.Point(4, 4);
            this.btnDO1.Margin = new System.Windows.Forms.Padding(4);
            this.btnDO1.Name = "btnDO1";
            this.btnDO1.Size = new System.Drawing.Size(92, 40);
            this.btnDO1.TabIndex = 44;
            this.btnDO1.Tag = "1";
            this.btnDO1.Text = "  DO1";
            this.btnDO1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDO1.UseVisualStyleBackColor = false;
            this.btnDO1.Click += new System.EventHandler(this.btnDO_Click);
            // 
            // btnDO2
            // 
            this.btnDO2.BackColor = System.Drawing.SystemColors.Control;
            this.btnDO2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDO2.ImageIndex = 0;
            this.btnDO2.ImageList = this.imglistDO;
            this.btnDO2.Location = new System.Drawing.Point(104, 4);
            this.btnDO2.Margin = new System.Windows.Forms.Padding(4);
            this.btnDO2.Name = "btnDO2";
            this.btnDO2.Size = new System.Drawing.Size(92, 40);
            this.btnDO2.TabIndex = 45;
            this.btnDO2.Tag = "2";
            this.btnDO2.Text = "  DO2";
            this.btnDO2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDO2.UseVisualStyleBackColor = false;
            this.btnDO2.Click += new System.EventHandler(this.btnDO_Click);
            // 
            // btnDO3
            // 
            this.btnDO3.BackColor = System.Drawing.SystemColors.Control;
            this.btnDO3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDO3.ImageIndex = 0;
            this.btnDO3.ImageList = this.imglistDO;
            this.btnDO3.Location = new System.Drawing.Point(4, 54);
            this.btnDO3.Margin = new System.Windows.Forms.Padding(4);
            this.btnDO3.Name = "btnDO3";
            this.btnDO3.Size = new System.Drawing.Size(92, 40);
            this.btnDO3.TabIndex = 46;
            this.btnDO3.Tag = "3";
            this.btnDO3.Text = "  DO3";
            this.btnDO3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDO3.UseVisualStyleBackColor = false;
            this.btnDO3.Click += new System.EventHandler(this.btnDO_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnPreview);
            this.groupBox3.Controls.Add(this.RealPlayWnd);
            this.groupBox3.Location = new System.Drawing.Point(18, 475);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(453, 459);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "非TSN设备";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(148, 429);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(119, 30);
            this.btnPreview.TabIndex = 74;
            this.btnPreview.Text = "Live View";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // RealPlayWnd
            // 
            this.RealPlayWnd.BackColor = System.Drawing.SystemColors.WindowText;
            this.RealPlayWnd.Location = new System.Drawing.Point(0, 24);
            this.RealPlayWnd.Name = "RealPlayWnd";
            this.RealPlayWnd.Size = new System.Drawing.Size(441, 399);
            this.RealPlayWnd.TabIndex = 69;
            this.RealPlayWnd.TabStop = false;
            this.RealPlayWnd.Click += new System.EventHandler(this.RealPlayWnd_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Controls.Add(this.groupBox7);
            this.groupBox4.Controls.Add(this.groupBox6);
            this.groupBox4.Location = new System.Drawing.Point(483, 475);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(791, 459);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "TSN设备";
            this.groupBox4.Enter += new System.EventHandler(this.groupBox4_Enter);
            // 
            // groupBox7
            // 
            this.groupBox7.Location = new System.Drawing.Point(534, 38);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(200, 288);
            this.groupBox7.TabIndex = 3;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "6Lowpan";
            // 
            // groupBox6
            // 
            this.groupBox6.Location = new System.Drawing.Point(293, 38);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(200, 288);
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "马达";
            // 
            // MainWindows
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1303, 940);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainWindows";
            this.Text = "MainWindows";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RealPlayWnd)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.PictureBox RealPlayWnd;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnDO4;
        private System.Windows.Forms.Button btnDO1;
        private System.Windows.Forms.Button btnDO2;
        private System.Windows.Forms.Button btnDO3;
        private System.Windows.Forms.ImageList imglistDO;
        private System.Windows.Forms.Button btnopen;
    }
}