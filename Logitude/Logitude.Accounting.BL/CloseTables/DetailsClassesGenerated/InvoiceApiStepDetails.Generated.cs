
   
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
   public class InvoiceApiStepDetails : InvoiceApiStep, ICloseTable<InvoiceApiStep, InvoiceApiStepDetails>
   {
       public List<InvoiceApiStepDetails> GetAll()
       {
		    var all = new List<InvoiceApiStepDetails>();  
            all.Add(new InvoiceApiStepDetails()
            {    
                Code = "1", 
                EnglishName = "Open Session", 
                LocalName = "פתיחת סשן", 
                IsAllowResend = false, 
			});
			 
            all.Add(new InvoiceApiStepDetails()
            {    
                Code = "2", 
                EnglishName = "Close Session", 
                LocalName = "סגירת סשן", 
                IsAllowResend = false, 
			});
			 
            all.Add(new InvoiceApiStepDetails()
            {    
                Code = "3", 
                EnglishName = "Get Invoices List", 
                LocalName = "קבלת רשימת חשבוניות", 
                IsAllowResend = false, 
			});
			 
            all.Add(new InvoiceApiStepDetails()
            {    
                Code = "4", 
                EnglishName = "Get Invoice", 
                LocalName = "קבלת חשבונית", 
                IsAllowResend = false, 
			});
			 
            all.Add(new InvoiceApiStepDetails()
            {    
                Code = "5", 
                EnglishName = "Generate Invoice", 
                LocalName = "הפקת חשבונית", 
                IsAllowResend = false, 
			});
			 
            all.Add(new InvoiceApiStepDetails()
            {    
                Code = "6", 
                EnglishName = "Get Confirmation Number", 
                LocalName = "הקצאה", 
                IsAllowResend = false, 
			});
			 
            all.Add(new InvoiceApiStepDetails()
            {    
                Code = "7", 
                EnglishName = "Approve Invoice", 
                LocalName = "אישור חשבונית", 
                IsAllowResend = false, 
			});
			 
            all.Add(new InvoiceApiStepDetails()
            {    
                Code = "8", 
                EnglishName = "Print Or Send Invoice", 
                LocalName = "הדפסה/שליחה במייל", 
                IsAllowResend = false, 
			});
			
            return all;
       }

	    public void MapPoco(InvoiceApiStep newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.EnglishName = this.EnglishName;  
		    newPoco.LocalName = this.LocalName;  
		    newPoco.IsAllowResend = this.IsAllowResend;   
        }

		public string GetSearchFields(InvoiceApiStep rec)
        {   
           return String.Concat(rec.Code,",",rec.EnglishName,",",rec.LocalName,",",rec.IsAllowResend,",");
        }
   }
}

