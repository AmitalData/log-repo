
   
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
   public class AccumalationStateDetails : AccumalationState, ICloseTable<AccumalationState, AccumalationStateDetails>
   {
       public List<AccumalationStateDetails> GetAll()
       {
		    var all = new List<AccumalationStateDetails>(); 
            return all;
       }

	    public void MapPoco(AccumalationState newPoco)
        {    
        }

		public string GetSearchFields(AccumalationState rec)
        {   
           return string.Empty;
        }
   }
}

