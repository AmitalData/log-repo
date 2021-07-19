
   
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
   public class CustomerIndicationTypeDetails : CustomerIndicationType, ICloseTable<CustomerIndicationType, CustomerIndicationTypeDetails>
   {
       public List<CustomerIndicationTypeDetails> GetAll()
       {
		    var all = new List<CustomerIndicationTypeDetails>();  
            all.Add(new CustomerIndicationTypeDetails()
            {    
                Code = "1", 
                SearchFields = "1,כללי", 
                Inactive = false, 
                LocalName = "כללי", 
			});
			 
            all.Add(new CustomerIndicationTypeDetails()
            {    
                Code = "2", 
                SearchFields = "2,תפעול", 
                Inactive = false, 
                LocalName = "תפעול", 
			});
			
            return all;
       }

	    public void MapPoco(CustomerIndicationType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(CustomerIndicationType rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

