 
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
   public partial class AmedmentTypeRepository:IRepository<AmedmentType>
   {
   
        private ICustomContext currentContext;
        public AmedmentTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AmedmentTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  AmedmentType GetSingle(string code)
        {
            return (from a in context.AmedmentTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AmedmentType> GetAll()
        {
            return from a in context.AmedmentTypes  
                   select a;
        }
				 
        public AmedmentType GetSingle(EntityKeyFields entityKeys)
        {
            AmedmentTypeKeys keys = entityKeys as AmedmentTypeKeys;
            return (from a in context.AmedmentTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AmedmentType entity)
        {
            onAdd();
            context.AmedmentTypes.Add(entity);
        }

        public void Remove(AmedmentType entity)
        {
            context.AmedmentTypes.Attach(entity);
            context.AmedmentTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AmedmentType entity)
        {
            onUpdate();
            context.AmedmentTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AmedmentType> All()
        {
            return context.AmedmentTypes.ToList();
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
	 