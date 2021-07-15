
   
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
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Def.EntityPMs; 
using Amital.QuoteOPM.Data;

namespace Amital.QuoteOPM.BL
{
   public class MarkUpOPTypeDetails : MarkUpOPType, ICloseTable<MarkUpOPType, MarkUpOPTypeDetails>
   {
       public List<MarkUpOPTypeDetails> GetAll()
       {
		    var all = new List<MarkUpOPTypeDetails>();  
            all.Add(new MarkUpOPTypeDetails()
            {    
                SearchFields = "F,Fixed", 
                Code = "F", 
                Name = "Fixed", 
			});
			 
            all.Add(new MarkUpOPTypeDetails()
            {    
                SearchFields = "P,Percentage", 
                Code = "P", 
                Name = "Percentage", 
			});
			
            return all;
       }

	    public void MapPoco(MarkUpOPType newPoco)
        {   
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(MarkUpOPType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

