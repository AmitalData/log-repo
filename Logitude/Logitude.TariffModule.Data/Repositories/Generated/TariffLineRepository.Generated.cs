 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.TariffModule.Data.Repositories
{
   public partial class TariffLineRepository:IRepository<TariffLine>
   {
   
        private ITariffModuleContext currentContext;
        public TariffLineRepository(int tenant)
        {
            currentContext = TariffModuleContext.GetContext(tenant);
        }

        public TariffLineRepository(ITariffModuleContext context)
        {
            currentContext = context;
        }

		 
		
		public  TariffLine GetSingle(string id, int tenant)
        {
            return (from a in context.TariffLines
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TariffLine> GetAll(int tenant)
        {
            return from a in context.TariffLines  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TariffLine GetSingle(EntityKeyFields entityKeys)
        {
            TariffLineKeys keys = entityKeys as TariffLineKeys;
            return (from a in context.TariffLines
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TariffLine entity)
        {
            onAdd();
            context.TariffLines.Add(entity);
        }

        public void Remove(TariffLine entity)
        {
            context.TariffLines.Attach(entity);
            context.TariffLines.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TariffLine entity)
        {
            onUpdate();
            context.TariffLines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TariffLine> All()
        {
            return context.TariffLines.ToList();
        }

        private ITariffModuleContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 