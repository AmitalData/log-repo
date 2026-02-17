
   
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
   public class AuthorizedSignerPermitDetails : AuthorizedSignerPermit, ICloseTable<AuthorizedSignerPermit, AuthorizedSignerPermitDetails>
   {
       public List<AuthorizedSignerPermitDetails> GetAll()
       {
		    var all = new List<AuthorizedSignerPermitDetails>(); 
            return all;
       }

	    public void MapPoco(AuthorizedSignerPermit newPoco)
        {    
        }

		public string GetSearchFields(AuthorizedSignerPermit rec)
        {   
           return string.Empty;
        }
   }
}

