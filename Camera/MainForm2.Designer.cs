namespace Camera
{
    partial class MainForm2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm2));
            this.Image_Xe_Vao_Truoc = new System.Windows.Forms.PictureBox();
            this.Image_Xe_Vao_Sau = new System.Windows.Forms.PictureBox();
            this.Image_Xe_Ra_Truoc = new System.Windows.Forms.PictureBox();
            this.Image_Xe_Ra_Sau = new System.Windows.Forms.PictureBox();
            this.lbStartTime = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPlate = new System.Windows.Forms.Button();
            this.btnFace = new System.Windows.Forms.Button();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.btnStart = new System.Windows.Forms.Button();
            this.lbPlate = new MetroFramework.Controls.MetroLabel();
            this.Image_Plate = new System.Windows.Forms.PictureBox();
            this.Image_ID = new System.Windows.Forms.PictureBox();
            this.lbID = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.timerFaceIn = new System.Windows.Forms.Timer(this.components);
            this.pVao = new System.Windows.Forms.Panel();
            this.pInfo = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbNotify = new MetroFramework.Controls.MetroLabel();
            this.btnAllow = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new MetroFramework.Controls.MetroButton();
            this.pRa = new System.Windows.Forms.Panel();
            this.pMenu = new System.Windows.Forms.Panel();
            this.btnHistory = new MetroFramework.Controls.MetroButton();
            this.btnMi = new System.Windows.Forms.Button();
            this.btnMax = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.timerPlateIn = new System.Windows.Forms.Timer(this.components);
            this.timerFaceOut = new System.Windows.Forms.Timer(this.components);
            this.timerPlateOut = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Image_Xe_Vao_Truoc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Xe_Vao_Sau)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Xe_Ra_Truoc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Xe_Ra_Sau)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Plate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_ID)).BeginInit();
            this.pVao.SuspendLayout();
            this.pInfo.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pRa.SuspendLayout();
            this.pMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // Image_Xe_Vao_Truoc
            // 
            this.Image_Xe_Vao_Truoc.Location = new System.Drawing.Point(14, 43);
            this.Image_Xe_Vao_Truoc.Name = "Image_Xe_Vao_Truoc";
            this.Image_Xe_Vao_Truoc.Size = new System.Drawing.Size(381, 227);
            this.Image_Xe_Vao_Truoc.TabIndex = 0;
            this.Image_Xe_Vao_Truoc.TabStop = false;
            // 
            // Image_Xe_Vao_Sau
            // 
            this.Image_Xe_Vao_Sau.Location = new System.Drawing.Point(14, 298);
            this.Image_Xe_Vao_Sau.Name = "Image_Xe_Vao_Sau";
            this.Image_Xe_Vao_Sau.Size = new System.Drawing.Size(381, 297);
            this.Image_Xe_Vao_Sau.TabIndex = 1;
            this.Image_Xe_Vao_Sau.TabStop = false;
            // 
            // Image_Xe_Ra_Truoc
            // 
            this.Image_Xe_Ra_Truoc.Image = ((System.Drawing.Image)(resources.GetObject("Image_Xe_Ra_Truoc.Image")));
            this.Image_Xe_Ra_Truoc.Location = new System.Drawing.Point(17, 40);
            this.Image_Xe_Ra_Truoc.Name = "Image_Xe_Ra_Truoc";
            this.Image_Xe_Ra_Truoc.Size = new System.Drawing.Size(367, 227);
            this.Image_Xe_Ra_Truoc.TabIndex = 2;
            this.Image_Xe_Ra_Truoc.TabStop = false;
            // 
            // Image_Xe_Ra_Sau
            // 
            this.Image_Xe_Ra_Sau.Image = ((System.Drawing.Image)(resources.GetObject("Image_Xe_Ra_Sau.Image")));
            this.Image_Xe_Ra_Sau.Location = new System.Drawing.Point(17, 295);
            this.Image_Xe_Ra_Sau.Name = "Image_Xe_Ra_Sau";
            this.Image_Xe_Ra_Sau.Size = new System.Drawing.Size(367, 297);
            this.Image_Xe_Ra_Sau.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Image_Xe_Ra_Sau.TabIndex = 3;
            this.Image_Xe_Ra_Sau.TabStop = false;
            // 
            // lbStartTime
            // 
            this.lbStartTime.AutoSize = true;
            this.lbStartTime.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lbStartTime.Location = new System.Drawing.Point(6, 55);
            this.lbStartTime.Name = "lbStartTime";
            this.lbStartTime.Size = new System.Drawing.Size(82, 13);
            this.lbStartTime.TabIndex = 15;
            this.lbStartTime.Text = "Thời gian gửi xe";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label2.Location = new System.Drawing.Point(6, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Hướng Xe";
            // 
            // btnPlate
            // 
            this.btnPlate.Location = new System.Drawing.Point(122, 524);
            this.btnPlate.Name = "btnPlate";
            this.btnPlate.Size = new System.Drawing.Size(75, 23);
            this.btnPlate.TabIndex = 9;
            this.btnPlate.Text = "Số Xe";
            this.btnPlate.UseVisualStyleBackColor = true;
            this.btnPlate.Click += new System.EventHandler(this.btnPlate_Click);
            // 
            // btnFace
            // 
            this.btnFace.Location = new System.Drawing.Point(9, 524);
            this.btnFace.Name = "btnFace";
            this.btnFace.Size = new System.Drawing.Size(75, 23);
            this.btnFace.TabIndex = 8;
            this.btnFace.Text = "Khuôn mặt";
            this.btnFace.UseVisualStyleBackColor = true;
            this.btnFace.Click += new System.EventHandler(this.btnFace_Click);
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.ForeColor = System.Drawing.Color.Green;
            this.metroLabel2.Location = new System.Drawing.Point(34, 269);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(54, 19);
            this.metroLabel2.TabIndex = 7;
            this.metroLabel2.Text = "Biển số:";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(9, 495);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "Nhận diện";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lbPlate
            // 
            this.lbPlate.AutoSize = true;
            this.lbPlate.BackColor = System.Drawing.Color.White;
            this.lbPlate.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lbPlate.ForeColor = System.Drawing.Color.Transparent;
            this.lbPlate.Location = new System.Drawing.Point(97, 269);
            this.lbPlate.Name = "lbPlate";
            this.lbPlate.Size = new System.Drawing.Size(38, 19);
            this.lbPlate.Style = MetroFramework.MetroColorStyle.Orange;
            this.lbPlate.TabIndex = 4;
            this.lbPlate.Text = "sdfg";
            this.lbPlate.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // Image_Plate
            // 
            this.Image_Plate.BackColor = System.Drawing.Color.Silver;
            this.Image_Plate.Location = new System.Drawing.Point(30, 181);
            this.Image_Plate.Name = "Image_Plate";
            this.Image_Plate.Size = new System.Drawing.Size(140, 78);
            this.Image_Plate.TabIndex = 3;
            this.Image_Plate.TabStop = false;
            // 
            // Image_ID
            // 
            this.Image_ID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.Image_ID.Location = new System.Drawing.Point(30, 43);
            this.Image_ID.Name = "Image_ID";
            this.Image_ID.Size = new System.Drawing.Size(140, 100);
            this.Image_ID.TabIndex = 2;
            this.Image_ID.TabStop = false;
            // 
            // lbID
            // 
            this.lbID.AutoSize = true;
            this.lbID.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lbID.ForeColor = System.Drawing.Color.DarkOrange;
            this.lbID.Location = new System.Drawing.Point(80, 154);
            this.lbID.Name = "lbID";
            this.lbID.Size = new System.Drawing.Size(51, 19);
            this.lbID.Style = MetroFramework.MetroColorStyle.Orange;
            this.lbID.TabIndex = 1;
            this.lbID.Text = "sadsfa";
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.ForeColor = System.Drawing.Color.Green;
            this.metroLabel1.Location = new System.Drawing.Point(50, 154);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(24, 19);
            this.metroLabel1.TabIndex = 0;
            this.metroLabel1.Text = "ID:";
            // 
            // timerFaceIn
            // 
            this.timerFaceIn.Tick += new System.EventHandler(this.timerFaceIn_Tick);
            // 
            // pVao
            // 
            this.pVao.Controls.Add(this.Image_Xe_Vao_Sau);
            this.pVao.Controls.Add(this.Image_Xe_Vao_Truoc);
            this.pVao.Location = new System.Drawing.Point(0, 4);
            this.pVao.Name = "pVao";
            this.pVao.Size = new System.Drawing.Size(411, 610);
            this.pVao.TabIndex = 5;
            // 
            // pInfo
            // 
            this.pInfo.Controls.Add(this.Image_Plate);
            this.pInfo.Controls.Add(this.Image_ID);
            this.pInfo.Controls.Add(this.groupBox2);
            this.pInfo.Controls.Add(this.btnAllow);
            this.pInfo.Controls.Add(this.groupBox1);
            this.pInfo.Controls.Add(this.btnSave);
            this.pInfo.Controls.Add(this.metroLabel1);
            this.pInfo.Controls.Add(this.lbID);
            this.pInfo.Controls.Add(this.lbPlate);
            this.pInfo.Controls.Add(this.btnStart);
            this.pInfo.Controls.Add(this.metroLabel2);
            this.pInfo.Controls.Add(this.btnPlate);
            this.pInfo.Controls.Add(this.btnFace);
            this.pInfo.Location = new System.Drawing.Point(414, 4);
            this.pInfo.Name = "pInfo";
            this.pInfo.Size = new System.Drawing.Size(200, 610);
            this.pInfo.TabIndex = 6;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.LemonChiffon;
            this.groupBox2.Controls.Add(this.lbNotify);
            this.groupBox2.ForeColor = System.Drawing.Color.Red;
            this.groupBox2.Location = new System.Drawing.Point(0, 298);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 64);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Thông báo hệ thống";
            // 
            // lbNotify
            // 
            this.lbNotify.AutoSize = true;
            this.lbNotify.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lbNotify.Location = new System.Drawing.Point(6, 27);
            this.lbNotify.Name = "lbNotify";
            this.lbNotify.Size = new System.Drawing.Size(194, 15);
            this.lbNotify.TabIndex = 0;
            this.lbNotify.Text = "Đang tiến hành nhận dạng khuôn mặt";
            // 
            // btnAllow
            // 
            this.btnAllow.Location = new System.Drawing.Point(122, 495);
            this.btnAllow.Name = "btnAllow";
            this.btnAllow.Size = new System.Drawing.Size(75, 23);
            this.btnAllow.TabIndex = 19;
            this.btnAllow.Text = "Nhận diện";
            this.btnAllow.UseVisualStyleBackColor = true;
            this.btnAllow.Click += new System.EventHandler(this.btnAllow_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Snow;
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lbStartTime);
            this.groupBox1.ForeColor = System.Drawing.Color.Red;
            this.groupBox1.Location = new System.Drawing.Point(0, 368);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox1.Size = new System.Drawing.Size(200, 111);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label1.Location = new System.Drawing.Point(6, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Lượt xe trong ngày: 1";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(60, 569);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseSelectable = true;
            // 
            // pRa
            // 
            this.pRa.Controls.Add(this.Image_Xe_Ra_Sau);
            this.pRa.Controls.Add(this.Image_Xe_Ra_Truoc);
            this.pRa.Location = new System.Drawing.Point(617, 7);
            this.pRa.Name = "pRa";
            this.pRa.Size = new System.Drawing.Size(397, 607);
            this.pRa.TabIndex = 17;
            // 
            // pMenu
            // 
            this.pMenu.Controls.Add(this.btnHistory);
            this.pMenu.Controls.Add(this.btnMi);
            this.pMenu.Controls.Add(this.btnMax);
            this.pMenu.Controls.Add(this.btnExit);
            this.pMenu.Location = new System.Drawing.Point(0, 7);
            this.pMenu.Name = "pMenu";
            this.pMenu.Size = new System.Drawing.Size(1017, 27);
            this.pMenu.TabIndex = 18;
            // 
            // btnHistory
            // 
            this.btnHistory.Location = new System.Drawing.Point(831, 1);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(75, 23);
            this.btnHistory.Style = MetroFramework.MetroColorStyle.Green;
            this.btnHistory.TabIndex = 14;
            this.btnHistory.Text = "Lịch sử";
            this.btnHistory.Theme = MetroFramework.MetroThemeStyle.Light;
            this.btnHistory.UseSelectable = true;
            this.btnHistory.Click += new System.EventHandler(this.btnHistory_Click);
            // 
            // btnMi
            // 
            this.btnMi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnMi.FlatAppearance.BorderSize = 0;
            this.btnMi.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.btnMi.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.btnMi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMi.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMi.Location = new System.Drawing.Point(912, 0);
            this.btnMi.Name = "btnMi";
            this.btnMi.Size = new System.Drawing.Size(34, 27);
            this.btnMi.TabIndex = 12;
            this.btnMi.Text = "_";
            this.btnMi.UseVisualStyleBackColor = false;
            this.btnMi.Click += new System.EventHandler(this.btnMi_Click);
            // 
            // btnMax
            // 
            this.btnMax.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnMax.FlatAppearance.BorderSize = 0;
            this.btnMax.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.btnMax.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.btnMax.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMax.Location = new System.Drawing.Point(947, 0);
            this.btnMax.Name = "btnMax";
            this.btnMax.Size = new System.Drawing.Size(34, 27);
            this.btnMax.TabIndex = 11;
            this.btnMax.Text = "□";
            this.btnMax.UseVisualStyleBackColor = false;
            this.btnMax.Click += new System.EventHandler(this.btnMax_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.btnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(982, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(34, 27);
            this.btnExit.TabIndex = 13;
            this.btnExit.Text = "X";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // timerPlateIn
            // 
            this.timerPlateIn.Tick += new System.EventHandler(this.timerPlateIn_Tick);
            // 
            // timerFaceOut
            // 
            this.timerFaceOut.Tick += new System.EventHandler(this.timerFaceOut_Tick);
            // 
            // timerPlateOut
            // 
            this.timerPlateOut.Tick += new System.EventHandler(this.timerPlateOut_Tick);
            // 
            // MainForm2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 615);
            this.ControlBox = false;
            this.Controls.Add(this.pMenu);
            this.Controls.Add(this.pInfo);
            this.Controls.Add(this.pRa);
            this.Controls.Add(this.pVao);
            this.DisplayHeader = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm2";
            this.Padding = new System.Windows.Forms.Padding(20, 30, 20, 20);
            this.Resize += new System.EventHandler(this.MainForm2_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.Image_Xe_Vao_Truoc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Xe_Vao_Sau)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Xe_Ra_Truoc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Xe_Ra_Sau)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Plate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_ID)).EndInit();
            this.pVao.ResumeLayout(false);
            this.pInfo.ResumeLayout(false);
            this.pInfo.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pRa.ResumeLayout(false);
            this.pMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Image_Xe_Vao_Truoc;
        private System.Windows.Forms.PictureBox Image_Xe_Vao_Sau;
        private System.Windows.Forms.PictureBox Image_Xe_Ra_Truoc;
        private System.Windows.Forms.PictureBox Image_Xe_Ra_Sau;
        private MetroFramework.Controls.MetroLabel lbID;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel lbPlate;
        private System.Windows.Forms.PictureBox Image_Plate;
        private System.Windows.Forms.PictureBox Image_ID;
        private System.Windows.Forms.Timer timerFaceIn;
        private System.Windows.Forms.Button btnStart;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private System.Windows.Forms.Button btnPlate;
        private System.Windows.Forms.Button btnFace;
        private System.Windows.Forms.Label lbStartTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pVao;
        private System.Windows.Forms.Panel pRa;
        private System.Windows.Forms.Panel pInfo;
        private System.Windows.Forms.Panel pMenu;
        private System.Windows.Forms.Button btnMi;
        private System.Windows.Forms.Button btnMax;
        private System.Windows.Forms.Button btnExit;
        private MetroFramework.Controls.MetroButton btnSave;
        private System.Windows.Forms.Timer timerPlateIn;
        private System.Windows.Forms.Timer timerFaceOut;
        private System.Windows.Forms.Timer timerPlateOut;
        private System.Windows.Forms.Label label1;
        private MetroFramework.Controls.MetroButton btnHistory;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAllow;
        private System.Windows.Forms.GroupBox groupBox2;
        private MetroFramework.Controls.MetroLabel lbNotify;
    }
}