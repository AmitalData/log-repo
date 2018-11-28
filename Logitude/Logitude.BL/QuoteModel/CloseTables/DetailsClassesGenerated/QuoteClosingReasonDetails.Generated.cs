

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
   public class QuoteClosingReasonDetails : QuoteClosingReason, ICloseTable<QuoteClosingReason, QuoteClosingReasonDetails>
   {
       public List<QuoteClosingReasonDetails> GetAll()
       {
		    var all = new List<QuoteClosingReasonDetails>();  
            all.Add(new QuoteClosingReasonDetails()
            {    
                Code = "EQ", 
                Name = "Expensive Quote", 
                SearchFields = "EQ,Expensive Quote,", 
			});
			 
            all.Add(new QuoteClosingReasonDetails()
            {    
                Code = "XQ", 
                Name = "Expired Quote", 
                SearchFields = "XQ,Expired Quote,", 
			});
			 
            all.Add(new QuoteClosingReasonDetails()
            {    
                Code = "GS", 
                Name = "Given directly to the Shipping Line", 
                SearchFields = "GS,Given directly to the Shipping Line,", 
			});
			 
            all.Add(new QuoteClosingReasonDetails()
            {    
                Code = "LS", 
                Name = "Lack of Service in the Last Shipment", 
                SearchFields = "LS,Lack of Service in the Last Shipment,", 
			});
			 
            all.Add(new QuoteClosingReasonDetails()
            {    
                Code = "LC", 
                Name = "Lost to Competitor", 
                SearchFields = "LC,Lost to Competitor,", 
			});
			
            return all;
       }

	    public void MapPoco(QuoteClosingReason newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(QuoteClosingReason rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

