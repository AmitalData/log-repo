 
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
   public partial class ExportStorgeCargoRepository:IRepository<ExportStorgeCargo>
   {
   
        private ICustomContext currentContext;
        public ExportStorgeCargoRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ExportStorgeCargoRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ExportStorgeCargo GetSingle(string id, int tenant)
        {
            return (from a in context.ExportStorgeCargos
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ExportStorgeCargo> GetAll(int tenant)
        {
            return from a in context.ExportStorgeCargos  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ExportStorgeCargo GetSingle(EntityKeyFields entityKeys)
        {
            ExportStorgeCargoKeys keys = entityKeys as ExportStorgeCargoKeys;
            return (from a in context.ExportStorgeCargos
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ExportStorgeCargo entity)
        {
            onAdd();
            context.ExportStorgeCargos.Add(entity);
        }

        public void Remove(ExportStorgeCargo entity)
        {
            context.ExportStorgeCargos.Attach(entity);
            context.ExportStorgeCargos.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ExportStorgeCargo entity)
        {
            onUpdate();
            context.ExportStorgeCargos.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ExportStorgeCargo> All()
        {
            return context.ExportStorgeCargos.ToList();
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
	 