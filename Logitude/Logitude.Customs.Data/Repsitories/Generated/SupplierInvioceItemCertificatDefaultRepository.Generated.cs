 
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
   public partial class SupplierInvioceItemCertificatDefaultRepository:IRepository<SupplierInvioceItemCertificatDefault>
   {
   
        private ICustomContext currentContext;
        public SupplierInvioceItemCertificatDefaultRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvioceItemCertificatDefaultRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvioceItemCertificatDefault GetSingle(string supplierinvioceexportdefaultid, int sequencenumeric, int tenant)
        {
            return (from a in context.SupplierInvioceItemCertificatDefaults
                    where a.SupplierInvioceExportDefaultId == supplierinvioceexportdefaultid && a.SequenceNumeric == sequencenumeric && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvioceItemCertificatDefault> GetAll(int tenant)
        {
            return from a in context.SupplierInvioceItemCertificatDefaults  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvioceItemCertificatDefault GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvioceItemCertificatDefaultKeys keys = entityKeys as SupplierInvioceItemCertificatDefaultKeys;
            return (from a in context.SupplierInvioceItemCertificatDefaults
                    where a.SupplierInvioceExportDefaultId == keys.SupplierInvioceExportDefaultId && a.SequenceNumeric == keys.SequenceNumeric
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvioceItemCertificatDefault entity)
        {
            onAdd();
            context.SupplierInvioceItemCertificatDefaults.Add(entity);
        }

        public void Remove(SupplierInvioceItemCertificatDefault entity)
        {
            context.SupplierInvioceItemCertificatDefaults.Attach(entity);
            context.SupplierInvioceItemCertificatDefaults.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvioceItemCertificatDefault entity)
        {
            onUpdate();
            context.SupplierInvioceItemCertificatDefaults.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvioceItemCertificatDefault> All()
        {
            return context.SupplierInvioceItemCertificatDefaults.ToList();
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
	 