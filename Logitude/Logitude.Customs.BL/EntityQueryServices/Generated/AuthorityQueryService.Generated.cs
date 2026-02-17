 
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
   public partial class AuthorityQueryService: EntityQueryService<Authority,AuthorityKeys,AuthorityPM,object,AuthorityKeys>
   {
   
        AuthorityRepository repository;
		ICustomContext  context;
        public AuthorityQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new AuthorityRepository(context);
            Repository = repository;
            mapping = new AuthorityDataMapping();
        }

        public AuthorityQueryService(AuthorityRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AuthorityDataMapping();
        }

        public AuthorityQueryService(ICustomContext context)
        {
            this.repository = new AuthorityRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AuthorityDataMapping();
        }
		 
		public  AuthorityPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AuthorityKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Authority entityPOCO)
        {
            AuthorityKeys entityKeys = new AuthorityKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 