
   
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
   public class TariffTypeDetails : TariffType, ICloseTable<TariffType, TariffTypeDetails>
   {
       public List<TariffTypeDetails> GetAll()
       {
		    var all = new List<TariffTypeDetails>();  
            all.Add(new TariffTypeDetails()
            {    
                Code = "AFC", 
                Name = "Air Freight Cost", 
                SearchFields = "AFC,Air Freight Cost", 
			});
			 
            all.Add(new TariffTypeDetails()
            {    
                Name = "Air Surcharges Cost", 
                Code = "ASC", 
                SearchFields = "ASC,Air Surcharges Cost", 
			});
			
            return all;
       }

	    public void MapPoco(TariffType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(TariffType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

