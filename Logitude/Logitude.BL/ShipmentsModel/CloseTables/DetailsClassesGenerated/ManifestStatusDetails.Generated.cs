

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
   public class ManifestStatusDetails : ManifestStatus, ICloseTable<ManifestStatus, ManifestStatusDetails>
   {
       public List<ManifestStatusDetails> GetAll()
       {
		    var all = new List<ManifestStatusDetails>();  
            all.Add(new ManifestStatusDetails()
            {    
                Code = "ACCP", 
                SearchFields = "ACCP,Accepted", 
                Name = "Accepted", 
			});
			 
            all.Add(new ManifestStatusDetails()
            {    
                Code = "DECL", 
                SearchFields = "DECL,Declined", 
                Name = "Declined", 
			});
			 
            all.Add(new ManifestStatusDetails()
            {    
                Code = "NSEN", 
                SearchFields = "NSEN,Not sent", 
                Name = "Not sent", 
			});
			 
            all.Add(new ManifestStatusDetails()
            {    
                Code = "RCVD", 
                SearchFields = "RCVD,Received", 
                Name = "Received", 
			});
			 
            all.Add(new ManifestStatusDetails()
            {    
                Code = "SENT", 
                SearchFields = "SENT,Sent", 
                Name = "Sent", 
			});
			
            return all;
       }

	    public void MapPoco(ManifestStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(ManifestStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

