
   
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
                Description = "Enable storing azure files under main root folder", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "LEX", 
                Name = "LogBoxExport", 
                SearchFields = "LEX,LogBoxExport", 
                Description = "Enable Export Shipment", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "URT", 
                Name = "Unicargo Report Toggle", 
                SearchFields = "URT,Unicargo Report Toggle,Activated a special report for Unicargo", 
                Description = "Activated a special report for Unicargo", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "FPG", 
                Name = "ARPayment Fetcha Pago", 
                SearchFields = "FPG,ARPayment Fetcha Pago", 
                Description = "Display Fetcha Pago in ARPayment  Screens", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "RRS", 
                Name = "Run Report on Secondary DB", 
                SearchFields = "RRS,Run Report on Secondary DB", 
                Description = "Running Report on Secondery DB", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "LV2", 
                Name = "LogGrid V2", 
                SearchFields = "LGV2,LogGrid V2", 
                Description = "Use Logitude Grid  version 2", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "SUB", 
                Name = "Shipment Sub Type", 
                SearchFields = "SUB,Shipment Sub Type,Enables users to manage the shipment sub-types,", 
                Description = "Enables users to manage the shipment sub-types", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "HRS", 
                Name = "Horse", 
                SearchFields = "HRS,Horse,Activates Horse Management in shipments", 
                Description = "Activates Horse Management in shipments", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "KPI", 
                Name = "KPI Document Fields", 
                SearchFields = "KPI,KPI Document Fields", 
                Description = "Display KPI documents fields in Bi report", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "STR", 
                SearchFields = "STR,Storage Pricing,Enables the storage invoicing mechanism", 
                Name = "Storage Pricing", 
                Description = "Enables the storage invoicing mechanism", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "OI2", 
                Name = "OceanInsightsV2", 
                SearchFields = "OI2,OceanInsightsV2", 
                Description = "Use Ocean insights tracking api version 2", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Name = "Shipment Warning Checkbox", 
                Code = "SWC", 
                SearchFields = "SWC,Shipment Warning Checkbox,Enables the credit limit new shipment warning", 
                Description = "Enables the credit limit new shipment warning", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "AMS", 
                Name = "AMS in Export", 
                SearchFields = "AMS,AMS in Export,Activates AMS customs transmission in export shipments", 
                Description = "Activates AMS customs transmission in export shipments", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "API", 
                SearchFields = "API,API Update,Allows tenants to update shipments via API", 
                Name = "API Update", 
                Description = "Allows tenants to update shipments via API", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Name = "Charges Types Direction Restrictions", 
                Code = "CTR", 
                SearchFields = "CTR,Charges Types Direction Restrictions,Charges Types Direction Restrictions", 
                Description = "Charges Types Direction Restrictions", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Name = "Branch Code in Counters", 
                SearchFields = "BCC,Branch Code in Counters,Branch Code in Counters", 
                Description = "Branch Code in Counters", 
                Code = "BCC", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "ACP", 
                SearchFields = "ACP,Accruals Approvement,Activates the accruals approve checkbox in shipment profit", 
                Name = "Accruals Approvement", 
                Description = "Activates the accruals approve checkbox in shipment profit", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "EQW", 
                Name = "Export Query Data Via WorkerRole", 
                Description = "Export Query Data to Excel Via WorkerRole", 
                SearchFields = "EQW,Export Query Data Via WorkerRole,Export Query Data to Excel Via WorkerRole", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "TAR", 
                Name = "All Tariffs", 
                SearchFields = "TAR,All Tariffs,Hide Tariffs Menu", 
                Description = "Hide Tariffs Menu", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "QMC", 
                Name = "Quote Multi Currency Mode", 
                SearchFields = "QMC,Quote Multi Currency Mode,Quote Multi Currency Mode", 
                Description = "Quote Multi Currency Mode", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "ADC", 
                Name = "Automation Document Copies", 
                SearchFields = "ADC,Automation Document Copies", 
                Description = "Automation Document Copies", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "DSR", 
                Name = "Documents Send Result in AP Invoices Automation", 
                Description = "Documents Send Result in AP Invoices Automation", 
                SearchFields = "DSR,Documents Send Result in AP Invoices Automation", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Name = "AWB Multiple Commodities", 
                Code = "AMC", 
                SearchFields = "AMC,AWB Multiple Commodities", 
                Description = "AWB Multiple Commodities", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "MAP", 
                Name = "Multipile AP Shipment", 
                SearchFields = "MAP,Multipile AP Shipment", 
                Description = "Multipile AP Shipment", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "CPH", 
                Name = "Consequent Pickup/Delivery", 
                SearchFields = "CPH,Consequent Pickup/Delivery", 
                Description = "Consequent Pickup/Delivery", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "PRE", 
                Name = "Master Pre/On Carriage", 
                SearchFields = "PRE,Master Pre/On Carriage", 
                Description = "Master Pre/On Carriage", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "BIF", 
                Name = "Invoices DWH", 
                SearchFields = "BIF,Invoices DWH,Allow Tenants to Show Invoice Fact on the BI Report Screen", 
                Description = "Allow Tenants to Show Invoice Fact on the BI Report Screen", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "SAS", 
                Name = "Standalone Shipment", 
                SearchFields = "SAS,Standalone Shipment", 
                Description = "Standalone Shipment", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Name = "Quotes Request Activated In Shared Logistic", 
                Code = "QRA", 
                SearchFields = "QRA, Quotes Request Activated In Shared Logistic", 
                Description = "Quotes Request Activated In Shared Logistic", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Name = "Ocean Insights Containers", 
                Code = "OIC", 
                SearchFields = "OIC,Ocean Insights Containers", 
                Description = "Ocean Insights Containers", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "CTL", 
                Name = "CollaborationTool", 
                SearchFields = "CTL,CollaborationTool", 
                Description = "Collaboration Tool", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "SDE", 
                Name = "Solve Duplicated Events Code", 
                SearchFields = "SDE,Duplicated Events code", 
                Description = "Solve Duplicated Events Code", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Name = "Lock Counter Procedure", 
                SearchFields = "LCP,Lock Counter Procedure", 
                Description = "Lock Counter Procedure", 
                Code = "LCP", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "PLE", 
                Name = "PL Export Shipments", 
                SearchFields = "PLE,PL Export Shipments,Allow to Create New Air Shipment From Private Label", 
                Description = "Allow to Create New Air Shipment From Private Label", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "CPT", 
                Name = "Carta Porte", 
                Description = "Carta Porte", 
                SearchFields = "CPT,Carta Porte", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "OPD", 
                Name = "Standalone Pickups/Deliveries Only ", 
                SearchFields = "OPD,Standalone Pickups/Deliveries Only ", 
                Description = "Only Allow Pickups/Deliveries to be Created with a Standalone Shipment", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "OIU", 
                Name = "Ocean Insights Shipment Update", 
                SearchFields = "OIU,Ocean Insights Shipment Update", 
                Description = "Ocean Insights Shipment Update", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "SFC", 
                Name = "Show file name as computed", 
                SearchFields = "Show file name as computed,SFC", 
                Description = "Show file name as computed", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "POD", 
                Name = "Convert POD Image to Pdf File", 
                SearchFields = "POD,Convert POD Image to Pdf File", 
                Description = "Convert POD Image to Pdf File", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "BAF", 
                Name = "ARInvoices DWH", 
                SearchFields = "BAF,ARInvoices DWH,Allow Tenants to Show ARInvoices Fact on the BI Report Screen", 
                Description = "Allow Tenants to Show ARInvoice Fact on the BI Report Screen", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "MLS", 
                Name = "Shipment Number In Generic Interface", 
                SearchFields = "MLS,Shipment Number In Generic Interface", 
                Description = "Sending shipment number for multiple AP invoice in generic interface", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "IDS", 
                Name = "InlandDomesticShipments DWH", 
                SearchFields = "IDS,InlandDomesticShipments DWH", 
                Description = "Allow Tenants to Show InlandDomesticShipments  Fact on the BI Report Screen", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Name = "Ocean Insights Sending Automatically Logic", 
                Code = "AOI", 
                SearchFields = "AOI,Ocean Insights Sending Automatically Logic", 
                Description = "Ocean Insights Sending Automatically Logic", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "SAL", 
                Name = "Use Security Access Level", 
                SearchFields = "SAL,Use Security Access Level", 
                Description = "Use Security Access Level", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Name = "Same User Login Enabled", 
                Description = "User can login to different environment at the same time", 
                Code = "ULE", 
                SearchFields = "ULE,Same User Login Enabled", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "AFO", 
                Name = "Advanced Filters Options", 
                SearchFields = "AFO,Advanced Filters Options,Enable New Option for Date Advanced Filters", 
                Description = "Enable New Option for Date Advanced Filters", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "QBT", 
                Name = "QBO Tax Id Calculations", 
                SearchFields = "QBT,QBO Tax Id Calculations", 
                Description = "QBO Tax Id Calculations", 
			});
			 
            all.Add(new ToggleDetails()
            {    
                Code = "AEV", 
                Name = "Automation Event Creation", 
                SearchFields = "AEV,Automation Event Creation", 
                Description = "Automation Event Creation", 
			});

            all.Add(new ToggleDetails()
            {    
                Code = "OPS", 
                Name = "Operational Status", 
                SearchFields = "OPS,Operational Status", 
                Description = "Operational Status", 
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

