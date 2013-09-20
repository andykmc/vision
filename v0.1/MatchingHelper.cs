using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using matching_test01;
using MathWorks.MATLAB.NET.Arrays;

namespace v0_1
{
    static class MatchingHelper
    {
        public static string matchedClass;
        public static bool isReranking = false;
        public static int rerankingImageName = -1;//-1 means not doing reranking
        public static double[] resultsForReranking ;//stores the image number when images are categories covered by similarity matrix

        public static string matchClass()
        {
            //MessageBox.Show("match initialize");
            MatchingClass matchtest = null;
            MWCharArray output = null;
            MWArray MWresult = null;

            matchtest = new MatchingClass();
            //MessageBox.Show("start match");
            MWresult = matchtest.build_matching_test_correct_csharp2();
            output = (MWCharArray)MWresult[1, 1];
            matchedClass = output.ToString();
            isReranking = false;
            rerankingImageName = -1;
            return matchedClass;
        }

        public static double[] SearchSimilar(int targetImageNum, int numberOfImageToRetrieve)
        {
            // To declare a 500x500 matrix 
            double[][] similarity = new double[500][];
            for (int x = 0; x < similarity.Length; x++)
            {
                similarity[x] = new double[500];
            }

            // To read the similarity matrix .txt file
            string[] lines = System.IO.File.ReadAllLines(@"similarity_matrix_new.txt");


            int result;
            int count = 0;

            // To construct the similarity matrix
            for (int i = 0; i < 500; i++)
                for (int j = 0; j < 500; j++)
                {
                    result = Convert.ToInt32(lines[count]);
                    similarity[i][j] = result / 10000.0;
                    count = count + 1;
                }

            // To re-rank the image
            double[][] arr = new double[2][];
            arr[0] = new double[500];    // To store the image names, all names are integers from 1 to 500
            arr[1] = new double[500];    // To store the similarity values

            // To perform the sorting of the similarity values
            // The smaller the value,  the more similar to the input
            ISort(targetImageNum, arr, 500, similarity);

            resultsForReranking = new double[numberOfImageToRetrieve];
            for (int i = 0; i < numberOfImageToRetrieve; i++)
            {
                resultsForReranking[i] = arr[0][i];
            }


            return resultsForReranking;
            // To show the first 14 image names and their corresponding similarity values
            //for (int i = 0; i < 2; i++)
            //{
            //    for (int j = 0; j < 14; j++)
            //        Console.Write("{0} ", arr[i][j]);
            //    Console.WriteLine();
            //}
        }

        static void ISort(int n, double[][] arr, int N, double[][] similarity)
        {
            // Description
            // To sort the arr[1] from smallest to largest

            // Inputs
            // n: the name of input image
            // arr: a 2d array to store the simlarity value related to the input image
            // N: size of the the array, default is 500
            // similarity: 500x500 similarity matrix, default is similarity

            for (int i = 0; i < N; i++)
            {
                arr[0][i] = i + 1;
                arr[1][i] = similarity[n][i];
            }

            for (int i = 0; i < N - 1; i++)
            {
                int minIdx = i;

                for (int j = i + 1; j < N; j++)
                    if (arr[1][minIdx] > arr[1][j])
                        minIdx = j;

                if (minIdx != i)
                {
                    double temp1 = arr[0][i], temp2 = arr[1][i];
                    arr[0][i] = arr[0][minIdx];
                    arr[0][minIdx] = temp1;
                    arr[1][i] = arr[1][minIdx];
                    arr[1][minIdx] = temp2;
                }
            }
        }
    }
}
