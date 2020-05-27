
   
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
   public class CustomsTransportModeDetails : CustomsTransportMode, ICloseTable<CustomsTransportMode, CustomsTransportModeDetails>
   {
       public List<CustomsTransportModeDetails> GetAll()
       {
		    var all = new List<CustomsTransportModeDetails>();  
            all.Add(new CustomsTransportModeDetails()
            {    
                Code = "A", 
                EnglishName = "Air", 
                SearchFields = "a,air,אויר", 
                Inactive = false, 
                LocalName = "אויר", 
			});
			 
            all.Add(new CustomsTransportModeDetails()
            {    
                Code = "L", 
                EnglishName = "Inland", 
                SearchFields = "l,inland,יבשה", 
                Inactive = false, 
                LocalName = "יבשה", 
			});
			 
            all.Add(new CustomsTransportModeDetails()
            {    
                Code = "O", 
                EnglishName = "Ocean", 
                SearchFields = "o,ocean,ים", 
                Inactive = false, 
                LocalName = "ים", 
			});
			
            return all;
       }

	    public void MapPoco(CustomsTransportMode newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.EnglishName = this.EnglishName;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(CustomsTransportMode rec)
        {   
           return String.Concat(rec.Code,",",rec.EnglishName,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

