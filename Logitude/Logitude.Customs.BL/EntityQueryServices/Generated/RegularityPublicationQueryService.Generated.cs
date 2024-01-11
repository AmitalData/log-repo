 
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
   public partial class RegularityPublicationQueryService: EntityQueryService<RegularityPublication,RegularityPublicationKeys,RegularityPublicationPM,object,RegularityPublicationKeys>
   {
   
        RegularityPublicationRepository repository;
		ICustomContext  context;
        public RegularityPublicationQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new RegularityPublicationRepository(context);
            Repository = repository;
            mapping = new RegularityPublicationDataMapping();
        }

        public RegularityPublicationQueryService(RegularityPublicationRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new RegularityPublicationDataMapping();
        }

        public RegularityPublicationQueryService(ICustomContext context)
        {
            this.repository = new RegularityPublicationRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new RegularityPublicationDataMapping();
        }
		 
		public  RegularityPublicationPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new RegularityPublicationKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(RegularityPublication entityPOCO)
        {
            RegularityPublicationKeys entityKeys = new RegularityPublicationKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 