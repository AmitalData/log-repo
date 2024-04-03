
   
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
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.BL.EntityPMs; 
using Logitude.DashboardModule.Data;

namespace Logitude.DashboardModule.BL
{
   public class PermissionLevelDetails : PermissionLevel, ICloseTable<PermissionLevel, PermissionLevelDetails>
   {
       public List<PermissionLevelDetails> GetAll()
       {
		    var all = new List<PermissionLevelDetails>();  
            all.Add(new PermissionLevelDetails()
            {    
                Code = "ONM", 
                Name = "Only Me", 
                SearchFields = "ONM,Only Me", 
			});
			 
            all.Add(new PermissionLevelDetails()
            {    
                Code = "ALL", 
                Name = "All Users", 
                SearchFields = "ALL, All Users", 
			});
			 
            all.Add(new PermissionLevelDetails()
            {    
                Code = "SPF", 
                Name = "Specific Users", 
                SearchFields = "SPF,Specific Users", 
			});
			 
            all.Add(new PermissionLevelDetails()
            {    
                Code = "PUB", 
                Name = "Public", 
                SearchFields = "PUB,Public", 
			});
			
            return all;
       }

	    public void MapPoco(PermissionLevel newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(PermissionLevel rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

