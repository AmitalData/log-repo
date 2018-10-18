

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
   public class LoginPolicyDetails : LoginPolicy, ICloseTable<LoginPolicy, LoginPolicyDetails>
   {
       public List<LoginPolicyDetails> GetAll()
       {
		    var all = new List<LoginPolicyDetails>();  
            all.Add(new LoginPolicyDetails()
            {    
                Code = "COMPIP", 
                SearchFields = "COMPIP,Company IPs only", 
                Name = "Company IPs only", 
			});
			 
            all.Add(new LoginPolicyDetails()
            {    
                Code = "DISABLED", 
                SearchFields = "DISABLED,Disabled", 
                Name = "Disabled", 
			});
			 
            all.Add(new LoginPolicyDetails()
            {    
                Code = "ENABLED", 
                SearchFields = "ENABLED,Enabled", 
                Name = "Enabled", 
			});
			 
            all.Add(new LoginPolicyDetails()
            {    
                Code = "ENFEXIPO", 
                SearchFields = "ENFEXIPO,Enabled for External IPs only", 
                Name = "Enabled for External IPs only", 
			});
			 
            all.Add(new LoginPolicyDetails()
            {    
                Code = "NOREST", 
                SearchFields = "NOREST,No Restriction", 
                Name = "No Restriction", 
			});
			 
            all.Add(new LoginPolicyDetails()
            {    
                Code = "TFAUTH", 
                SearchFields = "TFAUTH,Two Factor Authentication", 
                Name = "Two Factor Authentication", 
			});
			
            return all;
       }

	    public void MapPoco(LoginPolicy newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(LoginPolicy rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

