 
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
   public partial class CourierManifestStatusRepository:IRepository<CourierManifestStatus>
   {
   
        private ICustomContext currentContext;
        public CourierManifestStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CourierManifestStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CourierManifestStatus GetSingle(string code)
        {
            return (from a in context.CourierManifestStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CourierManifestStatus> GetAll()
        {
            return from a in context.CourierManifestStatuses  
                   select a;
        }
				 
        public CourierManifestStatus GetSingle(EntityKeyFields entityKeys)
        {
            CourierManifestStatusKeys keys = entityKeys as CourierManifestStatusKeys;
            return (from a in context.CourierManifestStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CourierManifestStatus entity)
        {
            onAdd();
            context.CourierManifestStatuses.Add(entity);
        }

        public void Remove(CourierManifestStatus entity)
        {
            context.CourierManifestStatuses.Attach(entity);
            context.CourierManifestStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CourierManifestStatus entity)
        {
            onUpdate();
            context.CourierManifestStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CourierManifestStatus> All()
        {
            return context.CourierManifestStatuses.ToList();
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
	 