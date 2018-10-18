
   
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
   public class TicketCreatedByTypeDetails : TicketCreatedByType, ICloseTable<TicketCreatedByType, TicketCreatedByTypeDetails>
   {
       public List<TicketCreatedByTypeDetails> GetAll()
       {
		    var all = new List<TicketCreatedByTypeDetails>();  
            all.Add(new TicketCreatedByTypeDetails()
            {    
                Code = "CUS", 
                Name = "Customer", 
                SearchFields = "Customer,CUS,", 
			});
			 
            all.Add(new TicketCreatedByTypeDetails()
            {    
                Code = "INU", 
                Name = "Internal User", 
                SearchFields = "Internal User,INU,", 
			});
			
            return all;
       }

	    public void MapPoco(TicketCreatedByType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(TicketCreatedByType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

