 
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
   public partial class ClientDrivingLicenseTypeRepository:IRepository<ClientDrivingLicenseType>
   {
        
		public List<ClientDrivingLicenseType> GetMulti(EntityKeyFields entityKeys)
        {
            ClientDrivingLicenseKeys clientKeys = entityKeys as ClientDrivingLicenseKeys;
            return (from a in context.ClientDrivingLicenseTypes
                    where a.ClientId == clientKeys.ClientId && a.ClientDrivingLicenseLine == clientKeys.Line
                    select a).ToList();
        }

   }

}
   