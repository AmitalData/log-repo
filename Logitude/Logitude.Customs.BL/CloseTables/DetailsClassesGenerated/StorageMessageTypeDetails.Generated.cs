
   
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
   public class StorageMessageTypeDetails : StorageMessageType, ICloseTable<StorageMessageType, StorageMessageTypeDetails>
   {
       public List<StorageMessageTypeDetails> GetAll()
       {
		    var all = new List<StorageMessageTypeDetails>(); 
            return all;
       }

	    public void MapPoco(StorageMessageType newPoco)
        {    
        }

		public string GetSearchFields(StorageMessageType rec)
        {   
           return string.Empty;
        }
   }
}

