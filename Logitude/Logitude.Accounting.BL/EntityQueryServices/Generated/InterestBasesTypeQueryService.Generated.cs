 
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
   public partial class InterestBasesTypeQueryService: EntityQueryService<InterestBasesType,InterestBasesTypeKeys,InterestBasesTypePM,object,InterestBasesTypeKeys>
   {
   
        InterestBasesTypeRepository repository;
		IAccountingContext  context;
        public InterestBasesTypeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new InterestBasesTypeRepository(context);
            Repository = repository;
            mapping = new InterestBasesTypeDataMapping();
        }

        public InterestBasesTypeQueryService(InterestBasesTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new InterestBasesTypeDataMapping();
        }

        public InterestBasesTypeQueryService(IAccountingContext context)
        {
            this.repository = new InterestBasesTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new InterestBasesTypeDataMapping();
        }
		 
		public  InterestBasesTypePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new InterestBasesTypeKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(InterestBasesType entityPOCO)
        {
            InterestBasesTypeKeys entityKeys = new InterestBasesTypeKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 