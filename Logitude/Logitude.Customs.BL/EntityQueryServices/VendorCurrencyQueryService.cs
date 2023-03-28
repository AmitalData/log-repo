 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Customs.BL.EntityQueryServices
{ 
   public partial class VendorCurrencyQueryService: EntityQueryService<VendorCurrency,VendorCurrencyKeys,VendorCurrencyPM,object,VendorCurrencyKeys>
   {
   
       
	    public  List<VendorCurrency> GetVendorCurrencyByVendorId(int tenant,string vendorId)
        {
          return repository.GetVendorCurrencyByVendorId(tenant, vendorId);
        }
     
	 
   }
   
}
	 