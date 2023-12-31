 
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
   public partial class CertificateOfOriginInvoiceQueryService: EntityQueryService<CertificateOfOriginInvoice,CertificateOfOriginInvoiceKeys,CertificateOfOriginInvoicePM,object,CertificateOfOriginInvoiceKeys>
   {
   
        CertificateOfOriginInvoiceRepository repository;
		ICustomContext  context;
        public CertificateOfOriginInvoiceQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CertificateOfOriginInvoiceRepository(context);
            Repository = repository;
            mapping = new CertificateOfOriginInvoiceDataMapping();
        }

        public CertificateOfOriginInvoiceQueryService(CertificateOfOriginInvoiceRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CertificateOfOriginInvoiceDataMapping();
        }

        public CertificateOfOriginInvoiceQueryService(ICustomContext context)
        {
            this.repository = new CertificateOfOriginInvoiceRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CertificateOfOriginInvoiceDataMapping();
        }
		 
		public  CertificateOfOriginInvoicePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CertificateOfOriginInvoiceKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CertificateOfOriginInvoice entityPOCO)
        {
            CertificateOfOriginInvoiceKeys entityKeys = new CertificateOfOriginInvoiceKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 