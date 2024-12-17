

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
   public class FeaturePackageTypeDetails : FeaturePackageType, ICloseTable<FeaturePackageType, FeaturePackageTypeDetails>
   {
       public List<FeaturePackageTypeDetails> GetAll()
       {
		    var all = new List<FeaturePackageTypeDetails>();  
            all.Add(new FeaturePackageTypeDetails()
            {    
                Code = "AD", 
                SearchFields = "AD,Add-on", 
                Name = "Add-on", 
			});
			 
            all.Add(new FeaturePackageTypeDetails()
            {    
                Code = "BS", 
                SearchFields = "BS,Base", 
                Name = "Base", 
			});
			 
            all.Add(new FeaturePackageTypeDetails()
            {    
                Code = "PK", 
                SearchFields = "PK,Package", 
                Name = "Package", 
			});
			
            return all;
       }

	    public void MapPoco(FeaturePackageType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(FeaturePackageType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

