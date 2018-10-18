

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
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.GlobalModel.EntityPMs; 
using Simplog.Global.Data.GlobalModel;

namespace Logitude.BL.GlobalModel
{
   public class RecurringPeriodDetails : RecurringPeriod, ICloseTable<RecurringPeriod, RecurringPeriodDetails>
   {
       public List<RecurringPeriodDetails> GetAll()
       {
		    var all = new List<RecurringPeriodDetails>();  
            all.Add(new RecurringPeriodDetails()
            {    
                Code = "MO", 
                SearchFields = "MO,Monthly", 
                Name = "Monthly", 
			});
			 
            all.Add(new RecurringPeriodDetails()
            {    
                Code = "QU", 
                SearchFields = "QU,Quarterly", 
                Name = "Quarterly", 
			});
			 
            all.Add(new RecurringPeriodDetails()
            {    
                Code = "YE", 
                SearchFields = "YE,Yearly", 
                Name = "Yearly", 
			});
			
            return all;
       }

	    public void MapPoco(RecurringPeriod newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(RecurringPeriod rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

