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
            MessageBox.Show("start match");
            MatchingHelper.matchClass();
        }

        public void backgroundWorkerMatch_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {}

        public void backgroundWorkerMatch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //MessageBox.Show("Matched result:" + MatchingHelper.matchedClass);
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
            //textBox.Text += "InkCanvas_LayoutUpdated\n"; ;
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
            //strokeNumLabel.Content = InkCanvas.Strokes.Count.ToString();
            //textBox.Text += "InkCanvas_MouseMove\n";
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
        //===================

        private void InkCanvas_EditingModeInvertedChanged(object sender, RoutedEventArgs e)
        {
            textBox.Text += "InkCanvas_EditingModeInvertedChanged\n";
        }

        private void InkCanvas_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            textBox.Text += "InkCanvas_FocusableChanged\n";
        }

        private void InkCanvas_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            textBox.Text += "InkCanvas_GiveFeedback\n";
        }

        private void InkCanvas_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            textBox.Text += "InkCanvas_GotKeyboardFocus\n";
        }

        private void InkCanvas_GotStylusCapture(object sender, StylusEventArgs e)
        {
            textBox.Text += "InkCanvas_GotStylusCapture\n";
        }

        private void InkCanvas_GotTouchCapture(object sender, TouchEventArgs e)
        {
            textBox.Text += "InkCanvas_GotTouchCapture\n";
        }

        private void InkCanvas_Initialized(object sender, EventArgs e)
        {
            //textBox.Text += "InkCanvas_Initialized\n";
            //MessageBox.Show("InkCanvas_Initialized");
        }

        private void InkCanvas_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            textBox.Text += "InkCanvas_IsEnabledChanged\n";
        }

        private void InkCanvas_IsHitTestVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            textBox.Text += "InkCanvas_IsHitTestVisibleChanged\n";
        }

        private void InkCanvas_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            textBox.Text += "InkCanvas_IsKeyboardFocusedChanged\n";
        }

        private void InkCanvas_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            textBox.Text += "InkCanvas_IsKeyboardFocusWithinChanged\n";
        }

        private void InkCanvas_IsStylusCapturedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            textBox.Text += "InkCanvas_IsStylusCapturedChanged\n";
        }

        private void InkCanvas_IsStylusCaptureWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            textBox.Text += "InkCanvas_IsStylusCaptureWithinChanged\n";
        }

        private void InkCanvas_IsStylusDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            textBox.Text += "InkCanvas_IsStylusDirectlyOverChanged\n";
        }

        private void InkCanvas_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //textBox.Text += "InkCanvas_IsVisibleChanged\n";
            //MessageBox.Show("InkCanvas_IsVisibleChanged");
        }

        private void InkCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            textBox.Text += "InkCanvas_KeyDown\n";
        }

        private void InkCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            textBox.Text += "InkCanvas_KeyUp\n";
        }

        private void InkCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            textBox.Text += "InkCanvas_Loaded\n";
        }

        private void InkCanvas_LostFocus(object sender, RoutedEventArgs e)
        {
            textBox.Text += "InkCanvas_LostFocus\n";
        }

        private void InkCanvas_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            textBox.Text += "InkCanvas_LostKeyboardFocus\n";
        }

        private void InkCanvas_LostMouseCapture(object sender, MouseEventArgs e)
        {
            textBox.Text += "InkCanvas_LostMouseCapture\n";
        }

        private void InkCanvas_LostStylusCapture(object sender, StylusEventArgs e)
        {
            textBox.Text += "InkCanvas_LostStylusCapture\n";
        }

        private void InkCanvas_LostTouchCapture(object sender, TouchEventArgs e)
        {
            textBox.Text += "InkCanvas_LostTouchCapture\n";
        }

        private void InkCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            textBox.Text += "InkCanvas_MouseWheel\n";
        }

        private void InkCanvas_PreviewDragEnter(object sender, DragEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewDragEnter\n";
        }

        private void InkCanvas_PreviewDragLeave(object sender, DragEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewDragLeave\n";
        }

        private void InkCanvas_PreviewDragOver(object sender, DragEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewDragOver\n";
        }

        private void InkCanvas_PreviewDrop(object sender, DragEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewDrop\n";
        }

        private void InkCanvas_PreviewGiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewGiveFeedback\n";
        }

        private void InkCanvas_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewGotKeyboardFocus\n";
        }

        private void InkCanvas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewKeyDown\n";
        }

        private void InkCanvas_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewKeyUp\n";
        }

        private void InkCanvas_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewLostKeyboardFocus\n";
        }

        private void InkCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewMouseDown\n";
        }

        private void InkCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewMouseLeftButtonDown\n";
        }

        private void InkCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewMouseLeftButtonUp\n";
        }

        private void InkCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewMouseMove\n";
        }

        private void InkCanvas_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewMouseRightButtonDown\n";
        }

        private void InkCanvas_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewMouseRightButtonUp\n";
        }

        private void InkCanvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewMouseUp\n";
        }

        private void InkCanvas_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewMouseWheel\n";
        }

        private void InkCanvas_PreviewQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewQueryContinueDrag\n";
        }

        private void InkCanvas_PreviewStylusButtonDown(object sender, StylusButtonEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewStylusButtonDown\n";
        }

        private void InkCanvas_PreviewStylusButtonUp(object sender, StylusButtonEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewStylusButtonUp\n";
        }

        private void InkCanvas_PreviewStylusDown(object sender, StylusDownEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewStylusDown\n";
        }

        private void InkCanvas_PreviewStylusInAirMove(object sender, StylusEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewStylusInAirMove\n";
        }

        private void InkCanvas_PreviewStylusInRange(object sender, StylusEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewStylusInRange\n";
        }

        private void InkCanvas_PreviewStylusMove(object sender, StylusEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewStylusMove\n";
        }

        private void InkCanvas_PreviewStylusOutOfRange(object sender, StylusEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewStylusOutOfRange\n";
        }

        private void InkCanvas_PreviewStylusSystemGesture(object sender, StylusSystemGestureEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewStylusSystemGesture\n";
        }

        private void InkCanvas_PreviewStylusUp(object sender, StylusEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewStylusUp\n";
        }

        private void InkCanvas_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewTextInput\n";
        }

        private void InkCanvas_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewTouchDown\n";
        }

        private void InkCanvas_PreviewTouchMove(object sender, TouchEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewTouchMove\n";
        }

        private void InkCanvas_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            textBox.Text += "InkCanvas_PreviewTouchUp\n";
        }

        private void InkCanvas_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            textBox.Text += "InkCanvas_RequestBringIntoView\n";
        }

        private void InkCanvas_SelectionResized(object sender, EventArgs e)
        {
            textBox.Text += "InkCanvas_SelectionResized\n";
        }

        private void InkCanvas_SelectionResizing(object sender, InkCanvasSelectionEditingEventArgs e)
        {
            textBox.Text += "InkCanvas_SelectionResizing\n";
        }

        private void InkCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //textBox.Text += "InkCanvas_SizeChanged\n";
        }

        private void InkCanvas_StrokeErased(object sender, RoutedEventArgs e)
        {
            textBox.Text += "InkCanvas_StrokeErased\n";
        }

        private void InkCanvas_StrokeErasing(object sender, InkCanvasStrokeErasingEventArgs e)
        {
            textBox.Text += "InkCanvas_StrokeErasing\n";
        }

        private void InkCanvas_StylusButtonDown(object sender, StylusButtonEventArgs e)
        {
            textBox.Text += "InkCanvas_StylusButtonDown\n";
        }

        private void InkCanvas_StylusButtonUp(object sender, StylusButtonEventArgs e)
        {
            textBox.Text += "InkCanvas_StylusButtonUp\n";
        }

        private void InkCanvas_StylusDown(object sender, StylusDownEventArgs e)
        {
            textBox.Text += "InkCanvas_StylusDown\n";
        }

        private void InkCanvas_StylusEnter(object sender, StylusEventArgs e)
        {
            textBox.Text += "InkCanvas_StylusEnter\n";
        }

        private void InkCanvas_StylusInAirMove(object sender, StylusEventArgs e)
        {
            textBox.Text += "InkCanvas_StylusInAirMove\n";
        }

        private void InkCanvas_StylusInRange(object sender, StylusEventArgs e)
        {
            textBox.Text += "InkCanvas_StylusInRange\n";
        }

        private void InkCanvas_StylusLeave(object sender, StylusEventArgs e)
        {
            textBox.Text += "InkCanvas_StylusLeave\n";
        }

        private void InkCanvas_StylusMove(object sender, StylusEventArgs e)
        {
            textBox.Text += "InkCanvas_StylusMove\n";
        }

        private void InkCanvas_StylusOutOfRange(object sender, StylusEventArgs e)
        {
            textBox.Text += "InkCanvas_StylusOutOfRange\n";
        }

        private void InkCanvas_StylusSystemGesture(object sender, StylusSystemGestureEventArgs e)
        {
            textBox.Text += "InkCanvas_StylusSystemGesture\n";
        }

        private void InkCanvas_StylusUp(object sender, StylusEventArgs e)
        {
            textBox.Text += "InkCanvas_StylusUp\n";
        }

        private void InkCanvas_TextInput(object sender, TextCompositionEventArgs e)
        {
            textBox.Text += "InkCanvas_TextInput\n";
        }

        private void InkCanvas_ToolTipClosing(object sender, ToolTipEventArgs e)
        {
            textBox.Text += "InkCanvas_ToolTipClosing\n";
        }

        private void InkCanvas_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            textBox.Text += "InkCanvas_ToolTipOpening\n";
        }

        private void InkCanvas_TouchDown(object sender, TouchEventArgs e)
        {
            textBox.Text += "InkCanvas_TouchDown\n";
        }

        private void InkCanvas_TouchEnter(object sender, TouchEventArgs e)
        {
            textBox.Text += "InkCanvas_TouchEnter\n";
        }

        private void InkCanvas_TouchLeave(object sender, TouchEventArgs e)
        {
            textBox.Text += "InkCanvas_TouchLeave\n";
        }

        private void InkCanvas_TouchMove(object sender, TouchEventArgs e)
        {
            textBox.Text += "InkCanvas_TouchMove\n";
        }

        private void InkCanvas_TouchUp(object sender, TouchEventArgs e)
        {
            textBox.Text += "InkCanvas_TouchUp\n";
        }

        private void InkCanvas_Unloaded(object sender, RoutedEventArgs e)
        {
            textBox.Text += "InkCanvas_Unloaded\n";
        }        
	}    
}