namespace Camera
{
    partial class SplashScreenForm
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
            this.lbLoading = new MetroFramework.Controls.MetroLabel();
            this.metroProgressBar1 = new MetroFramework.Controls.MetroProgressBar();
            this.SuspendLayout();
            // 
            // lbLoading
            // 
            this.lbLoading.AutoSize = true;
            this.lbLoading.Location = new System.Drawing.Point(23, 103);
            this.lbLoading.Name = "lbLoading";
            this.lbLoading.Size = new System.Drawing.Size(65, 19);
            this.lbLoading.TabIndex = 3;
            this.lbLoading.Text = "Loading...";
            // 
            // metroProgressBar1
            // 
            this.metroProgressBar1.Location = new System.Drawing.Point(23, 63);
            this.metroProgressBar1.Name = "metroProgressBar1";
            this.metroProgressBar1.ProgressBarStyle = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.metroProgressBar1.Size = new System.Drawing.Size(263, 23);
            this.metroProgressBar1.TabIndex = 2;
            // 
            // SplashScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 134);
            this.Controls.Add(this.lbLoading);
            this.Controls.Add(this.metroProgressBar1);
            this.Name = "SplashScreenForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel lbLoading;
        private MetroFramework.Controls.MetroProgressBar metroProgressBar1;
    }
}