 
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
   public partial class CourierMasterRepository:IRepository<CourierMaster>
   {
   
        private ICustomContext currentContext;
        public CourierMasterRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CourierMasterRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CourierMaster GetSingle(string id, int tenant)
        {
            return (from a in context.CourierMasters
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CourierMaster> GetAll(int tenant)
        {
            return from a in context.CourierMasters  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CourierMaster GetSingle(EntityKeyFields entityKeys)
        {
            CourierMasterKeys keys = entityKeys as CourierMasterKeys;
            return (from a in context.CourierMasters
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CourierMaster entity)
        {
            onAdd();
            context.CourierMasters.Add(entity);
        }

        public void Remove(CourierMaster entity)
        {
            context.CourierMasters.Attach(entity);
            context.CourierMasters.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CourierMaster entity)
        {
            onUpdate();
            context.CourierMasters.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CourierMaster> All()
        {
            return context.CourierMasters.ToList();
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
	 