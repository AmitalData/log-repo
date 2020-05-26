 
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
   public partial class ClassificationTypeQueryService: EntityQueryService<ClassificationType,ClassificationTypeKeys,ClassificationTypePM,object,ClassificationTypeKeys>
   {
   
        ClassificationTypeRepository repository;
		ICustomContext  context;
        public ClassificationTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ClassificationTypeRepository(context);
            Repository = repository;
            mapping = new ClassificationTypeDataMapping();
        }

        public ClassificationTypeQueryService(ClassificationTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ClassificationTypeDataMapping();
        }

        public ClassificationTypeQueryService(ICustomContext context)
        {
            this.repository = new ClassificationTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ClassificationTypeDataMapping();
        }
		 
		public  ClassificationTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ClassificationTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ClassificationType entityPOCO)
        {
            ClassificationTypeKeys entityKeys = new ClassificationTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 