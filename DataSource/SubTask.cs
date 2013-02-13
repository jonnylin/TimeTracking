using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource
{
    public class TaskObject : EntityBase, IDataModel
    {
        public bool IsCat
        {
            get
            {
                return (AppDataSource.FindTaskObject(this.UniqueId).hierarchyObj.Parent == null);
            }
        }

        public bool IsVisible { get; set; }

        public TimeEntryCollection TimeEntryCollection { get { return AppDataSource.CurrentObject.TaskCollection.GetCollectionById(UniqueId); } }

        public bool IsWorking { get; set; }

        private bool _isRunning;
        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                SetProperty(ref _isRunning, value);

                try
                {
                    AppDataSource.CurrentObject.RunningTaskManager.UpdateTask(this);
                }
                catch (Exception)
                {
                }
            }
        }

        public bool Finished { get; set; }
        public DateTime LastModified { get; set; }

        private bool _deleted;
        public bool Deleted
        {
            get { return _deleted; }
            set
            {
                _deleted = value;

                if (this._deleted)
                {
                    this.IsVisible = false;
                }
            }
        }

        public string Parent
        {
            get
            {
                return (from hierachyTaskObject in AppDataSource.NodeManager.GetAllLevels[1].LevelCollection where hierachyTaskObject.taskObj.UniqueId == this.UniqueId select hierachyTaskObject.hierarchyObj.Parent.Name).FirstOrDefault();
            }
        }


        private string _comment;
        public string Comment { get { return _comment; } set { SetProperty(ref _comment, value); } }

        public TaskObject(string name, string uniqueId, string comment, bool working, bool isVisible = true)
            : base(name, uniqueId)
        {
            this.IsVisible = isVisible;
            this.Comment = comment;
            this.IsRunning = false;
            this.Finished = false;
            this.IsWorking = working;
            this.Deleted = false;
        }

        public async Task<IDataModel> LoadData()
        {
            return null;
        }

        public async Task<bool> SaveData()
        {
            return true;
        }
    }
}
