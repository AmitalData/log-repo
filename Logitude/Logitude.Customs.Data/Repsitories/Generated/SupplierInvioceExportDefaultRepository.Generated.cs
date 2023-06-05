 
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
   public partial class SupplierInvioceExportDefaultRepository:IRepository<SupplierInvioceExportDefault>
   {
   
        private ICustomContext currentContext;
        public SupplierInvioceExportDefaultRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvioceExportDefaultRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvioceExportDefault GetSingle(string id, int tenant)
        {
            return (from a in context.SupplierInvioceExportDefaults
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvioceExportDefault> GetAll(int tenant)
        {
            return from a in context.SupplierInvioceExportDefaults  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvioceExportDefault GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvioceExportDefaultKeys keys = entityKeys as SupplierInvioceExportDefaultKeys;
            return (from a in context.SupplierInvioceExportDefaults
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvioceExportDefault entity)
        {
            onAdd();
            context.SupplierInvioceExportDefaults.Add(entity);
        }

        public void Remove(SupplierInvioceExportDefault entity)
        {
            context.SupplierInvioceExportDefaults.Attach(entity);
            context.SupplierInvioceExportDefaults.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvioceExportDefault entity)
        {
            onUpdate();
            context.SupplierInvioceExportDefaults.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvioceExportDefault> All()
        {
            return context.SupplierInvioceExportDefaults.ToList();
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
	 