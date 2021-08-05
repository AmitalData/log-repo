
   
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
   public class QuoteOPRatingDetails : QuoteOPRating, ICloseTable<QuoteOPRating, QuoteOPRatingDetails>
   {
       public List<QuoteOPRatingDetails> GetAll()
       {
		    var all = new List<QuoteOPRatingDetails>();  
            all.Add(new QuoteOPRatingDetails()
            {    
                Code = "C", 
                IndexOrder = 1, 
                SearchFields = "C,Cold", 
                Name = "Cold", 
			});
			 
            all.Add(new QuoteOPRatingDetails()
            {    
                Code = "H", 
                IndexOrder = 4, 
                SearchFields = "H,Hot", 
                Name = "Hot", 
			});
			 
            all.Add(new QuoteOPRatingDetails()
            {    
                Code = "N", 
                IndexOrder = 2, 
                SearchFields = "N,Neutral", 
                Name = "Neutral", 
			});
			 
            all.Add(new QuoteOPRatingDetails()
            {    
                Code = "W", 
                IndexOrder = 3, 
                SearchFields = "W,Warm", 
                Name = "Warm", 
			});
			
            return all;
       }

	    public void MapPoco(QuoteOPRating newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.IndexOrder = this.IndexOrder;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(QuoteOPRating rec)
        {   
           return String.Concat(rec.Code,",",rec.IndexOrder,",",rec.Name,",");
        }
   }
}

