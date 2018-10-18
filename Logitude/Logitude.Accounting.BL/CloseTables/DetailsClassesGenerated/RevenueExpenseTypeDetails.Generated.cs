
   
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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL
{
   public class RevenueExpenseTypeDetails : RevenueExpenseType, ICloseTable<RevenueExpenseType, RevenueExpenseTypeDetails>
   {
       public List<RevenueExpenseTypeDetails> GetAll()
       {
		    var all = new List<RevenueExpenseTypeDetails>();  
            all.Add(new RevenueExpenseTypeDetails()
            {    
                Code = "2", 
                Inactive = false, 
                LocalName = "הוצאות", 
                EnglishName = "Expense", 
			});
			 
            all.Add(new RevenueExpenseTypeDetails()
            {    
                Code = "3", 
                Inactive = false, 
                LocalName = "אחר", 
                EnglishName = "Other", 
			});
			 
            all.Add(new RevenueExpenseTypeDetails()
            {    
                Code = "1", 
                Inactive = false, 
                LocalName = "הכנסות", 
                EnglishName = "Revenue", 
			});
			
            return all;
       }

	    public void MapPoco(RevenueExpenseType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;  
		    newPoco.EnglishName = this.EnglishName;   
        }

		public string GetSearchFields(RevenueExpenseType rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",",rec.EnglishName,",");
        }
   }
}

