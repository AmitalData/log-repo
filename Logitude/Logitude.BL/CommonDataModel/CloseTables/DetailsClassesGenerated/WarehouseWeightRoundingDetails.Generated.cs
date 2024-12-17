

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
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs; 
using Simplog.Data.CommonDataModel;

namespace Logitude.BL.CommonDataModel
{
   public class WarehouseWeightRoundingDetails : WarehouseWeightRounding, ICloseTable<WarehouseWeightRounding, WarehouseWeightRoundingDetails>
   {
       public List<WarehouseWeightRoundingDetails> GetAll()
       {
		    var all = new List<WarehouseWeightRoundingDetails>();  
            all.Add(new WarehouseWeightRoundingDetails()
            {    
                Code = "NON", 
                Name = "None", 
                Display = "None", 
                SearchFields = "NON,None", 
			});
			 
            all.Add(new WarehouseWeightRoundingDetails()
            {    
                Code = "HAF", 
                Name = "Half", 
                Display = "0.5", 
                SearchFields = "HAF,Half", 
			});
			 
            all.Add(new WarehouseWeightRoundingDetails()
            {    
                Code = "ONE", 
                Name = "One", 
                Display = "1", 
                SearchFields = "ONE,One", 
			});
			
            return all;
       }

	    public void MapPoco(WarehouseWeightRounding newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
		    newPoco.Display = this.Display;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(WarehouseWeightRounding rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",",rec.Display,",");
        }
   }
}

