 
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
   public partial class UpdateCodeRepository:IRepository<UpdateCode>
   {
        
		public List<UpdateCode> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

   }

}
   