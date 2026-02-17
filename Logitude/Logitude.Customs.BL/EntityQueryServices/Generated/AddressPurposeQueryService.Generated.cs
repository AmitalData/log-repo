 
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
   public partial class AddressPurposeQueryService: EntityQueryService<AddressPurpose,AddressPurposeKeys,AddressPurposePM,object,AddressPurposeKeys>
   {
   
        AddressPurposeRepository repository;
		ICustomContext  context;
        public AddressPurposeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new AddressPurposeRepository(context);
            Repository = repository;
            mapping = new AddressPurposeDataMapping();
        }

        public AddressPurposeQueryService(AddressPurposeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AddressPurposeDataMapping();
        }

        public AddressPurposeQueryService(ICustomContext context)
        {
            this.repository = new AddressPurposeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AddressPurposeDataMapping();
        }
		 
		public  AddressPurposePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AddressPurposeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AddressPurpose entityPOCO)
        {
            AddressPurposeKeys entityKeys = new AddressPurposeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 