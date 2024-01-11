 
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
   public partial class CustomsBookTypeRepository:IRepository<CustomsBookType>
   {
   
        private ICustomContext currentContext;
        public CustomsBookTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsBookTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsBookType GetSingle(string code)
        {
            return (from a in context.CustomsBookTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsBookType> GetAll()
        {
            return from a in context.CustomsBookTypes  
                   select a;
        }
				 
        public CustomsBookType GetSingle(EntityKeyFields entityKeys)
        {
            CustomsBookTypeKeys keys = entityKeys as CustomsBookTypeKeys;
            return (from a in context.CustomsBookTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsBookType entity)
        {
            onAdd();
            context.CustomsBookTypes.Add(entity);
        }

        public void Remove(CustomsBookType entity)
        {
            context.CustomsBookTypes.Attach(entity);
            context.CustomsBookTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsBookType entity)
        {
            onUpdate();
            context.CustomsBookTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsBookType> All()
        {
            return context.CustomsBookTypes.ToList();
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
	 