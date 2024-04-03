 
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
   public partial class ServersNameRepository:IRepository<ServersName>
   {
   
        private ICustomContext currentContext;
        public ServersNameRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ServersNameRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ServersName GetSingle(string id, int tenant)
        {
            return (from a in context.ServersNames
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ServersName> GetAll(int tenant)
        {
            return from a in context.ServersNames  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ServersName GetSingle(EntityKeyFields entityKeys)
        {
            ServersNameKeys keys = entityKeys as ServersNameKeys;
            return (from a in context.ServersNames
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ServersName entity)
        {
            onAdd();
            context.ServersNames.Add(entity);
        }

        public void Remove(ServersName entity)
        {
            context.ServersNames.Attach(entity);
            context.ServersNames.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ServersName entity)
        {
            onUpdate();
            context.ServersNames.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ServersName> All()
        {
            return context.ServersNames.ToList();
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
	 