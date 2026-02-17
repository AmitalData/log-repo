 
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
   public partial class SalesTaxExemptionTypeQueryService: EntityQueryService<SalesTaxExemptionType,SalesTaxExemptionTypeKeys,SalesTaxExemptionTypePM,object,SalesTaxExemptionTypeKeys>
   {
   
        SalesTaxExemptionTypeRepository repository;
		ICustomContext  context;
        public SalesTaxExemptionTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SalesTaxExemptionTypeRepository(context);
            Repository = repository;
            mapping = new SalesTaxExemptionTypeDataMapping();
        }

        public SalesTaxExemptionTypeQueryService(SalesTaxExemptionTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SalesTaxExemptionTypeDataMapping();
        }

        public SalesTaxExemptionTypeQueryService(ICustomContext context)
        {
            this.repository = new SalesTaxExemptionTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SalesTaxExemptionTypeDataMapping();
        }
		 
		public  SalesTaxExemptionTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SalesTaxExemptionTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SalesTaxExemptionType entityPOCO)
        {
            SalesTaxExemptionTypeKeys entityKeys = new SalesTaxExemptionTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 