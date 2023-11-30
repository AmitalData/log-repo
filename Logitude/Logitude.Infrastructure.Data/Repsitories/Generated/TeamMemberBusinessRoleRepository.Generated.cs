 
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
   public partial class TeamMemberBusinessRoleRepository:IRepository<TeamMemberBusinessRole>
   {
   
        private IInfrastructureContext currentContext;
        public TeamMemberBusinessRoleRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public TeamMemberBusinessRoleRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  TeamMemberBusinessRole GetSingle(string id, int tenant)
        {
            return (from a in context.TeamMemberBusinessRoles
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TeamMemberBusinessRole> GetAll(int tenant)
        {
            return from a in context.TeamMemberBusinessRoles  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TeamMemberBusinessRole GetSingle(EntityKeyFields entityKeys)
        {
            TeamMemberBusinessRoleKeys keys = entityKeys as TeamMemberBusinessRoleKeys;
            return (from a in context.TeamMemberBusinessRoles
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TeamMemberBusinessRole entity)
        {
            onAdd();
            context.TeamMemberBusinessRoles.Add(entity);
        }

        public void Remove(TeamMemberBusinessRole entity)
        {
            context.TeamMemberBusinessRoles.Attach(entity);
            context.TeamMemberBusinessRoles.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TeamMemberBusinessRole entity)
        {
            onUpdate();
            context.TeamMemberBusinessRoles.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TeamMemberBusinessRole> All()
        {
            return context.TeamMemberBusinessRoles.ToList();
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
	 