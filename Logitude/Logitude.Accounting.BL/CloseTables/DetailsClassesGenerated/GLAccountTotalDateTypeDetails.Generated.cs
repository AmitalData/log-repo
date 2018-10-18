
   
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
   public class GLAccountTotalDateTypeDetails : GLAccountTotalDateType, ICloseTable<GLAccountTotalDateType, GLAccountTotalDateTypeDetails>
   {
       public List<GLAccountTotalDateTypeDetails> GetAll()
       {
		    var all = new List<GLAccountTotalDateTypeDetails>();  
            all.Add(new GLAccountTotalDateTypeDetails()
            {    
                Code = "1", 
                Name = "Accounting date", 
			});
			 
            all.Add(new GLAccountTotalDateTypeDetails()
            {    
                Code = "2", 
                Name = "Due Date", 
			});
			 
            all.Add(new GLAccountTotalDateTypeDetails()
            {    
                Code = "3", 
                Name = "Document Date", 
			});
			
            return all;
       }

	    public void MapPoco(GLAccountTotalDateType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(GLAccountTotalDateType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

