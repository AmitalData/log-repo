 
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
   public partial class CertificatesStatusRepository:IRepository<CertificatesStatus>
   {
   
        private ICustomContext currentContext;
        public CertificatesStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CertificatesStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CertificatesStatus GetSingle(string code)
        {
            return (from a in context.CertificatesStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CertificatesStatus> GetAll()
        {
            return from a in context.CertificatesStatuses  
                   select a;
        }
				 
        public CertificatesStatus GetSingle(EntityKeyFields entityKeys)
        {
            CertificatesStatusKeys keys = entityKeys as CertificatesStatusKeys;
            return (from a in context.CertificatesStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CertificatesStatus entity)
        {
            onAdd();
            context.CertificatesStatuses.Add(entity);
        }

        public void Remove(CertificatesStatus entity)
        {
            context.CertificatesStatuses.Attach(entity);
            context.CertificatesStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CertificatesStatus entity)
        {
            onUpdate();
            context.CertificatesStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CertificatesStatus> All()
        {
            return context.CertificatesStatuses.ToList();
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
	 