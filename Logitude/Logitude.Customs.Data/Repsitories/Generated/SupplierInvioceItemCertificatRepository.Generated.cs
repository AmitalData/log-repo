 
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
   public partial class SupplierInvioceItemCertificatRepository:IRepository<SupplierInvioceItemCertificat>
   {
   
        private ICustomContext currentContext;
        public SupplierInvioceItemCertificatRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierInvioceItemCertificatRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierInvioceItemCertificat GetSingle(string declarationid, int invoicecounterkey, int linenumber, int itemcertificatecounterkey, int tenant)
        {
            return (from a in context.SupplierInvioceItemCertificats
                    where a.DeclarationId == declarationid && a.InvoiceCounterKey == invoicecounterkey && a.LineNumber == linenumber && a.ItemCertificateCounterKey == itemcertificatecounterkey && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierInvioceItemCertificat> GetAll(int tenant)
        {
            return from a in context.SupplierInvioceItemCertificats  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupplierInvioceItemCertificat GetSingle(EntityKeyFields entityKeys)
        {
            SupplierInvioceItemCertificatKeys keys = entityKeys as SupplierInvioceItemCertificatKeys;
            return (from a in context.SupplierInvioceItemCertificats
                    where a.DeclarationId == keys.DeclarationId && a.InvoiceCounterKey == keys.InvoiceCounterKey && a.LineNumber == keys.LineNumber && a.ItemCertificateCounterKey == keys.ItemCertificateCounterKey
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierInvioceItemCertificat entity)
        {
            onAdd();
            context.SupplierInvioceItemCertificats.Add(entity);
        }

        public void Remove(SupplierInvioceItemCertificat entity)
        {
            context.SupplierInvioceItemCertificats.Attach(entity);
            context.SupplierInvioceItemCertificats.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierInvioceItemCertificat entity)
        {
            onUpdate();
            context.SupplierInvioceItemCertificats.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierInvioceItemCertificat> All()
        {
            return context.SupplierInvioceItemCertificats.ToList();
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
	 