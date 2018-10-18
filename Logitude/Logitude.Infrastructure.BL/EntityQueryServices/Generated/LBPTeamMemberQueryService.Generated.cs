 
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
   public partial class LBPTeamMemberQueryService: EntityQueryService<LBPTeamMember,LBPTeamMemberKeys,LBPTeamMemberPM,TeamPM,TeamKeys>
   {
   
        LBPTeamMemberRepository repository;
		IInfrastructureContext  context;
        public LBPTeamMemberQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new LBPTeamMemberRepository(context);
            Repository = repository;
            mapping = new LBPTeamMemberDataMapping();
        }

        public LBPTeamMemberQueryService(LBPTeamMemberRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new LBPTeamMemberDataMapping();
        }

        public LBPTeamMemberQueryService(IInfrastructureContext context)
        {
            this.repository = new LBPTeamMemberRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new LBPTeamMemberDataMapping();
        }
		 
		public  LBPTeamMemberPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new LBPTeamMemberKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(LBPTeamMember entityPOCO)
        {
            LBPTeamMemberKeys entityKeys = new LBPTeamMemberKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 