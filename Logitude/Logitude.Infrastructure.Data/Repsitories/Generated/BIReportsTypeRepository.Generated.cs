 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.Repsitories
{
   public partial class BIReportsTypeRepository:IRepository<BIReportsType>
   {
   
        private IInfrastructureContext currentContext;
        public BIReportsTypeRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public BIReportsTypeRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  BIReportsType GetSingle(string code)
        {
            return (from a in context.BIReportsTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<BIReportsType> GetAll()
        {
            return from a in context.BIReportsTypes  
                   select a;
        }
				 
        public BIReportsType GetSingle(EntityKeyFields entityKeys)
        {
            BIReportsTypeKeys keys = entityKeys as BIReportsTypeKeys;
            return (from a in context.BIReportsTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BIReportsType entity)
        {
            onAdd();
            context.BIReportsTypes.Add(entity);
        }

        public void Remove(BIReportsType entity)
        {
            context.BIReportsTypes.Attach(entity);
            context.BIReportsTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BIReportsType entity)
        {
            onUpdate();
            context.BIReportsTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BIReportsType> All()
        {
            return context.BIReportsTypes.ToList();
        }

        private IInfrastructureContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 