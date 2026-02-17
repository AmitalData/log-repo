
   
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
   public class AutomaticReconcileDetails : AutomaticReconcile, ICloseTable<AutomaticReconcile, AutomaticReconcileDetails>
   {
       public List<AutomaticReconcileDetails> GetAll()
       {
		    var all = new List<AutomaticReconcileDetails>();  
            all.Add(new AutomaticReconcileDetails()
            {    
                Code = "4", 
                LocalName = "תאריך חשבונאי", 
                SearchFields = "4,Accounting Date,תאריך חשבונאי", 
                Inactive = false, 
                EnglishName = "Accounting Date", 
			});
			 
            all.Add(new AutomaticReconcileDetails()
            {    
                Code = "3", 
                LocalName = "תאריך ערך", 
                SearchFields = "3,Due Date,תאריך ערך", 
                Inactive = false, 
                EnglishName = "Due Date", 
			});
			 
            all.Add(new AutomaticReconcileDetails()
            {    
                Code = "1", 
                LocalName = "סכום פתוח", 
                SearchFields = "1,Open Amount,סכום פתוח", 
                Inactive = false, 
                EnglishName = "Open Amount", 
			});
			 
            all.Add(new AutomaticReconcileDetails()
            {    
                Code = "5", 
                LocalName = "אסמכתא 1", 
                SearchFields = "5,Reference 1,אסמכתא 1", 
                Inactive = false, 
                EnglishName = "Reference 1", 
			});
			 
            all.Add(new AutomaticReconcileDetails()
            {    
                Code = "6", 
                LocalName = "אסמכתא 2", 
                SearchFields = "6,Reference 2,אסמכתא 2", 
                Inactive = false, 
                EnglishName = "Reference 2", 
			});
			 
            all.Add(new AutomaticReconcileDetails()
            {    
                Code = "7", 
                LocalName = "אסמכתא 3", 
                SearchFields = "7,Reference 3,אסמכתא 3", 
                Inactive = false, 
                EnglishName = "Reference 3", 
			});
			 
            all.Add(new AutomaticReconcileDetails()
            {    
                Code = "2", 
                LocalName = "תאריך אסמכתא", 
                SearchFields = "2,Reference Date,תאריך אסמכתא", 
                Inactive = false, 
                EnglishName = "Reference Date", 
			});
			
            return all;
       }

	    public void MapPoco(AutomaticReconcile newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.LocalName = this.LocalName;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.EnglishName = this.EnglishName;   
        }

		public string GetSearchFields(AutomaticReconcile rec)
        {   
           return String.Concat(rec.Code,",",rec.LocalName,",",rec.Inactive,",",rec.EnglishName,",");
        }
   }
}

