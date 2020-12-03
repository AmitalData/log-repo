 
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
   public partial class ConsignmentPackDangerQueryService: EntityQueryService<ConsignmentPackDanger,ConsignmentPackDangerKeys,ConsignmentPackDangerPM,ConsignmentPackagePM,ConsignmentPackageKeys>
   {
   
        ConsignmentPackDangerRepository repository;
		ICustomContext  context;
        public ConsignmentPackDangerQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ConsignmentPackDangerRepository(context);
            Repository = repository;
            mapping = new ConsignmentPackDangerDataMapping();
        }

        public ConsignmentPackDangerQueryService(ConsignmentPackDangerRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ConsignmentPackDangerDataMapping();
        }

        public ConsignmentPackDangerQueryService(ICustomContext context)
        {
            this.repository = new ConsignmentPackDangerRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ConsignmentPackDangerDataMapping();
        }
		 
		public  ConsignmentPackDangerPM GetSingle(string declarationid, int? consignmentnumber, int? linenumber, int? dangerouslineno,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ConsignmentPackDangerKeys(){ DeclarationId = declarationid, ConsignmentNumber = consignmentnumber, LineNumber = linenumber, DangerousLineNo = dangerouslineno };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ConsignmentPackDanger entityPOCO)
        {
            ConsignmentPackDangerKeys entityKeys = new ConsignmentPackDangerKeys() { DeclarationId = entityPOCO.DeclarationId, ConsignmentNumber = entityPOCO.ConsignmentNumber, LineNumber = entityPOCO.LineNumber, DangerousLineNo = entityPOCO.DangerousLineNo,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 