

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
   public class QuoteTypeDetails : QuoteType, ICloseTable<QuoteType, QuoteTypeDetails>
   {
       public List<QuoteTypeDetails> GetAll()
       {
		    var all = new List<QuoteTypeDetails>();  
            all.Add(new QuoteTypeDetails()
            {    
                SearchFields = "P,Routing Rates", 
                Code = "P", 
                Name = "Routing Rates", 
			});
			 
            all.Add(new QuoteTypeDetails()
            {    
                SearchFields = "A,Spot Rate", 
                Code = "A", 
                Name = "Spot Rate", 
			});
			
            return all;
       }

	    public void MapPoco(QuoteType newPoco)
        {   
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(QuoteType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

