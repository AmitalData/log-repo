 
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
 
namespace Logitude.Accounting.BL.EntityQueryServices
{ 
   public partial class CalculatedChartsOfAccountsLineQueryService 
   {
        public CalculatedChartsOfAccountsLinePM GetSingle(string id, bool getComposition, bool getFromCache)
        {
            EntityKeys = new CalculatedChartsOfAccountsLineKeys() { Id = id };

            return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }



    }

}
	 