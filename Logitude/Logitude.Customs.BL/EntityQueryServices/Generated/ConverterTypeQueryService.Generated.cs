 
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
   public partial class ConverterTypeQueryService: EntityQueryService<ConverterType,ConverterTypeKeys,ConverterTypePM,object,ConverterTypeKeys>
   {
   
        ConverterTypeRepository repository;
		ICustomContext  context;
        public ConverterTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ConverterTypeRepository(context);
            Repository = repository;
            mapping = new ConverterTypeDataMapping();
        }

        public ConverterTypeQueryService(ConverterTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ConverterTypeDataMapping();
        }

        public ConverterTypeQueryService(ICustomContext context)
        {
            this.repository = new ConverterTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ConverterTypeDataMapping();
        }
		 
		public  ConverterTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ConverterTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ConverterType entityPOCO)
        {
            ConverterTypeKeys entityKeys = new ConverterTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 