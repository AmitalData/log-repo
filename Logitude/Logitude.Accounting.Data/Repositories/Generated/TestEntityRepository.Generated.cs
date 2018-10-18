 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class TestEntityRepository:IRepository<TestEntity>
   {
   
        private IAccountingContext currentContext;
        public TestEntityRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public TestEntityRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  TestEntity GetSingle(string id, int tenant)
        {
            return (from a in context.TestEntities
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TestEntity> GetAll(int tenant)
        {
            return from a in context.TestEntities  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TestEntity GetSingle(EntityKeyFields entityKeys)
        {
            TestEntityKeys keys = entityKeys as TestEntityKeys;
            return (from a in context.TestEntities
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TestEntity entity)
        {
            onAdd();
            context.TestEntities.Add(entity);
        }

        public void Remove(TestEntity entity)
        {
            context.TestEntities.Attach(entity);
            context.TestEntities.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TestEntity entity)
        {
            onUpdate();
            context.TestEntities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TestEntity> All()
        {
            return context.TestEntities.ToList();
        }

        private IAccountingContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 