
   
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
   public class RequestToAdvanceAQueueDetails : RequestToAdvanceAQueue, ICloseTable<RequestToAdvanceAQueue, RequestToAdvanceAQueueDetails>
   {
       public List<RequestToAdvanceAQueueDetails> GetAll()
       {
		    var all = new List<RequestToAdvanceAQueueDetails>(); 
            return all;
       }

	    public void MapPoco(RequestToAdvanceAQueue newPoco)
        {    
        }

		public string GetSearchFields(RequestToAdvanceAQueue rec)
        {   
           return string.Empty;
        }
   }
}

