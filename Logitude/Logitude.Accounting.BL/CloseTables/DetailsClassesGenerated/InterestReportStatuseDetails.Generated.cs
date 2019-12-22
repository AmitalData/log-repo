
   
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
   public class InterestReportStatuseDetails : InterestReportStatuse, ICloseTable<InterestReportStatuse, InterestReportStatuseDetails>
   {
       public List<InterestReportStatuseDetails> GetAll()
       {
		    var all = new List<InterestReportStatuseDetails>();  
            all.Add(new InterestReportStatuseDetails()
            {    
                Code = "1", 
                EnglishName = "Draft", 
                SearchFields = "1,Draft,טיוטה", 
                LocalName = "טיוטה", 
			});
			 
            all.Add(new InterestReportStatuseDetails()
            {    
                Code = "2", 
                EnglishName = "Invoice", 
                SearchFields = "2,Invoice,הופקה חשבונית", 
                LocalName = "הופקה חשבונית", 
			});
			 
            all.Add(new InterestReportStatuseDetails()
            {    
                Code = "3", 
                EnglishName = "Cancelled", 
                SearchFields = "3,Cancelled,בוטל", 
                LocalName = "בוטל", 
			});
			
            return all;
       }

	    public void MapPoco(InterestReportStatuse newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.EnglishName = this.EnglishName;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(InterestReportStatuse rec)
        {   
           return String.Concat(rec.Code,",",rec.EnglishName,",",rec.LocalName,",");
        }
   }
}

