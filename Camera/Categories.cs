//using Emgu.CV;
//using Emgu.CV.CvEnum;
//using Emgu.CV.Features2D;
//using Emgu.CV.Flann;
//using Emgu.CV.ML;
//using Emgu.CV.Structure;
//using Emgu.CV.Util;
//using Emgu.CV.XFeatures2D;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Camera
//{
//    internal class Categorizer3 : ICategorizer
//    {
//        public string Name
//        {
//            get
//            {
//                return "Categorizer3";
//            }
//        }

//        public bool Train()
//        {
//            try
//            {
//                initDir();
//                Feature2D descriptorExtractor;
//                Feature2D featureDetector;
//                List<Mat> templates;
//                BOWKMeansTrainer bowtrainer;
//                BOWImgDescriptorExtractor bowDescriptorExtractor;
//                init(out descriptorExtractor, out featureDetector, out templates, out bowtrainer, out bowDescriptorExtractor);

//                List<Tuple<string, Mat>> train_set;
//                List<string> category_names;
//                make_train_set(out train_set, out category_names);

//                Mat vocab;
//                build_vocab(descriptorExtractor, featureDetector, templates, bowtrainer, out vocab);

//                bowDescriptorExtractor.SetVocabulary(vocab);

//                Dictionary<string, Mat> positive_data;
//                Dictionary<string, Mat> negative_data;
//                make_pos_neg(train_set, bowDescriptorExtractor, featureDetector, category_names, out positive_data, out negative_data);

//                this.train_classifiers(category_names, positive_data, negative_data);

//                return true;
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }
//        public event TrainedEventHandler Trained;
//        protected void OnTrained(string fn)
//        {
//            if (this.Trained != null)
//                this.Trained(fn);
//        }
//        public Categorizer3()
//        {
//        }
//        private Feature2D create_FeatureDetector()
//        {
//            return new SURF(500);
//            //return new KAZE();
//            //return new SIFT();
//            //return new Freak();
//        }
//        private BOWImgDescriptorExtractor create_bowDescriptorExtractor(Feature2D descriptorExtractor)
//        {
//            LinearIndexParams ip = new LinearIndexParams();
//            SearchParams sp = new SearchParams();
//            var descriptorMatcher = new FlannBasedMatcher(ip, sp);

//            return new BOWImgDescriptorExtractor(descriptorExtractor, descriptorMatcher);
//        }
//        private void init(out Feature2D descriptorExtractor, out Feature2D featureDetector, out List<Mat> templates, out BOWKMeansTrainer bowtrainer, out BOWImgDescriptorExtractor bowDescriptorExtractor)
//        {
//            int clusters = 1000;
//            featureDetector = create_FeatureDetector();

//            MCvTermCriteria term = new MCvTermCriteria(10000, 0.0001d);
//            term.Type = TermCritType.Iter | TermCritType.Eps;
//            bowtrainer = new BOWKMeansTrainer(clusters, term, 5, Emgu.CV.CvEnum.KMeansInitType.PPCenters);//****


//            BFMatcher matcher = new BFMatcher(DistanceType.L1);//****
//            descriptorExtractor = featureDetector;//******

//            bowDescriptorExtractor = create_bowDescriptorExtractor(descriptorExtractor);


//            templates = new List<Mat>();
//            string TEMPLATE_FOLDER = "C:\\Emgu\\book\\practical-opencv\\code\\src\\chapter8\\code8-5\\data\\templates";
//            //string TEMPLATE_FOLDER = "C:\\Emgu\\book\\practical-opencv\\code\\src\\chapter8\\code8-5\\data\\train_images";
//            foreach (var filename in Directory.GetFiles(TEMPLATE_FOLDER, "*", SearchOption.AllDirectories))
//            {
//                templates.Add(GetMat(filename, true));
//                this.OnTrained(filename);
//            }
//        }


//        void make_train_set(out List<Tuple<string, Mat>> train_set, out List<string> category_names)
//        {
//            string TRAIN_FOLDER = "C:\\Emgu\\book\\practical-opencv\\code\\src\\chapter8\\code8-5\\data\\train_images";

