 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.Repsitories
{
   public partial class DigitalTextCodeRepository:IRepository<DigitalTextCode>
   {
        
		public List<DigitalTextCode> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

   }

}
   