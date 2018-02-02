using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Camera
{
    public class FaceAndPlateDetection
    {
        Form _form;
        FaceAndPlateDetection(Form form)
        {
            _form = form;
        }
        public void CapFaceOutHading(MainForm2 form)
        {
            //Image<Bgr, byte> capFaceImageOut = null;

            //#region Load frame Face
            //if (isDemo)
            //{
            //    if (_currentFrameFaceIn < _totalFrameFaceIn)
            //    {
            //        capFaceImageOut = _captureFaceOut.QueryFrame().ToImage<Bgr, byte>();
            //        Image_Xe_Ra_Sau.Image = capFaceImageOut.Resize(Image_Xe_Vao_Sau.Width, Image_Xe_Ra_Truoc.Height, Inter.Cubic).Bitmap;
            //        _currentFrameFaceIn++;
            //        Image_Xe_Ra_Sau.Update();
            //    }

            //    //await Task.Delay(1000 / _fpsFaceIn / 4);
            //}
            //else if (_captureFaceOut.IsOpened)
            //{
            //    capFaceImageOut = _captureFaceOut.QueryFrame().ToImage<Bgr, byte>();
            //    Image_Xe_Vao_Sau.Image = capFaceImageOut.Resize(Image_Xe_Vao_Sau.Width, Image_Xe_Ra_Truoc.Height, Inter.Cubic).Bitmap;
            //    //Image_Xe_Vao_Sau.Update();
            //}
            //#endregion

            //if (capFaceImageOut != null && !isFinishFaceIn)
            //{
            //    gray_frameFace = capFaceImageOut.Convert<Gray, Byte>();

            //    //Face Detector
            //    Rectangle[] facesDetected = _face.DetectMultiScale(gray_frameFace, 1.3, 10, new Size(70, 70), Size.Empty);

            //    //Action for each element detected
            //    Parallel.For(0, facesDetected.Length, i =>
            //    {
            //        try
            //        {
            //            //facesDetected[i].X += (int)(facesDetected[i].Height * 0.15);
            //            //facesDetected[i].Y += (int)(facesDetected[i].Width * 0.22);
            //            //facesDetected[i].Height -= (int)(facesDetected[i].Height * 0.3);
            //            //facesDetected[i].Width -= (int)(facesDetected[i].Width * 0.35);
            //            Image<Gray, byte> result = capFaceImageOut.Copy(facesDetected[i]).Convert<Gray, byte>().Resize(100, 100, Inter.Cubic);
            //            Image<Bgr, byte> imageShow = capFaceImageOut.Copy(facesDetected[i]).Resize(100, 67, Inter.Cubic);

            //            result._EqualizeHist();
            //            //draw the face detected in the 0th (gray) channel with blue color
            //            capFaceImageOut.Draw(facesDetected[i], new Bgr(Color.Red), 2);

            //            _startPRDT = DateTime.Now;
            //            if (_recognition.IsTrained)
            //            {
            //                if (countNotReFace <= 3)
            //                {
            //                    string name = _recognition.Recognise(result);
            //                    int match_value = (int)_recognition.Get_Fisher_Distance;
            //                    if (name == "Unknown" || name == "")
            //                    {
            //                        countNotReFace++;
            //                        IsExistFace = false;
            //                    }
            //                    else
            //                    {
            //                        Image_ID.Image = imageShow.Bitmap;
            //                        isFinishFaceIn = true;
            //                        lbID.Text = name;
            //                        IsExistFace = true;
            //                    }
            //                }
            //                if (countNotReFace >= 3)
            //                {
            //                    isFinishFaceIn = true;
            //                }
            //            }
            //            isFinishFaceIn = true;


            //        }
            //        catch
            //        {
            //            //do nothing as parrellel loop buggy
            //            //No action as the error is useless, it is simply an error in 
            //            //no data being there to process and this occurss sporadically 
            //        }
            //    });
            //}
        }

    }
}
