using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace v0_1
{
    class VoicePipeline : UtilMPipeline
    {        
        string[] wstr = new string[] {"select", "selecting", "selecting mode", "rank", "ranking", "ranking mode", "swap forward", 
                "swap back", "reset", "restart", "cat", "kitty", "aeroplane", "plane", "dog", "puppy", "donut", "Eiffel Tower", "tower", 
                "Tom Mason", "Taylor Swift", "T-shirt", "tennis racket", "apple", "eyeglasses", "hourglass", "Moon", "sun", "revolver", 
                "gun", "hamburger", "pumpkin", "ladder", "skull", "car", "I MuSe", "search", "go to home", "back to home", "done"};
        protected int nframes;
        protected bool device_lost;
        bool voiceState;
        bool previousVoiceState;
        string alertLabel;
        string detectedPhrase;
        uint confidenceLevel;

        public VoicePipeline()
            : base()
        {
            EnableVoiceRecognition();
            nframes = 0;
            device_lost = false;
            voiceState = false;
            previousVoiceState = false;
            alertLabel = "";
            detectedPhrase = "";
            confidenceLevel = 0;
        }

        public override bool OnDisconnect()
        {
            //if (!device_lost) Console.WriteLine("Device disconnected");
            device_lost = true;
            return base.OnDisconnect();
        }

        public override void OnReconnect()
        {
            //Console.WriteLine("Device reconnected");
            device_lost = false;
        }

        public bool SetVoiceCommands()
        {
            return base.SetVoiceCommands(this.wstr);
        }

        public override void OnAlert(ref PXCMVoiceRecognition.Alert data)
        {
            voiceState = !voiceState;
            alertLabel = data.label.ToString();
            base.OnAlert(ref data);
        }

        bool commandDetected()
        {
            //if (voiceState != previousVoiceState && alertLabel == "")
            if (voiceState != previousVoiceState)
            {   
                previousVoiceState = voiceState;
                return true;
            }
            else return false;
        }

        public MyVoiceParams getDetectedPhrase()
        {
            if (commandDetected())
            {
                MyVoiceParams myVoiceParams = new MyVoiceParams();
                myVoiceParams.detectedPhrase = detectedPhrase;
                myVoiceParams.alertLabel = alertLabel;
                myVoiceParams.confidenceLevel = confidenceLevel;
                alertLabel = "";
                return myVoiceParams;
            }
            else
            {
                MyVoiceParams myVoiceParams = new MyVoiceParams();
                myVoiceParams.detectedPhrase = "";
                myVoiceParams.alertLabel = alertLabel;
                myVoiceParams.confidenceLevel = 0;
                alertLabel = "";
                return myVoiceParams;
            }
        }

        public override void OnRecognized(ref PXCMVoiceRecognition.Recognition data)
        {
            voiceState = !voiceState;
            //alertLabel = "";
            detectedPhrase = data.dictation;
            confidenceLevel = data.confidence;
            /*
            string str;
            str = data.dictation;
            
            Console.WriteLine("Recognized<{0}>", str);


            // Command words
            if (str.CompareTo("swap forward") == 0)
                Console.WriteLine("The previous image is shown!\n");
            else if (str.CompareTo("swap backward") == 0 || str.CompareTo("swap back") == 0)
                Console.WriteLine("The next image is shown!\n");
            else if (str.CompareTo("rank") == 0 || str.CompareTo("ranking") == 0 || str.CompareTo("ranking mode") == 0)
                Console.WriteLine("Ranking Mode is on!\n");
            else if (str.CompareTo("select") == 0 || str.CompareTo("selecting") == 0 || str.CompareTo("selecting mode") == 0)
                Console.WriteLine("The image is selected!\n");

            // Category
            else if (str.CompareTo("cat") == 0 || str.CompareTo("kitty") == 0)
                Console.WriteLine("Images from 1 to 25 are shown!\n");
            else if (str.CompareTo("aeroplane") == 0 || str.CompareTo("plane") == 0)
                Console.WriteLine("Images from 26 to 50 are shown!\n");
            else if (str.CompareTo("dog") == 0 || str.CompareTo("puppy") == 0)
                Console.WriteLine("Images from 51 to 75 are shown!\n");
            else if (str.CompareTo("donut") == 0)
                Console.WriteLine("Images from 76 to 100 are shown!\n");
            else if (str.CompareTo("Eiffel Tower") == 0 || str.CompareTo("tower") == 0)
                Console.WriteLine("Images from 101 to 125 are shown!\n");
            else if (str.CompareTo("Tom Mason") == 0)
                Console.WriteLine("Images from 126 to 150 are shown!\n");
            else if (str.CompareTo("Taylor Swift") == 0)
                Console.WriteLine("Images from 151 to 175 are shown!\n");
            else if (str.CompareTo("T-shirt") == 0)
                Console.WriteLine("Images from 176 to 200 are shown!\n");
            else if (str.CompareTo("tennis racket") == 0)
                Console.WriteLine("Images from 201 to 225 are shown!\n");
            else if (str.CompareTo("apple") == 0)
                Console.WriteLine("Images from 226 to 250 are shown!\n");
            else if (str.CompareTo("eyeglasses") == 0)
                Console.WriteLine("Images from 251 to 275 are shown!\n");
            else if (str.CompareTo("hourglass") == 0)
                Console.WriteLine("Images from 276 to 300 are shown!\n");
            else if (str.CompareTo("Moon") == 0)
                Console.WriteLine("Images from 301 to 325 are shown!\n\n");
            else if (str.CompareTo("sun") == 0)
                Console.WriteLine("Images from 326 to 350 are shown!\n");
            else if (str.CompareTo("revolver") == 0 || str.CompareTo("gun") == 0)
                Console.WriteLine("Images from 351 to 375 are shown!\n");
            else if (str.CompareTo("hamburger") == 0)
                Console.WriteLine("Images from 376 to 400 are shown!\n");
            else if (str.CompareTo("pumpkin") == 0)
                Console.WriteLine("Images from 401 to 425 are shown!\n");
            else if (str.CompareTo("ladder") == 0)
                Console.WriteLine("Images from 426 to 450 are shown!\n");
            else if (str.CompareTo("skull") == 0)
                Console.WriteLine("Images from 451 to 475 are shown!\n");
            else if (str.CompareTo("car") == 0)
                Console.WriteLine("Images from 476 to 500 are shown!\n");
            else if (str.CompareTo("I MuSe") == 0)
                Console.WriteLine("Voice Command!\n");
            else
                Console.WriteLine("Please speak a command word or a name of categories.\n");
            */
        }

        //public override bool OnNewFrame() {
        //    Console.Write(".");
        //    return (++nframes<50000);
        //}
    }

    /*
    class Program
    {
        static void Main2(string[] args)
        {

            string[] wstr = new string[] {"select", "selecting", "selecting mode", "rank", "ranking", "ranking mode", "swap forward", 
                "swap back", "reset", "restart", "cat", "kitty", "aeroplane", "plane", "dog", "puppy", "donut", "Eiffel Tower", "tower", 
                "Tom Mason", "Taylor Swift", "T-shirt", "tennis racket", "apple", "eyeglasses", "hourglass", "Moon", "sun", "revolver", 
                "gun", "hamburger", "pumpkin", "ladder", "skull", "car", "I MuSe"};
            int cnt;
            cnt = wstr.Length;    // To find the length of wstr[]

            // To print out the commands
            Console.WriteLine("Selecting Mode Command: {0}, {1}, {2}", wstr[0], wstr[1], wstr[2]);
            Console.WriteLine("Ranking Mode Command: {0}, {1}, {2}", wstr[3], wstr[4], wstr[5]);
            Console.WriteLine("Swapping Mode Command: {0}, {1}", wstr[6], wstr[7]);
            Console.WriteLine("Reset Mode Command: {0}, {1} \n", wstr[8], wstr[9]);

            // To print out the 20 categories, each category has 25 images
            Console.WriteLine("Category: ");
            for (int i = 10; i < cnt - 1; i++)
            {
                Console.Write("{0}, ", wstr[i]);
            }
            Console.WriteLine("{0}\n", wstr[cnt - 1]);

            // To do the voice setting
            VoicePipeline pipeline = new VoicePipeline();
            Console.WriteLine("Please speak a command word or a name of categories.\n\n");
            pipeline.SetVoiceCommands(wstr);

            if (!pipeline.LoopFrames())
                Console.WriteLine("Failed to initialize or stream data");

            pipeline.Dispose();
        }
    }
    */
}
