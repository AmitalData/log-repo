
   
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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL
{
   public class ImporterTypeForClaimDetails : ImporterTypeForClaim, ICloseTable<ImporterTypeForClaim, ImporterTypeForClaimDetails>
   {
       public List<ImporterTypeForClaimDetails> GetAll()
       {
		    var all = new List<ImporterTypeForClaimDetails>();  
            all.Add(new ImporterTypeForClaimDetails()
            {    
                Code = "1", 
                LocalName = "עולה חדש", 
                Inactive = false, 
                SearchFields = "1,עולה חדש", 
			});
			 
            all.Add(new ImporterTypeForClaimDetails()
            {    
                Code = "2", 
                LocalName = "תושב חוזר", 
                Inactive = false, 
                SearchFields = "2,תושב חוזר", 
			});
			 
            all.Add(new ImporterTypeForClaimDetails()
            {    
                Code = "3", 
                LocalName = "תייר", 
                Inactive = false, 
                SearchFields = "3,תייר", 
			});
			 
            all.Add(new ImporterTypeForClaimDetails()
            {    
                Code = "4", 
                LocalName = "שגרירות", 
                Inactive = false, 
                SearchFields = "4,שגרירות", 
			});
			 
            all.Add(new ImporterTypeForClaimDetails()
            {    
                Code = "5", 
                LocalName = "חייל", 
                Inactive = false, 
                SearchFields = "5,חייל", 
			});
			 
            all.Add(new ImporterTypeForClaimDetails()
            {    
                Code = "6", 
                LocalName = "יבוא אישי", 
                Inactive = false, 
                SearchFields = "6,יבוא אישי", 
			});
			 
            all.Add(new ImporterTypeForClaimDetails()
            {    
                Code = "7", 
                LocalName = "סוכן מכס", 
                Inactive = false, 
                SearchFields = "7,סוכן מכס", 
			});
			 
            all.Add(new ImporterTypeForClaimDetails()
            {    
                Code = "8", 
                LocalName = "יבואן מסחרי", 
                Inactive = false, 
                SearchFields = "8,יבואן מסחרי", 
			});
			 
            all.Add(new ImporterTypeForClaimDetails()
            {    
                Code = "9", 
                LocalName = "מלכ''ר", 
                Inactive = false, 
                SearchFields = "9,מלכ''ר", 
			});
			
            return all;
       }

	    public void MapPoco(ImporterTypeForClaim newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.LocalName = this.LocalName;  
		    newPoco.Inactive = this.Inactive;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(ImporterTypeForClaim rec)
        {   
           return String.Concat(rec.Code,",",rec.LocalName,",",rec.Inactive,",");
        }
   }
}

