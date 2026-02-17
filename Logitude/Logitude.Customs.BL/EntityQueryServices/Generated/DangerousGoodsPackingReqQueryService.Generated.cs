 
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
   public partial class DangerousGoodsPackingReqQueryService: EntityQueryService<DangerousGoodsPackingReq,DangerousGoodsPackingReqKeys,DangerousGoodsPackingReqPM,object,DangerousGoodsPackingReqKeys>
   {
   
        DangerousGoodsPackingReqRepository repository;
		ICustomContext  context;
        public DangerousGoodsPackingReqQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DangerousGoodsPackingReqRepository(context);
            Repository = repository;
            mapping = new DangerousGoodsPackingReqDataMapping();
        }

        public DangerousGoodsPackingReqQueryService(DangerousGoodsPackingReqRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DangerousGoodsPackingReqDataMapping();
        }

        public DangerousGoodsPackingReqQueryService(ICustomContext context)
        {
            this.repository = new DangerousGoodsPackingReqRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DangerousGoodsPackingReqDataMapping();
        }
		 
		public  DangerousGoodsPackingReqPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DangerousGoodsPackingReqKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DangerousGoodsPackingReq entityPOCO)
        {
            DangerousGoodsPackingReqKeys entityKeys = new DangerousGoodsPackingReqKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 