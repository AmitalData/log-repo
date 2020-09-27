
   
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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL
{
   public class CustomsBookTypeDetails : CustomsBookType, ICloseTable<CustomsBookType, CustomsBookTypeDetails>
   {
       public List<CustomsBookTypeDetails> GetAll()
       {
		    var all = new List<CustomsBookTypeDetails>();  
            all.Add(new CustomsBookTypeDetails()
            {    
                Code = "1", 
                SearchFields = "1,יבוא", 
                Inactive = false, 
                LocalName = "יבוא", 
			});
			 
            all.Add(new CustomsBookTypeDetails()
            {    
                Code = "2", 
                SearchFields = "2,יצוא", 
                Inactive = false, 
                LocalName = "יצוא", 
			});
			 
            all.Add(new CustomsBookTypeDetails()
            {    
                Code = "3", 
                SearchFields = "3,אוטונומיה", 
                Inactive = false, 
                LocalName = "אוטונומיה", 
			});
			
            return all;
       }

	    public void MapPoco(CustomsBookType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(CustomsBookType rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

