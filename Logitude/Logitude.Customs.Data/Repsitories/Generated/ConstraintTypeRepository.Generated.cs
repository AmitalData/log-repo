 
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
   public partial class ConstraintTypeRepository:IRepository<ConstraintType>
   {
   
        private ICustomContext currentContext;
        public ConstraintTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ConstraintTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ConstraintType GetSingle(string code)
        {
            return (from a in context.ConstraintTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ConstraintType> GetAll()
        {
            return from a in context.ConstraintTypes  
                   select a;
        }
				 
        public ConstraintType GetSingle(EntityKeyFields entityKeys)
        {
            ConstraintTypeKeys keys = entityKeys as ConstraintTypeKeys;
            return (from a in context.ConstraintTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ConstraintType entity)
        {
            onAdd();
            context.ConstraintTypes.Add(entity);
        }

        public void Remove(ConstraintType entity)
        {
            context.ConstraintTypes.Attach(entity);
            context.ConstraintTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ConstraintType entity)
        {
            onUpdate();
            context.ConstraintTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ConstraintType> All()
        {
            return context.ConstraintTypes.ToList();
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
	 