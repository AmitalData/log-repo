 
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
   public partial class BIFoldersPermissionQueryService: EntityQueryService<BIFoldersPermission,BIFoldersPermissionKeys,BIFoldersPermissionPM,BIReportFolderPM,BIReportFolderKeys>
   {
   
        BIFoldersPermissionRepository repository;
		IInfrastructureContext  context;
        public BIFoldersPermissionQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new BIFoldersPermissionRepository(context);
            Repository = repository;
            mapping = new BIFoldersPermissionDataMapping();
        }

        public BIFoldersPermissionQueryService(BIFoldersPermissionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BIFoldersPermissionDataMapping();
        }

        public BIFoldersPermissionQueryService(IInfrastructureContext context)
        {
            this.repository = new BIFoldersPermissionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BIFoldersPermissionDataMapping();
        }
		 
		public  BIFoldersPermissionPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BIFoldersPermissionKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BIFoldersPermission entityPOCO)
        {
            BIFoldersPermissionKeys entityKeys = new BIFoldersPermissionKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 