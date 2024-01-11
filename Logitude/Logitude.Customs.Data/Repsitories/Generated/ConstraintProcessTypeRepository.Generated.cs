 
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
   public partial class ConstraintProcessTypeRepository:IRepository<ConstraintProcessType>
   {
   
        private ICustomContext currentContext;
        public ConstraintProcessTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ConstraintProcessTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ConstraintProcessType GetSingle(string code)
        {
            return (from a in context.ConstraintProcessTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ConstraintProcessType> GetAll()
        {
            return from a in context.ConstraintProcessTypes  
                   select a;
        }
				 
        public ConstraintProcessType GetSingle(EntityKeyFields entityKeys)
        {
            ConstraintProcessTypeKeys keys = entityKeys as ConstraintProcessTypeKeys;
            return (from a in context.ConstraintProcessTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ConstraintProcessType entity)
        {
            onAdd();
            context.ConstraintProcessTypes.Add(entity);
        }

        public void Remove(ConstraintProcessType entity)
        {
            context.ConstraintProcessTypes.Attach(entity);
            context.ConstraintProcessTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ConstraintProcessType entity)
        {
            onUpdate();
            context.ConstraintProcessTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ConstraintProcessType> All()
        {
            return context.ConstraintProcessTypes.ToList();
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
	 