
   
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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL
{
   public class SIIRequestStatusDetails : SIIRequestStatus, ICloseTable<SIIRequestStatus, SIIRequestStatusDetails>
   {
       public List<SIIRequestStatusDetails> GetAll()
       {
		    var all = new List<SIIRequestStatusDetails>();  
            all.Add(new SIIRequestStatusDetails()
            {    
                Code = "0", 
                Name = "תקין", 
                SearchFields = "תקין,0", 
			});
			 
            all.Add(new SIIRequestStatusDetails()
            {    
                Code = "100", 
                Name = "נכשל", 
                SearchFields = "100,נכשל", 
			});
			
            return all;
       }

	    public void MapPoco(SIIRequestStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(SIIRequestStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

