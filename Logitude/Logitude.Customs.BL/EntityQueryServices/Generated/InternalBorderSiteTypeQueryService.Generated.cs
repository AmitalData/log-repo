 
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
   public partial class InternalBorderSiteTypeQueryService: EntityQueryService<InternalBorderSiteType,InternalBorderSiteTypeKeys,InternalBorderSiteTypePM,object,InternalBorderSiteTypeKeys>
   {
   
        InternalBorderSiteTypeRepository repository;
		ICustomContext  context;
        public InternalBorderSiteTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new InternalBorderSiteTypeRepository(context);
            Repository = repository;
            mapping = new InternalBorderSiteTypeDataMapping();
        }

        public InternalBorderSiteTypeQueryService(InternalBorderSiteTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new InternalBorderSiteTypeDataMapping();
        }

        public InternalBorderSiteTypeQueryService(ICustomContext context)
        {
            this.repository = new InternalBorderSiteTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new InternalBorderSiteTypeDataMapping();
        }
		 
		public  InternalBorderSiteTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new InternalBorderSiteTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(InternalBorderSiteType entityPOCO)
        {
            InternalBorderSiteTypeKeys entityKeys = new InternalBorderSiteTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 