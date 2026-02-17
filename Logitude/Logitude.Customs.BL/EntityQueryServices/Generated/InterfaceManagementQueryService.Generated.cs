 
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
   public partial class InterfaceManagementQueryService: EntityQueryService<InterfaceManagement,InterfaceManagementKeys,InterfaceManagementPM,object,InterfaceManagementKeys>
   {
   
        InterfaceManagementRepository repository;
		ICustomContext  context;
        public InterfaceManagementQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new InterfaceManagementRepository(context);
            Repository = repository;
            mapping = new InterfaceManagementDataMapping();
        }

        public InterfaceManagementQueryService(InterfaceManagementRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new InterfaceManagementDataMapping();
        }

        public InterfaceManagementQueryService(ICustomContext context)
        {
            this.repository = new InterfaceManagementRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new InterfaceManagementDataMapping();
        }
		 
		public  InterfaceManagementPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new InterfaceManagementKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(InterfaceManagement entityPOCO)
        {
            InterfaceManagementKeys entityKeys = new InterfaceManagementKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 