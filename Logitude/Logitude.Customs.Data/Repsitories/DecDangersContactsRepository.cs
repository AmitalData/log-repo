 
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
   public partial class DecDangersContactRepository:IRepository<DecDangersContact>
   {
        
		public List<DecDangersContact> GetMulti(EntityKeyFields entityKeys)
        {

            DeclarationKeys declarationKeys = entityKeys as DeclarationKeys;

            return (from a in context.DecDangersContacts
                    where a.DeclarationId == declarationKeys.Id
                    select a).ToList();
        }

   }

}
   