using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Devart.Data.Oracle;
using System.Diagnostics;


namespace Unifreight.Data.AmitalModel
{
    public partial class AmitalContext
    {
        public string AmitalUserGrant { get; }
        public AmitalContext(int tenantSeed, DbConnection connection, DbCompiledModel model, string connSchemaUserId)
: base(connection, model)
        {
            Debug.WriteLine("AmitalContext:" + connSchemaUserId);
            AmitalUserGrant = connSchemaUserId;

            _TenantSeed = tenantSeed;
            InitConfiguration();

        }

        private static ConcurrentDictionary<Tuple<int, string>, DbCompiledModel> _ModelCache
            = new ConcurrentDictionary<Tuple<int, string>, DbCompiledModel>();


        private static ConcurrentDictionary<Tuple<int, string>, string> _ModelName
            = new ConcurrentDictionary<Tuple<int, string>, string>();
        public static AmitalContext Create(int tenantSeed, DbConnection connection)
        {
            string ConnSchemaUserId =
            _ModelName.GetOrAdd(
                Tuple.Create(tenantSeed, connection.ConnectionString),
                t =>
                {
                    string ConnSchemaUserId1 = DbContextBaseUtil.GetSchemaAMITAL_DB(tenantSeed);
                    return ConnSchemaUserId1;
                });
            
            var compiledModel = _ModelCache.GetOrAdd(
                Tuple.Create(tenantSeed, connection.ConnectionString),
                t =>
                {
                    var modelBuilder = GetBuilder();
                    modelBuilder.SetDefaultSchema(LogitudeDBSchema.LOGITUDE_MAIN, ConnSchemaUserId);

                    var model = modelBuilder.Build(connection);
                    return model.Compile();
                });
            return new AmitalContext(tenantSeed, connection, compiledModel, ConnSchemaUserId);
        }
        /// <summary>
        /// Creates the database and/or tables for a new tenant
        /// </summary>
        public static void ProvisionTenant(string tenantSchema, DbConnection connection)
        {

            //using (var ctx = Create(tenantSchema, connection))
            //{
            //    if (!ctx.Database.Exists())
            //    {
            //        ctx.Database.Create();
            //    }
            //    //else
            //    {
            //        var createScript = ((IObjectContextAdapter)ctx).ObjectContext.CreateDatabaseScript();
            //        ctx.Database.ExecuteSqlCommand(createScript);
            //    }
            //}
        }

        public static void TestIt()
        {

            //using (var connection = new OracleConnection(@"User Id=AMINETCST_MAIN;  Password=AMINETCST_MAIN;Direct=True;Data Source=10.10.10.96;port=1521;sid=amital"))
            //{
            //    //ContactContext.ProvisionTenant("personal", connection);
            //    //ContactContext.ProvisionTenant("work", connection);

            //    using (var ctx = AmitalContext.Create("AMI593", connection))
            //    {
            //        var poco=ctx.CCUFILEMs.FirstOrDefault();

            //    }
            //}

            //using (var connection = new OracleConnection(@"User Id=AMINETCST_MAIN;  Password=AMINETCST_MAIN;Direct=True;Data Source=10.10.10.96;port=1521;sid=amital"))
            //{
            //    //ContactContext.ProvisionTenant("personal", connection);
            //    //ContactContext.ProvisionTenant("work", connection);

            //    using (var ctx = AmitalContext.Create("AMI583", connection))
            //    {
            //        var poco = ctx.CCUFILEMs.FirstOrDefault();

            //    }
            //}

        }
    }
}
