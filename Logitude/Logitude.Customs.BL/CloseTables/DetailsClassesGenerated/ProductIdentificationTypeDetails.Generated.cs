
   
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
   public class ProductIdentificationTypeDetails : ProductIdentificationType, ICloseTable<ProductIdentificationType, ProductIdentificationTypeDetails>
   {
       public List<ProductIdentificationTypeDetails> GetAll()
       {
		    var all = new List<ProductIdentificationTypeDetails>();  
            all.Add(new ProductIdentificationTypeDetails()
            {    
                Code = "MN", 
                SearchFields = "MN,דגם יצרן", 
                Inactive = false, 
                LocalName = "דגם יצרן", 
			});
			 
            all.Add(new ProductIdentificationTypeDetails()
            {    
                Code = "SS", 
                SearchFields = "SS,מספר קטלוגי", 
                Inactive = false, 
                LocalName = "מספר קטלוגי", 
			});
			
            return all;
       }

	    public void MapPoco(ProductIdentificationType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(ProductIdentificationType rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

