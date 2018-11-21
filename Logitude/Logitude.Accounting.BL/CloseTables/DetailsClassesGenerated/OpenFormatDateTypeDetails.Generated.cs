
   
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
   public class OpenFormatDateTypeDetails : OpenFormatDateType, ICloseTable<OpenFormatDateType, OpenFormatDateTypeDetails>
   {
       public List<OpenFormatDateTypeDetails> GetAll()
       {
		    var all = new List<OpenFormatDateTypeDetails>();  
            all.Add(new OpenFormatDateTypeDetails()
            {    
                Code = "1", 
                EnglishName = "Accounting Date", 
                LocalName = "תאריל חשבונאי", 
                SearchFields = "1,Accounting Date,תאריל חשבונאי", 
			});
			 
            all.Add(new OpenFormatDateTypeDetails()
            {    
                Code = "2", 
                EnglishName = "Due Date", 
                LocalName = "תאריך פרעון", 
                SearchFields = "2,Due Date,תאריך פרעון", 
			});
			
            return all;
       }

	    public void MapPoco(OpenFormatDateType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.EnglishName = this.EnglishName;  
		    newPoco.LocalName = this.LocalName;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(OpenFormatDateType rec)
        {   
           return String.Concat(rec.Code,",",rec.EnglishName,",",rec.LocalName,",");
        }
   }
}

