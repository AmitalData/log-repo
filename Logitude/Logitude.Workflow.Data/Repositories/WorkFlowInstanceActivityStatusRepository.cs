 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Workflow.Data.Repositories
{
   public partial class WorkFlowInstanceActivityStatusRepository:IRepository<WorkFlowInstanceActivityStatus>
   {
        
		public List<WorkFlowInstanceActivityStatus> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

   }

}
   