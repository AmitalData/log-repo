
   
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
   public class CargoIdentityQualifierDetails : CargoIdentityQualifier, ICloseTable<CargoIdentityQualifier, CargoIdentityQualifierDetails>
   {
       public List<CargoIdentityQualifierDetails> GetAll()
       {
		    var all = new List<CargoIdentityQualifierDetails>();  
            all.Add(new CargoIdentityQualifierDetails()
            {    
                Code = "CN", 
                EnglishName = "CN", 
                SearchFields = "CN", 
                InActive = false, 
                LocalName = "CN", 
			});
			 
            all.Add(new CargoIdentityQualifierDetails()
            {    
                Code = "ZZZ", 
                EnglishName = "zzzzz", 
                SearchFields = "ZZZ", 
                InActive = false, 
                LocalName = "zzz", 
			});
			
            return all;
       }

	    public void MapPoco(CargoIdentityQualifier newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.EnglishName = this.EnglishName;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.InActive = this.InActive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(CargoIdentityQualifier rec)
        {   
           return String.Concat(rec.Code,",",rec.EnglishName,",",rec.InActive,",",rec.LocalName,",");
        }
   }
}

