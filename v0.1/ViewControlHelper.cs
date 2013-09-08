using System;
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
        public List<views> previousViews;
        public views currentView;
        
        public ViewControlHelper(Window mainWindow)
        {
            parentWindow = mainWindow;
            viewList = (ListBox)parentWindow.FindName("viewList");
            previousViews = new List<views>();
            currentView = views.view_home;
        }        

        public void gotoView(views view)
        {
            if (getCurrentView() != view)
            {
                addPreviousView(currentView);
                setCurrentView(view);
                viewList.SelectedIndex = (int)view;
            }
        }

        public void gotoPreviousView()
        {            
            if (previousViews.Count > 0)
            {
                setCurrentView(getPreviousView());
                removePreviousView();
                viewList.SelectedIndex = (int)currentView;                
            }

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
