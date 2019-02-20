 
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
   public partial class GatepassRequestQueryService: EntityQueryService<GatepassRequest,GatepassRequestKeys,GatepassRequestPM,object,GatepassRequestKeys>
   {
   
        GatepassRequestRepository repository;
		ICustomContext  context;
        public GatepassRequestQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new GatepassRequestRepository(context);
            Repository = repository;
            mapping = new GatepassRequestDataMapping();
        }

        public GatepassRequestQueryService(GatepassRequestRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new GatepassRequestDataMapping();
        }

        public GatepassRequestQueryService(ICustomContext context)
        {
            this.repository = new GatepassRequestRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new GatepassRequestDataMapping();
        }
		 
		public  GatepassRequestPM GetSingle(string mastercourierid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new GatepassRequestKeys(){ MasterCourierId = mastercourierid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(GatepassRequest entityPOCO)
        {
            GatepassRequestKeys entityKeys = new GatepassRequestKeys() { MasterCourierId = entityPOCO.MasterCourierId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 