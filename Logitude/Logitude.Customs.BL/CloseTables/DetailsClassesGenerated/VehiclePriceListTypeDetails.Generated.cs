
   
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
   public class VehiclePriceListTypeDetails : VehiclePriceListType, ICloseTable<VehiclePriceListType, VehiclePriceListTypeDetails>
   {
       public List<VehiclePriceListTypeDetails> GetAll()
       {
		    var all = new List<VehiclePriceListTypeDetails>();  
            all.Add(new VehiclePriceListTypeDetails()
            {    
                Code = "1", 
                EnglishName = "price1", 
                SearchFields = "price1,1", 
                Inactive = false, 
                LocalName = "price1", 
			});
			
            return all;
       }

	    public void MapPoco(VehiclePriceListType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.EnglishName = this.EnglishName;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(VehiclePriceListType rec)
        {   
           return String.Concat(rec.Code,",",rec.EnglishName,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

