using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Camera
{
    public class LPR
    {
        public List<Image<Bgr, byte>> PlateImagesList = new List<Image<Bgr, byte>>();
        Image Plate_Draw;

        OcrImage ocrImage;
        static string PATH_OCR = "";
        public void ProcessImage(Bitmap image, out Image Plate_Draw, out Image<Bgr, byte> PlateImagesResize)
        {
            PlateImagesList.Clear();
            FindLicensePlate(image, out Plate_Draw, out PlateImagesResize);
        }
        public void ProcessImageWithCrop(Bitmap image, out Image Plate_Draw, out Image<Bgr, byte> PlateImagesResize)
        {
            PlateImagesList.Clear();
            FindLicensePlateCrop(image, out Plate_Draw, out PlateImagesResize);
        }
        public void FindLicensePlate(Bitmap image, out System.Drawing.Image plateDraw, out Image<Bgr, byte> PlateImagesResize)
        {
            plateDraw = null;
            PlateImagesResize = null;
            Image<Bgr, byte> frame;
            bool isface = false;
            Bitmap src;
            //pictureBox2.Image = new Image<Gray, byte>(image).ToBitmap();
            System.Drawing.Image dst = image;
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
                            PlateImagesResize = PlateImagesList[0] = PlateImagesList[0].Resize(400, 400, Inter.Linear);
                            return;
                        }

                        //CvInvoke.Imshow("12345", PlateImagesList[0]);
                    }
                }
            }


        }
        public void FindLicensePlateCrop(Bitmap image, out System.Drawing.Image plateDraw, out Image<Bgr, byte> PlateImagesResize)
        {
            plateDraw = null;
            PlateImagesResize = null;
            Image<Bgr, byte> frame;
            bool isface = false;
            Bitmap src;
            //pictureBox2.Image = new Image<Gray, byte>(image).ToBitmap();
            System.Drawing.Image dst = image;
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
                           // CvInvoke.Imshow("adsf", tmp);
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
                            PlateImagesResize = PlateImagesList[0] = PlateImagesList[0].Resize(400, 400, Inter.Linear);

                            //PlateImagesResize =  PlateImagesResize.GetSubRect(new Rectangle(40, 40, 300, 320));
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
            //ORBTest
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
            Image Plate_Draw;
            Image<Bgr, byte> PlateImagesResize;
            ProcessImage(link,out Plate_Draw,out PlateImagesResize);
            if (PlateImagesList.Count != 0)
            {
                Image<Bgr, byte> src = new Image<Bgr, byte>(PlateImagesList[0].ToBitmap());
                Bitmap grayframe;
                List<Rectangle> listRect = new List<Rectangle>();
                List<Mat> listMat = new List<Mat>();

                //CvInvoke.Imshow("tEST",src);
                //Image<Bgr, byte> imageAAA = new Image<Bgr, byte>("D:\\Test\\Test\\1.jpg");
                FindContours con = new FindContours();
                con.IdentifyContours(src.Bitmap, out grayframe, out listRect, out listMat);
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
                else if (listMat.Count == 6)
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
                string replacement = Regex.Replace(zz, @"\t|\n|\r", "");
                //lbPlate.Text = replacement;
                char[] arr = replacement.ToCharArray();
                Array.Reverse(arr);

            }
        }



        /// <summary>
        /// Phương thức được sử dụng để xác định vùng bao quanh tất cả các con số trong biển số xe
        /// </summary>
        /// <param name="link">Đường dẫn tới hình</param>
        /// <param name="hinhbienso">Giá trị xuất ra hình biển số</param>
        /// <param name="listRect">Giá trị xuất ra danh sách vùng bao quanh các chữ số xe</param>
        /// <param name="listMat">Giá trị xuất ra danh sách hình các chữ số xe</param>
        public bool DetectRectangleLPR(Bitmap link, out Bitmap hinhbienso, out List<Rectangle> listRect, out List<Mat> listMat)
        {
            hinhbienso = null;
            Image Plate_Draw = null;
            Image<Bgr, byte> PlateImagesResize = null;
            ProcessImage(link,out Plate_Draw,out PlateImagesResize);
            listRect = null;
            listMat = null;
            if (PlateImagesList.Count != 0)
            {
                Image<Bgr, byte> src = new Image<Bgr, byte>(PlateImagesList[0].ToBitmap());
                Bitmap grayframe;
                //CvInvoke.Imshow("tEST",src);
                //Image<Bgr, byte> imageAAA = new Image<Bgr, byte>("D:\\Test\\Test\\1.jpg");
                FindContours con = new FindContours();
                con.IdentifyContours(src.Bitmap, out grayframe, out listRect, out listMat);
                hinhbienso = grayframe;
                Image<Gray, byte> dst = new Image<Gray, byte>(grayframe);
                grayframe = dst.ToBitmap();
                return true;
            }
            return false;
        }
    }
}
