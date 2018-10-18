
   
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
   public class TicketSourceDetails : TicketSource, ICloseTable<TicketSource, TicketSourceDetails>
   {
       public List<TicketSourceDetails> GetAll()
       {
		    var all = new List<TicketSourceDetails>();  
            all.Add(new TicketSourceDetails()
            {    
                Code = "EXS", 
                Name = "External System", 
                SearchFields = "External System,EXS,", 
			});
			 
            all.Add(new TicketSourceDetails()
            {    
                Code = "LOG", 
                Name = "Logitude", 
                SearchFields = "Logitude,LOG,", 
			});
			 
            all.Add(new TicketSourceDetails()
            {    
                Code = "MAL", 
                Name = "Mail", 
                SearchFields = "Mail,MAL,", 
			});
			
            return all;
       }

	    public void MapPoco(TicketSource newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(TicketSource rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

