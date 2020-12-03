
   
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
   public class CustomerRoleTypeDetails : CustomerRoleType, ICloseTable<CustomerRoleType, CustomerRoleTypeDetails>
   {
       public List<CustomerRoleTypeDetails> GetAll()
       {
		    var all = new List<CustomerRoleTypeDetails>();  
            all.Add(new CustomerRoleTypeDetails()
            {    
                Code = "1", 
                SearchFields = "1,סוכן,,", 
                Inactive = false, 
                LocalName = "סוכן", 
			});
			 
            all.Add(new CustomerRoleTypeDetails()
            {    
                Code = "4", 
                SearchFields = "4,יבואן,,", 
                Inactive = false, 
                LocalName = "יבואן", 
			});
			
            return all;
       }

	    public void MapPoco(CustomerRoleType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(CustomerRoleType rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

