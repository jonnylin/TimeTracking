using System;
using System.Collections.Generic;

namespace DataSource
{
    public class TaskCollection
    {
        private static List<TimeEntryCollection> _uniqueIdCollection = new List<TimeEntryCollection>();

        public List<TimeEntryCollection> UniqueIdCollection
        {
            get { return _uniqueIdCollection; }
            set { _uniqueIdCollection = value; }
        }

        public TimeEntryCollection GetCollectionById(string id)
        {
            foreach (var item in _uniqueIdCollection)
            {
                if (item.TaskObject.UniqueId == id)
                {
                    return item;
                }
            }
            return null;
        }

        public void AddToCollection(TimeEntry timeEntry)
        {
            foreach (var item in _uniqueIdCollection)
            {
                if (item.TaskObject.UniqueId == timeEntry.UniqueId)
                {
                    item.TimeEntries.Add(timeEntry);
                    return;
                }
            }

            _uniqueIdCollection.Add(new TimeEntryCollection(timeEntry.UniqueId, timeEntry));
        }
    }

    public class TimeEntryCollection
    {
        public TaskObject TaskObject { get; private set; }

        private List<TimeEntry> _timeEntries = new List<TimeEntry>();

        public List<TimeEntry> TimeEntries
        {
            get { return _timeEntries; }
            set { _timeEntries = value; }
        }

        public TimeSpan TotalTime
        {
            get
            {
                TimeSpan returnTime = new TimeSpan();

                foreach (var item in TimeEntries)
                {
                    returnTime += item.TimeTaken;
                }

                return returnTime;
            }
        }

        public TimeEntryCollection(string id, TimeEntry timeEntry)
        {
            TaskObject = AppDataSource.GetTaskObjectById(id);
            TimeEntries.Add(timeEntry);
        }
    }
}
