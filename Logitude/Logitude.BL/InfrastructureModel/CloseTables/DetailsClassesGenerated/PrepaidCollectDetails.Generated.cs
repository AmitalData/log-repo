

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
   public class PrepaidCollectDetails : PrepaidCollect, ICloseTable<PrepaidCollect, PrepaidCollectDetails>
   {
       public List<PrepaidCollectDetails> GetAll()
       {
		    var all = new List<PrepaidCollectDetails>();  
            all.Add(new PrepaidCollectDetails()
            {    
                SearchFields = "both,b", 
                Id = "B", 
                DisplayInLOV = false, 
                Name = "Both", 
			});
			 
            all.Add(new PrepaidCollectDetails()
            {    
                SearchFields = "collect,c", 
                Id = "C", 
                DisplayInLOV = true, 
                Name = "Collect", 
			});
			 
            all.Add(new PrepaidCollectDetails()
            {    
                SearchFields = "prepaid,p", 
                Id = "P", 
                DisplayInLOV = true, 
                Name = "Prepaid", 
			});
			
            return all;
       }

	    public void MapPoco(PrepaidCollect newPoco)
        {   
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Id = this.Id;  
		    newPoco.DisplayInLOV = this.DisplayInLOV;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(PrepaidCollect rec)
        {   
           return String.Concat(rec.Id,",",rec.DisplayInLOV,",",rec.Name,",");
        }
		public string Code
        {
            get
            {
                return this.Id;
            }
            set
            {
                this.Id = value;
            }
        }
   }
}

