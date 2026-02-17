

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
   public class VatMandatoryTypeDetails : VatMandatoryType, ICloseTable<VatMandatoryType, VatMandatoryTypeDetails>
   {
       public List<VatMandatoryTypeDetails> GetAll()
       {
		    var all = new List<VatMandatoryTypeDetails>();  
            all.Add(new VatMandatoryTypeDetails()
            {    
                Code = "MSC", 
                Name = "Mandatory for a specific country", 
                SearchFields = "MSC,Mandatory for a specific country", 
			});
			 
            all.Add(new VatMandatoryTypeDetails()
            {    
                Code = "MFA", 
                Name = "Mandatory for all countries", 
                SearchFields = "MFA,Mandatory for all countries", 
			});
			 
            all.Add(new VatMandatoryTypeDetails()
            {    
                Code = "MNT", 
                Name = "Not Mandatory", 
                SearchFields = "MNT,Not Mandatory", 
			});
			
            return all;
       }

	    public void MapPoco(VatMandatoryType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(VatMandatoryType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

