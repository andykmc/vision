//ce
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace v0_1
{
    class MyParams
    {
        public float nodeX;
        public float nodeY;
        public uint opennes;
        public PXCMGesture.Alert.Label alertLabel;
        public PXCMGesture.Gesture.Label gestureLabel;

        public MyParams()
        {
            nodeX = -1;
            nodeY = -1;
            opennes = 0;
            alertLabel = PXCMGesture.Alert.Label.LABEL_ANY;
            gestureLabel = PXCMGesture.Gesture.Label.LABEL_ANY;

        }

        public void reset()
        {
            nodeX = -1;
            nodeY = -1;
            opennes = 0;
            alertLabel = PXCMGesture.Alert.Label.LABEL_ANY;
            gestureLabel = PXCMGesture.Gesture.Label.LABEL_ANY;
        }
    }
}
