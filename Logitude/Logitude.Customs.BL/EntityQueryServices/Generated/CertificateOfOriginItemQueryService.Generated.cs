 
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
   public partial class CertificateOfOriginItemQueryService: EntityQueryService<CertificateOfOriginItem,CertificateOfOriginItemKeys,CertificateOfOriginItemPM,CertificateOfOriginInvoicePM,CertificateOfOriginInvoiceKeys>
   {
   
        CertificateOfOriginItemRepository repository;
		ICustomContext  context;
        public CertificateOfOriginItemQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CertificateOfOriginItemRepository(context);
            Repository = repository;
            mapping = new CertificateOfOriginItemDataMapping();
        }

        public CertificateOfOriginItemQueryService(CertificateOfOriginItemRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CertificateOfOriginItemDataMapping();
        }

        public CertificateOfOriginItemQueryService(ICustomContext context)
        {
            this.repository = new CertificateOfOriginItemRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CertificateOfOriginItemDataMapping();
        }
		 
		public  CertificateOfOriginItemPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CertificateOfOriginItemKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CertificateOfOriginItem entityPOCO)
        {
            CertificateOfOriginItemKeys entityKeys = new CertificateOfOriginItemKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 