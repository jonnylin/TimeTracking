using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource
{
    public class HierarchyObject: EntityBase
    {
        public TaskObject Parent { get; set; }
        public TaskObject CurrentNode { get; set; }

        public TimeSpan TotalTimeTaken
        {
            get
            {
                TimeSpan timeSpan = new TimeSpan();
                foreach (var item in Children)
                {
                    if (item.TimeEntryCollection != null)
                    {
                        timeSpan += item.TimeEntryCollection.TotalTime; 
                    }
                }

                if (CurrentNode.TimeEntryCollection != null) timeSpan += CurrentNode.TimeEntryCollection.TotalTime;

                return timeSpan;
            }
        }

        public ObservableCollection<TaskObject> Children { get; set; }
        public HierarchyObject(string uniqueId, string name, TaskObject parent, TaskObject node) 
        :base (uniqueId, name)
        {
            this.Parent = parent;
            this.CurrentNode = node;
            this.Children = new ObservableCollection<TaskObject>();
        }

    }
}
