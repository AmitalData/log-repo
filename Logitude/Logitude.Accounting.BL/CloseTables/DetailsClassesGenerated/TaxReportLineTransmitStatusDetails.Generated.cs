
   
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
   public class TaxReportLineTransmitStatusDetails : TaxReportLineTransmitStatus, ICloseTable<TaxReportLineTransmitStatus, TaxReportLineTransmitStatusDetails>
   {
       public List<TaxReportLineTransmitStatusDetails> GetAll()
       {
		    var all = new List<TaxReportLineTransmitStatusDetails>();  
            all.Add(new TaxReportLineTransmitStatusDetails()
            {    
                Code = "1", 
                EnglishName = "For transmit", 
                LocalName = "לשידור", 
                SearchFields = "1,For transmit,לשידור", 
			});
			 
            all.Add(new TaxReportLineTransmitStatusDetails()
            {    
                Code = "2", 
                EnglishName = "Not for transmit for this report", 
                LocalName = "לא לשידור בדוח הזה", 
                SearchFields = "2,Not for transmit for this report,לא לשידור בדוח הזה", 
			});
			 
            all.Add(new TaxReportLineTransmitStatusDetails()
            {    
                Code = "3", 
                EnglishName = "Not for transmit at all", 
                LocalName = "לא לשידור בכלל", 
                SearchFields = "3,Not for transmit at all,לא לשידור בכלל", 
			});
			 
            all.Add(new TaxReportLineTransmitStatusDetails()
            {    
                Code = "0", 
                EnglishName = "Without Transmit", 
                LocalName = "ללא סטטוס העברה", 
                SearchFields = "0,ללא סטטוס העברה,Without Transmit", 
			});
			
            return all;
       }

	    public void MapPoco(TaxReportLineTransmitStatus newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.EnglishName = this.EnglishName;  
		    newPoco.LocalName = this.LocalName;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(TaxReportLineTransmitStatus rec)
        {   
           return String.Concat(rec.Code,",",rec.EnglishName,",",rec.LocalName,",");
        }
   }
}

