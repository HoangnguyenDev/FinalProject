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
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.lbStartTime = new System.Windows.Forms.Label();
            this.lbDirector = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnPlate = new System.Windows.Forms.Button();
            this.btnFace = new System.Windows.Forms.Button();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.btnStart = new System.Windows.Forms.Button();
            this.lbPlate = new MetroFramework.Controls.MetroLabel();
            this.Image_Plate = new System.Windows.Forms.PictureBox();
            this.Image_ID = new System.Windows.Forms.PictureBox();
            this.lbID = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lbNotify = new MetroFramework.Controls.MetroLabel();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Xe_Vao_Truoc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Xe_Vao_Sau)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Xe_Ra_Truoc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Xe_Ra_Sau)).BeginInit();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Plate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_ID)).BeginInit();
            this.SuspendLayout();
            // 
            // Image_Xe_Vao_Truoc
            // 
            this.Image_Xe_Vao_Truoc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Image_Xe_Vao_Truoc.Location = new System.Drawing.Point(41, 63);
            this.Image_Xe_Vao_Truoc.Name = "Image_Xe_Vao_Truoc";
            this.Image_Xe_Vao_Truoc.Size = new System.Drawing.Size(400, 260);
            this.Image_Xe_Vao_Truoc.TabIndex = 0;
            this.Image_Xe_Vao_Truoc.TabStop = false;
            // 
            // Image_Xe_Vao_Sau
            // 
            this.Image_Xe_Vao_Sau.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Image_Xe_Vao_Sau.Location = new System.Drawing.Point(41, 376);
            this.Image_Xe_Vao_Sau.Name = "Image_Xe_Vao_Sau";
            this.Image_Xe_Vao_Sau.Size = new System.Drawing.Size(400, 260);
            this.Image_Xe_Vao_Sau.TabIndex = 1;
            this.Image_Xe_Vao_Sau.TabStop = false;
            // 
            // Image_Xe_Ra_Truoc
            // 
            this.Image_Xe_Ra_Truoc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Image_Xe_Ra_Truoc.Image = ((System.Drawing.Image)(resources.GetObject("Image_Xe_Ra_Truoc.Image")));
            this.Image_Xe_Ra_Truoc.Location = new System.Drawing.Point(681, 63);
            this.Image_Xe_Ra_Truoc.Name = "Image_Xe_Ra_Truoc";
            this.Image_Xe_Ra_Truoc.Size = new System.Drawing.Size(450, 260);
            this.Image_Xe_Ra_Truoc.TabIndex = 2;
            this.Image_Xe_Ra_Truoc.TabStop = false;
            // 
            // Image_Xe_Ra_Sau
            // 
            this.Image_Xe_Ra_Sau.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Image_Xe_Ra_Sau.Image = ((System.Drawing.Image)(resources.GetObject("Image_Xe_Ra_Sau.Image")));
            this.Image_Xe_Ra_Sau.Location = new System.Drawing.Point(681, 376);
            this.Image_Xe_Ra_Sau.Name = "Image_Xe_Ra_Sau";
            this.Image_Xe_Ra_Sau.Size = new System.Drawing.Size(450, 260);
            this.Image_Xe_Ra_Sau.TabIndex = 3;
            this.Image_Xe_Ra_Sau.TabStop = false;
            // 
            // groupBox
            // 
            this.groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.groupBox.BackColor = System.Drawing.Color.Transparent;
            this.groupBox.Controls.Add(this.lbNotify);
            this.groupBox.Controls.Add(this.lbStartTime);
            this.groupBox.Controls.Add(this.lbDirector);
            this.groupBox.Controls.Add(this.label2);
            this.groupBox.Controls.Add(this.comboBox1);
            this.groupBox.Controls.Add(this.label1);
            this.groupBox.Controls.Add(this.btnSave);
            this.groupBox.Controls.Add(this.btnPlate);
            this.groupBox.Controls.Add(this.btnFace);
            this.groupBox.Controls.Add(this.metroLabel2);
            this.groupBox.Controls.Add(this.btnStart);
            this.groupBox.Controls.Add(this.lbPlate);
            this.groupBox.Controls.Add(this.Image_Plate);
            this.groupBox.Controls.Add(this.Image_ID);
            this.groupBox.Controls.Add(this.lbID);
            this.groupBox.Controls.Add(this.metroLabel1);
            this.groupBox.Location = new System.Drawing.Point(447, 63);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(228, 573);
            this.groupBox.TabIndex = 4;
            this.groupBox.TabStop = false;
            // 
            // lbStartTime
            // 
            this.lbStartTime.AutoSize = true;
            this.lbStartTime.Location = new System.Drawing.Point(30, 343);
            this.lbStartTime.Name = "lbStartTime";
            this.lbStartTime.Size = new System.Drawing.Size(82, 13);
            this.lbStartTime.TabIndex = 15;
            this.lbStartTime.Text = "Thời gian gửi xe";
            // 
            // lbDirector
            // 
            this.lbDirector.AutoSize = true;
            this.lbDirector.Location = new System.Drawing.Point(94, 368);
            this.lbDirector.Name = "lbDirector";
            this.lbDirector.Size = new System.Drawing.Size(0, 13);
            this.lbDirector.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 368);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Hướng Xe";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(79, 543);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 546);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Chế độ";
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(94, 503);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Lưu lại";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnPlate
            // 
            this.btnPlate.Location = new System.Drawing.Point(125, 460);
            this.btnPlate.Name = "btnPlate";
            this.btnPlate.Size = new System.Drawing.Size(75, 23);
            this.btnPlate.TabIndex = 9;
            this.btnPlate.Text = "Số Xe";
            this.btnPlate.UseVisualStyleBackColor = true;
            this.btnPlate.Click += new System.EventHandler(this.btnPlate_Click);
            // 
            // btnFace
            // 
            this.btnFace.Location = new System.Drawing.Point(33, 460);
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
            this.metroLabel2.Location = new System.Drawing.Point(13, 227);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(51, 19);
            this.metroLabel2.TabIndex = 7;
            this.metroLabel2.Text = "Biển số";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(94, 412);
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
            this.lbPlate.ForeColor = System.Drawing.Color.Transparent;
            this.lbPlate.Location = new System.Drawing.Point(69, 227);
            this.lbPlate.Name = "lbPlate";
            this.lbPlate.Size = new System.Drawing.Size(34, 19);
            this.lbPlate.TabIndex = 4;
            this.lbPlate.Text = "sdfg";
            this.lbPlate.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // Image_Plate
            // 
            this.Image_Plate.Image = ((System.Drawing.Image)(resources.GetObject("Image_Plate.Image")));
            this.Image_Plate.Location = new System.Drawing.Point(70, 135);
            this.Image_Plate.Name = "Image_Plate";
            this.Image_Plate.Size = new System.Drawing.Size(100, 89);
            this.Image_Plate.TabIndex = 3;
            this.Image_Plate.TabStop = false;
            // 
            // Image_ID
            // 
            this.Image_ID.Image = ((System.Drawing.Image)(resources.GetObject("Image_ID.Image")));
            this.Image_ID.Location = new System.Drawing.Point(69, 22);
            this.Image_ID.Name = "Image_ID";
            this.Image_ID.Size = new System.Drawing.Size(100, 89);
            this.Image_ID.TabIndex = 2;
            this.Image_ID.TabStop = false;
            // 
            // lbID
            // 
            this.lbID.AutoSize = true;
            this.lbID.Location = new System.Drawing.Point(104, 114);
            this.lbID.Name = "lbID";
            this.lbID.Size = new System.Drawing.Size(45, 19);
            this.lbID.TabIndex = 1;
            this.lbID.Text = "sadsfa";
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(51, 114);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(21, 19);
            this.metroLabel1.TabIndex = 0;
            this.metroLabel1.Text = "ID";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lbNotify
            // 
            this.lbNotify.AutoSize = true;
            this.lbNotify.Location = new System.Drawing.Point(79, 280);
            this.lbNotify.Name = "lbNotify";
            this.lbNotify.Size = new System.Drawing.Size(0, 0);
            this.lbNotify.TabIndex = 16;
            // 
            // MainForm2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1179, 676);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.Image_Xe_Ra_Sau);
            this.Controls.Add(this.Image_Xe_Ra_Truoc);
            this.Controls.Add(this.Image_Xe_Vao_Sau);
            this.Controls.Add(this.Image_Xe_Vao_Truoc);
            this.MaximizeBox = false;
            this.Name = "MainForm2";
            this.Text = "Quản lý xe ";
            this.ResizeBegin += new System.EventHandler(this.MainForm2_ResizeBegin);
            ((System.ComponentModel.ISupportInitialize)(this.Image_Xe_Vao_Truoc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Xe_Vao_Sau)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Xe_Ra_Truoc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Xe_Ra_Sau)).EndInit();
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Plate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_ID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Image_Xe_Vao_Truoc;
        private System.Windows.Forms.PictureBox Image_Xe_Vao_Sau;
        private System.Windows.Forms.PictureBox Image_Xe_Ra_Truoc;
        private System.Windows.Forms.PictureBox Image_Xe_Ra_Sau;
        private System.Windows.Forms.GroupBox groupBox;
        private MetroFramework.Controls.MetroLabel lbID;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel lbPlate;
        private System.Windows.Forms.PictureBox Image_Plate;
        private System.Windows.Forms.PictureBox Image_ID;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnStart;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private System.Windows.Forms.Button btnPlate;
        private System.Windows.Forms.Button btnFace;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label lbStartTime;
        private System.Windows.Forms.Label lbDirector;
        private System.Windows.Forms.Label label2;
        private MetroFramework.Controls.MetroLabel lbNotify;
    }
}