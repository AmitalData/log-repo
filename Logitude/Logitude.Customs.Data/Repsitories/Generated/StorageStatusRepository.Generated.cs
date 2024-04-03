 
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
   public partial class StorageStatusRepository:IRepository<StorageStatus>
   {
   
        private ICustomContext currentContext;
        public StorageStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public StorageStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  StorageStatus GetSingle(string code)
        {
            return (from a in context.StorageStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<StorageStatus> GetAll()
        {
            return from a in context.StorageStatuses  
                   select a;
        }
				 
        public StorageStatus GetSingle(EntityKeyFields entityKeys)
        {
            StorageStatusKeys keys = entityKeys as StorageStatusKeys;
            return (from a in context.StorageStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(StorageStatus entity)
        {
            onAdd();
            context.StorageStatuses.Add(entity);
        }

        public void Remove(StorageStatus entity)
        {
            context.StorageStatuses.Attach(entity);
            context.StorageStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(StorageStatus entity)
        {
            onUpdate();
            context.StorageStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<StorageStatus> All()
        {
            return context.StorageStatuses.ToList();
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
	 