 
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
   public partial class AmendmentFieldStatusQueryService: EntityQueryService<AmendmentFieldStatus,AmendmentFieldStatusKeys,AmendmentFieldStatusPM,object,AmendmentFieldStatusKeys>
   {
   
        AmendmentFieldStatusRepository repository;
		ICustomContext  context;
        public AmendmentFieldStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new AmendmentFieldStatusRepository(context);
            Repository = repository;
            mapping = new AmendmentFieldStatusDataMapping();
        }

        public AmendmentFieldStatusQueryService(AmendmentFieldStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AmendmentFieldStatusDataMapping();
        }

        public AmendmentFieldStatusQueryService(ICustomContext context)
        {
            this.repository = new AmendmentFieldStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AmendmentFieldStatusDataMapping();
        }
		 
		public  AmendmentFieldStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AmendmentFieldStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AmendmentFieldStatus entityPOCO)
        {
            AmendmentFieldStatusKeys entityKeys = new AmendmentFieldStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 