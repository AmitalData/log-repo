 
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
   public partial class ExportReferenceRepository:IRepository<ExportReference>
   {
   
        private ICustomContext currentContext;
        public ExportReferenceRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ExportReferenceRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ExportReference GetSingle(string id, int tenant)
        {
            return (from a in context.ExportReferences
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ExportReference> GetAll(int tenant)
        {
            return from a in context.ExportReferences  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ExportReference GetSingle(EntityKeyFields entityKeys)
        {
            ExportReferenceKeys keys = entityKeys as ExportReferenceKeys;
            return (from a in context.ExportReferences
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ExportReference entity)
        {
            onAdd();
            context.ExportReferences.Add(entity);
        }

        public void Remove(ExportReference entity)
        {
            context.ExportReferences.Attach(entity);
            context.ExportReferences.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ExportReference entity)
        {
            onUpdate();
            context.ExportReferences.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ExportReference> All()
        {
            return context.ExportReferences.ToList();
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
	 