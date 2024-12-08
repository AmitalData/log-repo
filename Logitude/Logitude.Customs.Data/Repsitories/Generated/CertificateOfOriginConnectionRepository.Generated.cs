 
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
   public partial class CertificateOfOriginConnectionRepository:IRepository<CertificateOfOriginConnection>
   {
   
        private ICustomContext currentContext;
        public CertificateOfOriginConnectionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CertificateOfOriginConnectionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CertificateOfOriginConnection GetSingle(string id, int tenant)
        {
            return (from a in context.CertificateOfOriginConnections
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CertificateOfOriginConnection> GetAll(int tenant)
        {
            return from a in context.CertificateOfOriginConnections  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CertificateOfOriginConnection GetSingle(EntityKeyFields entityKeys)
        {
            CertificateOfOriginConnectionKeys keys = entityKeys as CertificateOfOriginConnectionKeys;
            return (from a in context.CertificateOfOriginConnections
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CertificateOfOriginConnection entity)
        {
            onAdd();
            context.CertificateOfOriginConnections.Add(entity);
        }

        public void Remove(CertificateOfOriginConnection entity)
        {
            context.CertificateOfOriginConnections.Attach(entity);
            context.CertificateOfOriginConnections.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CertificateOfOriginConnection entity)
        {
            onUpdate();
            context.CertificateOfOriginConnections.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CertificateOfOriginConnection> All()
        {
            return context.CertificateOfOriginConnections.ToList();
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
	 