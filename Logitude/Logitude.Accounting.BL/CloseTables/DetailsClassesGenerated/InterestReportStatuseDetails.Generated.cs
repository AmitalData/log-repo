
   
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
                LocalName = "טיוטה", 
                EnglishName = "Draft", 
                Code = "1", 
                SearchFields = "1,Draft,טיוטה", 
			});
			 
            all.Add(new InterestReportStatuseDetails()
            {    
                LocalName = "הופקה חשבונית", 
                EnglishName = "Invoice", 
                Code = "2", 
                SearchFields = "2,Invoice,הופקה חשבונית", 
			});
			 
            all.Add(new InterestReportStatuseDetails()
            {    
                LocalName = "בוטל", 
                EnglishName = "Cancelled", 
                Code = "3", 
                SearchFields = "3,Cancelled,בוטל", 
			});
			
            return all;
       }

	    public void MapPoco(InterestReportStatuse newPoco)
        {   
		    newPoco.LocalName = this.LocalName;  
		    newPoco.EnglishName = this.EnglishName;  
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(InterestReportStatuse rec)
        {   
           return String.Concat(rec.LocalName,",",rec.EnglishName,",",rec.Code,",");
        }
   }
}

