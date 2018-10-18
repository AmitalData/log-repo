 
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
   public partial class VendorCommunicationRepository:IRepository<VendorCommunication>
   {
        
		public List<VendorCommunication> GetMulti(EntityKeyFields entityKeys)
        {

            CustomsVendorKeys vendorKeys = entityKeys as CustomsVendorKeys;

            return (from a in context.VendorCommunications
                    where a.VendorId == vendorKeys.Id
                    select a).ToList();
        }

   }

}
   