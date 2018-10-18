 
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
   public partial class UIMessageAdditionalQueryService: EntityQueryService<UIMessageAdditional,UIMessageAdditionalKeys,UIMessageAdditionalPM,object,UIMessageAdditionalKeys>
   {
   
        UIMessageAdditionalRepository repository;
		ICustomContext  context;
        public UIMessageAdditionalQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new UIMessageAdditionalRepository(context);
            Repository = repository;
            mapping = new UIMessageAdditionalDataMapping();
        }

        public UIMessageAdditionalQueryService(UIMessageAdditionalRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new UIMessageAdditionalDataMapping();
        }

        public UIMessageAdditionalQueryService(ICustomContext context)
        {
            this.repository = new UIMessageAdditionalRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new UIMessageAdditionalDataMapping();
        }
		 
		public  UIMessageAdditionalPM GetSingle(string id, string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new UIMessageAdditionalKeys(){ Id = id, Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(UIMessageAdditional entityPOCO)
        {
            UIMessageAdditionalKeys entityKeys = new UIMessageAdditionalKeys() { Id = entityPOCO.Id, Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 