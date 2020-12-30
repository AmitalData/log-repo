 
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
   public partial class TariffVersionUploadedExcelRepository:IRepository<TariffVersionUploadedExcel>
   {
   
        private ITariffModuleContext currentContext;
        public TariffVersionUploadedExcelRepository(int tenant)
        {
            currentContext = TariffModuleContext.GetContext(tenant);
        }

        public TariffVersionUploadedExcelRepository(ITariffModuleContext context)
        {
            currentContext = context;
        }

		 
		
		public  TariffVersionUploadedExcel GetSingle(string id, int tenant)
        {
            return (from a in context.TariffVersionUploadedExcels
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TariffVersionUploadedExcel> GetAll(int tenant)
        {
            return from a in context.TariffVersionUploadedExcels  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TariffVersionUploadedExcel GetSingle(EntityKeyFields entityKeys)
        {
            TariffVersionUploadedExcelKeys keys = entityKeys as TariffVersionUploadedExcelKeys;
            return (from a in context.TariffVersionUploadedExcels
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TariffVersionUploadedExcel entity)
        {
            onAdd();
            context.TariffVersionUploadedExcels.Add(entity);
        }

        public void Remove(TariffVersionUploadedExcel entity)
        {
            context.TariffVersionUploadedExcels.Attach(entity);
            context.TariffVersionUploadedExcels.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TariffVersionUploadedExcel entity)
        {
            onUpdate();
            context.TariffVersionUploadedExcels.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TariffVersionUploadedExcel> All()
        {
            return context.TariffVersionUploadedExcels.ToList();
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
	 