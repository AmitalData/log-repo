 
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
   public partial class CertificateOfOriginTypeCodeEnumRepository:IRepository<CertificateOfOriginTypeCodeEnum>
   {
   
        private ICustomContext currentContext;
        public CertificateOfOriginTypeCodeEnumRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CertificateOfOriginTypeCodeEnumRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CertificateOfOriginTypeCodeEnum GetSingle(string code)
        {
            return (from a in context.CertificateOfOriginTypeCodeEnums
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CertificateOfOriginTypeCodeEnum> GetAll()
        {
            return from a in context.CertificateOfOriginTypeCodeEnums  
                   select a;
        }
				 
        public CertificateOfOriginTypeCodeEnum GetSingle(EntityKeyFields entityKeys)
        {
            CertificateOfOriginTypeCodeEnumKeys keys = entityKeys as CertificateOfOriginTypeCodeEnumKeys;
            return (from a in context.CertificateOfOriginTypeCodeEnums
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CertificateOfOriginTypeCodeEnum entity)
        {
            onAdd();
            context.CertificateOfOriginTypeCodeEnums.Add(entity);
        }

        public void Remove(CertificateOfOriginTypeCodeEnum entity)
        {
            context.CertificateOfOriginTypeCodeEnums.Attach(entity);
            context.CertificateOfOriginTypeCodeEnums.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CertificateOfOriginTypeCodeEnum entity)
        {
            onUpdate();
            context.CertificateOfOriginTypeCodeEnums.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CertificateOfOriginTypeCodeEnum> All()
        {
            return context.CertificateOfOriginTypeCodeEnums.ToList();
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
	 