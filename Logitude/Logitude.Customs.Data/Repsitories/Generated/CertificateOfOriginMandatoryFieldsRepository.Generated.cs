 
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
   public partial class CertificateOfOriginMandatoryFieldsRepository:IRepository<CertificateOfOriginMandatoryFields>
   {
   
        private ICustomContext currentContext;
        public CertificateOfOriginMandatoryFieldsRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CertificateOfOriginMandatoryFieldsRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CertificateOfOriginMandatoryFields GetSingle(string code)
        {
            return (from a in context.CertificateOfOriginMandatoryFieldss
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CertificateOfOriginMandatoryFields> GetAll()
        {
            return from a in context.CertificateOfOriginMandatoryFieldss  
                   select a;
        }
				 
        public CertificateOfOriginMandatoryFields GetSingle(EntityKeyFields entityKeys)
        {
            CertificateOfOriginMandatoryFieldsKeys keys = entityKeys as CertificateOfOriginMandatoryFieldsKeys;
            return (from a in context.CertificateOfOriginMandatoryFieldss
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CertificateOfOriginMandatoryFields entity)
        {
            onAdd();
            context.CertificateOfOriginMandatoryFieldss.Add(entity);
        }

        public void Remove(CertificateOfOriginMandatoryFields entity)
        {
            context.CertificateOfOriginMandatoryFieldss.Attach(entity);
            context.CertificateOfOriginMandatoryFieldss.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CertificateOfOriginMandatoryFields entity)
        {
            onUpdate();
            context.CertificateOfOriginMandatoryFieldss.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CertificateOfOriginMandatoryFields> All()
        {
            return context.CertificateOfOriginMandatoryFieldss.ToList();
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
	 