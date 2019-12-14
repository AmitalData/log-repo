
   
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
   public class InterestEntityTypeDetails : InterestEntityType, ICloseTable<InterestEntityType, InterestEntityTypeDetails>
   {
       public List<InterestEntityTypeDetails> GetAll()
       {
		    var all = new List<InterestEntityTypeDetails>();  
            all.Add(new InterestEntityTypeDetails()
            {    
                Code = "1", 
                EnglishName = "ARInvoice", 
                LocalName = "חשבונית", 
                SearchFields = "1,חשבונית,ARInvoice", 
			});
			 
            all.Add(new InterestEntityTypeDetails()
            {    
                Code = "2", 
                EnglishName = "ARPayment", 
                SearchFields = "2,ARPayment,קבלה", 
                LocalName = "קבלה", 
			});
			 
            all.Add(new InterestEntityTypeDetails()
            {    
                Code = "3", 
                EnglishName = "Journal", 
                SearchFields = "3,Journal,פקודת יומן", 
                LocalName = "פקודת יומן", 
			});
			 
            all.Add(new InterestEntityTypeDetails()
            {    
                Code = "4", 
                EnglishName = "Open Balance", 
                SearchFields = "4,Open Balance,יתרת פתיחה", 
                LocalName = "יתרת פתיחה", 
			});
			
            return all;
       }

	    public void MapPoco(InterestEntityType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.EnglishName = this.EnglishName;  
		    newPoco.LocalName = this.LocalName;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(InterestEntityType rec)
        {   
           return String.Concat(rec.Code,",",rec.EnglishName,",",rec.LocalName,",");
        }
   }
}

