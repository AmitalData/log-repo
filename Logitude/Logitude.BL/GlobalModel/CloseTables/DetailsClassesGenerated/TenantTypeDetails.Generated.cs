

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
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.GlobalModel.EntityPMs; 
using Simplog.Global.Data.GlobalModel;

namespace Logitude.BL.GlobalModel
{
   public class TenantTypeDetails : TenantType, ICloseTable<TenantType, TenantTypeDetails>
   {
       public List<TenantTypeDetails> GetAll()
       {
		    var all = new List<TenantTypeDetails>();  
            all.Add(new TenantTypeDetails()
            {    
                Code = "AIR", 
                SearchFields = "AIR,Airlines", 
                Name = "Airlines", 
			});
			 
            all.Add(new TenantTypeDetails()
            {    
                Code = "CRM", 
                SearchFields = "CRM,CRM", 
                Name = "CRM", 
			});
			 
            all.Add(new TenantTypeDetails()
            {    
                Code = "CUT", 
                SearchFields = "CUT,Customs", 
                Name = "Customs", 
			});
			 
            all.Add(new TenantTypeDetails()
            {    
                Code = "FOR", 
                SearchFields = "FOR,Forwarder", 
                Name = "Forwarder", 
			});
			 
            all.Add(new TenantTypeDetails()
            {    
                Code = "SHC", 
                SearchFields = "SHC,Shipper/Consignee", 
                Name = "Shipper/Consignee", 
			});
			
            return all;
       }

	    public void MapPoco(TenantType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(TenantType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

