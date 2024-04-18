 
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
   public partial class CertificateOfOriginRepository:IRepository<CertificateOfOrigin>
   {
   
        private ICustomContext currentContext;
        public CertificateOfOriginRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CertificateOfOriginRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CertificateOfOrigin GetSingle(string id, int tenant)
        {
            return (from a in context.CertificateOfOrigins
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CertificateOfOrigin> GetAll(int tenant)
        {
            return from a in context.CertificateOfOrigins  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CertificateOfOrigin GetSingle(EntityKeyFields entityKeys)
        {
            CertificateOfOriginKeys keys = entityKeys as CertificateOfOriginKeys;
            return (from a in context.CertificateOfOrigins
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CertificateOfOrigin entity)
        {
            onAdd();
            context.CertificateOfOrigins.Add(entity);
        }

        public void Remove(CertificateOfOrigin entity)
        {
            context.CertificateOfOrigins.Attach(entity);
            context.CertificateOfOrigins.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CertificateOfOrigin entity)
        {
            onUpdate();
            context.CertificateOfOrigins.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CertificateOfOrigin> All()
        {
            return context.CertificateOfOrigins.ToList();
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
	 