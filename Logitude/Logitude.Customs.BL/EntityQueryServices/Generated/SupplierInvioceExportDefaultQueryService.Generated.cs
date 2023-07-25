 
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
   public partial class SupplierInvioceExportDefaultQueryService: EntityQueryService<SupplierInvioceExportDefault,SupplierInvioceExportDefaultKeys,SupplierInvioceExportDefaultPM,object,SupplierInvioceExportDefaultKeys>
   {
   
        SupplierInvioceExportDefaultRepository repository;
		ICustomContext  context;
        public SupplierInvioceExportDefaultQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SupplierInvioceExportDefaultRepository(context);
            Repository = repository;
            mapping = new SupplierInvioceExportDefaultDataMapping();
        }

        public SupplierInvioceExportDefaultQueryService(SupplierInvioceExportDefaultRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SupplierInvioceExportDefaultDataMapping();
        }

        public SupplierInvioceExportDefaultQueryService(ICustomContext context)
        {
            this.repository = new SupplierInvioceExportDefaultRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SupplierInvioceExportDefaultDataMapping();
        }
		 
		public  SupplierInvioceExportDefaultPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SupplierInvioceExportDefaultKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SupplierInvioceExportDefault entityPOCO)
        {
            SupplierInvioceExportDefaultKeys entityKeys = new SupplierInvioceExportDefaultKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 