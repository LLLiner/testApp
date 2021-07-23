using System;
using System.Windows.Forms;

namespace Client
{
    partial class ICPSetting
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
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.RealPlayWnd = new System.Windows.Forms.PictureBox();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.RealPlayWnd)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(102, 237);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(104, 64);
            this.btnLogin.TabIndex = 74;
            this.btnLogin.Text = "Login";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(103, 333);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(101, 44);
            this.btnPreview.TabIndex = 73;
            this.btnPreview.Text = "Live View";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click_1);
            // 
            // RealPlayWnd
            // 
            this.RealPlayWnd.BackColor = System.Drawing.SystemColors.WindowText;
            this.RealPlayWnd.Location = new System.Drawing.Point(348, 12);
            this.RealPlayWnd.Name = "RealPlayWnd";
            this.RealPlayWnd.Size = new System.Drawing.Size(441, 399);
            this.RealPlayWnd.TabIndex = 75;
            this.RealPlayWnd.TabStop = false;
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(102, 166);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(141, 25);
            this.textBoxPort.TabIndex = 70;
            this.textBoxPort.Text = "8000";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 171);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 72;
            this.label6.Text = "设备端口";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(102, 116);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(141, 25);
            this.textBoxIP.TabIndex = 69;
            this.textBoxIP.Text = "192.168.1.64";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 127);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 15);
            this.label9.TabIndex = 71;
            this.label9.Text = "ICP的IP";
            // 
            // ICPSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 428);
            this.Controls.Add(this.RealPlayWnd);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.textBoxIP);
            this.Name = "ICPSetting";
            this.Text = "ICPWindow";
            ((System.ComponentModel.ISupportInitialize)(this.RealPlayWnd)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void btnPreview_Click_1(object sender, EventArgs e)
        {
            //实时画面
            if (m_lUserID < 0)
            {
                MessageBox.Show("Please login the device firstly");
                return;
            }

            if (m_lRealHandle < 0)
            {
                CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
                lpPreviewInfo.hPlayWnd = RealPlayWnd.Handle;//预览窗口
                //lpPreviewInfo.lChannel = Int16.Parse(textBoxChannel.Text);//预te览的设备通道
                lpPreviewInfo.lChannel = Int16.Parse("1");//预te览的设备通道
                lpPreviewInfo.dwStreamType = 0;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
                lpPreviewInfo.dwDisplayBufNum = 1; //播放库播放缓冲区最大缓冲帧数
                lpPreviewInfo.byProtoType = 0;
                lpPreviewInfo.byPreviewMode = 0;

                /*if (textBoxID.Text != "")
                {
                    lpPreviewInfo.lChannel = -1;
                    byte[] byStreamID = System.Text.Encoding.Default.GetBytes(textBoxID.Text);
                    lpPreviewInfo.byStreamID = new byte[32];
                    byStreamID.CopyTo(lpPreviewInfo.byStreamID, 0);
                }*/


                if (RealData == null)
                {
                    RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数
                }

                IntPtr pUser = new IntPtr();//用户数据

                //打开预览 Start live view 
                m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null/*RealData*/, pUser);
                if (m_lRealHandle < 0)
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_RealPlay_V40 failed, error code= " + iLastErr; //预览失败，输出错误号
                    MessageBox.Show(str);
                    return;
                }
                else
                {
                    //预览成功
                    btnPreview.Text = "Stop Live View";
                }
            }
            else
            {
                //停止预览 Stop live view 
                if (!CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_StopRealPlay failed, error code= " + iLastErr;
                    MessageBox.Show(str);
                    return;
                }
                m_lRealHandle = -1;
                btnPreview.Text = "Live View";

            }
            return;
        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.PictureBox RealPlayWnd;
        private TextBox textBoxPort;
        private Label label6;
        private TextBox textBoxIP;
        private Label label9;
    }
}