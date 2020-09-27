 
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
   public partial class ExporterRoleTypeQueryService: EntityQueryService<ExporterRoleType,ExporterRoleTypeKeys,ExporterRoleTypePM,object,ExporterRoleTypeKeys>
   {
   
        ExporterRoleTypeRepository repository;
		ICustomContext  context;
        public ExporterRoleTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ExporterRoleTypeRepository(context);
            Repository = repository;
            mapping = new ExporterRoleTypeDataMapping();
        }

        public ExporterRoleTypeQueryService(ExporterRoleTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ExporterRoleTypeDataMapping();
        }

        public ExporterRoleTypeQueryService(ICustomContext context)
        {
            this.repository = new ExporterRoleTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ExporterRoleTypeDataMapping();
        }
		 
		public  ExporterRoleTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ExporterRoleTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ExporterRoleType entityPOCO)
        {
            ExporterRoleTypeKeys entityKeys = new ExporterRoleTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 