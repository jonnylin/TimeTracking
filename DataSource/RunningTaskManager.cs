using DataSource.Common;
using System.Collections.ObjectModel;

namespace DataSource
{
    public class RunningTaskManager : BindableBase
    {
        private bool _showHelpText = true;
        public bool ShowHelpText
        {
            get { return _showHelpText; }
            set { SetProperty(ref _showHelpText, value); }
        }

        private bool _showHelpTextTasks = true;
        public bool ShowHelpTextTasks
        {
            get { return _showHelpTextTasks; }
            set { SetProperty(ref _showHelpTextTasks, value); }
        }

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
            ShowHelpText = true;

            if (taskObject.IsVisible)
            {
                ShowHelpText = false;

                if (taskObject.Deleted)
                {
                    NotRunningTasks.Remove(taskObject);
                    RunningTasks.Remove(taskObject);
                }
                else
                {
                    UpdateLists(taskObject);
                }
            }

            ShowHelpTextTasks = NotRunningTasks.Count == 0;
        }

        private void UpdateLists(TaskObject taskObject)
        {
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
