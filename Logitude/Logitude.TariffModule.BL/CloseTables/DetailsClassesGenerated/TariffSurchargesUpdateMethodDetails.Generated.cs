
   
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
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.EntityPMs; 
using Logitude.TariffModule.Data;

namespace Logitude.TariffModule.BL.CLoseTable
{
   public class TariffSurchargesUpdateMethodDetails : TariffSurchargesUpdateMethod, ICloseTable<TariffSurchargesUpdateMethod, TariffSurchargesUpdateMethodDetails>
   {
       public List<TariffSurchargesUpdateMethodDetails> GetAll()
       {
		    var all = new List<TariffSurchargesUpdateMethodDetails>();  
            all.Add(new TariffSurchargesUpdateMethodDetails()
            {    
                Code = "BA", 
                Name = "Batch", 
                SearchFields = "BA,Batch", 
			});
			 
            all.Add(new TariffSurchargesUpdateMethodDetails()
            {    
                Code = "MA", 
                Name = "Manual", 
                SearchFields = "MA,Manual", 
			});
			
            return all;
       }

	    public void MapPoco(TariffSurchargesUpdateMethod newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(TariffSurchargesUpdateMethod rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

