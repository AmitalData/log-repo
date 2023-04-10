
   
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
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs; 
using Logitude.Infrastructure.Data;

namespace Logitude.Infrastructure.BL
{
   public class DigitalPortalLanguageDetails : DigitalPortalLanguage, ICloseTable<DigitalPortalLanguage, DigitalPortalLanguageDetails>
   {
       public List<DigitalPortalLanguageDetails> GetAll()
       {
		    var all = new List<DigitalPortalLanguageDetails>();  
            all.Add(new DigitalPortalLanguageDetails()
            {    
                Code = "EN", 
                Name = "English", 
                DisplayText = "English", 
                SearchFields = "", 
			});
			 
            all.Add(new DigitalPortalLanguageDetails()
            {    
                Code = "ES", 
                Name = "Spanish", 
                DisplayText = "Espanol", 
                SearchFields = "", 
			});
			
            return all;
       }

	    public void MapPoco(DigitalPortalLanguage newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
		    newPoco.DisplayText = this.DisplayText;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(DigitalPortalLanguage rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",",rec.DisplayText,",");
        }
   }
}

