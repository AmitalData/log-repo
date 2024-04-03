 
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
   public partial class CustomsEnvoirmentTypeRepository:IRepository<CustomsEnvoirmentType>
   {
   
        private ICustomContext currentContext;
        public CustomsEnvoirmentTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsEnvoirmentTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsEnvoirmentType GetSingle(string code)
        {
            return (from a in context.CustomsEnvoirmentTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsEnvoirmentType> GetAll()
        {
            return from a in context.CustomsEnvoirmentTypes  
                   select a;
        }
				 
        public CustomsEnvoirmentType GetSingle(EntityKeyFields entityKeys)
        {
            CustomsEnvoirmentTypeKeys keys = entityKeys as CustomsEnvoirmentTypeKeys;
            return (from a in context.CustomsEnvoirmentTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsEnvoirmentType entity)
        {
            onAdd();
            context.CustomsEnvoirmentTypes.Add(entity);
        }

        public void Remove(CustomsEnvoirmentType entity)
        {
            context.CustomsEnvoirmentTypes.Attach(entity);
            context.CustomsEnvoirmentTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsEnvoirmentType entity)
        {
            onUpdate();
            context.CustomsEnvoirmentTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsEnvoirmentType> All()
        {
            return context.CustomsEnvoirmentTypes.ToList();
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
	 