using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Auto_parking
{
    public class EmguCVExtension
    {
        public static int count_pixel(Mat img, bool black_pixel = true)
        {
            Image<Gray, byte> binary = new Image<Gray, byte>(img.Bitmap);
            int black = 0;
            int white = 0;
            for (int i = 0; i < binary.Rows; ++i)
                for (int j = 0; j < binary.Cols; ++j)
                {
                    if (binary.Data[i, j, 0] == 0)
                        black++;
                    else
                        white++;
                }
            if (black_pixel)
                return black;
            else
                return white;
        }
        public static List<float> calculate_feature(Mat src)
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
            CvInvoke.Resize(img, img, new Size(40, 40));
            int h = img.Rows / 4;
            int w = img.Cols / 4;
            int S = count_pixel(img);
            int T = img.Cols * img.Rows;
            for (int i = 0; i < img.Rows; i += h)
            {
                for (int j = 0; j < img.Cols; j += w)
                {
                    Mat cell = new Mat(img, new Rectangle(i, j, h, w));
                    int s = count_pixel(cell);
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
