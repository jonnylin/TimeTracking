using DataSource;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TimeTracker
{
    public sealed partial class CommentUserControl : UserControl
    {
        CommentUserControl thisControl;

        public CommentUserControl()
        {
            this.InitializeComponent();

            thisControl = this;
        }


        public void SaveComment(object sender, RoutedEventArgs e)
        {
            TimeEntry timeEntry = TimeManager.GetCurrentObject().RunningTimeEntry;

            timeEntry.Comment = commentBox.Text;
        }
    }
}
