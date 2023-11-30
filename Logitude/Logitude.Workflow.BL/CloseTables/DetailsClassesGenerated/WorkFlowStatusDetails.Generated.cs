
   
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
   public class WorkFlowStatusDetails : WorkFlowStatus, ICloseTable<WorkFlowStatus, WorkFlowStatusDetails>
   {
       public List<WorkFlowStatusDetails> GetAll()
       {
		    var all = new List<WorkFlowStatusDetails>();  
            all.Add(new WorkFlowStatusDetails()
            {    
                Code = "PUED", 
                Name = "Published", 
                SearchFields = "PUED,Published", 
			});
			 
            all.Add(new WorkFlowStatusDetails()
            {    
                Code = "PAED", 
                Name = "Paused", 
                SearchFields = "PAED,Paused", 
			});
			 
            all.Add(new WorkFlowStatusDetails()
            {    
                Code = "DRFT", 
                Name = "Draft", 
                SearchFields = "DRFT,Draft", 
			});
			 
            all.Add(new WorkFlowStatusDetails()
            {    
                Code = "ACVE", 
                Name = "Active", 
                SearchFields = "ACVE,Active", 
			});
			 
            all.Add(new WorkFlowStatusDetails()
            {    
                Code = "INVE", 
                Name = "Inactive", 
                SearchFields = "INVE,Inactive", 
			});
			
            return all;
       }

	    public void MapPoco(WorkFlowStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(WorkFlowStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

