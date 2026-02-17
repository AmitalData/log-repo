 
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
   public partial class PhysicalCheckOperationQueryService: EntityQueryService<PhysicalCheckOperation,PhysicalCheckOperationKeys,PhysicalCheckOperationPM,object,PhysicalCheckOperationKeys>
   {
   
        PhysicalCheckOperationRepository repository;
		ICustomContext  context;
        public PhysicalCheckOperationQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new PhysicalCheckOperationRepository(context);
            Repository = repository;
            mapping = new PhysicalCheckOperationDataMapping();
        }

        public PhysicalCheckOperationQueryService(PhysicalCheckOperationRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PhysicalCheckOperationDataMapping();
        }

        public PhysicalCheckOperationQueryService(ICustomContext context)
        {
            this.repository = new PhysicalCheckOperationRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PhysicalCheckOperationDataMapping();
        }
		 
		public  PhysicalCheckOperationPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PhysicalCheckOperationKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PhysicalCheckOperation entityPOCO)
        {
            PhysicalCheckOperationKeys entityKeys = new PhysicalCheckOperationKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 