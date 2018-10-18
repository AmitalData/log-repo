

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
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs; 
using Simplog.Data.CommonDataModel;

namespace Logitude.BL.CommonDataModel
{
   public class CustomerStatusDetails : CustomerStatus, ICloseTable<CustomerStatus, CustomerStatusDetails>
   {
       public List<CustomerStatusDetails> GetAll()
       {
		    var all = new List<CustomerStatusDetails>();  
            all.Add(new CustomerStatusDetails()
            {    
                Code = "ACT", 
                SearchFields = "ACT,Active", 
                Name = "Active", 
			});
			 
            all.Add(new CustomerStatusDetails()
            {    
                Code = "INA", 
                SearchFields = "INA,Inactive", 
                Name = "Inactive", 
			});
			 
            all.Add(new CustomerStatusDetails()
            {    
                Code = "POT", 
                SearchFields = "POT,Potential", 
                Name = "Potential", 
			});
			 
            all.Add(new CustomerStatusDetails()
            {    
                Code = "WAC", 
                SearchFields = "WAC,Waiting for Activation", 
                Name = "Waiting for Activation", 
			});
			
            return all;
       }

	    public void MapPoco(CustomerStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(CustomerStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

