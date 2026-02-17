 
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
   public partial class TestEntityQueryService: EntityQueryService<TestEntity,TestEntityKeys,TestEntityPM,object,TestEntityKeys>
   {
   
        TestEntityRepository repository;
		IAccountingContext  context;
        public TestEntityQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new TestEntityRepository(context);
            Repository = repository;
            mapping = new TestEntityDataMapping();
        }

        public TestEntityQueryService(TestEntityRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TestEntityDataMapping();
        }

        public TestEntityQueryService(IAccountingContext context)
        {
            this.repository = new TestEntityRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TestEntityDataMapping();
        }
		 
		public  TestEntityPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TestEntityKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TestEntity entityPOCO)
        {
            TestEntityKeys entityKeys = new TestEntityKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 