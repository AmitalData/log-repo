 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Accounting.BL.EntityQueryServices
{ 
   public partial class InterestEntityTypeQueryService: EntityQueryService<InterestEntityType,InterestEntityTypeKeys,InterestEntityTypePM,object,InterestEntityTypeKeys>
   {
   
        InterestEntityTypeRepository repository;
		IAccountingContext  context;
        public InterestEntityTypeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new InterestEntityTypeRepository(context);
            Repository = repository;
            mapping = new InterestEntityTypeDataMapping();
        }

        public InterestEntityTypeQueryService(InterestEntityTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new InterestEntityTypeDataMapping();
        }

        public InterestEntityTypeQueryService(IAccountingContext context)
        {
            this.repository = new InterestEntityTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new InterestEntityTypeDataMapping();
        }
		 
		public  InterestEntityTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new InterestEntityTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(InterestEntityType entityPOCO)
        {
            InterestEntityTypeKeys entityKeys = new InterestEntityTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 