 
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
   public partial class TeamQueryService: EntityQueryService<Team,TeamKeys,TeamPM,object,TeamKeys>
   {
   
        TeamRepository repository;
		IInfrastructureContext  context;
        public TeamQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new TeamRepository(context);
            Repository = repository;
            mapping = new TeamDataMapping();
        }

        public TeamQueryService(TeamRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TeamDataMapping();
        }

        public TeamQueryService(IInfrastructureContext context)
        {
            this.repository = new TeamRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TeamDataMapping();
        }
		 
		public  TeamPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TeamKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Team entityPOCO)
        {
            TeamKeys entityKeys = new TeamKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 