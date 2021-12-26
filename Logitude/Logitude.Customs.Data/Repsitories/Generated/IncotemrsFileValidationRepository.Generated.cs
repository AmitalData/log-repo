 
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
   public partial class IncotemrsFileValidationRepository:IRepository<IncotemrsFileValidation>
   {
   
        private ICustomContext currentContext;
        public IncotemrsFileValidationRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public IncotemrsFileValidationRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  IncotemrsFileValidation GetSingle(string code)
        {
            return (from a in context.IncotemrsFileValidations
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<IncotemrsFileValidation> GetAll()
        {
            return from a in context.IncotemrsFileValidations  
                   select a;
        }
				 
        public IncotemrsFileValidation GetSingle(EntityKeyFields entityKeys)
        {
            IncotemrsFileValidationKeys keys = entityKeys as IncotemrsFileValidationKeys;
            return (from a in context.IncotemrsFileValidations
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(IncotemrsFileValidation entity)
        {
            onAdd();
            context.IncotemrsFileValidations.Add(entity);
        }

        public void Remove(IncotemrsFileValidation entity)
        {
            context.IncotemrsFileValidations.Attach(entity);
            context.IncotemrsFileValidations.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(IncotemrsFileValidation entity)
        {
            onUpdate();
            context.IncotemrsFileValidations.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<IncotemrsFileValidation> All()
        {
            return context.IncotemrsFileValidations.ToList();
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
	 