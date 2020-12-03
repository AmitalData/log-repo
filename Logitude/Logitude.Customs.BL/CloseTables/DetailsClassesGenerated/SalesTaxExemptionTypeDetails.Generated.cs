
   
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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL
{
   public class SalesTaxExemptionTypeDetails : SalesTaxExemptionType, ICloseTable<SalesTaxExemptionType, SalesTaxExemptionTypeDetails>
   {
       public List<SalesTaxExemptionTypeDetails> GetAll()
       {
		    var all = new List<SalesTaxExemptionTypeDetails>();  
            all.Add(new SalesTaxExemptionTypeDetails()
            {    
                Code = "6", 
                SearchFields = "6,קוד 6 - פטור כללי ממס קניה", 
                Inactive = false, 
                LocalName = "קוד 6 - פטור כללי ממס קניה", 
			});
			 
            all.Add(new SalesTaxExemptionTypeDetails()
            {    
                Code = "7", 
                SearchFields = "7,פטור ממס קניה לעוסק רשום", 
                Inactive = false, 
                LocalName = "פטור ממס קניה לעוסק רשום", 
			});
			
            return all;
       }

	    public void MapPoco(SalesTaxExemptionType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(SalesTaxExemptionType rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

