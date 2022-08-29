using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Simplog.Server.Infrastructure;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data; 
using Logitude.Workflow.Data.EntityMapping;

namespace Logitude.Workflow.Data
{

    public interface IWorkflowContext : IContext
    {
   
       	 IDbSet<WorkFlow> WorkFlows { get; }
		 IDbSet<WorkFlowInstance> WorkFlowInstances { get; }
		 IDbSet<WorkFlowInstanceStatus> WorkFlowInstanceStatuses { get; }
		 IDbSet<WorkFlowStatus> WorkFlowStatuses { get; }
	 
         void SetAsModified(object entity);
         void DetectChanges();
         int SaveChanges();

    }
}