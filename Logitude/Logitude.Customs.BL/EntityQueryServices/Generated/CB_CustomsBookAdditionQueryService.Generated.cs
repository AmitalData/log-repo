 
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
   public partial class CB_CustomsBookAdditionQueryService: EntityQueryService<CB_CustomsBookAddition,CB_CustomsBookAdditionKeys,CB_CustomsBookAdditionPM,object,CB_CustomsBookAdditionKeys>
   {
   
        CB_CustomsBookAdditionRepository repository;
		ICustomContext  context;
        public CB_CustomsBookAdditionQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_CustomsBookAdditionRepository(context);
            Repository = repository;
            mapping = new CB_CustomsBookAdditionDataMapping();
        }

        public CB_CustomsBookAdditionQueryService(CB_CustomsBookAdditionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_CustomsBookAdditionDataMapping();
        }

        public CB_CustomsBookAdditionQueryService(ICustomContext context)
        {
            this.repository = new CB_CustomsBookAdditionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_CustomsBookAdditionDataMapping();
        }
		 
		public  CB_CustomsBookAdditionPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_CustomsBookAdditionKeys(){ ID = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_CustomsBookAddition entityPOCO)
        {
            CB_CustomsBookAdditionKeys entityKeys = new CB_CustomsBookAdditionKeys() { ID = entityPOCO.ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 