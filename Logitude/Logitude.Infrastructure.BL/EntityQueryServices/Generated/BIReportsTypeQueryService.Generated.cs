 
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
   public partial class BIReportsTypeQueryService: EntityQueryService<BIReportsType,BIReportsTypeKeys,BIReportsTypePM,object,BIReportsTypeKeys>
   {
   
        BIReportsTypeRepository repository;
		IInfrastructureContext  context;
        public BIReportsTypeQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new BIReportsTypeRepository(context);
            Repository = repository;
            mapping = new BIReportsTypeDataMapping();
        }

        public BIReportsTypeQueryService(BIReportsTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BIReportsTypeDataMapping();
        }

        public BIReportsTypeQueryService(IInfrastructureContext context)
        {
            this.repository = new BIReportsTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BIReportsTypeDataMapping();
        }
		 
		public  BIReportsTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BIReportsTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BIReportsType entityPOCO)
        {
            BIReportsTypeKeys entityKeys = new BIReportsTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 