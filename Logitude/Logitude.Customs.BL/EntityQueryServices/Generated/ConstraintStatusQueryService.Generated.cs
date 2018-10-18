 
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
   public partial class ConstraintStatusQueryService: EntityQueryService<ConstraintStatus,ConstraintStatusKeys,ConstraintStatusPM,object,ConstraintStatusKeys>
   {
   
        ConstraintStatusRepository repository;
		ICustomContext  context;
        public ConstraintStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ConstraintStatusRepository(context);
            Repository = repository;
            mapping = new ConstraintStatusDataMapping();
        }

        public ConstraintStatusQueryService(ConstraintStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ConstraintStatusDataMapping();
        }

        public ConstraintStatusQueryService(ICustomContext context)
        {
            this.repository = new ConstraintStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ConstraintStatusDataMapping();
        }
		 
		public  ConstraintStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ConstraintStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ConstraintStatus entityPOCO)
        {
            ConstraintStatusKeys entityKeys = new ConstraintStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 