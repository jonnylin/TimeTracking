using DataSource.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource
{
    public class TaskManager : BindableBase
    {
        private TaskObject _newObject;

        public TaskObject NewObject
        {
            get
            {
                return _newObject ?? (_newObject = new TaskObject("", Guid.NewGuid().ToString(), "", false, false));
            }
            set { SetProperty(ref _newObject, value); }
        }

        public void AddTaskObject(string parentId)
        {
           string newID= AppDataSource.AddTask(NewObject.Name, NewObject.IsWorking, 1, NewObject.Comment, false);

            AppDataSource.ModifyNodeParent(newID, parentId);
        }
    }
}
