 
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
   public partial class ContainerizationHataraStatusQueryService: EntityQueryService<ContainerizationHataraStatus,ContainerizationHataraStatusKeys,ContainerizationHataraStatusPM,object,ContainerizationHataraStatusKeys>
   {
   
        ContainerizationHataraStatusRepository repository;
		ICustomContext  context;
        public ContainerizationHataraStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ContainerizationHataraStatusRepository(context);
            Repository = repository;
            mapping = new ContainerizationHataraStatusDataMapping();
        }

        public ContainerizationHataraStatusQueryService(ContainerizationHataraStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ContainerizationHataraStatusDataMapping();
        }

        public ContainerizationHataraStatusQueryService(ICustomContext context)
        {
            this.repository = new ContainerizationHataraStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ContainerizationHataraStatusDataMapping();
        }
		 
		public  ContainerizationHataraStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ContainerizationHataraStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ContainerizationHataraStatus entityPOCO)
        {
            ContainerizationHataraStatusKeys entityKeys = new ContainerizationHataraStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 