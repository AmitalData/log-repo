
   
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
   public class ReconcileMethodDetails : ReconcileMethod, ICloseTable<ReconcileMethod, ReconcileMethodDetails>
   {
       public List<ReconcileMethodDetails> GetAll()
       {
		    var all = new List<ReconcileMethodDetails>();  
            all.Add(new ReconcileMethodDetails()
            {    
                Code = "1", 
                SearchFields = "1,Foreign Currency,מטבע חוץ", 
                Inactive = false, 
                EnglishName = "Foreign Currency", 
                LocalName = "מטבע חוץ", 
			});
			 
            all.Add(new ReconcileMethodDetails()
            {    
                Code = "0", 
                SearchFields = "0,Local Currency,מטבע מקומי", 
                Inactive = false, 
                EnglishName = "Local Currency", 
                LocalName = "מטבע מקומי", 
			});
			
            return all;
       }

	    public void MapPoco(ReconcileMethod newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.EnglishName = this.EnglishName;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(ReconcileMethod rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.EnglishName,",",rec.LocalName,",");
        }
   }
}

