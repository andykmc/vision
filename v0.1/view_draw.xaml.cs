using System;
//using System.Net; //+
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;//+
using System.Windows.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using matching_test01;

namespace v0_1
{
	public partial class view_draw : Page
	{
        Window parentWindow;
        TextBox textBox;
        ViewControlHelper viewControlHelper;
        AbortableBackgroundWorker backgroundWorkerMatch;

		public view_draw()
		{
			InitializeComponent();
            parentWindow = null;            
		}

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {            
            //set viewControlHelper after page is loaded into main window            
            parentWindow = Window.GetWindow(this);
            textBox = (TextBox)parentWindow.FindName("eventBox");
            viewControlHelper = ViewControlHelper.Instance;
            backgroundWorkerMatch = new AbortableBackgroundWorker();
            backgroundWorkerMatch.WorkerReportsProgress = true;
            backgroundWorkerMatch.WorkerSupportsCancellation = true;
            backgroundWorkerMatch.DoWork += new DoWorkEventHandler(this.backgroundWorkerMatch_DoWork);
            backgroundWorkerMatch.ProgressChanged += new ProgressChangedEventHandler(this.backgroundWorkerMatch_ProgressChanged);
            backgroundWorkerMatch.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorkerMatch_RunWorkerCompleted);
        }

        private void goResultViewButton_Click(object sender, RoutedEventArgs e)
        {   
            if (backgroundWorkerMatch.IsBusy == false)
            {
                InkCanvasUtil.SaveCanvas(parentWindow, WrapperCanvas, "sketch.jpg");
                backgroundWorkerMatch.RunWorkerAsync(); 
            }
        }

        public void backgroundWorkerMatch_DoWork(object sender, DoWorkEventArgs e)
        {
            /*
            MessageBox.Show("match initialize");
            MatchingClass matchtest = null;
            MWCharArray output = null;
            MWArray MWresult = null;
            
            String r;
            
            matchtest = new MatchingClass();
            MessageBox.Show("start match");
            MWresult = matchtest.build_matching_test_correct_csharp2();
            output = (MWCharArray)MWresult[1, 1];
            r = output.ToString();
            //MessageBox.Show("end match");
            */
            MessageBox.Show("start match");
            MatchingHelper.matchClass();
            //backgroundWorkerMatch.ReportProgress(100);
        }

        public void backgroundWorkerMatch_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {}

