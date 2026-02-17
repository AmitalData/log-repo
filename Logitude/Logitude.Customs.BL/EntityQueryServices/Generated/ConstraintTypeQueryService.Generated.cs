 
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
   public partial class ConstraintTypeQueryService: EntityQueryService<ConstraintType,ConstraintTypeKeys,ConstraintTypePM,object,ConstraintTypeKeys>
   {
   
        ConstraintTypeRepository repository;
		ICustomContext  context;
        public ConstraintTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ConstraintTypeRepository(context);
            Repository = repository;
            mapping = new ConstraintTypeDataMapping();
        }

        public ConstraintTypeQueryService(ConstraintTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ConstraintTypeDataMapping();
        }

        public ConstraintTypeQueryService(ICustomContext context)
        {
            this.repository = new ConstraintTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ConstraintTypeDataMapping();
        }
		 
		public  ConstraintTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ConstraintTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ConstraintType entityPOCO)
        {
            ConstraintTypeKeys entityKeys = new ConstraintTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 