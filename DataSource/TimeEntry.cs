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
        public DateTime StartTime { get; set; }

        public TimeSpan TimeTaken { get; set; }

        public string UniqueId { get; set; }

        public string Comment { get; set; }

        public void SetTimeTaken(DateTime newTime)
        {
            this.TimeTaken = newTime - StartTime;
        }
    }
}
