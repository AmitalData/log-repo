
   
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
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.EntityPMs; 
using Logitude.TariffModule.Data;

namespace Logitude.TariffModule.BL.CLoseTable
{
   public class TariffTypeDetails : TariffType, ICloseTable<TariffType, TariffTypeDetails>
   {
       public List<TariffTypeDetails> GetAll()
       {
		    var all = new List<TariffTypeDetails>();  
            all.Add(new TariffTypeDetails()
            {    
                TransportModeCode = "A", 
                Code = "AFC", 
                Name = "Air Freight Cost", 
                SearchFields = "AFC,Air Freight Cost", 
			});
			 
            all.Add(new TariffTypeDetails()
            {    
                TransportModeCode = "A", 
                Name = "Air Surcharges Cost", 
                Code = "ASC", 
                SearchFields = "ASC,Air Surcharges Cost", 
			});
			 
            all.Add(new TariffTypeDetails()
            {    
                TransportModeCode = "O", 
                Code = "OSC", 
                Name = "Ocean Surcharges Cost", 
                SearchFields = "OSC,Ocean Surcharges Cost", 
			});
			 
            all.Add(new TariffTypeDetails()
            {    
                TransportModeCode = "O", 
                Name = "Ocean LCL Freight Cost", 
                Code = "OLC", 
                SearchFields = "OLC,Ocean LCL Freight Cost", 
			});
			 
            all.Add(new TariffTypeDetails()
            {    
                TransportModeCode = "O", 
                Name = "Ocean FCL Freight Cost", 
                Code = "OFC", 
                SearchFields = "OFC,Ocean FCL Freight Cost", 
			});
			 
            all.Add(new TariffTypeDetails()
            {    
                TransportModeCode = "O", 
                Name = "Ocean FCL Surcharges Cost", 
                Code = "OFS", 
                SearchFields = "OFS,Ocean FCL Surcharges Cost", 
			});
			 
            all.Add(new TariffTypeDetails()
            {    
                DirectionCode = "E", 
                Name = "Export Customs Charges Cost", 
                Code = "ECC", 
                SearchFields = "ECC,Export Customs Charges Cost", 
			});
			 
            all.Add(new TariffTypeDetails()
            {    
                DirectionCode = "I", 
                Name = "Import Customs Charges Cost", 
                Code = "ICC", 
                SearchFields = "ICC,Import Customs Charges Cost", 
			});
			 
            all.Add(new TariffTypeDetails()
            {    
                DirectionCode = "", 
                Name = "Inland FTL Charges Cost", 
                Code = "IFT", 
                SearchFields = "IFT,Inland FTL Charges Cost", 
                TransportModeCode = "I", 
			});
			 
            all.Add(new TariffTypeDetails()
            {    
                Code = "ICS", 
                Name = "Import Local Charges Sale", 
                SearchFields = "ICS,Import Local Charges Sale", 
                DirectionCode = "I"
			});
			 
            all.Add(new TariffTypeDetails()
            {    
                Code = "ECS", 
                Name = "Export Local Charges Sale", 
                SearchFields = "ECS,Export Local Charges Sale", 
                DirectionCode = "E"
			});
			
            return all;
       }

	    public void MapPoco(TariffType newPoco)
        {   
		    newPoco.TransportModeCode = this.TransportModeCode;  
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(TariffType rec)
        {   
           return String.Concat(rec.TransportModeCode,",",rec.Code,",",rec.Name,",");
        }
   }
}

