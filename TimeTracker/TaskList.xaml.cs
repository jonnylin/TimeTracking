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

        private enum PopUpType
        {
            NewTask, StartTask, AddComment
        }

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
            this.DataContext = AppDataSource.CurrentObject;
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
                    ShowPopUp(PopUpType.StartTask);
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

        private void ShowPopUp(PopUpType popUpType)
        {
            normalControl.IsEnabled = false;
            this.Opacity = .3;
            popupControl.IsEnabled = true;
            logincontrol1.IsOpen = true;
            pop.Width = Window.Current.Bounds.Width;

            UpdatePopupBoxContent(popUpType);
        }

        private void UpdatePopupBoxContent(PopUpType popUpType)
        {
            switch (popUpType)
            {
                case PopUpType.NewTask:

                    TaskGrid.Visibility = Visibility.Visible;
                    CommentGrid.Visibility = Visibility.Collapsed;
                    newTaskUserControl.Visibility = Visibility.Visible;
                    comboBoxAndButton.Visibility = Visibility.Collapsed;

                    break;
                case PopUpType.StartTask:

                    TaskGrid.Visibility = Visibility.Visible;
                    CommentGrid.Visibility = Visibility.Collapsed;
                    newTaskUserControl.Visibility = Visibility.Collapsed;
                    comboBoxAndButton.Visibility = Visibility.Visible;
                    break;

                case PopUpType.AddComment:

                    TaskGrid.Visibility = Visibility.Collapsed;
                    CommentGrid.Visibility = Visibility.Visible;

                    break;

                default:
                    throw new ArgumentOutOfRangeException("popUpType");
            }
        }


        private void PauseButtonClick(object sender, RoutedEventArgs e)
        {
            TaskObject selectedObject = (sender as Button).DataContext as TaskObject;

            ShowPopUp(PopUpType.AddComment);
            CommentUserControl.DataContext = selectedObject;
        }

        private void NewTimeEntryObject(string id)
        {
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

        private void NewTaskButtonClick(object sender, RoutedEventArgs e)
        {
            UpdatePopupBoxContent(PopUpType.NewTask);
        }

        private async void StartTaskBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult messageBoxResult = new MessageBoxResult();
                string newTaskId;

                if (newTaskUserControl.Visibility == Visibility.Visible)
                {
                    newTaskId = newTaskUserControl.NewWorkingTask(sender, e);

                    messageBoxResult = await MessageBox.ShowAsync("Would you like to start this task now?", "Task Added", MessageBoxButton.YesNo);
                }
                else
                {
                    TaskObject selectedTask = TaskComboBox.SelectedItem as TaskObject;

                    newTaskId = selectedTask.UniqueId;
                }

                if (messageBoxResult != MessageBoxResult.No)
                {
                    NewTimeEntryObject(newTaskId); 
                }

                HidePopUp();
            }
            catch (Exception)
            {
                MessageBox.ShowAsync("Error adding task, please check your fields", "Error", MessageBoxButton.OK);
            }
        }

        private void AddTaskClick(object sender, RoutedEventArgs e)
        {
            ShowPopUp(PopUpType.NewTask);

            isFromAppBar = true;
            BottomAppBar.IsOpen = false;
        }

        private void StartTaskClick(object sender, RoutedEventArgs e)
        {
            ShowPopUp(PopUpType.StartTask);

            isFromAppBar = true;
            BottomAppBar.IsOpen = false;
        }

        private void SaveComment(object sender, RoutedEventArgs e)
        {
            CommentUserControl.SaveComment(sender, e);

            CollapseCommentBox();
        }

        private void CancelComment(object sender, RoutedEventArgs e)
        {
            CollapseCommentBox();
        }

        private void CollapseCommentBox()
        {
            TaskObject selectedObject = CommentUserControl.DataContext as TaskObject;

            if (selectedObject.IsRunning)
            {
                PauseTask();
            }
            else
            {
                NewTimeEntryObject(selectedObject.UniqueId);
            }

            HidePopUp();
        }
    }
}
