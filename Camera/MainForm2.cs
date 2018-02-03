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
        private CascadeClassifier _face = new CascadeClassifier(Application.StartupPath + "/haarcascade_frontalface_alt2.xml");//Our face detection method 
        private CascadeClassifier _plate = new CascadeClassifier(Application.StartupPath + "\\biensoxemay.xml");
        private Classifier_Train _recognition = new Classifier_Train(Classifier_Train.LoadClassifierType.Face);
        private Image<Gray, byte> _gray_frameFace = null; //grayscale current image aquired from webcam for processing
        private int _fpsPlate;

        //int current = 0;
        private VideoCapture _capturePlateIn = null;
        private VideoCapture _captureFaceIn = null; //This is our capture variable
        private VideoCapture _capturePlateOut = null;
        private VideoCapture _captureFaceOut = null; //This is our capture variable
        private int _countErrorPR = 0;
        private int _fpsFaceIn = 30;
        private int _fpsFaceOut = 30;
        private int _fpsPlateOut = 30;
        private int _fpsPlateIn = 30;
        #region variable demo
        private int _totalFramePlateIn;
        private int _totalFramePlateOut;
        private int _totalFrameFaceIn;
        private int _totalFrameFaceOut;
        private int _currentFramePlateIn = 0;
        private int _currentFramePlateOut = 0;
        private int _currentFrameFaceIn = 0;
        private int _currentFrameFaceOut = 0;

        private string _checkOCRPlateOut = "";
        private int _countNotReFace = 0;
        private const string PATH_PLATE_IN = "Demo\\XeVaoBienSo.mp4";
        private const string PATH_FACE_IN = "Demo\\XeVaoMat.mp4";
        private string _checkOCRPlateIn = "";
        private bool _isCheckPlateOut = false;

        private Image<Bgr, byte> _image_Store_FaceIn;
        private Image<Bgr, byte> _image_Store_FaceOut;
        private Image<Bgr, byte> _image_Store_PlateIn;
        private Image<Bgr, byte> _image_Store_PlateOut;
        private int _watingFaceIn;
        private DateTime _startWatingTimeFaceIn;
        private DateTime _startWatingTimeFaceOut;

        #endregion
        private LPR _lprPlateIn;
        private LPR _lprPlateOut;
        private const int DELAY_PR_DT = 400;
        private DataContext _dataContext = new DataContext();
        private SVM svm;
        private List<string> _listFace = new List<string>();
        private List<int> _listLabel = new List<int>();
        private bool _IsExistFaceIn = false;
        private bool _IsExistFaceOut = false;
        private bool _isCheckPlateIn = false;
        private bool _isEx = false;
        private bool _isDemo = false;
        private bool _isFinishFaceIn = false;
        private bool _isFinishFaceOut = false;
        private bool _isFinishPlateIn = false;
        private bool _isFinishPlateOut = false;
        private bool _isFinishIn = false;
        private bool _isFinishOut = false;
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
            _lprPlateIn = new LPR(svm);
            _lprPlateOut = new LPR(svm);
            lbPlate.Text = "";
            _dataContext.LoadTraningFace(out _listFace, out _listLabel);
            _recognition.Update(_listFace, _listLabel);
            lbCount.Text = _dataContext.GetCountGoOut().ToString();
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
                _capturePlateIn = new Emgu.CV.VideoCapture(1);
                _capturePlateIn.SetCaptureProperty(CapProp.FrameWidth, 640);
                _capturePlateIn.SetCaptureProperty(CapProp.FrameHeight, 480);

                _captureFaceIn = new Emgu.CV.VideoCapture(0);
                _captureFaceIn.SetCaptureProperty(CapProp.FrameWidth, 320);
                _captureFaceIn.SetCaptureProperty(CapProp.FrameHeight, 240);

                timerFaceIn.Enabled = true;
                timerPlateIn.Enabled = true;



                //_capturePlateOut = new Emgu.CV.VideoCapture(1);
                //_capturePlateOut.SetCaptureProperty(CapProp.FrameWidth, 640);
                //_capturePlateOut.SetCaptureProperty(CapProp.FrameHeight, 480);

                //_captureFaceOut = new Emgu.CV.VideoCapture(0);
                //_captureFaceOut.SetCaptureProperty(CapProp.FrameWidth, 320);
                //_captureFaceOut.SetCaptureProperty(CapProp.FrameHeight, 240);

                //timerFaceOut.Enabled = true;
                //timerPlateOut.Enabled = true;
            }
            #endregion

            //if (_captureFaceOut == null)
            //{
            //    Image_Xe_Ra_Truoc.Image = (new Image<Bgr, byte>("Picture\\camera_not_found.png")).Bitmap;
            //    Image_Xe_Ra_Truoc.Update();
            //}
            //if (_capturePlateOut == null)
            //{
            //    Image_Xe_Ra_Sau.Image = (new Image<Bgr, byte>("Picture\\camera_not_found.png")).Bitmap;
            //    Image_Xe_Ra_Sau.Update();
            //}
            //if (_captureFaceIn == null)
            //{
            //    Image_Xe_Vao_Truoc.Image = (new Image<Bgr, byte>("Picture\\camera_not_found.png")).Bitmap;
            //    Image_Xe_Vao_Truoc.Update();
            //}
            //if (_capturePlateIn == null)
            //{
            //    Image_Xe_Vao_Sau.Image = (new Image<Bgr, byte>("Picture\\camera_not_found.png")).Bitmap;
            //    Image_Xe_Vao_Sau.Update();
            //}
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
            _isFinishFaceIn = false;
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

            pVao.Location = new Point(0, 30);
            pVao.Size = new Size(Size.Width / 5 * 2, Size.Height-30);
            
            pInfo.Location = new Point(pVao.Size.Width, 5);
            pInfo.Size = new Size(Size.Width/5, Size.Height);
            pRa.Location = new Point(pInfo.Location.X + pInfo.Size.Width, 30);
            pRa.Size = new Size(Size.Width / 5 * 2, Size.Height-30);

            //Image_Xe_Ra_Truoc.Size = new Size(pRa.Size.Width - 20, (pRa.Size.Height-30) / 2 - 20);
            //Image_Xe_Ra_Truoc.Location = new Point(pRa.Location.X + 10, pRa.Location.Y + 30);
            //Image_Xe_Ra_Sau.Size = new Size(pRa.Size.Width - 20, (pRa.Size.Height-30) / 2 - 20);
            //Image_Xe_Ra_Sau.Location = new Point(pRa.Location.X + 10, pRa.Location.Y + 30 + 20 + Image_Xe_Ra_Truoc.Height);

           // Image_Xe_Vao_Truoc.Size = new Size(pVao.Size.Width - 20, (pVao.Size.Height - 30) / 2 - 20);
           // Image_Xe_Vao_Truoc.Location = new Point(pVao.Location.X + 10, pVao.Location.Y + 30);
            //Image_Xe_Vao_Sau.Size = new Size(pVao.Size.Width - 20, (pVao.Size.Height - 30) / 2 - 20);
            //Image_Xe_Vao_Sau.Location = new Point(pVao.Location.X + 10, pVao.Location.Y + 30 + 20 + Image_Xe_Vao_Truoc.Height);
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

            if(_isFinishIn)
            { 
                DateTime currentDT = DateTime.Now;
                int subDT = currentDT.Subtract(_startWatingTimeFaceIn).Milliseconds;
                if (subDT > Global.WAITING_TIME_SECONDS && _isFinishPlateIn)
                {
                    _isFinishPlateIn = false;
                    _isFinishIn = false;

                    lbNotify.Text = Global.TEXT_DETECT_FACE;
                }
                else if(_isFinishPlateIn)
                {
                    lbNotify.Text = Global.TEXT_WAITING;
                }
            }
            if (capFaceImageIn != null && !_isFinishPlateIn)
            {
               // lbNotify.Text = Global.TEXT_DETECT_FACE;
                _gray_frameFace = capFaceImageIn.Convert<Gray, Byte>();

                //Face Detector
                Rectangle[] facesDetected = _face.DetectMultiScale(_gray_frameFace, 1.2);

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

                        if (_recognition.IsTrained)
                        {
                            if (_countNotReFace <= 3)
                            {
                                string name = _recognition.Recognise(result);
                                int match_value = (int)_recognition.Get_Fisher_Distance;
                                if (name == "Unknown" || name == "")
                                {
                                    _countNotReFace++;
                                    lbID.Text = "New user";
                                    _IsExistFaceIn = false;
                                }
                                else
                                {
                                    Image_ID.Image = imageShow.Bitmap;
                                    _image_Store_FaceIn = new Image<Bgr, byte>(imageShow.Bitmap);
                                    _isFinishFaceIn = true;
                                    lbID.Text = name;
                                    _IsExistFaceIn = true;
                                }
                            }
                            if (_countNotReFace >= 3)
                            {
                                Image_ID.Image = imageShow.Bitmap;
                                _image_Store_FaceIn = new Image<Bgr, byte>(imageShow.Bitmap);
                                _isFinishFaceIn = true;
                                lbID.Text = "New user";
                            }
                        }
                        else
                        {
                            Image_ID.Image = imageShow.Bitmap;
                            _image_Store_FaceIn = new Image<Bgr, byte>(imageShow.Bitmap);
                            _isFinishFaceIn = true;
                            _IsExistFaceIn = false;
                            lbID.Text = "New user";
                        }
                        Image_Xe_Vao_Sau.Image = capFaceImageIn.Resize(Image_Xe_Vao_Sau.Width, Image_Xe_Ra_Truoc.Height, Inter.Cubic).Bitmap;
                    }
                    catch
                    {
                        //do nothing as parrellel loop buggy
                        //No action as the error is useless, it is simply an error in 
                        //no data being there to process and this occurss sporadically 
                    }
                });
            }
            if(_isFinishFaceIn)
                Image_ID.Image = _image_Store_FaceIn.Bitmap;

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

            if (capPrImageIn != null && _isFinishFaceIn && !_isFinishIn)
            {
                lbNotify.Text = Global.TEXT_DETECT_PLATE;
                try
                {
                    Image Plate_Draw;
                    Image<Bgr, byte> PlateResize;
                    _lprPlateIn.ProcessImage(capPrImageIn.Bitmap, out Plate_Draw, out PlateResize, _plate);
                    if (_lprPlateIn.PlateImagesList.Count != 0)
                    {

                        _isFinishPlateIn = true;
                        Bitmap imagePlate = null;
                        string biensoxe = _lprPlateIn.GetRecoginatinon(_lprPlateIn.PlateImagesList, out imagePlate);
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
                                if (_checkOCRPlateIn != biensoxe)
                                {
                                    _isCheckPlateIn = false;
                                    _checkOCRPlateIn = biensoxe;
                                }
                                else
                                {
                                    lbNotify.Text = Global.TEXT_SAVE;
                                    _isCheckPlateIn = true;
                                }
                                lbPlate.Text = biensoxe;
                                if (_isCheckPlateIn)
                                {
                                    if (_IsExistFaceIn)
                                    {
                                        _dataContext.CreateGoLeave(Int32.Parse(lbID.Text), biensoxe, pathface, pathPlate, pathFull);
                                        _captureFaceIn.QueryFrame().ToImage<Bgr, byte>().Save(pathFull);
                                        imagePlate.Save(pathPlate);
                                        _image_Store_FaceIn.Save(pathface);
                                        string OCR = biensoxe;
                                        lbPlate.Text = OCR;
                                        lbNotify.Text = Global.TEXT_SUCCESS;
                                        _isFinishFaceIn = false;
                                        _isFinishIn = true;
                                        _startWatingTimeFaceIn = DateTime.Now;
                                    }
                                    else
                                    {
                                        _dataContext.CreateMember(biensoxe, pathface, pathPlate, pathFull);
                                        _captureFaceIn.QueryFrame().ToImage<Bgr, byte>().Save(pathFull);
                                        imagePlate.Save(pathPlate);
                                        _image_Store_FaceIn.Save(pathface);
                                        string OCR = biensoxe;
                                        lbPlate.Text = OCR;
                                        lbNotify.Text = Global.TEXT_SUCCESS;
                                        _isFinishFaceIn = false;
                                        _isFinishIn = true;
                                        _startWatingTimeFaceIn = DateTime.Now;
                                    }
                                    lbDirector.Text = Global.TEXT_DIRECTOR_GO;
                                    _dataContext.LoadTraningFace(out _listFace, out _listLabel);
                                    _recognition.Update(_listFace, _listLabel);
                                }

                                _isFinishPlateIn = true;
                              
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
                if (_currentFrameFaceOut < _totalFrameFaceOut)
                {
                    capFaceImageOut = _captureFaceOut.QueryFrame().ToImage<Bgr, byte>();
                    Image_Xe_Ra_Sau.Image = capFaceImageOut.Resize(pRa.Size.Width - 20, (pRa.Size.Height - 30) / 2 - 20, Inter.Cubic).Bitmap;
                    _currentFrameFaceOut++;
                    Image_Xe_Ra_Sau.Update();
                }

                //await Task.Delay(1000 / _fpsFaceIn / 4);
            }
            else if (_captureFaceOut.IsOpened)
            {
                capFaceImageOut = _captureFaceOut.QueryFrame().ToImage<Bgr, byte>();
                Image_Xe_Ra_Sau.Image = capFaceImageOut.Resize(Image_Xe_Ra_Sau.Width, Image_Xe_Ra_Sau.Height, Inter.Cubic).Bitmap;
               // Image_Xe_Ra_Sau.Location = new Point(pRa.Location.X + 10, pRa.Location.Y + 30 + 20 + Image_Xe_Ra_Truoc.Height);
                Image_Xe_Ra_Sau.Update();
            }
            #endregion

            if (_isFinishOut)
            {
                DateTime currentDT = DateTime.Now;
                int subDT = currentDT.Subtract(_startWatingTimeFaceOut).Milliseconds;
                if (subDT > Global.WAITING_TIME_SECONDS && _isFinishPlateOut)
                {
                    _isFinishPlateOut = false;
                    _isFinishOut = false;

                    lbNotify.Text = Global.TEXT_DETECT_FACE;
                }
                else if (_isFinishPlateOut)
                {
                    lbNotify.Text = Global.TEXT_WAITING;
                }
            }
            // MessageBox.Show(Image_Xe_Ra_Sau.Location.X.ToString() + Image_Xe_Ra_Sau.Location.Y.ToString());
            #region Xử lý và nhận diện hình ảnh
            if (capFaceImageOut != null && !_isFinishFaceOut)
            {
                _gray_frameFace = capFaceImageOut.Convert<Gray, Byte>();

                //Face Detector
                Rectangle[] facesDetected = _face.DetectMultiScale(_gray_frameFace, 1.3, 10, new Size(70, 70), Size.Empty);

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
                        Image<Bgr, byte> imageShow = capFaceImageOut.Copy(facesDetected[i]).Resize(Image_ID.Width, Image_ID.Height, Inter.Cubic);

                        result._EqualizeHist();
                        //draw the face detected in the 0th (gray) channel with blue color
                        capFaceImageOut.Draw(facesDetected[i], new Bgr(Color.Red), 2);

                        if (_recognition.IsTrained)
                        {
                            if (_countNotReFace <= 3)
                            {
                                string name = _recognition.Recognise(result);
                                int match_value = (int)_recognition.Get_Fisher_Distance;
                                if (name == "Unknown" || name == "")
                                {
                                    _countNotReFace++;
                                    _IsExistFaceOut = false;
                                }
                                else
                                {
                                    Image_ID.Image = imageShow.Bitmap;
                                    _image_Store_FaceOut = new Image<Bgr, byte>(imageShow.Bitmap);
                                    _isFinishFaceOut = true;
                                    lbID.Text = name;
                                    _IsExistFaceOut = true;
                                }
                            }
                            if (_countNotReFace >= 3)
                            {
                                Image_ID.Image = imageShow.Bitmap;
                                _image_Store_FaceOut = new Image<Bgr, byte>(imageShow.Bitmap);
                                _isFinishFaceOut = true;
                            }
                        }
                        else
                        {
                            Image_ID.Image = imageShow.Bitmap;
                            _image_Store_FaceOut = new Image<Bgr, byte>(imageShow.Bitmap);
                            _isFinishFaceOut = true;
                            _IsExistFaceOut = false;
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
            #endregion
        }

        private void timerPlateOut_Tick(object sender, EventArgs e)

        {
            Image<Bgr, byte> capPrImageOut = null;
            if (_isDemo)
            {
                if (_currentFramePlateOut < _totalFramePlateOut)
                {
                    capPrImageOut = _capturePlateOut.QueryFrame().ToImage<Bgr, byte>();
                    _currentFramePlateOut++;
                    Image_Xe_Ra_Truoc.Image = capPrImageOut.Resize(Image_Xe_Ra_Truoc.Width, Image_Xe_Ra_Truoc.Height, Inter.Cubic).Bitmap;
                    Image_Xe_Ra_Truoc.Update();
                }
                //await Task.Delay(1000 / _fpsFaceIn / 4);
            }
            else if (_capturePlateOut.IsOpened)
            {
                capPrImageOut = _capturePlateOut.QueryFrame().ToImage<Bgr, byte>();
                Image_Xe_Ra_Truoc.Image = capPrImageOut.Resize(Image_Xe_Ra_Truoc.Width, Image_Xe_Ra_Truoc.Height, Inter.Cubic).Bitmap;
            }

            if (capPrImageOut != null && _isFinishFaceOut && !_isFinishOut)
            {
                lbNotify.Text = Global.TEXT_DETECT_PLATE;
                try
                {
                    Image Plate_Draw;
                    Image<Bgr, byte> PlateResize;
                    _lprPlateOut.ProcessImage(capPrImageOut.Bitmap, out Plate_Draw, out PlateResize, _plate);
                    if (_lprPlateOut.PlateImagesList.Count != 0)
                    {

                        _isFinishPlateOut = true;
                        Bitmap imagePlate = null;
                        string biensoxe = _lprPlateOut.GetRecoginatinon(_lprPlateOut.PlateImagesList, out imagePlate);
                        Image_Plate.Image = (new Image<Bgr, byte>(imagePlate).Resize(Image_Plate.Width, Image_Plate.Height, Inter.Cubic)).Bitmap;
                        #region Xử lý khi đã nhận diện biển số thành công (Lưu ảnh và lưu vào cơ sở dữ liệu
                        if (imagePlate != null)
                        {

                            DateTime currentDT = DateTime.Now;
                            string stringDT = currentDT.Year.ToString() + currentDT.Month.ToString()
                                + currentDT.Day.ToString() + currentDT.Hour.ToString()
                                + currentDT.Minute.ToString() + currentDT.Second.ToString();
                            string pathface = Global.PATH_FACE_IMAGE + stringDT + ".jpg";
                            string pathPlate = Global.PATH_PLATE_IMAGE + stringDT + ".jpg"; ;
                            string pathFull = Global.PATH_FULL_IMAGE + stringDT + ".jpg"; ;
                            if (biensoxe.Length > 7)
                            {

                                if (_checkOCRPlateOut != biensoxe)
                                {
                                    _isCheckPlateOut = false;
                                    _checkOCRPlateOut = biensoxe;
                                }
                                else
                                {
                                    lbNotify.Text = Global.TEXT_SAVE;
                                    _isCheckPlateOut = true;
                                }
                                lbPlate.Text = biensoxe;
                                if (_isCheckPlateOut)
                                {
                                    if (_IsExistFaceOut)
                                    {
                                        _dataContext.CheckGoLeave(Int32.Parse(lbID.Text), biensoxe, pathface, pathPlate, pathFull);
                                        _captureFaceOut.QueryFrame().ToImage<Bgr, byte>().Save(pathFull);
                                        imagePlate.Save(pathPlate);
                                        _image_Store_FaceOut.Save(pathface);
                                        string OCR = biensoxe;
                                        lbPlate.Text = OCR;
                                        lbNotify.Text = Global.TEXT_SUCCESS;
                                        _isFinishFaceOut = false;
                                        _isFinishOut = true;
                                        _startWatingTimeFaceOut = DateTime.Now;
                                    }
                                    else
                                    {
                                        ReasonForm reasonForm = new ReasonForm(null, biensoxe,
                                            null, _captureFaceOut.QueryFrame().ToImage<Bgr, byte>(), 
                                            null, _image_Store_FaceOut, 
                                            null,new Image<Bgr, byte>(imagePlate));
                                        reasonForm.Show();
                                        _dataContext.CreateMember(biensoxe, pathface, pathPlate, pathFull);
                                        _captureFaceOut.QueryFrame().ToImage<Bgr, byte>().Save(pathFull);
                                        imagePlate.Save(pathPlate);
                                        _image_Store_FaceOut.Save(pathface);
                                        string OCR = biensoxe;
                                        lbPlate.Text = OCR;
                                        lbNotify.Text = Global.TEXT_SUCCESS;
                                        _isFinishFaceOut = false;
                                        _isFinishOut = true;
                                        _startWatingTimeFaceOut = DateTime.Now;
                                    }
                                    lbDirector.Text = Global.TEXT_DIRECTOR_OUT;
                                    _dataContext.LoadTraningFace(out _listFace, out _listLabel);
                                    _recognition.Update(_listFace, _listLabel);
                                }

                                _isFinishPlateOut = true;

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

        private void btnHistory_Click(object sender, EventArgs e)
        {
            HistoryForm historyForm = new HistoryForm();
            historyForm.Show();
        }

        private void btnAllow_Click(object sender, EventArgs e)
        {

        }

        private void btnSwitchCamera_Click(object sender, EventArgs e)
        {
            timerFaceIn.Enabled = false;
            timerPlateIn.Enabled = false;
            _captureFaceIn.Pause();
            _captureFaceIn.Dispose();
            _capturePlateIn.Pause();
            _capturePlateIn.Dispose();
            Thread.Sleep(500);
            _capturePlateOut = new Emgu.CV.VideoCapture(0);
            _capturePlateOut.SetCaptureProperty(CapProp.FrameWidth, 640);
            _capturePlateOut.SetCaptureProperty(CapProp.FrameHeight, 480);

            _captureFaceOut = new Emgu.CV.VideoCapture(1);
            _captureFaceOut.SetCaptureProperty(CapProp.FrameWidth, 320);
            _captureFaceOut.SetCaptureProperty(CapProp.FrameHeight, 240);

            
            timerFaceOut.Enabled = true;
            timerPlateOut.Enabled = true;
            
        }

        private void Image_Xe_Ra_Sau_Click(object sender, EventArgs e)
        {
        }

        private void btnAllow_Click_1(object sender, EventArgs e)
        {
            ReasonForm reasonForm = new ReasonForm(null,null,null,null,null,null,null,null);
            reasonForm.Show();
        }
    }
}
