

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
   public class ValidByTypeDetails : ValidByType, ICloseTable<ValidByType, ValidByTypeDetails>
   {
       public List<ValidByTypeDetails> GetAll()
       {
		    var all = new List<ValidByTypeDetails>();  
            all.Add(new ValidByTypeDetails()
            {    
                Code = "EAD", 
                Name = "ETD/ATD", 
                SearchFields = "EAD,ETD/ATD", 
			});
			
            return all;
       }

	    public void MapPoco(ValidByType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(ValidByType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

