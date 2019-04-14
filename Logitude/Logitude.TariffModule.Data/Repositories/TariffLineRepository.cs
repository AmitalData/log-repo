 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.TariffModule.Data.Repositories
{
   public partial class TariffLineRepository:IRepository<TariffLine>
   {
        
		public List<TariffLine> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

   }

}
   