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
    public partial class MainForm2 : MetroForm
    {
        #region định nghĩa
        public CascadeClassifier _face = new CascadeClassifier(Application.StartupPath + "/haarcascade_frontalface_default.xml");//Our face detection method 
        CascadeClassifier _plate = new CascadeClassifier(Application.StartupPath + "\\biensoxemay.xml");
        //CascadeClassifier _plate = new CascadeClassifier(Application.StartupPath + "\\output-hv-33-x25.xml");

        Classifier_Train _recognition = new Classifier_Train(Classifier_Train.LoadClassifierType.Face);

        Image<Gray, byte> gray_frameFace = null; //grayscale current image aquired from webcam for processing

        private int _fpsPlate;

        List<Image<Bgr, byte>> PlateImagesList = new List<Image<Bgr, byte>>();

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
        bool isFinishFaceOut = false;
        bool _isFinishPlateIn = false;
        int countNotReFace = 0;
        const string PATH_PLATE_IN = "Demo\\XeVaoBienSo.mp4";
        const string PATH_FACE_IN = "Demo\\XeVaoMat.mp4";
        Image<Bgr, byte> Image_Store_FaceIn;
        Image<Bgr, byte> Image_Store_FaceOut;
        Image<Bgr, byte> Image_Store_PlateIn;
        Image<Bgr, byte> Image_Store_PlateOut;
        int _watingFaceIn;
        DateTime _startWatingTimeFaceIn;

        #endregion


        public bool _isEx = false;
        public bool _isDemo = false;
        public LPR _lpr;
        public DateTime _startPRDT;
        public DateTime _currentPRDT;
        const int DELAY_PR_DT = 400;
        DataContext _dataContext = new DataContext();
        private SVM svm;

        public List<string> listFace = new List<string>();
        public List<int> listLabel = new List<int>();
        public bool IsExistFace = false;
        #endregion
        public MainForm2()
        {
            //Thread t = new Thread(new ThreadStart(Loading));
            //t.Start();
            //for (int i = 0; i <= 1000; i++)
            //    Thread.Sleep(10);
            // t.Abort();
            InitializeComponent();
            lbID.Text = "";
            svm = SVMExtension.Create();
            _lpr = new LPR(svm);
            lbPlate.Text = "";
            _dataContext.LoadTraningFace(out listFace, out listLabel);
            _recognition.Update(listFace, listLabel);
            #region Khởi tạo Camera nếu là Demo
            if (_isDemo)
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
                _capturePlateIn = new Emgu.CV.VideoCapture(0);
                _capturePlateIn.SetCaptureProperty(CapProp.FrameWidth, 640);
                _capturePlateIn.SetCaptureProperty(CapProp.FrameHeight, 480);

                //_capturePlateOut = new Emgu.CV.VideoCapture(0);
                //_capturePlateOut.SetCaptureProperty(CapProp.FrameWidth, 640);
                //_capturePlateOut.SetCaptureProperty(CapProp.FrameHeight, 480);

                _captureFaceIn = new Emgu.CV.VideoCapture(1);
                _captureFaceIn.SetCaptureProperty(CapProp.FrameWidth, 320);
                _captureFaceIn.SetCaptureProperty(CapProp.FrameHeight, 240);

                //_captureFaceOut = new Emgu.CV.VideoCapture(1);
                //_captureFaceOut.SetCaptureProperty(CapProp.FrameWidth, 320);
                //_captureFaceOut.SetCaptureProperty(CapProp.FrameHeight, 240);

                timerFaceIn.Enabled = true;
                timerPlateIn.Enabled = true;
            }
            #endregion
            
            //timer1.Enabled = true;
            if (_captureFaceOut == null)
            {
                Image_Xe_Ra_Truoc.Image = (new Image<Bgr, byte>("Picture\\camera_not_found.png")).Bitmap;
                Image_Xe_Ra_Truoc.Update();
            }
            if (_capturePlateOut == null)
            {
                Image_Xe_Ra_Sau.Image = (new Image<Bgr, byte>("Picture\\camera_not_found.png")).Bitmap;
                Image_Xe_Ra_Truoc.Update();
            }
            if (_captureFaceIn == null)
            {
                Image_Xe_Vao_Truoc.Image = (new Image<Bgr, byte>("Picture\\camera_not_found.png")).Bitmap;
                Image_Xe_Vao_Truoc.Update();
            }
            if (_capturePlateIn == null)
            {
                Image_Xe_Vao_Sau.Image = (new Image<Bgr, byte>("Picture\\camera_not_found.png")).Bitmap;
                Image_Xe_Vao_Truoc.Update();
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
            _isFinishPlateIn = false;
        }
       
        private void btnStart_Click(object sender, EventArgs e)
        {
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
            Image_ID.Image = (new Image<Bgr, byte>("Picture\\avatar-sample.jpg")).Bitmap;
            btnSave.Enabled = false;
            _isEx = false;
            lbID.Text = "";
            lbPlate.Text = "";
        }

        private void MainForm2_Resize(object sender, EventArgs e)
        {
            pMenu.Size = new Size(Size.Width, pMenu.Size.Height);
            btnExit.Location = new Point(pMenu.Size.Width - btnExit.Size.Width, btnExit.Location.Y);
            btnMax.Location = new Point(btnExit.Location.X - btnMax.Size.Width - 1, btnMax.Location.Y);
            btnMi.Location = new Point(btnMax.Location.X - btnMi.Size.Width - 1, btnMi.Location.Y);
            btnHistory.Location = new Point(btnMi.Location.X - btnHistory.Size.Width - 1, btnMi.Location.Y);

            pVao.Location = new Point(0, 5);
            pVao.Size = new Size(Size.Width / 5 * 2, Size.Height);
            pRa.Location = new Point(Size.Width/5 * 3, 5);
            pRa.Size = new Size(Size.Width / 5 * 2, Size.Height);
            pInfo.Location = new Point(pVao.Size.Width, 5);
            pInfo.Size = new Size(Size.Width/5, Size.Height);

            Image_Xe_Ra_Truoc.Size = new Size(pRa.Size.Width - 20, (pRa.Size.Height-30) / 2 - 20);
            Image_Xe_Ra_Truoc.Location = new Point(pRa.Location.X + 10, pRa.Location.Y + 30);
            Image_Xe_Ra_Sau.Size = new Size(pRa.Size.Width - 20, (pRa.Size.Height-30) / 2 - 20);
            Image_Xe_Ra_Sau.Location = new Point(pRa.Location.X + 10, pRa.Location.Y + 30 + 20 + Image_Xe_Ra_Truoc.Height);

            Image_Xe_Vao_Truoc.Size = new Size(pVao.Size.Width - 20, (pVao.Size.Height-30) / 2 - 20);
            Image_Xe_Vao_Truoc.Location = new Point(pVao.Location.X + 10, pVao.Location.Y + 30);
            Image_Xe_Vao_Sau.Size = new Size(pVao.Size.Width - 20, (pVao.Size.Height-30) / 2 - 20);
            Image_Xe_Vao_Sau.Location = new Point(pVao.Location.X + 10, pVao.Location.Y + 30 + 20 + Image_Xe_Vao_Truoc.Height);


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
            Image<Bgr, byte> capFaceImageIn = null;

            #region Load frame Face
            if (_isDemo)
            {
                if (_currentFrameFaceIn < _totalFrameFaceIn)
                {
                    capFaceImageIn = _captureFaceIn.QueryFrame().ToImage<Bgr, byte>();
                    Image_Xe_Vao_Sau.Image = capFaceImageIn.Resize(Image_Xe_Vao_Sau.Width, Image_Xe_Ra_Truoc.Height, Inter.Cubic).Bitmap;
                    _currentFrameFaceIn++;
                    Image_Xe_Vao_Sau.Update();
                }
                
                //await Task.Delay(1000 / _fpsFaceIn / 4);
            }
            else if(_captureFaceIn.IsOpened)
            {
                capFaceImageIn = _captureFaceIn.QueryFrame().ToImage<Bgr, byte>();
                Image_Xe_Vao_Sau.Image = capFaceImageIn.Resize(Image_Xe_Vao_Sau.Width, Image_Xe_Ra_Truoc.Height, Inter.Cubic).Bitmap;
                //Image_Xe_Vao_Sau.Update();
            }
            #endregion
            DateTime currentDT = DateTime.Now;
            if (currentDT.Subtract(_startWatingTimeFaceIn).Milliseconds > Global.WAITING_TIME)
            {
                _isFinishPlateIn = false;
                lbNotify.Text = Global.TEXT_DETECT_FACE;
            }
            else if(isFinishFaceIn && _isFinishPlateIn)
            {
                lbNotify.Text = Global.TEXT_WAITING;
            }
            if (capFaceImageIn != null && !_isFinishPlateIn)
            {
               // lbNotify.Text = Global.TEXT_DETECT_FACE;
                gray_frameFace = capFaceImageIn.Convert<Gray, Byte>();

                //Face Detector
                Rectangle[] facesDetected = _face.DetectMultiScale(gray_frameFace, 1.2);

                //Action for each element detected
                Parallel.For(0, facesDetected.Length, i =>
                {
                    try
                    {
                        //facesDetected[i].X += (int)(facesDetected[i].Height * 0.15);
                        //facesDetected[i].Y += (int)(facesDetected[i].Width * 0.22);
                        //facesDetected[i].Height -= (int)(facesDetected[i].Height * 0.3);
                        //facesDetected[i].Width -= (int)(facesDetected[i].Width * 0.35);
                        Image<Gray, byte> result = capFaceImageIn.Copy(facesDetected[i]).Convert<Gray, byte>().Resize(100, 100, Inter.Cubic);
                        Image<Bgr, byte> imageShow = capFaceImageIn.Copy(facesDetected[i]).Resize(Image_ID.Width, Image_ID.Height, Inter.Cubic);

                        result._EqualizeHist();
                        //draw the face detected in the 0th (gray) channel with blue color
                        capFaceImageIn.Draw(facesDetected[i], new Bgr(Color.Red), 2);

                        _startPRDT = DateTime.Now;
                        if (_recognition.IsTrained)
                        {
                            if (countNotReFace <= 3)
                            {
                                string name = _recognition.Recognise(result);
                                int match_value = (int)_recognition.Get_Fisher_Distance;
                                if (name == "Unknown" || name == "")
                                {
                                    countNotReFace++;
                                    IsExistFace = false;
                                }
                                else
                                {
                                    Image_ID.Image = imageShow.Bitmap;
                                    Image_Store_FaceIn = new Image<Bgr, byte>(imageShow.Bitmap);
                                    isFinishFaceIn = true;
                                    lbID.Text = name;
                                    IsExistFace = true;
                                }
                            }
                            if (countNotReFace >= 3)
                            {
                                Image_ID.Image = imageShow.Bitmap;
                                Image_Store_FaceIn = new Image<Bgr, byte>(imageShow.Bitmap);
                                isFinishFaceIn = true;
                            }
                        }
                        else
                        {
                            Image_ID.Image = imageShow.Bitmap;
                            Image_Store_FaceIn = new Image<Bgr, byte>(imageShow.Bitmap);
                            isFinishFaceIn = true;
                            IsExistFace = false;
                        }
                    }
                    catch
                    {
                        //do nothing as parrellel loop buggy
                        //No action as the error is useless, it is simply an error in 
                        //no data being there to process and this occurss sporadically 
                    }
                });
            }
            if(isFinishFaceIn)
                Image_ID.Image = Image_Store_FaceIn.Bitmap;

        }
        private void timerPlateIn_Tick(object sender, EventArgs e)
        {
            Image<Bgr, byte> capPrImageIn = null;
            if (_isDemo)
            {
                if (_currentFramePlateIn < _totalFramePlateIn)
                {
                    capPrImageIn = _capturePlateIn.QueryFrame().ToImage<Bgr, byte>();
                    _currentFramePlateIn++;
                    Image_Xe_Vao_Truoc.Image = capPrImageIn.Resize(Image_Xe_Vao_Truoc.Width, Image_Xe_Ra_Truoc.Height, Inter.Cubic).Bitmap;
                    Image_Xe_Vao_Truoc.Update();
                }
                //await Task.Delay(1000 / _fpsFaceIn / 4);
            }
            else if(_capturePlateIn.IsOpened)
            {
                capPrImageIn = _capturePlateIn.QueryFrame().ToImage<Bgr, byte>();
                Image_Xe_Vao_Truoc.Image = capPrImageIn.Resize(Image_Xe_Vao_Truoc.Width, Image_Xe_Ra_Truoc.Height, Inter.Cubic).Bitmap;
            }

            if (capPrImageIn != null && isFinishFaceIn)
            {
                lbNotify.Text = Global.TEXT_DETECT_PLATE;
                try
                {
                    Image Plate_Draw;
                    Image<Bgr, byte> PlateResize;
                    _currentPRDT = DateTime.Now;
                    _lpr.ProcessImage(capPrImageIn.Bitmap, out Plate_Draw, out PlateResize, _plate);
                    if (_lpr.PlateImagesList.Count != 0)
                    {

                        _isFinishPlateIn = true;
                        Bitmap imagePlate = null;
                        string biensoxe = _lpr.GetRecoginatinon(_lpr.PlateImagesList, out imagePlate);
                        Image_Plate.Image = (new Image<Bgr,byte>(imagePlate).Resize(Image_Plate.Width, Image_Plate.Height,Inter.Cubic)).Bitmap;
                        #region Xử lý khi đã nhận diện biển số thành công (Lưu ảnh và lưu vào cơ sở dữ liệu
                        if (imagePlate != null)
                        {

                            DateTime currentDT = DateTime.Now;
                            string stringDT = currentDT.Year.ToString() + currentDT.Month.ToString() 
                                + currentDT.Day.ToString() + currentDT.Hour.ToString() 
                                + currentDT.Minute.ToString() + currentDT.Second.ToString();
                            string pathface = Global.PATH_FACE_IMAGE+ stringDT+".jpg";
                            string pathPlate = Global.PATH_PLATE_IMAGE + stringDT + ".jpg"; ;
                            string pathFull =  Global.PATH_FULL_IMAGE + stringDT + ".jpg"; ;
                            if(biensoxe.Length > 7)
                            {
                                lbNotify.Text = Global.TEXT_SAVE;
                                if (IsExistFace)
                                {
                                    _dataContext.CreateGoLeave(Int32.Parse(lbID.Text), biensoxe, pathface, pathPlate, pathFull);
                                    _captureFaceIn.QueryFrame().ToImage<Bgr, byte>().Save(pathFull);
                                    imagePlate.Save(pathPlate);
                                    Image_Store_FaceIn.Save(pathface);
                                    string OCR = biensoxe;
                                    lbPlate.Text = OCR;
                                    lbNotify.Text = Global.TEXT_SUCCESS;
                                    isFinishFaceIn = false;
                                }
                                else
                                {
                                    _dataContext.CreateMember(biensoxe, pathface, pathPlate, pathFull);
                                    _captureFaceIn.QueryFrame().ToImage<Bgr, byte>().Save(pathFull);
                                    imagePlate.Save(pathPlate);
                                    Image_Store_FaceIn.Save(pathface);
                                    string OCR = biensoxe;
                                    lbPlate.Text = OCR;
                                    lbNotify.Text = Global.TEXT_SUCCESS;
                                    isFinishFaceIn = false;
                                }
                                _isFinishPlateIn = true;
                                _startWatingTimeFaceIn = DateTime.Now;
                                _dataContext.LoadTraningFace(out listFace, out listLabel);
                                _recognition.Update(listFace, listLabel);
                            }
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        
        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
            this.Close();
        }

        private void timerFaceOut_Tick(object sender, EventArgs e)
        {
            Image<Bgr, byte> capFaceImageOut = null;

            #region Load frame Face
            if (_isDemo)
            {
                if (_currentFrameFaceIn < _totalFrameFaceIn)
                {
                    capFaceImageOut = _captureFaceOut.QueryFrame().ToImage<Bgr, byte>();
                    Image_Xe_Ra_Sau.Image = capFaceImageOut.Resize(Image_Xe_Vao_Sau.Width, Image_Xe_Ra_Truoc.Height, Inter.Cubic).Bitmap;
                    _currentFrameFaceIn++;
                    Image_Xe_Ra_Sau.Update();
                }

                //await Task.Delay(1000 / _fpsFaceIn / 4);
            }
            else if (_captureFaceOut.IsOpened)
            {
                capFaceImageOut = _captureFaceOut.QueryFrame().ToImage<Bgr, byte>();
                Image_Xe_Vao_Sau.Image = capFaceImageOut.Resize(Image_Xe_Vao_Sau.Width, Image_Xe_Ra_Truoc.Height, Inter.Cubic).Bitmap;
                //Image_Xe_Vao_Sau.Update();
            }
            #endregion

            if (capFaceImageOut != null && !isFinishFaceIn)
            {
                gray_frameFace = capFaceImageOut.Convert<Gray, Byte>();

                //Face Detector
                Rectangle[] facesDetected = _face.DetectMultiScale(gray_frameFace, 1.3, 10, new Size(70, 70), Size.Empty);

                //Action for each element detected
                Parallel.For(0, facesDetected.Length, i =>
                {
                    try
                    {
                        //facesDetected[i].X += (int)(facesDetected[i].Height * 0.15);
                        //facesDetected[i].Y += (int)(facesDetected[i].Width * 0.22);
                        //facesDetected[i].Height -= (int)(facesDetected[i].Height * 0.3);
                        //facesDetected[i].Width -= (int)(facesDetected[i].Width * 0.35);
                        Image<Gray, byte> result = capFaceImageOut.Copy(facesDetected[i]).Convert<Gray, byte>().Resize(100, 100, Inter.Cubic);
                        Image<Bgr, byte> imageShow = capFaceImageOut.Copy(facesDetected[i]).Resize(100, 67, Inter.Cubic);

                        result._EqualizeHist();
                        //draw the face detected in the 0th (gray) channel with blue color
                        capFaceImageOut.Draw(facesDetected[i], new Bgr(Color.Red), 2);

                        _startPRDT = DateTime.Now;
                        if (_recognition.IsTrained)
                        {
                            if (countNotReFace <= 3)
                            {
                                string name = _recognition.Recognise(result);
                                int match_value = (int)_recognition.Get_Fisher_Distance;
                                if (name == "Unknown" || name == "")
                                {
                                    countNotReFace++;
                                    IsExistFace = false;
                                }
                                else
                                {
                                    Image_ID.Image = imageShow.Bitmap;
                                    isFinishFaceIn = true;
                                    lbID.Text = name;
                                    IsExistFace = true;
                                }
                            }
                            if (countNotReFace >= 3)
                            {
                                countNotReFace = 0;
                                isFinishFaceIn = true;
                            }
                        }
                        isFinishFaceIn = true;


                    }
                    catch
                    {
                        //do nothing as parrellel loop buggy
                        //No action as the error is useless, it is simply an error in 
                        //no data being there to process and this occurss sporadically 
                    }
                });
            }
        }

        private void timerPlateOut_Tick(object sender, EventArgs e)
        {

        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            HistoryForm historyForm = new HistoryForm();
            historyForm.Show();
        }

        private void btnAllow_Click(object sender, EventArgs e)
        {

        }
    }
}
