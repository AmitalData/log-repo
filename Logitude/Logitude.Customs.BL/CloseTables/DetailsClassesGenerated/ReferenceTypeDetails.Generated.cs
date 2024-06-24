
   
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
   public class ReferenceTypeDetails : ReferenceType, ICloseTable<ReferenceType, ReferenceTypeDetails>
   {
       public List<ReferenceTypeDetails> GetAll()
       {
		    var all = new List<ReferenceTypeDetails>();  
            all.Add(new ReferenceTypeDetails()
            {    
                Code = "ORD", 
                SearchFields = "ORD,הזמנת לקוח", 
                Inactive = false, 
                LocalName = "הזמנת לקוח", 
                EnglishName = "Order", 
			});
			 
            all.Add(new ReferenceTypeDetails()
            {    
                Code = "SHP", 
                SearchFields = "SHP,תיק יבואן", 
                Inactive = false, 
                LocalName = "תיק יבואן", 
                EnglishName = "Shipment", 
			});
			
            return all;
       }

	    public void MapPoco(ReferenceType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;  
		    newPoco.EnglishName = this.EnglishName;   
        }

		public string GetSearchFields(ReferenceType rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",",rec.EnglishName,",");
        }
   }
}

