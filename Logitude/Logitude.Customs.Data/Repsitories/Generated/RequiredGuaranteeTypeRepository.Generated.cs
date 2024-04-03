 
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
   public partial class RequiredGuaranteeTypeRepository:IRepository<RequiredGuaranteeType>
   {
   
        private ICustomContext currentContext;
        public RequiredGuaranteeTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public RequiredGuaranteeTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  RequiredGuaranteeType GetSingle(string id, int tenant)
        {
            return (from a in context.RequiredGuaranteeTypes
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<RequiredGuaranteeType> GetAll(int tenant)
        {
            return from a in context.RequiredGuaranteeTypes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public RequiredGuaranteeType GetSingle(EntityKeyFields entityKeys)
        {
            RequiredGuaranteeTypeKeys keys = entityKeys as RequiredGuaranteeTypeKeys;
            return (from a in context.RequiredGuaranteeTypes
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(RequiredGuaranteeType entity)
        {
            onAdd();
            context.RequiredGuaranteeTypes.Add(entity);
        }

        public void Remove(RequiredGuaranteeType entity)
        {
            context.RequiredGuaranteeTypes.Attach(entity);
            context.RequiredGuaranteeTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(RequiredGuaranteeType entity)
        {
            onUpdate();
            context.RequiredGuaranteeTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<RequiredGuaranteeType> All()
        {
            return context.RequiredGuaranteeTypes.ToList();
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
	 