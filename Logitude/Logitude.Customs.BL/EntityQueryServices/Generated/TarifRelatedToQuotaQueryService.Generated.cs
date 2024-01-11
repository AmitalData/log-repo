 
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
   public partial class TarifRelatedToQuotaQueryService: EntityQueryService<TarifRelatedToQuota,TarifRelatedToQuotaKeys,TarifRelatedToQuotaPM,object,TarifRelatedToQuotaKeys>
   {
   
        TarifRelatedToQuotaRepository repository;
		ICustomContext  context;
        public TarifRelatedToQuotaQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new TarifRelatedToQuotaRepository(context);
            Repository = repository;
            mapping = new TarifRelatedToQuotaDataMapping();
        }

        public TarifRelatedToQuotaQueryService(TarifRelatedToQuotaRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TarifRelatedToQuotaDataMapping();
        }

        public TarifRelatedToQuotaQueryService(ICustomContext context)
        {
            this.repository = new TarifRelatedToQuotaRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TarifRelatedToQuotaDataMapping();
        }
		 
		public  TarifRelatedToQuotaPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TarifRelatedToQuotaKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TarifRelatedToQuota entityPOCO)
        {
            TarifRelatedToQuotaKeys entityKeys = new TarifRelatedToQuotaKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 