

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
   public class PortTimeZoneDetails : PortTimeZone, ICloseTable<PortTimeZone, PortTimeZoneDetails>
   {
       public List<PortTimeZoneDetails> GetAll()
       {
		    var all = new List<PortTimeZoneDetails>(); 
            return all;
       }

	    public void MapPoco(PortTimeZone newPoco)
        {    
        }

		public string GetSearchFields(PortTimeZone rec)
        {   
           return string.Empty;
        }
   }
}

