 
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
   public partial class MeasureTypeQueryService: EntityQueryService<MeasureType,MeasureTypeKeys,MeasureTypePM,object,MeasureTypeKeys>
   {
   
        MeasureTypeRepository repository;
		IInfrastructureContext  context;
        public MeasureTypeQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new MeasureTypeRepository(context);
            Repository = repository;
            mapping = new MeasureTypeDataMapping();
        }

        public MeasureTypeQueryService(MeasureTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new MeasureTypeDataMapping();
        }

        public MeasureTypeQueryService(IInfrastructureContext context)
        {
            this.repository = new MeasureTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new MeasureTypeDataMapping();
        }
		 
		public  MeasureTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new MeasureTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(MeasureType entityPOCO)
        {
            MeasureTypeKeys entityKeys = new MeasureTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 