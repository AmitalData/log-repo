
   
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
   public class GLAccountTypeDetails : GLAccountType, ICloseTable<GLAccountType, GLAccountTypeDetails>
   {
       public List<GLAccountTypeDetails> GetAll()
       {
		    var all = new List<GLAccountTypeDetails>();  
            all.Add(new GLAccountTypeDetails()
            {    
                Code = "1", 
                Inactive = false, 
                LocalName = "כרטיס", 
                EnglishName = "Card", 
			});
			 
            all.Add(new GLAccountTypeDetails()
            {    
                Code = "2", 
                Inactive = false, 
                LocalName = "לקוח", 
                EnglishName = "Client", 
			});
			 
            all.Add(new GLAccountTypeDetails()
            {    
                Code = "3", 
                Inactive = false, 
                LocalName = "ספק", 
                EnglishName = "Vendor", 
			});
			 
            all.Add(new GLAccountTypeDetails()
            {    
                Code = "4", 
                Inactive = false, 
                LocalName = "ג'וב", 
                EnglishName = "Job", 
			});
			 
            all.Add(new GLAccountTypeDetails()
            {    
                Code = "5", 
                Inactive = false, 
                LocalName = "תיק", 
                EnglishName = "File", 
			});
			
            return all;
       }

	    public void MapPoco(GLAccountType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;  
		    newPoco.EnglishName = this.EnglishName;   
        }

		public string GetSearchFields(GLAccountType rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",",rec.EnglishName,",");
        }
   }
}

