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
using System.Drawing;
using System.IO;
using System.Linq;
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



        ImageForm IF;
        public StepByStepForm()
        {
            capture = new Emgu.CV.VideoCapture(0);

            IF = new ImageForm();

            InitializeComponent();
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
            CvInvoke.AdaptiveThreshold(grayImage, binary, 255, AdaptiveThresholdType.GaussianC, ThresholdType.Binary, 55, 5);
            //Mat gray, binary;
            // CvInvoke.Imshow("binary",binary);
            pictureBox4.Image = binary.Bitmap;
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
            pictureBox5.Image = grayImage.Bitmap;
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
    }
}
