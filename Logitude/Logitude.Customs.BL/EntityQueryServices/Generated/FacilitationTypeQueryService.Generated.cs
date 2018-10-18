 
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
   public partial class FacilitationTypeQueryService: EntityQueryService<FacilitationType,FacilitationTypeKeys,FacilitationTypePM,object,FacilitationTypeKeys>
   {
   
        FacilitationTypeRepository repository;
		ICustomContext  context;
        public FacilitationTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new FacilitationTypeRepository(context);
            Repository = repository;
            mapping = new FacilitationTypeDataMapping();
        }

        public FacilitationTypeQueryService(FacilitationTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new FacilitationTypeDataMapping();
        }

        public FacilitationTypeQueryService(ICustomContext context)
        {
            this.repository = new FacilitationTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new FacilitationTypeDataMapping();
        }
		 
		public  FacilitationTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new FacilitationTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(FacilitationType entityPOCO)
        {
            FacilitationTypeKeys entityKeys = new FacilitationTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 