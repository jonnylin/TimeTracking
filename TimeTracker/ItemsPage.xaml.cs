using System.Linq;

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DataSource;

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace TimeTracker
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class ItemsPage : TimeTracker.Common.LayoutAwarePage
    {
        public ItemsPage()
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
        }

        private void HidePopUp()
        {
            normalControl.IsEnabled = true;
            this.Opacity = 1;
            popupControl.IsEnabled = false;
            logincontrol1.IsOpen = false;
        }

        private void ShowNewTaskBox(bool isCat)
        {
            normalControl.IsEnabled = false;
            this.Opacity = .4;
            popupControl.IsEnabled = true;
            logincontrol1.IsOpen = true;
            pop.Width = Window.Current.Bounds.Width;

            if (isCat)
            {
                newTaskUserControl.Visibility = Visibility.Collapsed;
                newCatUserControl.Visibility = Visibility.Visible;
            }
            else
            {
                newTaskUserControl.Visibility = Visibility.Visible;
                newCatUserControl.Visibility = Visibility.Collapsed;
            }
        }

        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TaskList));
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            ShowNewTaskBox(false);

            BottomAppBar.IsOpen = false;
        }

        private void AddCatClick(object sender, RoutedEventArgs e)
        {
            ShowNewTaskBox(true);

            BottomAppBar.IsOpen = false;
        }

        private  async void DeleteCategory(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = await MessageBox.ShowAsync("Are you sure you want to delete the category? All tasks within the category will be ", "Delete Task", MessageBoxButton.YesNo);

            var button = sender as Button;
            if (button != null)
            {
                var selectedHierachyObject = button.DataContext as HierarchyTaskObject;

                if (selectedHierachyObject != null)
                {
                    TaskObject selectedObject = selectedHierachyObject.taskObj;

                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            {
                                AppDataSource.DeleteTask(selectedObject.UniqueId);
                                break;
                            }
                        case MessageBoxResult.No:
                            {
                                break;
                            }
                    }
                }
            }
        }

        private async void DeleteTask(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = await MessageBox.ShowAsync("Are you sure you want to delete task?", "Delete Task", MessageBoxButton.YesNo);

            var button = sender as Button;
            if (button != null)
            {
                var selectedObject = button.DataContext as TaskObject;

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        {
                            if (selectedObject != null) AppDataSource.DeleteTask(selectedObject.UniqueId);
                            break;
                        }
                    case MessageBoxResult.No:
                        {
                            break;
                        }
                }
            }
        }

        private void StartTaskBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (newTaskUserControl.Visibility == Visibility.Visible)
                {
                    newTaskUserControl.NewWorkingTask(sender, e);
                }
                else
                {
                    newCatUserControl.NewCategory(sender, e);
                }

                HidePopUp();
            }
            catch (Exception)
            {
                MessageBox.ShowAsync("Error adding task, please check your fields", "Error", MessageBoxButton.OK);
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            HidePopUp();
        }

        #region Drag and Drop


        private void ListView_DragItemsStarting_1(object sender, DragItemsStartingEventArgs e)
        {
            var item = e.Items.FirstOrDefault();
            if (item == null)
                return;

            e.Data.Properties.Add("item", item);
            e.Data.Properties.Add("dragSource", sender);
        }

        private void CategoryDrop(object sender, DragEventArgs e)
        {
            object gridSource;
            e.Data.Properties.TryGetValue("dragSource", out gridSource);

            if (gridSource == sender)
                return;

            object sourceItem;
            e.Data.Properties.TryGetValue("item", out sourceItem);
            if (sourceItem == null)
                return;

            TaskObject droppedObject = sourceItem as TaskObject;

            AppDataSource.ModifyNodeParent(droppedObject.UniqueId, ((sender as Grid).DataContext as HierarchyTaskObject).taskObj.UniqueId);
            //
        } 
        #endregion
    }
}
