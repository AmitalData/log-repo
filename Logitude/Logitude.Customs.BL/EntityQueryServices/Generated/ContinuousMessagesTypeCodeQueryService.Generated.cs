 
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
   public partial class ContinuousMessagesTypeCodeQueryService: EntityQueryService<ContinuousMessagesTypeCode,ContinuousMessagesTypeCodeKeys,ContinuousMessagesTypeCodePM,object,ContinuousMessagesTypeCodeKeys>
   {
   
        ContinuousMessagesTypeCodeRepository repository;
		ICustomContext  context;
        public ContinuousMessagesTypeCodeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ContinuousMessagesTypeCodeRepository(context);
            Repository = repository;
            mapping = new ContinuousMessagesTypeCodeDataMapping();
        }

        public ContinuousMessagesTypeCodeQueryService(ContinuousMessagesTypeCodeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ContinuousMessagesTypeCodeDataMapping();
        }

        public ContinuousMessagesTypeCodeQueryService(ICustomContext context)
        {
            this.repository = new ContinuousMessagesTypeCodeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ContinuousMessagesTypeCodeDataMapping();
        }
		 
		public  ContinuousMessagesTypeCodePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ContinuousMessagesTypeCodeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ContinuousMessagesTypeCode entityPOCO)
        {
            ContinuousMessagesTypeCodeKeys entityKeys = new ContinuousMessagesTypeCodeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 