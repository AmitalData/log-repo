 
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
   public partial class InceptionCodeQueryService: EntityQueryService<InceptionCode,InceptionCodeKeys,InceptionCodePM,object,InceptionCodeKeys>
   {
   
        InceptionCodeRepository repository;
		ICustomContext  context;
        public InceptionCodeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new InceptionCodeRepository(context);
            Repository = repository;
            mapping = new InceptionCodeDataMapping();
        }

        public InceptionCodeQueryService(InceptionCodeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new InceptionCodeDataMapping();
        }

        public InceptionCodeQueryService(ICustomContext context)
        {
            this.repository = new InceptionCodeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new InceptionCodeDataMapping();
        }
		 
		public  InceptionCodePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new InceptionCodeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(InceptionCode entityPOCO)
        {
            InceptionCodeKeys entityKeys = new InceptionCodeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 