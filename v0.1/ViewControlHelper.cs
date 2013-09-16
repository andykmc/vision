using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace v0_1
{
    public sealed class ViewControlHelper
    {
        static ViewControlHelper instance = null;
        static readonly object padlock = new object();

        Window parentWindow;
        ListBox viewList;
        List<views> viewsHistory;
        views currentView;
        views viewChoosed;


        public List<views> previousViews;
        //Label testLabel;
        //public bool needDebouncing;
        //IDisposable timer;
        

        private ViewControlHelper()
        {
            parentWindow = App.Current.MainWindow;
            viewList = (ListBox)parentWindow.FindName("viewList");
            viewsHistory = new List<views>();
            currentView = views.view_home;
            viewsHistory.Add(currentView);
            viewChoosed = views.view_none;
        }

        public static ViewControlHelper Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ViewControlHelper();
                    }
                    return instance;
                }
            }
        }

        /*public ViewControlHelper(Window mainWindow, views rootView)
        {
            //parentWindow = mainWindow;
            parentWindow = App.Current.MainWindow;
            viewList = (ListBox)parentWindow.FindName("viewList");
            //testLabel = (Label)parentWindow.FindName("testLabel");
            previousViews = new List<views>();
            currentView = rootView;
            viewsHistory = new List<views>();
            viewsHistory.Add(rootView);
            //needDebouncing = false;
            viewChoosed = views.view_none;
        }*/

        public void gotoView(views view)
        {   
            //if (getCurrentView() != view && needDebouncing == false)
            if (currentView != view)
            {
                viewChoosed = view;
                currentView = viewChoosed;
                viewsHistory.Add(view);
                viewList.SelectedIndex = (int)view;

                //needDebouncing = true;
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
            if (viewsHistory.Count > 1)
            {   
                viewsHistory.RemoveAt(viewsHistory.Count - 1);
                viewChoosed = viewsHistory.Last();
                currentView = viewChoosed;
                viewList.SelectedIndex = (int)currentView;
            }
            //testLabel.Content = previousViews.Count().ToString();
        }

        public views getCurrentView()
        {
            return currentView;
        }

        public views getViewChoosed()
        {
            return viewChoosed;
        }

        public List<views> getViewsHistory()
        {
            return viewsHistory;
        }

        public void gotoHomeView()
        {
            viewsHistory.Clear();
            gotoView(views.view_home);
        }
    }
}
