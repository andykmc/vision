using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace v0_1
{
    class GesturePipeline : UtilMPipeline
    {
        public MyGestureParams geoNodeParams;
        protected int nframes;
        protected bool device_lost;

        public GesturePipeline()
            : base()
        {
            EnableGesture();
            EnableImage(PXCMImage.ColorFormat.COLOR_FORMAT_RGB32, 640, 480);
            // Select a depth stream with resolution
            EnableImage(PXCMImage.ColorFormat.COLOR_FORMAT_DEPTH, 320, 240);
            nframes = 0;
            device_lost = false;
            geoNodeParams = new MyGestureParams();
        }
        public override void OnGesture(ref PXCMGesture.Gesture data)
        {
            if (data.active) geoNodeParams.gestureLabel = data.label;
        }
        public override void OnAlert(ref PXCMGesture.Alert alert)
        {
            geoNodeParams.alertLabel = alert.label;
        }

        public override bool OnDisconnect()
        {
            device_lost = true;
            return base.OnDisconnect();
        }
        public override void OnReconnect()
        {
            device_lost = false;
        }

        public override bool AcquireFrame(bool wait)
        {
            PXCMGesture gesture = QueryGesture();
            PXCMGesture.GeoNode ndata;
            pxcmStatus sts = gesture.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY, out ndata);

            if (sts >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                if ((ndata.positionImage.x >= 0) && (ndata.positionImage.x <= 320))
                    geoNodeParams.nodeX = ndata.positionImage.x;
                if ((ndata.positionImage.y >= 0) && (ndata.positionImage.y <= 240))
                    geoNodeParams.nodeY = ndata.positionImage.y;
                geoNodeParams.opennes = ndata.openness;
            }
            return base.AcquireFrame(wait);
        }
        /*
        public override bool OnNewFrame()
        {
            System.Windows.Forms.MessageBox.Show("get new frame");
            PXCMGesture gesture = QueryGesture();
            PXCMGesture.GeoNode ndata;
            pxcmStatus sts = gesture.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY, out ndata);

            if (sts >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                //nodeX = ndata.positionImage.x;
                geoNodeParams.nodeX = ndata.positionImage.x;
                geoNodeParams.nodeY = ndata.positionImage.y;
            }

            //Console.WriteLine("node HAND_MIDDLE ({0},{1})", ndata.positionImage.x, ndata.positionImage.y);

            return (++nframes < 50000);
        }
         */
    }
}
