using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TimeTracker
{
    public class ListViewStyleSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return (DataTemplate)Application.Current.Resources["SubTaskItemTemplate"];
        }

    }
}
