

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
   public class WeightUnitDetails : WeightUnit, ICloseTable<WeightUnit, WeightUnitDetails>
   {
       public List<WeightUnitDetails> GetAll()
       {
		    var all = new List<WeightUnitDetails>();  
            all.Add(new WeightUnitDetails()
            {    
                SearchFields = "kg,kilogram", 
                Code = "KG", 
                Name = "Kilogram", 
			});
			 
            all.Add(new WeightUnitDetails()
            {    
                SearchFields = "mt,metric ton", 
                Code = "MT", 
                Name = "Metric Ton", 
			});
			 
            all.Add(new WeightUnitDetails()
            {    
                SearchFields = "lb,pound", 
                Code = "LB", 
                Name = "Pound", 
			});
			
            return all;
       }

	    public void MapPoco(WeightUnit newPoco)
        {   
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(WeightUnit rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

