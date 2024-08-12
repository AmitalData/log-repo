 
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
   public partial class GatepassRequestRepository:IRepository<GatepassRequest>
   {
   
        private ICustomContext currentContext;
        public GatepassRequestRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public GatepassRequestRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  GatepassRequest GetSingle(string id, int tenant)
        {
            return (from a in context.GatepassRequests
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<GatepassRequest> GetAll(int tenant)
        {
            return from a in context.GatepassRequests  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public GatepassRequest GetSingle(EntityKeyFields entityKeys)
        {
            GatepassRequestKeys keys = entityKeys as GatepassRequestKeys;
            return (from a in context.GatepassRequests
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(GatepassRequest entity)
        {
            onAdd();
            context.GatepassRequests.Add(entity);
        }

        public void Remove(GatepassRequest entity)
        {
            context.GatepassRequests.Attach(entity);
            context.GatepassRequests.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(GatepassRequest entity)
        {
            onUpdate();
            context.GatepassRequests.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GatepassRequest> All()
        {
            return context.GatepassRequests.ToList();
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
	 