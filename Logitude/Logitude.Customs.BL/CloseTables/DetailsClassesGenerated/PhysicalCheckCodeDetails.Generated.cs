
   
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
   public class PhysicalCheckCodeDetails : PhysicalCheckCode, ICloseTable<PhysicalCheckCode, PhysicalCheckCodeDetails>
   {
       public List<PhysicalCheckCodeDetails> GetAll()
       {
		    var all = new List<PhysicalCheckCodeDetails>();  
            all.Add(new PhysicalCheckCodeDetails()
            {    
                Code = "1", 
                Name = "בדיקה פיזית פתוחה", 
                SearchFields = "1,בדיקה פיזית פתוחה", 
			});
			 
            all.Add(new PhysicalCheckCodeDetails()
            {    
                Code = "2", 
                Name = "בדיקה פיזית סגורה", 
                SearchFields = "2,בדיקה פיזית סגורה", 
			});
			 
            all.Add(new PhysicalCheckCodeDetails()
            {    
                Name = "ללא בדיקה", 
                SearchFields = "ללא בדיקה", 
                Code = "N", 
			});
			
            return all;
       }

	    public void MapPoco(PhysicalCheckCode newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(PhysicalCheckCode rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

