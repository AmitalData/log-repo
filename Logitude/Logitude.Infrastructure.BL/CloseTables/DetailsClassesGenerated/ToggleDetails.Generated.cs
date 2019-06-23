
   
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
   public class ToggleDetails : Toggle, ICloseTable<Toggle, ToggleDetails>
   {
       public List<ToggleDetails> GetAll()
       {
		    var all = new List<ToggleDetails>();  
            all.Add(new ToggleDetails()
            {    
                Code = "TST", 
                Name = "Test Toggle", 
                SearchFields = "TST,Test Toggle", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Name = "EnableAzureRootFolder", 
                SearchFields = "EZR,EnableAzureRootFolder", 
                Code = "EZR", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "LEX", 
                Name = "LogBoxExport", 
                SearchFields = "LEX,LogBoxExport", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "URT", 
                Name = "Unicargo Report Toggle", 
                SearchFields = "URT,Unicargo Report Toggle", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "BDR", 
                Name = "Build Document Report Service", 
                SearchFields = "BDR,Build Document Report Service", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Name = "QuotationRoutingRatesQuotes", 
                Code = "QRR", 
                SearchFields = "QRR,QuotationRoutingRatesQuotes", 
			});
			
            return all;
       }

	    public void MapPoco(Toggle newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(Toggle rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

