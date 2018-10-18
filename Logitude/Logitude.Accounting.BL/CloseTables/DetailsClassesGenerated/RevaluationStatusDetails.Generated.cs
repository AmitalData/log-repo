
   
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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL
{
   public class RevaluationStatusDetails : RevaluationStatus, ICloseTable<RevaluationStatus, RevaluationStatusDetails>
   {
       public List<RevaluationStatusDetails> GetAll()
       {
		    var all = new List<RevaluationStatusDetails>(); 
            return all;
       }

	    public void MapPoco(RevaluationStatus newPoco)
        {    
        }

		public string GetSearchFields(RevaluationStatus rec)
        {   
           return string.Empty;
        }
   }
}

