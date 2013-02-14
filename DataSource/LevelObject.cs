using System.Collections.ObjectModel;

namespace DataSource
{
    public class LevelObject : EntityBase
    {
        private ObservableCollection<HierarchyTaskObject> _levelCollection;

        public ObservableCollection<HierarchyTaskObject> LevelCollection
        {
            get { return _levelCollection; }
            set { _levelCollection = value; }
        }

        private ObservableCollection<HierarchyTaskObject> _visibleLevelCollection = new ObservableCollection<HierarchyTaskObject>();

        public ObservableCollection<HierarchyTaskObject> VisibleLevelCollection
        {
            get
            {
                _visibleLevelCollection.Clear();

                foreach (HierarchyTaskObject hierarchyTaskObject in _levelCollection)
                {
                    if (hierarchyTaskObject.taskObj.IsVisible)
                    {
                        _visibleLevelCollection.Add(hierarchyTaskObject);
                    }
                }

                return _visibleLevelCollection;
            }
        }

        public LevelObject(string uniqueid, string name)
            : base(name, uniqueid)
        {
            _levelCollection = new ObservableCollection<HierarchyTaskObject>();
        }

        public bool AddTaskToLevel(HierarchyTaskObject addition)
        {
            bool added = false;
            if (_levelCollection.Count > 0)
            {
                foreach (HierarchyTaskObject item in _levelCollection)
                {
                    if (item.hierarchyObj != null && item.taskObj != null)
                    {
                        if (!item.hierarchyObj.UniqueId.Equals(addition.hierarchyObj.UniqueId)
                            && !item.taskObj.UniqueId.Equals(addition.taskObj.UniqueId))
                        {
                            added = AddItem(addition);
                        }
                    }
                }
            }
            else
            {
                added = AddItem(addition);
            }

            return added;
        }

        private bool AddItem(HierarchyTaskObject addition)
        {
            _levelCollection.Add(addition);

            if (addition.taskObj.IsVisible)
            {
                _visibleLevelCollection.Add(addition);
            }

            return true;
        }

        public bool RemoveFromCollection(HierarchyTaskObject remove)
        {
            bool removed = false;
            if (_levelCollection.Count > 0)
            {
                if (_visibleLevelCollection.Contains(remove))
                {
                    _visibleLevelCollection.Remove(remove);
                }
                _levelCollection.Remove(remove);
                removed = true;
            }
            return removed;
        }
    }
}
