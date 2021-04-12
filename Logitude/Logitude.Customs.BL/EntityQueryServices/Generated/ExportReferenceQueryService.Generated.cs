 
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
   public partial class ExportReferenceQueryService: EntityQueryService<ExportReference,ExportReferenceKeys,ExportReferencePM,Customs.ExportStorgePM,Customs.ExportStorgeKeys>
   {
   
        ExportReferenceRepository repository;
		ICustomContext  context;
        public ExportReferenceQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ExportReferenceRepository(context);
            Repository = repository;
            mapping = new ExportReferenceDataMapping();
        }

        public ExportReferenceQueryService(ExportReferenceRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ExportReferenceDataMapping();
        }

        public ExportReferenceQueryService(ICustomContext context)
        {
            this.repository = new ExportReferenceRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ExportReferenceDataMapping();
        }
		 
		public  ExportReferencePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ExportReferenceKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ExportReference entityPOCO)
        {
            ExportReferenceKeys entityKeys = new ExportReferenceKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 