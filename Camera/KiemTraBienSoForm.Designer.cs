namespace Camera
{
    partial class KiemTraBienSoXeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KiemTraBienSoXeForm));
            this.Image_Xe_Vao_Truoc = new System.Windows.Forms.PictureBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.Image_Plate = new System.Windows.Forms.PictureBox();
            this.timerFaceIn = new System.Windows.Forms.Timer(this.components);
            this.pVao = new System.Windows.Forms.Panel();
            this.pInfo = new System.Windows.Forms.Panel();
            this.btnORB = new System.Windows.Forms.Button();
            this.btnLoadImage = new System.Windows.Forms.Button();
            this.btnSave = new MetroFramework.Controls.MetroButton();
            this.pRa = new System.Windows.Forms.Panel();
            this.pMenu = new System.Windows.Forms.Panel();
            this.btnMi = new System.Windows.Forms.Button();
            this.btnMax = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.timerPlateIn = new System.Windows.Forms.Timer(this.components);
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Xe_Vao_Truoc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Plate)).BeginInit();
            this.pVao.SuspendLayout();
            this.pInfo.SuspendLayout();
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
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(71, 345);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "Nhận diện";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // Image_Plate
            // 
            this.Image_Plate.Image = ((System.Drawing.Image)(resources.GetObject("Image_Plate.Image")));
            this.Image_Plate.Location = new System.Drawing.Point(60, 159);
            this.Image_Plate.Name = "Image_Plate";
            this.Image_Plate.Size = new System.Drawing.Size(100, 89);
            this.Image_Plate.TabIndex = 3;
            this.Image_Plate.TabStop = false;
            // 
            // timerFaceIn
            // 
            this.timerFaceIn.Tick += new System.EventHandler(this.timerFaceIn_Tick);
            // 
            // pVao
            // 
            this.pVao.Controls.Add(this.Image_Xe_Vao_Truoc);
            this.pVao.Location = new System.Drawing.Point(0, 4);
            this.pVao.Name = "pVao";
            this.pVao.Size = new System.Drawing.Size(411, 610);
            this.pVao.TabIndex = 5;
            this.pVao.Paint += new System.Windows.Forms.PaintEventHandler(this.pVao_Paint);
            // 
            // pInfo
            // 
            this.pInfo.Controls.Add(this.btnORB);
            this.pInfo.Controls.Add(this.btnLoadImage);
            this.pInfo.Controls.Add(this.btnSave);
            this.pInfo.Controls.Add(this.Image_Plate);
            this.pInfo.Controls.Add(this.btnStart);
            this.pInfo.Location = new System.Drawing.Point(414, 4);
            this.pInfo.Name = "pInfo";
            this.pInfo.Size = new System.Drawing.Size(200, 610);
            this.pInfo.TabIndex = 6;
            // 
            // btnORB
            // 
            this.btnORB.BackColor = System.Drawing.Color.Silver;
            this.btnORB.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnORB.FlatAppearance.BorderSize = 2;
            this.btnORB.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.btnORB.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnORB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnORB.Font = new System.Drawing.Font("Segoe UI Semibold", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnORB.ImageKey = "(none)";
            this.btnORB.Location = new System.Drawing.Point(19, 96);
            this.btnORB.Name = "btnORB";
            this.btnORB.Size = new System.Drawing.Size(178, 47);
            this.btnORB.TabIndex = 18;
            this.btnORB.Text = "Sử dụng ORB";
            this.btnORB.UseVisualStyleBackColor = false;
            this.btnORB.Click += new System.EventHandler(this.btnORB_Click);
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.BackColor = System.Drawing.Color.Silver;
            this.btnLoadImage.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnLoadImage.FlatAppearance.BorderSize = 2;
            this.btnLoadImage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.btnLoadImage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnLoadImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadImage.Font = new System.Drawing.Font("Segoe UI Semibold", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadImage.ImageKey = "(none)";
            this.btnLoadImage.Location = new System.Drawing.Point(19, 43);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(178, 47);
            this.btnLoadImage.TabIndex = 17;
            this.btnLoadImage.Text = "Nhận dạng files";
            this.btnLoadImage.UseVisualStyleBackColor = false;
            this.btnLoadImage.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(71, 294);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseSelectable = true;
            // 
            // pRa
            // 
            this.pRa.Controls.Add(this.richTextBox1);
            this.pRa.Location = new System.Drawing.Point(617, 7);
            this.pRa.Name = "pRa";
            this.pRa.Size = new System.Drawing.Size(397, 607);
            this.pRa.TabIndex = 17;
            // 
            // pMenu
            // 
            this.pMenu.Controls.Add(this.btnMi);
            this.pMenu.Controls.Add(this.btnMax);
            this.pMenu.Controls.Add(this.btnExit);
            this.pMenu.Location = new System.Drawing.Point(0, 7);
            this.pMenu.Name = "pMenu";
            this.pMenu.Size = new System.Drawing.Size(1017, 27);
            this.pMenu.TabIndex = 18;
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
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(25, 40);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(352, 205);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // KiemTraBienSoXeForm
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
            this.Name = "KiemTraBienSoXeForm";
            this.Padding = new System.Windows.Forms.Padding(20, 30, 20, 20);
            this.Resize += new System.EventHandler(this.MainForm2_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.Image_Xe_Vao_Truoc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Image_Plate)).EndInit();
            this.pVao.ResumeLayout(false);
            this.pInfo.ResumeLayout(false);
            this.pRa.ResumeLayout(false);
            this.pMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Image_Xe_Vao_Truoc;
        private System.Windows.Forms.PictureBox Image_Plate;
        private System.Windows.Forms.Timer timerFaceIn;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Panel pVao;
        private System.Windows.Forms.Panel pRa;
        private System.Windows.Forms.Panel pInfo;
        private System.Windows.Forms.Panel pMenu;
        private System.Windows.Forms.Button btnMi;
        private System.Windows.Forms.Button btnMax;
        private System.Windows.Forms.Button btnExit;
        private MetroFramework.Controls.MetroButton btnSave;
        private System.Windows.Forms.Timer timerPlateIn;
        private System.Windows.Forms.Button btnORB;
        private System.Windows.Forms.Button btnLoadImage;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}