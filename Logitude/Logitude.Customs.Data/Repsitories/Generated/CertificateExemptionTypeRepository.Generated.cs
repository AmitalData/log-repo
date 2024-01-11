 
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
   public partial class CertificateExemptionTypeRepository:IRepository<CertificateExemptionType>
   {
   
        private ICustomContext currentContext;
        public CertificateExemptionTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CertificateExemptionTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CertificateExemptionType GetSingle(string code)
        {
            return (from a in context.CertificateExemptionTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CertificateExemptionType> GetAll()
        {
            return from a in context.CertificateExemptionTypes  
                   select a;
        }
				 
        public CertificateExemptionType GetSingle(EntityKeyFields entityKeys)
        {
            CertificateExemptionTypeKeys keys = entityKeys as CertificateExemptionTypeKeys;
            return (from a in context.CertificateExemptionTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CertificateExemptionType entity)
        {
            onAdd();
            context.CertificateExemptionTypes.Add(entity);
        }

        public void Remove(CertificateExemptionType entity)
        {
            context.CertificateExemptionTypes.Attach(entity);
            context.CertificateExemptionTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CertificateExemptionType entity)
        {
            onUpdate();
            context.CertificateExemptionTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CertificateExemptionType> All()
        {
            return context.CertificateExemptionTypes.ToList();
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
	 