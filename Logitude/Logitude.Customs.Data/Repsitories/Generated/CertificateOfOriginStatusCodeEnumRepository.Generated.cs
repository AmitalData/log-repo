 
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
   public partial class CertificateOfOriginStatusCodeEnumRepository:IRepository<CertificateOfOriginStatusCodeEnum>
   {
   
        private ICustomContext currentContext;
        public CertificateOfOriginStatusCodeEnumRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CertificateOfOriginStatusCodeEnumRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CertificateOfOriginStatusCodeEnum GetSingle(string code)
        {
            return (from a in context.CertificateOfOriginStatusCodeEnums
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CertificateOfOriginStatusCodeEnum> GetAll()
        {
            return from a in context.CertificateOfOriginStatusCodeEnums  
                   select a;
        }
				 
        public CertificateOfOriginStatusCodeEnum GetSingle(EntityKeyFields entityKeys)
        {
            CertificateOfOriginStatusCodeEnumKeys keys = entityKeys as CertificateOfOriginStatusCodeEnumKeys;
            return (from a in context.CertificateOfOriginStatusCodeEnums
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CertificateOfOriginStatusCodeEnum entity)
        {
            onAdd();
            context.CertificateOfOriginStatusCodeEnums.Add(entity);
        }

        public void Remove(CertificateOfOriginStatusCodeEnum entity)
        {
            context.CertificateOfOriginStatusCodeEnums.Attach(entity);
            context.CertificateOfOriginStatusCodeEnums.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CertificateOfOriginStatusCodeEnum entity)
        {
            onUpdate();
            context.CertificateOfOriginStatusCodeEnums.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CertificateOfOriginStatusCodeEnum> All()
        {
            return context.CertificateOfOriginStatusCodeEnums.ToList();
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
	 