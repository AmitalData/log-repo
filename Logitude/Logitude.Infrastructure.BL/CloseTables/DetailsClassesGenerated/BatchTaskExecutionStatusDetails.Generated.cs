
   
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
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs; 
using Logitude.Infrastructure.Data;

namespace Logitude.Infrastructure.BL
{
   public class BatchTaskExecutionStatusDetails : BatchTaskExecutionStatus, ICloseTable<BatchTaskExecutionStatus, BatchTaskExecutionStatusDetails>
   {
       public List<BatchTaskExecutionStatusDetails> GetAll()
       {
		    var all = new List<BatchTaskExecutionStatusDetails>();  
            all.Add(new BatchTaskExecutionStatusDetails()
            {    
                Code = "C", 
                Name = "Created", 
                SearchFields = "C,Created", 
			});
			 
            all.Add(new BatchTaskExecutionStatusDetails()
            {    
                Code = "I", 
                Name = "In Progress", 
                SearchFields = "I,In Progress", 
			});
			 
            all.Add(new BatchTaskExecutionStatusDetails()
            {    
                Code = "D", 
                Name = "Done", 
                SearchFields = "D,Done", 
			});
			 
            all.Add(new BatchTaskExecutionStatusDetails()
            {    
                Code = "F", 
                Name = "Failed", 
                SearchFields = "F,Failed", 
			});
			
            return all;
       }

	    public void MapPoco(BatchTaskExecutionStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(BatchTaskExecutionStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

