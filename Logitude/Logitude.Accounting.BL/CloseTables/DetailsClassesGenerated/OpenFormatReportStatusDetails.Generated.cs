
   
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
   public class OpenFormatReportStatusDetails : OpenFormatReportStatus, ICloseTable<OpenFormatReportStatus, OpenFormatReportStatusDetails>
   {
       public List<OpenFormatReportStatusDetails> GetAll()
       {
		    var all = new List<OpenFormatReportStatusDetails>();  
            all.Add(new OpenFormatReportStatusDetails()
            {    
                Code = "1", 
                EnglishName = "Created", 
                LocalName = "נוצר", 
                SearchFields = "1,Created,נוצר", 
			});
			 
            all.Add(new OpenFormatReportStatusDetails()
            {    
                Code = "2", 
                EnglishName = "In Progress", 
                LocalName = "בתהליך", 
                SearchFields = "2,In Progress,בתהליך", 
			});
			 
            all.Add(new OpenFormatReportStatusDetails()
            {    
                Code = "3", 
                EnglishName = "Completed", 
                LocalName = "הושלם", 
                SearchFields = "3,Completed,הושלם", 
			});
			 
            all.Add(new OpenFormatReportStatusDetails()
            {    
                Code = "4", 
                EnglishName = "Failed", 
                LocalName = "נכשל", 
                SearchFields = "4,Failed,נכשל", 
			});
			
            return all;
       }

	    public void MapPoco(OpenFormatReportStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.EnglishName = this.EnglishName;  
		    newPoco.LocalName = this.LocalName;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(OpenFormatReportStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.EnglishName,",",rec.LocalName,",");
        }
   }
}

