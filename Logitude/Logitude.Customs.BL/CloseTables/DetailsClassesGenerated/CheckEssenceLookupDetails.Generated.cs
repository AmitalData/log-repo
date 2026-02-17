
   
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
   public class CheckEssenceLookupDetails : CheckEssenceLookup, ICloseTable<CheckEssenceLookup, CheckEssenceLookupDetails>
   {
       public List<CheckEssenceLookupDetails> GetAll()
       {
		    var all = new List<CheckEssenceLookupDetails>(); 
            return all;
       }

	    public void MapPoco(CheckEssenceLookup newPoco)
        {    
        }

		public string GetSearchFields(CheckEssenceLookup rec)
        {   
           return string.Empty;
        }
   }
}

