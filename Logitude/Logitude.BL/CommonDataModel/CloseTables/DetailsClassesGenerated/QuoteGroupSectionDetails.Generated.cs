

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
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs; 
using Simplog.Data.CommonDataModel;

namespace Logitude.BL.CommonDataModel
{
   public class QuoteGroupSectionDetails : QuoteGroupSection, ICloseTable<QuoteGroupSection, QuoteGroupSectionDetails>
   {
       public List<QuoteGroupSectionDetails> GetAll()
       {
		    var all = new List<QuoteGroupSectionDetails>();  
            all.Add(new QuoteGroupSectionDetails()
            {    
                Id = "1", 
                Tenant = 1, 
                Code = "O", 
                Name = "Origin Charges", 
			});
			 
            all.Add(new QuoteGroupSectionDetails()
            {    
                Id = "2", 
                Tenant = 1, 
                Code = "F", 
                Name = "Freight Charges", 
			});
			 
            all.Add(new QuoteGroupSectionDetails()
            {    
                Id = "3", 
                Tenant = 1, 
                Code = "D", 
                Name = "Destination Charges", 
			});
			
            return all;
       }

	    public void MapPoco(QuoteGroupSection newPoco)
        {   
		    newPoco.Id = this.Id;  
		    newPoco.Tenant = this.Tenant;  
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(QuoteGroupSection rec)
        {   
           return String.Concat(rec.Id,",",rec.Tenant,",",rec.Code,",",rec.Name,",");
        }
   }
}

