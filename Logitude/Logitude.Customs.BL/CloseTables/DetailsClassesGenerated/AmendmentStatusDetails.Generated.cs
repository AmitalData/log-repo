
   
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
   public class AmendmentStatusDetails : AmendmentStatus, ICloseTable<AmendmentStatus, AmendmentStatusDetails>
   {
       public List<AmendmentStatusDetails> GetAll()
       {
		    var all = new List<AmendmentStatusDetails>();  
            all.Add(new AmendmentStatusDetails()
            {    
                Code = 1, 
                Name = "ממתין לאישור/דחיה", 
                SearchFields = "1,ממתין לאישור/דחיה", 
			});
			 
            all.Add(new AmendmentStatusDetails()
            {    
                Code = 2, 
                Name = "שגוי", 
                SearchFields = "2,שגוי", 
			});
			 
            all.Add(new AmendmentStatusDetails()
            {    
                Code = 3, 
                Name = "תיקון אושר", 
                SearchFields = "3,תיקון אושר", 
			});
			 
            all.Add(new AmendmentStatusDetails()
            {    
                Code = 4, 
                Name = "תיקון נדחה", 
                SearchFields = "4,תיקון נדחה", 
			});
			 
            all.Add(new AmendmentStatusDetails()
            {    
                Code = 5, 
                Name = "5,בוטל", 
                SearchFields = "4,תיקון נדחה", 
			});
			 
            all.Add(new AmendmentStatusDetails()
            {    
                Code = 6, 
                Name = "אושרה חלקית", 
                SearchFields = "6,אושרה חלקית", 
			});
			
            return all;
       }

	    public void MapPoco(AmendmentStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(AmendmentStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

