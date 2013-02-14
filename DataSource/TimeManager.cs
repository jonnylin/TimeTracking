using DataSource.Common;
using System;

namespace DataSource
{
    public class TimeManager : BindableBase
    {
        private static TimeManager _timeManager = new TimeManager();

        public static TimeManager CurrentObject
        {
            get { return _timeManager; }
        }

        private TimeEntry _runningTimeEntry;

        public TimeEntry RunningTimeEntry
        {
            get
            {
                return this._runningTimeEntry;
            }

            set { SetProperty(ref this._runningTimeEntry, value); }
        }

        public static void StartNonWorkingTask()
        {
            StartTimerById("nwt");
        }

        public static void SavePreviousTask()
        {
            TaskObject taskToSave = AppDataSource.GetTaskObjectById(_timeManager.RunningTimeEntry.UniqueId);
            taskToSave.IsRunning = false;

            _timeManager.RunningTimeEntry.SetTimeTaken(DateTime.Now);

            AppDataSource.CurrentObject.TaskCollection.AddToCollection(_timeManager.RunningTimeEntry);

            AppDataSource.CurrentObject.DayManager.Today.TimeEntriesInADay.Add(_timeManager.RunningTimeEntry);
        }

        public static void StartTimerById(string id)
        {
            if (_timeManager.RunningTimeEntry != null)
            {
                SavePreviousTask();
            }

            TimeEntry timeEntry = new TimeEntry() { StartTime = DateTime.Now, UniqueId = id };
            _timeManager.RunningTimeEntry = timeEntry;

            TaskObject taskToStart = AppDataSource.GetTaskObjectById(id);
            taskToStart.IsRunning = true;
        }
    }
}
