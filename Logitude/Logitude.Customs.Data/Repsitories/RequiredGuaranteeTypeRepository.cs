 
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
   public partial class RequiredGuaranteeTypeRepository:IRepository<RequiredGuaranteeType>
   {

       public List<RequiredGuaranteeType> GetMulti(EntityKeyFields entityKeys)
       {

           GuaranteeKeys parentKeys = entityKeys as GuaranteeKeys;

           return (from a in context.RequiredGuaranteeTypes
                   where a.GuaranteeId == parentKeys.Id
                   select a).ToList();
       }	

   }

}
   