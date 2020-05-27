

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
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityPMs; 
using Simplog.Data.ShipmentsModel;

namespace Logitude.BL.ShipmentsModel
{
   public class CustomsTransferTypeDetails : CustomsTransferType, ICloseTable<CustomsTransferType, CustomsTransferTypeDetails>
   {
       public List<CustomsTransferTypeDetails> GetAll()
       {
		    var all = new List<CustomsTransferTypeDetails>();  
            all.Add(new CustomsTransferTypeDetails()
            {    
                Code = "AMOS", 
                Name = "AMANAC Ocean Shipments", 
                SearchFields = "AMOS,AMANAC Ocean Shipments", 
			});
			 
            all.Add(new CustomsTransferTypeDetails()
            {    
                Code = "AMAS", 
                Name = "AMANAC Air Shipments", 
                SearchFields = "AMAS,AMANAC Air Shipments", 
			});
			
            return all;
       }

	    public void MapPoco(CustomsTransferType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(CustomsTransferType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

