using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace v0_1
{
	public partial class view_voice : Page
	{
        ViewControlHelper viewControlHelper;
		public view_voice()
		{
			InitializeComponent();
		}

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //set viewControlHelper after page is loaded into main window
            Window parentWindow = Window.GetWindow(this);
            viewControlHelper = new ViewControlHelper(parentWindow, views.view_voice); 
        }
	}
}