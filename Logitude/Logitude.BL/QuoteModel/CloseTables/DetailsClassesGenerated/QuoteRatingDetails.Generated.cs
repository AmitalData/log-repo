

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
   public class QuoteRatingDetails : QuoteRating, ICloseTable<QuoteRating, QuoteRatingDetails>
   {
       public List<QuoteRatingDetails> GetAll()
       {
		    var all = new List<QuoteRatingDetails>();  
            all.Add(new QuoteRatingDetails()
            {    
                Code = "C", 
                IndexOrder = 1, 
                SearchFields = "C,Cold", 
                Name = "Cold", 
			});
			 
            all.Add(new QuoteRatingDetails()
            {    
                Code = "H", 
                IndexOrder = 4, 
                SearchFields = "H,Hot", 
                Name = "Hot", 
			});
			 
            all.Add(new QuoteRatingDetails()
            {    
                Code = "N", 
                IndexOrder = 2, 
                SearchFields = "N,Neutral", 
                Name = "Neutral", 
			});
			 
            all.Add(new QuoteRatingDetails()
            {    
                Code = "W", 
                IndexOrder = 3, 
                SearchFields = "W,Warm", 
                Name = "Warm", 
			});
			
            return all;
       }

	    public void MapPoco(QuoteRating newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.IndexOrder = this.IndexOrder;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(QuoteRating rec)
        {   
           return String.Concat(rec.Code,",",rec.IndexOrder,",",rec.Name,",");
        }
   }
}

