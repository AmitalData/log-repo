

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

namespace Logitude.Customs.BL.CLoseTable
{
   public class UIMessageDetails : UIMessage, ICloseTable<UIMessage, UIMessageDetails>
   {
       public List<UIMessageDetails> GetAll()
       {
		    var all = new List<UIMessageDetails>(); 
            return all;
       }

	    public void MapPoco(UIMessage newPoco)
        {    
        }

		public string GetSearchFields(UIMessage rec)
        {   
           return string.Empty;
        }
   }
}

