

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
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs; 
using Simplog.Data.InfrastructureModel;

namespace Logitude.BL.InfrastructureModel
{
   public class EntityStatusTypeDetails : EntityStatusType, ICloseTable<EntityStatusType, EntityStatusTypeDetails>
   {
       public List<EntityStatusTypeDetails> GetAll()
       {
		    var all = new List<EntityStatusTypeDetails>();  
            all.Add(new EntityStatusTypeDetails()
            {    
                Name = "Operational", 
                Code = "O", 
                SearchFields = "O,Operational", 
			});
			 
            all.Add(new EntityStatusTypeDetails()
            {    
                Name = "Physical", 
                Code = "P", 
                SearchFields = "P,Physical", 
			});
			 
            all.Add(new EntityStatusTypeDetails()
            {    
                Name = "Billing", 
                Code = "B", 
                SearchFields = "B,Billing", 
			});
			
            return all;
       }

	    public void MapPoco(EntityStatusType newPoco)
        {   
		    newPoco.Name = this.Name;  
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(EntityStatusType rec)
        {   
           return String.Concat(rec.Name,",",rec.Code,",");
        }
   }
}

