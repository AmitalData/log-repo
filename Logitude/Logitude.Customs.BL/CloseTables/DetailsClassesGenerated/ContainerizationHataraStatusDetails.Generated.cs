
   
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
   public class ContainerizationHatataStatusDetails : ContainerizationHatataStatus, ICloseTable<ContainerizationHatataStatus, ContainerizationHatataStatusDetails>
   {
       public List<ContainerizationHatataStatusDetails> GetAll()
       {
		    var all = new List<ContainerizationHatataStatusDetails>();  
            all.Add(new ContainerizationHatataStatusDetails()
            {    
                Code = "1", 
                Name = "המכלה הותרה", 
                SearchFields = "המכלה הותרה", 
			});
			 
            all.Add(new ContainerizationHatataStatusDetails()
            {    
                Code = "2", 
                Name = "המכלה טרם הותרה", 
                SearchFields = "המכלה טרם הותרה", 
			});
			
            return all;
       }

	    public void MapPoco(ContainerizationHatataStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(ContainerizationHatataStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

