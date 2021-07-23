namespace Client
{
    partial class Frmmain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frmmain));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnReadDO = new System.Windows.Forms.Button();
            this.txtaddr = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDINum = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtDONum = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ip1 = new IPAddressControlLib.IPAddressControl();
            this.txtport = new System.Windows.Forms.TextBox();
            this.btnopen = new System.Windows.Forms.Button();
            this.imglistDO = new System.Windows.Forms.ImageList(this.components);
            this.imglistDI = new System.Windows.Forms.ImageList(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDO8 = new System.Windows.Forms.Button();
            this.btnDO7 = new System.Windows.Forms.Button();
            this.btnDO6 = new System.Windows.Forms.Button();
            this.btnDO5 = new System.Windows.Forms.Button();
            this.btnDO4 = new System.Windows.Forms.Button();
            this.btnDO3 = new System.Windows.Forms.Button();
            this.btnDO2 = new System.Windows.Forms.Button();
            this.btnDO1 = new System.Windows.Forms.Button();
            this.btnOpenAll = new System.Windows.Forms.Button();
            this.btnCloseAll = new System.Windows.Forms.Button();
            this.RealPlayWnd = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RealPlayWnd)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnReadDO);
            this.groupBox3.Controls.Add(this.txtaddr);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txtDINum);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.btnClear);
            this.groupBox3.Controls.Add(this.txtDONum);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.ip1);
            this.groupBox3.Controls.Add(this.txtport);
            this.groupBox3.Controls.Add(this.btnopen);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(928, 100);
            this.groupBox3.TabIndex = 56;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "设备IP及其端口";
            // 
            // btnReadDO
            // 
            this.btnReadDO.Location = new System.Drawing.Point(645, 64);
            this.btnReadDO.Margin = new System.Windows.Forms.Padding(4);
            this.btnReadDO.Name = "btnReadDO";
            this.btnReadDO.Size = new System.Drawing.Size(100, 29);
            this.btnReadDO.TabIndex = 62;
            this.btnReadDO.Text = "读取DO";
            this.btnReadDO.UseVisualStyleBackColor = true;
            this.btnReadDO.Click += new System.EventHandler(this.btnReadDO_Click);
            // 
            // txtaddr
            // 
            this.txtaddr.Location = new System.Drawing.Point(95, 66);
            this.txtaddr.Margin = new System.Windows.Forms.Padding(4);
            this.txtaddr.Name = "txtaddr";
            this.txtaddr.Size = new System.Drawing.Size(87, 25);
            this.txtaddr.TabIndex = 58;
            this.txtaddr.Text = "254";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 71);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 57;
            this.label5.Text = "设备地址";
            // 
            // txtDINum
            // 
            this.txtDINum.Location = new System.Drawing.Point(464, 66);
            this.txtDINum.Margin = new System.Windows.Forms.Padding(4);
            this.txtDINum.Name = "txtDINum";
            this.txtDINum.Size = new System.Drawing.Size(87, 25);
            this.txtDINum.TabIndex = 56;
            this.txtDINum.Text = "20";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(393, 71);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 15);
            this.label4.TabIndex = 55;
            this.label4.Text = "DI数量";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(773, 22);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 29);
            this.btnClear.TabIndex = 54;
            this.btnClear.Text = "清空接收";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // txtDONum
            // 
            this.txtDONum.Location = new System.Drawing.Point(289, 66);
            this.txtDONum.Margin = new System.Windows.Forms.Padding(4);
            this.txtDONum.Name = "txtDONum";
            this.txtDONum.Size = new System.Drawing.Size(87, 25);
            this.txtDONum.TabIndex = 53;
            this.txtDONum.Text = "8";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(219, 71);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 15);
            this.label3.TabIndex = 52;
            this.label3.Text = "DO数量";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(293, 29);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 50;
            this.label2.Text = "端口号：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 49;
            this.label1.Text = "IP:";
            // 
            // ip1
            // 
            this.ip1.AllowInternalTab = false;
            this.ip1.AutoHeight = true;
            this.ip1.BackColor = System.Drawing.SystemColors.Window;
            this.ip1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ip1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ip1.Location = new System.Drawing.Point(79, 25);
            this.ip1.Margin = new System.Windows.Forms.Padding(4);
            this.ip1.MinimumSize = new System.Drawing.Size(126, 25);
            this.ip1.Name = "ip1";
            this.ip1.ReadOnly = false;
            this.ip1.Size = new System.Drawing.Size(172, 25);
            this.ip1.TabIndex = 48;
            this.ip1.Text = "127.0.0.1";
            this.ip1.Click += new System.EventHandler(this.ip1_Click);
            // 
            // txtport
            // 
            this.txtport.Location = new System.Drawing.Point(372, 25);
            this.txtport.Margin = new System.Windows.Forms.Padding(4);
            this.txtport.Name = "txtport";
            this.txtport.Size = new System.Drawing.Size(87, 25);
            this.txtport.TabIndex = 35;
            this.txtport.Text = "10000";
            // 
            // btnopen
            // 
            this.btnopen.Location = new System.Drawing.Point(645, 22);
            this.btnopen.Margin = new System.Windows.Forms.Padding(4);
            this.btnopen.Name = "btnopen";
            this.btnopen.Size = new System.Drawing.Size(100, 29);
            this.btnopen.TabIndex = 36;
            this.btnopen.Text = "打开连接";
            this.btnopen.UseVisualStyleBackColor = true;
            this.btnopen.Click += new System.EventHandler(this.btnopen_Click);
            // 
            // imglistDO
            // 
            this.imglistDO.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglistDO.ImageStream")));
            this.imglistDO.TransparentColor = System.Drawing.Color.Transparent;
            this.imglistDO.Images.SetKeyName(0, "comclosed.ico");
            this.imglistDO.Images.SetKeyName(1, "comopened.ico");
            this.imglistDO.Images.SetKeyName(2, "24_Help.ico");
            // 
            // imglistDI
            // 
            this.imglistDI.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglistDI.ImageStream")));
            this.imglistDI.TransparentColor = System.Drawing.Color.Transparent;
            this.imglistDI.Images.SetKeyName(0, "comclosed.ico");
            this.imglistDI.Images.SetKeyName(1, "comopened.ico");
            this.imglistDI.Images.SetKeyName(2, "24_Help.ico");
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDO8);
            this.groupBox2.Controls.Add(this.btnDO7);
            this.groupBox2.Controls.Add(this.btnDO6);
            this.groupBox2.Controls.Add(this.btnDO5);
            this.groupBox2.Controls.Add(this.btnDO4);
            this.groupBox2.Controls.Add(this.btnDO3);
            this.groupBox2.Controls.Add(this.btnDO2);
            this.groupBox2.Controls.Add(this.btnDO1);
            this.groupBox2.Controls.Add(this.btnOpenAll);
            this.groupBox2.Controls.Add(this.btnCloseAll);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 100);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(928, 132);
            this.groupBox2.TabIndex = 57;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DO输出";
            // 
            // btnDO8
            // 
            this.btnDO8.BackColor = System.Drawing.SystemColors.Control;
            this.btnDO8.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDO8.ImageIndex = 0;
            this.btnDO8.ImageList = this.imglistDO;
            this.btnDO8.Location = new System.Drawing.Point(376, 84);
            this.btnDO8.Margin = new System.Windows.Forms.Padding(4);
            this.btnDO8.Name = "btnDO8";
            this.btnDO8.Size = new System.Drawing.Size(107, 40);
            this.btnDO8.TabIndex = 68;
            this.btnDO8.Tag = "8";
            this.btnDO8.Text = "  DO8";
            this.btnDO8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDO8.UseVisualStyleBackColor = false;
            this.btnDO8.Click += new System.EventHandler(this.btnDO_Click);
            // 
            // btnDO7
            // 
            this.btnDO7.BackColor = System.Drawing.SystemColors.Control;
            this.btnDO7.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDO7.ImageIndex = 0;
            this.btnDO7.ImageList = this.imglistDO;
            this.btnDO7.Location = new System.Drawing.Point(253, 84);
            this.btnDO7.Margin = new System.Windows.Forms.Padding(4);
            this.btnDO7.Name = "btnDO7";
            this.btnDO7.Size = new System.Drawing.Size(107, 40);
            this.btnDO7.TabIndex = 46;
            this.btnDO7.Tag = "7";
            this.btnDO7.Text = "  DO7";
            this.btnDO7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDO7.UseVisualStyleBackColor = false;
            this.btnDO7.Click += new System.EventHandler(this.btnDO_Click);
            // 
            // btnDO6
            // 
            this.btnDO6.BackColor = System.Drawing.SystemColors.Control;
            this.btnDO6.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDO6.ImageIndex = 0;
            this.btnDO6.ImageList = this.imglistDO;
            this.btnDO6.Location = new System.Drawing.Point(133, 84);
            this.btnDO6.Margin = new System.Windows.Forms.Padding(4);
            this.btnDO6.Name = "btnDO6";
            this.btnDO6.Size = new System.Drawing.Size(107, 40);
            this.btnDO6.TabIndex = 45;
            this.btnDO6.Tag = "6";
            this.btnDO6.Text = "  DO6";
            this.btnDO6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDO6.UseVisualStyleBackColor = false;
            this.btnDO6.Click += new System.EventHandler(this.btnDO_Click);
            // 
            // btnDO5
            // 
            this.btnDO5.BackColor = System.Drawing.SystemColors.Control;
            this.btnDO5.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDO5.ImageIndex = 0;
            this.btnDO5.ImageList = this.imglistDO;
            this.btnDO5.Location = new System.Drawing.Point(16, 84);
            this.btnDO5.Margin = new System.Windows.Forms.Padding(4);
            this.btnDO5.Name = "btnDO5";
            this.btnDO5.Size = new System.Drawing.Size(107, 40);
            this.btnDO5.TabIndex = 44;
            this.btnDO5.Tag = "5";
            this.btnDO5.Text = "  DO5";
            this.btnDO5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDO5.UseVisualStyleBackColor = false;
            this.btnDO5.Click += new System.EventHandler(this.btnDO_Click);
            // 
            // btnDO4
            // 
            this.btnDO4.BackColor = System.Drawing.SystemColors.Control;
            this.btnDO4.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDO4.ImageIndex = 0;
            this.btnDO4.ImageList = this.imglistDO;
            this.btnDO4.Location = new System.Drawing.Point(376, 25);
            this.btnDO4.Margin = new System.Windows.Forms.Padding(4);
            this.btnDO4.Name = "btnDO4";
            this.btnDO4.Size = new System.Drawing.Size(107, 40);
            this.btnDO4.TabIndex = 43;
            this.btnDO4.Tag = "4";
            this.btnDO4.Text = "  DO4";
            this.btnDO4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDO4.UseVisualStyleBackColor = false;
            this.btnDO4.Click += new System.EventHandler(this.btnDO_Click);
            // 
            // btnDO3
            // 
            this.btnDO3.BackColor = System.Drawing.SystemColors.Control;
            this.btnDO3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDO3.ImageIndex = 0;
            this.btnDO3.ImageList = this.imglistDO;
            this.btnDO3.Location = new System.Drawing.Point(253, 25);
            this.btnDO3.Margin = new System.Windows.Forms.Padding(4);
            this.btnDO3.Name = "btnDO3";
            this.btnDO3.Size = new System.Drawing.Size(107, 40);
            this.btnDO3.TabIndex = 42;
            this.btnDO3.Tag = "3";
            this.btnDO3.Text = "  DO3";
            this.btnDO3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDO3.UseVisualStyleBackColor = false;
            this.btnDO3.Click += new System.EventHandler(this.btnDO_Click);
            // 
            // btnDO2
            // 
            this.btnDO2.BackColor = System.Drawing.SystemColors.Control;
            this.btnDO2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDO2.ImageIndex = 0;
            this.btnDO2.ImageList = this.imglistDO;
            this.btnDO2.Location = new System.Drawing.Point(133, 25);
            this.btnDO2.Margin = new System.Windows.Forms.Padding(4);
            this.btnDO2.Name = "btnDO2";
            this.btnDO2.Size = new System.Drawing.Size(107, 40);
            this.btnDO2.TabIndex = 41;
            this.btnDO2.Tag = "2";
            this.btnDO2.Text = "  DO2";
            this.btnDO2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDO2.UseVisualStyleBackColor = false;
            this.btnDO2.Click += new System.EventHandler(this.btnDO_Click);
            // 
            // btnDO1
            // 
            this.btnDO1.BackColor = System.Drawing.SystemColors.Control;
            this.btnDO1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDO1.ImageIndex = 0;
            this.btnDO1.ImageList = this.imglistDO;
            this.btnDO1.Location = new System.Drawing.Point(16, 25);
            this.btnDO1.Margin = new System.Windows.Forms.Padding(4);
            this.btnDO1.Name = "btnDO1";
            this.btnDO1.Size = new System.Drawing.Size(107, 40);
            this.btnDO1.TabIndex = 40;
            this.btnDO1.Tag = "1";
            this.btnDO1.Text = "  DO1";
            this.btnDO1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDO1.UseVisualStyleBackColor = false;
            this.btnDO1.Click += new System.EventHandler(this.btnDO_Click);
            // 
            // btnOpenAll
            // 
            this.btnOpenAll.BackColor = System.Drawing.SystemColors.Control;
            this.btnOpenAll.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOpenAll.ImageKey = "comopened.ico";
            this.btnOpenAll.ImageList = this.imglistDO;
            this.btnOpenAll.Location = new System.Drawing.Point(696, 46);
            this.btnOpenAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenAll.Name = "btnOpenAll";
            this.btnOpenAll.Size = new System.Drawing.Size(152, 56);
            this.btnOpenAll.TabIndex = 39;
            this.btnOpenAll.Tag = "1";
            this.btnOpenAll.Text = "  打开全部";
            this.btnOpenAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenAll.UseVisualStyleBackColor = false;
            this.btnOpenAll.Click += new System.EventHandler(this.btnOpenAll_Click);
            // 
            // btnCloseAll
            // 
            this.btnCloseAll.BackColor = System.Drawing.SystemColors.Control;
            this.btnCloseAll.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCloseAll.ImageKey = "comclosed.ico";
            this.btnCloseAll.ImageList = this.imglistDO;
            this.btnCloseAll.Location = new System.Drawing.Point(508, 46);
            this.btnCloseAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnCloseAll.Name = "btnCloseAll";
            this.btnCloseAll.Size = new System.Drawing.Size(152, 56);
            this.btnCloseAll.TabIndex = 38;
            this.btnCloseAll.Tag = "1";
            this.btnCloseAll.Text = "  关闭全部";
            this.btnCloseAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCloseAll.UseVisualStyleBackColor = false;
            this.btnCloseAll.Click += new System.EventHandler(this.btnCloseAll_Click);
            // 
            // RealPlayWnd
            // 
            this.RealPlayWnd.BackColor = System.Drawing.SystemColors.WindowText;
            this.RealPlayWnd.Location = new System.Drawing.Point(257, 239);
            this.RealPlayWnd.Name = "RealPlayWnd";
            this.RealPlayWnd.Size = new System.Drawing.Size(660, 486);
            this.RealPlayWnd.TabIndex = 58;
            this.RealPlayWnd.TabStop = false;
            this.RealPlayWnd.Click += new System.EventHandler(this.RealPlayWnd_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 359);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 65;
            this.label6.Text = "设备端口";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 297);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 15);
            this.label9.TabIndex = 64;
            this.label9.Text = "ICP的IP";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(99, 354);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(141, 25);
            this.textBoxPort.TabIndex = 61;
            this.textBoxPort.Text = "8000";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(99, 286);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(141, 25);
            this.textBoxIP.TabIndex = 60;
            this.textBoxIP.Text = "192.168.1.64";
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(102, 489);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(101, 44);
            this.btnPreview.TabIndex = 66;
            this.btnPreview.Text = "Live View";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(95, 403);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(104, 64);
            this.btnLogin.TabIndex = 67;
            this.btnLogin.Text = "Login";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // Frmmain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(928, 814);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.RealPlayWnd);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Frmmain";
            this.Text = "TCP/IP 客户端调试";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Frmmain_FormClosed);
            this.Load += new System.EventHandler(this.Frmmain_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RealPlayWnd)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public IPAddressControlLib.IPAddressControl ip1;
        public System.Windows.Forms.TextBox txtport;
        private System.Windows.Forms.Button btnopen;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox txtDONum;
        private System.Windows.Forms.ImageList imglistDO;
        private System.Windows.Forms.ImageList imglistDI;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnDO8;
        private System.Windows.Forms.Button btnDO7;
        private System.Windows.Forms.Button btnDO6;
        private System.Windows.Forms.Button btnDO5;
        private System.Windows.Forms.Button btnDO4;
        private System.Windows.Forms.Button btnDO3;
        private System.Windows.Forms.Button btnDO2;
        private System.Windows.Forms.Button btnDO1;
        private System.Windows.Forms.Button btnOpenAll;
        private System.Windows.Forms.Button btnCloseAll;
        public System.Windows.Forms.TextBox txtDINum;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox txtaddr;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnReadDO;
        private System.Windows.Forms.PictureBox RealPlayWnd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnLogin;
    }
}

