using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Xml;
using System.Windows.Media.Animation;

namespace v0_1
{
    public class AnimatedViewer
    {
        MainWindow mainWindow;
        int oldSelectedIndex;
        public AnimatedViewer(MainWindow window)
        {
            mainWindow = window;
            oldSelectedIndex = 0;
        }

        //Render the captured view into the Rectangle
        public void RenderCapturedView()
        {
            var viewer = mainWindow.viewer;
            var rectanglevisual = mainWindow.rectanglevisual;
            var viewList = mainWindow.viewList;
            XmlElement root = (XmlElement)viewer.DataContext;
            XmlNodeList xnl = root.SelectNodes("Page");

            if (viewer.ActualHeight > 0 && viewer.ActualWidth > 0)
            {
                RenderTargetBitmap rtb = RenderBitmap(viewer);
                rectanglevisual.Fill = new ImageBrush(BitmapFrame.Create(rtb));
            }

            viewer.ItemsSource = xnl;

            if (oldSelectedIndex < viewList.SelectedIndex)
            {
                viewer.BeginStoryboard((Storyboard)mainWindow.Resources["slideDownToUp"]);
            }
            else
            {
                viewer.BeginStoryboard((Storyboard)mainWindow.Resources["slideUpToDown"]);
            }

            oldSelectedIndex = viewList.SelectedIndex;
        }
        
        //Capturing image of current view
        private RenderTargetBitmap RenderBitmap(FrameworkElement element)
        {
            double topLeft = 0;
            double topRight = 0;
            int width = (int)element.ActualWidth;
            int height = (int)element.ActualHeight;
            double dpiX = 96; // this is the magic number
            double dpiY = 96; // this is the magic number

            PixelFormat pixelFormat = PixelFormats.Default;
            VisualBrush elementBrush = new VisualBrush(element);
            DrawingVisual visual = new DrawingVisual();
            DrawingContext dc = visual.RenderOpen();

            dc.DrawRectangle(elementBrush, null, new Rect(topLeft, topRight, width, height));
            dc.Close();

            RenderTargetBitmap bitmap = new RenderTargetBitmap(width, height, dpiX, dpiY, pixelFormat);

            bitmap.Render(visual);
            return bitmap;
        }
    }
}
