 
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
   public partial class SupplierInvioceItemCertificatDefaultQueryService: EntityQueryService<SupplierInvioceItemCertificatDefault,SupplierInvioceItemCertificatDefaultKeys,SupplierInvioceItemCertificatDefaultPM,SupplierInvioceExportDefaultPM,SupplierInvioceExportDefaultKeys>
   {
   
        SupplierInvioceItemCertificatDefaultRepository repository;
		ICustomContext  context;
        public SupplierInvioceItemCertificatDefaultQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SupplierInvioceItemCertificatDefaultRepository(context);
            Repository = repository;
            mapping = new SupplierInvioceItemCertificatDefaultDataMapping();
        }

        public SupplierInvioceItemCertificatDefaultQueryService(SupplierInvioceItemCertificatDefaultRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SupplierInvioceItemCertificatDefaultDataMapping();
        }

        public SupplierInvioceItemCertificatDefaultQueryService(ICustomContext context)
        {
            this.repository = new SupplierInvioceItemCertificatDefaultRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SupplierInvioceItemCertificatDefaultDataMapping();
        }
		 
		public  SupplierInvioceItemCertificatDefaultPM GetSingle(string supplierinvioceexportdefaultid, int sequencenumeric,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SupplierInvioceItemCertificatDefaultKeys(){ SupplierInvioceExportDefaultId = supplierinvioceexportdefaultid, SequenceNumeric = sequencenumeric };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SupplierInvioceItemCertificatDefault entityPOCO)
        {
            SupplierInvioceItemCertificatDefaultKeys entityKeys = new SupplierInvioceItemCertificatDefaultKeys() { SupplierInvioceExportDefaultId = entityPOCO.SupplierInvioceExportDefaultId, SequenceNumeric = entityPOCO.SequenceNumeric,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 