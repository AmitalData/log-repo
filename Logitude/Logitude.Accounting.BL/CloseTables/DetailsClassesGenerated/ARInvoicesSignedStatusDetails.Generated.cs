
   
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
   public class ARInvoicesSignedStatusDetails : ARInvoicesSignedStatus, ICloseTable<ARInvoicesSignedStatus, ARInvoicesSignedStatusDetails>
   {
       public List<ARInvoicesSignedStatusDetails> GetAll()
       {
		    var all = new List<ARInvoicesSignedStatusDetails>();  
            all.Add(new ARInvoicesSignedStatusDetails()
            {    
                Code = "0", 
                LocalName = "לא נחתמה", 
                EnglishName = "Not signed", 
			});
			 
            all.Add(new ARInvoicesSignedStatusDetails()
            {    
                Code = "1", 
                LocalName = "נחתמה וטרם נשלחה", 
                EnglishName = "Signed and not yet sent", 
			});
			 
            all.Add(new ARInvoicesSignedStatusDetails()
            {    
                Code = "2", 
                LocalName = "חתימה נכשלה", 
                EnglishName = "Signature failed", 
			});
			 
            all.Add(new ARInvoicesSignedStatusDetails()
            {    
                Code = "3", 
                LocalName = "נחתמה ונשלחה במייל", 
                EnglishName = "Signed and sent by email", 
			});
			 
            all.Add(new ARInvoicesSignedStatusDetails()
            {    
                Code = "4", 
                LocalName = "נחתמה והשליחה במייל לא הצליחה", 
                EnglishName = "signed,  email was not successful", 
			});
			
            return all;
       }

	    public void MapPoco(ARInvoicesSignedStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.LocalName = this.LocalName;  
		    newPoco.EnglishName = this.EnglishName;   
        }

		public string GetSearchFields(ARInvoicesSignedStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.LocalName,",",rec.EnglishName,",");
        }
   }
}

