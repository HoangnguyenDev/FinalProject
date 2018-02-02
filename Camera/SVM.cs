using Auto_parking;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.ML;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Camera
{
    public static class SVMExtension
    {     
        public static SVM Create()
        {
            SVM svm = new SVM();
            FileStorage file = new FileStorage("svm.txt", FileStorage.Mode.Read);
            svm.Read(file.GetNode("opencv_ml_svm"));
            return svm;
        }
        private static List<String> List_folder(string sDir)
        {
            List<String> listFolder = new List<String>();
            string[] files = Directory.GetFiles(sDir);
            string[] dirs = Directory.GetDirectories(sDir);

            foreach (string item2 in dirs)
            {
                FileInfo f = new FileInfo(item2);

                listFolder.Add(f.Name);

            }

            foreach (string item in files)
            {
                FileInfo f = new FileInfo(item);

                listFolder.Add(f.Name);

            }

            return listFolder;
        }
        private static List<String> List_file(string sDir)
        {
            List<String> files = new List<String>();
            foreach (string f in Directory.GetFiles(sDir))
            {
                files.Add(f);
            }

            return files;
        }
        public static bool TrainSVM(string savepath, string trainImgpath)
        {
            const int number_of_class = 30;
            const int number_of_sample = 10;
            const int number_of_feature = 32;

            //Train SVM OpenCV 3.1
            SVM svm = new SVM();
            svm.Type = SVM.SvmType.CSvc;
            svm.SetKernel(SVM.SvmKernelType.Rbf);
            svm.Gamma = 0.5;
            svm.C = 16;
            svm.TermCriteria = new MCvTermCriteria();

            List<string> folders = List_folder(trainImgpath);
            if (folders.Count <= 0)
            {
                //do something
                return false;
            }
            if (number_of_class != folders.Count || number_of_sample <= 0 || number_of_class <= 0)
            {
                //do something
                return false;
            }
            Mat src;
            Mat data = new Mat(number_of_sample * number_of_class, number_of_feature, Emgu.CV.CvEnum.DepthType.Cv32F,1);
            Mat label = new Mat(number_of_sample * number_of_class, 1, Emgu.CV.CvEnum.DepthType.Cv32F,1);
            int index = 0;
            for (int i = 0; i < folders.Count; ++i)
            {
                List<string> files = List_file(folders[i]);
                if (files.Count <= 0 || files.Count != number_of_sample)
                {
                    return false;
                }
                string folder_path = folders[i];
                string label_folder = folder_path.Substring(folder_path.Length - 1);
                for (int j = 0; j < files.Count; ++j)
                {
                    src = CvInvoke.Imread(files[j]);
                    if (src.IsEmpty)
                    {
                        return false;
                    }

                    List<float> feature = EmguCVExtension.calculate_feature(src);
                    // Open CV3.1
                    Mat m = new Mat(1, 32, DepthType.Cv32F, 1);
                    for (int k = 0; k < feature.Count(); ++k)
                    {
                        data.SetValue(index, k, feature[i]);
                    }
                    label.SetValue(index, 0, i);
                    index++;
                }
            }
            // SVM Train OpenCV 3.1
            svm.TrainAuto(new TrainData(data, Emgu.CV.ML.MlEnum.DataLayoutType.RowSample, label));
            svm.Save(savepath);
            return true;
        }
    }
}
