using Auto_parking;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.ML;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Camera
{
    public partial class StepByStepForm : MetroForm
    {
        static string PATH_OCR = "";
        OcrImage ocrImage;

        #region định nghĩa
        List<Image<Bgr, byte>> PlateImagesList = new List<Image<Bgr, byte>>();
        const int MARGIN_RECT = 3;
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

        public SVM svm;
        ImageForm IF;
        public StepByStepForm()
        {
            //capture = new Emgu.CV.VideoCapture(0);
           // fLANN.LoadData("D:\\GitHub\\FinalProject\\Camera\\bin\\x64\\Debug\\Picture\\FULL\\LPR");
           // var list = fLANN.Reconize(new Image<Gray, byte>("D:\\GitHub\\FinalProject\\Camera\\bin\\x64\\Debug\\Picture\\FULL\\LPR\\BienSoXe.jpg"));
            
            IF = new ImageForm();
            svm = SVMExtension.Create();
            InitializeComponent();
            //SVMFuntion svm = new SVMFuntion();
            //svm.Training();
            //LoadSVMFromFile("OCR.xml");
            Test();
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

            FindLicensePlate(image, out Plate_Draw);

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
                pictureBox2.Image = PlateImagesList[0].ToBitmap();
                CvInvoke.Imwrite("BienSoXe.jpg", PlateImagesList[0].Mat);
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
                //if (listMat.Count == 7)
                //{
                //    for (int i = 0; i <= 3; i++)
                //    {

                //        ocrImage = new OcrImage(listMat[i], PATH_OCR);
                //        ocrImage.SetOcr(OcrImage.TypeOcr.BOTH);
                //        string cs = ocrImage.Recoginatinon();
                //        zz += cs;
                //    }
                //    for (int i = 4; i <= 6; i++)
                //    {

                //        ocrImage = new OcrImage(listMat[i], PATH_OCR);
                //        ocrImage.SetOcr(OcrImage.TypeOcr.BOTH);
                //        string cs = ocrImage.Recoginatinon();
                //        //char cs = character_recognition(listMat[i]);
                //        zz += cs;
                //    }
                //}
                //else if (listMat.Count == 8)
                //{
                //    for (int i = 0; i <= 4; i++)
                //    {

                //        ocrImage = new OcrImage(listMat[i], PATH_OCR);
                //        ocrImage.SetOcr(OcrImage.TypeOcr.NUMBER);
                //        string cs = ocrImage.Recoginatinon();
                //        zz += cs;
                //    }
                //    for (int i = 5; i <= 7; i++)
                //    {

                //        ocrImage = new OcrImage(listMat[i], PATH_OCR);
                //        ocrImage.SetOcr(OcrImage.TypeOcr.BOTH);
                //        string cs = ocrImage.Recoginatinon();
                //        zz += cs;
                //    }
                //}
                //else if (listMat.Count == 9)
                //{
                //    for (int i = 0; i <= 4; i++)
                //    {

                //        ocrImage = new OcrImage(listMat[i], PATH_OCR);
                //        ocrImage.SetOcr(OcrImage.TypeOcr.NUMBER);
                //        string cs = ocrImage.Recoginatinon();
                //        zz += cs;
                //    }
                //    for (int i = 5; i <= 8; i++)
                //    {

                //        ocrImage = new OcrImage(listMat[i], PATH_OCR);
                //        ocrImage.SetOcr(OcrImage.TypeOcr.BOTH);
                //        string cs = ocrImage.Recoginatinon();
                //        zz += cs;
                //    }
                //}
                //else if (listMat.Count == 10)
                //{
                //    for (int i = 0; i <= 4; i++)
                //    {

                //        ocrImage = new OcrImage(listMat[i], PATH_OCR);
                //        ocrImage.SetOcr(OcrImage.TypeOcr.NUMBER);
                //        string cs = ocrImage.Recoginatinon();
                //        zz += cs;
                //    }
                //    for (int i = 5; i <= 9; i++)
                //    {

                //        ocrImage = new OcrImage(listMat[i], PATH_OCR);
                //        ocrImage.SetOcr(OcrImage.TypeOcr.BOTH);
                //        string cs = ocrImage.Recoginatinon();
                //        zz += cs;
                //    }
                //}
                //else
                //{
                    for (int i = 0; i < listMat.Count; i++)
                    {

                        //ocrImage = new OcrImage(listMat[i], PATH_OCR);
                        //ocrImage.SetOcr(OcrImage.TypeOcr.BOTH);
                        char cs = Recoginatinon(svm, listMat[i]);
                        zz += cs;
                    }

                //}
                string replacement = Regex.Replace(zz, @"\t|\n|\r", "");
                txtLPR.Text = replacement;
                char[] arr = replacement.ToCharArray();
                Array.Reverse(arr);
                txtLPR.Text = new string(arr);
            }

        }

        public int IdentifyContours(Bitmap colorImage, out Bitmap processedGray, out List<Rectangle> listRect, out List<Mat> listMat)
        {
            Image<Gray, byte> grayImage = new Image<Gray, byte>(colorImage);
            Image<Bgr, byte> bgrImageBackup = new Image<Bgr, byte>(colorImage);
            Image<Gray, byte> grayImageBackup = new Image<Gray, byte>(colorImage);
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            //grayImage = grayImage.Resize(400, 400, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            Mat binary = new Image<Gray, byte>(grayImage.Width, grayImage.Height).Mat;
            pictureBox3.Image = grayImageBackup.Bitmap;
            Image<Bgr, byte> color = new Image<Bgr, byte>(colorImage);
            CvInvoke.AdaptiveThreshold(grayImage, binary, 255, AdaptiveThresholdType.MeanC, ThresholdType.Binary, 55, 5);
            //Mat gray, binary;
            // CvInvoke.Imshow("binary",binary);
            List<Mat> cb = new List<Mat>();
            Mat or_binary = binary.Clone();
            Mat _plate = binary.Clone();
            Mat hierachy = new Mat();
            Mat element = CvInvoke.GetStructuringElement(ElementShape.Cross, new Size(3, 3), new Point(0, 0));
            CvInvoke.Erode(binary, binary, element, new Point(0, 0), 1, BorderType.Constant, new MCvScalar());

            CvInvoke.Dilate(binary, binary, element, new Point(0, 0), 1, BorderType.Constant, new MCvScalar());
            pictureBox4.Image = binary.Bitmap;
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
                if (sub_r.Width > 20 && sub_r.Width < 160
                       && sub_r.Height > 80 && sub_r.Height < 160 && sub_r.Y > 0 && sub_r.X > 0)
                {
                    Mat cj = new Mat(_plate, sub_r);
                    double ratio = (double)EmguCVExtension.count_pixel(cj) / (cj.Cols * cj.Rows);
                    if (ratio > 0.2 && ratio < 0.7)
                    {
                        int X = sub_r.X - MARGIN_RECT > 0 ? sub_r.X - MARGIN_RECT : sub_r.X;
                        int Y = sub_r.Y - MARGIN_RECT > 0 ? sub_r.Y - MARGIN_RECT : sub_r.Y;
                        r_characters.Add(new Rectangle(X, Y, sub_r.Width + MARGIN_RECT*2, sub_r.Height + MARGIN_RECT*2));
                        //CvInvoke.Rectangle(grayImage, sub_r, new MCvScalar(0, 0, 255), 2, LineType.EightConnected, 0);
                        //CvInvoke.Rectangle(binary, sub_r, new MCvScalar(0, 0, 255), 2, LineType.EightConnected, 0);
                    }
                }
                               
                
            }
            pictureBox5.Image = binary.Bitmap;
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
                //r_characters.Clear();
                
                foreach (var item in c)
                {
                    double maxArea = 0.0;
                    VectorOfVectorOfPoint contours2 = new VectorOfVectorOfPoint();
                    Mat hierachy2 = new Mat();
                    CvInvoke.FindContours(item, contours2, hierachy2, RetrType.External, ChainApproxMethod.ChainApproxSimple, new Point(0, 0));
                    Mat result = new Mat();
                    int savedContour = -1;
                    for (int i = 0; i < contours2.Size; i++)
                    {
                        double area = CvInvoke.ContourArea(contours2[i]);
                        if (area > maxArea)
                        {
                            maxArea = area;
                            savedContour = i;
                        }
                    }
                    // Create mask
                    CvInvoke.DrawContours(item, contours2, savedContour, new MCvScalar(255));

                    // apply the mask:
                    cb.Add(item);
                   // r_characters.Add(new Rectangle(max.X - MARGIN_RECT, max.Y - MARGIN_RECT, max.Width + MARGIN_RECT * 2, max.Height + MARGIN_RECT * 2));
                    //CvInvoke.Rectangle(grayImage, sub_r, new MCvScalar(0, 0, 255), 2, LineType.EightConnected, 0);
                   // CvInvoke.Rectangle(binary, max, new MCvScalar(0, 0, 255), 2, LineType.EightConnected, 0);
                    
                }
                for (int f = 0; f < cb.Count; ++f)
                {
                   // Mat cj = new Mat(_plate, r_characters[f]);
                    //  CvInvoke.Imshow(i.ToString() + ".jpg", new Mat(_plate, r_characters[i]));
                    CvInvoke.Imwrite(f.ToString() + ".jpg", cb[f]);

                }
            }
           
 
            listRect = r_characters;
            processedGray = grayImage.ToBitmap();

            listMat = cb;
            return 1;

        }

        private void metroButton1_Click(object sender, EventArgs e)
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
            pictureBox1.Image = (new Image<Bgr, byte>(dlg.FileName)).Bitmap;
            Reconize(startupPath, out temp1, out temp2, out temp3);
        }

        public static Image<Bgr, Byte> Resize(Image<Bgr, Byte> im)
        {
            return im.Resize(64, 128, Inter.Linear);
        }
        public static float[] GetVector(Image<Bgr, Byte> im)
        {
            HOGDescriptor hog = new HOGDescriptor();    // with defaults values
            Image<Bgr, Byte> imageOfInterest = Resize(im);
            Point[] p = new Point[imageOfInterest.Width * imageOfInterest.Height];
            int k = 0;
            for (int i = 0; i < imageOfInterest.Width; i++)
            {
                for (int j = 0; j < imageOfInterest.Height; j++)
                {
                    Point p1 = new Point(i, j);
                    p[k++] = p1;
                }
            }

            return hog.Compute(imageOfInterest, new Size(8, 8), new Size(0, 0), p);
        }
        public void traning()
        {
            //GetVector - function from people detection file 
            //float[] hog = GetVector(new Image<Bgr, byte>(image));
            //SVM svm = new SVM();

          //  svm.TrainAuto(new TrainData(training_mat, Emgu.CV.ML.MlEnum.DataLayoutType.RowSample, lables));
        }
        public char Recoginatinon(SVM svm, Mat img_character)
        {

            List<float> feature = EmguCVExtension.calculate_feature(img_character);
            // Open CV3.1
            Mat m = new Mat(1, 32, DepthType.Cv32F, 1);
            for (int i = 0; i < feature.Count(); ++i)
            {
                float temp = feature[i];
                m.SetValue(0, i, temp);
            }
            char c = '*';

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
        public static SVM LoadSVMFromFile(String path)
        {
            SVM svm = new SVM();
            svm.Type = SVM.SvmType.CSvc; 
            FileStorage fs = new FileStorage(path, FileStorage.Mode.Read);
            svm.Read(fs.GetNode("opencv_ml_svm"));
            fs.ReleaseAndGetString();
            return svm;
        }
        //private void LoadTrainData()
        //{
        //    List<float[]> trainList = new List<float[]>();
        //    List<int> trainLabel = new List<int>();

        //    StreamReader reader = new StreamReader(TraingDataPath);

        //    string line = "";
        //    if (!File.Exists(TraingDataPath))
        //    {
        //        throw new Exception("File Not found");
        //    }

        //    while ((line = reader.ReadLine()) != null)
        //    {
        //        int firstIndex = line.IndexOf(',');
        //        int currentLabel = Convert.ToInt32(line.Substring(0, firstIndex));
        //        string currentData = line.Substring(firstIndex + 1);
        //        float[] data = currentData.Split(',').Select(x => float.Parse(x)).ToArray();

        //        trainList.Add(data);
        //        trainLabel.Add(currentLabel);

        //    }

        //    TrainData = new Matrix<float>(To2D<float>(trainList.ToArray()));
        //    TrainLabel = new Matrix<int>(trainLabel.ToArray());

        //}


        private T[,] To2D<T>(T[][] source)
        {
            try
            {
                int FirstDim = source.Length;
                int SecondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

                var result = new T[FirstDim, SecondDim];
                for (int i = 0; i < FirstDim; ++i)
                    for (int j = 0; j < SecondDim; ++j)
                        result[i, j] = source[i][j];

                return result;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("The given jagged array is not rectangular.");
            }
        }
        
        public void Test()
        {
            SVM svm = new SVM();
            FileStorage file = new FileStorage("sdf.txt", FileStorage.Mode.Read);
            svm.Read(file.GetNode("opencv_ml_svm"));


            //svm = new SVM();
            //svm.C = 100;
            //svm.Type = SVM.SvmType.CSvc;
            //svm.Gamma = 0.005;
            //svm.SetKernel(SVM.SvmKernelType.Linear);
            //svm.TermCriteria = new MCvTermCriteria(1000, 1e-6);
            //svm.Train(TrainData, Emgu.CV.ML.MlEnum.DataLayoutType.RowSample, TrainLabel);
            //svm.Save("svm.txt");

            //for (int i = 0; i < TestData.Rows; i++)
            //{
            //    Matrix<float> row = TestData.GetRow(i);
            //    float predict = svm.Predict(row);
            //}
            Mat img_character = new Mat("D:\\GitHub\\FinalProject\\Camera\\bin\\x64\\Debug\\1.jpg",ImreadModes.AnyDepth);

            List<float> feature = EmguCVExtension.calculate_feature(img_character);
            // Open CV3.1
            Mat m = new Mat(1, 32, DepthType.Cv32F, 1);
            for (int i = 0; i < feature.Count(); ++i)
            {
                float temp = feature[i];
                m.SetValue(0,i,temp);
            }

           

            char c = '*';

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
        }
        public char character_recognition(Mat img_character)
        {

            return '*';
        }




        //public static void SaveSVMToFile(SVM model, String path)
        //{
        //    if (File.Exists(path)) File.Delete(path);
        //    FileStorage fs = new FileStorage(path, FileStorage.Mode.Write);
        //    model.Write(fs);
        //    fs.ReleaseAndGetString();
        //}
        //public void Traning()
        //{
        //    Emgu.CV.ML.SVM model = new Emgu.CV.ML.SVM();
        //    model.SetKernel(Emgu.CV.ML.SVM.SvmKernelType.Linear);
        //    model.Type = Emgu.CV.ML.SVM.SvmType.CSvc;
        //    model.C = 1;
        //    model.TermCriteria = new MCvTermCriteria(100, 0.00001);
        //    bool trained = model.TrainAuto(my_trainData, 5);
        //    model.Save("SVM_Model.xml");
        //}
        //private string character_recognition(Mat img_character)
        //{
        //    Matrix<float> matrix = new Matrix<float>(10,10,1);
        //    Matrix<float> reponse = new Matrix<float>(10, 10, 1);
        //    Matrix<float> sample = new Matrix<float>(1, 2);
        //    Image<Bgr, Byte> img = new Image<Bgr, byte>(500, 500);

        //    TrainData trainData = new TrainData(matrix,Emgu.CV.ML.MlEnum.DataLayoutType.ColSample, reponse);
        //    //Load SVM training file OpenCV 3.1
        //    //SVM svm = new SVM().;
        //    FileStorage fs = new FileStorage(Application.StartupPath +"\\"+ "svm.txt", FileStorage.Mode.Read);
        //    //svm.  .Read(fs.GetRoot());
        //    bool trained = svm.TrainAuto(trainData, 5);
        //    //svm.Save("SVM_Model.xml");
        //    //SVM svmNew;
        //    //svmNew.load("D:/svm.txt");

        //    char c = '*';
        //    for (int i = 0; i < img_character.Height; i++)
        //    {
        //        for (int j = 0; j < img_character.Width; j++)
        //        {
        //            sample.Data[0, 0] = j;
        //            sample.Data[0, 1] = i;


        //            int ri = (int)(svm.Predict(img_character)); // Open CV 3.1
        //                                                        /*int ri = int(svmNew.predict(m));*/
        //            if (ri >= 0 && ri <= 9)
        //                c = (char)(ri + 48); //ma ascii 0 = 48
        //            if (ri >= 10 && ri < 18)
        //                c = (char)(ri + 55); //ma accii A = 5, --> tu A-H
        //            if (ri >= 18 && ri < 22)
        //                c = (char)(ri + 55 + 2); //K-N, bo I,J
        //            if (ri == 22) c = 'P';
        //            if (ri == 23) c = 'S';
        //            if (ri >= 24 && ri < 27)
        //                c = (char)(ri + 60); //T-V,  
        //            if (ri >= 27 && ri < 30)
        //                c = (char)(ri + 61); //X-Z
        //        }
        //    }



        //    return c.ToString();

        //}
    }
}
