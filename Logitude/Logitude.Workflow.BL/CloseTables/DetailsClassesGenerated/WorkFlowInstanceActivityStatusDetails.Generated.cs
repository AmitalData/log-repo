
   
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
   public class WorkFlowInstanceActivityStatusDetails : WorkFlowInstanceActivityStatus, ICloseTable<WorkFlowInstanceActivityStatus, WorkFlowInstanceActivityStatusDetails>
   {
       public List<WorkFlowInstanceActivityStatusDetails> GetAll()
       {
		    var all = new List<WorkFlowInstanceActivityStatusDetails>();  
            all.Add(new WorkFlowInstanceActivityStatusDetails()
            {    
                Code = "COED", 
                Name = "Completed", 
                SearchFields = "COED,Completed", 
			});
			 
            all.Add(new WorkFlowInstanceActivityStatusDetails()
            {    
                Code = "STED", 
                Name = "Started", 
                SearchFields = "STED,Started", 
			});
			 
            all.Add(new WorkFlowInstanceActivityStatusDetails()
            {    
                Code = "FAED", 
                Name = "Failed", 
                SearchFields = "FAED,Failed", 
			});
			
            return all;
       }

	    public void MapPoco(WorkFlowInstanceActivityStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(WorkFlowInstanceActivityStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

