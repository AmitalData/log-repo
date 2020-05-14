 
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
   public partial class NbcDeclarationTypeQueryService: EntityQueryService<NbcDeclarationType,NbcDeclarationTypeKeys,NbcDeclarationTypePM,object,NbcDeclarationTypeKeys>
   {
   
        NbcDeclarationTypeRepository repository;
		ICustomContext  context;
        public NbcDeclarationTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new NbcDeclarationTypeRepository(context);
            Repository = repository;
            mapping = new NbcDeclarationTypeDataMapping();
        }

        public NbcDeclarationTypeQueryService(NbcDeclarationTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new NbcDeclarationTypeDataMapping();
        }

        public NbcDeclarationTypeQueryService(ICustomContext context)
        {
            this.repository = new NbcDeclarationTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new NbcDeclarationTypeDataMapping();
        }
		 
		public  NbcDeclarationTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new NbcDeclarationTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(NbcDeclarationType entityPOCO)
        {
            NbcDeclarationTypeKeys entityKeys = new NbcDeclarationTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 