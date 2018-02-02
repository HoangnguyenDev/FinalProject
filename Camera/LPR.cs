using Auto_parking;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.ML;
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
        SVM _svm;
        public LPR(SVM svm)
        {
            _svm = svm;
        }
        public List<Image<Bgr, byte>> PlateImagesList = new List<Image<Bgr, byte>>();
        Image Plate_Draw;

        OcrImage ocrImage;
        static string PATH_OCR = "";
        public List<Image<Bgr, byte>> ProcessImage(Bitmap image, out Image Plate_Draw, out Image<Bgr, byte> PlateImagesResize, CascadeClassifier plate)
        {
            PlateImagesList.Clear();
            return FindLicensePlateNew(image, out Plate_Draw, out PlateImagesResize, plate);
        }
        public void ProcessImageWithCrop(Bitmap image, out Image Plate_Draw, out Image<Bgr, byte> PlateImagesResize)
        {
            PlateImagesList.Clear();
            FindLicensePlateCrop(image, out Plate_Draw, out PlateImagesResize);
        }
        public List<Image<Bgr, byte>> FindLicensePlate(Bitmap image, out System.Drawing.Image plateDraw, out Image<Bgr, byte> PlateImagesResize)
        {
            plateDraw = null;
            PlateImagesResize = null;
            Image<Bgr, byte> frame;
            bool isface = false;
            Bitmap src;
            System.Drawing.Image dst = image;
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


                        Rectangle[] faces = cascadeClassifier.DetectMultiScale(grayframe, 1.1, 3, new Size(0, 0));
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
                            return PlateImagesList;
                        }
                    }
                }
            }
            return PlateImagesList;


        }
        public List<Image<Bgr, byte>> FindLicensePlateNew(Bitmap image, out System.Drawing.Image plateDraw, out Image<Bgr, byte> PlateImagesResize, CascadeClassifier _plate)
        {
            plateDraw = null;
            PlateImagesResize = null;

            Image<Bgr, byte> frame = new Image<Bgr, byte>(image);
            bool isFound = false;
            Image<Gray, byte> grayframe = frame.Convert<Gray, byte>();
            Rectangle[] faces = _plate.DetectMultiScale(grayframe, 1.1, 8, new Size(0, 0));
            Parallel.For(0, faces.Length, i =>
            {
                try
                {
                    Image<Bgr, byte> tmp = frame.Copy();
                    tmp.ROI = faces[i];
                    frame.Draw(faces[i], new Bgr(Color.Blue), 2);

                    PlateImagesList.Add(tmp);
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
                    isFound = true;
                }
                catch { }
            });
            if(isFound)
                PlateImagesResize = PlateImagesList[0] = PlateImagesList[0].Resize(400, 400, Inter.Linear);
            return PlateImagesList;


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
        
        /// <summary>
        /// Phương thức được sử dụng để xuất biển số xe ra màn hình
        /// </summary>
        /// <param name="link">Đường dẫn tới hình</param>
        /// <param name="hinhbienso">Giá trị xuất ra hình biển số</param>
        /// <param name="bienso_text">Giá trị xuất ra chữ số dưới dạng text</param>
        private void Reconize(Bitmap link, out Bitmap hinhbienso, out string bienso_text, CascadeClassifier plate)
        {
            hinhbienso = null;
            bienso_text = "";
            Image Plate_Draw;
            Image<Bgr, byte> PlateImagesResize;
            ProcessImage(link, out Plate_Draw, out PlateImagesResize, plate);
            if (PlateImagesList.Count != 0)
            {
                Image<Bgr, byte> src = new Image<Bgr, byte>(PlateImagesList[0].ToBitmap());
                Bitmap grayframe;
                List<Rectangle> listRect = new List<Rectangle>();
                List<Mat> listMat = new List<Mat>();

                FindContours con = new FindContours();
                con.IdentifyContours(src.Bitmap, out grayframe, out listRect, out listMat);
                hinhbienso = grayframe;
                Image<Gray, byte> dst = new Image<Gray, byte>(grayframe);
                grayframe = dst.ToBitmap();
                string zz = "";

                grayframe = dst.ToBitmap();
                if (listMat.Count >= 7)
                {
                    for (int i = 0; i < listMat.Count; i++)
                    {

                        char cs = Recoginatinon(_svm, listMat[i]);
                        zz += cs;
                    }
                }

                string replacement = Regex.Replace(zz, @"\t|\n|\r", "");
                char[] arr = replacement.ToCharArray();
                Array.Reverse(arr);
                bienso_text = new string(arr);
            }
        }

        /// <summary>
        /// Phương thức được sử dụng để xuất biển số xe ra màn hình
        /// </summary>
        /// <param name="PlateImagesList">Danh sách khung xe lấy được</param>
        /// <param name="hinhbienso">Giá trị xuất Hình ảnh khung xe</param>
        public string GetRecoginatinon(List<Image<Bgr, byte>> PlateImagesList, out Bitmap hinhbienso)
        {
            Image<Bgr, byte> src = new Image<Bgr, byte>(PlateImagesList[0].ToBitmap());
            Bitmap grayframe;
            List<Rectangle> listRect = new List<Rectangle>();
            List<Mat> listMat = new List<Mat>();

            FindContours con = new FindContours();
            con.IdentifyContours(src.Bitmap, out grayframe, out listRect, out listMat);
            hinhbienso = grayframe;
            Image<Gray, byte> dst = new Image<Gray, byte>(grayframe);
            grayframe = dst.ToBitmap();
            string zz = "";

            grayframe = dst.ToBitmap();
            if (listMat.Count >= 7)
            {
                for (int i = 0; i < listMat.Count; i++)
                {

                    char cs = Recoginatinon(_svm, listMat[i]);
                    zz += cs;
                }
            }

            string replacement = Regex.Replace(zz, @"\t|\n|\r", "");
            char[] arr = replacement.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
        /// <summary>
        /// Phương thức được sử dụng để xác định vùng bao quanh tất cả các con số trong biển số xe
        /// </summary>
        /// <param name="link">Đường dẫn tới hình</param>
        /// <param name="hinhbienso">Giá trị xuất ra hình biển số</param>
        /// <param name="listRect">Giá trị xuất ra danh sách vùng bao quanh các chữ số xe</param>
        /// <param name="listMat">Giá trị xuất ra danh sách hình các chữ số xe</param>
        public bool DetectRectangleLPR(Bitmap link, out Bitmap hinhbienso, out List<Rectangle> listRect, out List<Mat> listMat,CascadeClassifier plate)
        {
            hinhbienso = null;
            Image Plate_Draw = null;
            Image<Bgr, byte> PlateImagesResize = null;
            ProcessImage(link,out Plate_Draw,out PlateImagesResize, plate);
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

        /// <summary>
        /// Phương thức được sử dụng để lấy ký tự trong hình
        /// </summary>
        /// <param name="svm">Nhập vào kết nối SVM</param>
        /// <param name="listRect">Nhập vào hình cần lấy dưới dạng Mat theo hình nhị phân</param>
        /// <returns Trả về ký tự cần lấy></returns>
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
