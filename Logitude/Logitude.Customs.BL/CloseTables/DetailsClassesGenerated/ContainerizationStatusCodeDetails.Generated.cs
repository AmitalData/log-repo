
   
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
   public class ContainerizationStatusCodeDetails : ContainerizationStatusCode, ICloseTable<ContainerizationStatusCode, ContainerizationStatusCodeDetails>
   {
       public List<ContainerizationStatusCodeDetails> GetAll()
       {
		    var all = new List<ContainerizationStatusCodeDetails>();  
            all.Add(new ContainerizationStatusCodeDetails()
            {    
                Code = "1", 
                Name = "המכלה תקינה, טרם הותרה", 
                SearchFields = "המכלה תקינה, טרם הותרה", 
			});
			 
            all.Add(new ContainerizationStatusCodeDetails()
            {    
                Code = "2", 
                Name = "המכלה שגויה", 
                SearchFields = "המכלה שגויה", 
			});
			 
            all.Add(new ContainerizationStatusCodeDetails()
            {    
                Code = "3", 
                Name = "המכלה מבוטלת", 
                SearchFields = "המכלה מבוטלת", 
			});
			
            return all;
       }

	    public void MapPoco(ContainerizationStatusCode newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(ContainerizationStatusCode rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

