
   
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
   public class ImporterTypeForClaimDetails : ImporterTypeForClaim, ICloseTable<ImporterTypeForClaim, ImporterTypeForClaimDetails>
   {
       public List<ImporterTypeForClaimDetails> GetAll()
       {
		    var all = new List<ImporterTypeForClaimDetails>(); 
            return all;
       }

	    public void MapPoco(ImporterTypeForClaim newPoco)
        {    
        }

		public string GetSearchFields(ImporterTypeForClaim rec)
        {   
           return string.Empty;
        }
   }
}

