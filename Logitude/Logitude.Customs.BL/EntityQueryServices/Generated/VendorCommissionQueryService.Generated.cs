 
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
   public partial class VendorCommissionQueryService: EntityQueryService<VendorCommission,VendorCommissionKeys,VendorCommissionPM,object,VendorCommissionKeys>
   {
   
        VendorCommissionRepository repository;
		ICustomContext  context;
        public VendorCommissionQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new VendorCommissionRepository(context);
            Repository = repository;
            mapping = new VendorCommissionDataMapping();
        }

        public VendorCommissionQueryService(VendorCommissionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new VendorCommissionDataMapping();
        }

        public VendorCommissionQueryService(ICustomContext context)
        {
            this.repository = new VendorCommissionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new VendorCommissionDataMapping();
        }
		 
		public  VendorCommissionPM GetSingle(string vendorid, string customerid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new VendorCommissionKeys(){ VendorId = vendorid, CustomerId = customerid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(VendorCommission entityPOCO)
        {
            VendorCommissionKeys entityKeys = new VendorCommissionKeys() { VendorId = entityPOCO.VendorId, CustomerId = entityPOCO.CustomerId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 