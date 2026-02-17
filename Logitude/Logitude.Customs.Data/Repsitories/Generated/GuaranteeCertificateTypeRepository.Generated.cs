 
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
   public partial class GuaranteeCertificateTypeRepository:IRepository<GuaranteeCertificateType>
   {
   
        private ICustomContext currentContext;
        public GuaranteeCertificateTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public GuaranteeCertificateTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  GuaranteeCertificateType GetSingle(string code)
        {
            return (from a in context.GuaranteeCertificateTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<GuaranteeCertificateType> GetAll()
        {
            return from a in context.GuaranteeCertificateTypes  
                   select a;
        }
				 
        public GuaranteeCertificateType GetSingle(EntityKeyFields entityKeys)
        {
            GuaranteeCertificateTypeKeys keys = entityKeys as GuaranteeCertificateTypeKeys;
            return (from a in context.GuaranteeCertificateTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(GuaranteeCertificateType entity)
        {
            onAdd();
            context.GuaranteeCertificateTypes.Add(entity);
        }

        public void Remove(GuaranteeCertificateType entity)
        {
            context.GuaranteeCertificateTypes.Attach(entity);
            context.GuaranteeCertificateTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(GuaranteeCertificateType entity)
        {
            onUpdate();
            context.GuaranteeCertificateTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GuaranteeCertificateType> All()
        {
            return context.GuaranteeCertificateTypes.ToList();
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
	 