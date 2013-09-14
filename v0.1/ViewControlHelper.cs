﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace v0_1
{
    public class ViewControlHelper
    {
        Window parentWindow;
        ListBox viewList;
        //Label testLabel;
        public List<views> previousViews;
        public views currentView;
        //public bool needDebouncing;
        //IDisposable timer;
        public views viewChoosed;
        
        public ViewControlHelper(Window mainWindow)
        {
            parentWindow = mainWindow;
            viewList = (ListBox)parentWindow.FindName("viewList");
            //testLabel = (Label)parentWindow.FindName("testLabel");
            previousViews = new List<views>();
            currentView = views.view_home;
            //needDebouncing = false;
            viewChoosed = views.view_none;
        }        

        public void gotoView(views view)
        {   
            //if (getCurrentView() != view && needDebouncing == false)
            if (getCurrentView() != view)
            {
                viewChoosed = view;
                //needDebouncing = true;
                addPreviousView(currentView);
                setCurrentView(view);
                viewList.SelectedIndex = (int)view;
                
                /*testLabel.Content = "TRUE";
                timer = EasyTimer.SetTimeout(() =>
                {
                    needDebouncing = false;
                    //testLabel.Content = "FALSE";
                }, 5000);*/
            }
            //testLabel.Content = previousViews.Count().ToString();
        }

        public void gotoPreviousView()
        {            
            if (previousViews.Count > 0)
            {
                viewChoosed = getPreviousView();
                setCurrentView(getPreviousView());
                removePreviousView();
                viewList.SelectedIndex = (int)currentView;                
            }
            //testLabel.Content = previousViews.Count().ToString();
        }

        public void gotoHomeView()
        {
            resetPreviousViews();
            setCurrentView(views.view_home);
            viewList.SelectedIndex = (int)views.view_home;
        }

        public void setCurrentView(views view)
        {
            currentView = view;
        }

        public void addPreviousView(views view)
        {
            previousViews.Add(view);
        }

        public views getCurrentView()
        {
            return currentView;
        }

        public views getPreviousView()
        {
            return previousViews.Last();
        }

        public void removePreviousView()
        {
            previousViews.RemoveAt(previousViews.Count - 1);
        }

        public void resetPreviousViews()
        {
            previousViews = new List<views>();
        }
    }
}
