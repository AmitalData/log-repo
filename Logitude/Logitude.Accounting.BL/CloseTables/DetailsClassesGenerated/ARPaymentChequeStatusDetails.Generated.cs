
   
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
   public class ARPaymentChequeStatusDetails : ARPaymentChequeStatus, ICloseTable<ARPaymentChequeStatus, ARPaymentChequeStatusDetails>
   {
       public List<ARPaymentChequeStatusDetails> GetAll()
       {
		    var all = new List<ARPaymentChequeStatusDetails>();  
            all.Add(new ARPaymentChequeStatusDetails()
            {    
                Code = "1", 
                SearchFields = "בקופה,In Cashbook,1,", 
                LocalName = "בקופה", 
                Inactive = false, 
                EnglishName = "In Cashbook", 
			});
			 
            all.Add(new ARPaymentChequeStatusDetails()
            {    
                Code = "2", 
                SearchFields = "משמרת,In Bank,2,", 
                LocalName = "משמרת", 
                Inactive = false, 
                EnglishName = "In Bank", 
			});
			 
            all.Add(new ARPaymentChequeStatusDetails()
            {    
                Code = "3", 
                LocalName = "הופקד- טרם נפרע", 
                EnglishName = "In Bank Account", 
                SearchFields = "3,In Bank Account,הופקד- טרם נפרע", 
                Inactive = false, 
			});
			 
            all.Add(new ARPaymentChequeStatusDetails()
            {    
                Code = "4", 
                SearchFields = "בקופה - הוחזר מהבנק,Returned From Bank,4,", 
                LocalName = "בקופה - הוחזר מהבנק", 
                Inactive = false, 
                EnglishName = "Returned From Bank", 
			});
			 
            all.Add(new ARPaymentChequeStatusDetails()
            {    
                Code = "5", 
                SearchFields = "הוחזר ללקוח,Returned To Customer,5,", 
                LocalName = "הוחזר ללקוח", 
                Inactive = false, 
                EnglishName = "Returned To Customer", 
			});
			 
            all.Add(new ARPaymentChequeStatusDetails()
            {    
                Code = "6", 
                LocalName = "נפרע", 
                EnglishName = "Redeemed", 
                SearchFields = "6,Redeemed,נפרע", 
                Inactive = false, 
			});
			
            return all;
       }

	    public void MapPoco(ARPaymentChequeStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.LocalName = this.LocalName;  
		    newPoco.Inactive = this.Inactive;  
		    newPoco.EnglishName = this.EnglishName;   
        }

		public string GetSearchFields(ARPaymentChequeStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.LocalName,",",rec.Inactive,",",rec.EnglishName,",");
        }
   }
}

