

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
   public class ConfirmationNumberStatusDetails : ConfirmationNumberStatus, ICloseTable<ConfirmationNumberStatus, ConfirmationNumberStatusDetails>
   {
       public List<ConfirmationNumberStatusDetails> GetAll()
       {
		    var all = new List<ConfirmationNumberStatusDetails>();  
            all.Add(new ConfirmationNumberStatusDetails()
            {    
                Code = "1", 
                Name = "Confirmation number needed", 
                LocalName = "נדרש הקצאה", 
			});
			 
            all.Add(new ConfirmationNumberStatusDetails()
            {    
                Code = "2", 
                Name = "Confirmation number received", 
                LocalName = "הקצאה התקבלה", 
			});
			 
            all.Add(new ConfirmationNumberStatusDetails()
            {    
                Code = "3", 
                Name = "Confirmation number not received", 
                LocalName = "לא התקבלה הקצאה", 
			});
			 
            all.Add(new ConfirmationNumberStatusDetails()
            {    
                Code = "4", 
                Name = "Confirmation number not needed", 
                LocalName = "לא נדרש הקצאה", 
			});
			 
            all.Add(new ConfirmationNumberStatusDetails()
            {    
                Code = "5", 
                Name = "Confirmation number failed", 
                LocalName = "כשלון בקבלת הקצאה", 
			});
			
            return all;
       }

	    public void MapPoco(ConfirmationNumberStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(ConfirmationNumberStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",",rec.LocalName,",");
        }
   }
}

