

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
   public class ContainerTrackingProviderDetails : ContainerTrackingProvider, ICloseTable<ContainerTrackingProvider, ContainerTrackingProviderDetails>
   {
       public List<ContainerTrackingProviderDetails> GetAll()
       {
		    var all = new List<ContainerTrackingProviderDetails>(); 
            return all;
       }

	    public void MapPoco(ContainerTrackingProvider newPoco)
        {    
        }

		public string GetSearchFields(ContainerTrackingProvider rec)
        {   
           return string.Empty;
        }
   }
}

