
   
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
   public class CheckEntityTypeDetails : CheckEntityType, ICloseTable<CheckEntityType, CheckEntityTypeDetails>
   {
       public List<CheckEntityTypeDetails> GetAll()
       {
		    var all = new List<CheckEntityTypeDetails>(); 
            return all;
       }

	    public void MapPoco(CheckEntityType newPoco)
        {    
        }

		public string GetSearchFields(CheckEntityType rec)
        {   
           return string.Empty;
        }
   }
}

