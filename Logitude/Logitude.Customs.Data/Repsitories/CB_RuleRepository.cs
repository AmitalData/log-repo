 
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
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System.Data.Common;
using Simplog.Data.InfrastructureModel;
using Logitude.Customs.Data.EntityLists;
using System.Data.SqlClient;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CB_RuleRepository:IRepository<CB_Rule>
   {
        
		public List<CB_Rule> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

     
    }

}
   