//            category_names = new List<string>();
//            train_set = new List<Tuple<string, Mat>>();
//            foreach (var dir in Directory.GetDirectories(TRAIN_FOLDER))
//            {
//                // Get category name from name of the folder
//                string category = new DirectoryInfo(dir).Name;
//                category_names.Add(category);
//                foreach (var filename in Directory.GetFiles(dir))
//                {
//                    train_set.Add(new Tuple<string, Mat>(category, GetMat(filename, true)));
//                    this.OnTrained(filename);
//                }
//            }
//        }

//        void build_vocab(Feature2D descriptorExtractor, Feature2D featureDetector, List<Mat> templates, BOWKMeansTrainer bowtrainer, out Mat vocab)
//        {
//            Mat vocab_descriptors = new Mat();
//            foreach (Mat templ in templates)
//            {
//                Mat desc = new Mat();
//                VectorOfKeyPoint kp = new VectorOfKeyPoint(featureDetector.Detect(templ));
//                descriptorExtractor.Compute(templ, kp, desc);
//                vocab_descriptors.PushBack(desc);
//            }

//            bowtrainer.Add(vocab_descriptors);
//            vocab = new Mat();
//            bowtrainer.Cluster(vocab);

//            string fn = getVocabularyFileName();
//            using (FileStorage fs = new FileStorage(fn, FileStorage.Mode.Write))
//            {
//                fs.Write(vocab, "vocab");
//                fs.ReleaseAndGetString();
//            }
//        }

//        void make_pos_neg(List<Tuple<string, Mat>> train_set, BOWImgDescriptorExtractor bowDescriptorExtractor, Feature2D featureDetector, List<string> category_names,
//            out Dictionary<string, Mat> positive_data, out Dictionary<string, Mat> negative_data)
//        {
//            positive_data = new Dictionary<string, Mat>();
//            negative_data = new Dictionary<string, Mat>();

//            foreach (var tu in train_set)
//            {
//                string category = tu.Item1;
//                Mat im = tu.Item2;
//                Mat feat = new Mat();

//                VectorOfKeyPoint kp = new VectorOfKeyPoint(featureDetector.Detect(im));
//                bowDescriptorExtractor.Compute(im, kp, feat);

//                for (int cat_index = 0; cat_index < category_names.Count; cat_index++)
//                {
//                    string check_category = category_names[cat_index];
//                    if (check_category.CompareTo(category) == 0)
//                    {
//                        if (!positive_data.ContainsKey(check_category))
//                            positive_data[check_category] = new Mat();
//                        positive_data[check_category].PushBack(feat);
//                    }
//                    else
//                    {
//                        if (!negative_data.ContainsKey(check_category))
//                            negative_data[check_category] = new Mat();
//                        negative_data[check_category].PushBack(feat);
//                    }
//                }
//            }
//        }
//        void train_classifiers(List<string> category_names, Dictionary<string, Mat> positive_data, Dictionary<string, Mat> negative_data)
//        {
//            for (int i = 0; i < category_names.Count; i++)
//            {
//                string category = category_names[i];

//                // Postive training data has labels 1
//                Mat train_data = positive_data[category];
//                Mat train_labels = new Mat(train_data.Rows, 1, DepthType.Cv32S, 1);
//                {
//                    for (int col = 0; col < train_labels.Cols; col++)
//                        for (int row = 0; row < train_labels.Rows; row++)
//                            train_labels.SetValue(row, col, (int)1);
//                    train_labels.SetTo(new MCvScalar(1));


//                    // Negative training data has labels 0
//                    train_data.PushBack(negative_data[category]);
//                    Mat m = new Mat(negative_data[category].Rows, 1, DepthType.Cv32S, 1);
//                    {
//                        for (int col = 0; col < m.Cols; col++)
//                            for (int row = 0; row < m.Rows; row++)
//                                m.SetValue(row, col, (int)0);
//                        m.SetTo(new MCvScalar(0));

//                        train_labels.PushBack(m);
//                    }

//                    SVM svm = new SVM();
//                    svm.C = 312.5;
//                    svm.Gamma = 0.50625000000000009;
//                    svm.SetKernel(SVM.SvmKernelType.Rbf);
//                    svm.Type = SVM.SvmType.CSvc;

