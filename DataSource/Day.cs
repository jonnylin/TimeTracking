using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataSource.Common;

namespace DataSource
{
    public class Day : BindableBase, IDataModel
    {
        DateTimeFormatInfo dtfi = CultureInfo.CurrentUICulture.DateTimeFormat;

        private DateTime _createdTime = new DateTime();
        public string CreatedTime { get { return _createdTime.ToString("d"); } }

        private ObservableCollection<TimeEntry> _timeEntriesInADay = new ObservableCollection<TimeEntry>();

        public ObservableCollection<TimeEntry> TimeEntriesInADay
        {
            get { return _timeEntriesInADay; }
            set { _timeEntriesInADay = value; }
        }

        public async Task<IDataModel> LoadData()
        {
            return null;
        }

        public async Task<bool> SaveData()
        {
            return true;
        }

        public TimeSpan TotalTime()
        {
            return new TimeSpan(1, 20, 30);
        }

        public Day()
        {
            this._createdTime = DateTime.Now;
        }
    }
}
