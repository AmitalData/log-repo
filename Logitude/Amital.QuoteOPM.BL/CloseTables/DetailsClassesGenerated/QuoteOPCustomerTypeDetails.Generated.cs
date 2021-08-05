
   
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
   public class QuoteOPCustomerTypeDetails : QuoteOPCustomerType, ICloseTable<QuoteOPCustomerType, QuoteOPCustomerTypeDetails>
   {
       public List<QuoteOPCustomerTypeDetails> GetAll()
       {
		    var all = new List<QuoteOPCustomerTypeDetails>();  
            all.Add(new QuoteOPCustomerTypeDetails()
            {    
                Code = "AGT", 
                Name = "Agent", 
                SearchFields = "agt,agent", 
                ShowInLOV = true, 
			});
			 
            all.Add(new QuoteOPCustomerTypeDetails()
            {    
                Code = "CON", 
                Name = "Consignee", 
                SearchFields = "con,consignee", 
                ShowInLOV = true, 
			});
			 
            all.Add(new QuoteOPCustomerTypeDetails()
            {    
                Code = "NOT", 
                Name = "Notify", 
                SearchFields = "not,notify", 
                ShowInLOV = true, 
			});
			 
            all.Add(new QuoteOPCustomerTypeDetails()
            {    
                Code = "OTH", 
                Name = "Other", 
                SearchFields = "oth,other", 
                ShowInLOV = false, 
			});
			 
            all.Add(new QuoteOPCustomerTypeDetails()
            {    
                Code = "SHI", 
                Name = "Shipper", 
                SearchFields = "shi,shipper", 
                ShowInLOV = true, 
			});
			
            return all;
       }

	    public void MapPoco(QuoteOPCustomerType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.ShowInLOV = this.ShowInLOV;   
        }

		public string GetSearchFields(QuoteOPCustomerType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",",rec.ShowInLOV,",");
        }
   }
}

