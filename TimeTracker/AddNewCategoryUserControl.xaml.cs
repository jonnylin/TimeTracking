using System;
using DataSource;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimeTracker
{
    public sealed partial class AddNewCategoryUserControl : UserControl
    {
        public AddNewCategoryUserControl()
        {
            this.InitializeComponent();
        }

        public async void NewCategory(object sender, RoutedEventArgs e)
        {
            string name = nameBox.Text;
            string comment = commentBox.Text;

            string id = Guid.NewGuid().ToString();

            string returnVal = AppDataSource.AddTask(name, false, 1, comment, true);
            await AppDataSource.SaveLocalData();
        }
    }
}
