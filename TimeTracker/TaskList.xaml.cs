using System;
using System.Collections.Generic;
using DataSource;
using TimeTracker.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace TimeTracker
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class TaskList : TimeTracker.Common.LayoutAwarePage
    {
        private bool isFromAppBar = false;

        public TaskList()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            this.DataContext = AppDataSource.GetCurrentObject();
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void ListBox_SelectionChanged_1(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            // TODO: Add event handler implementation here.
        }

        private async void PauseTask()
        {
            MessageBoxResult result = await MessageBox.ShowAsync("Would you like to start another task?", "Task Stopped", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    ShowNewTaskBox(false);
                    break;
                case MessageBoxResult.No:
                    TimeManager.StartNonWorkingTask();
                    break;
                default:
                    TimeManager.StartNonWorkingTask();
                    break;
            }
        }

        private void HidePopUp()
        {
            isFromAppBar = false;
            normalControl.IsEnabled = true;
            this.Opacity = 1;
            popupControl.IsEnabled = false;
            logincontrol1.IsOpen = false;
        }

        private void ShowNewTaskBox(bool isNewTask)
        {
            normalControl.IsEnabled = false;
            this.Opacity = .4;
            popupControl.IsEnabled = true;
            logincontrol1.IsOpen = true;
            pop.Width = Window.Current.Bounds.Width;

            if (isNewTask)
            {
                newTaskUserControl.Visibility = Windows.UI.Xaml.Visibility.Visible;
                comboBoxAndButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else
            {
                newTaskUserControl.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                comboBoxAndButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TaskObject selectedObject = (sender as Button).DataContext as TaskObject;

            if (selectedObject.IsRunning)
            {
                PauseTask();
            }
            else
            {
                NewTimeEntryObject(selectedObject.UniqueId);
            }
        }

        private void NewTimeEntryObject(string id)
        {
            //string comment

            TimeManager.StartTimerById(id);
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            if (!isFromAppBar)
            {
                TimeManager.StartNonWorkingTask();
            }

            HidePopUp();
        }

        private void newTaskButton_Click(object sender, RoutedEventArgs e)
        {
            newTaskUserControl.Visibility = Windows.UI.Xaml.Visibility.Visible;
            comboBoxAndButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void startTaskBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string newTaskId;

                if (newTaskUserControl.Visibility == Windows.UI.Xaml.Visibility.Visible)
                {
                    newTaskId = newTaskUserControl.NewWorkingTask(sender, e);
                }
                else
                {
                    TaskObject selectedTask = TaskComboBox.SelectedItem as TaskObject;

                    newTaskId = selectedTask.UniqueId;
                }

                NewTimeEntryObject(newTaskId);
                HidePopUp();
            }
            catch (Exception)
            {
                MessageBox.ShowAsync("Error adding task, please check your fields", "Error", MessageBoxButton.OK);
            }
        }

        private void addTaskClick(object sender, RoutedEventArgs e)
        {
            ShowNewTaskBox(true);

            isFromAppBar = true;
            BottomAppBar.IsOpen = false;
        }

        private void startTaskClick(object sender, RoutedEventArgs e)
        {
            ShowNewTaskBox(false);

            isFromAppBar = true;
            BottomAppBar.IsOpen = false;
        }

        private void saveComment(object sender, RoutedEventArgs e)
        {

        }

    }
}
