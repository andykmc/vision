using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using Bing;
using System.Net;

namespace v0_1
{
	public partial class view_result
	{
        ViewControlHelper viewControlHelper;
        BackgroundWorker backgroundWorkerBing;
        const int numberOfImagesToRetrieve = 5;

		public view_result()
		{
			this.InitializeComponent();

			// Insert code required on object creation below this point.
		}

        private void ResultPage_Loaded(object sender, RoutedEventArgs e)
        {
            viewControlHelper = ViewControlHelper.Instance;
            ResultNameTextBox.Text = MatchingHelper.matchedClass;
            backgroundWorkerBing = new BackgroundWorker();
            backgroundWorkerBing.WorkerReportsProgress = true;
            backgroundWorkerBing.WorkerSupportsCancellation = true;
            backgroundWorkerBing.DoWork += new DoWorkEventHandler(this.backgroundWorkerBing_DoWork);
            backgroundWorkerBing.ProgressChanged += new ProgressChangedEventHandler(this.backgroundWorkerBing_ProgressChanged);
            backgroundWorkerBing.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorkerBing_RunWorkerCompleted);
            backgroundWorkerBing.RunWorkerAsync();
        }

        public void backgroundWorkerBing_DoWork(object sender, DoWorkEventArgs e)
        {
            string targetImageClass = MatchingHelper.matchedClass;

            if (MatchingHelper.isReranking && MatchingHelper.rerankingImageName != -1)
            {
                double[] rerankedImages = MatchingHelper.SearchSimilar(MatchingHelper.rerankingImageName, numberOfImagesToRetrieve);
                backgroundWorkerBing.ReportProgress(50, rerankedImages);
                return;
            }
            //check if the class of the image is covered by our similarity matching
            for (int j = 0; j < 20; j++)
            {
                string command = VoiceControlHelper.voiceCommands[j];
                if (targetImageClass == command)
                {
                    MatchingHelper.isReranking = false;
                    List<double> randomChosedImages = new List<double>();
                    Random randomGen = new Random();
                    for (int k = 0; k < numberOfImagesToRetrieve; k++)
                    {
                        double imageNum = 0;
                        do
                        {
                            imageNum = j * 25 + randomGen.Next(1, 26);
                        } while (randomChosedImages.Contains(imageNum));
                        randomChosedImages.Add(imageNum);
                    }
                    backgroundWorkerBing.ReportProgress(50, randomChosedImages.ToArray());
                    return;
                }
            }

            MatchingHelper.isReranking = false;
            // this is the service root uri for the Bing search service  
            var serviceRootUri = new Uri("https://api.datamarket.azure.com/Data.ashx/Bing/Search/v1");

            // this is the Account Key generated for this app 
            var accountKey = "TG4vg/aL3DjSYJVl25mmyPVHhCCLCuObnR26ggQZhWc=";

            // the BingSearchContainer gives us access to the Bing services 
            BingSearchContainer bsc = new BingSearchContainer(serviceRootUri);

            // Give the BingSearchContainer access to the subscription 
            bsc.Credentials = new NetworkCredential(accountKey, accountKey);

            // building the query
            var imageQuery = bsc.Image(targetImageClass, null, null, "Strict", null, null, "Size:Medium");

            IEnumerable<ImageResult> imageResults = imageQuery.Execute();
            backgroundWorkerBing.ReportProgress(99, imageResults);
        }

