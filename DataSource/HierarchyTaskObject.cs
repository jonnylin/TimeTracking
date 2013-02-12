using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource
{
    public class HierarchyTaskObject: EntityBase
    {
        public TaskObject taskObj { get; set; }
        public HierarchyObject hierarchyObj { get; set; }

        public HierarchyTaskObject(string uniqueid, string name):base (name, uniqueid){}
    }
}
