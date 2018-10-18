 
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
   public partial class PackageMeasureQualifierQueryService: EntityQueryService<PackageMeasureQualifier,PackageMeasureQualifierKeys,PackageMeasureQualifierPM,object,PackageMeasureQualifierKeys>
   {
   
        PackageMeasureQualifierRepository repository;
		ICustomContext  context;
        public PackageMeasureQualifierQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new PackageMeasureQualifierRepository(context);
            Repository = repository;
            mapping = new PackageMeasureQualifierDataMapping();
        }

        public PackageMeasureQualifierQueryService(PackageMeasureQualifierRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PackageMeasureQualifierDataMapping();
        }

        public PackageMeasureQualifierQueryService(ICustomContext context)
        {
            this.repository = new PackageMeasureQualifierRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PackageMeasureQualifierDataMapping();
        }
		 
		public  PackageMeasureQualifierPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PackageMeasureQualifierKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PackageMeasureQualifier entityPOCO)
        {
            PackageMeasureQualifierKeys entityKeys = new PackageMeasureQualifierKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 