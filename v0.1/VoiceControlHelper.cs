﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Controls;//for MessageBox

namespace v0_1
{
    static class VoiceControlHelper
    {
        public static string[] voiceCommands = new string[] {"donut", "T-shirt", "tennis racket", "apple", "eyeglasses", "hourglass", "Moon", "revolver", 
                "sun", "hamburger", "ladder", "pumpkin", "rainbow", "skull", "snowman", "blimp", "candle", "comb", "computer monitor", "fire hydrant", 
                "select", "selecting", "selecting mode", "rank", "ranking", "ranking mode", "swap forward", "swap back", "reset", "restart", 
                "I MuSe", "search", "go to home", "back to home", "done"};
        

        static ViewControlHelper viewControlHelper;
        public static MyVoiceParams detectedVoiceParams;
        static string commandSentence;
        static Window parentWindow;
        static bool keywordDetected;
        public static bool voiceControlEnabled;
        static bool toSearch;
        const int defaultMinConfidenceLevel = 40;
        static Label parentWindowVoiceLabel;


        public static void Initialize()
        {
            viewControlHelper = ViewControlHelper.Instance;
            detectedVoiceParams = new MyVoiceParams();
            keywordDetected = false;
            voiceControlEnabled = false;
            toSearch = false;
            commandSentence = "";
            parentWindow = App.Current.MainWindow;
        }

        public static void RunVoiceControl()
        {
            if (detectedVoiceParams.alertLabel == "" && detectedVoiceParams.confidenceLevel > defaultMinConfidenceLevel)
            {
                CheckVoiceCommand();
                if (keywordDetected)
                {
                    parentWindowVoiceLabel = (Label)parentWindow.FindName("VoiceCommandLabel");
                    parentWindowVoiceLabel.Content = detectedVoiceParams.detectedPhrase;
                    //MessageBox.Show("Phrase:" + detectedVoiceParams.detectedPhrase + " Confidence:" + detectedVoiceParams.confidenceLevel + " Alert:" + detectedVoiceParams.alertLabel);
                }
            }
        }

        //return true if a word is detected
        static bool CheckVoiceCommand()
        {   
            switch (detectedVoiceParams.detectedPhrase)
            {
                case "":
                    EndOfaVoiceControlCommand();
                    return false;
                case "I MuSe":
                    voiceControlEnabled = true;
                    if (viewControlHelper.getCurrentView() == views.view_home)
                        viewControlHelper.gotoVoiceViewDirectly();
                    //toSearch = false;
                    break;
                case "go to home":
                case "back to home":
                    if (voiceControlEnabled)
                        viewControlHelper.gotoHomeView();
                    EndOfaVoiceControlCommand();
                    break;
                case "search":
                    toSearch = true;
                    break;
                //all other words defined om VoicePipeline
                default:
                    if (voiceControlEnabled && toSearch)
                    {
                        MatchingHelper.matchedClass = detectedVoiceParams.detectedPhrase;
                        if (viewControlHelper.getCurrentView() == views.view_result)
                            viewControlHelper.gotoHomeView();
                        viewControlHelper.gotoView(views.view_result);
                        //commandSentence += detectedVoiceParams.detectedPhrase;
                    }
                    if (toSearch)
                        EndOfaVoiceControlCommand();
                    break;
            }
            keywordDetected = true;
            return true;
        }

        static void EndOfaVoiceControlCommand()
        {
            voiceControlEnabled = false;
            keywordDetected = false;
            toSearch = false;
            commandSentence = "";
            //MessageBox.Show("Voice Control paused");
        }
    }
}
