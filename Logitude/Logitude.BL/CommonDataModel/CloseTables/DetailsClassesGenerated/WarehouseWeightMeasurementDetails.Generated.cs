

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
   public class WarehouseWeightMeasurementDetails : WarehouseWeightMeasurement, ICloseTable<WarehouseWeightMeasurement, WarehouseWeightMeasurementDetails>
   {
       public List<WarehouseWeightMeasurementDetails> GetAll()
       {
		    var all = new List<WarehouseWeightMeasurementDetails>();  
            all.Add(new WarehouseWeightMeasurementDetails()
            {    
                Name = "Gross Weight", 
                Code = "GRWT", 
                SearchFields = "GRWT,Gross Weight", 
			});
			 
            all.Add(new WarehouseWeightMeasurementDetails()
            {    
                Name = "Chargeable Weight", 
                Code = "CHWT", 
                SearchFields = "CHWT,Chargeable Weight", 
			});
			
            return all;
       }

	    public void MapPoco(WarehouseWeightMeasurement newPoco)
        {   
		    newPoco.Name = this.Name;  
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(WarehouseWeightMeasurement rec)
        {   
           return String.Concat(rec.Name,",",rec.Code,",");
        }
   }
}

