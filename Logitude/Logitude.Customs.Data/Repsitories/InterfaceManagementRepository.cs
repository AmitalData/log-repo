 
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
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class InterfaceManagementRepository:IRepository<InterfaceManagement>
   {
        
		public List<InterfaceManagement> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public InterfaceManagement GetSingleInterfaceManagement(EntityKeyFields entityKeys)
        {
            InterfaceManagementKeys keys = entityKeys as InterfaceManagementKeys;
            return (from a in context.InterfaceManagements.Include("InterfaceSendOption")
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }

   }

}
   