 
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
   public partial class BusinessRoleQueryService: EntityQueryService<BusinessRole,BusinessRoleKeys,BusinessRolePM,object,BusinessRoleKeys>
   {
   
        BusinessRoleRepository repository;
		IInfrastructureContext  context;
        public BusinessRoleQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new BusinessRoleRepository(context);
            Repository = repository;
            mapping = new BusinessRoleDataMapping();
        }

        public BusinessRoleQueryService(BusinessRoleRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BusinessRoleDataMapping();
        }

        public BusinessRoleQueryService(IInfrastructureContext context)
        {
            this.repository = new BusinessRoleRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BusinessRoleDataMapping();
        }
		 
		public  BusinessRolePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BusinessRoleKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BusinessRole entityPOCO)
        {
            BusinessRoleKeys entityKeys = new BusinessRoleKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 