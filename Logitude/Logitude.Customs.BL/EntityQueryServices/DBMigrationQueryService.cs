using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class DBMigrationQueryService : EntityQueryService<DBMigration, DBMigrationKeys, DBMigrationPM, object, DBMigrationKeys>
    {
        public override void GetComposition(EntityKeyFields entityKeys, DBMigrationPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            var DBMigrationKeys = entityKeys as DBMigrationKeys;
            var myDBMigrationLineQueryService = new DBMigrationLineQueryService(context);
            
            ///entityPM.my = myDBMigrationLineQueryService.GetMulti(DBMigrationKeysKeys, true);
            
            

        }

        internal DBMigrationPM GetLastPM()
        {
            throw new NotImplementedException();
        }
    }
}
