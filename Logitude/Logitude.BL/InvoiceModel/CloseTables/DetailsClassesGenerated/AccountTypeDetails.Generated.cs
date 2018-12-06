

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
   public class AccountTypeDetails : AccountType, ICloseTable<AccountType, AccountTypeDetails>
   {
       public List<AccountTypeDetails> GetAll()
       {
		    var all = new List<AccountTypeDetails>();  
            all.Add(new AccountTypeDetails()
            {    
                Code = "AP", 
                Name = "Account Payables", 
                SearchFields = "ap,account payables", 
			});
			 
            all.Add(new AccountTypeDetails()
            {    
                Code = "AR", 
                Name = "Account Receivables", 
                SearchFields = "ar,account receivables", 
			});
			 
            all.Add(new AccountTypeDetails()
            {    
                Code = "BN", 
                Name = "Bank", 
                SearchFields = "bn,bank", 
			});
			 
            all.Add(new AccountTypeDetails()
            {    
                Code = "CC", 
                Name = "Credit Card", 
                SearchFields = "cc,credit card", 
			});
			 
            all.Add(new AccountTypeDetails()
            {    
                Code = "CO", 
                Name = "Cost of goods sold", 
                SearchFields = "co,cost of goods sold", 
			});
			 
            all.Add(new AccountTypeDetails()
            {    
                Code = "EX", 
                Name = "Expense", 
                SearchFields = "EX,Expense", 
			});
			 
            all.Add(new AccountTypeDetails()
            {    
                Code = "IN", 
                Name = "Income", 
                SearchFields = "in,income", 
			});
			 
            all.Add(new AccountTypeDetails()
            {    
                Code = "UF", 
                Name = "undeposited funds", 
                SearchFields = "uf,undeposited funds", 
			});
			
            return all;
       }

	    public void MapPoco(AccountType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(AccountType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

