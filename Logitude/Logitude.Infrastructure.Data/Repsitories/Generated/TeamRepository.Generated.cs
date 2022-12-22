 
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
   public partial class TeamRepository:IRepository<Team>
   {
   
        private IInfrastructureContext currentContext;
        public TeamRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public TeamRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  Team GetSingle(string id, int tenant)
        {
            return (from a in context.Teams
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Team> GetAll(int tenant)
        {
            return from a in context.Teams  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Team GetSingle(EntityKeyFields entityKeys)
        {
            TeamKeys keys = entityKeys as TeamKeys;
            return (from a in context.Teams
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Team entity)
        {
            onAdd();
            context.Teams.Add(entity);
        }

        public void Remove(Team entity)
        {
            context.Teams.Attach(entity);
            context.Teams.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Team entity)
        {
            onUpdate();
            context.Teams.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Team> All()
        {
            return context.Teams.ToList();
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
	 