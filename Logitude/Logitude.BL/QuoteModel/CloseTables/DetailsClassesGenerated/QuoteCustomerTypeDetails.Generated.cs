

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
                SearchFields = "AGT,Agent,True,", 
                ShowInLOV = true, 
			});
			 
            all.Add(new QuoteCustomerTypeDetails()
            {    
                Code = "CON", 
                Name = "Consignee", 
                SearchFields = "CON,Consignee,True,", 
                ShowInLOV = true, 
			});
			 
            all.Add(new QuoteCustomerTypeDetails()
            {    
                Code = "NOT", 
                Name = "Notify", 
                SearchFields = "NOT,Notify,True,", 
                ShowInLOV = true, 
			});
			 
            all.Add(new QuoteCustomerTypeDetails()
            {    
                Code = "OTH", 
                Name = "Other", 
                SearchFields = "OTH,Other,False,", 
                ShowInLOV = false, 
			});
			 
            all.Add(new QuoteCustomerTypeDetails()
            {    
                Code = "SHI", 
                Name = "Shipper", 
                SearchFields = "SHI,Shipper,True,", 
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

