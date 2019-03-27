 
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
using System.Data.Entity;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class DeclarationCourierStatusRepository:IRepository<DeclarationCourierStatus>
   {
        
		public List<DeclarationCourierStatus> GetMulti(EntityKeyFields entityKeys)
        {

            DeclarationKeys declarationKeys = entityKeys as DeclarationKeys;

            return (from a in context.DeclarationCourierStatuses
                    where a.DeclarationId == declarationKeys.Id
                    select a).ToList();
        }

        public List<DeclarationCourierStatus> GetDeclarationsById(List<string> declarationIds)
        {

            List<DeclarationCourierStatus> declarations = (from a in context.DeclarationCourierStatuses
                                              where declarationIds.Contains(a.DeclarationId)
                                              select a).ToList();

            return declarations;

        }

        public int Lock_forUpdateNOWAIT(string declarationIds)
        {

            var succ = (context as DbContext).FirstOrDefaultFUNOWAITWhere<DeclarationCourierStatus>(rec => rec.DeclarationId == declarationIds);
            //var oracleTransaction =Transaction.Current as OracleTransaction;
            //context.Database.

            return succ;
        }

    }

}
   