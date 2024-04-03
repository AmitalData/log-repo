
   
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
   public class WorkFlowInstanceStatusDetails : WorkFlowInstanceStatus, ICloseTable<WorkFlowInstanceStatus, WorkFlowInstanceStatusDetails>
   {
       public List<WorkFlowInstanceStatusDetails> GetAll()
       {
		    var all = new List<WorkFlowInstanceStatusDetails>();  
            all.Add(new WorkFlowInstanceStatusDetails()
            {    
                Code = "ACVE", 
                Name = "Active", 
                SearchFields = "ACVE,Active", 
			});
			 
            all.Add(new WorkFlowInstanceStatusDetails()
            {    
                Code = "COED", 
                Name = "Completed", 
                SearchFields = "COED,Completed", 
			});
			 
            all.Add(new WorkFlowInstanceStatusDetails()
            {    
                Name = "Failed", 
                Code = "FAED", 
                SearchFields = "FAED,Failed", 
			});
			
            return all;
       }

	    public void MapPoco(WorkFlowInstanceStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(WorkFlowInstanceStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

