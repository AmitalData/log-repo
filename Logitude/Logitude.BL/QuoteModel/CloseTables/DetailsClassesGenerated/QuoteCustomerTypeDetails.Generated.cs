

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
   public class QuoteCustomerTypeDetails : QuoteCustomerType, ICloseTable<QuoteCustomerType, QuoteCustomerTypeDetails>
   {
       public List<QuoteCustomerTypeDetails> GetAll()
       {
		    var all = new List<QuoteCustomerTypeDetails>();  
            all.Add(new QuoteCustomerTypeDetails()
            {    
                Code = "AGT", 
                Name = "Agent", 
                SearchFields = "agt,agent", 
                ShowInLOV = true, 
			});
			 
            all.Add(new QuoteCustomerTypeDetails()
            {    
                Code = "CON", 
                Name = "Consignee", 
                SearchFields = "con,consignee", 
                ShowInLOV = true, 
			});
			 
            all.Add(new QuoteCustomerTypeDetails()
            {    
                Code = "NOT", 
                Name = "Notify", 
                SearchFields = "not,notify", 
                ShowInLOV = true, 
			});
			 
            all.Add(new QuoteCustomerTypeDetails()
            {    
                Code = "OTH", 
                Name = "Other", 
                SearchFields = "oth,other", 
                ShowInLOV = false, 
			});
			 
            all.Add(new QuoteCustomerTypeDetails()
            {    
                Code = "SHI", 
                Name = "Shipper", 
                SearchFields = "shi,shipper", 
                ShowInLOV = true, 
			});
			
            return all;
       }

	    public void MapPoco(QuoteCustomerType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.ShowInLOV = this.ShowInLOV;   
        }

		public string GetSearchFields(QuoteCustomerType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",",rec.ShowInLOV,",");
        }
   }
}

