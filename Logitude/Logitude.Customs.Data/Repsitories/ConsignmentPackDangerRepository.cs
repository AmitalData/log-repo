 
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
   public partial class ConsignmentPackDangerRepository:IRepository<ConsignmentPackDanger>
   {
        
		public List<ConsignmentPackDanger> GetMulti(EntityKeyFields entityKeys)
        {

            ConsignmentPackageKeys consignmentPackageKeys = entityKeys as ConsignmentPackageKeys;

            return (from a in context.ConsignmentPackDangers
                    where a.DeclarationId == consignmentPackageKeys.DeclarationId && a.LineNumber == consignmentPackageKeys.LineNumber && a.ConsignmentNumber ==consignmentPackageKeys.ConsignmentNumber
                    select a).ToList();
        }

   }

}
   