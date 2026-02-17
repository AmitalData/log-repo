 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Customs.BL.EntityQueryServices
{ 
   public partial class AgentTalkBackTypeQueryService: EntityQueryService<AgentTalkBackType,AgentTalkBackTypeKeys,AgentTalkBackTypePM,object,AgentTalkBackTypeKeys>
   {
   
        AgentTalkBackTypeRepository repository;
		ICustomContext  context;
        public AgentTalkBackTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new AgentTalkBackTypeRepository(context);
            Repository = repository;
            mapping = new AgentTalkBackTypeDataMapping();
        }

        public AgentTalkBackTypeQueryService(AgentTalkBackTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AgentTalkBackTypeDataMapping();
        }

        public AgentTalkBackTypeQueryService(ICustomContext context)
        {
            this.repository = new AgentTalkBackTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AgentTalkBackTypeDataMapping();
        }
		 
		public  AgentTalkBackTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AgentTalkBackTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AgentTalkBackType entityPOCO)
        {
            AgentTalkBackTypeKeys entityKeys = new AgentTalkBackTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 