
   
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
   public class ChartOfAccountsTypeDetails : ChartOfAccountsType, ICloseTable<ChartOfAccountsType, ChartOfAccountsTypeDetails>
   {
       public List<ChartOfAccountsTypeDetails> GetAll()
       {
		    var all = new List<ChartOfAccountsTypeDetails>();  
            all.Add(new ChartOfAccountsTypeDetails()
            {    
                Code = "1", 
                SearchFields = "1,Revenuesהכנסות", 
                Inactive = false, 
                LocalName = "הכנסות", 
                EnglishName = "Revenues", 
			});
			 
            all.Add(new ChartOfAccountsTypeDetails()
            {    
                Code = "2", 
                SearchFields = "2,Expensesהוצאות", 
                Inactive = false, 
                LocalName = "הוצאות", 
                EnglishName = "Expenses", 
			});
			 
            all.Add(new ChartOfAccountsTypeDetails()
            {    
                Code = "3", 
                SearchFields = "3,Customersלקוחות", 
                Inactive = false, 
                LocalName = "לקוחות", 
                EnglishName = "Customers", 
			});
			 
            all.Add(new ChartOfAccountsTypeDetails()
            {    
                Code = "4", 
                SearchFields = "4,Vendorsספקים", 
                Inactive = false, 
                LocalName = "ספקים", 
                EnglishName = "Vendors", 
			});
			 
            all.Add(new ChartOfAccountsTypeDetails()
            {    
                Code = "5", 
                SearchFields = "5,Banksבנקים", 
                Inactive = false, 
                LocalName = "בנקים", 
                EnglishName = "Banks", 
			});
			 
            all.Add(new ChartOfAccountsTypeDetails()
            {    
                Code = "6", 
                SearchFields = "6,Workersעובדים", 
                Inactive = false, 
                LocalName = "עובדים", 
                EnglishName = "Workers", 
			});
			 
            all.Add(new ChartOfAccountsTypeDetails()
            {    
                Code = "7", 
                SearchFields = "7,Debtors And Creditorsחו''זים", 
                Inactive = false, 
                LocalName = "חו''זים", 
                EnglishName = "Debtors And Creditors", 
			});
			
            return all;
       }

	    public void MapPoco(ChartOfAccountsType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;  
		    newPoco.EnglishName = this.EnglishName;   
        }

		public string GetSearchFields(ChartOfAccountsType rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",",rec.EnglishName,",");
        }
   }
}

