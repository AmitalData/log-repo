 
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
   public partial class CertificateOfOriginItemRepository:IRepository<CertificateOfOriginItem>
   {
   
        private ICustomContext currentContext;
        public CertificateOfOriginItemRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CertificateOfOriginItemRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CertificateOfOriginItem GetSingle(string id, int tenant)
        {
            return (from a in context.CertificateOfOriginItems
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CertificateOfOriginItem> GetAll(int tenant)
        {
            return from a in context.CertificateOfOriginItems  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CertificateOfOriginItem GetSingle(EntityKeyFields entityKeys)
        {
            CertificateOfOriginItemKeys keys = entityKeys as CertificateOfOriginItemKeys;
            return (from a in context.CertificateOfOriginItems
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CertificateOfOriginItem entity)
        {
            onAdd();
            context.CertificateOfOriginItems.Add(entity);
        }

        public void Remove(CertificateOfOriginItem entity)
        {
            context.CertificateOfOriginItems.Attach(entity);
            context.CertificateOfOriginItems.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CertificateOfOriginItem entity)
        {
            onUpdate();
            context.CertificateOfOriginItems.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CertificateOfOriginItem> All()
        {
            return context.CertificateOfOriginItems.ToList();
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
	 