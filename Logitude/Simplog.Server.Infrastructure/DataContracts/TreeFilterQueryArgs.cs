using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.DataContracts
{
   public class TreeFilterQueryArgs
    {
        public string ObjectTableName { get; set; }
        public string AdditionalTreeFilter { get; set; }
        public int Tenant { get; set; }
        public string ParentEntityId { get; set; }
        public string ParentObjectTableName { get; set; }
        public object ParentEntity { get; set; }
        public Type Type { get; set; }
    }
}
