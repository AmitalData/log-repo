
   
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
   public class WorkFlowVersionStatusDetails : WorkFlowVersionStatus, ICloseTable<WorkFlowVersionStatus, WorkFlowVersionStatusDetails>
   {
       public List<WorkFlowVersionStatusDetails> GetAll()
       {
		    var all = new List<WorkFlowVersionStatusDetails>();  
            all.Add(new WorkFlowVersionStatusDetails()
            {    
                Code = "ACVE", 
                Name = "Active", 
                SearchFields = "ACVE,Active", 
			});
			 
            all.Add(new WorkFlowVersionStatusDetails()
            {    
                Code = "INVE", 
                Name = "Inactive", 
                SearchFields = "INVE,Inactive", 
			});
			 
            all.Add(new WorkFlowVersionStatusDetails()
            {    
                Name = "Draft", 
                SearchFields = "Draft,DRFT", 
                Code = "DRFT", 
			});
			
            return all;
       }

	    public void MapPoco(WorkFlowVersionStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(WorkFlowVersionStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

