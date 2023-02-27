
   
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
   public class ExpressionCategoryDetails : ExpressionCategory, ICloseTable<ExpressionCategory, ExpressionCategoryDetails>
   {
       public List<ExpressionCategoryDetails> GetAll()
       {
		    var all = new List<ExpressionCategoryDetails>();  
            all.Add(new ExpressionCategoryDetails()
            {    
                Name = "Math", 
                Code = "MTH", 
                SearchFields = "Math,MTH", 
			});
			 
            all.Add(new ExpressionCategoryDetails()
            {    
                Name = "Date & Time", 
                Code = "DTE", 
                SearchFields = "Date & Time,DTE", 
			});
			 
            all.Add(new ExpressionCategoryDetails()
            {    
                Name = "Text", 
                Code = "TXT", 
                SearchFields = "TXT,Text", 
			});
			
            return all;
       }

	    public void MapPoco(ExpressionCategory newPoco)
        {   
		    newPoco.Name = this.Name;  
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(ExpressionCategory rec)
        {   
           return String.Concat(rec.Name,",",rec.Code,",");
        }
   }
}

