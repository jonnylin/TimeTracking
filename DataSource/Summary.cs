using DataSource.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource
{
    public class Summary : BindableBase
    {
        private static Summary _summary = new Summary();

        private static TimeSpan TimeTaken(bool working)
        {
            TimeSpan timeSpan = new TimeSpan(0);

            foreach (Day day in AppDataSource.GetCurrentObject().AllDays)
            {
                foreach (TimeEntry task in day.TimeEntriesInADay)
                {
                    try
                    {
                        if (AppDataSource.IsTaskWorking(task.UniqueId))
                        {
                            if (working)
                            {
                            timeSpan += task.TimeTaken;
                            }
                        }
                        else
                        {
                            if (!working)
                            {
                                timeSpan += task.TimeTaken;
                            }
                        }
                    }
                    catch (NullReferenceException)
                    {
                    }
                }
            }

            return timeSpan;
        }

        public TimeSpan TotalWorkingHour
        {
            get
            {
                return  TimeTaken(true);
            }
        }

        public TimeSpan TotalNonWorkingHour
        {
            get
            {
                return TimeTaken(false);
            }
        }

    }
}
