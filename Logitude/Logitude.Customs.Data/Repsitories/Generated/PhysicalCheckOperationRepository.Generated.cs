 
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
   public partial class PhysicalCheckOperationRepository:IRepository<PhysicalCheckOperation>
   {
   
        private ICustomContext currentContext;
        public PhysicalCheckOperationRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PhysicalCheckOperationRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PhysicalCheckOperation GetSingle(string code)
        {
            return (from a in context.PhysicalCheckOperations
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PhysicalCheckOperation> GetAll()
        {
            return from a in context.PhysicalCheckOperations  
                   select a;
        }
				 
        public PhysicalCheckOperation GetSingle(EntityKeyFields entityKeys)
        {
            PhysicalCheckOperationKeys keys = entityKeys as PhysicalCheckOperationKeys;
            return (from a in context.PhysicalCheckOperations
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PhysicalCheckOperation entity)
        {
            onAdd();
            context.PhysicalCheckOperations.Add(entity);
        }

        public void Remove(PhysicalCheckOperation entity)
        {
            context.PhysicalCheckOperations.Attach(entity);
            context.PhysicalCheckOperations.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PhysicalCheckOperation entity)
        {
            onUpdate();
            context.PhysicalCheckOperations.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PhysicalCheckOperation> All()
        {
            return context.PhysicalCheckOperations.ToList();
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
	 