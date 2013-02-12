using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataSource.Common;

namespace DataSource
{
    public class EntityBase : BindableBase
    {
        private string _uniqueId;
        public string UniqueId { get { return this._uniqueId; } }

        private string _name;
        public string Name { get { return _name; } 
            set { SetProperty(ref _name, value); } }


        public EntityBase(string name, string uniqueId)
        {
            this.Name = name;
            this._uniqueId = uniqueId;
        }
    }
}
