

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
using Logitude.Accounting.BL.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL
{
   public class ReconcileCurrencyTypeDetails : ReconcileCurrencyType, ICloseTable<ReconcileCurrencyType, ReconcileCurrencyTypeDetails>
   {
       public List<ReconcileCurrencyTypeDetails> GetAll()
       {
		    var all = new List<ReconcileCurrencyTypeDetails>(); 
            return all;
       }

	    public void MapPoco(ReconcileCurrencyType newPoco)
        {    
        }

		public string GetSearchFields(ReconcileCurrencyType rec)
        {   
           return string.Empty;
        }
   }
}

