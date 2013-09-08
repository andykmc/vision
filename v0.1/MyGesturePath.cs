using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;


namespace v0_1
{
    class MyGesturePath
    {
        public Point OldPoint;
        public Point NewPoint;

        public MyGesturePath(Point oldPoint, Point newPoint)
        {
            this.OldPoint = oldPoint;
            this.NewPoint = newPoint;
        }
    }
}
