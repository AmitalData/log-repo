using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.EntityLists
{
    public partial class DeclarationReferantDataList
    {
        public IEnumerable<string> OccuredStatuses { get; set; }= new List<string>();

    }
}
