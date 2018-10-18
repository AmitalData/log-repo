
   
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
   public class EscalationActionTimeIndicatorDetails : EscalationActionTimeIndicator, ICloseTable<EscalationActionTimeIndicator, EscalationActionTimeIndicatorDetails>
   {
       public List<EscalationActionTimeIndicatorDetails> GetAll()
       {
		    var all = new List<EscalationActionTimeIndicatorDetails>();  
            all.Add(new EscalationActionTimeIndicatorDetails()
            {    
                Code = "AF", 
                Name = "After", 
                SearchFields = "After,AF,", 
			});
			 
            all.Add(new EscalationActionTimeIndicatorDetails()
            {    
                Code = "BF", 
                Name = "Before", 
                SearchFields = "Before,BF,", 
			});
			 
            all.Add(new EscalationActionTimeIndicatorDetails()
            {    
                Code = "IM", 
                Name = "Immediately", 
                SearchFields = "Immediately,IM,", 
			});
			
            return all;
       }

	    public void MapPoco(EscalationActionTimeIndicator newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(EscalationActionTimeIndicator rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

