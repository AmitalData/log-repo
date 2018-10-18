 
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
   public partial class ConsignmentPackageRepository:IRepository<ConsignmentPackage>
   {
        
		public List<ConsignmentPackage> GetMulti(EntityKeyFields entityKeys)
        {

            ConsignmentKeys ConsignmentKeys = entityKeys as ConsignmentKeys;

            return (from a in context.ConsignmentPackages
                    where a.DeclarationId == ConsignmentKeys.DeclarationId && a.ConsignmentNumber == ConsignmentKeys.ConsignmentNumber 
                    select a).ToList();
        }


        public void FastDeleteMulti(DeclarationKeys entityKeyFields)
        {

            (context as DbContextBase)
                .DeleteWhere<ConsignmentPackage>(rec => rec.DeclarationId == entityKeyFields.Id);
        }
   }

}
   