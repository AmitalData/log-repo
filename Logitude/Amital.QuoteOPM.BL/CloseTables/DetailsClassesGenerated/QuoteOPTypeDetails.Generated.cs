
   
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
   public class QuoteOPTypeDetails : QuoteOPType, ICloseTable<QuoteOPType, QuoteOPTypeDetails>
   {
       public List<QuoteOPTypeDetails> GetAll()
       {
		    var all = new List<QuoteOPTypeDetails>();  
            all.Add(new QuoteOPTypeDetails()
            {    
                SearchFields = "P,Routing Rates", 
                Code = "P", 
                Name = "Routing Rates", 
			});
			 
            all.Add(new QuoteOPTypeDetails()
            {    
                SearchFields = "A,Spot Rate", 
                Code = "A", 
                Name = "Spot Rate", 
			});
			
            return all;
       }

	    public void MapPoco(QuoteOPType newPoco)
        {   
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(QuoteOPType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

