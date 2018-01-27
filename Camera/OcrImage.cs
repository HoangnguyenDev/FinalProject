using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Camera
{
    public class OcrImage
    {
        private Mat _source;
        private Tesseract _ocr;
        public OcrImage(Mat source,string path)
        {
            _source = source;
            InitOcr(path, "eng", OcrEngineMode.TesseractLstmCombined);
        }
        public enum TypeOcr
        {
            NUMBER,
            TEXT,
            BOTH,
        }
        public void SetOcr(TypeOcr type)
        {
            switch (type)
            {
                case TypeOcr.NUMBER:
                    _ocr.SetVariable("tessedit_char_whitelist", "1234567890");
                    break;
                case TypeOcr.TEXT:
                    _ocr.SetVariable("tessedit_char_whitelist", "ABCDEFHKLMNPRSTUVXY");
                    break;
                case TypeOcr.BOTH:
                    _ocr.SetVariable("tessedit_char_whitelist", "ABCDEFHKLMNPRSTVXY1234567890");
                    break;
            }
        }
        //private static void TesseractDownloadLangFile(String folder, String lang)
        //{
        //    String subfolderName = "tessdata";
        //    String folderName = System.IO.Path.Combine(folder, subfolderName);
        //    if (!System.IO.Directory.Exists(folderName))
        //    {
        //        System.IO.Directory.CreateDirectory(folderName);
        //    }
        //    String dest = System.IO.Path.Combine(folderName, String.Format("{0}.traineddata", lang));
        //    if (!System.IO.File.Exists(dest))
        //        using (System.Net.WebClient webclient = new System.Net.WebClient())
        //        {
        //            String source =
        //                String.Format("https://github.com/tesseract-ocr/tessdata/blob/4592b8d453889181e01982d22328b5846765eaad/{0}.traineddata?raw=true", lang);

        //            Console.WriteLine(String.Format("Downloading file from '{0}' to '{1}'", source, dest));
        //            webclient.DownloadFile(source, dest);
        //            Console.WriteLine(String.Format("Download completed"));
        //        }
        //}

        private void InitOcr(String path, String lang, OcrEngineMode mode)
        {
            try
            {
                if (_ocr != null)
                {
                    _ocr.Dispose();
                    _ocr = null;
                }


                _ocr = new Tesseract(path, lang, mode);

            }
            catch (Exception e)
            {
                _ocr = null;
                MessageBox.Show(e.Message, "Failed to initialize tesseract OCR engine", MessageBoxButtons.OK);
            }
        }

        private enum OCRMode
        {
            /// <summary>
            /// Perform a full page OCR
            /// </summary>
            FullPage,

            /// <summary>
            /// Detect the text region before applying OCR.
            /// </summary>
            TextDetection
        }
        
        public string Recoginatinon()
        {
#if !DEBUG
         try
#endif
            {

                Mat result = new Mat();
                String ocredText = GetText(_ocr, _source, OCRMode.FullPage, result);
                return ocredText;
            }
#if !DEBUG
         catch (Exception exception)
         {
            MessageBox.Show(exception.Message);
            return "";
         }
#endif
        }

        private static String GetText(Tesseract ocr, Mat image, OCRMode mode, Mat imageColor)
        {
            Bgr drawCharColor = new Bgr(Color.Red);

            if (image.NumberOfChannels == 1)
                CvInvoke.CvtColor(image, imageColor, ColorConversion.Gray2Bgr);
            else
                image.CopyTo(imageColor);


            if (mode == OCRMode.FullPage)
            {

                ocr.SetImage(imageColor);

                if (ocr.Recognize() != 0)
                    throw new Exception("Failed to recognizer image");
                Tesseract.Character[] characters = ocr.GetCharacters();
                if (characters.Length == 0)
                {
                    Mat imgGrey = new Mat();
                    CvInvoke.CvtColor(image, imgGrey, ColorConversion.Bgr2Gray);
                    Mat imgThresholded = new Mat();
                    CvInvoke.Threshold(imgGrey, imgThresholded, 65, 255, ThresholdType.Binary);
                    ocr.SetImage(imgThresholded);
                    characters = ocr.GetCharacters();
                    imageColor = imgThresholded;
                    if (characters.Length == 0)
                    {
                        CvInvoke.Threshold(image, imgThresholded, 190, 255, ThresholdType.Binary);
                        ocr.SetImage(imgThresholded);
                        characters = ocr.GetCharacters();
                        imageColor = imgThresholded;
                    }
                }
                foreach (Tesseract.Character c in characters)
                {
                    CvInvoke.Rectangle(imageColor, c.Region, drawCharColor.MCvScalar);
                }

                return ocr.GetUTF8Text();

            }
            else
            {
                bool checkInvert = true;

                Rectangle[] regions;

                using (
                   ERFilterNM1 er1 = new ERFilterNM1("trained_classifierNM1.xml", 8, 0.00025f, 0.13f, 0.4f, true, 0.1f))
                using (ERFilterNM2 er2 = new ERFilterNM2("trained_classifierNM2.xml", 0.3f))
                {
                    int channelCount = image.NumberOfChannels;
                    UMat[] channels = new UMat[checkInvert ? channelCount * 2 : channelCount];

                    for (int i = 0; i < channelCount; i++)
                    {
                        UMat c = new UMat();
                        CvInvoke.ExtractChannel(image, c, i);
                        channels[i] = c;
                    }

                    if (checkInvert)
                    {
                        for (int i = 0; i < channelCount; i++)
                        {
                            UMat c = new UMat();
                            CvInvoke.BitwiseNot(channels[i], c);
                            channels[i + channelCount] = c;
                        }
                    }

                    VectorOfERStat[] regionVecs = new VectorOfERStat[channels.Length];
                    for (int i = 0; i < regionVecs.Length; i++)
                        regionVecs[i] = new VectorOfERStat();

                    try
                    {
                        for (int i = 0; i < channels.Length; i++)
                        {
                            er1.Run(channels[i], regionVecs[i]);
                            er2.Run(channels[i], regionVecs[i]);
                        }
                        using (VectorOfUMat vm = new VectorOfUMat(channels))
                        {
                            regions = ERFilter.ERGrouping(image, vm, regionVecs, ERFilter.GroupingMethod.OrientationHoriz,
                               "trained_classifier_erGrouping.xml", 0.5f);
                        }
                    }
                    finally
                    {
                        foreach (UMat tmp in channels)
                            if (tmp != null)
                                tmp.Dispose();
                        foreach (VectorOfERStat tmp in regionVecs)
                            if (tmp != null)
                                tmp.Dispose();
                    }

                    Rectangle imageRegion = new Rectangle(Point.Empty, imageColor.Size);
                    for (int i = 0; i < regions.Length; i++)
                    {
                        Rectangle r = ScaleRectangle(regions[i], 1.1);

                        r.Intersect(imageRegion);
                        regions[i] = r;
                    }

                }


                List<Tesseract.Character> allChars = new List<Tesseract.Character>();
                String allText = String.Empty;
                foreach (Rectangle rect in regions)
                {
                    using (Mat region = new Mat(image, rect))
                    {
                        ocr.SetImage(region);
                        if (ocr.Recognize() != 0)
                            throw new Exception("Failed to recognize image");
                        Tesseract.Character[] characters = ocr.GetCharacters();

                        //convert the coordinates from the local region to global
                        for (int i = 0; i < characters.Length; i++)
                        {
                            Rectangle charRegion = characters[i].Region;
                            charRegion.Offset(rect.Location);
                            characters[i].Region = charRegion;

                        }
                        allChars.AddRange(characters);

                        allText += ocr.GetUTF8Text() + Environment.NewLine;

                    }
                }

                Bgr drawRegionColor = new Bgr(Color.Red);
                foreach (Rectangle rect in regions)
                {
                    CvInvoke.Rectangle(imageColor, rect, drawRegionColor.MCvScalar);
                }
                foreach (Tesseract.Character c in allChars)
                {
                    CvInvoke.Rectangle(imageColor, c.Region, drawCharColor.MCvScalar);
                }

                return allText;

            }

        }
        
        private static Rectangle ScaleRectangle(Rectangle r, double scale)
        {
            double centerX = r.Location.X + r.Width / 2.0;
            double centerY = r.Location.Y + r.Height / 2.0;
            double newWidth = Math.Round(r.Width * scale);
            double newHeight = Math.Round(r.Height * scale);
            return new Rectangle((int)Math.Round(centerX - newWidth / 2.0), (int)Math.Round(centerY - newHeight / 2.0),
               (int)newWidth, (int)newHeight);
        }
    }
}
