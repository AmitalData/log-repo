 
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
   public partial class ConsignmentQueryService: EntityQueryService<Consignment,ConsignmentKeys,ConsignmentPM,DeclarationPM,DeclarationKeys>
   {
   
        ConsignmentRepository repository;
		ICustomContext  context;
        public ConsignmentQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ConsignmentRepository(context);
            Repository = repository;
            mapping = new ConsignmentDataMapping();
        }

        public ConsignmentQueryService(ConsignmentRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ConsignmentDataMapping();
        }

        public ConsignmentQueryService(ICustomContext context)
        {
            this.repository = new ConsignmentRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ConsignmentDataMapping();
        }
		 
		public  ConsignmentPM GetSingle(string declarationid, int? consignmentnumber,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ConsignmentKeys(){ DeclarationId = declarationid, ConsignmentNumber = consignmentnumber };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Consignment entityPOCO)
        {
            ConsignmentKeys entityKeys = new ConsignmentKeys() { DeclarationId = entityPOCO.DeclarationId, ConsignmentNumber = entityPOCO.ConsignmentNumber,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 