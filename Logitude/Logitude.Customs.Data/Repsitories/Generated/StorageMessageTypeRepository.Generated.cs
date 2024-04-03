 
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
   public partial class StorageMessageTypeRepository:IRepository<StorageMessageType>
   {
   
        private ICustomContext currentContext;
        public StorageMessageTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public StorageMessageTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  StorageMessageType GetSingle(string code)
        {
            return (from a in context.StorageMessageTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<StorageMessageType> GetAll()
        {
            return from a in context.StorageMessageTypes  
                   select a;
        }
				 
        public StorageMessageType GetSingle(EntityKeyFields entityKeys)
        {
            StorageMessageTypeKeys keys = entityKeys as StorageMessageTypeKeys;
            return (from a in context.StorageMessageTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(StorageMessageType entity)
        {
            onAdd();
            context.StorageMessageTypes.Add(entity);
        }

        public void Remove(StorageMessageType entity)
        {
            context.StorageMessageTypes.Attach(entity);
            context.StorageMessageTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(StorageMessageType entity)
        {
            onUpdate();
            context.StorageMessageTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<StorageMessageType> All()
        {
            return context.StorageMessageTypes.ToList();
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
	 