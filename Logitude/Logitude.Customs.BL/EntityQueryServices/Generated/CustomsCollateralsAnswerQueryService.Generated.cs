 
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
   public partial class CustomsCollateralsAnswerQueryService: EntityQueryService<CustomsCollateralsAnswer,CustomsCollateralsAnswerKeys,CustomsCollateralsAnswerPM,CustomsCollateralPM,CustomsCollateralKeys>
   {
   
        CustomsCollateralsAnswerRepository repository;
		ICustomContext  context;
        public CustomsCollateralsAnswerQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsCollateralsAnswerRepository(context);
            Repository = repository;
            mapping = new CustomsCollateralsAnswerDataMapping();
        }

        public CustomsCollateralsAnswerQueryService(CustomsCollateralsAnswerRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsCollateralsAnswerDataMapping();
        }

        public CustomsCollateralsAnswerQueryService(ICustomContext context)
        {
            this.repository = new CustomsCollateralsAnswerRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsCollateralsAnswerDataMapping();
        }
		 
		public  CustomsCollateralsAnswerPM GetSingle(string customscollateralid, int linenumber,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsCollateralsAnswerKeys(){ CustomsCollateralId = customscollateralid, LineNumber = linenumber };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsCollateralsAnswer entityPOCO)
        {
            CustomsCollateralsAnswerKeys entityKeys = new CustomsCollateralsAnswerKeys() { CustomsCollateralId = entityPOCO.CustomsCollateralId, LineNumber = entityPOCO.LineNumber,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 