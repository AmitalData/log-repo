 
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
   public partial class TPGFileTypeQueryService: EntityQueryService<TPGFileType,TPGFileTypeKeys,TPGFileTypePM,object,TPGFileTypeKeys>
   {
   
        TPGFileTypeRepository repository;
		ICustomContext  context;
        public TPGFileTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new TPGFileTypeRepository(context);
            Repository = repository;
            mapping = new TPGFileTypeDataMapping();
        }

        public TPGFileTypeQueryService(TPGFileTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TPGFileTypeDataMapping();
        }

        public TPGFileTypeQueryService(ICustomContext context)
        {
            this.repository = new TPGFileTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TPGFileTypeDataMapping();
        }
		 
		public  TPGFileTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TPGFileTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TPGFileType entityPOCO)
        {
            TPGFileTypeKeys entityKeys = new TPGFileTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 