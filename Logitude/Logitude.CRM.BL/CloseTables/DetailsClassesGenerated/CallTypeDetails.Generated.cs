
   
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
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs; 
using Logitude.CRM.Data;

namespace Logitude.CRM.BL.CLoseTable
{
   public class CallTypeDetails : CallType, ICloseTable<CallType, CallTypeDetails>
   {
       public List<CallTypeDetails> GetAll()
       {
		    var all = new List<CallTypeDetails>();  
            all.Add(new CallTypeDetails()
            {    
                Code = "I", 
                SearchFields = "I,Incoming", 
                Name = "Incoming", 
			});
			 
            all.Add(new CallTypeDetails()
            {    
                Code = "O", 
                SearchFields = "O,Outgoing", 
                Name = "Outgoing", 
			});
			
            return all;
       }

	    public void MapPoco(CallType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(CallType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

