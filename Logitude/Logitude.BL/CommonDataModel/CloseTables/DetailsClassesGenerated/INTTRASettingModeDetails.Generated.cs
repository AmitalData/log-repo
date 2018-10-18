

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
   public class INTTRASettingModeDetails : INTTRASettingMode, ICloseTable<INTTRASettingMode, INTTRASettingModeDetails>
   {
       public List<INTTRASettingModeDetails> GetAll()
       {
		    var all = new List<INTTRASettingModeDetails>(); 
            return all;
       }

	    public void MapPoco(INTTRASettingMode newPoco)
        {    
        }

		public string GetSearchFields(INTTRASettingMode rec)
        {   
           return string.Empty;
        }
   }
}

