 
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
   public partial class ExportStorgeRepository:IRepository<ExportStorge>
   {
   
        private ICustomContext currentContext;
        public ExportStorgeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ExportStorgeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ExportStorge GetSingle(string id, int tenant)
        {
            return (from a in context.ExportStorges
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ExportStorge> GetAll(int tenant)
        {
            return from a in context.ExportStorges  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ExportStorge GetSingle(EntityKeyFields entityKeys)
        {
            ExportStorgeKeys keys = entityKeys as ExportStorgeKeys;
            return (from a in context.ExportStorges
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ExportStorge entity)
        {
            onAdd();
            context.ExportStorges.Add(entity);
        }

        public void Remove(ExportStorge entity)
        {
            context.ExportStorges.Attach(entity);
            context.ExportStorges.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ExportStorge entity)
        {
            onUpdate();
            context.ExportStorges.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ExportStorge> All()
        {
            return context.ExportStorges.ToList();
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
	 