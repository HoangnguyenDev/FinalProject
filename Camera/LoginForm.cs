using Emgu.CV;
using Emgu.CV.Structure;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Camera
{
    public partial class LoginForm : MetroForm
    {
        #region định nghĩa
        public CascadeClassifier _face = new CascadeClassifier(Application.StartupPath + "/haarcascade_frontalface_default.xml");//Our face detection method 
        Classifier_Train _recognition = new Classifier_Train();

        Image<Bgr, Byte> currentFrame; //current image aquired from webcam for display
        Image<Gray, byte> result, TrainedFace = null; //used to store the result image and trained face
        Image<Gray, byte> gray_frame = null; //grayscale current image aquired from webcam for processing

        public VideoCapture grabber; //This is our capture variable
        DateTime dateTime;
        DateTime endTime;
        bool stopCamera = false;
        #endregion
        public LoginForm()
        {
            dateTime = DateTime.Now;
            InitializeComponent();
            if (_recognition.IsTrained) { }
            initialise_capture();
        }
        public void initialise_capture()
        {
            grabber = new VideoCapture();
            grabber.QueryFrame();
            //Initialize the FrameGraber event
            //if (parrellelToolStripMenuItem.Checked)
            //{
            //    Application.Idle += new EventHandler(FrameGrabber_Parrellel);
            //}
            //else
            //{
           // if(!stopCamera)
                Application.Idle += new EventHandler(FrameGrabber_Standard);
            //}
        }
        //Process Frame
        void FrameGrabber_Standard(object sender, EventArgs e)
        {
            if (!stopCamera) { 
            //Get the current frame form capture device
            currentFrame = grabber.QueryFrame().ToImage<Bgr, byte>().Resize(320, 240, Emgu.CV.CvEnum.Inter.Cubic);

            //Convert it to Grayscale
            if (currentFrame != null)
            {
                gray_frame = currentFrame.Convert<Gray, Byte>();

                //Face Detector
                Rectangle[] facesDetected = _face.DetectMultiScale(gray_frame, 1.2, 10, new Size(50, 50), Size.Empty);

                //Action for each element detected
                Parallel.For(0, facesDetected.Length, i =>
                {
                    try
                    {
                        //facesDetected[i].X += (int)(facesDetected[i].Height * 0.15);
                        //facesDetected[i].Y += (int)(facesDetected[i].Width * 0.22);
                        //facesDetected[i].Height -= (int)(facesDetected[i].Height * 0.3);
                        //facesDetected[i].Width -= (int)(facesDetected[i].Width * 0.35);

                        result = currentFrame.Copy(facesDetected[i]).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.Inter.Cubic);
                        result._EqualizeHist();
                        //draw the face detected in the 0th (gray) channel with blue color
                        currentFrame.Draw(facesDetected[i], new Bgr(Color.Red), 2);
                        Thread.Sleep(500);
                        stopCamera = true;
                        grabber.Dispose();
                        grabber = null;
                        Hide();
                        MainForm2 main = new MainForm2();
                        main.Show();
                        if (_recognition.IsTrained)
                        {
                            string name = _recognition.Recognise(result);
                            int match_value = (int)_recognition.Get_Fisher_Distance;

                            //Draw the label for each face detected and recognized
                            currentFrame.Draw(name + " ", new Point(facesDetected[i].X - 2, facesDetected[i].Y - 2), Emgu.CV.CvEnum.FontFace.HersheyComplex, 1, new Bgr(Color.LightGreen));
                            //ADD_Face_Found(result, name, match_value);
                        }

                    }
                    catch
                    {
                        //do nothing as parrellel loop buggy
                        //No action as the error is useless, it is simply an error in 
                        //no data being there to process and this occurss sporadically 
                    }
                });

                //Show the faces procesed and recognized
                pictureBox1.Image = currentFrame.ToBitmap();
                endTime = DateTime.Now;
                TimeSpan time = endTime.Subtract(dateTime);
                if (time.Milliseconds > 5000)
                { 
                    MainForm2 main = new MainForm2();
                    main.Show();
                }

            }
            }
        }
    }
}

