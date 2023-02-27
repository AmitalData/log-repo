
   
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
   public class OperatorCategoryDetails : OperatorCategory, ICloseTable<OperatorCategory, OperatorCategoryDetails>
   {
       public List<OperatorCategoryDetails> GetAll()
       {
		    var all = new List<OperatorCategoryDetails>();  
            all.Add(new OperatorCategoryDetails()
            {    
                Code = "MTH", 
                Name = "Math", 
                SearchFields = "MTH,Math", 
			});
			 
            all.Add(new OperatorCategoryDetails()
            {    
                Code = "BOL", 
                Name = "Logical", 
                SearchFields = "BOL,Logical", 
			});
			
            return all;
       }

	    public void MapPoco(OperatorCategory newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(OperatorCategory rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

