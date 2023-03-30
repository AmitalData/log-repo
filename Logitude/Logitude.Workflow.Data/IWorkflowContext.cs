using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Simplog.Server.Infrastructure;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data; 
using Logitude.Workflow.Data.EntityMapping;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.Workflow.Data
{

    public interface IWorkflowContext : IContext
    {
   
       	 IDbSet<Expression> Expressions { get; }
		 IDbSet<ExpressionCategory> ExpressionCategories { get; }
		 IDbSet<Operator> Operators { get; }
		 IDbSet<OperatorCategory> OperatorCategories { get; }
		 IDbSet<ServiceProviderSubscription> ServiceProviderSubscriptions { get; }
		 IDbSet<Task> Tasks { get; }
		 IDbSet<TaskExtended> TasksExtended { get; }
		 IDbSet<TaskPriority> TaskPriorities { get; }
		 IDbSet<TaskStatus> TaskStatuses { get; }
		 IDbSet<TaskType> TaskTypes { get; }
		 IDbSet<WorkFlow> WorkFlows { get; }
		 IDbSet<WorkFlowInstance> WorkFlowInstances { get; }
		 IDbSet<WorkFlowInstanceActivity> WorkFlowInstanceActivities { get; }
		 IDbSet<WorkFlowInstanceActivityStatus> WorkFlowInstanceActivityStatuses { get; }
		 IDbSet<WorkFlowInstanceStatus> WorkFlowInstanceStatuses { get; }
		 IDbSet<WorkFlowInstanceVariable> WorkFlowInstanceVariables { get; }
		 IDbSet<WorkFlowStatus> WorkFlowStatuses { get; }
		 IDbSet<WorkFlowTriggerType> WorkFlowTriggerTypes { get; }
		 IDbSet<WorkFlowVersion> WorkFlowVersions { get; }
		 IDbSet<WorkFlowVersionStatus> WorkFlowVersionStatuses { get; }
         IDbSet<CustomFieldsMainObject> CustomFieldsMainObjects { get; }

		void SetAsModified(object entity);
         void DetectChanges();
         int SaveChanges();

    }
}