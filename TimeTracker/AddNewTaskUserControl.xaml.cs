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
    public sealed partial class AddNewTaskUserControl : UserControl
    {
        public AddNewTaskUserControl()
        {
            this.InitializeComponent();

            this.Loaded += AddNewTaskUserControl_Loaded;
        }

        void AddNewTaskUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            categoryComboBox.DataContext = AppDataSource.GetCurrentObject();
        }

        public string NewWorkingTask(object sender, RoutedEventArgs e)
        {
            string name = nameBox.Text;
            string comment = commentBox.Text;
            bool isWorking = (bool)workingTask.IsChecked;
            int level = int.Parse(levelBox.Text);

            HierarchyTaskObject selectedLevel = categoryComboBox.SelectedItem as HierarchyTaskObject;

            if (selectedLevel == null)
            {
                selectedLevel = AppDataSource.FindTaskObject("noCat");
            }

            string id = Guid.NewGuid().ToString();

            AppDataSource.ModifyNodeParent(AppDataSource.AddTask(name, isWorking, level, comment, false, id), selectedLevel.taskObj.UniqueId);

            return id;
        }
    }
}
