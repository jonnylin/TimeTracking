using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource
{
    public class LevelObject: EntityBase
    {
        private ObservableCollection<HierarchyTaskObject> _levelCollection;

        public ObservableCollection<HierarchyTaskObject> LevelCollection
        {
            get { return _levelCollection; }
            set { _levelCollection = value; }
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
                    if (item.hierarchyObj!=null && item.taskObj!=null) 
                    {
                        if (!item.hierarchyObj.UniqueId.Equals(addition.hierarchyObj.UniqueId) 
                            && !item.taskObj.UniqueId.Equals(addition.taskObj.UniqueId))
                        {
                            _levelCollection.Add(addition);
                            added = true;
                        }
                    }
                }
            }
            else
            {
                _levelCollection.Add(addition);
                added = true;
            }
            return added;
        }

        public bool RemoveFromCollection(HierarchyTaskObject remove)
        {
            bool removed = false;
            if (_levelCollection.Count > 0)
            {
                _levelCollection.Remove(remove);
                removed = true;
            }
            return removed;
        }
    }
}
