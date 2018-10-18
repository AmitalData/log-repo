 
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
   public partial class DeclarationTaxRepository:IRepository<DeclarationTax>
   {



       public List<DeclarationTax> GetMulti(EntityKeyFields entityKeys)
        {

            DeclarationKeys declarationKeys = entityKeys as DeclarationKeys;

            return (from a in context.DeclarationTaxes
                    where a.DeclarationId == declarationKeys.Id
                    select a).ToList();
        }

       public IQueryable<DeclarationTax> GetDeclarationTaxesForDeclarationId(string declarationId,int tenant)
       {
           return from a in context.DeclarationTaxes.Include("ParagraphType")
                  where a.DeclarationId==declarationId && a.Tenant == tenant 
                  select a;
       }



       public void FastDeleteMulti(DeclarationKeys entityKeyFields)
       {

           (context as DbContextBase)
               .DeleteWhere<DeclarationTax>(rec => rec.DeclarationId == entityKeyFields.Id);
       }
   }

}
   