        public void backgroundWorkerBing_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int i = 1;
            switch ((int)e.ProgressPercentage)
            {
                case 50://case of the image category is being covered by the similarity matrix
                    double[] rerankedImages = (double[])e.UserState;
                    MatchingHelper.resultsForReranking = rerankedImages;
                    for (i = 0; i < rerankedImages.Length; i++)
                    {
                        Ellipse image = (Ellipse)this.FindName("Image" + (i+1).ToString());
                        ImageBrush bimage = new ImageBrush();
                        BitmapSource source = new BitmapImage(new Uri("file:///" + Directory.GetCurrentDirectory() + "/similarity_images/" + rerankedImages[i].ToString() + ".jpg", UriKind.Absolute));
                        bimage.ImageSource = source;
                        image.Fill = bimage;
                    }
                    break;
                default:
                    IEnumerable<ImageResult> imageResults = (IEnumerable<ImageResult>)e.UserState;
                    
                    foreach (var result in imageResults)
                    {
                        Ellipse image = (Ellipse) this.FindName("Image" + i.ToString());
                        ImageBrush bimage = new ImageBrush();
                        BitmapSource source = new BitmapImage(new Uri(result.MediaUrl, UriKind.Absolute));
                        bimage.ImageSource = source;
                        image.Fill = bimage;
                        i++;
                        if (i == 6)
                        { break; }
                    }
                    break;
            }
        }

        public void backgroundWorkerBing_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {}

        private void rerankButton_Click(object sender, RoutedEventArgs e)
        {
            if (rerankButton.Opacity != 1)
            { 
                rerankButton.Opacity = 1;
                SolidColorBrush greenBrush = new SolidColorBrush();
                greenBrush.Color = (Color)ColorConverter.ConvertFromString("#FF96E971");
                this.Image1.StrokeThickness = 4;
                this.Image1.Stroke = greenBrush;
                this.Image2.StrokeThickness = 4;
                this.Image2.Stroke = greenBrush;
                this.Image3.StrokeThickness = 4;
                this.Image3.Stroke = greenBrush;
                this.Image4.StrokeThickness = 4;
                this.Image4.Stroke = greenBrush;
                this.Image5.StrokeThickness = 4;
                this.Image5.Stroke = greenBrush;
            }
            else
            { 
                rerankButton.Opacity = 0.6;
                rerankButton.InvalidateVisual();
                SolidColorBrush blackBrush = new SolidColorBrush();
                blackBrush.Color = Colors.Black;
                this.Image1.StrokeThickness = 1;
                this.Image1.Stroke = blackBrush;
                this.Image2.StrokeThickness = 1;
                this.Image2.Stroke = blackBrush;
                this.Image3.StrokeThickness = 1;
                this.Image3.Stroke = blackBrush;
                this.Image4.StrokeThickness = 1;
                this.Image4.Stroke = blackBrush;
                this.Image5.StrokeThickness = 1;
                this.Image5.Stroke = blackBrush;
            }

            
        }

        private void Image1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Image1.StrokeThickness == 4)
            {
                MatchingHelper.isReranking = true;
                MatchingHelper.rerankingImageName = (int)MatchingHelper.resultsForReranking[0];
                this.ResultPage_Loaded(new object(), new RoutedEventArgs());
            }
        }

        private void Image2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Image2.StrokeThickness == 4)
            {
                MatchingHelper.isReranking = true;
                MatchingHelper.rerankingImageName = (int)MatchingHelper.resultsForReranking[1];
                this.ResultPage_Loaded(new object(), new RoutedEventArgs());
            }
        }

        private void Image3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Image3.StrokeThickness == 4)
            {
                MatchingHelper.isReranking = true;
                MatchingHelper.rerankingImageName = (int)MatchingHelper.resultsForReranking[2];
                this.ResultPage_Loaded(new object(), new RoutedEventArgs());
            }
        }

        private void Image4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Image4.StrokeThickness == 4)
            {
                MatchingHelper.isReranking = true;
                MatchingHelper.rerankingImageName = (int)MatchingHelper.resultsForReranking[3];
                this.ResultPage_Loaded(new object(), new RoutedEventArgs());
            }
        }

        private void Image5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Image5.StrokeThickness == 4)
            {
                MatchingHelper.isReranking = true;
                MatchingHelper.rerankingImageName = (int)MatchingHelper.resultsForReranking[4];
                this.ResultPage_Loaded(new object(), new RoutedEventArgs());
            }
        }       
	}
}