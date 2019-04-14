 
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
   public partial class TariffTypeRepository:IRepository<TariffType>
   {
   
        private ITariffModuleContext currentContext;
        public TariffTypeRepository(int tenant)
        {
            currentContext = TariffModuleContext.GetContext(tenant);
        }

        public TariffTypeRepository(ITariffModuleContext context)
        {
            currentContext = context;
        }

		 
		
		public  TariffType GetSingle(string code)
        {
            return (from a in context.TariffTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TariffType> GetAll()
        {
            return from a in context.TariffTypes  
                   select a;
        }
				 
        public TariffType GetSingle(EntityKeyFields entityKeys)
        {
            TariffTypeKeys keys = entityKeys as TariffTypeKeys;
            return (from a in context.TariffTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TariffType entity)
        {
            onAdd();
            context.TariffTypes.Add(entity);
        }

        public void Remove(TariffType entity)
        {
            context.TariffTypes.Attach(entity);
            context.TariffTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TariffType entity)
        {
            onUpdate();
            context.TariffTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TariffType> All()
        {
            return context.TariffTypes.ToList();
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
	 