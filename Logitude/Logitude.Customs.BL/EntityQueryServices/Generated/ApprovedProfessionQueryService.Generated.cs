 
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
   public partial class ApprovedProfessionQueryService: EntityQueryService<ApprovedProfession,ApprovedProfessionKeys,ApprovedProfessionPM,object,ApprovedProfessionKeys>
   {
   
        ApprovedProfessionRepository repository;
		ICustomContext  context;
        public ApprovedProfessionQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ApprovedProfessionRepository(context);
            Repository = repository;
            mapping = new ApprovedProfessionDataMapping();
        }

        public ApprovedProfessionQueryService(ApprovedProfessionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ApprovedProfessionDataMapping();
        }

        public ApprovedProfessionQueryService(ICustomContext context)
        {
            this.repository = new ApprovedProfessionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ApprovedProfessionDataMapping();
        }
		 
		public  ApprovedProfessionPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ApprovedProfessionKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ApprovedProfession entityPOCO)
        {
            ApprovedProfessionKeys entityKeys = new ApprovedProfessionKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 