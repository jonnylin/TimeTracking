using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource
{
    public class TaskManager
    {
        private TaskObject _newObject;

        public TaskObject NewObject
        {
            get { return _newObject ?? (_newObject = new TaskObject("", Guid.NewGuid().ToString(), "", false, false)); }
            set { _newObject = value; }
        }

        public void AddTaskObject(string parentId)
        {
            NewObject.IsVisible = true;
            AppDataSource.CreateHierachyTaskObject(false,NewObject);
            AppDataSource.ModifyNodeParent(NewObject.UniqueId, parentId);
        }

        public void DeleteTempObject()
        {
            NewObject = null;
        }
    }
}
