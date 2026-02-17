 
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
   public partial class PayerTypeQueryService: EntityQueryService<PayerType,PayerTypeKeys,PayerTypePM,object,PayerTypeKeys>
   {
   
        PayerTypeRepository repository;
		ICustomContext  context;
        public PayerTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new PayerTypeRepository(context);
            Repository = repository;
            mapping = new PayerTypeDataMapping();
        }

        public PayerTypeQueryService(PayerTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PayerTypeDataMapping();
        }

        public PayerTypeQueryService(ICustomContext context)
        {
            this.repository = new PayerTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PayerTypeDataMapping();
        }
		 
		public  PayerTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PayerTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PayerType entityPOCO)
        {
            PayerTypeKeys entityKeys = new PayerTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 