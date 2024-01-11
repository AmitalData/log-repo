 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class StorageStatusTableRepository:IRepository<StorageStatusTable>
   {
   
        private ICustomContext currentContext;
        public StorageStatusTableRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public StorageStatusTableRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  StorageStatusTable GetSingle(string code)
        {
            return (from a in context.StorageStatusTables
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<StorageStatusTable> GetAll()
        {
            return from a in context.StorageStatusTables  
                   select a;
        }
				 
        public StorageStatusTable GetSingle(EntityKeyFields entityKeys)
        {
            StorageStatusTableKeys keys = entityKeys as StorageStatusTableKeys;
            return (from a in context.StorageStatusTables
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(StorageStatusTable entity)
        {
            onAdd();
            context.StorageStatusTables.Add(entity);
        }

        public void Remove(StorageStatusTable entity)
        {
            context.StorageStatusTables.Attach(entity);
            context.StorageStatusTables.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(StorageStatusTable entity)
        {
            onUpdate();
            context.StorageStatusTables.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<StorageStatusTable> All()
        {
            return context.StorageStatusTables.ToList();
        }

        private ICustomContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 