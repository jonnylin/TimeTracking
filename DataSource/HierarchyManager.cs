using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource
{
    public class HierarchyManager: EntityBase
    {
        private static ObservableCollection<LevelObject> _allLevels;
        public Guid guid { get; set; }
        
        //List<List<int>> list = new List<List<int>>();

        public HierarchyManager(string uniqueId, string name, Guid guid_input) : base (uniqueId, name)
        {
            _allLevels = new ObservableCollection<LevelObject>();
            guid = guid_input;
        }

        public ObservableCollection<LevelObject> GetAllLevels
        {
            get { return _allLevels; }
        }

        public bool AddLevel(int index)
        {
            bool added = false;
            if (_allLevels.Count > 0)
            {
                if (index-1 < _allLevels.Count)
                {
                    if (_allLevels[index-1] != null)
                    {
                        _allLevels.Insert(index-1, new LevelObject(Guid.NewGuid().ToString(), (index-1).ToString()));
                        added = true;
                    }
                }
                else
                {
                    _allLevels.Add(new LevelObject(Guid.NewGuid().ToString(), (_allLevels.Count+1).ToString()));
                    added = true;
                }
            }
            else
            {
                _allLevels.Add(new LevelObject(Guid.NewGuid().ToString(), "1"));
                added = true;
            }
            UpdateLevelLabel();

            return added;
        }

        public bool RemoveLevel(int index)
        {
            bool removed = false;
            if (_allLevels.Count > 0)
            {
                if (_allLevels[index-1].LevelCollection.Count.Equals(0))
                {
                    _allLevels.Remove(_allLevels[index-1]);
                    removed = true;
                }
            }
            UpdateLevelLabel();

            return removed;
        }

        public bool AddTaskNode(HierarchyObject taskRelations, TaskObject taskToAdd, LevelObject levelToAddTo)
        {
            bool added = false;
            if (_allLevels.Count > 0)
            {
                foreach (LevelObject item in _allLevels)
                {
                    if (item.UniqueId.Equals(levelToAddTo.UniqueId))
                    {
                        bool valid = true;
                        foreach (HierarchyTaskObject items in item.LevelCollection)
                        {
                            if (taskRelations.UniqueId == items.hierarchyObj.UniqueId 
                                && taskToAdd.UniqueId == items.taskObj.UniqueId)
                            {
                                valid = false;
                            }
                        }
                        if(valid == true)
                        {
                            HierarchyTaskObject TO = new HierarchyTaskObject(Guid.NewGuid().ToString(), taskRelations.Name+taskToAdd.Name);
                            TO.hierarchyObj = taskRelations;
                            TO.taskObj = taskToAdd;
                            levelToAddTo.LevelCollection.Add(TO);
                            added = true;
                            break;
                        }
                    }
                }
            }
            return added;
        }

        public bool RemoveTaskNode(HierarchyTaskObject taskToRemove)
        {
            bool removed = false;

            if (_allLevels.Count > 0)
            {
                foreach (LevelObject element in _allLevels)
                {
                    foreach (HierarchyTaskObject item in element.LevelCollection)
                    {
                        if (taskToRemove.hierarchyObj.UniqueId == item.hierarchyObj.UniqueId
                            && taskToRemove.taskObj.UniqueId == item.taskObj.UniqueId)
                        {
                            //element.LevelCollection.Remove(taskToRemove);
                            taskToRemove.taskObj.Deleted = true;
                            removed = true;
                            break;
                        }
                    }
                    foreach (HierarchyTaskObject item in element.LevelCollection)
                    {
                        if (item.hierarchyObj.Parent != null)
                        {
                            if (item.hierarchyObj.Parent.UniqueId.Equals(taskToRemove.hierarchyObj.CurrentNode.UniqueId))
                            {
                                item.hierarchyObj.Parent = taskToRemove.hierarchyObj.Parent;
                            }
                            else
                            {
                                item.hierarchyObj.Parent = null;
                            }
                        }
                    }
                }
            }
            return removed;
        }

        public bool MoveTask(int levelIndex, HierarchyTaskObject taskToMove)
        {
            bool validMove = true;
            for (int i = levelIndex-1; i < _allLevels.Count; i++)
            {
                for (int x = 0; x < _allLevels[i].LevelCollection.Count; x++)
                {
                    if ((taskToMove.hierarchyObj.Parent != null 
                        && _allLevels[i].LevelCollection[x]!=null 
                        && _allLevels[i].LevelCollection[x].hierarchyObj.CurrentNode.UniqueId.Equals(taskToMove.hierarchyObj.Parent.UniqueId))
                        || (_allLevels[i].LevelCollection[x]!=null 
                        && _allLevels[i].LevelCollection[x].hierarchyObj.Parent!=null 
                        &&_allLevels[i].LevelCollection[x].hierarchyObj.Parent.UniqueId.Equals(taskToMove.hierarchyObj.CurrentNode.UniqueId)))
                        {
                            validMove = false;
                            break;
                        }
                }
                if (validMove == false)
                {
                    break;
                }
            }
            if (validMove == true)
            {
                bool removed = false;
                foreach (LevelObject level in _allLevels)
                {
                    foreach (HierarchyTaskObject taskObj in level.LevelCollection)
                    {
                        if (taskObj.taskObj.UniqueId.Equals(taskToMove.taskObj.UniqueId))
                        {
                            level.LevelCollection.Remove(taskObj);
                            removed = true;
                            break;
                        }
                    }
                    if (removed == true)
                    {
                        break;
                    }
                }
                _allLevels[levelIndex-1].LevelCollection.Add(taskToMove);
            }
            return validMove;
        }

        public void UpdateLevelLabel()
        {
            for (int i = 0; i < _allLevels.Count; i++)
            {
                _allLevels[i].Name = (i + 1).ToString();
            }
        }
    }
}
