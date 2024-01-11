 
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
   public partial class ComputationMethodQueryService: EntityQueryService<ComputationMethod,ComputationMethodKeys,ComputationMethodPM,object,ComputationMethodKeys>
   {
   
        ComputationMethodRepository repository;
		ICustomContext  context;
        public ComputationMethodQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ComputationMethodRepository(context);
            Repository = repository;
            mapping = new ComputationMethodDataMapping();
        }

        public ComputationMethodQueryService(ComputationMethodRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ComputationMethodDataMapping();
        }

        public ComputationMethodQueryService(ICustomContext context)
        {
            this.repository = new ComputationMethodRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ComputationMethodDataMapping();
        }
		 
		public  ComputationMethodPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ComputationMethodKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ComputationMethod entityPOCO)
        {
            ComputationMethodKeys entityKeys = new ComputationMethodKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 