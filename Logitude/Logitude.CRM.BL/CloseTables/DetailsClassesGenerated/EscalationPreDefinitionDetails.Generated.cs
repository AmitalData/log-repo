
   
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
   public class EscalationPreDefinitionDetails : EscalationPreDefinition, ICloseTable<EscalationPreDefinition, EscalationPreDefinitionDetails>
   {
       public List<EscalationPreDefinitionDetails> GetAll()
       {
		    var all = new List<EscalationPreDefinitionDetails>();  
            all.Add(new EscalationPreDefinitionDetails()
            {    
                Code = "CM", 
                Name = "Classification Manager ", 
                SearchFields = "Classification Manager ,CM,", 
			});
			 
            all.Add(new EscalationPreDefinitionDetails()
            {    
                Code = "CN", 
                Name = "Classification Notify", 
                SearchFields = "Classification Notify,CN,", 
			});
			 
            all.Add(new EscalationPreDefinitionDetails()
            {    
                Code = "GM", 
                Name = "Group Manager", 
                SearchFields = "Group Manager,GM,", 
			});
			 
            all.Add(new EscalationPreDefinitionDetails()
            {    
                Code = "GN", 
                Name = "Group Notify", 
                SearchFields = "Group Notify,GN,", 
			});
			 
            all.Add(new EscalationPreDefinitionDetails()
            {    
                Code = "OW", 
                Name = "Owner", 
                SearchFields = "Owner,OW,", 
			});
			
            return all;
       }

	    public void MapPoco(EscalationPreDefinition newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(EscalationPreDefinition rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

