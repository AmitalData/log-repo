
   
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
   public class ItemGovernmentProcedureTypeDetails : ItemGovernmentProcedureType, ICloseTable<ItemGovernmentProcedureType, ItemGovernmentProcedureTypeDetails>
   {
       public List<ItemGovernmentProcedureTypeDetails> GetAll()
       {
		    var all = new List<ItemGovernmentProcedureTypeDetails>(); 
            return all;
       }

	    public void MapPoco(ItemGovernmentProcedureType newPoco)
        {    
        }

		public string GetSearchFields(ItemGovernmentProcedureType rec)
        {   
           return string.Empty;
        }
   }
}

