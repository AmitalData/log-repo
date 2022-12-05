 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class ServersNameRepository:IRepository<ServersName>
   {
        
		public List<ServersName> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public bool Any()
        {
            return  context.ServersNames.Any();
        }

        public List<string> GetServiceNameListByMachineName(string machineName)
        {
            machineName = machineName.ToLower();
            var serviceNameList = context.ServersNames
                .Where(r => r.ServerName.ToLower() == machineName)
                .Select(r => r.ServiceName)
                .ToList();

            return serviceNameList;

        }

    }

}
   