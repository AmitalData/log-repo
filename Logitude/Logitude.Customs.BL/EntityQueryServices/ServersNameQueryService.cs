using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class ServersNameQueryService //: EntityQueryService<ServersName, ServersNameKeys, ServersNamePM, object, ServersNameKeys>
    {


        public bool Any()
        {   
            return this.repository.Any();
        }

        public List<string> GetServiceNameListByMachineName(string machineName)
        {
            return this.repository.GetServiceNameListByMachineName(machineName);

        }
    }
}
