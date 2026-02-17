 
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
   public partial class PhysicalCheckStatusMessageQueryService: EntityQueryService<PhysicalCheckStatusMessage,PhysicalCheckStatusMessageKeys,PhysicalCheckStatusMessagePM,object,PhysicalCheckStatusMessageKeys>
   {
   
        PhysicalCheckStatusMessageRepository repository;
		ICustomContext  context;
        public PhysicalCheckStatusMessageQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new PhysicalCheckStatusMessageRepository(context);
            Repository = repository;
            mapping = new PhysicalCheckStatusMessageDataMapping();
        }

        public PhysicalCheckStatusMessageQueryService(PhysicalCheckStatusMessageRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PhysicalCheckStatusMessageDataMapping();
        }

        public PhysicalCheckStatusMessageQueryService(ICustomContext context)
        {
            this.repository = new PhysicalCheckStatusMessageRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PhysicalCheckStatusMessageDataMapping();
        }
		 
		public  PhysicalCheckStatusMessagePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PhysicalCheckStatusMessageKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PhysicalCheckStatusMessage entityPOCO)
        {
            PhysicalCheckStatusMessageKeys entityKeys = new PhysicalCheckStatusMessageKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 