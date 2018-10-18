 
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
   public partial class InvoiceTypeQueryService: EntityQueryService<InvoiceType,InvoiceTypeKeys,InvoiceTypePM,object,InvoiceTypeKeys>
   {
   
        InvoiceTypeRepository repository;
		ICustomContext  context;
        public InvoiceTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new InvoiceTypeRepository(context);
            Repository = repository;
            mapping = new InvoiceTypeDataMapping();
        }

        public InvoiceTypeQueryService(InvoiceTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new InvoiceTypeDataMapping();
        }

        public InvoiceTypeQueryService(ICustomContext context)
        {
            this.repository = new InvoiceTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new InvoiceTypeDataMapping();
        }
		 
		public  InvoiceTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new InvoiceTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(InvoiceType entityPOCO)
        {
            InvoiceTypeKeys entityKeys = new InvoiceTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 