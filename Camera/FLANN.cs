using Emgu.CV;
using Emgu.CV.Features2D;
using Emgu.CV.Flann;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.XFeatures2D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Camera
{
    public class FLANN
    {
        Matrix<float> dbDescs;
        IList<IndecesMapping> imap;
        private List<String> DirSearch(string sDir)
        {
            List<String> files = new List<String>();
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    files.Add(f);
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    files.AddRange(DirSearch(d));
                }
            }
            catch (System.Exception excpt)
            {
                MessageBox.Show(excpt.Message);
            }

            return files;
        }
        public Matrix<float> LoadData(string path)
        {
            List<String> list = DirSearch(Application.StartupPath + "\\" + path);
            imap = new List<IndecesMapping>();
            string queryImage = "D:\\bien-so-xe-cac-tinh_0.jpg";


            // compute descriptors for each image
            var dbDescsList = ComputeMultipleDescriptors(list, out imap);

            // concatenate all DB images descriptors into single Matrix
            dbDescs = ConcatDescriptors(dbDescsList);
            return dbDescs;
        }
        public IList<IndecesMapping> Reconize(Image<Gray, byte> queryImage)
        {
            Matrix<float> queryDescriptors = ComputeSingleDescriptors(queryImage);

            FindMatches(dbDescs, queryDescriptors, ref imap);

            return imap;
        }

        public void ResetSimilarity()
        {
            foreach (var item in imap)
            {
                item.Similarity = 0;
            }
        }
        public IList<IndecesMapping> Match()
        {
            List<String> list = DirSearch("D:\\test");
            string queryImage = "D:\\bien-so-xe-cac-tinh_0.jpg";

            IList<IndecesMapping> imap;

            // compute descriptors for each image
            var dbDescsList = ComputeMultipleDescriptors(list, out imap);

            // concatenate all DB images descriptors into single Matrix
            Matrix<float> dbDescs = ConcatDescriptors(dbDescsList);

            // compute descriptors for the query image
            Matrix<float> queryDescriptors = ComputeSingleDescriptors(queryImage);

            FindMatches(dbDescs, queryDescriptors, ref imap);

            return imap;
        }
        public IList<IndecesMapping> Match(Image<Gray, byte> image)
        {
            List<String> list = DirSearch("D:\\test");

            IList<IndecesMapping> imap;

            // compute descriptors for each image
            var dbDescsList = ComputeMultipleDescriptors(list, out imap);

            // concatenate all DB images descriptors into single Matrix
            Matrix<float> dbDescs = ConcatDescriptors(dbDescsList);

            // compute descriptors for the query image
            Matrix<float> queryDescriptors = ComputeSingleDescriptors(image);

            FindMatches(dbDescs, queryDescriptors, ref imap);

            return imap;
        }
        /// <summary>
        /// Computes image descriptors.
        /// </summary>
        /// <param name="fileName">Image filename.</param>
        /// <returns>The descriptors for the given image.</returns>
        public Matrix<float> ComputeSingleDescriptors(string fileName) // old return Matrix<float>
        {
            Mat descsTmp = new Mat();

            using (Image<Gray, byte> img = new Image<Gray, byte>(fileName))
            {
                #region depreciated
                //VectorOfKeyPoint keyPoints = detector.DetectKeyPointsRaw(img, null);
                //descs = detector.ComputeDescriptorsRaw(img, null, keyPoints);
                #endregion

                VectorOfKeyPoint keyPoints = new VectorOfKeyPoint();
                detector.DetectAndCompute(img, null, keyPoints, descsTmp, false);
            }

            Matrix<float> descs = new Matrix<float>(descsTmp.Rows, descsTmp.Cols);
            descsTmp.CopyTo(descs);

            return descs;
        }
        public Matrix<float> ComputeSingleDescriptors(Image<Gray, byte> image) // old return Matrix<float>
        {
            Mat descsTmp = new Mat();

            #region depreciated
            //VectorOfKeyPoint keyPoints = detector.DetectKeyPointsRaw(img, null);
            //descs = detector.ComputeDescriptorsRaw(img, null, keyPoints);
            #endregion

            VectorOfKeyPoint keyPoints = new VectorOfKeyPoint();
            detector.DetectAndCompute(image, null, keyPoints, descsTmp, false);

            Matrix<float> descs = new Matrix<float>(descsTmp.Rows, descsTmp.Cols);
            descsTmp.CopyTo(descs);

            return descs;
        }
        /// <summary>
        /// Convenience method for computing descriptors for multiple images.
        /// On return imap is filled with structures specifying which descriptor ranges in the concatenated matrix belong to what image. 
        /// </summary>
        /// <param name="fileNames">Filenames of images to process.</param>
        /// <param name="imap">List of IndecesMapping to hold descriptor ranges for each image.</param>
        /// <returns>List of descriptors for the given images.</returns>
        public IList<Matrix<float>> ComputeMultipleDescriptors(List<String> fileNames, out IList<IndecesMapping> imap)
        {
            imap = new List<IndecesMapping>();

            IList<Matrix<float>> descs = new List<Matrix<float>>();

            int r = 0;

            for (int i = 0; i < fileNames.Count; i++)
            {
                var desc = ComputeSingleDescriptors(fileNames[i]);
                descs.Add(desc);

                imap.Add(new IndecesMapping()
                {
                    fileName = fileNames[i],
                    IndexStart = r,
                    IndexEnd = r + desc.Rows - 1
                });

                r += desc.Rows;
            }

            return descs;
        }
        /// <summary>
        /// Computes 'similarity' value (IndecesMapping.Similarity) for each image in the collection against our query image.
        /// </summary>
        /// <param name="dbDescriptors">Query image descriptor.</param>
        /// <param name="queryDescriptors">Consolidated db images descriptors.</param>
        /// <param name="images">List of IndecesMapping to hold the 'similarity' value for each image in the collection.</param>
        public void FindMatches(Matrix<float> dbDescriptors, Matrix<float> queryDescriptors, ref IList<IndecesMapping> imap)
        {
            var indices = new Matrix<int>(queryDescriptors.Rows, 2); // matrix that will contain indices of the 2-nearest neighbors found
            var dists = new Matrix<float>(queryDescriptors.Rows, 2); // matrix that will contain distances to the 2-nearest neighbors found

            // create FLANN index with 4 kd-trees and perform KNN search over it look for 2 nearest neighbours
            var flannIndex = new Index(dbDescriptors, new KdTreeIndexParams(4));
           // FlannBasedMatcher flannBasedMatcher = new FlannBasedMatcher(new KdTreeIndexParams(4), );
            flannIndex.KnnSearch(queryDescriptors, indices, dists, 2, 24);

            for (int i = 0; i < indices.Rows; i++)
            {
                // filter out all inadequate pairs based on distance between pairs
                if (dists.Data[i, 0] < (0.6 * dists.Data[i, 1]))
                {
                    // find image from the db to which current descriptor range belongs and increment similarity value.
                    // in the actual implementation this should be done differently as it's not very efficient for large image collections.
                    foreach (var img in imap)
                    {
                        if (img.IndexStart <= i && img.IndexEnd >= i)
                        {
                            img.Similarity++;
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Concatenates descriptors from different sources (images) into single matrix.
        /// </summary>
        /// <param name="descriptors">Descriptors to concatenate.</param>
        /// <returns>Concatenated matrix.</returns>
        public Matrix<float> ConcatDescriptors(IList<Matrix<float>> descriptors)
        {
            int cols = descriptors[0].Cols;
            int rows = descriptors.Sum(a => a.Rows);

            float[,] concatedDescs = new float[rows, cols];

            int offset = 0;

            foreach (var descriptor in descriptors)
            {
                // append new descriptors

                Buffer.BlockCopy(descriptor.ManagedArray, 0, concatedDescs, offset, sizeof(float) * descriptor.ManagedArray.Length);
                offset += sizeof(float) * descriptor.ManagedArray.Length;


            }

            return new Matrix<float>(concatedDescs);
        }
        public class IndecesMapping
        {
            public int IndexStart { get; set; }
            public int IndexEnd { get; set; }
            public int Similarity { get; set; }
            public string fileName { get; set; }
        }
        private const double surfHessianThresh = 300;
        private const bool surfExtendedFlag = true;
        private SURF detector = new SURF(1000);
    }
}
