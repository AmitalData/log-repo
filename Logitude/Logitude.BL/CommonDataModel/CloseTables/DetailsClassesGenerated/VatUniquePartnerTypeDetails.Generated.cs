

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
   public class VatUniquePartnerTypeDetails : VatUniquePartnerType, ICloseTable<VatUniquePartnerType, VatUniquePartnerTypeDetails>
   {
       public List<VatUniquePartnerTypeDetails> GetAll()
       {
		    var all = new List<VatUniquePartnerTypeDetails>();  
            all.Add(new VatUniquePartnerTypeDetails()
            {    
                Code = "ALL", 
                Name = "All Customers", 
                SearchFields = "ALL,All Customers", 
			});
			 
            all.Add(new VatUniquePartnerTypeDetails()
            {    
                Code = "CUS", 
                Name = "Active Customers Only", 
                SearchFields = "CUS,Active Customers Only", 
			});
			 
            all.Add(new VatUniquePartnerTypeDetails()
            {    
                Code = "POT", 
                Name = "Potential Customers Only", 
                SearchFields = "POT,Potential Customers Only", 
			});
			
            return all;
       }

	    public void MapPoco(VatUniquePartnerType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(VatUniquePartnerType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

