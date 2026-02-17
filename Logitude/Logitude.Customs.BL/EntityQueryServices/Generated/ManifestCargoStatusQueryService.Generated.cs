 
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
   public partial class ManifestCargoStatusQueryService: EntityQueryService<ManifestCargoStatus,ManifestCargoStatusKeys,ManifestCargoStatusPM,object,ManifestCargoStatusKeys>
   {
   
        ManifestCargoStatusRepository repository;
		ICustomContext  context;
        public ManifestCargoStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ManifestCargoStatusRepository(context);
            Repository = repository;
            mapping = new ManifestCargoStatusDataMapping();
        }

        public ManifestCargoStatusQueryService(ManifestCargoStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ManifestCargoStatusDataMapping();
        }

        public ManifestCargoStatusQueryService(ICustomContext context)
        {
            this.repository = new ManifestCargoStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ManifestCargoStatusDataMapping();
        }
		 
		public  ManifestCargoStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ManifestCargoStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ManifestCargoStatus entityPOCO)
        {
            ManifestCargoStatusKeys entityKeys = new ManifestCargoStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 