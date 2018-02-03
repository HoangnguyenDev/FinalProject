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
            capture = new Emgu.CV.VideoCapture(0);

            IF = new ImageForm();
            svm = SVMExtension.Create();
            InitializeComponent();
        }
        public void ProcessImage(string urlImage)
        {
            PlateImagesList.Clear();
            PlateTextList.Clear();
            FileStream fs = new FileStream(urlImage, FileMode.Open, FileAccess.Read);
            Image img = Image.FromStream(fs);
            Bitmap image = new Bitmap(img);
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
            Image dst = image;
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

                IdentifyContours(src.Bitmap, out grayframe, out listRect, out listMat);

                Image<Gray, byte> dst = new Image<Gray, byte>(grayframe);
                grayframe = dst.ToBitmap();
                string zz = "";
                for (int i = 0; i < listMat.Count; i++)
                {

                    char cs = Recoginatinon(svm, listMat[i]);
                    zz += cs;
                }

                string replacement = Regex.Replace(zz, @"\t|\n|\r", "");
                txtLPR.Text = replacement;
                char[] arr = replacement.ToCharArray();
                Array.Reverse(arr);
                txtLPR.Text = new string(arr);
            }

        }

        public int IdentifyContours(Bitmap colorImage, out Bitmap processedGray, out List<Rectangle> listRect, out List<Mat> listMat)
        {
            #region Xác định và cắt các vùng của từng chữ số
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
                        r_characters.Add(new Rectangle(X, Y, sub_r.Width + MARGIN_RECT * 2, sub_r.Height + MARGIN_RECT * 2));
                    }
                }
            }
            #endregion
            pictureBox5.Image = binary.Bitmap;
            #region Sap xep va xoa bo vung den du thua
            if (r_characters.Count >= 7)
            {
                #region Sap xep
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
                    c.Add(cj);
                   // CvInvoke.Imshow(i.ToString() + ".jpg", cj);
                }
                #endregion

                #region Xoa bo cac vung den nhi phan du thua
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
                }
                for (int f = 0; f < cb.Count; ++f)
                {
                    CvInvoke.Imwrite(f.ToString() + ".jpg", cb[f]);
                }
                #endregion
            }
            #endregion

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
    }
}

    
