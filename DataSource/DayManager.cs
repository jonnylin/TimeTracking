using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataSource.Common;

namespace DataSource
{
    public class DayManager : BindableBase
    {
        private static DayManager _dayManager = new DayManager();
        public static DayManager CurrentDayManager
        {
            get { return _dayManager; }
            set { _dayManager = value; }
        }

        private ObservableCollection<Day> _allDays;
        public ObservableCollection<Day> AllDays
        {
            get
            {
                if (_dayManager._allDays == null)
                {
                    _dayManager._allDays = new ObservableCollection<Day>();
                }
                return _dayManager._allDays;
            }
        }

        private Day _today;
        public Day Today
        {
            get
            {
                if (_dayManager._today == null || _dayManager._today.CreatedTime.Date != DateTime.Today)
                {
                    _dayManager._today = new Day();

                    _dayManager.AllDays.Add(_dayManager._today);
                }

                return _dayManager._today;
            }
            set { _dayManager._today = value; }
        }


    }
}
