
   
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;  
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.CloseTablesClasses;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs; 
using Logitude.Workflow.Data;

namespace Logitude.Workflow.BL.CLoseTable
{
   public class WorkFlowTriggerTypeDetails : WorkFlowTriggerType, ICloseTable<WorkFlowTriggerType, WorkFlowTriggerTypeDetails>
   {
       public List<WorkFlowTriggerTypeDetails> GetAll()
       {
		    var all = new List<WorkFlowTriggerTypeDetails>();  
            all.Add(new WorkFlowTriggerTypeDetails()
            {    
                SearchFields = "Record Triggered, RecordTriggered", 
                Code = "RecordTriggered", 
                Name = "Record Triggered", 
			});
			 
            all.Add(new WorkFlowTriggerTypeDetails()
            {    
                SearchFields = "External Event Triggered,EventTriggered", 
                Code = "EventTriggered", 
                Name = "External Event Triggered", 
			});
			
            return all;
       }

	    public void MapPoco(WorkFlowTriggerType newPoco)
        {   
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(WorkFlowTriggerType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

