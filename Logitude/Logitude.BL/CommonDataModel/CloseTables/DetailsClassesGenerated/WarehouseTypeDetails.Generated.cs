

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
   public class WarehouseTypeDetails : WarehouseType, ICloseTable<WarehouseType, WarehouseTypeDetails>
   {
       public List<WarehouseTypeDetails> GetAll()
       {
		    var all = new List<WarehouseTypeDetails>();  
            all.Add(new WarehouseTypeDetails()
            {    
                Code = "BO", 
                SearchFields = "BO,Bonded", 
                Name = "Bonded", 
			});
			 
            all.Add(new WarehouseTypeDetails()
            {    
                Code = "TM", 
                SearchFields = "TM,Terminal", 
                Name = "Terminal", 
			});
			
            return all;
       }

	    public void MapPoco(WarehouseType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(WarehouseType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

