 
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
   public partial class ContactRoleTypeQueryService: EntityQueryService<ContactRoleType,ContactRoleTypeKeys,ContactRoleTypePM,object,ContactRoleTypeKeys>
   {
   
        ContactRoleTypeRepository repository;
		ICustomContext  context;
        public ContactRoleTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ContactRoleTypeRepository(context);
            Repository = repository;
            mapping = new ContactRoleTypeDataMapping();
        }

        public ContactRoleTypeQueryService(ContactRoleTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ContactRoleTypeDataMapping();
        }

        public ContactRoleTypeQueryService(ICustomContext context)
        {
            this.repository = new ContactRoleTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ContactRoleTypeDataMapping();
        }
		 
		public  ContactRoleTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ContactRoleTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ContactRoleType entityPOCO)
        {
            ContactRoleTypeKeys entityKeys = new ContactRoleTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 