 
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
   public partial class CustomsBookQueryService: EntityQueryService<CustomsBook,CustomsBookKeys,CustomsBookPM,object,CustomsBookKeys>
   {
   
        CustomsBookRepository repository;
		ICustomContext  context;
        public CustomsBookQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsBookRepository(context);
            Repository = repository;
            mapping = new CustomsBookDataMapping();
        }

        public CustomsBookQueryService(CustomsBookRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsBookDataMapping();
        }

        public CustomsBookQueryService(ICustomContext context)
        {
            this.repository = new CustomsBookRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsBookDataMapping();
        }
		 
		public  CustomsBookPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsBookKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsBook entityPOCO)
        {
            CustomsBookKeys entityKeys = new CustomsBookKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 