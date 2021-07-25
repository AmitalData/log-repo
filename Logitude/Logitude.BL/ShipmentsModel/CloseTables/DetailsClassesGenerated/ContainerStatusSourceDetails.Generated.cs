

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
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityPMs; 
using Simplog.Data.ShipmentsModel;

namespace Logitude.BL.ShipmentsModel
{
   public class ContainerStatusSourceDetails : ContainerStatusSource, ICloseTable<ContainerStatusSource, ContainerStatusSourceDetails>
   {
       public List<ContainerStatusSourceDetails> GetAll()
       {
		    var all = new List<ContainerStatusSourceDetails>();  
            all.Add(new ContainerStatusSourceDetails()
            {    
                Code = "INT", 
                Name = "INTTRA", 
                SearchFields = "INT,INTTRA", 
			});
			 
            all.Add(new ContainerStatusSourceDetails()
            {    
                Code = "OIN", 
                Name = "Ocean Insights", 
                SearchFields = "OIN,Ocean Insights", 
			});
			
            return all;
       }

	    public void MapPoco(ContainerStatusSource newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(ContainerStatusSource rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

