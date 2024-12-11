

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
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs; 
using Simplog.Data.CommonDataModel;

namespace Logitude.BL.CommonDataModel
{
   public class FeatureAccessLevelDetails : FeatureAccessLevel, ICloseTable<FeatureAccessLevel, FeatureAccessLevelDetails>
   {
       public List<FeatureAccessLevelDetails> GetAll()
       {
		    var all = new List<FeatureAccessLevelDetails>();  
            all.Add(new FeatureAccessLevelDetails()
            {    
                Code = "US", 
                Name = "User", 
                SearchFields = "US,User", 
			});
			 
            all.Add(new FeatureAccessLevelDetails()
            {    
                Code = "PR", 
                Name = "Parent", 
                SearchFields = "PR,Parent", 
			});
			 
            all.Add(new FeatureAccessLevelDetails()
            {    
                Code = "OR", 
                Name = "Organization", 
                SearchFields = "OR,Organization", 
			});
			 
            all.Add(new FeatureAccessLevelDetails()
            {    
                Code = "NO", 
                Name = "None", 
                SearchFields = "NO,None", 
			});
			 
            all.Add(new FeatureAccessLevelDetails()
            {    
                Code = "BU", 
                Name = "Business Unit", 
                SearchFields = "BU,Business Unit", 
			});
			
            return all;
       }

	    public void MapPoco(FeatureAccessLevel newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(FeatureAccessLevel rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

