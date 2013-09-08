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

namespace v0_1
{
	public partial class view_draw : Page
	{
        ViewControlHelper viewControlHelper;
		public view_draw()
		{
			InitializeComponent();
		}

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {            
            //set viewControlHelper after page is loaded into main window
            Window parentWindow = Window.GetWindow(this);
            viewControlHelper = new ViewControlHelper(parentWindow);            
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

            
            //int i = 0;
            //cursor position is added by one due to testing 1px border on viewing canvas
            /*
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
	}
}