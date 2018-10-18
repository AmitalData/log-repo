

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
   public class TemperatureUnitDetails : TemperatureUnit, ICloseTable<TemperatureUnit, TemperatureUnitDetails>
   {
       public List<TemperatureUnitDetails> GetAll()
       {
		    var all = new List<TemperatureUnitDetails>();  
            all.Add(new TemperatureUnitDetails()
            {    
                Code = "CEL", 
                SearchFields = "CEL,Celsius", 
                Name = "Celsius", 
			});
			 
            all.Add(new TemperatureUnitDetails()
            {    
                Code = "FAH", 
                SearchFields = "FAH,Fahrenheit", 
                Name = "Fahrenheit", 
			});
			
            return all;
       }

	    public void MapPoco(TemperatureUnit newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(TemperatureUnit rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