        public void backgroundWorkerMatch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Matched result:" + MatchingHelper.matchedClass);
            backgroundWorkerMatch.Abort();
            backgroundWorkerMatch.Dispose();
            viewControlHelper.gotoView(views.view_result);
        }

        private void testButton_Click(object sender, RoutedEventArgs e)
        {   
            //Point position = e.GetPosition(this);
            //double x = position.X;
            //double y = position.Y;
            
            Line linetodraw = new Line();
            linetodraw.X1 = 0;
            linetodraw.Y1 = 0;
            linetodraw.X2 = 10;
            linetodraw.Y2 = 10;
            linetodraw.StrokeThickness = 2;
            linetodraw.Stroke = new SolidColorBrush(Colors.Black);
            this.InkCanvas.Children.Add(linetodraw);
            InkCanvas.Strokes.Clear();
            
           
            /*
           //int i = 0;
           //cursor position is added by one due to testing 1px border on viewing canvas
           
           NativeMethods.SetCursorPos(
               (int)Mouse.GetPosition(this).X + (int)Application.Current.MainWindow.Left + (int)SystemParameters.FixedFrameVerticalBorderWidth + 1 + 100,
               (int)Mouse.GetPosition(this).Y + (int)Application.Current.MainWindow.Top + (int)SystemParameters.FixedFrameHorizontalBorderHeight + (int)SystemParameters.WindowCaptionHeight + 1
           );
           for (int i = 1; i <= 200; i++)
           {
               NativeMethods.SetCursorPos(
                   (int)Mouse.GetPosition(this).X + (int)Application.Current.MainWindow.Left + (int)SystemParameters.FixedFrameVerticalBorderWidth + 1 + 100 + i,
                   (int)Mouse.GetPosition(this).Y + (int)Application.Current.MainWindow.Top + (int)SystemParameters.FixedFrameHorizontalBorderHeight + (int)SystemParameters.WindowCaptionHeight + 1 + i
               );
               this.InkCanvas.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Left) { RoutedEvent = InkCanvas.MouseDownEvent });
           }
           public partial class NativeMethods
           {
               /// Return Type: BOOL->int  
               ///X: int  
               ///Y: int  
               [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "SetCursorPos")]
               [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
               public static extern bool SetCursorPos(int X, int Y);
           }
           */

        }

        private void InkCanvas_ActiveEditingModeChanged(object sender, RoutedEventArgs e)
        {   
            textBox.Text += "InkCanvas_ActiveEditingModeChanged\n";
            //MessageBox.Show("InkCanvas_ActiveEditingModeChanged");
        }

        private void InkCanvas_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            textBox.Text += "InkCanvas_ContextMenuClosing\n";
        }

        private void InkCanvas_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            textBox.Text += "InkCanvas_ContextMenuOpening\n";
        }

        private void InkCanvas_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            textBox.Text += "InkCanvas_DataContextChanged\n";
        }

        private void InkCanvas_DefaultDrawingAttributesReplaced(object sender, System.Windows.Ink.DrawingAttributesReplacedEventArgs e)
        {
            textBox.Text += "InkCanvas_DefaultDrawingAttributesReplaced\n";

        }

        private void InkCanvas_DragEnter(object sender, DragEventArgs e)
        {
            textBox.Text += "InkCanvas_DragEnter\n";
        }

        private void InkCanvas_DragLeave(object sender, DragEventArgs e)
        {
            textBox.Text += "InkCanvas_DragLeave\n";
        }

        private void InkCanvas_DragOver(object sender, DragEventArgs e)
        {
            textBox.Text += "InkCanvas_DragOver\n";
        }

        private void InkCanvas_Drop(object sender, DragEventArgs e)
        {
            textBox.Text += "InkCanvas_Drop\n";
        }

        private void InkCanvas_EditingModeChanged(object sender, RoutedEventArgs e)
        {
            textBox.Text += "InkCanvas_EditingModeChanged\n";
        }

        private void InkCanvas_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox.Text += "InkCanvas_GotFocus\n";
        }

        private void InkCanvas_GotMouseCapture(object sender, MouseEventArgs e)
        {
            textBox.Text += "InkCanvas_GotMouseCapture\n";
        }

        private void InkCanvas_IsMouseCapturedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            textBox.Text += "InkCanvas_IsMouseCapturedChanged\n";
        }

        private void InkCanvas_IsMouseCaptureWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            textBox.Text += "InkCanvas_IsMouseCaptureWithinChanged\n";
        }

        private void InkCanvas_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            textBox.Text += "InkCanvas_IsMouseDirectlyOverChanged\n";
        }

        private void InkCanvas_LayoutUpdated(object sender, EventArgs e)
        {
            //Label label = (Label)parentWindow.FindName("eventLabel");
            //label.Content = "InkCanvas_LayoutUpdated";
            //MessageBox.Show("InkCanvas_LayoutUpdated");
        }

        private void InkCanvas_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            textBox.Text += "InkCanvas_ManipulationCompleted\n";
        }

        private void InkCanvas_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            textBox.Text += "InkCanvas_ManipulationDelta\n";
        }

        private void InkCanvas_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            textBox.Text += "InkCanvas_ManipulationInertiaStarting\n";
        }

        private void InkCanvas_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            textBox.Text += "InkCanvas_ManipulationStarted\n";
        }

        private void InkCanvas_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            textBox.Text += "InkCanvas_ManipulationStarting\n";
        }

        private void InkCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            textBox.Text += "InkCanvas_MouseDown\n";
        }

        private void InkCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            textBox.Text += "InkCanvas_MouseMove\n";
        }

        private void InkCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            textBox.Text += "InkCanvas_MouseUp\n";
        }

        private void InkCanvas_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            textBox.Text += "InkCanvas_QueryContinueDrag\n";
        }

        private void InkCanvas_QueryCursor(object sender, QueryCursorEventArgs e)
        {
            textBox.Text += "InkCanvas_QueryCursor\n";
        }

        private void InkCanvas_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            textBox.Text += "InkCanvas_StrokeCollected\n";
        }

        private void InkCanvas_StrokesReplaced(object sender, InkCanvasStrokesReplacedEventArgs e)
        {
            textBox.Text += "InkCanvas_StrokesReplaced\n";
        }

        private void InkCanvas_Gesture(object sender, InkCanvasGestureEventArgs e)
        {
            textBox.Text += "InkCanvas_Gesture\n";
        }

        private void InkCanvas_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            textBox.Text += "InkCanvas_ManipulationBoundaryFeedback\n";
        }

        private void InkCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            textBox.Text += "InkCanvas_MouseEnter\n";
        }

        private void InkCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            textBox.Text += "InkCanvas_MouseLeave\n";
        }

        private void InkCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            textBox.Text += "InkCanvas_MouseLeftButtonDown\n";
        }

        private void InkCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            textBox.Text += "InkCanvas_MouseLeftButtonUp\n";
        }

        private void InkCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            textBox.Text += "InkCanvas_MouseRightButtonDown\n";
        }

        private void InkCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            textBox.Text += "InkCanvas_MouseRightButtonUp\n";
        }

        private void InkCanvas_SelectionChanged(object sender, EventArgs e)
        {
            textBox.Text += "InkCanvas_SelectionChanged\n";
        }

        private void InkCanvas_SelectionChanging(object sender, InkCanvasSelectionChangingEventArgs e)
        {
            textBox.Text += "InkCanvas_SelectionChanging\n";
        }

        private void InkCanvas_SelectionMoved(object sender, EventArgs e)
        {
            textBox.Text += "InkCanvas_SelectionMoved\n";
        }

        private void InkCanvas_SelectionMoving(object sender, InkCanvasSelectionEditingEventArgs e)
        {
            textBox.Text += "InkCanvas_SelectionMoving\n";
        }

        private void InkCanvas_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            textBox.Text += "InkCanvas_SourceUpdated\n";
        }

        private void InkCanvas_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            textBox.Text += "InkCanvas_TargetUpdated\n";
        }        
	}    
}