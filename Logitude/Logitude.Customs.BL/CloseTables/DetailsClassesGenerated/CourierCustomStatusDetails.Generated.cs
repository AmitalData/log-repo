
   
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
   public class CourierCustomStatusDetails : CourierCustomStatus, ICloseTable<CourierCustomStatus, CourierCustomStatusDetails>
   {
       public List<CourierCustomStatusDetails> GetAll()
       {
		    var all = new List<CourierCustomStatusDetails>(); 
            return all;
       }

	    public void MapPoco(CourierCustomStatus newPoco)
        {    
        }

		public string GetSearchFields(CourierCustomStatus rec)
        {   
           return string.Empty;
        }
   }
}

