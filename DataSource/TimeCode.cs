using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource
{
    public class TimeCode : EntityBase, IDataModel
    {
        public ObservableCollection<SubTask> SubTasks { get; set; }
        public List<SubTask> SubTasksList { get; set; }

        public async Task<IDataModel> LoadData()
        {
            return new TimeCode("New Time Code Name",Parameters.randomID,"Comment comment comment blah blah");
        }

        public async Task<bool> SaveData()
        {
            return true;
        }

        public TimeSpan TotalTime()
        {
            return new TimeSpan(1, 20, 30);
        }

        public TimeCode(string name, string id, string comment) : base(name,comment,id)
        {

        }
    }
}
