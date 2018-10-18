

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
   public class DimensionsUnitDetails : DimensionsUnit, ICloseTable<DimensionsUnit, DimensionsUnitDetails>
   {
       public List<DimensionsUnitDetails> GetAll()
       {
		    var all = new List<DimensionsUnitDetails>();  
            all.Add(new DimensionsUnitDetails()
            {    
                SearchFields = "cm,cm", 
                Code = "Cm", 
                Name = "Cm", 
			});
			 
            all.Add(new DimensionsUnitDetails()
            {    
                SearchFields = "ft,ft", 
                Code = "Ft", 
                Name = "Ft", 
			});
			 
            all.Add(new DimensionsUnitDetails()
            {    
                SearchFields = "inc,inch", 
                Code = "Inc", 
                Name = "Inch", 
			});
			
            return all;
       }

	    public void MapPoco(DimensionsUnit newPoco)
        {   
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(DimensionsUnit rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

