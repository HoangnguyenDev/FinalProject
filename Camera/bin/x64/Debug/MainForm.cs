using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.ML;
using Emgu.CV.OCR;
using Emgu.CV.UI;
using Emgu.Util;
using System.Diagnostics;
using Emgu.CV.CvEnum;
using System.IO;
using System.IO.Ports;
using System.Collections;
using System.Threading;
using System.Media;
using System.Runtime.InteropServices;
using Emgu.CV.Util;
using EmguCV.Extension;
using static Camera.FLANN;
using System.Text.RegularExpressions;

namespace Camera
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        static string  PATH_OCR = "";
        OcrImage ocrImage;

        #region định nghĩa
        List<Image<Bgr, byte>> PlateImagesList = new List<Image<Bgr, byte>>();
        const int MARGIN_RECT = 4;
        Image Plate_Draw;
        List<string> PlateTextList = new List<string>();
        List<Rectangle> listRect = new List<Rectangle>();
        PictureBox[] box = new PictureBox[12];

        private string m_path = Application.StartupPath + @"\data\";
        private List<string> lstimages = new List<string>();
        private const string m_lang = "eng";

        //int current = 0;
        VideoCapture capture = null;
        #endregion


        #region di chuyển
        bool mouseDown = false;
        Point lastLocation;
        private void button_Leave(object sender, EventArgs e)
        {
            Button bsen = (Button)sender;
            bsen.ForeColor = Color.Black;
        }

        private void button_Enter(object sender, EventArgs e)
        {
            Button bsen = (Button)sender;
            bsen.ForeColor = Color.White;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
            this.Close();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (mouseDown == false && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                mouseDown = true;
                lastLocation = e.Location;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                //contextMenuStrip1.Show(this.DesktopLocation.X + e.X, this.DesktopLocation.Y + e.Y);	
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.SetDesktopLocation(this.DesktopLocation.X - lastLocation.X + e.X, this.DesktopLocation.Y - lastLocation.Y + e.Y);
                this.Update();
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
        private void panel1_MouseHover(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(15, 15, 15);
        }
        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(64, 64, 64);
        }
        #endregion

        ImageForm IF;
        private void MainForm_Load(object sender, EventArgs e)
        {
            capture = new Emgu.CV.VideoCapture(0);
            timer1.Enabled = true;

            IF = new ImageForm();


             m_path = System.Environment.CurrentDirectory + "\\";
            string[] ports = SerialPort.GetPortNames();
            for (int i = 0; i < box.Length; i++)
            {
                box[i] = new PictureBox();
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (IF.Visible == false)
            {
                IF.Show();
            }
            else
            {
                IF.Hide();
            }
        }
        bool success = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (success == true)
            {
                success = false;
                new Thread(() =>
                {
                    try
                    {
                        capture.SetCaptureProperty(CapProp.FrameWidth, 640);
                        capture.SetCaptureProperty(CapProp.FrameHeight, 480);
                        Image<Bgr, byte> cap = capture.QueryFrame().ToImage<Bgr,byte>();
                        if (cap != null)
                        {
                            MethodInvoker mi = delegate
                            {
                                try
                                {
                                    Bitmap bmp = cap.ToBitmap();
                                    pictureBox_WC.Image = bmp;
                                    IF.pictureBox4.Image = bmp;
                                    pictureBox_WC.Update();
                                    IF.pictureBox4.Update();
                                }
                                catch (Exception ex)
                                { }
                            };
                            if (InvokeRequired)
                                Invoke(mi);
                        }
                    }
                    catch (Exception) { }
                    success = true;
                }).Start();
                
            }
            
            
        }

        public void ProcessImage(string urlImage)
        {
            PlateImagesList.Clear();
            PlateTextList.Clear();
            FileStream fs = new FileStream(urlImage, FileMode.Open, FileAccess.Read);
            Image img = Image.FromStream(fs);
            Bitmap image = new Bitmap(img);
            //pictureBox2.Image = image;
            IF.pictureBox2.Image = image;
            fs.Close();

            FindLicensePlate4(image, out Plate_Draw);

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

        private string Ocr(Bitmap image_s, bool isFull, bool isNum = false)
        {
            string temp = "";
            Image<Gray, byte> src = new Image<Gray, byte>(image_s);
            double ratio = 1;
            while (true)
            {
                ratio = (double)CvInvoke.CountNonZero(src) / (src.Width * src.Height);
                if (ratio > 0.5) break;
                src = src.Dilate(2);
            }
            Bitmap image = src.ToBitmap();

            //TesseractProcessor ocr;
            //if (isFull)
            //    ocr = full_tesseract;
            //else if (isNum)
            //    ocr = num_tesseract;
            //else
            //    ocr = ch_tesseract;

            //int cou = 0;
            //ocr.Clear();
            //ocr.ClearAdaptiveClassifier();
            //temp = ocr.Apply(image);
            //while (temp.Length > 3)
            //{
            //    Image<Gray, byte> temp2 = new Image<Gray, byte>(image);
            //    temp2 = temp2.Erode(2);
            //    image = temp2.ToBitmap();
            //    ocr.Clear();
            //    ocr.ClearAdaptiveClassifier();
            //    temp = ocr.Apply(image);
            //    cou++;
            //    if (cou > 10)
            //    {
            //        temp = "";
            //        break;
            //    }
            //}
            return temp;

        }

        public void FindLicensePlate4(Bitmap image, out Image plateDraw)
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


                        Rectangle[] faces = cascadeClassifier.DetectMultiScale(grayframe,1.1,8,new Size(0, 0));
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
                            IF.pictureBox2.Image = showimg.ToBitmap();
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

        private void Reconize(string link, out Image hinhbienso, out string bienso, out string bienso_text)
        {
            for (int i = 0; i < box.Length; i++)
            {
                this.Controls.Remove(box[i]);
            }

            hinhbienso = null;
            bienso = "";
            bienso_text = "";
            ProcessImage(link);
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
                else if(listMat.Count == 10)
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
                else
                {
                    for (int i = 0; i < listMat.Count; i++)
                    {

                        ocrImage = new OcrImage(listMat[i], PATH_OCR);
                        ocrImage.SetOcr(OcrImage.TypeOcr.BOTH);
                        string cs = ocrImage.Recoginatinon();
                        zz += cs;
                    }
                   
                }
                string replacement = Regex.Replace(zz, @"\t|\n|\r", "");
                text_BiensoVAO.Text = replacement;
                char[] arr = replacement.ToCharArray();
                Array.Reverse(arr);
                text_BiensoVAO.Text = new string(arr);

            }

        }




        private char character_recognition(Mat img_character)
        {
            try
            {
                //Load SVM training file OpenCV 3.1
                /*Ptr<SVM> svmNew = SVM::create();
                svmNew = SVM::load("D:/svm.txt");*/

                SVM svm = new SVM();
                FileStorage file = new FileStorage("svm.txt", FileStorage.Mode.Read);
                svm.Read(file.GetNode("opencv_ml_svm"));
                char c = '*';

                List<float> feature = calculate_feature(img_character);
                // Open CV3.1
                Mat m = new Mat(30, 1, DepthType.Cv32F, 0);
                for (int i = 0; i < feature.Count; ++i)
                {
                    float temp = feature[i];

                    var element = new float[1];
                    element[0] = temp;
                    var target = element;
                    Marshal.Copy(target, 0, (IntPtr)((int)m.DataPointer + (0 * m.Cols + i) * m.ElementSize), 1);



                    //m.SetValue(0, i,temp);
                }
                //Mat m = Mat(number_of_feature,1, CV_32FC1);		// Open CV 2.4
                //for (size_t i = 0; i < feature.size(); ++i)
                //{
                //float temp = feature[i];
                //m.at<float>(i,0) = temp;
                //}
                //Mat testingSVMBud = CvInvoke.Imread("D:\\licPlaterec_Data\\licPlaterec_Data\\Data\\00\\1.jpg", 0);
                //Mat fV = CvInvoke.ExtractFeatures(testingSVMBud).clone();
                //float prediction = svm.Predict(testingSVMBud);
                //convert 2d to 1d
                //Mat testDataMat(1, 2, CV_32FC1, testingSVMBud);
                int ri = (int)(svm.Predict(m)); // Open CV 3.1
                                                  /*int ri = int(svmNew.predict(m));*/
                if (ri >= 0 && ri <= 9)
                    c = (char)(ri + 48); //ma ascii 0 = 48
                if (ri >= 10 && ri < 18)
                    c = (char)(ri + 55); //ma accii A = 5, --> tu A-H
                if (ri >= 18 && ri < 22)
                    c = (char)(ri + 55 + 2); //K-N, bo I,J
                if (ri == 22) c = 'P';
                if (ri == 23) c = 'S';
                if (ri >= 24 && ri < 27)
                    c = (char)(ri + 60); //T-V,  
                if (ri >= 27 && ri < 30)
                    c = (char)(ri + 61); //X-Z
                return c;
            }
            catch(Exception e)
            {
                string error = e.Message;
                MessageBox.Show(error);
                return '*';
            }


        }



        public int IdentifyContours(Bitmap colorImage,out Bitmap processedGray, out List<Rectangle> listRect, out List<Mat> listMat)
        {
            Image<Gray, byte> grayImage = new Image<Gray, byte>(colorImage);
            Image<Bgr,byte> bgrImageBackup = new Image<Bgr, byte>(colorImage);
            Image<Gray, byte> grayImageBackup = new Image<Gray, byte>(colorImage);
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            //grayImage = grayImage.Resize(400, 400, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            Mat binary = new Image<Gray, byte>(grayImage.Width, grayImage.Height).Mat;
            pictureBox_BiensoVAO.Image = grayImage.Bitmap;
            Image<Bgr, byte> color = new Image<Bgr, byte>(colorImage);
            CvInvoke.AdaptiveThreshold(grayImage, binary, 255, AdaptiveThresholdType.GaussianC, ThresholdType.Binary, 55, 5);
            //Mat gray, binary;
           // CvInvoke.Imshow("binary",binary);
            pictureBox_BiensoRA.Image = binary.Bitmap;
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
                    Mat cj =new Mat(_plate,sub_r);
                    double ratio = (double)EmguCVExtension.count_pixel(cj) / (cj.Cols * cj.Rows);
                    if (ratio > 0.2 && ratio < 0.7)
                    {
                        r_characters.Add(new Rectangle(sub_r.X + MARGIN_RECT, sub_r.Y + MARGIN_RECT, sub_r.Width + MARGIN_RECT, sub_r.Height + MARGIN_RECT));
                        CvInvoke.Rectangle(grayImage, sub_r,new MCvScalar(0, 0, 255), 2, LineType.EightConnected, 0);
                    }
                }
            }
            pictureBox_XeRA.Image = grayImage.Bitmap;
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
                List<Rectangle> listUp = new List<Rectangle>();
                List<Rectangle> listDown = new List<Rectangle>();
                bool r0 = false;
                for (int i = 0; i < r_characters.Count - 1; ++i)
                {

                    if (r_characters[0].Y <= r_characters[i + 1].Y + 30&& r_characters[0].Y + 30 > r_characters[i + 1].Y)
                    {
                        listUp.Add(r_characters[i+1]);
                        if (!r0)
                        {
                            listUp.Add(r_characters[0]);
                            r0 = true;
                        }
                    }
                    else
                    {
                        listDown.Add(r_characters[i+1]);
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
                    Mat cj = new Mat(_plate,r_characters[i]);
                  //  CvInvoke.Imshow(i.ToString() + ".jpg", new Mat(_plate, r_characters[i]));
                    //CvInvoke.Imwrite(i.ToString()+".jpg", new Mat(binary, r_characters[i]));
                    c.Add(cj);
                }
            }
            // Plate recoginatinon
            //for (int i = 0; i < c.Count; i++)
            //{
            //    string result;
            //    for (int j = 0; j < c[i].; ++j)
            //    {

            //        char cs = character_recognition(c[i]);
            //        //result.push_back(cs);

            //    }
            //    //text_recognition.push_back(result);
            //    //System::String ^ str = gcnew System::String(result.c_str()); // Convert std string to System String
            //    //textBox1->Text += str;
            //}

            //rectangle(image, r, Scalar(0, 255, 0), 2, 8, 0);
            listRect = r_characters;
            processedGray = grayImage.ToBitmap();
            listMat = c;
            return 1;

        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            //while (true) ;
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image (*.bmp; *.jpg; *.jpeg; *.png) |*.bmp; *.jpg; *.jpeg; *.png|All files (*.*)|*.*||";
            dlg.InitialDirectory = Application.StartupPath + "\\ImageTest";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            string startupPath = dlg.FileName;

            Image temp1;
            string temp2, temp3;
            pictureBox_XeVAO.Image = (new Image<Bgr,byte>(dlg.FileName)).Bitmap;
            Reconize(startupPath, out temp1, out temp2, out temp3);
           
            //if (temp3 == "")
            //    text_BiensoVAO.Text = "ko nhận dạng dc biển số";
            //else
            //    text_BiensoVAO.Text = temp3;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (capture != null)
            {
                timer1.Enabled = false;
                pictureBox_XeRA.Image = null;
                IF.pictureBox2.Image = null;
                capture.QueryFrame().Save("aa.bmp");
                FileStream fs = new FileStream(m_path + "aa.bmp", FileMode.Open, FileAccess.Read);
                Image temp = Image.FromStream(fs);
                fs.Close();
                pictureBox_XeRA.Image = temp;
                IF.pictureBox2.Image = temp;
                pictureBox_XeRA.Update();
                IF.pictureBox2.Update();
                Image temp1;
                string temp2, temp3;
                Reconize(m_path + "aa.bmp", out temp1, out temp2, out temp3);
                pictureBox_XeVAO.Image = temp1;
                if(temp3 == "")
                    text_BiensoVAO.Text = "ko nhận dạng dc biển số";
                else
                    text_BiensoVAO.Text = temp3;

                timer1.Enabled = true;
            }
            
        }

        #region RESIZE
        private void MainForm_Resize(object sender, EventArgs e)
        {
            //Thread.Sleep(100);
            //Size a = Size;
            panel1.Size = new Size(Size.Width, panel1.Size.Height);
            button1.Location = new Point(panel1.Size.Width - button1.Size.Width, button1.Location.Y);
            button6.Location = new Point(button1.Location.X - button6.Size.Width - 1, button6.Location.Y);
            button8.Location = new Point(button6.Location.X - button8.Size.Width - 1, button8.Location.Y);

            panel_VAO.Location = new Point(0, 0);
            panel_VAO.Size = new Size((Size.Width - 184) / 2, Size.Height);
            panel_RA.Location = new Point(panel_VAO.Size.Width, 0);
            panel_RA.Size = new Size((Size.Width - 184) / 2, Size.Height);
            panel_WC.Location = new Point(panel_VAO.Size.Width + panel_RA.Size.Width, 0);
            panel_WC.Size = new Size(184, Size.Height);

            panel5.Location = new Point(Size.Width - panel5.Width, Size.Height - panel5.Height);

            pictureBox_XeVAO.Size = new Size(panel_VAO.Size.Width * 294 / 306, panel_VAO.Size.Width * 260 / 306);
            pictureBox_XeVAO.Location = new Point(panel_VAO.Size.Width / 2 - pictureBox_XeVAO.Size.Width / 2, pictureBox_XeVAO.Location.Y);
            groupBox1.Location = new Point(groupBox1.Location.X, pictureBox_XeVAO.Location.Y + pictureBox_XeVAO.Size.Height + 6);

            int h = panel_VAO.Height - groupBox1.Location.Y - 10;
            int w = h * 294 / 194;
            if( h > 194 && w < panel_VAO.Width - 12)
                groupBox1.Size = new Size(w, h);
            else
            {
                w = panel_VAO.Width - 12;
                h = w * 194 / 294;
                if(w > 294 && h < panel_VAO.Height - groupBox1.Location.Y - 10)
                    groupBox1.Size = new Size(w, h);
            }
            pictureBox_BiensoVAO.Size = new Size(groupBox1.Size.Width - 14 - pictureBox_BiensoVAO.Location.X
                , groupBox1.Size.Height - 67 - pictureBox_BiensoVAO.Location.Y);

            ///////////////////////

            pictureBox_XeRA.Size = new Size(panel_RA.Size.Width * 294 / 306, panel_RA.Size.Width * 260 / 306);
            pictureBox_XeRA.Location = new Point(panel_RA.Size.Width / 2 - pictureBox_XeRA.Size.Width / 2, pictureBox_XeRA.Location.Y);
            groupBox2.Location = new Point(groupBox2.Location.X, pictureBox_XeRA.Location.Y + pictureBox_XeRA.Size.Height + 6);

            h = panel_RA.Height - groupBox2.Location.Y - 10;
            w = h * 294 / 194;
            if (h > 194 && w < panel_RA.Width - 12)
                groupBox2.Size = new Size(w, h);
            else
            {
                w = panel_RA.Width - 12;
                h = w * 194 / 294;
                if (w > 294 && h < panel_RA.Height - groupBox2.Location.Y - 10)
                    groupBox2.Size = new Size(w, h);
            }
            pictureBox_BiensoRA.Size = new Size(groupBox2.Size.Width - 14 - pictureBox_BiensoRA.Location.X
                , groupBox2.Size.Height - 67 - pictureBox_BiensoRA.Location.Y);
        }
        private void resizeInGr(GroupBox gr, ref TextBox tx, ref Label lb, int dis_d, int dis_r_t, int dis_r_l, bool t)
        {
            if(dis_r_t < 0)
            {
                tx.Location = new Point(tx.Location.X, gr.Size.Height - dis_d);
                lb.Location = new Point(lb.Location.X, gr.Size.Height - dis_d);
            }
            else
            {
                tx.Location = new Point(gr.Size.Width - dis_r_t, gr.Size.Height - dis_d);
                lb.Location = new Point(gr.Size.Width - dis_r_l, gr.Size.Height - dis_d);
            }
            if (t)
                tx.Size = new Size(gr.Size.Width - 3 - tx.Location.X, tx.Size.Height);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void splitter1_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (mouseDown)
            {
                int w = Size.Width + (e.X - lastLocation.X);
                if (w < 796)
                {
                    w = 796;
                }
                this.Size = new Size(w, Size.Height);
                this.Update();
            }
        }

        private void splitter2_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                int h = Size.Height + (e.Y - lastLocation.Y);
                if (h < 504)
                {
                    h = 504;
                }
                this.Size = new Size(Size.Width, h);
                this.Update();
            }
        }

        private void panel5_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                int w = Size.Width + (e.X - lastLocation.X);
                if (w < 796)
                {
                    w = 796;
                }
                int h = Size.Height + (e.Y - lastLocation.Y);
                if (h < 504)
                {
                    h = 504;
                }
                this.Size = new Size(w, h);
                this.Update();
            }
        }

        #endregion

        #region WEBCAM
        WEBCAM[] cam = new WEBCAM[3];
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                PictureBox p = (PictureBox)sender;
                for (int i = 0; i < cam.Length; i++)
                {
                    if (cam[i] != null && cam[i].status == "run" && cam[i].pb == p.Name)
                    {
                        cam[i].Stop();
                        cam[i] = null;
                    }
                }
                ContextMenu m = new ContextMenu();
                List<string> ls = WEBCAM.get_all_cam();
                for(int i = 0; i<=2 & i < ls.Count; i++)
                {
                    m.MenuItems.Add(ls[i], (s, e2) => {
                        MenuItem menuItem = s as MenuItem;
                        ContextMenu owner = menuItem.Parent as ContextMenu;
                        PictureBox pb = (PictureBox)owner.SourceControl;
                        if (cam[menuItem.Index] != null && cam[menuItem.Index].status == "run")
                        {
                            cam[menuItem.Index].Stop();
                            //cam[menuItem.Index] = null;
                        }
                        cam[menuItem.Index] = new WEBCAM();
                        cam[menuItem.Index].Start(menuItem.Index);
                        cam[menuItem.Index].put_picturebox(pb.Name);
                    });
                }
                m.Show(p, new Point(e.X, e.Y));
            }
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < cam.Length; i++)
                {
                    if (cam[i] != null && cam[i].status == "run" && cam[i].image != null)
                    {
                        MethodInvoker mi = delegate
                        {
                            PictureBox pb = this.Controls.Find(cam[i].pb, true).FirstOrDefault() as PictureBox;
                            pb.Image = cam[i].image;
                            pb.Update();
                            pb.Invalidate();
                        };
                        if (InvokeRequired)
                        {
                            Invoke(mi);
                            return;
                        }
                            
                        PictureBox pb2 = this.Controls.Find(cam[i].pb, true).FirstOrDefault() as PictureBox;
                        pb2.Image = cam[i].image;
                        pb2.Update();
                        pb2.Invalidate();
                    }
                }
            }
            catch (Exception) { }
        }

        #endregion


        static List<float> calculate_feature(Mat src)
        {
            Mat img = new Mat();
            if (src.NumberOfChannels == 3)
            {
                CvInvoke.CvtColor(src, img, ColorConversion.Rgb2Gray);
                CvInvoke.Threshold(img, img, 100, 255, ThresholdType.Binary);
            }
            else
            {
                CvInvoke.Threshold(src, img, 100, 255, ThresholdType.Binary);
            }

            List<float> r = new List<float>();
            //vector<int> cell_pixel;
            CvInvoke.Resize(img, img,new Size(40, 40));
            int h = img.Rows / 4;
            int w = img.Cols / 4;
            int S = EmguCVExtension.count_pixel(img);
            int T = img.Cols * img.Rows;
            for (int i = 0; i < img.Rows; i += h)
            {
                for (int j = 0; j < img.Cols; j += w)
                {
                    Mat cell = new Mat(img,new Rectangle(i, j, h, w));
                    int s = EmguCVExtension.count_pixel(cell);
                    float f = (float)s / S;
                    r.Add(f);
                }
            }

            for (int i = 0; i < 16; i += 4)
            {
                float f = r[i] + r[i + 1] + r[i + 2] + r[i + 3];
                r.Add(f);
            }

            for (int i = 0; i < 4; ++i)
            {
                float f = r[i] + r[i + 4] + r[i + 8] + r[i + 12];
                r.Add(f);
            }

            r.Add(r[0] + r[5] + r[10] + r[15]);
            r.Add(r[3] + r[6] + r[9] + r[12]);
            r.Add(r[0] + r[1] + r[4] + r[5]);
            r.Add(r[2] + r[3] + r[6] + r[7]);
            r.Add(r[8] + r[9] + r[12] + r[13]);
            r.Add(r[10] + r[11] + r[14] + r[15]);
            r.Add(r[5] + r[6] + r[9] + r[10]);
            r.Add(r[0] + r[1] + r[2] + r[3] + r[4] + r[7] + r[8] + r[11] + r[12] + r[13] + r[14] + r[15]);

            return r; //32 feature
        }
    }
}
