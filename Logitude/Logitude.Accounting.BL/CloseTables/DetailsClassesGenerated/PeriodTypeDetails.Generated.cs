
   
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
   public class PeriodTypeDetails : PeriodType, ICloseTable<PeriodType, PeriodTypeDetails>
   {
       public List<PeriodTypeDetails> GetAll()
       {
		    var all = new List<PeriodTypeDetails>();  
            all.Add(new PeriodTypeDetails()
            {    
                Code = "1", 
                LocalName = "תקופה חשבונאית", 
                SearchFields = "1,Accounting,חשבונאות", 
                Inactive = false, 
                EnglishName = "Accounting", 
			});
			 
            all.Add(new PeriodTypeDetails()
            {    
                Code = "3", 
                EnglishName = "Interest Invoice", 
                LocalName = "חשבונית ריבית", 
                SearchFields = "3,Interest Invoice,חשבונית ריבית", 
                Inactive = false, 
			});
			 
            all.Add(new PeriodTypeDetails()
            {    
                Code = "2", 
                LocalName = "חשבונית כללית", 
                SearchFields = "2,Invoice,חשבונית כללית", 
                Inactive = false, 
                EnglishName = "Invoice", 
			});
			
            return all;
       }

	    public void MapPoco(PeriodType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.LocalName = this.LocalName;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.EnglishName = this.EnglishName;   
        }

		public string GetSearchFields(PeriodType rec)
        {   
           return String.Concat(rec.Code,",",rec.LocalName,",",rec.Inactive,",",rec.EnglishName,",");
        }
   }
}

