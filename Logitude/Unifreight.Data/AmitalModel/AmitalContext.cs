using Devart.Data.Oracle;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel
{

    public partial class AmitalContext : Simplog.Server.Infrastructure.IContext
    {
        
        static AmitalContext()
        {
            var featureSuppressOracleMonitor = true;// ConfigurationManager.AppSettings["20180122.SuppressOracleMonitor"] == "1";
            if (featureSuppressOracleMonitor)
            {

            }
            else
            {
                Devart.Data.Oracle.OracleMonitor monitor = new Devart.Data.Oracle.OracleMonitor() { IsActive = true };
            }
            var config = Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance;
            //config.Workarounds.ColumnTypeCasingConventionCompatibility = true; //if 
            
            
            ////config.DmlOptions.BatchUpdates.Enabled = true;
            //config.CodeFirstOptions.TruncateLongDefaultNames = true;

            ////config.CodeFirstOptions.ColumnTypeCasingConventionCompatibility = true ;
            ////Model.ssdl(205,6) : error 0040: The Type CHAR is not qualified with a namespace or alias. Only primitive types can be used without qualification. 
            ////Model.ssdl(206,6) : error 0040: The Type VARCHAR2 is not qualified with a namespace or alias
            config.Workarounds.ColumnTypeCasingConventionCompatibility = true; //crash if false

            // Now, you switch off schema name generation while generating DDL scripts and DML:
            //move to DbContextBase config.Workarounds.IgnoreSchemaName = true;
            
            
            ///config.Workarounds.DisableQuoting = false;
        
            
        }
#if true
        private AmitalContext()
        {
            Database.SetInitializer<AmitalContext>(null);
            //SetOracleMonitor();
        }
#else
        public AmitalContext()
            : this(GetDBCon(), 208)
        {
            Database.SetInitializer<AmitalContext>(null);
            SetOracleMonitor();

        }

        private static DbConnection GetDBCon()
        {
            OracleConnectionStringBuilder oraCSB = new OracleConnectionStringBuilder();
            oraCSB.Direct = true;
            oraCSB.Server = "10.10.10.67";
            oraCSB.Port = 1521;
            oraCSB.Sid = "amital";
            oraCSB.UserId = "amitestm";
            oraCSB.Password = "amitestm";
            OracleConnection myConnection = new OracleConnection(oraCSB.ConnectionString);

            //DbConnection con = new Devart.Data.Oracle.OracleConnection("Data Source=srv64bit;User Id=devart;Password=devart;");


            return myConnection;
        }
#endif



        //public AmitalContext(DbConnection conn,int tenantSeed)
        //   : base(conn, true)
        //{
        //    InitConfiguration();
        //    _TenantSeed = tenantSeed;
        //}

        private void InitConfiguration()
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
            //config.QueryOptions.CaseInsensitiveComparison = true;
            //config.QueryOptions.CaseInsensitiveLike = true;

            Database.SetInitializer<AmitalContext>(null);
            //SetOracleMonitor();
            //this.Database.Connection.StateChange += (sender, myStateChangeEventArgs) =>
            //{
            //    var state = myStateChangeEventArgs.CurrentState;
            //};
        }



        //public static  void SetOracleMonitor()
        //{
        //    Devart.Data.Oracle.OracleMonitor monitor = new Devart.Data.Oracle.OracleMonitor() { IsActive = true };
        //}
        public static AmitalContext GetContext(int tenantSeed)
        {            
            string dbConnectionInfo = null;
            //if (DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn )
            //{

            GlobalDB currentDb = GlobalDbHelper.GetGlobalDB(tenantSeed);
            LogitudeCustomsSettingsM settings = LogitudeSettings.GetLogitudeCustomsSettingsMInject(tenantSeed);
            dbConnectionInfo = settings == null || settings.IsConnectedToUniFreight ? currentDb.DBConnection : settings.UnfConnectionString;
            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);
            AmitalContext context = Create(tenantSeed, connection);//new AmitalContext(connection, tenantSeed);

            return context;

            //}
            //else
            //{
            //    if (LogitudeSettings.GetLogitudeCustomsSettingsMInject == null)
            //    {
            //        throw new Exception("LogitudeSettings.GetdbConnectionInfoFromTenantInject is null ,Please Init ");
            //    }
            //    var myFuncGetConn = LogitudeSettings.GetLogitudeCustomsSettingsMInject;
            //    dbConnectionInfo = myFuncGetConn(tenantSeed).UnfConnectionString;
            //    return GetContextByDBInfo(dbConnectionInfo, tenantSeed);
            //}
        }
        public static AmitalContext GetContextByDBInfo(string dbConnectionInfo, int tenantSeed)
        {
            OracleConnectionStringBuilder oraCSB = DbContextBaseUtil.GetOracleConStrBuilder(dbConnectionInfo);
            OracleConnection myConnection = new OracleConnection(oraCSB.ConnectionString);
            //config.Workarounds.DisableQuoting = true;

            //DbConnection con = new Devart.Data.Oracle.OracleConnection("Data Source=srv64bit;User Id=devart;Password=devart;");


            var context = Create( tenantSeed, myConnection);
            return context;
        }


#if false
        public bool DisableQuoting
        {
            get
            {
                return Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance.Workarounds.DisableQuoting;
            }
            set
            {
                Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance.Workarounds.DisableQuoting = value;
            }
        }
        
#endif


        //Dictionary<string, System.Collections.IList> CacheWrapper = new Dictionary<string, System.Collections.IList>();
        //protected override void Dispose(bool disposing)
        //{
        //    foreach (var item in CacheWrapper)
        //    {
        //        item.Value.Clear();

        //    }
        //    CacheWrapper.Clear(); 

        //    base.Dispose(disposing);
        //}
        readonly int _TenantSeed;
        public  int TenantSeed
        {
            get
            {
                return _TenantSeed;
            }
        }

        public DbConnection GetConnection()
        {
            return this.Database.Connection;
        }

        public DbContext GetActiveDbContext()
        {
            return this;
        }

        public void SetAsModified(object entity)
        {
            this.Entry(entity).State = EntityState.Modified;
        }

        public void DetectChanges()
        {
            this.ChangeTracker.DetectChanges();
        }

        public int SaveChanges()
        {
            try
            {
                //Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance.Workarounds.DisableQuoting = false;
                DetectChanges();
                var val = base.SaveChanges();
                return val;
            }
            finally
            {
                //Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance.Workarounds.DisableQuoting = true;
            }
            
        }

        
    }
}
