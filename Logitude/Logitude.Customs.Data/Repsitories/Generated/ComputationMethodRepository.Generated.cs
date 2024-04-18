 
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
   public partial class ComputationMethodRepository:IRepository<ComputationMethod>
   {
   
        private ICustomContext currentContext;
        public ComputationMethodRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ComputationMethodRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ComputationMethod GetSingle(string code)
        {
            return (from a in context.ComputationMethods
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ComputationMethod> GetAll()
        {
            return from a in context.ComputationMethods  
                   select a;
        }
				 
        public ComputationMethod GetSingle(EntityKeyFields entityKeys)
        {
            ComputationMethodKeys keys = entityKeys as ComputationMethodKeys;
            return (from a in context.ComputationMethods
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ComputationMethod entity)
        {
            onAdd();
            context.ComputationMethods.Add(entity);
        }

        public void Remove(ComputationMethod entity)
        {
            context.ComputationMethods.Attach(entity);
            context.ComputationMethods.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ComputationMethod entity)
        {
            onUpdate();
            context.ComputationMethods.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ComputationMethod> All()
        {
            return context.ComputationMethods.ToList();
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
	 