
   
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
   public class AccountingEntityDetails : AccountingEntity, ICloseTable<AccountingEntity, AccountingEntityDetails>
   {
       public List<AccountingEntityDetails> GetAll()
       {
		    var all = new List<AccountingEntityDetails>();  
            all.Add(new AccountingEntityDetails()
            {    
                Code = "1", 
                LocalName = "פקודת יומן", 
                EnglishName = "Journal", 
			});
			 
            all.Add(new AccountingEntityDetails()
            {    
                Code = "2", 
                LocalName = "חשבונית לקוח", 
                EnglishName = "ARInvoice", 
			});
			 
            all.Add(new AccountingEntityDetails()
            {    
                Code = "3", 
                LocalName = "קבלה לקוח", 
                EnglishName = "ARPayment", 
			});
			 
            all.Add(new AccountingEntityDetails()
            {    
                Code = "4", 
                LocalName = "חשבונית ספק", 
                EnglishName = "APInvoice", 
			});
			 
            all.Add(new AccountingEntityDetails()
            {    
                Code = "5", 
                LocalName = "תשלום לספק", 
                EnglishName = "APPayment", 
			});
			 
            all.Add(new AccountingEntityDetails()
            {    
                Code = "6", 
                LocalName = "הפקדת המחאות", 
                EnglishName = "Cheque Deposit", 
			});
			 
            all.Add(new AccountingEntityDetails()
            {    
                Code = "7", 
                LocalName = "הפקדת מזומן", 
                EnglishName = "Cash Deposit", 
			});
			 
            all.Add(new AccountingEntityDetails()
            {    
                Code = "8", 
                LocalName = "שערוך", 
                EnglishName = "Revaluation", 
			});
			 
            all.Add(new AccountingEntityDetails()
            {    
                Code = "9", 
                LocalName = "מערכת המחאות", 
                EnglishName = "Payment Cheque", 
			});
			 
            all.Add(new AccountingEntityDetails()
            {    
                Code = "10", 
                LocalName = "התאמה", 
                EnglishName = "Adjustment", 
			});
			 
            all.Add(new AccountingEntityDetails()
            {    
                Code = "11", 
                LocalName = "העברת שנה", 
                EnglishName = "Year Transfer", 
			});
			 
            all.Add(new AccountingEntityDetails()
            {    
                Code = 12, 
                LocalName = "התאמת בנק", 
                EnglishName = "Bank Adjustment", 
			});
			
            return all;
       }

	    public void MapPoco(AccountingEntity newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.LocalName = this.LocalName;  
		    newPoco.EnglishName = this.EnglishName;   
        }

		public string GetSearchFields(AccountingEntity rec)
        {   
           return String.Concat(rec.Code,",",rec.LocalName,",",rec.EnglishName,",");
        }
   }
}

