
   
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
   public class TaxReportStatusDetails : TaxReportStatus, ICloseTable<TaxReportStatus, TaxReportStatusDetails>
   {
       public List<TaxReportStatusDetails> GetAll()
       {
		    var all = new List<TaxReportStatusDetails>();  
            all.Add(new TaxReportStatusDetails()
            {    
                Code = "1", 
                Name = "Sent", 
                LocalName = "דווח", 
                SearchFields = "1,Sent,דווח", 
			});
			 
            all.Add(new TaxReportStatusDetails()
            {    
                Code = "2", 
                Name = "Not fro sending", 
                LocalName = "לא לדיווח", 
                SearchFields = "2,Not for sending,לא לדיווח", 
			});
			 
            all.Add(new TaxReportStatusDetails()
            {    
                Code = "3", 
                Name = "Not sent", 
                LocalName = "לא דווח", 
                SearchFields = "3,Not sent,לא דווח", 
			});
			
            return all;
       }

	    public void MapPoco(TaxReportStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
		    newPoco.LocalName = this.LocalName;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(TaxReportStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",",rec.LocalName,",");
        }
   }
}

