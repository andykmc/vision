using System;
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
            string imageName = MatchingHelper.matchedClass;
            // this is the service root uri for the Bing search service  
            var serviceRootUri = new Uri("https://api.datamarket.azure.com/Data.ashx/Bing/Search/v1");

            // this is the Account Key generated for this app 
            var accountKey = "TG4vg/aL3DjSYJVl25mmyPVHhCCLCuObnR26ggQZhWc=";

            // the BingSearchContainer gives us access to the Bing services 
            BingSearchContainer bsc = new BingSearchContainer(serviceRootUri);

            // Give the BingSearchContainer access to the subscription 
            bsc.Credentials = new NetworkCredential(accountKey, accountKey);

            // building the query
            var imageQuery = bsc.Image(imageName, null, null, "Strict", null, null, "Size:Medium");

            IEnumerable<ImageResult> imageResults = imageQuery.Execute();
            backgroundWorkerBing.ReportProgress(99, imageResults);
        }

        public void backgroundWorkerBing_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            IEnumerable<ImageResult> imageResults = (IEnumerable<ImageResult>)e.UserState;
            int i = 1;
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
        }

        public void backgroundWorkerBing_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {}

       
	}
}