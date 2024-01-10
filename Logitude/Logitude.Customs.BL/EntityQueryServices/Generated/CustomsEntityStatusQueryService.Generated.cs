 
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
   public partial class CustomsEntityStatusQueryService: EntityQueryService<CustomsEntityStatus,CustomsEntityStatusKeys,CustomsEntityStatusPM,object,CustomsEntityStatusKeys>
   {
   
        CustomsEntityStatusRepository repository;
		ICustomContext  context;
        public CustomsEntityStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsEntityStatusRepository(context);
            Repository = repository;
            mapping = new CustomsEntityStatusDataMapping();
        }

        public CustomsEntityStatusQueryService(CustomsEntityStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsEntityStatusDataMapping();
        }

        public CustomsEntityStatusQueryService(ICustomContext context)
        {
            this.repository = new CustomsEntityStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsEntityStatusDataMapping();
        }
		 
		public  CustomsEntityStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsEntityStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsEntityStatus entityPOCO)
        {
            CustomsEntityStatusKeys entityKeys = new CustomsEntityStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 