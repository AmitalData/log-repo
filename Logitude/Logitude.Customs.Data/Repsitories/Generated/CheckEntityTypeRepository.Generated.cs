 
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
   public partial class CheckEntityTypeRepository:IRepository<CheckEntityType>
   {
   
        private ICustomContext currentContext;
        public CheckEntityTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CheckEntityTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CheckEntityType GetSingle(string code)
        {
            return (from a in context.CheckEntityTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CheckEntityType> GetAll()
        {
            return from a in context.CheckEntityTypes  
                   select a;
        }
				 
        public CheckEntityType GetSingle(EntityKeyFields entityKeys)
        {
            CheckEntityTypeKeys keys = entityKeys as CheckEntityTypeKeys;
            return (from a in context.CheckEntityTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CheckEntityType entity)
        {
            onAdd();
            context.CheckEntityTypes.Add(entity);
        }

        public void Remove(CheckEntityType entity)
        {
            context.CheckEntityTypes.Attach(entity);
            context.CheckEntityTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CheckEntityType entity)
        {
            onUpdate();
            context.CheckEntityTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CheckEntityType> All()
        {
            return context.CheckEntityTypes.ToList();
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
	 