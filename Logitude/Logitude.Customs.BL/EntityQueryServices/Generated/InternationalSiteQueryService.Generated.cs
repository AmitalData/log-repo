 
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
   public partial class InternationalSiteQueryService: EntityQueryService<InternationalSite,InternationalSiteKeys,InternationalSitePM,object,InternationalSiteKeys>
   {
   
        InternationalSiteRepository repository;
		ICustomContext  context;
        public InternationalSiteQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new InternationalSiteRepository(context);
            Repository = repository;
            mapping = new InternationalSiteDataMapping();
        }

        public InternationalSiteQueryService(InternationalSiteRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new InternationalSiteDataMapping();
        }

        public InternationalSiteQueryService(ICustomContext context)
        {
            this.repository = new InternationalSiteRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new InternationalSiteDataMapping();
        }
		 
		public  InternationalSitePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new InternationalSiteKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(InternationalSite entityPOCO)
        {
            InternationalSiteKeys entityKeys = new InternationalSiteKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 