using Auto_parking;
using Emgu.CV;
using Emgu.CV.CvEnum;
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
        Classifier_Train _recognition = new Classifier_Train();

        Image<Bgr, Byte> currentFrameFace; //current image aquired from webcam for display
        Image<Gray, byte> result, TrainedFace = null; //used to store the result image and trained face
        Image<Gray, byte> gray_frameFace = null; //grayscale current image aquired from webcam for processing

        

        private int _fpsPlate;

        static string PATH_OCR = "";
        OcrImage ocrImage;

        List<Image<Bgr, byte>> PlateImagesList = new List<Image<Bgr, byte>>();
        const int MARGIN_RECT = 4;
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
        DateTime startTime;
        DateTime endTime;
#endregion


        public bool isFinish = false;
        public bool isStartFace = false;
        public bool isStartPlate = false;
        public bool isEx = false;
        public bool isDemo = true;
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
            //this.Image_ID.Location = new System.Drawing.Point(69, 22);
            lbPlate.Text = "";
            if (isDemo)
            {
                _capturePlateIn = new Emgu.CV.VideoCapture("Demo\\XeRaBienSo.mp4");
                _capturePlateIn.SetCaptureProperty(CapProp.FrameWidth, 640);
                _capturePlateIn.SetCaptureProperty(CapProp.FrameHeight, 480);
                //_capturePlateIn.SetCaptureProperty(CapProp., 480);
                _fpsPlateIn = (Int32) _capturePlateIn.GetCaptureProperty(CapProp.Fps);
                _totalFramePlateIn = Convert.ToInt32(_capturePlateIn.GetCaptureProperty(CapProp.FrameCount)) - 4;

                _captureFaceIn = new Emgu.CV.VideoCapture("Demo\\XeRaMat.mp4");
                _captureFaceIn.SetCaptureProperty(CapProp.FrameWidth, 320);
                _captureFaceIn.SetCaptureProperty(CapProp.FrameHeight, 240);
                _fpsFaceIn = (Int32)_captureFaceIn.GetCaptureProperty(CapProp.Fps);
                _totalFrameFaceIn = (Int32) _captureFaceIn.GetCaptureProperty(CapProp.FrameCount) -4 ;
            }
            else
            {
                _capturePlateIn = new Emgu.CV.VideoCapture(1);
                _capturePlateIn.SetCaptureProperty(CapProp.FrameWidth, 640);
                _capturePlateIn.SetCaptureProperty(CapProp.FrameHeight, 480);
                _captureFaceIn = new Emgu.CV.VideoCapture(0);
                _captureFaceIn.SetCaptureProperty(CapProp.FrameWidth, 320);
                _captureFaceIn.SetCaptureProperty(CapProp.FrameHeight, 240);
            }
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
            Application.Idle += new EventHandler(FrameGrabber);
            startTime = DateTime.Now;
            //timer1.Interval = 1000 / _fpsFaceIn;
            //timer1.Tick += new EventHandler(timer1_Tick);
            //timer1.Start();
            
        }
        void Loading()
        {
            SplashScreenForm frm = new SplashScreenForm();
            Application.Run(frm);
        }


        async void FrameGrabber(object sender, EventArgs e)
        {
            try
            {
                Image<Bgr, byte> capPrImageIn = null;
                Image<Bgr, byte> capFaceImageIn = null;
                if (isDemo)
                {
                    if(_currentFrameFaceIn < _totalFrameFaceIn)
                    { 
                    
                        _currentFrameFaceIn++;
                    }
                    
                    if (_currentFramePlateIn < _totalFramePlateIn)
                    {
                        
                        _currentFramePlateIn++;
                    }
                    capFaceImageIn = _captureFaceIn.QueryFrame().ToImage<Bgr, byte>();
                    capPrImageIn = _capturePlateIn.QueryFrame().ToImage<Bgr, byte>();
                    Image_Xe_Vao_Truoc.Image = capPrImageIn.Resize(Image_Xe_Vao_Truoc.Width, Image_Xe_Ra_Truoc.Height, Inter.Cubic).Bitmap;
                    Image_Xe_Vao_Sau.Image = capFaceImageIn.Resize(Image_Xe_Vao_Sau.Width, Image_Xe_Ra_Truoc.Height, Inter.Cubic).Bitmap;
                    Image_Xe_Vao_Truoc.Update();
                    Image_Xe_Vao_Sau.Update();
                    await Task.Delay(1000 / _fpsFaceIn/4);
                }
                else
                {
                    capPrImageIn = _capturePlateIn.QueryFrame().ToImage<Bgr, byte>();
                    capFaceImageIn = _captureFaceIn.QueryFrame().ToImage<Bgr, byte>();
                    Image_Xe_Vao_Truoc.Image = capPrImageIn.Resize(Image_Xe_Vao_Truoc.Width, Image_Xe_Ra_Truoc.Height, Inter.Cubic).Bitmap;
                    Image_Xe_Vao_Sau.Image = capFaceImageIn.Resize(Image_Xe_Vao_Sau.Width, Image_Xe_Ra_Truoc.Height, Inter.Cubic).Bitmap;
                    Image_Xe_Vao_Truoc.Update();
                    Image_Xe_Vao_Sau.Update();
                }
                //if (isStart)
                //{
                //lbNotify.Text = "Đang Tiến Hành";
                if (capPrImageIn != null && isStartPlate)
                {
                    try
                    {

                        //Image_Xe_Vao_Truoc.Update();
                        Bitmap temp1;
                        string temp2, temp3;


                        endTime = DateTime.Now;
                        //if (endTime.Subtract(startTime).Milliseconds > 1000)
                        //{
                        //    _exPlate = true;
                        //    startTime = DateTime.Now;
                        //}
                        //Image<Bgr, byte> image = new Image<Bgr, byte>("ImageTest\\HV-002.bmp");
                        Reconize(capPrImageIn.Bitmap, out temp1, out temp2, out temp3);
                        //if (temp3 !="")
                        //{
                        Image_Plate.Image = new Image<Gray, byte>(temp1).Resize(100, 67, Inter.Cubic).Bitmap;
                        isStartPlate = false;
                        //}
                        //if (!_exPlate)
                        //    CvInvoke.Imshow(DateTime.Now.ToString(), new Image<Bgr, byte>(temp1));
                        //await Task.Delay(1000 / _fpsPlate);

                        //image_PR.Image = temp1;
                        if (temp3 == "")
                            lbPlate.Text = "Cannot recognize license plate !";
                        else
                            lbPlate.Text = temp3;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }

                if (capFaceImageIn != null && isStartFace)
                {
                    gray_frameFace = capFaceImageIn.Convert<Gray, Byte>();

                    //Face Detector
                    Rectangle[] facesDetected = _face.DetectMultiScale(gray_frameFace, 1.2, 10, new Size(50, 50), Size.Empty);

                    //Action for each element detected
                    Parallel.For(0, facesDetected.Length, i =>
                    {
                        try
                        {
                            //facesDetected[i].X += (int)(facesDetected[i].Height * 0.15);
                            //facesDetected[i].Y += (int)(facesDetected[i].Width * 0.22);
                            //facesDetected[i].Height -= (int)(facesDetected[i].Height * 0.3);
                            //facesDetected[i].Width -= (int)(facesDetected[i].Width * 0.35);

                            result = capFaceImageIn.Copy(facesDetected[i]).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.Inter.Cubic);
                            result._EqualizeHist();
                            //draw the face detected in the 0th (gray) channel with blue color
                            capFaceImageIn.Draw(facesDetected[i], new Bgr(Color.Red), 2);
                            //ToeicExam toeicExam = new ToeicExam();
                            //toeicExam.Show();
                            //Hide();
                            //if (_recognition.IsTrained)
                            //{
                            //    string name = _recognition.Recognise(result);
                            //    int match_value = (int)_recognition.Get_Fisher_Distance;

                            //    //Draw the label for each face detected and recognized
                            //    //currentFrame.Draw(name + " ", new Point(facesDetected[i].X - 2, facesDetected[i].Y - 2), Emgu.CV.CvEnum.FontFace.HersheyComplex, 1, new Bgr(Color.LightGreen));
                            //    //ADD_Face_Found(result, name, match_value);
                            //}
                            Image_ID.Image = result.Resize(100, 67, Inter.Cubic).Bitmap;
                            lbID.Text = "13520558";
                            isStartFace = false;

                        }
                        catch
                        {
                            //do nothing as parrellel loop buggy
                            //No action as the error is useless, it is simply an error in 
                            //no data being there to process and this occurss sporadically 
                        }
                    });
                }
                // }
                //if (!isStartFace && !isStartPlate)
                //{
                //isStart = false;
                //lbNotify.Text = "Đã lưu";
                //lbNotify.BackColor = System.Drawing.Color.Green;
                //}
                if (!isStartPlate && !isStartFace & isEx)
                    btnSave.Enabled = true;
            }
            catch (Exception ex){
                MessageBox.Show(ex.ToString());
            }
        }
        bool success = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                Image<Bgr, byte> capPrImageIn = null;
                Image<Bgr, byte> capFaceImageIn = null;
                if (isDemo)
                {
                    capPrImageIn = _capturePlateIn.QueryFrame().ToImage<Bgr, byte>().Rotate(90, new Bgr());
                    capFaceImageIn = _captureFaceIn.QueryFrame().ToImage<Bgr, byte>().Rotate(90, new Bgr());
                    Image_Xe_Vao_Truoc.Image = capPrImageIn.Resize(400, 260, Inter.Cubic).Rotate(90, new Bgr()).Bitmap;
                    Image_Xe_Vao_Sau.Image = capFaceImageIn.Resize(400, 260, Inter.Cubic).Rotate(90, new Bgr()).Bitmap;
                }
                else
                {
                    capPrImageIn = _capturePlateIn.QueryFrame().ToImage<Bgr, byte>();
                    capFaceImageIn = _captureFaceIn.QueryFrame().ToImage<Bgr, byte>();
                    Image_Xe_Vao_Truoc.Image = capPrImageIn.Resize(400, 260, Inter.Cubic).Bitmap;
                    Image_Xe_Vao_Sau.Image = capFaceImageIn.Resize(400, 260, Inter.Cubic).Bitmap;
                }
                    //if (isStart)
                //{
                    //lbNotify.Text = "Đang Tiến Hành";
                    if (capPrImageIn != null && isStartPlate)
                    {
                        try
                        {

                            Image_Xe_Vao_Truoc.Update();
                            Bitmap temp1;
                            string temp2, temp3;


                            endTime = DateTime.Now;
                            //if (endTime.Subtract(startTime).Milliseconds > 1000)
                            //{
                            //    _exPlate = true;
                            //    startTime = DateTime.Now;
                            //}
                            //Image<Bgr, byte> image = new Image<Bgr, byte>("ImageTest\\HV-002.bmp");
                            Reconize(capPrImageIn.Bitmap, out temp1, out temp2, out temp3);
                            //if (temp3 !="")
                            //{
                            Image_Plate.Image = new Image<Gray, byte>(temp1).Resize(100, 67, Inter.Cubic).Bitmap;
                            isStartPlate = false;
                            //}
                            //if (!_exPlate)
                            //    CvInvoke.Imshow(DateTime.Now.ToString(), new Image<Bgr, byte>(temp1));
                            //await Task.Delay(1000 / _fpsPlate);

                            //image_PR.Image = temp1;
                            if (temp3 == "")
                                lbPlate.Text = "Cannot recognize license plate !";
                            else
                                lbPlate.Text = temp3;
                        }
                        catch (Exception ex)
                        { }
                    }

                    if (capFaceImageIn != null && isStartFace)
                    {
                        gray_frameFace = capFaceImageIn.Convert<Gray, Byte>();

                        //Face Detector
                        Rectangle[] facesDetected = _face.DetectMultiScale(gray_frameFace, 1.2, 10, new Size(50, 50), Size.Empty);

                        //Action for each element detected
                        Parallel.For(0, facesDetected.Length, i =>
                        {
                            try
                            {
                                //facesDetected[i].X += (int)(facesDetected[i].Height * 0.15);
                                //facesDetected[i].Y += (int)(facesDetected[i].Width * 0.22);
                                //facesDetected[i].Height -= (int)(facesDetected[i].Height * 0.3);
                                //facesDetected[i].Width -= (int)(facesDetected[i].Width * 0.35);

                                result = capFaceImageIn.Copy(facesDetected[i]).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.Inter.Cubic);
                                result._EqualizeHist();
                                //draw the face detected in the 0th (gray) channel with blue color
                                capFaceImageIn.Draw(facesDetected[i], new Bgr(Color.Red), 2);
                                //ToeicExam toeicExam = new ToeicExam();
                                //toeicExam.Show();
                                //Hide();
                                //if (_recognition.IsTrained)
                                //{
                                //    string name = _recognition.Recognise(result);
                                //    int match_value = (int)_recognition.Get_Fisher_Distance;

                                //    //Draw the label for each face detected and recognized
                                //    //currentFrame.Draw(name + " ", new Point(facesDetected[i].X - 2, facesDetected[i].Y - 2), Emgu.CV.CvEnum.FontFace.HersheyComplex, 1, new Bgr(Color.LightGreen));
                                //    //ADD_Face_Found(result, name, match_value);
                                //}
                                Image_ID.Image = result.Resize(100, 67, Inter.Cubic).Bitmap;
                                lbID.Text = "13520558";
                                isStartFace = false;

                            }
                            catch
                            {
                                //do nothing as parrellel loop buggy
                                //No action as the error is useless, it is simply an error in 
                                //no data being there to process and this occurss sporadically 
                            }
                        });
                    }
                // }
                //if (!isStartFace && !isStartPlate)
                //{
                //isStart = false;
                //lbNotify.Text = "Đã lưu";
                //lbNotify.BackColor = System.Drawing.Color.Green;
                //}
                if (!isStartPlate && !isStartFace & isEx)
                    btnSave.Enabled = true;
            }
            catch { }
        }
        

        public void ProcessImage(Bitmap image)
        {
            PlateImagesList.Clear();
            PlateTextList.Clear();
            FindLicensePlate(image, out Plate_Draw);

        }
        public void FindLicensePlate(Bitmap image, out Image plateDraw)
        {
            plateDraw = null;
            Image<Bgr, byte> frame;
            bool isface = false;
            Bitmap src;
            //pictureBox2.Image = new Image<Gray, byte>(image).ToBitmap();
            Image dst = image;
            //HaarCascade haar = new HaarCascade(Application.StartupPath + "\\output-hv-33-x25.xml");
            for (float i = 0; i <= 20; i = i + 3)
            {
                for (float s = -1; s <= 1 && s + i != 1; s += 2)
                {
                    src = RotateImage(dst, i * s);
                    PlateImagesList.Clear();
                    frame = new Image<Bgr, byte>(src);
                    using (Image<Gray, byte> grayframe = new Image<Gray, byte>(src))
                    {
                        CascadeClassifier cascadeClassifier = new CascadeClassifier(Application.StartupPath + "\\output-hv-33-x25.xml");


                        Rectangle[] faces = cascadeClassifier.DetectMultiScale(grayframe, 1.1, 8, new Size(0, 0));
                        foreach (var face in faces)
                        {
                            Image<Bgr, byte> tmp = frame.Copy();
                            tmp.ROI = face;

                            frame.Draw(face, new Bgr(Color.Blue), 2);

                            PlateImagesList.Add(tmp);
                            isface = true;
                        }
                        if (isface)
                        {
                            Image<Bgr, byte> showimg = frame.Clone();
                            plateDraw = (Image)showimg.ToBitmap();
                            //showimg = frame.Resize(imageBox1.Width, imageBox1.Height, 0);
                            //pictureBox1.Image = showimg.ToBitmap();
                            //IF.pictureBox2.Image = showimg.ToBitmap();
                            if (PlateImagesList.Count > 1)
                            {
                                for (int k = 1; k < PlateImagesList.Count; k++)
                                {
                                    if (PlateImagesList[0].Width < PlateImagesList[k].Width)
                                    {
                                        PlateImagesList[0] = PlateImagesList[k];
                                    }
                                }
                            }
                            PlateImagesList[0] = PlateImagesList[0].Resize(400, 400, Inter.Linear);
                            return;
                        }

                        //CvInvoke.Imshow("12345", PlateImagesList[0]);
                    }
                }
            }


        }
        public static Bitmap RotateImage(Image image, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            PointF offset = new PointF((float)image.Width / 2, (float)image.Height / 2);

            //create a new empty bitmap to hold rotated image
            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(rotatedBmp);

            //Put the rotation point in the center of the image
            g.TranslateTransform(offset.X, offset.Y);

            //rotate the image
            g.RotateTransform(angle);

            //move the image back
            g.TranslateTransform(-offset.X, -offset.Y);

            //draw passed in image onto graphics object
            g.DrawImage(image, new PointF(0, 0));

            return rotatedBmp;
        }

        private void Reconize(Bitmap link, out Bitmap hinhbienso, out string bienso, out string bienso_text)
        {
            hinhbienso = null;
            bienso = "";
            bienso_text = "";
            ProcessImage(link);
            if (_exPlate)
            {
                if (PlateImagesList.Count != 0)
                {
                    Image<Bgr, byte> src = new Image<Bgr, byte>(PlateImagesList[0].ToBitmap());
                    Bitmap grayframe;
                    FindContours con = new FindContours();
                    List<Rectangle> listRect = new List<Rectangle>();
                    List<Mat> listMat = new List<Mat>();
                    Bitmap color;

                    Image<Gray, byte> demo;
                    //CvInvoke.Imshow("tEST",src);
                    //Image<Bgr, byte> imageAAA = new Image<Bgr, byte>("D:\\Test\\Test\\1.jpg");

                    IdentifyContours(src.Bitmap, out grayframe, out listRect, out listMat);
                    hinhbienso = grayframe;
                    //int c = con.IdentifyContours(src.ToBitmap(), 50, false, out grayframe, out color, out listRect);
                    ////int z = con.count;
                    //pictureBox_BiensoVAO.Image = color;
                    //IF.pictureBox1.Image = color;
                    //hinhbienso = Plate_Draw;
                    //pictureBox_BiensoRA.Image = grayframe;
                    //IF.pictureBox3.Image = grayframe;
                    ////textBox2.Text = c.ToString();
                    Image<Gray, byte> dst = new Image<Gray, byte>(grayframe);
                    //dst = dst.Dilate(2);
                    //dst = dst.Erode(3);
                    grayframe = dst.ToBitmap();
                    //pictureBox2.Image = grayframe.Clone(listRect[2], grayframe.PixelFormat);
                    string zz = "";
                    // Plate recoginatinon
                    _exPlate = false;
                    
                    if (listMat.Count == 7)
                    {
                        for (int i = 0; i <= 3; i++)
                        {

                            ocrImage = new OcrImage(listMat[i], PATH_OCR);
                            ocrImage.SetOcr(OcrImage.TypeOcr.BOTH);
                            string cs = ocrImage.Recoginatinon();
                            //char cs = character_recognition(listMat[i]);
                            zz += cs;
                            //IList<IndecesMapping> list = flann.Reconize(dst);
                            //string fileName = list.OrderByDescending(p => p.Similarity).First().fileName;
                            //int Similar = list.OrderByDescending(p => p.Similarity).First().Similarity;
                            //CvInvoke.Imshow (i.ToString(), new Image<Bgr,byte>(
                            //    fileName));
                            //flann.ResetSimilarity();
                            //text_recognition.push_back(result);
                            //System::String ^ str = gcnew System::String(result.c_str()); // Convert std string to System String
                            //textBox1->Text += str;
                        }
                        for (int i = 4; i <= 6; i++)
                        {

                            ocrImage = new OcrImage(listMat[i], PATH_OCR);
                            ocrImage.SetOcr(OcrImage.TypeOcr.BOTH);
                            string cs = ocrImage.Recoginatinon();
                            //char cs = character_recognition(listMat[i]);
                            zz += cs;
                        }
                    }
                    else if (listMat.Count == 8)
                    {
                        for (int i = 0; i <= 4; i++)
                        {

                            ocrImage = new OcrImage(listMat[i], PATH_OCR);
                            ocrImage.SetOcr(OcrImage.TypeOcr.NUMBER);
                            string cs = ocrImage.Recoginatinon();
                            zz += cs;
                        }
                        for (int i = 5; i <= 7; i++)
                        {

                            ocrImage = new OcrImage(listMat[i], PATH_OCR);
                            ocrImage.SetOcr(OcrImage.TypeOcr.BOTH);
                            string cs = ocrImage.Recoginatinon();
                            zz += cs;
                        }
                    }
                    else if (listMat.Count == 9)
                    {
                        for (int i = 0; i <= 4; i++)
                        {

                            ocrImage = new OcrImage(listMat[i], PATH_OCR);
                            ocrImage.SetOcr(OcrImage.TypeOcr.NUMBER);
                            string cs = ocrImage.Recoginatinon();
                            zz += cs;
                        }
                        for (int i = 5; i <= 8; i++)
                        {

                            ocrImage = new OcrImage(listMat[i], PATH_OCR);
                            ocrImage.SetOcr(OcrImage.TypeOcr.BOTH);
                            string cs = ocrImage.Recoginatinon();
                            zz += cs;
                        }
                    }
                    else if (listMat.Count == 10)
                    {
                        for (int i = 0; i <= 4; i++)
                        {

                            ocrImage = new OcrImage(listMat[i], PATH_OCR);
                            ocrImage.SetOcr(OcrImage.TypeOcr.NUMBER);
                            string cs = ocrImage.Recoginatinon();
                            zz += cs;
                        }
                        for (int i = 5; i <= 9; i++)
                        {

                            ocrImage = new OcrImage(listMat[i], PATH_OCR);
                            ocrImage.SetOcr(OcrImage.TypeOcr.BOTH);
                            string cs = ocrImage.Recoginatinon();
                            zz += cs;
                        }
                    }
                    else if(listMat.Count == 6)
                    {
                        for (int i = 0; i < listMat.Count; i++)
                        {

                            ocrImage = new OcrImage(listMat[i], PATH_OCR);
                            ocrImage.SetOcr(OcrImage.TypeOcr.BOTH);
                            string cs = ocrImage.Recoginatinon();
                            zz += cs;
                        }
                        

                    }
                    //else
                        _exPlate = true;
                    string replacement = Regex.Replace(zz, @"\t|\n|\r", "");
                    //lbPlate.Text = replacement;
                    char[] arr = replacement.ToCharArray();
                    Array.Reverse(arr);
                    lbPlate.Text = bienso_text = new string(arr);

                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            isStartFace = true;
            isStartPlate = true;
            isEx = true;
        }


        private void btnFace_Click(object sender, EventArgs e)
        {
            isStartFace = true;
            isEx = true;
        }

        private void btnPlate_Click(object sender, EventArgs e)
        {
            isStartPlate = true;
            isEx = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        public void Reset()
        {
            Image_ID.Image = (new Image<Bgr, byte>("Picture\\avatar-sample.jpg")).Bitmap;
            isStartPlate = false;
            isStartFace = false;
            btnSave.Enabled = false;
            isEx = false;
            lbID.Text = "";
            lbPlate.Text = "";
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
            this.Close();
        }

        public int IdentifyContours(Bitmap colorImage, out Bitmap processedGray, out List<Rectangle> listRect, out List<Mat> listMat)
        {
            Image<Gray, byte> grayImage = new Image<Gray, byte>(colorImage);
            Image<Bgr, byte> bgrImageBackup = new Image<Bgr, byte>(colorImage);
            Image<Gray, byte> grayImageBackup = new Image<Gray, byte>(colorImage);
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            //grayImage = grayImage.Resize(400, 400, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            Mat binary = new Image<Gray, byte>(grayImage.Width, grayImage.Height).Mat;
            //pictureBox_BiensoVAO.Image = grayImage.Bitmap;
            Image<Bgr, byte> color = new Image<Bgr, byte>(colorImage);
            CvInvoke.AdaptiveThreshold(grayImage, binary, 255, AdaptiveThresholdType.GaussianC, ThresholdType.Binary, 55, 5);
            //Mat gray, binary;
            // CvInvoke.Imshow("binary",binary);
            //pictureBox_BiensoRA.Image = binary.Bitmap;
            Mat or_binary = binary.Clone();
            Mat _plate = binary.Clone();
            Mat hierachy = new Mat();
            Mat element = CvInvoke.GetStructuringElement(ElementShape.Cross, new Size(3, 3), new Point(0, 0));
            CvInvoke.Erode(binary, binary, element, new Point(0, 0), 1, BorderType.Constant, new MCvScalar());

            CvInvoke.Dilate(binary, binary, element, new Point(0, 0), 1, BorderType.Constant, new MCvScalar());
            CvInvoke.FindContours(binary, contours, hierachy, RetrType.Tree, ChainApproxMethod.ChainApproxSimple, new Point(0, 0));
            //if (contours.Size < 8)
            //    continue;
            int count = 0;
            //Mat sub_image = image(r);
            List<Mat> c = new List<Mat>();
            List<Rectangle> r_characters = new List<Rectangle>();
            for (int j = 0; j < contours.Size; ++j)
            {
                Rectangle sub_r = CvInvoke.BoundingRectangle(contours[j]);
                if (sub_r.Width > 50 && sub_r.Width < 160
                       && sub_r.Height > 80 && sub_r.Height < 160)
                {
                    Mat cj = new Mat(_plate, sub_r);
                    double ratio = (double)EmguCVExtension.count_pixel(cj) / (cj.Cols * cj.Rows);
                    if (ratio > 0.2 && ratio < 0.7)
                    {
                        r_characters.Add(new Rectangle(sub_r.X + MARGIN_RECT, sub_r.Y + MARGIN_RECT, sub_r.Width + MARGIN_RECT, sub_r.Height + MARGIN_RECT));
                        CvInvoke.Rectangle(grayImage, sub_r, new MCvScalar(0, 0, 255), 2, LineType.EightConnected, 0);
                    }
                }
            }
            //pictureBox_XeRA.Image = grayImage.Bitmap;
            Console.WriteLine(r_characters.Count());
            if (r_characters.Count >= 7)
            {
                //sap xep
                //for (int i = 0; i < r_characters.Count - 1; ++i)
                //{
                //    for (int j = i + 1; j < r_characters.Count; ++j)
                //    {
                //        Rectangle temp;
                //        if (r_characters[j].X < r_characters[i].X && r_characters[j].Y == r_characters[i].Y)
                //        {
                //            temp = r_characters[j];
                //            r_characters[j] = r_characters[i];
                //            r_characters[i] = temp;
                //        }
                //    }
                //}
                //CvInvoke.Imshow("asd", grayImage);
                List<Rectangle> listUp = new List<Rectangle>();
                List<Rectangle> listDown = new List<Rectangle>();
                bool r0 = false;
                for (int i = 0; i < r_characters.Count - 1; ++i)
                {

                    if (r_characters[0].Y <= r_characters[i + 1].Y + 30 && r_characters[0].Y + 30 > r_characters[i + 1].Y)
                    {
                        listUp.Add(r_characters[i + 1]);
                        if (!r0)
                        {
                            listUp.Add(r_characters[0]);
                            r0 = true;
                        }
                    }
                    else
                    {
                        listDown.Add(r_characters[i + 1]);
                        if (!r0)
                        {
                            listDown.Add(r_characters[0]);
                            r0 = true;
                        }
                    }
                }
                for (int i = 0; i < listDown.Count - 1; i++)
                {
                    for (int j = i + 1; j < listDown.Count; ++j)
                    {
                        Rectangle temp;
                        if (listDown[j].X > listDown[i].X)
                        {
                            temp = listDown[j];
                            listDown[j] = listDown[i];
                            listDown[i] = temp;
                        }
                    }
                }
                for (int i = 0; i < listUp.Count - 1; i++)
                {
                    for (int j = i + 1; j < listUp.Count; ++j)
                    {
                        Rectangle temp;
                        if (listUp[j].X > listUp[i].X)
                        {
                            temp = listUp[j];
                            listUp[j] = listUp[i];
                            listUp[i] = temp;
                        }
                    }
                }
                r_characters.Clear();
                r_characters.AddRange(listUp);
                r_characters.AddRange(listDown);
                for (int i = 0; i < r_characters.Count; ++i)
                {
                    Mat cj = new Mat(_plate, r_characters[i]);
                    //  CvInvoke.Imshow(i.ToString() + ".jpg", new Mat(_plate, r_characters[i]));
                    //CvInvoke.Imwrite(i.ToString()+".jpg", new Mat(binary, r_characters[i]));
                    c.Add(cj);
                }
            }
            listRect = r_characters;
            processedGray = grayImage.ToBitmap();
            listMat = c;
            return 1;

        }



    }
}
