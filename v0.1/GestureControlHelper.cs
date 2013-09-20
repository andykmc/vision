using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace v0_1
{
    static class GestureControlHelper
    {
        static Window parentWindow;
        static Label XLabel;
        static Label YLabel;
        static Image hand_palm;
        static Image hand_grip;
        static Label testLabel;
        static Label currentPageLabel;
        static Label debouncingLabel;
        static TextBox eventBox;
                
        static ViewControlHelper viewControlHelper;
        public static MyGestureParams geoNodeParams;
        static MyGesturePath myPath;
        static Point newPoint;
        static Point oldPoint;
        static bool needDebouncing;
        static bool inkCanvasMouseDown;
        static IDisposable debouncingTimer;
        public static Button hoveredButton;
        public static InkCanvas hoveredInkCanvas;
        const int opennesThreadshold = 20;

        public static void Initialize()
        {
            viewControlHelper = ViewControlHelper.Instance;
            geoNodeParams = new MyGestureParams();
            myPath = new MyGesturePath(oldPoint, newPoint);
            needDebouncing = false;
            inkCanvasMouseDown = false;
            debouncingTimer = null;
            hoveredButton = null;
            hoveredInkCanvas = null;

            parentWindow = App.Current.MainWindow;
            XLabel = (Label) parentWindow.FindName("XLabel");
            YLabel = (Label) parentWindow.FindName("YLabel");
            hand_palm = (Image)parentWindow.FindName("hand_palm");
            hand_grip = (Image)parentWindow.FindName("hand_grip");
            testLabel = (Label)parentWindow.FindName("testLabel");
            currentPageLabel = (Label)parentWindow.FindName("currentPageLabel");
            debouncingLabel = (Label)parentWindow.FindName("debouncingLabel");
            eventBox = (TextBox)parentWindow.FindName("eventBox");
            newPoint = new Point();
            oldPoint = new Point(-1, -1);
        }

        public static void RunGestureControl()
        {
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

                newPoint.X = 640 - Convert.ToInt32(geoNodeParams.nodeX) * 2;
                newPoint.Y = Convert.ToInt32(geoNodeParams.nodeY) * 2;

                //check opennes
                if (geoNodeParams.opennes > opennesThreadshold)
                {
                    hand_palm.Visibility = Visibility.Visible;
                    hand_grip.Visibility = Visibility.Hidden;

                    //======Start of method 1========
                    raiseInkCanvasMouseEvent("MouseUpEvent");
                    //======End of method 1========
                }
                else if (geoNodeParams.opennes >= 0 && geoNodeParams.opennes <= opennesThreadshold)
                {
                    hand_palm.Visibility = Visibility.Hidden;
                    hand_grip.Visibility = Visibility.Visible;

                    //draw line
                    //SolidBrush color = new SolidBrush(Color.Black);

                    if ((oldPoint.X == -1) || (oldPoint.Y == -1))
                    {
                        oldPoint = newPoint;
                    }
                    else
                    {
                        myPath.OldPoint = oldPoint;
                        myPath.NewPoint = newPoint;

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
                        //======Start of method 1========
                        raiseInkCanvasMouseEvent("MouseDownEvent");
                        //======End of method 1========

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
                        //=======End of method 2=========

                        //======Method 3 under investigation=====
                        /*
                         * Target: 
                        StrokeCollection strokeCollection = new StrokeCollection();
                        Stroke stroke = new Stroke();
                        strokeCollection.
                        */
                        //======Method 3 under investigation=====
                    }
                }
                //if (geoNodeParams.opennes <= opennesThreadshold) selectable = false;
                oldPoint = newPoint;

                if (geoNodeParams.gestureLabel == PXCMGesture.Gesture.Label.LABEL_NAV_SWIPE_DOWN)
                {
                    viewControlHelper.gotoPreviousView();
                }
                else
                {
                    hand_palm.Visibility = Visibility.Hidden;
                    hand_grip.Visibility = Visibility.Hidden;
                    //======Start of method 1========
                    raiseInkCanvasMouseEvent("MouseUpEvent");
                    //======End of method 1========
                }
                var viewString = "Views:";
                foreach (var view in viewControlHelper.getViewsHistory())
                {
                    viewString += view.ToString() + ",";
                }
                testLabel.Content = viewString;
                //testLabel.Content = viewControlHelper.previousViews.Count().ToString();
                currentPageLabel.Content = viewControlHelper.getCurrentView().ToString();
                debouncingLabel.Content = needDebouncing.ToString();

                //capture the events
                var events = EventManager.GetRoutedEvents();
                foreach (var routedEvent in events)
                {
                    EventManager.RegisterClassHandler(typeof(UserControl), routedEvent, new RoutedEventHandler(handler));
                }
                eventBox.ScrollToEnd();
            }
        }

        //For controlling the InkCanvas
        static void raiseInkCanvasMouseEvent(string eventName)
        {
            switch (eventName)
            {
                case "MouseUpEvent":
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
                    break;
                case "MouseDownEvent":
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
                    break;
            }

        }

        //capture the events
        internal static void handler(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(e.RoutedEvent.ToString());
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
    }
}
