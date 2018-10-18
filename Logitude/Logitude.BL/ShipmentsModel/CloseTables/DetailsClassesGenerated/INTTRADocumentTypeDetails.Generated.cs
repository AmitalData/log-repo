

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
   public class INTTRADocumentTypeDetails : INTTRADocumentType, ICloseTable<INTTRADocumentType, INTTRADocumentTypeDetails>
   {
       public List<INTTRADocumentTypeDetails> GetAll()
       {
		    var all = new List<INTTRADocumentTypeDetails>();  
            all.Add(new INTTRADocumentTypeDetails()
            {    
                Code = "COPY", 
                SearchFields = "COPY,BL Copy", 
                Name = "BL Copy", 
			});
			 
            all.Add(new INTTRADocumentTypeDetails()
            {    
                Code = "ORIG", 
                SearchFields = "ORIG,BL Original", 
                Name = "BL Original", 
			});
			 
            all.Add(new INTTRADocumentTypeDetails()
            {    
                Code = "LADN", 
                SearchFields = "LADN,House BL", 
                Name = "House BL", 
			});
			 
            all.Add(new INTTRADocumentTypeDetails()
            {    
                Code = "BILL", 
                SearchFields = "BILL,Sea Waybill", 
                Name = "Sea Waybill", 
			});
			
            return all;
       }

	    public void MapPoco(INTTRADocumentType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(INTTRADocumentType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

