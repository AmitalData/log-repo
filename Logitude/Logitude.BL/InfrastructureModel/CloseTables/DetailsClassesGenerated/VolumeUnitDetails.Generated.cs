

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
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs; 
using Simplog.Data.InfrastructureModel;

namespace Logitude.BL.InfrastructureModel
{
   public class VolumeUnitDetails : VolumeUnit, ICloseTable<VolumeUnit, VolumeUnitDetails>
   {
       public List<VolumeUnitDetails> GetAll()
       {
		    var all = new List<VolumeUnitDetails>();  
            all.Add(new VolumeUnitDetails()
            {    
                SearchFields = "cbf,cbf", 
                Code = "CBF", 
                Name = "CBF", 
			});
			 
            all.Add(new VolumeUnitDetails()
            {    
                SearchFields = "cbi,cbi", 
                Code = "CBI", 
                Name = "CBI", 
			});
			 
            all.Add(new VolumeUnitDetails()
            {    
                SearchFields = "cbm,cbm", 
                Code = "CBM", 
                Name = "CBM", 
			});
			 
            all.Add(new VolumeUnitDetails()
            {    
                SearchFields = "TES,TES", 
                Code = "TES", 
                Name = "TES", 
			});
			
            return all;
       }

	    public void MapPoco(VolumeUnit newPoco)
        {   
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(VolumeUnit rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

