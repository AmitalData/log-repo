 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Accounting.BL.EntityQueryServices
{ 
   public partial class GLAccountCounterQueryService
   {		 
		public  GLAccountCounterPM GetByPrefix(string prefix, int tenant)
        {
            GLAccountCounter counter = repository.GetByPrefix(prefix, tenant);

            if (counter != null)
            {
                EntityPM = new GLAccountCounterPM();
                mapping.CustomPOCOToPM(EntityPM, counter);
                mapping.POCOToPM(EntityPM, counter);
            }

            return EntityPM;
        }

    }
   
}
	 