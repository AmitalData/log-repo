

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
                Code = "O", 
                Name = "Origin Charges", 
                Searchfields = "l,origin charges", 
			});
			 
            all.Add(new QuoteGroupSectionDetails()
            {    
                Code = "F", 
                Name = "Freight Charges", 
                Searchfields = "l,freight charges", 
			});
			 
            all.Add(new QuoteGroupSectionDetails()
            {    
                Code = "D", 
                Name = "Destination Charges", 
                Searchfields = "l,destination charges", 
			});
			
            return all;
       }

	    public void MapPoco(QuoteGroupSection newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
		    newPoco.Searchfields = this.Searchfields;   
        }

		public string GetSearchFields(QuoteGroupSection rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",",rec.Searchfields,",");
        }
   }
}

