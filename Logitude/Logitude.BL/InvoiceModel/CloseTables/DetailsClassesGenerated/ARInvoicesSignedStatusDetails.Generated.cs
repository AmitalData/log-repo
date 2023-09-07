

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
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.InvoiceModel.EntityPMs; 
using Simplog.Data.InvoiceModel;

namespace Logitude.BL.InvoiceModel
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
                EnglishName = "No signed", 
                SearchFields = "", 
			});
			 
            all.Add(new ARInvoicesSignedStatusDetails()
            {    
                Code = "1", 
                LocalName = "נחתמה וטרם נשלחה", 
                EnglishName = "Signed and not sent", 
                SearchFields = "", 
			});
			 
            all.Add(new ARInvoicesSignedStatusDetails()
            {    
                Code = "2", 
                LocalName = "חתימה נכשלה", 
                EnglishName = "Signature faild", 
                SearchFields = "", 
			});
			 
            all.Add(new ARInvoicesSignedStatusDetails()
            {    
                Code = "3", 
                LocalName = "נחתמה ונשלחה במייל", 
                EnglishName = "Signed and sent by email", 
                SearchFields = "", 
			});
			 
            all.Add(new ARInvoicesSignedStatusDetails()
            {    
                Code = "4", 
                LocalName = "נחתמה והשליחה במייל לא הצליחה", 
                EnglishName = "Signed, email was not successful", 
                SearchFields = "", 
			});
			
            return all;
       }

	    public void MapPoco(ARInvoicesSignedStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.LocalName = this.LocalName;  
		    newPoco.EnglishName = this.EnglishName;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(ARInvoicesSignedStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.LocalName,",",rec.EnglishName,",");
        }
   }
}

