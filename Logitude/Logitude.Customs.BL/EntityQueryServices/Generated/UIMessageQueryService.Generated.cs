 
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
   public partial class UIMessageQueryService: EntityQueryService<UIMessage,UIMessageKeys,UIMessagePM,object,UIMessageKeys>
   {
   
        UIMessageRepository repository;
		ICustomContext  context;
        public UIMessageQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new UIMessageRepository(context);
            Repository = repository;
            mapping = new UIMessageDataMapping();
        }

        public UIMessageQueryService(UIMessageRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new UIMessageDataMapping();
        }

        public UIMessageQueryService(ICustomContext context)
        {
            this.repository = new UIMessageRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new UIMessageDataMapping();
        }
		 
		public  UIMessagePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new UIMessageKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(UIMessage entityPOCO)
        {
            UIMessageKeys entityKeys = new UIMessageKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 