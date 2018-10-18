
   
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
   public class ReconcileExternalPageStatusDetails : ReconcileExternalPageStatus, ICloseTable<ReconcileExternalPageStatus, ReconcileExternalPageStatusDetails>
   {
       public List<ReconcileExternalPageStatusDetails> GetAll()
       {
		    var all = new List<ReconcileExternalPageStatusDetails>();  
            all.Add(new ReconcileExternalPageStatusDetails()
            {    
                Code = "1", 
                SearchFields = "1,Draft,טיוטה,", 
                Inactive = false, 
                LocalName = "טיוטה", 
                EnglishName = "Draft", 
			});
			 
            all.Add(new ReconcileExternalPageStatusDetails()
            {    
                Code = "2", 
                SearchFields = "2,Approved,מאושר,", 
                Inactive = false, 
                LocalName = "מאושר", 
                EnglishName = "Approved", 
			});
			 
            all.Add(new ReconcileExternalPageStatusDetails()
            {    
                Code = "3", 
                SearchFields = "3,Cancelled,מבוטל,", 
                Inactive = false, 
                LocalName = "מבוטל", 
                EnglishName = "Cancelled", 
			});
			
            return all;
       }

	    public void MapPoco(ReconcileExternalPageStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;  
		    newPoco.EnglishName = this.EnglishName;   
        }

		public string GetSearchFields(ReconcileExternalPageStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",",rec.EnglishName,",");
        }
   }
}