//                    svm.Train(train_data, Emgu.CV.ML.MlEnum.DataLayoutType.RowSample, train_labels);

//                    var fn = getSVMFileName(category);
//                    svm.SaveSVMToFile(fn);

//                    notFromFile[category] = svm;
//                }
//            }
//        }
//        Dictionary<string, SVM> notFromFile = new Dictionary<string, SVM>();//****


//        private void initDir()
//        {
//            var dir = getSaveDir();
//            if (Directory.Exists(dir))
//                foreach (var fn in Directory.GetFiles(dir))
//                    File.Delete(fn);
//        }
//        private string getSaveDir()
//        {
//            string dir = Path.Combine(Path.GetTempPath(), "Dece", "SVMS");
//            if (!Directory.Exists(dir))
//                Directory.CreateDirectory(dir);
//            return dir;
//        }
//        private string getSVMFileName(string category)
//        {
//            return Path.Combine(getSaveDir(), category + ".svm");
//        }
//        private string getVocabularyFileName()
//        {
//            return Path.Combine(getSaveDir(), "vocabulary.voc");
//        }


//        //[HandleProcessCorruptedStateExceptions]
//        public IEnumerable<ImageInfo> Categorize(IEnumerable<string> imageFileNames)
//        {
//            var featureDetector = create_FeatureDetector();
//            var bowDescriptorExtractor = create_bowDescriptorExtractor(featureDetector);

//            Mat vocab = new Mat();
//            using (var fs = new FileStorage(getVocabularyFileName(), FileStorage.Mode.Read))
//                fs["vocab"].ReadMat(vocab);


//            bowDescriptorExtractor.SetVocabulary(vocab);

//            Dictionary<string, string> svms = new Dictionary<string, string>();
//            foreach (var xml in Directory.GetFiles(getSaveDir(), "*.svm"))
//                svms.Add(Path.GetFileNameWithoutExtension(xml), xml);


//            Dictionary<string, SVM> dic = new Dictionary<string, SVM>();

//            foreach (var fn in imageFileNames)
//            {
//                string scoreTxt = Environment.NewLine;
//                float score = float.MaxValue;
//                //float score = float.MinValue;
//                string cat = "";

//                try
//                {
//                    using (Mat frame_g = GetMat(fn, false))
//                    {
//                        using (Mat img = new Mat())
//                        {
//                            VectorOfKeyPoint kp = new VectorOfKeyPoint(featureDetector.Detect(frame_g));

//                            bowDescriptorExtractor.Compute(frame_g, kp, img);

//                            foreach (var category in svms.Keys)
//                            {
//                                SVM svm = null;
//                                if (!dic.ContainsKey(category))
//                                {
//                                    string svmFn = svms[category];
//                                    svm = new SVM();
//                                    svm.LoadSVMFromFile(svmFn);
//                                    dic[category] = svm;
//                                }
//                                else
//                                    svm = dic[category];
//                                svm = notFromFile[category];//*************

//                                float classVal = svm.Predict(img, null);
//                                float scoreVal = svm.Predict(img, null, 1);
//                                //float signMul = (classVal < 0) == (scoreVal < 0) ? 1f : -1f;
//                                //float score1 = signMul * scoreVal;

//                                scoreTxt += string.Format("{0}-{1}: {2}{3}", category, classVal.ToString(), scoreVal.ToString("N3"), Environment.NewLine);

//                                if (scoreVal < score)
//                                {
//                                    score = scoreVal;
//                                    cat = category;
//                                }
//                            }
//                        }
//                    }
//                }
//                catch (Exception)
//                {
//                    score = 0f;
//                    cat = "hata";
//                }
//                if (string.IsNullOrEmpty(cat))
//                    score = 0f;
//                yield return new ImageInfo(fn, cat, scoreTxt);
//            }
//        }


//        private static object matLocker = new object();
//        public Mat GetMat(string fn, bool train)
//        {
//            lock (matLocker)
//            {
//                var mat = new Mat(fn, ImreadModes.Color);
//                var mat2 = new Mat();
//                mat.ConvertTo(mat2, DepthType.Cv8U);
//                return mat2;
//            }
//        }
//    }
//}
