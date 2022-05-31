 
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
   public partial class AvailableStatusFieldQueryService: EntityQueryService<AvailableStatusField,AvailableStatusFieldKeys,AvailableStatusFieldPM,object,AvailableStatusFieldKeys>
   {
   
        AvailableStatusFieldRepository repository;
		ICustomContext  context;
        public AvailableStatusFieldQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new AvailableStatusFieldRepository(context);
            Repository = repository;
            mapping = new AvailableStatusFieldDataMapping();
        }

        public AvailableStatusFieldQueryService(AvailableStatusFieldRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AvailableStatusFieldDataMapping();
        }

        public AvailableStatusFieldQueryService(ICustomContext context)
        {
            this.repository = new AvailableStatusFieldRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AvailableStatusFieldDataMapping();
        }
		 
		public  AvailableStatusFieldPM GetSingle(string fieldcode,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AvailableStatusFieldKeys(){ FieldCode = fieldcode };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AvailableStatusField entityPOCO)
        {
            AvailableStatusFieldKeys entityKeys = new AvailableStatusFieldKeys() { FieldCode = entityPOCO.FieldCode,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 