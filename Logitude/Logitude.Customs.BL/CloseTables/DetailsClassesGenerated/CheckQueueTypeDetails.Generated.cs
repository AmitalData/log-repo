
   
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
   public class CheckQueueTypeDetails : CheckQueueType, ICloseTable<CheckQueueType, CheckQueueTypeDetails>
   {
       public List<CheckQueueTypeDetails> GetAll()
       {
		    var all = new List<CheckQueueTypeDetails>();  
            all.Add(new CheckQueueTypeDetails()
            {    
                Code = "1", 
                SearchFields = "1,משקף", 
                Inactive = false, 
                LocalName = "משקף", 
                EnglishName = "Reflect", 
			});
			 
            all.Add(new CheckQueueTypeDetails()
            {    
                Code = "2", 
                SearchFields = "2,מכסי", 
                Inactive = false, 
                LocalName = "מכסי", 
                EnglishName = "Customs", 
			});
			 
            all.Add(new CheckQueueTypeDetails()
            {    
                Code = "3", 
                SearchFields = "3,משקף + מכסי", 
                Inactive = false, 
                LocalName = "משקף + מכסי", 
                EnglishName = "Reflect AND Customs", 
			});
			
            return all;
       }

	    public void MapPoco(CheckQueueType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;  
		    newPoco.EnglishName = this.EnglishName;   
        }

		public string GetSearchFields(CheckQueueType rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",",rec.EnglishName,",");
        }
   }
}

