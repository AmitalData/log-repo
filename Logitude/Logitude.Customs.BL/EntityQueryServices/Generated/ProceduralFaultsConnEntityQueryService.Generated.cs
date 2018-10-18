 
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
   public partial class ProceduralFaultsConnEntityQueryService: EntityQueryService<ProceduralFaultsConnEntity,ProceduralFaultsConnEntityKeys,ProceduralFaultsConnEntityPM,ProceduralFaultPM,ProceduralFaultKeys>
   {
   
        ProceduralFaultsConnEntityRepository repository;
		ICustomContext  context;
        public ProceduralFaultsConnEntityQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ProceduralFaultsConnEntityRepository(context);
            Repository = repository;
            mapping = new ProceduralFaultsConnEntityDataMapping();
        }

        public ProceduralFaultsConnEntityQueryService(ProceduralFaultsConnEntityRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ProceduralFaultsConnEntityDataMapping();
        }

        public ProceduralFaultsConnEntityQueryService(ICustomContext context)
        {
            this.repository = new ProceduralFaultsConnEntityRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ProceduralFaultsConnEntityDataMapping();
        }
		 
		public  ProceduralFaultsConnEntityPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ProceduralFaultsConnEntityKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ProceduralFaultsConnEntity entityPOCO)
        {
            ProceduralFaultsConnEntityKeys entityKeys = new ProceduralFaultsConnEntityKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 