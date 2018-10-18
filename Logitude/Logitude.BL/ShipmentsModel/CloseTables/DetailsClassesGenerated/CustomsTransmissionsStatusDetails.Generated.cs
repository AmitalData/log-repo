

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
   public class CustomsTransmissionsStatusDetails : CustomsTransmissionsStatus, ICloseTable<CustomsTransmissionsStatus, CustomsTransmissionsStatusDetails>
   {
       public List<CustomsTransmissionsStatusDetails> GetAll()
       {
		    var all = new List<CustomsTransmissionsStatusDetails>();  
            all.Add(new CustomsTransmissionsStatusDetails()
            {    
                Code = "ACPT", 
                SearchFields = "ACPT,Accepted", 
                Name = "Accepted", 
			});
			 
            all.Add(new CustomsTransmissionsStatusDetails()
            {    
                Code = "EROR", 
                SearchFields = "EROR,Error", 
                Name = "Error", 
			});
			 
            all.Add(new CustomsTransmissionsStatusDetails()
            {    
                Code = "NSEN", 
                SearchFields = "NSEN,Not sent", 
                Name = "Not sent", 
			});
			 
            all.Add(new CustomsTransmissionsStatusDetails()
            {    
                Code = "SENT", 
                SearchFields = "SENT,Sent", 
                Name = "Sent", 
			});
			
            return all;
       }

	    public void MapPoco(CustomsTransmissionsStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(CustomsTransmissionsStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

