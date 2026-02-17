 
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
   public partial class VendorCommunicationQueryService: EntityQueryService<VendorCommunication,VendorCommunicationKeys,VendorCommunicationPM,CustomsVendorPM,CustomsVendorKeys>
   {
   
        VendorCommunicationRepository repository;
		ICustomContext  context;
        public VendorCommunicationQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new VendorCommunicationRepository(context);
            Repository = repository;
            mapping = new VendorCommunicationDataMapping();
        }

        public VendorCommunicationQueryService(VendorCommunicationRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new VendorCommunicationDataMapping();
        }

        public VendorCommunicationQueryService(ICustomContext context)
        {
            this.repository = new VendorCommunicationRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new VendorCommunicationDataMapping();
        }
		 
		public  VendorCommunicationPM GetSingle(string vendorid, int linenumber,bool getComposition, bool getFromCache)
        {
             EntityKeys = new VendorCommunicationKeys(){ VendorId = vendorid, LineNumber = linenumber };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(VendorCommunication entityPOCO)
        {
            VendorCommunicationKeys entityKeys = new VendorCommunicationKeys() { VendorId = entityPOCO.VendorId, LineNumber = entityPOCO.LineNumber,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 