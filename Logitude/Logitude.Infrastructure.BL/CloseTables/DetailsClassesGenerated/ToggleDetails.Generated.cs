
   
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
                Description = "Test Toggle", 
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
                Code = "RRW", 
                Name = "Run Report Via WorkerRole", 
                SearchFields = "RRW,Run Report Via WorkerRole", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "FPG", 
                Name = "ARPayment Fetcha Pago", 
                SearchFields = "FPG,ARPayment Fetcha Pago", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "TJC", 
                Name = "Ticket Jumping Counter", 
                SearchFields = "TJC,Ticket Jumping Counter", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "BDW", 
                Name = "Build Document Via WorkerRole", 
                SearchFields = "BDW,Build Document Via WorkerRole", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "RRS", 
                Name = "Run Report on Secondary DB", 
                SearchFields = "RRS,Run Report on Secondary DB", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "LV2", 
                Name = "LogGrid V2", 
                SearchFields = "LGV2,LogGrid V2", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Name = "Card Searchs Toggle", 
                Code = "CST", 
                SearchFields = "CST,Card Searchs Toggle", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "SUB", 
                Name = "Shipment Sub Type", 
                SearchFields = "SUB,Shipment Sub Type", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "HRS", 
                Name = "Horse", 
                SearchFields = "HRS,Horse", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "KPI", 
                Name = "KPI Document Fields", 
                SearchFields = "KPI,KPI Document Fields", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "STR", 
                SearchFields = "STR,Storage Pricing", 
                Name = "Storage Pricing", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "OI2", 
                Name = "OceanInsightsV2", 
                SearchFields = "OI2,OceanInsightsV2", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Name = "Shipment Warning Checkbox", 
                Code = "SWC", 
                SearchFields = "SWC,Shipment Warning Checkbox", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "AMS", 
                Name = "AMS in Export", 
                SearchFields = "AMS,AMS in Export", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "API", 
                SearchFields = "API,API Update", 
                Name = "API Update", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "LIC", 
                Name = "License Management", 
                SearchFields = "LIC,License Management", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Name = "CRM Customer Quick Search", 
                Code = "CQS", 
                SearchFields = "CQS,CRM Customer Quick Search", 
                Description = "CRM Customer Quick Search", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Name = "Charges Types Direction Restrictions", 
                Code = "CTR", 
                SearchFields = "CTR,Charges Types Direction Restrictions", 
                Description = "Charges Types Direction Restrictions", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Name = "Branch Code in Counters", 
                SearchFields = "Branch Code in Counters", 
                Description = "Branch Code in Counters", 
                Code = "BCC", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "ACP", 
                SearchFields = "ACP,Accruals Approvement", 
                Name = "Accruals Approvement", 
                Description = "Accruals Approvement Toggle", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "EQW", 
                Name = "Export Query Data Via WorkerRole", 
                Description = "Export Query Data to Excel Via WorkerRole", 
                SearchFields = "Export Query Data Via WorkerRole", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "QMC", 
                Name = "Quote Multi Currency Mode", 
                SearchFields = "QMC, Quote Multi Currency Mode", 
                Description = "Quote Multi Currency Mode", 
			});
			
            return all;
       }

	    public void MapPoco(Toggle newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Description = this.Description;   
        }

		public string GetSearchFields(Toggle rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",",rec.Description,",");
        }
   }
}

