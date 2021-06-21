 
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
   public partial class ContainerizationStatusCodeQueryService: EntityQueryService<ContainerizationStatusCode,ContainerizationStatusCodeKeys,ContainerizationStatusCodePM,object,ContainerizationStatusCodeKeys>
   {
   
        ContainerizationStatusCodeRepository repository;
		ICustomContext  context;
        public ContainerizationStatusCodeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ContainerizationStatusCodeRepository(context);
            Repository = repository;
            mapping = new ContainerizationStatusCodeDataMapping();
        }

        public ContainerizationStatusCodeQueryService(ContainerizationStatusCodeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ContainerizationStatusCodeDataMapping();
        }

        public ContainerizationStatusCodeQueryService(ICustomContext context)
        {
            this.repository = new ContainerizationStatusCodeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ContainerizationStatusCodeDataMapping();
        }
		 
		public  ContainerizationStatusCodePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ContainerizationStatusCodeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ContainerizationStatusCode entityPOCO)
        {
            ContainerizationStatusCodeKeys entityKeys = new ContainerizationStatusCodeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 