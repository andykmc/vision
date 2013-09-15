using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace v0_1
{
    class MyVoiceParams
    {
        public uint confidenceLevel;
        public string detectedPhrase;
        public string alertLabel;

        public MyVoiceParams()
        {
            confidenceLevel = 0;
            detectedPhrase = "";
            alertLabel = "";
        }

        public void reset()
        {
            confidenceLevel = 0;
            detectedPhrase = "";
            alertLabel = "";
        }
    }
}
