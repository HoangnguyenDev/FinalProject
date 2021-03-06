﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Auto_parking;

namespace Camera
{
    class FindContours
    {
        const int MARGIN_RECT = 4;
        public int count = 0;
        /// <summary>
        /// Method used to process the image and set the output result images.
        /// </summary>
        /// <param name="colorImage">Source color image.</param>
        /// <param name="thresholdValue">Value used for thresholding.</param>
        /// <param name="processedGray">Resulting gray image.</param>
        /// <param name="processedColor">Resulting color image.</param>
        public int IdentifyContours(Bitmap colorImage, int thresholdValue, bool invert, out Bitmap processedGray, out Bitmap processedColor, out List<Rectangle> list)
        {
            List<Rectangle> listR = new List<Rectangle>();
            #region Conversion To grayscale
            Image<Gray, byte> grayImage = new Image<Gray, byte>(colorImage);
            //grayImage = grayImage.Resize(400, 400, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            Image<Gray, byte> bi = new Image<Gray, byte>(grayImage.Width, grayImage.Height);
            Image<Bgr, byte> color = new Image<Bgr, byte>(colorImage);

            #endregion


            #region  Image normalization and inversion (if required)

            ////CvInvoke.cvAdaptiveThreshold(grayImage, grayImage, 255,
            ////    Emgu.CV.CvEnum.ADAPTIVE_THRESHOLD_TYPE.CV_ADAPTIVE_THRESH_MEAN_C, Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY, 21, 2);
            ////string ff = grayImage.GetAverage().Intensity;

            ////grayImage = grayImage.ThresholdBinary(new Gray(grayImage.GetAverage().Intensity / 2.5), new Gray(255));

            ////double thr = cout_avg(grayImage) / 1.5;
            //double thr = cout_avg_new(grayImage)/1.5;
            //grayImage = grayImage.ThresholdBinary(new Gray(thr), new Gray(255));
            ////grayImage = grayImage.Dilate(3);
            //if (invert)
            //{
            //    grayImage._Not();
            //}
            //#endregion

            //#region Extracting the Contours
            //using (MemStorage storage = new MemStorage())
            //{

            //    Contour<Point> contours = grayImage.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, storage);
            //    while (contours != null)
            //    {

            //        Rectangle rect = contours.BoundingRectangle;
            //        //Contour<Point> currentContour = contours.ApproxPoly(contours.Perimeter * 0.015, storage);
            //        //color.Draw(currentContour.BoundingRectangle, new Bgr(0, 255, 0), 1);
            //        CvInvoke.cvDrawContours(color, contours, new MCvScalar(255, 255, 0), new MCvScalar(0), -1, 1, Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED, new Point(0, 0));
            //        if (rect.Width > 20 && rect.Width < 150
            //            && rect.Height > 80 && rect.Height < 150)
            //        {
            //            count++;
            //            Contour<Point> currentContour = contours.ApproxPoly(contours.Perimeter * 0.015, storage);
            //            CvInvoke.cvDrawContours(color, contours, new MCvScalar(0,255,255), new MCvScalar(255), -1, 3, Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED, new Point(0, 0));

            //            color.Draw(contours.BoundingRectangle, new Bgr(0, 255, 0), 2);
            //            bi.Draw(contours, new Gray(255), -1);
            //            listR.Add(contours.BoundingRectangle);


            //        }

            //        contours = contours.HNext;

            //    }
            //    for (int i = 0; i < count; i++)
            //    {
            //        for (int j = i + 1; j < count; j++)
            //        {
            //            if(  (listR[j].X < (listR[i].X + listR[i].Width) && listR[j].X > listR[i].X)
            //                && (listR[j].Y < (listR[i].Y + listR[i].Width) && listR[j].Y > listR[i].Y)  )
            //            {
            //                listR.RemoveAt(j);
            //                count--;
            //                j --;
            //            }
            //            else if(  (listR[i].X < (listR[j].X + listR[j].Width) && listR[i].X > listR[j].X)
            //                && (listR[i].Y < (listR[j].Y + listR[j].Width) && listR[i].Y > listR[j].Y))
            //            {
            //                listR.RemoveAt(i);
            //                count--;
            //                i--;
            //                break;
            //            }

            //        }
            //    }
            //}
            #endregion

            #region tim gia tri thresh de co so ky tu lon nhat

            //Image<Gray, byte> bi2 = new Image<Gray, byte>(grayImage.Width, grayImage.Height);
            //Image<Bgr, byte> color2 = new Image<Bgr, byte>(colorImage);
            //double thr = cout_avg_new(grayImage);
            double thr = 0;
            if (thr == 0)
            {
                thr = grayImage.GetAverage().Intensity;
            }
            //double thr = 50;
            //double min = 0, max = 255;
            //if (thr - 80 > 0)
            //{
            //    min = thr - 80;
            //}
            //if (thr + 80 < 255)
            //{
            //    max = thr + 80;
            //}
            //List<Rectangle> list_best = null;
            Rectangle[] li = new Rectangle[9];
            Image<Bgr, byte> color_b = new Image<Bgr, byte>(colorImage); ;
            Image<Gray, byte> src_b = grayImage.Clone();
            Image<Gray, byte> bi_b = bi.Clone();
            Image<Bgr, byte> color2;
            Image<Gray, byte> src = null;
            Image<Gray, byte> bi2;
            int c = 0, c_best = 0;
            //IntPtr a = color_b.Ptr;
            //CvInvoke.cvReleaseImage(ref a);
            for (double value = 0; value <= 127; value += 3)
            {
                for (int s = -1; s <= 1 && s + value != 1; s += 2)
                {
                    color2 = new Image<Bgr, byte>(colorImage);
                    //src = grayImage.Clone();
                    bi2 = bi.Clone();
                    listR.Clear();
                    //list_best.Clear();
                    c = 0;
                    double t = 127 + value * s;
                    src = grayImage.ThresholdBinary(new Gray(t), new Gray(255));
                    using (Mat hierachy = new Mat())
                    using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
                    {
                        CvInvoke.FindContours(src, contours, hierachy, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                        int count = contours.Size;
                        if(count > 0)
                        { 
                            for (int i = 0; i < count; i++)
                            {
                                        using (VectorOfPoint contour = contours[i])
                                        {
                                            Rectangle rect = CvInvoke.BoundingRectangle(contour);
                                            //CvInvoke.DrawContours(color2, contours, i, new MCvScalar(255, 255, 0),1,LineType.EightConnected, hierachy, -1,new Point(0, 0));
                                //            //CvInvoke.DrawContours(color2, contours, i, new MCvScalar(0), 1, LineType.EightConnected, null, -1, new Point(0, 0));


                                            double ratio = (double)rect.Width / rect.Height;
                                            if (rect.Width > 20 && rect.Width < 150
                                                && rect.Height > 80 && rect.Height < 180
                                                && ratio > 0.1 && ratio < 1.1 && rect.X > 20)
                                            {
                                                c++;
                                //                //CvInvoke.DrawContours(color2, contours, i, new MCvScalar(255, 0, 0),3, LineType.EightConnected, null, -1, new Point(0, 0));
                                //       //         CvInvoke.InRange(color2, new ScalarArray(new MCvScalar(0, 255, 255)),
                                //       //new ScalarArray(new MCvScalar(255)), color2);
                                //                color2.Draw(CvInvoke.BoundingRectangle(contour), new Bgr(0, 255, 0), 2);
                                //                bi2.Draw(CvInvoke.BoundingRectangle(contour), new Gray(255), -1);
                                                listR.Add(CvInvoke.BoundingRectangle(contours));


                                            }
                                        }
                            }
                        }
                    }
                        //IntPtr a = color_b.Ptr;
                        //CvInvoke.cvReleaseImage(ref a);
                        double avg_h = 0;
                    double dis = 0;
                    for (int i = 0; i < c; i++)
                    {
                        avg_h += listR[i].Height;
                        for (int j = i + 1; j < c; j++)
                        {
                            if ((listR[j].X < (listR[i].X + listR[i].Width) && listR[j].X > listR[i].X)
                                && (listR[j].Y < (listR[i].Y + listR[i].Width) && listR[j].Y > listR[i].Y))
                            {
                                //avg_h -= listR[j].Height;
                                listR.RemoveAt(j);
                                c--;
                                j--;
                            }
                            else if ((listR[i].X < (listR[j].X + listR[j].Width) && listR[i].X > listR[j].X)
                                && (listR[i].Y < (listR[j].Y + listR[j].Width) && listR[i].Y > listR[j].Y))
                            {
                                avg_h -= listR[i].Height;
                                listR.RemoveAt(i);
                                c--;
                                i--;
                                break;
                            }

                        }
                    }
                    avg_h = avg_h / c;
                    for (int i = 0; i < c; i++)
                    {
                        dis += Math.Abs(avg_h - listR[i].Height);
                    }

                    if (c <= 8 && c > 1 && c > c_best && dis <= c * 8)
                    {
                        listR.CopyTo(li);
                        c_best = c;
                        color_b = color2;
                        bi_b = bi2;
                        src_b = src;
                        //dis_b = dis;
                        //if (c == 8)
                        //{
                        //    break;
                        //}
                    }
                }
                if (c_best == 8) break;
            }

            count = c_best;
            grayImage = src_b;
            color = color_b;
            bi = bi_b;
            listR.Clear();
            for (int i = 0; i < li.Length; i++)
            {
                if (li[i].Height != 0) listR.Add(li[i]);
            }



            #endregion

            #region Asigning output
            processedColor = color.ToBitmap();
            processedGray = grayImage.ToBitmap();
            list = listR;
            #endregion
            return count;
        }

        /// <summary>
        /// Phương thức được sửa dụng để xử lý hình ảnh và xuất ra thông tin của biển số xe bao gồm processedGray, List<Rect> và List<Mat>
        /// </summary>
        /// <param name="colorImage">Source color image.</param>
        /// <param name="processedGray">Resulting gray image.</param>
        /// <param name="listRect">Kết quả xuất ra List<Rectagle> của biển số xe</param>
        /// <param name="listMat">Kết quả xuất ra List<Mat> của biển số xe</param>
        public int IdentifyContours(Bitmap colorImage, out Bitmap processedGray, out List<Rectangle> listRect, out List<Mat> listMat)
        {
            #region Xác định và cắt các vùng của từng chữ số
            Image<Gray, byte> grayImage = new Image<Gray, byte>(colorImage);
            Image<Bgr, byte> bgrImageBackup = new Image<Bgr, byte>(colorImage);
            Image<Gray, byte> grayImageBackup = new Image<Gray, byte>(colorImage);
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            //grayImage = grayImage.Resize(400, 400, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            Mat binary = new Image<Gray, byte>(grayImage.Width, grayImage.Height).Mat;
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

                }
                #endregion

                #region Xoa bo cac vung den nhi phan du thua
                foreach (var item in c)
                {
                    try
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
                    }
                    catch { }
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
    }

}
