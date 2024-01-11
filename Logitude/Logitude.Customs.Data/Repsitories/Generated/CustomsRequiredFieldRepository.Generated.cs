 
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
   public partial class CustomsRequiredFieldRepository:IRepository<CustomsRequiredField>
   {
   
        private ICustomContext currentContext;
        public CustomsRequiredFieldRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsRequiredFieldRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsRequiredField GetSingle(string id, int tenant)
        {
            return (from a in context.CustomsRequiredFields
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsRequiredField> GetAll(int tenant)
        {
            return from a in context.CustomsRequiredFields  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomsRequiredField GetSingle(EntityKeyFields entityKeys)
        {
            CustomsRequiredFieldKeys keys = entityKeys as CustomsRequiredFieldKeys;
            return (from a in context.CustomsRequiredFields
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsRequiredField entity)
        {
            onAdd();
            context.CustomsRequiredFields.Add(entity);
        }

        public void Remove(CustomsRequiredField entity)
        {
            context.CustomsRequiredFields.Attach(entity);
            context.CustomsRequiredFields.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsRequiredField entity)
        {
            onUpdate();
            context.CustomsRequiredFields.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsRequiredField> All()
        {
            return context.CustomsRequiredFields.ToList();
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
	 