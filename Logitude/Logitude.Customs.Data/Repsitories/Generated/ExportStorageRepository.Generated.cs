 
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
   public partial class ExportStorageRepository:IRepository<ExportStorage>
   {
   
        private ICustomContext currentContext;
        public ExportStorageRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ExportStorageRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ExportStorage GetSingle(string id, int tenant)
        {
            return (from a in context.ExportStorages
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ExportStorage> GetAll(int tenant)
        {
            return from a in context.ExportStorages  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ExportStorage GetSingle(EntityKeyFields entityKeys)
        {
            ExportStorageKeys keys = entityKeys as ExportStorageKeys;
            return (from a in context.ExportStorages
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ExportStorage entity)
        {
            onAdd();
            context.ExportStorages.Add(entity);
        }

        public void Remove(ExportStorage entity)
        {
            context.ExportStorages.Attach(entity);
            context.ExportStorages.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ExportStorage entity)
        {
            onUpdate();
            context.ExportStorages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ExportStorage> All()
        {
            return context.ExportStorages.ToList();
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
	 