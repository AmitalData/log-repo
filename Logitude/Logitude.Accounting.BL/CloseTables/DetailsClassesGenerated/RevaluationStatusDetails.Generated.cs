
   
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
            all.Add(new RevaluationStatusDetails()
            {    
                Code = "1", 
                Name = "In Progress", 
                LocalName = "בתהליך", 
                SearchFields = "1,In Progress,בתהליך", 
			});
			 
            all.Add(new RevaluationStatusDetails()
            {    
                Code = "2", 
                Name = "Done", 
                LocalName = "הסתיים", 
                SearchFields = "2,Done,הסתיים", 
			});
			 
            all.Add(new RevaluationStatusDetails()
            {    
                Code = "3", 
                Name = "Failed", 
                SearchFields = "3,Failed,נִכשָׁל", 
                LocalName = "נִכשָׁל", 
			});
			
            return all;
       }

	    public void MapPoco(RevaluationStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
		    newPoco.LocalName = this.LocalName;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(RevaluationStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",",rec.LocalName,",");
        }
   }
}

