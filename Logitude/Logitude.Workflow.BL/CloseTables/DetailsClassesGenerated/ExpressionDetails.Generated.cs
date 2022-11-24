
   
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
   public class ExpressionDetails : Expression, ICloseTable<Expression, ExpressionDetails>
   {
       public List<ExpressionDetails> GetAll()
       {
		    var all = new List<ExpressionDetails>();  
            all.Add(new ExpressionDetails()
            {    
                Code = "DATEDIF", 
                Name = "DATEDIF", 
                SearchFields = "DATEDIF", 
                Body = "(start_date,end_date,unit)", 
                Description = "Calculates the number of days, months, or years between two dates", 
                CategoryCode = "DTE", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "MIN", 
                Name = "MIN", 
                SearchFields = "MIN", 
                Body = "(number1,number2)", 
                Description = "Return the smallest value from the numbers supplied", 
                CategoryCode = "MTH", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "MAX", 
                Name = "MAX", 
                SearchFields = "MAX", 
                Body = "(number1,number2)", 
                Description = "Return the biggest value from the numbers supplied", 
                CategoryCode = "MTH", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "COUNT", 
                Name = "COUNT", 
                SearchFields = "COUNT", 
                Body = "(list)", 
                Description = "Return the count of list elements", 
                CategoryCode = "MTH", 
			});
			
            return all;
       }

	    public void MapPoco(Expression newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Body = this.Body;  
		    newPoco.Description = this.Description;  
		    newPoco.CategoryCode = this.CategoryCode;   
        }

		public string GetSearchFields(Expression rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",",rec.Body,",",rec.Description,",",rec.CategoryCode,",");
        }
   }
}

