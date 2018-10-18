 
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
   public partial class TapagConnectionTableQueryService: EntityQueryService<TapagConnectionTable,TapagConnectionTableKeys,TapagConnectionTablePM,object,TapagConnectionTableKeys>
   {
   
        TapagConnectionTableRepository repository;
		ICustomContext  context;
        public TapagConnectionTableQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new TapagConnectionTableRepository(context);
            Repository = repository;
            mapping = new TapagConnectionTableDataMapping();
        }

        public TapagConnectionTableQueryService(TapagConnectionTableRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TapagConnectionTableDataMapping();
        }

        public TapagConnectionTableQueryService(ICustomContext context)
        {
            this.repository = new TapagConnectionTableRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TapagConnectionTableDataMapping();
        }
		 
		public  TapagConnectionTablePM GetSingle(string tapagid, string declarationid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TapagConnectionTableKeys(){ TapagId = tapagid, DeclarationId = declarationid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TapagConnectionTable entityPOCO)
        {
            TapagConnectionTableKeys entityKeys = new TapagConnectionTableKeys() { TapagId = entityPOCO.TapagId, DeclarationId = entityPOCO.DeclarationId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 