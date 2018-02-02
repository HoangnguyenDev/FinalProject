using Auto_parking;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.ML;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Camera
{
    public partial class KiemTraBienSoXeForm : MetroForm
    {
        #region định nghĩa
        public CascadeClassifier _face = new CascadeClassifier(Application.StartupPath + "/haarcascade_frontalface_alt2.xml");//Our face detection method 
        Classifier_Train _recognition = new Classifier_Train(Classifier_Train.LoadClassifierType.Face);

        Image<Bgr, Byte> currentFrameFace; //current image aquired from webcam for display
        Image<Gray, byte> result, TrainedFace = null; //used to store the result image and trained face
        Image<Gray, byte> gray_frameFace = null; //grayscale current image aquired from webcam for processing

        private int _fpsPlate;


        Image Plate_Draw;
        List<string> PlateTextList = new List<string>();
        List<Rectangle> listRect = new List<Rectangle>();
        PictureBox[] box = new PictureBox[12];

        private string m_path = Application.StartupPath + @"\data\";
        private List<string> lstimages = new List<string>();
        private const string m_lang = "eng";
        private bool _exPlate = true;

        //int current = 0;
        VideoCapture _capturePlateIn = null;
        VideoCapture _captureFaceIn = null; //This is our capture variable
        VideoCapture _capturePlateOut = null;
        VideoCapture _captureFaceOut = null; //This is our capture variable
        int _countErrorPR = 0;
        int _fpsFaceIn = 30;
        int _fpsFaceOut = 30;
        int _fpsPlateOut = 30;

        int _fpsPlateIn = 30;
        #region variable demo
        int _totalFramePlateIn;
        int _totalFramePlateOut;
        int _totalFrameFaceIn;
        int _totalFrameFaceOut;
        int _currentFramePlateIn = 0;
        int _currentFramePlateOut = 0;
        int _currentFrameFaceIn = 0;
        int _currentFrameFaceOut = 0;
        bool isFinishFaceIn = false;
        bool isFinishPlateIn = false;
        int countNotReFace = 0;
        const string PATH_PLATE_IN = "Demo\\XeVaoBienSo.mp4";
        const string PATH_FACE_IN = "Demo\\XeVaoMat.mp4";
        #endregion


        public bool isEx = false;
        public bool isDemo = false;
        public LPR _lpr;
        public DateTime _startPRDT;
        public DateTime _currentPRDT;
        const int DELAY_PR_DT = 400;
        admin_dangkythitoeicEntities database = new admin_dangkythitoeicEntities();
        #endregion
        public KiemTraBienSoXeForm()
        {
            //Thread t = new Thread(new ThreadStart(Loading));
            //t.Start();
            //for (int i = 0; i <= 1000; i++)
            //    Thread.Sleep(10);
            // t.Abort();
            InitializeComponent();
            SVM svm = new SVM();
            _lpr = new LPR(svm);

            #region Khởi tạo Camera nếu là Demo
            if (isDemo)
            {
                _capturePlateIn = new Emgu.CV.VideoCapture(PATH_PLATE_IN);
                _capturePlateIn.SetCaptureProperty(CapProp.FrameWidth, 640);
                _capturePlateIn.SetCaptureProperty(CapProp.FrameHeight, 480);
                _fpsPlateIn = (Int32)_capturePlateIn.GetCaptureProperty(CapProp.Fps);
                _totalFramePlateIn = Convert.ToInt32(_capturePlateIn.GetCaptureProperty(CapProp.FrameCount)) - 4;

                _captureFaceIn = new Emgu.CV.VideoCapture(PATH_FACE_IN);
                _captureFaceIn.SetCaptureProperty(CapProp.FrameWidth, 320);
                _captureFaceIn.SetCaptureProperty(CapProp.FrameHeight, 240);
                _fpsFaceIn = (Int32)_captureFaceIn.GetCaptureProperty(CapProp.Fps);
                _totalFrameFaceIn = (Int32)_captureFaceIn.GetCaptureProperty(CapProp.FrameCount) - 4;
                timerFaceIn.Enabled = true;
                timerPlateIn.Enabled = true;

            }
            #endregion
            #region Khởi tạo Camera bình thường
            else
            {
                //_capturePlateIn = new Emgu.CV.VideoCapture(1);
                //_capturePlateIn.SetCaptureProperty(CapProp.FrameWidth, 640);
               // _capturePlateIn.SetCaptureProperty(CapProp.FrameHeight, 480);
                //_captureFaceIn = new Emgu.CV.VideoCapture(1);
                //_captureFaceIn.SetCaptureProperty(CapProp.FrameWidth, 320);
                //_captureFaceIn.SetCaptureProperty(CapProp.FrameHeight, 240);
               // timerFaceIn.Enabled = true;
                //timerPlateIn.Enabled = true;
            }
            #endregion

            
            //timer1.Enabled = true;
            if (_captureFaceOut == null)
            {
            }
            if (_capturePlateOut == null)
            {
            }
            //Application.Idle += new EventHandler(FrameGrabber);
            //timer1.Interval = 1000 / _fpsFaceIn;
            //timer1.Tick += new EventHandler(timer1_Tick);
            //timer1.Start();

        }
        void Loading()
        {
            SplashScreenForm frm = new SplashScreenForm();
            Application.Run(frm);
        }


        public void SaveIn()
        {
            isFinishFaceIn = false;
            isFinishPlateIn = false;
        }
       
        private void btnStart_Click(object sender, EventArgs e)
        {
            isFinishFaceIn = true;
        }


        private void btnFace_Click(object sender, EventArgs e)
        {
        }

        private void btnPlate_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        public void Reset()
        {
            btnSave.Enabled = false;
            isEx = false;
        }

        private void MainForm2_Resize(object sender, EventArgs e)
        {
            pMenu.Size = new Size(Size.Width, pMenu.Size.Height);
            btnExit.Location = new Point(pMenu.Size.Width - btnExit.Size.Width, btnExit.Location.Y);
            btnMax.Location = new Point(btnExit.Location.X - btnMax.Size.Width - 1, btnMax.Location.Y);
            btnMi.Location = new Point(btnMax.Location.X - btnMi.Size.Width - 1, btnMi.Location.Y);

            pVao.Location = new Point(0, 5);
            pVao.Size = new Size(Size.Width / 5 * 2, Size.Height);
            pRa.Location = new Point(Size.Width/5 * 3, 5);
            pRa.Size = new Size(Size.Width / 5 * 2, Size.Height);
            pInfo.Location = new Point(pVao.Size.Width, 5);
            pInfo.Size = new Size(Size.Width/5, Size.Height);


            
            Image_Xe_Vao_Truoc.Size = new Size(pVao.Size.Width - 20, (pVao.Size.Height-30) / 2 - 20);
            Image_Xe_Vao_Truoc.Location = new Point(pVao.Location.X + 10, pVao.Location.Y + 30);
        }

        private void btnMi_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void btnMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
        }
        private void timerFaceIn_Tick(object sender, EventArgs e)
        {
        }
        private void timerPlateIn_Tick(object sender, EventArgs e)
        {
            Image<Bgr, byte> capPrImageIn = null;
            if (isDemo)
            {
                if (_currentFramePlateIn < _totalFramePlateIn)
                {
                    capPrImageIn = _capturePlateIn.QueryFrame().ToImage<Bgr, byte>();
                    _currentFramePlateIn++;
                    Image_Xe_Vao_Truoc.Update();
                }
                //await Task.Delay(1000 / _fpsFaceIn / 4);
            }
            else
            {
                capPrImageIn = _capturePlateIn.QueryFrame().ToImage<Bgr, byte>();
                //Image_Xe_Vao_Truoc.Update();
                //Image_Xe_Vao_Sau.Update();
            }

            if (capPrImageIn != null && isFinishFaceIn)
            {
                try
                {
                    List<Rectangle> listRect = new List<Rectangle>();
                    List<Mat> listMat = new List<Mat>();
                    Bitmap imagePlate;
                    Image Plate_Draw;
                    Image<Bgr, byte> Plate_Draw_Resize;

                    _currentPRDT = DateTime.Now;
                    //if (_currentPRDT.Subtract(_startPRDT).TotalMilliseconds > DELAY_PR_DT)
                    //{
                    //    _startPRDT = DateTime.Now;
                        _lpr.ProcessImage(capPrImageIn.Bitmap,out Plate_Draw,out Plate_Draw_Resize, null);
                        if (Plate_Draw_Resize != null)
                        {
                            isFinishPlateIn = true;
                            Image_Plate.Image = Plate_Draw;
                        }
                            //if (_lpr.DetectRectangleLPR(capPrImageIn.Bitmap, out imagePlate, out listRect, out listMat))
                            //{
                               
                            //}
                    //}
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void pVao_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image (*.bmp; *.jpg; *.jpeg; *.png) |*.bmp; *.jpg; *.jpeg; *.png|All files (*.*)|*.*||";
            dlg.InitialDirectory = "E:\\GreenParking";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            string startupPath = dlg.FileName;

            Bitmap temp1 = (new Image<Bgr, byte>(dlg.FileName)).Bitmap;
            string temp2, temp3;
            Image Plate_Draw;
            Image<Bgr,byte> PlateResize;
            Image_Xe_Vao_Truoc.Image = temp1;
            _lpr.ProcessImageWithCrop(temp1,out Plate_Draw, out PlateResize);
            if (PlateResize != null)
            {
                Image_Plate.Image = PlateResize.Bitmap;
                Random random = new Random();
                string fileName = Application.StartupPath + "\\TestPlate\\" + random.Next(1000).ToString() + ".jpg";
                PlateResize.Save(fileName);
                MessageBox.Show("Đã xác nhận biển số và lưu thành công");
            }

        }

        private void btnORB_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image (*.bmp; *.jpg; *.jpeg; *.png) |*.bmp; *.jpg; *.jpeg; *.png|All files (*.*)|*.*||";
            dlg.InitialDirectory = "E:\\GreenParking";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            string startupPath = dlg.FileName;

            Bitmap temp1 = (new Image<Bgr, byte>(dlg.FileName)).Bitmap;
            string temp2, temp3;
            Image Plate_Draw;
            Image<Bgr, byte> PlateResize;
            Image_Xe_Vao_Truoc.Image = temp1;
            _lpr.ProcessImage(temp1, out Plate_Draw, out PlateResize, null);
            if (PlateResize != null)
            {
                richTextBox1.Clear();
                Bitmap masterImage = PlateResize.Bitmap;
                FLANN fLANN = new FLANN();
                Image<Gray, byte> imageOut = new Image<Gray, byte>(masterImage);
                fLANN.LoadData("TestPlate");
                var list = fLANN.Reconize(imageOut);
                int i = 0;
                foreach (var item in list.OrderByDescending(p => p.Similarity).Take(3).ToList())
                {
                    i++;
                    richTextBox1.AppendText(
                        "filename: " + item.fileName + " Similarity: " + item.Similarity + "\n");
                    CvInvoke.Imshow(i.ToString(),new Image<Bgr,byte> (item.fileName));
                }
            }




        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
            this.Close();
        }

       

    }
}
