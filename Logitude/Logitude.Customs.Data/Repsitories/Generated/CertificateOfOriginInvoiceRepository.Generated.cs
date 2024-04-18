 
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
   public partial class CertificateOfOriginInvoiceRepository:IRepository<CertificateOfOriginInvoice>
   {
   
        private ICustomContext currentContext;
        public CertificateOfOriginInvoiceRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CertificateOfOriginInvoiceRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CertificateOfOriginInvoice GetSingle(string id, int tenant)
        {
            return (from a in context.CertificateOfOriginInvoices
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CertificateOfOriginInvoice> GetAll(int tenant)
        {
            return from a in context.CertificateOfOriginInvoices  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CertificateOfOriginInvoice GetSingle(EntityKeyFields entityKeys)
        {
            CertificateOfOriginInvoiceKeys keys = entityKeys as CertificateOfOriginInvoiceKeys;
            return (from a in context.CertificateOfOriginInvoices
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CertificateOfOriginInvoice entity)
        {
            onAdd();
            context.CertificateOfOriginInvoices.Add(entity);
        }

        public void Remove(CertificateOfOriginInvoice entity)
        {
            context.CertificateOfOriginInvoices.Attach(entity);
            context.CertificateOfOriginInvoices.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CertificateOfOriginInvoice entity)
        {
            onUpdate();
            context.CertificateOfOriginInvoices.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CertificateOfOriginInvoice> All()
        {
            return context.CertificateOfOriginInvoices.ToList();
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
	 