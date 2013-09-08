﻿using System;
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
        Point oldPoint;
        Point newPoint;
        List<DependencyObject> hitResultsList;
        Button hoveredButton;
        int opennesThreadshold;
        bool selectable;

		public MainWindow()
		{
            this.InitializeComponent();
            //for viewer animation effect
            animatedViewer = new AnimatedViewer(this);
            //for changing pages
            viewControlHelper = new ViewControlHelper(this);
            //for gesture control
            this.backgroundWorker1 = new BackgroundWorker();
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new DoWorkEventHandler(this.backgroundWorker1_DoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            this.backgroundWorker1.RunWorkerAsync();
            hitResultsList = new List<DependencyObject>();
            opennesThreadshold = 20;
            selectable = true;
		}

        private void goDrawViewButton_Click(object sender, RoutedEventArgs e)
        {
            viewControlHelper.gotoView(views.view_draw);
        }

        private void goVoiceViewButton_Click(object sender, RoutedEventArgs e)
        {
            viewControlHelper.gotoView(views.view_voice);
        }
        
        public void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            MyPipeline pipeline = new MyPipeline();
            PXCMGesture.Alert.Label alertLabel = PXCMGesture.Alert.Label.LABEL_ANY;
            MyParams geoNodeParams = new MyParams();

            pipeline.Init();
            bool hasFrame;
            MessageBox.Show("pipline initialized");
            while (true)
            {
                hasFrame = pipeline.AcquireFrame(false);
                if (hasFrame)
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
                    if (!pipeline.ReleaseFrame()) break;
                }
                //check if need to terminate the pipline when window is closing
                if (backgroundWorker1.CancellationPending)
                { pipeline.Dispose(); e.Cancel = true; return; }
            }
            pipeline.Dispose();
        }
        
        public void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        { }        
        
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {            
            MyParams geoNodeParams = new MyParams();
            geoNodeParams = (MyParams)e.UserState;
            
            //move the hand
            XLabel.Content = geoNodeParams.nodeX.ToString();
            YLabel.Content = geoNodeParams.nodeY.ToString();
            
            NativeMethods.SetCursorPos(
                (int)Application.Current.MainWindow.Left + (int)SystemParameters.FixedFrameVerticalBorderWidth + 1 + 640 - (int)geoNodeParams.nodeX * 2,
                (int)Application.Current.MainWindow.Top + (int)SystemParameters.FixedFrameHorizontalBorderHeight + (int)SystemParameters.WindowCaptionHeight + 1 + (int)geoNodeParams.nodeY * 2
            );            

            //check existance of geoNode
            if ((geoNodeParams.alertLabel != PXCMGesture.Alert.Label.LABEL_ANY) &&
                (geoNodeParams.alertLabel != PXCMGesture.Alert.Label.LABEL_GEONODE_INACTIVE) &&
                (geoNodeParams.alertLabel != PXCMGesture.Alert.Label.LABEL_FOV_BLOCKED))
            {
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
                    selectable = true;
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
                        if ((newPoint.X > 160) && (newPoint.X < 480) &&
                            (newPoint.Y > 110) && (newPoint.Y < 430))
                        {
                            MyGesturePath myPath = new MyGesturePath(oldPoint, newPoint);

                            if ((oldPoint.X != newPoint.X) || (oldPoint.Y != newPoint.Y))
                            {
                                //this.myPathList.Add(myPath);                                
                            }
                            //panel1.Invalidate();
                            //drawingCanvas.Invalidate();
                            // signalLight.Invalidate();
                        }
                    }
                    //Raise the mouse click event                    
                    // Perform actions on the hit test results list. 
                    if (hoveredButton != null)
                    {
                        hoveredButton.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Left) { RoutedEvent = Button.ClickEvent });
                    }
                }
                if (geoNodeParams.opennes <= opennesThreadshold) selectable = false;
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
        }
        
        
        //Render the captured view into the Rectangle
        private void viewList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
        view_voice
    }
}