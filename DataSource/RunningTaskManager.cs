using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource
{
    public class RunningTaskManager
    {
        private ObservableCollection<TaskObject> _runningTasks = new ObservableCollection<TaskObject>();
        public ObservableCollection<TaskObject> RunningTasks
        {
            get
            {
                return this._runningTasks;
            }
            set { this._runningTasks = value; }
        }

        public void UpdateTask(TaskObject taskObject)
        {
            //if(taskObject.Deleted)
            //{
            //    NotRunningTasks.Remove(taskObject);
            //    RunningTasks.Remove(taskObject);
            //}
            if (taskObject.IsRunning)
            {
                if (!RunningTasks.Contains(taskObject))
                {
                    RunningTasks.Add(taskObject);
                }
                if (NotRunningTasks.Contains(taskObject))
                {
                    NotRunningTasks.Remove(taskObject);
                }
            }
            else
            {
                if (!NotRunningTasks.Contains(taskObject))
                {
                    NotRunningTasks.Add(taskObject);
                }
                if (RunningTasks.Contains(taskObject))
                {
                    RunningTasks.Remove(taskObject);
                }
            }
        }

        private ObservableCollection<TaskObject> _notRunningTasks = new ObservableCollection<TaskObject>();
        public ObservableCollection<TaskObject> NotRunningTasks
        {
            get
            {
                return this._notRunningTasks;
            }
            set { this._notRunningTasks = value; }
        }

    }
}
