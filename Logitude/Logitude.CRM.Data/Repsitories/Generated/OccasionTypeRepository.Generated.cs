 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class OccasionTypeRepository:IRepository<OccasionType>
   {
   
        private ICRMContext currentContext;
        public OccasionTypeRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public OccasionTypeRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  OccasionType GetSingle(string id, int tenant)
        {
            return (from a in context.OccasionTypes
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<OccasionType> GetAll(int tenant)
        {
            return from a in context.OccasionTypes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public OccasionType GetSingle(EntityKeyFields entityKeys)
        {
            OccasionTypeKeys keys = entityKeys as OccasionTypeKeys;
            return (from a in context.OccasionTypes
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(OccasionType entity)
        {
            onAdd();
            context.OccasionTypes.Add(entity);
        }

        public void Remove(OccasionType entity)
        {
            context.OccasionTypes.Attach(entity);
            context.OccasionTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(OccasionType entity)
        {
            onUpdate();
            context.OccasionTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OccasionType> All()
        {
            return context.OccasionTypes.ToList();
        }

        private ICRMContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 