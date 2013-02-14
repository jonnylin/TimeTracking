using DataSource.Common;
using Newtonsoft.Json;
using Windows.Storage;

using System.IO;
using System.Runtime.Serialization;
using Windows.Storage.Streams;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource
{
    public class AppDataSource: BindableBase, IDataModel
    {
        private static AppDataSource _appDataSource = new AppDataSource();

        public static AppDataSource CurrentObject
        {
            get { return _appDataSource; }
            set { _appDataSource = value; }
        }

        private Guid guid { get; set; }
        public HierarchyManager NodeManager { get; set; }
        private string JSON;
        private SaveLoadData model;

        private string folderName = "TimeTracker";
        private string fileName = "TimeTracker.json";

        private RunningTaskManager _runningTaskManager;
        public RunningTaskManager RunningTaskManager
        {
            get
            {
                if (_runningTaskManager == null)
                {
                    _runningTaskManager = new RunningTaskManager();
                }
                return _runningTaskManager;
            }
            set { _runningTaskManager = value; }
        }

        private TaskCollection _taskCollection;
        public TaskCollection TaskCollection
        {
            get
            {
                if (_taskCollection == null)
                {
                    _taskCollection = new TaskCollection();
                }
                return _taskCollection;
            }
            set { _taskCollection = value; }
        }

        private ObservableCollection<TaskObject> _totalTasks;
        public ObservableCollection<TaskObject> TotalTasks
        {
            get
            {
                if (_totalTasks == null)
                {
                    _totalTasks = new ObservableCollection<TaskObject>();
                }
                return _totalTasks;
            }
        }

        private ObservableCollection<TaskObject> _visibleTasks;
        public ObservableCollection<TaskObject> VisibleTasks
        {
            get
            {
                this._visibleTasks = new ObservableCollection<TaskObject>();

                foreach (var item in _appDataSource.TotalTasks.Where(t => t.IsVisible == true))
                {
                    this._visibleTasks.Add(item);
                }

                return this._visibleTasks;
            }
            set { this._visibleTasks = value; }
        }

        private ObservableCollection<TaskObject> _nonWorkingTasks;
        public ObservableCollection<TaskObject> NonWorkingTasks
        {
            get
            {
                this._nonWorkingTasks = new ObservableCollection<TaskObject>();

                foreach (var item in _appDataSource.TotalTasks.Where(t => t.IsWorking == false))
                {
                    this._nonWorkingTasks.Add(item);
                }

                return this._nonWorkingTasks;
            }
            set { this._nonWorkingTasks = value; }
        }

        private ObservableCollection<TaskObject> _workingTasks;
        public ObservableCollection<TaskObject> WorkingTasks
        {
            get
            {
                this._workingTasks = new ObservableCollection<TaskObject>();

                foreach (var item in _appDataSource.TotalTasks.Where(t => t.IsWorking == true))
                {
                    this._workingTasks.Add(item);
                }

                return this._workingTasks;
            }
            set { this._workingTasks = value; }
        }

        public static bool IsTaskWorking(string id)
        {
            return GetTaskObjectById(id).IsWorking;
        }

        private ObservableCollection<Day> _allDays;
        public ObservableCollection<Day> AllDays
        {
            get
            {
                if (_allDays == null)
                {
                    _allDays = new ObservableCollection<Day>();
                }
                return this._allDays;
            }
        }

        public static Day GetTodayObject()
        {
            return _appDataSource.Today;
        }

        private Day _today;
        public Day Today
        {
            get
            {
                if (_today == null)
                {
                    _today = new Day();

                    this.AllDays.Add(_today);
                }
                return _today;
            }
            set { _today = value; }
        }

        public static TaskObject GetTaskObjectById(string id)
        {
            var gottenTasks = _appDataSource.TotalTasks.Where(task => task.UniqueId == id);
            if (gottenTasks.Count() != 0)
            {
                return gottenTasks.First();
            }
            return null;
        }

        public static string AddTask(string taskName, bool working, int level, string comment, bool isCategory, string id = null, bool isVisible = true)
        {
            string newID = id;

            if (newID == null)
            {
                newID = Guid.NewGuid().ToString();
            }

            TaskObject taskCreated = new TaskObject(taskName, newID.ToString(), comment, working,isVisible);
            HierarchyObject hierarchyNodeMetaData = new HierarchyObject(Guid.NewGuid().ToString(), "hierarchyObject_", null, taskCreated);

            if (isCategory == true)
            {
                _appDataSource.NodeManager.AddTaskNode(hierarchyNodeMetaData, taskCreated, _appDataSource.NodeManager.GetAllLevels[0]);
            }
            else
            {
                _appDataSource.NodeManager.AddTaskNode(hierarchyNodeMetaData, taskCreated, _appDataSource.NodeManager.GetAllLevels[1]);
            }
            _appDataSource.TotalTasks.Add(taskCreated);

            return taskCreated.UniqueId;
        }

        public static bool DeleteTask(string taskid)
        {
            bool found = false;
            if (FindTaskObject(taskid).hierarchyObj.Parent != null)
            {
                foreach (HierarchyTaskObject item in _appDataSource.NodeManager.GetAllLevels[0].LevelCollection)
                {
                    foreach (TaskObject child in item.hierarchyObj.Children)
                    {
                        if (child.UniqueId.Equals(taskid))
                        {
                            item.hierarchyObj.Children.Remove(child);
                            found = true;
                            break;
                        }
                    }
                    if (found == true)
                    {
                        break;
                    }
                }
            }

            _appDataSource.NodeManager.RemoveTaskNode(FindTaskObject(taskid));
            _appDataSource.RunningTaskManager.UpdateTask(FindTask(taskid));

            FindTaskObject(taskid).hierarchyObj.Parent = null;
            FindTaskObject(taskid).hierarchyObj.Children.Clear();

            //leave in collections? (flagged as removed)

            //_appDataSource.NonWorkingTasks.Remove(FindTask(taskid));
            //_appDataSource.WorkingTasks.Remove(FindTask(taskid));
            //_appDataSource.RunningTaskManager.NotRunningTasks.Remove(FindTask(taskid));
            //_appDataSource.RunningTaskManager.RunningTasks.Remove(FindTask(taskid));
            //TotalTasks.Remove(FindTask(taskid));  //diff method needed (specific to this collection)
            return true;
        }

        public static ObservableCollection<TaskObject> GetTasksByCategory(string categoryName)
        {
            var children = new ObservableCollection<TaskObject>();
            foreach (var category in _appDataSource.NodeManager.GetAllLevels[0].LevelCollection)
            {
                if (category.taskObj.Name.Equals(categoryName))
                {
                    children = category.hierarchyObj.Children;
                }
            }
            return children;
        }

        //set or reset parent of node (id req) to parent (id req)
        public async static Task<bool> ModifyNodeParent(string uniqueid, string newParentid)
        {
            HierarchyTaskObject itemToModify = FindTaskObject(uniqueid);
            if (newParentid == "null")
            {
                itemToModify.hierarchyObj.Parent = null;
            }
            else
            {
                if (ValidParentMove(itemToModify, newParentid))
                {
                    if (itemToModify.hierarchyObj.Parent != null)
                    {
                        bool found = false;
                        FindObjectById(itemToModify.hierarchyObj.Parent.UniqueId).hierarchyObj.Children.Remove(
                            itemToModify.taskObj);

                        foreach (var item in _appDataSource.NodeManager.GetAllLevels[0].LevelCollection)
                        {
                            foreach (var task in item.hierarchyObj.Children)
                            {
                                if (task.UniqueId.Equals(itemToModify.taskObj.UniqueId))
                                {
                                    //item.hierarchyObj.Children.Remove(itemToModify.taskObj);
                                    item.hierarchyObj.Children.Remove(task);
                                    found = true;
                                    break;
                                }
                            }
                            if(found == true)
                            {
                                break;
                            }
                        }
                    }
                    itemToModify.hierarchyObj.Parent = FindTaskObject(newParentid).taskObj;
                    FindObjectById(itemToModify.hierarchyObj.Parent.UniqueId).hierarchyObj.Children.Add(itemToModify.taskObj);
                }
            }
            //last modified updated
            return true;
        }

        public static ObservableCollection<TaskObject> GetChildrenOfNode(HierarchyTaskObject nodeToGetChildrenOf)
        {
            return nodeToGetChildrenOf.hierarchyObj.Children;
        }

        public void ModifyNodeDetails(string name, string uniqueid, string comment, bool isNonWorking)
        {
            //populate GUI fields with existing information
            HierarchyTaskObject toEdit = FindTaskObject(uniqueid);
            toEdit.taskObj.Name = name;
            toEdit.taskObj.Comment = comment;
            toEdit.taskObj.IsWorking = isNonWorking;

            foreach (TaskObject item in TotalTasks)
            {
                if (item.UniqueId.Equals(toEdit.taskObj.UniqueId))
                {
                    item.Name = toEdit.taskObj.Comment;
                    item.Comment = toEdit.taskObj.Comment;
                    item.IsWorking = toEdit.taskObj.IsWorking;
                }
            }
            //modify start and end times
            //last modified updated
        }

        //public static void MoveTaskById(string id, int level)
        //{
        //    NodeManager.MoveTask(level, FindObjectById(id));
        //}

        public static void AddLevel(int indexToAddAt)
        {
            _appDataSource.NodeManager.AddLevel(indexToAddAt);
        }

        public static void RemoveLevel(int indexToRemoveAt)
        {
            _appDataSource.NodeManager.RemoveLevel(indexToRemoveAt);
        }

        //finds a hierarchy task object based on id
        public static HierarchyTaskObject FindObjectById(string id)
        {
            HierarchyTaskObject objectToMove = null;
            foreach (LevelObject item in _appDataSource.NodeManager.GetAllLevels)
            {
                foreach (HierarchyTaskObject item2 in item.LevelCollection)
                {
                    if (item2.taskObj.UniqueId.Equals(id))
                    {
                        objectToMove = item2;
                        break;
                    }
                }
                if (objectToMove != null)
                {
                    break;
                }
            }
            return objectToMove;
        }

        //check if attempted parent reconfiguration is valid
        public static bool ValidParentMove(HierarchyTaskObject change, string newParentid)
        {
            bool valid = false;
            int levelWithChange = 0;
            foreach (LevelObject level in _appDataSource.NodeManager.GetAllLevels)
            {
                bool changedValue = false;
                foreach (HierarchyTaskObject item in level.LevelCollection)
                {
                    if (item.hierarchyObj.UniqueId.Equals(change.hierarchyObj.UniqueId))
                    {
                        levelWithChange = Convert.ToInt32(level.Name);
                        changedValue = true;
                        break;
                    }
                }
                if (changedValue == true)
                {
                    break;
                }
            }
            for (int i = 0; i < levelWithChange + 1; i++)
            {
                bool found = false;
                //count through higher levels and check
                foreach (HierarchyTaskObject parentItem in _appDataSource.NodeManager.GetAllLevels[i].LevelCollection)
                {
                    if (parentItem.hierarchyObj.CurrentNode.UniqueId.Equals(newParentid))
                    {
                        valid = true;
                        found = true;
                        break;
                    }
                }
                if(found==true)
                {
                    break;
                }
            }
            return valid;
        }

        public static HierarchyTaskObject FindTaskObject(string uniqueid)
        {
            HierarchyTaskObject itemLookingFor = null;
            bool found = false;

            foreach (LevelObject level in _appDataSource.NodeManager.GetAllLevels)
            {
                foreach (HierarchyTaskObject item in level.LevelCollection)
                {
                    if (item.taskObj.UniqueId.Equals(uniqueid))
                    {
                        itemLookingFor = item;
                        found = true;
                        break;
                    }
                }
                if (found == true)
                {
                    break;
                }
            }
            return itemLookingFor;
        }

        public static TaskObject FindTask(string uniqueid)
        {
            TaskObject itemLookingFor = null;

            foreach (TaskObject item in _appDataSource.TotalTasks)
            {
                if (item.UniqueId.Equals(uniqueid))
                {
                    itemLookingFor = item;
                    break;
                }
            }
            return itemLookingFor;
        }


        public static async Task<bool> SaveLocalData()
        {
            _appDataSource.JSON = JsonConvert.SerializeObject(_appDataSource,Formatting.None,new JsonSerializerSettings(){ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            var file = await _appDataSource.model.CreateFile(_appDataSource.folderName, _appDataSource.fileName);
            await FileIO.WriteTextAsync(file, _appDataSource.JSON);

            return true;
        }

        public static async Task<bool> LoadLocalData()
        {
            AppDataSource tempSource = new AppDataSource();
            tempSource.model = new SaveLoadData();
            await tempSource.model.GetFile(tempSource.folderName, tempSource.fileName);
            if (tempSource.model.saveExists)
            {
                _appDataSource.model = new SaveLoadData();
                _appDataSource = JsonConvert.DeserializeObject<AppDataSource>(await _appDataSource.model.LoadFile(_appDataSource.folderName, _appDataSource.fileName));
                _appDataSource.model = new SaveLoadData();
            }
            else
            {
                SetupDefault();
            }
            return true;
        }

        public static void SetupDefault()
        {
            _appDataSource.model = new SaveLoadData();

            //LoadLocalData();

            _appDataSource.NodeManager = new HierarchyManager(Guid.NewGuid().ToString(), "HierarchyManager", _appDataSource.guid);
            _appDataSource.NodeManager.AddLevel(0);
            _appDataSource.NodeManager.AddLevel(1);

            AddTask("No Category", false, 1, "No Category assigned", true, "noCat");
            AddTask("Non Working Task", false, 1, "No active tasks", true, "nwt", false);
            AddTask("Time Tracker App1", false, 1, "Very Important Project1", true);
            AddTask("Time Tracker App2", false, 1, "Very Important Project2", true);
            AddTask("Time Tracker App3", false, 1, "Very Important Project3", true);
            AddTask("Time Tracker App4", false, 1, "Very Important Project4", true);
            AddTask("Time Tracker App5", false, 1, "Very Important Project5", true);
            AddTask("Time Tracker App6", false, 1, "Very Important Project6", true);
            AddTask("Time Tracker App7", false, 1, "Very Important Project7", true);
            AddTask("Time Tracker App8", false, 1, "Very Important Project8", true);
            AddTask("Time Tracker App9", false, 1, "Very Important Project9", true);
            AddTask("Time Tracker App10", false, 1, "Very Important Project10", true);


            TimeManager.StartTimerById("nwt");
            AppDataSource._appDataSource.TotalTasks.Clear();
            //AddTask("No Category", false, 1, "No Category assigned", true, "noCat");

            //if (AppDataSource.GetTaskObjectById("nwt") == null)
            //{
            //    AppDataSource.AddTask("Non Working Task", false, 1, "No active tasks", true, "nwt", false);
            //}
        }

        

        static AppDataSource()
        {
            //AddTask(new TaskObject("Non Working Task", "nwt", "No active tasks", false),);
            //AddTask("Non Working Task", false, 1, "No active tasks", "nwt");
            //AddTask(new TaskObject("Lunch", "lunch", "Yum", false));
            //AddTask(new TaskObject("End of day", "eod", "Xbox!", false));
            //AddTask(new TaskObject("Test Task1", "1", "Test Task comment", true));
            //AddTask(new TaskObject("Test Task2", "2", "Test Task comment", true));
            //AddTask(new TaskObject("Test Task3", "3", "Test Task comment", true));
            //AddTask(new TaskObject("Test Task4", "4", "Test Task comment", true));
            //AddTask(new TaskObject("Test Task5", "5", "Test Task comment", true));
            //AddTask(new TaskObject("Test Task6", "6", "Test Task comment", true));
            //AddTask(new TaskObject("Test Task7", "7", "Test Task comment", true));
        }

        public Task<IDataModel> LoadData()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveData()
        {
            throw new NotImplementedException();
        }
    }
}
