 
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
   public partial class CustomsDocumentsTicketQueryService: EntityQueryService<CustomsDocumentsTicket,CustomsDocumentsTicketKeys,CustomsDocumentsTicketPM,object,CustomsDocumentsTicketKeys>
   {
   
        CustomsDocumentsTicketRepository repository;
		ICustomContext  context;
        public CustomsDocumentsTicketQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsDocumentsTicketRepository(context);
            Repository = repository;
            mapping = new CustomsDocumentsTicketDataMapping();
        }

        public CustomsDocumentsTicketQueryService(CustomsDocumentsTicketRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsDocumentsTicketDataMapping();
        }

        public CustomsDocumentsTicketQueryService(ICustomContext context)
        {
            this.repository = new CustomsDocumentsTicketRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsDocumentsTicketDataMapping();
        }
		 
		public  CustomsDocumentsTicketPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsDocumentsTicketKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsDocumentsTicket entityPOCO)
        {
            CustomsDocumentsTicketKeys entityKeys = new CustomsDocumentsTicketKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 