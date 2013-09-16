using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Windows.Ink;

namespace v0_1
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        //for viewer animation effect
        AnimatedViewer animatedViewer;
        //for changing pages
        ViewControlHelper viewControlHelper;
        //for gesture control
        BackgroundWorker backgroundWorker1;
        BackgroundWorker backgroundWorker2;
        Point oldPoint;
        Point newPoint;
        List<DependencyObject> hitResultsList;
        Button hoveredButton;
        InkCanvas hoveredInkCanvas;
        int opennesThreadshold;
        bool needDebouncing;
        bool inkCanvasMouseDown;
        IDisposable debouncingTimer;

		public MainWindow()
		{
            this.InitializeComponent();
            //for viewer animation effect
            animatedViewer = new AnimatedViewer(this);
            //for changing pages
            viewControlHelper = ViewControlHelper.Instance;
            //for gesture control
            backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(this.backgroundWorker1_DoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.RunWorkerAsync();
            /*backgroundWorker2 = new BackgroundWorker();
            backgroundWorker2.WorkerReportsProgress = true;
            backgroundWorker2.WorkerSupportsCancellation = true;
            backgroundWorker2.DoWork += new DoWorkEventHandler(this.backgroundWorker2_DoWork);
            backgroundWorker2.ProgressChanged += new ProgressChangedEventHandler(this.backgroundWorker2_ProgressChanged);
            //backgroundWorker2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            backgroundWorker2.RunWorkerAsync();*/
            hitResultsList = new List<DependencyObject>();
            opennesThreadshold = 20;
            needDebouncing = false;
            hoveredButton = null;
            hoveredInkCanvas = null;
            debouncingTimer = null;
            inkCanvasMouseDown = false;
		}

        private void goDrawViewButton_Click(object sender, RoutedEventArgs e)
        {
            //viewList.SelectedIndex = (int)views.view_draw;
            viewControlHelper.gotoView(views.view_draw);
        }

        private void goVoiceViewButton_Click(object sender, RoutedEventArgs e)
        {
            //viewList.SelectedIndex = (int)views.view_voice;
            viewControlHelper.gotoView(views.view_voice);
        }
        public void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            VoicePipeline pipeline = new VoicePipeline();
            pipeline.SetVoiceCommands();
            pipeline.EnableVoiceRecognition(0);
            if (!pipeline.Init())
            {
                return;
            }
            MessageBox.Show("Voice pipline initialized");
            while (true)
            {
                //check if need to terminate the pipline when window is closing
                if (backgroundWorker2.CancellationPending)
                {
                    pipeline.PauseVoiceRecognition(true);
                    pipeline.ReleaseFrame();
                    pipeline.Close();
                    pipeline.Dispose();
                    MessageBox.Show("stop backgroundWorker2");
                    e.Cancel = true; return;
                }

                if (pipeline.AcquireFrame(true))
                {
                    if (pipeline.IsAudioFrame())
                    {
                        MyVoiceParams detectedVoiceParams = pipeline.getDetectedPhrase();
                        if (detectedVoiceParams.detectedPhrase != "")
                        {
                            backgroundWorker2.ReportProgress(1, detectedVoiceParams);
                        }
                    }
                    else
                    {
                        pipeline.ReleaseFrame();
                        continue;
                    }
                }
                else
                {
                    MessageBox.Show("Failed to initialize VoicePipeline");
                    break;
                }

                pipeline.ReleaseFrame();
            }
            pipeline.Close();
            pipeline.Dispose();
        }

        public void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            GesturePipeline pipeline = new GesturePipeline();
            PXCMGesture.Alert.Label alertLabel = PXCMGesture.Alert.Label.LABEL_ANY;
            MyGestureParams geoNodeParams = new MyGestureParams();

            pipeline.Init();
            MessageBox.Show("Gesture pipline initialized");
            backgroundWorker1.ReportProgress(2);
            while (true)
            {
                //check if need to terminate the pipline when window is closing
                if (backgroundWorker1.CancellationPending)
                {
                    pipeline.PauseGesture(true);
                    pipeline.ReleaseFrame();
                    pipeline.Close();
                    pipeline.Dispose();
                    MessageBox.Show("stop backgroundWorker1");
                    e.Cancel = true; return;
                }

                if (pipeline.AcquireFrame(true))
                {
                    if (pipeline.IsImageFrame())
                    {
                        if (alertLabel != pipeline.geoNodeParams.alertLabel)
                        { alertLabel = pipeline.geoNodeParams.alertLabel; }
                        //check existance of geoNode
                        if ((alertLabel != PXCMGesture.Alert.Label.LABEL_ANY) &&
                            (alertLabel != PXCMGesture.Alert.Label.LABEL_GEONODE_INACTIVE) &&
                            (alertLabel != PXCMGesture.Alert.Label.LABEL_FOV_BLOCKED))
                        {
                            geoNodeParams = pipeline.geoNodeParams;
                            alertLabel = geoNodeParams.alertLabel;
                            backgroundWorker1.ReportProgress(1, geoNodeParams);
                        }
                        else
                        {
                            geoNodeParams.reset();
                            geoNodeParams.alertLabel = alertLabel;
                            backgroundWorker1.ReportProgress(1, geoNodeParams);
                        }
                    }
                    else
                    {
                        pipeline.ReleaseFrame();
                        continue;
                    }
                    if (!pipeline.ReleaseFrame()) break;
                }
                else
                {
                    MessageBox.Show("Failed to initialize GesturePipeline");
                    break;
                }
            }
            pipeline.Dispose();
        }
        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            MyVoiceParams detectedVoiceParams = (MyVoiceParams)e.UserState;
            if (detectedVoiceParams.alertLabel == "" && detectedVoiceParams.confidenceLevel > 40)
            { MessageBox.Show("Phrase:" + detectedVoiceParams.detectedPhrase + " Confidence:" + detectedVoiceParams.confidenceLevel + " Alert:" + detectedVoiceParams.alertLabel); }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 2)
            {
                backgroundWorker2 = new BackgroundWorker();
                backgroundWorker2.WorkerReportsProgress = true;
                backgroundWorker2.WorkerSupportsCancellation = true;
                backgroundWorker2.DoWork += new DoWorkEventHandler(this.backgroundWorker2_DoWork);
                backgroundWorker2.ProgressChanged += new ProgressChangedEventHandler(this.backgroundWorker2_ProgressChanged);
                backgroundWorker2.RunWorkerAsync();
                return;
            }

            MyGestureParams geoNodeParams = new MyGestureParams();
            geoNodeParams = (MyGestureParams)e.UserState;
            MyGesturePath myPath = null;
            
            //move the hand
            XLabel.Content = geoNodeParams.nodeX.ToString();
            YLabel.Content = geoNodeParams.nodeY.ToString();

            //check existance of geoNode
            if ((geoNodeParams.alertLabel != PXCMGesture.Alert.Label.LABEL_ANY) &&
                (geoNodeParams.alertLabel != PXCMGesture.Alert.Label.LABEL_GEONODE_INACTIVE) &&
                (geoNodeParams.alertLabel != PXCMGesture.Alert.Label.LABEL_FOV_BLOCKED))
            {
                NativeMethods.SetCursorPos(
                    (int)Application.Current.MainWindow.Left + (int)SystemParameters.FixedFrameVerticalBorderWidth + 1 + 640 - (int)geoNodeParams.nodeX * 2,
                    (int)Application.Current.MainWindow.Top + (int)SystemParameters.FixedFrameHorizontalBorderHeight + (int)SystemParameters.WindowCaptionHeight + 1 + (int)geoNodeParams.nodeY * 2
                );

                if ((geoNodeParams.alertLabel == PXCMGesture.Alert.Label.LABEL_FOV_LEFT) ||
                    (geoNodeParams.alertLabel == PXCMGesture.Alert.Label.LABEL_FOV_RIGHT) ||
                    (geoNodeParams.alertLabel == PXCMGesture.Alert.Label.LABEL_FOV_TOP) ||
                    (geoNodeParams.alertLabel == PXCMGesture.Alert.Label.LABEL_FOV_BOTTOM))
                {
                    //signalLight.FillColor = Color.Yellow;
                    //signalLabel.Text = "Too close to dection boundary";
                }
                else
                {
                   // signalLight.FillColor = Color.Green;
                    //signalLabel.Text = "Under detection";
                }

                newPoint = new Point(640 - Convert.ToInt32(geoNodeParams.nodeX) * 2, Convert.ToInt32(geoNodeParams.nodeY) * 2);
                //check opennes
                if (geoNodeParams.opennes > opennesThreadshold)
                {
                    this.hand_palm.Visibility = Visibility.Visible;
                    this.hand_grip.Visibility = Visibility.Hidden;

                    //Raise the InkCanvas mouse up event
                    //if (hoveredInkCanvas != null)
                    if (hoveredInkCanvas != null && inkCanvasMouseDown == true)
                    {
                        //======Start of method 1========
                        hoveredInkCanvas.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Left) { RoutedEvent = InkCanvas.MouseUpEvent });
                        inkCanvasMouseDown = false;
                        hoveredInkCanvas.ReleaseMouseCapture();
                        //=======End of method 1=========
                    }
                }
                else if (geoNodeParams.opennes >= 0 && geoNodeParams.opennes <= opennesThreadshold)
                {
                    this.hand_palm.Visibility = Visibility.Hidden;
                    this.hand_grip.Visibility = Visibility.Visible;

                    //draw line
                    //SolidBrush color = new SolidBrush(Color.Black);

                    if ((oldPoint.X == -1) || (oldPoint.Y == -1))
                    {
                        oldPoint = newPoint;
                    }
                    else
                    {
                        myPath = new MyGesturePath(oldPoint, newPoint);
                        
                        /*if ((newPoint.X > 160) && (newPoint.X < 480) &&
                            (newPoint.Y > 110) && (newPoint.Y < 430))
                        {
                            myPath = new MyGesturePath(oldPoint, newPoint);

                            if ((oldPoint.X != newPoint.X) || (oldPoint.Y != newPoint.Y))
                            {
                                //this.myPathList.Add(myPath);                                
                            }
                            //panel1.Invalidate();
                            //drawingCanvas.Invalidate();
                            // signalLight.Invalidate();
                        }*/
                    }
                    // Perform actions on the hit test results list.
                    //Raise the Button mouse click event
                    if (hoveredButton != null && needDebouncing == false)
                    {
                        needDebouncing = true;
                        hoveredButton.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Left) { RoutedEvent = Button.ClickEvent });                        
                        debouncingTimer = EasyTimer.SetTimeout(() =>
                        {
                            needDebouncing = false;                            
                        }, 1000);
                    }
                    //Raise the InkCanvas mouse down event
                    //if (hoveredInkCanvas != null)
                    if (hoveredInkCanvas != null && inkCanvasMouseDown == false)
                    {
                        //======Start of method 1========(to be used with Raising InkcCanvas mouse down event)
                        /* Problem: can not correct show the drawn immediately, of shown after finishing one stroke
                         * Findings: 1. this method is done by simulating mouse down event
                         *           2. may be missing the event for adding stroke to stroke property of InkCanvas
                         */
                        inkCanvasMouseDown = true;
                        hoveredInkCanvas.Focus();
                        hoveredInkCanvas.CaptureMouse();
                        hoveredInkCanvas.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Left) { RoutedEvent = InkCanvas.MouseDownEvent });
                        //=======End of method 1=========

                        
                        //======Start of method 2========
                        /*
                         * Problem: can show the stroke correctly, but very high latency
                         * Findings: probably because this method simply add child to Inkcanvas, processing power and memory consuming
                        Line linetodraw = new Line();
                        linetodraw.X1 = myPath.OldPoint.X;//0
                        linetodraw.Y1 = myPath.OldPoint.Y;//0
                        linetodraw.X2 = myPath.NewPoint.X;//10
                        linetodraw.Y2 = myPath.NewPoint.Y;//10
                        linetodraw.StrokeThickness = 2;
                        linetodraw.Stroke = new SolidColorBrush(Colors.Black);
                        this.hoveredInkCanvas.Children.Add(linetodraw);
                        */
                        //=======End of method 1=========

                        //======Method 3 under investigation=====
                        /*
                         * Target: 
                        StrokeCollection strokeCollection = new StrokeCollection();
                        Stroke stroke = new Stroke();
                        strokeCollection.
                        */
                    }
                }
                //if (geoNodeParams.opennes <= opennesThreadshold) selectable = false;
                oldPoint = newPoint;

                if (geoNodeParams.gestureLabel == PXCMGesture.Gesture.Label.LABEL_NAV_SWIPE_DOWN)
                {
                    viewControlHelper.gotoPreviousView();
                }
            }
            else
            {
                this.hand_palm.Visibility = Visibility.Hidden;
                this.hand_grip.Visibility = Visibility.Hidden;
            }
            var viewString = "Views:";
            foreach (var view in viewControlHelper.getViewsHistory())
            {
                viewString += view.ToString() + ",";
            }
            this.testLabel.Content = viewString;
            //this.testLabel.Content = viewControlHelper.previousViews.Count().ToString();
            this.currentPageLabel.Content = viewControlHelper.getCurrentView().ToString();
            this.debouncingLabel.Content = needDebouncing.ToString();

            //capture the events
            var events = EventManager.GetRoutedEvents();
            foreach (var routedEvent in events)
            {
                EventManager.RegisterClassHandler(typeof(UserControl), routedEvent, new RoutedEventHandler(handler));
            }
            eventBox.ScrollToEnd();
        }

        public void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        { }

        //capture the events
        internal static void handler(object sender, RoutedEventArgs e)
        {            
            MessageBox.Show(e.RoutedEvent.ToString());
        }
        
        //Render the captured view into the Rectangle
        private void viewList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //viewControlHelper.viewsHistory.Add((views)viewList.SelectedIndex);
            animatedViewer.RenderCapturedView();
        }
        //For controlling the cursor
        public partial class NativeMethods
        {
            /// Return Type: BOOL->int  
            ///X: int  
            ///Y: int  
            [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "SetCursorPos")]
            [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
            public static extern bool SetCursorPos(int X, int Y);
        }

        //for dete traversing down the 
        public static T FindInTree<T>(DependencyObject start) where T : class
        {
            while (start != null && !(start is T))
                start = VisualTreeHelper.GetParent(start);
            return start as T;
        }
        private void ProcessHitTestResultsList()
        {   
            foreach (var item in hitResultsList)
            {
                //Search for the top most button
                //MessageBox.Show(item.ToString(), "process", MessageBoxButton.OKCancel);                
                var tempButtonBase = FindInTree<Button>(item);
                if (tempButtonBase != null && tempButtonBase.GetType() == typeof(Button))
                {
                    //MessageBox.Show("process hit button", "process", MessageBoxButton.OKCancel);
                    hoveredButton = tempButtonBase;
                    break;
                }
                else
                {
                    hoveredButton = null;
                }
                //Then search for the top most InkCanvas
                var tempInkCanvasBase = FindInTree<InkCanvas>(item);
                if (tempInkCanvasBase != null && tempInkCanvasBase.GetType() == typeof(InkCanvas))
                {
                    hoveredInkCanvas = tempInkCanvasBase;
                    break;
                }
                else
                {
                    hoveredInkCanvas = null;
                }
            }
            //MessageBox.Show("bottom reached", "process", MessageBoxButton.OKCancel);
        }

        //Respond to the mouse move event by setting up a hit test filter and results enumeration.
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {            
            Point pt = e.GetPosition(this);
            hitResultsList.Clear();
            //VisualTreeHelper.HitTest(LayoutRoot, new HitTestFilterCallback(MyHitTestFilter),
            //    new HitTestResultCallback(MyHitTestResult), new PointHitTestParameters(pt));
            VisualTreeHelper.HitTest(LayoutRoot, null,
               new HitTestResultCallback(MyHitTestResult), new PointHitTestParameters(pt));
            // Perform actions on the hit test results list. 
            if (hitResultsList.Count > 0)
            {
                ProcessHitTestResultsList();
            }
        }
        // Return the result of the hit test to the callback. 
        public HitTestResultBehavior MyHitTestResult(HitTestResult result)
        {
            // Add the hit test result to the list that will be processed after the enumeration.
            hitResultsList.Add(result.VisualHit);
            return HitTestResultBehavior.Stop;

            // Set the behavior to return visuals at all z-order levels. 
            //return HitTestResultBehavior.Continue;
        }
        // Filter the hit test values for each object in the enumeration.
        public HitTestFilterBehavior MyHitTestFilter(DependencyObject o)
        {
            // Test for the object value you want to filter
            if (o.GetType() == typeof(ItemsControl))
            {
                // Visual object and descendants are NOT part of hit test results enumeration.                
                return HitTestFilterBehavior.ContinueSkipSelfAndChildren;                                
            }
            else
            {
                // Visual object is part of hit test results enumeration. 
                return HitTestFilterBehavior.Continue;
            }
        }
        //terminate the pipline when window is closing
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            
            backgroundWorker1.CancelAsync();
            backgroundWorker2.CancelAsync();
        }

        private void previousViewButton_Click(object sender, RoutedEventArgs e)
        {
            viewControlHelper.gotoPreviousView();
        }
	}

    public enum views
    {
        view_home,
        view_draw,
        view_voice,
        view_result,
        view_none
    }
}