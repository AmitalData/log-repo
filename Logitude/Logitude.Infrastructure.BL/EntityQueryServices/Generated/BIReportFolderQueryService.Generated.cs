 
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
   public partial class BIReportFolderQueryService: EntityQueryService<BIReportFolder,BIReportFolderKeys,BIReportFolderPM,object,BIReportFolderKeys>
   {
   
        BIReportFolderRepository repository;
		IInfrastructureContext  context;
        public BIReportFolderQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new BIReportFolderRepository(context);
            Repository = repository;
            mapping = new BIReportFolderDataMapping();
        }

        public BIReportFolderQueryService(BIReportFolderRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BIReportFolderDataMapping();
        }

        public BIReportFolderQueryService(IInfrastructureContext context)
        {
            this.repository = new BIReportFolderRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BIReportFolderDataMapping();
        }
		 
		public  BIReportFolderPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BIReportFolderKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BIReportFolder entityPOCO)
        {
            BIReportFolderKeys entityKeys = new BIReportFolderKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 