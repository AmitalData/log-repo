 
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
   public partial class DigitalTextCodeRepository:IRepository<DigitalTextCode>
   {
   
        private IInfrastructureContext currentContext;
        public DigitalTextCodeRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public DigitalTextCodeRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  DigitalTextCode GetSingle(string id, int tenant)
        {
            return (from a in context.DigitalTextCodes
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DigitalTextCode> GetAll(int tenant)
        {
            return from a in context.DigitalTextCodes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DigitalTextCode GetSingle(EntityKeyFields entityKeys)
        {
            DigitalTextCodeKeys keys = entityKeys as DigitalTextCodeKeys;
            return (from a in context.DigitalTextCodes
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DigitalTextCode entity)
        {
            onAdd();
            context.DigitalTextCodes.Add(entity);
        }

        public void Remove(DigitalTextCode entity)
        {
            context.DigitalTextCodes.Attach(entity);
            context.DigitalTextCodes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DigitalTextCode entity)
        {
            onUpdate();
            context.DigitalTextCodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DigitalTextCode> All()
        {
            return context.DigitalTextCodes.ToList();
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
	 