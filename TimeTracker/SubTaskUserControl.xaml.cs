using DataSource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimeTracker
{
    public sealed partial class SubTaskUserControl : UserControl
    {
        private const char collapseString = (char)0xE00F;
        private const char expandString = (char)0xE00E;

        public SubTaskUserControl()
        {
            this.InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Button clickedButton = (sender as Button);
            //TaskObject selectedTask = clickedButton.Tag as TaskObject;

            //if (selectedTask.CollapseChild)
            //{
            //    selectedTask.CollapseChild = false;

            //    clickedButton.Content = expandString;
            //}
            //else
            //{
            //    selectedTask.CollapseChild = true;

            //    clickedButton.Content = collapseString;
            //}
        }
    }
}
