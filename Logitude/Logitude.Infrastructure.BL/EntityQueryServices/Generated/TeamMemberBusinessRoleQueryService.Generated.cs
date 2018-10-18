 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityDataMappings;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityKeys;
using Logitude.Infrastructure.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Infrastructure.BL.EntityQueryServices
{ 
   public partial class TeamMemberBusinessRoleQueryService: EntityQueryService<TeamMemberBusinessRole,TeamMemberBusinessRoleKeys,TeamMemberBusinessRolePM,LBPTeamMemberPM,LBPTeamMemberKeys>
   {
   
        TeamMemberBusinessRoleRepository repository;
		IInfrastructureContext  context;
        public TeamMemberBusinessRoleQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new TeamMemberBusinessRoleRepository(context);
            Repository = repository;
            mapping = new TeamMemberBusinessRoleDataMapping();
        }

        public TeamMemberBusinessRoleQueryService(TeamMemberBusinessRoleRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TeamMemberBusinessRoleDataMapping();
        }

        public TeamMemberBusinessRoleQueryService(IInfrastructureContext context)
        {
            this.repository = new TeamMemberBusinessRoleRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TeamMemberBusinessRoleDataMapping();
        }
		 
		public  TeamMemberBusinessRolePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TeamMemberBusinessRoleKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TeamMemberBusinessRole entityPOCO)
        {
            TeamMemberBusinessRoleKeys entityKeys = new TeamMemberBusinessRoleKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 