 
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
   public partial class DBMigrationLineQueryService: EntityQueryService<DBMigrationLine,DBMigrationLineKeys,DBMigrationLinePM,DBMigrationPM,DBMigrationKeys>
   {
   
        DBMigrationLineRepository repository;
		ICustomContext  context;
        public DBMigrationLineQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DBMigrationLineRepository(context);
            Repository = repository;
            mapping = new DBMigrationLineDataMapping();
        }

        public DBMigrationLineQueryService(DBMigrationLineRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DBMigrationLineDataMapping();
        }

        public DBMigrationLineQueryService(ICustomContext context)
        {
            this.repository = new DBMigrationLineRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DBMigrationLineDataMapping();
        }
		 
		public  DBMigrationLinePM GetSingle(string id, int counterkey,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DBMigrationLineKeys(){ Id = id, CounterKey = counterkey };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DBMigrationLine entityPOCO)
        {
            DBMigrationLineKeys entityKeys = new DBMigrationLineKeys() { Id = entityPOCO.Id, CounterKey = entityPOCO.CounterKey,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 