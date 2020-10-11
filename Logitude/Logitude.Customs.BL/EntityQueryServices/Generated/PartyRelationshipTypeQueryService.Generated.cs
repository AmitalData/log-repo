 
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
   public partial class PartyRelationshipTypeQueryService: EntityQueryService<PartyRelationshipType,PartyRelationshipTypeKeys,PartyRelationshipTypePM,object,PartyRelationshipTypeKeys>
   {
   
        PartyRelationshipTypeRepository repository;
		ICustomContext  context;
        public PartyRelationshipTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new PartyRelationshipTypeRepository(context);
            Repository = repository;
            mapping = new PartyRelationshipTypeDataMapping();
        }

        public PartyRelationshipTypeQueryService(PartyRelationshipTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PartyRelationshipTypeDataMapping();
        }

        public PartyRelationshipTypeQueryService(ICustomContext context)
        {
            this.repository = new PartyRelationshipTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PartyRelationshipTypeDataMapping();
        }
		 
		public  PartyRelationshipTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PartyRelationshipTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PartyRelationshipType entityPOCO)
        {
            PartyRelationshipTypeKeys entityKeys = new PartyRelationshipTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 