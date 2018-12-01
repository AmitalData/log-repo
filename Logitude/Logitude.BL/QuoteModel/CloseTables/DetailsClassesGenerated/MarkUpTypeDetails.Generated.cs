

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
using Simplog.Data.QuoteModel.EntityPOCOs;
using Logitude.BL.QuoteModel.EntityPMs; 
using Simplog.Data.QuoteModel;

namespace Logitude.BL.QuoteModel
{
   public class MarkUpTypeDetails : MarkUpType, ICloseTable<MarkUpType, MarkUpTypeDetails>
   {
       public List<MarkUpTypeDetails> GetAll()
       {
		    var all = new List<MarkUpTypeDetails>();  
            all.Add(new MarkUpTypeDetails()
            {    
                SearchFields = "F,Fixed,", 
                Code = "F", 
                Name = "Fixed", 
			});
			 
            all.Add(new MarkUpTypeDetails()
            {    
                SearchFields = "P,Percentage,", 
                Code = "P", 
                Name = "Percentage", 
			});
			
            return all;
       }

	    public void MapPoco(MarkUpType newPoco)
        {   
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(MarkUpType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

