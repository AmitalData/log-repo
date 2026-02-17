
   
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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL
{
   public class IntegrityCheckStatusDetails : IntegrityCheckStatus, ICloseTable<IntegrityCheckStatus, IntegrityCheckStatusDetails>
   {
       public List<IntegrityCheckStatusDetails> GetAll()
       {
		    var all = new List<IntegrityCheckStatusDetails>();  
            all.Add(new IntegrityCheckStatusDetails()
            {    
                Code = "1", 
                Name = "Created", 
			});
			 
            all.Add(new IntegrityCheckStatusDetails()
            {    
                Code = "2", 
                Name = "In Progress", 
			});
			 
            all.Add(new IntegrityCheckStatusDetails()
            {    
                Code = "3", 
                Name = "Check Completed", 
			});
			 
            all.Add(new IntegrityCheckStatusDetails()
            {    
                Code = "4", 
                Name = "Fix Completed", 
			});
			 
            all.Add(new IntegrityCheckStatusDetails()
            {    
                Code = "5", 
                Name = "Failed", 
			});
			
            return all;
       }

	    public void MapPoco(IntegrityCheckStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(IntegrityCheckStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

