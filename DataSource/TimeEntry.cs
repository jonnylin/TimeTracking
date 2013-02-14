using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataSource.Common;

namespace DataSource
{
    public class TimeEntry : BindableBase
    {
        private DateTime _startTime;
        private TimeSpan _timeTaken;
        private string _comment;

        public DateTime StartTime
        {
            get { return _startTime; }
            set { SetProperty(ref _startTime, value); }
        }

        public DateTime EndTime
        {
            get { return this.StartTime + TimeTaken; }
        }

        public TimeSpan TimeTaken
        {
            get { return _timeTaken; }
            set { SetProperty(ref _timeTaken, value); }
        }

        public string UniqueId { get; set; }

        public string TaskName
        {
            get { return AppDataSource.GetTaskObjectById(this.UniqueId).Name; }
        }

        public string Comment
        {
            get { return _comment; }
            set { SetProperty(ref _comment, value); }
        }

        public void SetTimeTaken(DateTime newTime)
        {
            this.TimeTaken = newTime - StartTime;
        }
    }
}
