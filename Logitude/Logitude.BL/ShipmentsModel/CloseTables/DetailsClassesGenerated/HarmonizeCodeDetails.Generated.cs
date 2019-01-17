

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
   public class HarmonizeCodeDetails : HarmonizeCode, ICloseTable<HarmonizeCode, HarmonizeCodeDetails>
   {
       public List<HarmonizeCodeDetails> GetAll()
       {
		    var all = new List<HarmonizeCodeDetails>(); 
            return all;
       }

	    public void MapPoco(HarmonizeCode newPoco)
        {    
        }

		public string GetSearchFields(HarmonizeCode rec)
        {   
           return string.Empty;
        }
   }
}

