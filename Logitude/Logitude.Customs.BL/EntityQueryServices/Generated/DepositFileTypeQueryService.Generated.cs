 
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
   public partial class DepositFileTypeQueryService: EntityQueryService<DepositFileType,DepositFileTypeKeys,DepositFileTypePM,object,DepositFileTypeKeys>
   {
   
        DepositFileTypeRepository repository;
		ICustomContext  context;
        public DepositFileTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DepositFileTypeRepository(context);
            Repository = repository;
            mapping = new DepositFileTypeDataMapping();
        }

        public DepositFileTypeQueryService(DepositFileTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DepositFileTypeDataMapping();
        }

        public DepositFileTypeQueryService(ICustomContext context)
        {
            this.repository = new DepositFileTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DepositFileTypeDataMapping();
        }
		 
		public  DepositFileTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DepositFileTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DepositFileType entityPOCO)
        {
            DepositFileTypeKeys entityKeys = new DepositFileTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 