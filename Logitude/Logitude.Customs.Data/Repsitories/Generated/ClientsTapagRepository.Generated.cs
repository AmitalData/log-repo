 
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
   public partial class ClientsTapagRepository:IRepository<ClientsTapag>
   {
   
        private ICustomContext currentContext;
        public ClientsTapagRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClientsTapagRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClientsTapag GetSingle(string id, int tenant)
        {
            return (from a in context.ClientsTapags
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ClientsTapag> GetAll(int tenant)
        {
            return from a in context.ClientsTapags  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ClientsTapag GetSingle(EntityKeyFields entityKeys)
        {
            ClientsTapagKeys keys = entityKeys as ClientsTapagKeys;
            return (from a in context.ClientsTapags
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClientsTapag entity)
        {
            onAdd();
            context.ClientsTapags.Add(entity);
        }

        public void Remove(ClientsTapag entity)
        {
            context.ClientsTapags.Attach(entity);
            context.ClientsTapags.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClientsTapag entity)
        {
            onUpdate();
            context.ClientsTapags.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClientsTapag> All()
        {
            return context.ClientsTapags.ToList();
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
	 