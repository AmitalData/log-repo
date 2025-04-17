
   
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
   public class RequestHandlingAuthorityDetails : RequestHandlingAuthority, ICloseTable<RequestHandlingAuthority, RequestHandlingAuthorityDetails>
   {
       public List<RequestHandlingAuthorityDetails> GetAll()
       {
		    var all = new List<RequestHandlingAuthorityDetails>();  
            all.Add(new RequestHandlingAuthorityDetails()
            {    
                Code = "1", 
                EnglishName = "The Standards Institution", 
                SearchFields = "מכון תקנים", 
                LocalName = "מכון תקנים", 
			});
			
            return all;
       }

	    public void MapPoco(RequestHandlingAuthority newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.EnglishName = this.EnglishName;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(RequestHandlingAuthority rec)
        {   
           return String.Concat(rec.Code,",",rec.EnglishName,",",rec.LocalName,",");
        }
   }
}

