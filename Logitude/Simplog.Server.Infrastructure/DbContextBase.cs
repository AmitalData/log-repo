using Devart.Data.Oracle;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace Simplog.Server.Infrastructure
{
    public enum LogitudeDBSchema
    {
        none, LOGITUDE_GLOBAL, LOGITUDE_MAIN, LOGITUDE_LOGS, AMITAL_DB
    }

    public abstract class DbContextBase : DbContext
    {
        FixedSizedQueue<string> _MyLogQueue = new FixedSizedQueue<string>(30);
        abstract public LogitudeDBSchema LogitudeDBSchema { get; }

        public DbContextBase(DbConnection connection, DbCompiledModel model)
: base(connection, model, contextOwnsConnection: false)
        {
            Database.Connection.StateChange += Connection_StateChange;
        }

        public DbContextBase()
            : base()
        {
            if (LogitudeSettings.DatabaseManagementSystem.Equals("oracle", StringComparison.OrdinalIgnoreCase))
            {
                var itzikHave2rememberToCheck = true;
                if (itzikHave2rememberToCheck)
                {
                    //SELECT * FROM AMINEt_MAIN.Declarations Extent1 WHERE((Extent1.DeclarationNumber = :p__linq__0) OR ((Extent1.DeclarationNumber IS NULL) AND(:p__linq__0 IS NULL))) AND(Extent1.Tenant = :p__linq__1)
                    (this as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 
                }
            }

            Database.Connection.StateChange += Connection_StateChange;
            ///this.Database.CommandTimeout = 240;
            InitLog();
        }
        public override int SaveChanges()
        {
            bool suppressThrow = false;
            var saveChangeLogger = CreateLogger();
            var commandTimeout = this.Database.CommandTimeout;
            try
            {
                var to = 2 * 60;
                if (Transaction.Current != null)
                {
                    //to =Transaction.Current. 
                }

                if (LogitudeSettings.DatabaseManagementSystem == "oracle")
                {
                    this.Database.CommandTimeout = to;
                }
                else if(ApplicationAppInfo.WorkerRoleCall)
                {
                    this.Database.CommandTimeout = 10; 
                }
                else if (!ApplicationAppInfo.WorkerRoleCall)
                {
                    this.Database.CommandTimeout = 15; 
                }
                var intSave = base.SaveChanges();
                var testEx = false;
                if (testEx)
                {
                    throw new Exception("TTTEST");
                }
                return intSave;
            }
            catch (DbEntityValidationException myDbEntityValidationException)
            {
                var e1 = ExceptionFormatDbEntityUtil.GetFormated(myDbEntityValidationException);
                AmitalDebuggerUtil.Break(AmitalDebuggerLevel.Information);
                if (suppressThrow)
                {
                    return -999;
                }
                throw e1;
            }
            catch (DbUpdateException dbu)
            {
                FormatDbUpdateException(dbu);
                if (this.LogitudeDBSchema == LogitudeDBSchema.LOGITUDE_MAIN)
                {
                    if (LogitudeSettings.HandleDbExceptionInject != null)
                    {

                        LogitudeSettings.HandleDbExceptionInject(dbu, "SaveChanges:DbUpdateException", GetString4LogGroupBy(saveChangeLogger, Environment.StackTrace.ToString()));
                    }
                }
                //AmitalDebuggerUtil.Break(AmitalDebuggerLevel.Error); 
                if (suppressThrow)
                {
                    return -999;
                }

                throw dbu;
            }
            catch (Exception e)
            {

                if (this.LogitudeDBSchema != LogitudeDBSchema.LOGITUDE_LOGS && LogitudeSettings.HandleDbExceptionInject != null)
                {
                    LogitudeSettings.HandleDbExceptionInject(e, "SaveChanges:Exception", GetString4LogGroupBy(saveChangeLogger, Environment.StackTrace.ToString()));
                }
                else
                {
                    AmitalDebuggerUtil.Break(AmitalDebuggerLevel.Critical);
                }
                if (suppressThrow)
                {
                    return -999;
                }

                throw;
            }

            finally
            {
                saveChangeLogger.Dispose();
                this.Database.CommandTimeout = commandTimeout;
            }

        }

        private static string GetString4LogGroupBy(IDbContextLogger saveChangeLogger, string EnvironmentStackTraceToString)
        {

            string mySaveLog = saveChangeLogger.ToString() ?? "";
            string myStack = EnvironmentStackTraceToString ?? "";
            string myHeader = "";
            try
            {
                string[] lines = mySaveLog
                    .Split(Environment.NewLine.ToCharArray())
                    .Skip(1)
                    .ToArray();

                mySaveLog = string.Join(Environment.NewLine, lines);
                mySaveLog = mySaveLog.TrimStart(Environment.NewLine.ToCharArray());
                myHeader = EnvironmentStackTraceToString.Split(Environment.NewLine.ToCharArray()).LastOrDefault(l => l.Contains(".cs:line")) ?? "";
                myHeader += ":";
            }
            catch { }
            return myHeader + "~" + mySaveLog + "~" + myStack; ;




        }

        private void FormatDbUpdateException(DbUpdateException dbu)
        {
            var builder = new StringBuilder("A DbUpdateException was caught while saving changes. ");

            try
            {
                foreach (var result in dbu.Entries)
                {
                    builder.AppendFormat("Type: {0} was part of the problem. ", result.Entity.GetType().Name).AppendLine();


                    if (result.State != EntityState.Deleted && result.CurrentValues != null)
                    {

                        foreach (var propertyName in result.CurrentValues.PropertyNames)
                        {
                            builder.AppendLine().AppendFormat("Property Name: {0}", propertyName);

                            if (result.State != EntityState.Added)
                            {
                                //get original value
                                var orgVal = result.OriginalValues[propertyName];
                                builder.AppendFormat("     Original Value: {0}", orgVal);
                            }
                            //get current values
                            var curVal = result.CurrentValues[propertyName];
                            builder.AppendFormat("     Current Value: {0}", curVal);
                        }


                    }


                }
            }
            catch (Exception e)
            {
                builder.Append("Error parsing DbUpdateException: " + e.ToString());
            }

            string message = builder.ToString();
            dbu.ChangeExceptionMess(message);
            //return dbu;
        }

        public IDbContextLogger CreateLogger()
        {
            //this.Database.Log = null;
            return new DbContextLogger(this.Database) as IDbContextLogger;
            //this.Database.Log += DbContextLogger.LogMe;

        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            _MyLogQueue.ToList().ForEach(
                m =>
                {
                    sb.AppendLine(m);
                });
            return base.ToString() + sb.ToString();

        }
        private void InitLog()
        {
            if (LogitudeSettings.DatabaseManagementSystem != "oracle")
            {
                return;
            }
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                return;
            }


            if (DbContextBaseUtil.ToLog == null)
            {
                DbContextBaseUtil.ToLog = false;

                Debug.WriteLine(@"DbContextBase:ToLog:(Default:False due Memory Leak if not Disposed)Any time any place u can set: 
Simplog.Server.Infrastructure.DbContextBaseUtil.ToLog =true;");
                AmitalDebuggerUtil.Break();
            }
            Simplog.Server.Infrastructure.DbContextBaseUtil.ToLog = Simplog.Server.Infrastructure.DbContextBaseUtil.ToLog;
            if (DbContextBaseUtil.ToLog.GetValueOrDefault())
            {
                Simplog.Server.Infrastructure.Helpers.TransactionFactory.RegisterTransactionCompleted();
                this.Database.Log += EnqueueLog;
            }

        }
        private void EnqueueLog(string mess)
        {
            try
            {
                if (mess == Environment.NewLine)
                {
                    return;
                }
                Debug.WriteLine(mess);
                _MyLogQueue.Enqueue(mess);
            }
            catch (Exception)
            {

                throw;
            }

        }

        protected override void Dispose(bool disposing)
        {
            try
            {


                if (this.Database != null)
                {
                    this.Database.Log -= EnqueueLog;
                    Database.Connection.StateChange -= Connection_StateChange;

                }
            }
            catch (Exception)
            {


            }

            base.Dispose(disposing);
        }
        public DbContextBase(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Database.Connection.StateChange += Connection_StateChange;
            InitLog();
        }
        public DbContextBase(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
            Database.Connection.StateChange += Connection_StateChange;
            InitLog();
        }

        private void Connection_StateChange(object sender, StateChangeEventArgs args)
        {
            if (LogitudeSettings.System2RedirectFraction == 0)
            {
                return;
            }
            if (ApplySnapshotIsolation())
            {
                SetTransactionIsolationLevel(args);
            }
        }

        private bool ApplySnapshotIsolation()
        {
            Random random = new Random();
            var LuckyNumber = random.Next(1, 101);
            return ((LuckyNumber % LogitudeSettings.System2RedirectFraction) == 0);
        }

        private void SetTransactionIsolationLevel(StateChangeEventArgs args)
        {
            if (args.CurrentState == ConnectionState.Open && args.OriginalState != ConnectionState.Open)
            {
               
                    using (var command = Database.Connection.CreateCommand())
                    {
                        if (Transaction.Current == null)
                        {
                            command.CommandText = "SET TRANSACTION ISOLATION LEVEL SNAPSHOT";
                        }
       
                        else
                        {
                            switch (Transaction.Current.IsolationLevel)
                            {
                                case IsolationLevel.ReadCommitted:
                                    command.CommandText = "SET TRANSACTION ISOLATION LEVEL READ COMMITTED";
                                    break;
                                case IsolationLevel.ReadUncommitted:
                                    command.CommandText = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED";
                                    break;
                                case IsolationLevel.Snapshot:
                                    command.CommandText = "SET TRANSACTION ISOLATION LEVEL SNAPSHOT";
                                    break;
                                case IsolationLevel.Serializable:
                                    command.CommandText = "SET TRANSACTION ISOLATION LEVEL SERIALIZABLE";
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            command.ExecuteNonQuery();
                        }
                }
            }
        }

 static string _UserSlashPass = null;
        public void ExecuteInSys(string mainConnectionString, List<string> unifreightTables, Func<string> GetConnetionStringFunc)
        {
            var UserSlashPass = _UserSlashPass??GetConnetionStringFunc();
            var UserSlashPassList = new List<string>(UserSlashPass.Split(new char[] { '/' }));

            var main_ocsb = new OracleConnectionStringBuilder(mainConnectionString);
            var sys = new OracleConnectionStringBuilder()
            {
                Direct = main_ocsb.Direct,
                Server = main_ocsb.Server,
                Port = main_ocsb.Port,
                Sid = main_ocsb.Sid,
                UserId = UserSlashPassList[0],//  "system",
                Password = UserSlashPassList[1],// "manager1",

            };
            var lines = unifreightTables
                .Select(tbl => string.Format("GRANT select ,insert ,update ,delete on  {0}  TO {1} ", tbl, main_ocsb.UserId))
                .ToList();

            var mySysConnection = new OracleConnection(sys.ConnectionString);

            mySysConnection.Open();
            try
            {
                foreach (var line in lines)
                {

                    try
                    {
                        Debug.WriteLine(line + " ;");
                        OracleCommand myCommand = mySysConnection.CreateCommand(line);//"INSERT INTO Test.Dept(DeptNo, DName) Values(50, 'DEVELOPMENT')");
                        myCommand.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {

                        Console.WriteLine(line);
                        if (line.Contains(".GAQ"))
                        {
                            
                        }
                        else
                        {
                            throw new Exception(line, e);
                        }
                        
                    }
                }

            }
            finally
            {
                mySysConnection.Close();
            }

            //"sqlplus user/pass@(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(Host=hostname.network)(Port=1521))(CONNECT_DATA=(SID=remote_SID)))"
            //"sqlplus system/manager1@(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(Host=hostname.network)(Port=1521))(CONNECT_DATA=(SID=remote_SID)))"
        }
        public List<string> GetTableNames(string pocoNamespace)
        {


            List<string> tableNameList = new List<string>();
            // use DBContext to get ObjectContext
            //DatabaseContext db = new DatabaseContext();
            IObjectContextAdapter adapter = this as IObjectContextAdapter;
            var objectContext = adapter.ObjectContext;

            ReadOnlyCollection<EntityType> allTypes = objectContext.MetadataWorkspace.GetItems<EntityType>(DataSpace.CSpace);
            if (false)
            {
                return allTypes.ToList().Select(t => t.Name).ToList();
            }
            Regex objectRegex = new Regex(@"^OBJECT:\(\[(?<database>[^\]]+)\]\.\[(?<schema>[^\]]+)\]\.\[(?<table>[^\]]+)\]\.\[(?<field>[^\]]+)\]\)$", RegexOptions.ExplicitCapture);


            foreach (EntityType item in allTypes)
            {

                string typeName =
                    pocoNamespace
                    + "." + item.Name;
                Type type = //Type.GetType(typeName);
                            //item.GetType().Assembly.GetType(typeName);
                    this.GetType().Assembly.GetType(typeName);
                if (type==null)
                {
                    throw new Exception($"Problem with DbSet<{typeName}> defintion .. Maybe namespace not  Unifreight.Data.AmitalModel.EntityPOCOs ");
                }
                string sql = this.Set(type).ToString();

                var m = objectRegex.Match(sql);
                if (m.Success)
                {
                    string table = m.Groups["schema"] + "." + m.Groups["table"];
                    if (!tableNameList.Contains(table))
                    {
                        tableNameList.Add(table);
                    }
                }
                else
                {
                    var schemaAndTable = sql.Substring(sql.IndexOf("FROM ") + 5).Split(' ')[0];
                    tableNameList.Add(schemaAndTable);
                }




                //Regex regex = new Regex(@"FROM \[dbo\]\.\[(?<table>.*)\] AS");
                //Match match = regex.Match(sql);
                //tableNameList.Add(match.Groups["table"].Value);
            }

            return tableNameList;
        }


        const bool FeatureRemoveCaseInsensitive = true;
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {


            if (LogitudeSettings.DatabaseManagementSystem != "oracle")
            {
                return;
            }
            //Devart.Data.Oracle.OracleMonitor monitor = new Devart.Data.Oracle.OracleMonitor() { IsActive = true };
            var config = Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance;
            config.Workarounds.DisableQuoting = true;
            config.Workarounds.IgnoreSchemaName = true;
            if (false)//in DbMigrations
            {
                config.CodeFirstOptions.TruncateLongDefaultNames = true;
                config.Workarounds.ColumnTypeCasingConventionCompatibility = true;
            }
            if (FeatureRemoveCaseInsensitive)
            {
                config.QueryOptions.CaseInsensitiveComparison = false;
                config.QueryOptions.CaseInsensitiveLike = false;

                //modelBuilder.Conventions.Remove<ColumnTypeCasingConvention>();
            }
            var connStr = this.Database.Connection.ConnectionString;
            var oraCSB = new OracleConnectionStringBuilder(connStr);
            var ConnSchemaUserId = oraCSB.UserId;

            if (DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
            {
                modelBuilder.SetDefaultSchema(LogitudeDBSchema, ConnSchemaUserId);
            }
            base.OnModelCreating(modelBuilder);
        }
        public interface IDbContextLogger : IDisposable
        {
            string ToString();
            void AddExplainLog(string ExplainLog);
            string ToString(int LastCharacter);
        }
        class DbContextLogger : IDbContextLogger
        {

            StringBuilder _StringBuilder;
            Action _DisposeMe;
            private DbContextLogger()
            {
            }
            internal DbContextLogger(Database database)
            {
                // TODO: Complete member initialization
                _StringBuilder = new StringBuilder();
                database.Log += LogMe; ;
                this._DisposeMe = ()
                    =>
                {
                    try
                    {
                        database.Log -= LogMe;
                    }
                    catch
                    {


                    }

                };
            }

            private void LogMe(string mess)
            {
                if (_StringBuilder == null)
                    _StringBuilder = new StringBuilder();
                if (mess == Environment.NewLine)
                {
                    return;
                }
                _StringBuilder.AppendLine(mess);
            }


            public override string ToString()
            {
                return _StringBuilder.ToString();
            }
            public string ToString(int lastCharacter)
            {
                var s = this.ToString();
                var sLast = s.Substring(Math.Max(0, s.Length - lastCharacter));
                return sLast;
            }
            public void Dispose()
            {
                _StringBuilder = null;
                _DisposeMe();
                _DisposeMe = null;
            }




            public void AddExplainLog(string ExplainLog)
            {
                LogMe(ExplainLog);
            }
        }


        public Nullable<returnType> ExecuteReaderSingleResult<returnType>(string sqlReturn1Row, Func<DbDataReader, Nullable<returnType>> GetReturnTypeFromReader)
        where returnType : struct
        {
            Debug.WriteLine(sqlReturn1Row);
            ////sqlReturn1Row = sqlReturn1Row.TrimEnd(" "[0]).TrimEnd(";"[0]);
            using (var command = this.Database.Connection.CreateCommand())
            {


                if (this.Database.Connection.State != System.Data.ConnectionState.Open)
                {
                    this.Database.Connection.Open();
                }
                command.CommandText = sqlReturn1Row;


                using (var dataReader = command.ExecuteReader(CommandBehavior.CloseConnection | CommandBehavior.SingleResult)
                    )
                {

                    if (dataReader.FieldCount < 1)
                    {
                        return null;
                    }

                    if (!dataReader.Read())
                    {
                        return null;
                    }
                    if (dataReader.IsDBNull(0))
                    {
                        return null;
                    }



                    var ReturnValue = GetReturnTypeFromReader(dataReader);

                    return ReturnValue;
                }
            }

        }

        public int ExecuteNonQuery(string sqlReturn1Row)
        {




            Debug.WriteLine(sqlReturn1Row);



            using (var connection =
              new OracleConnection(
                  /*"User Id=Scott;Password=tiger;Data Source=Ora;"*/
                  this.Database.Connection.ConnectionString)
            )
            {
                using (var command = new OracleCommand(sqlReturn1Row, connection))
                {
                    ///AddParams(command, MyParams);
                    command.CommandType = CommandType.Text;
                    int rowsAffected = command.ExecuteNonQuery();
                    // todo get affected ????????????
                    //For UPDATE, INSERT, and DELETE statements, the return value is the number of rows affected by the command. For all other types of statements, the return value is -1. If a rollback occurs, the return value is also -1.

                    return rowsAffected;


                }
            }

            //using (var command = this.Database.Connection.CreateCommand())
            //{


            //    if (this.Database.Connection.State != System.Data.ConnectionState.Open)
            //    {
            //        this.Database.Connection.Open();
            //    }
            //    command.CommandText = sqlReturn1Row;


            //    int affect = command.ExecuteNonQuery();
            //    return affect;

            //}

        }


    }

    public static class LogitudeExceptionExtU
    {
        public static void ChangeExceptionMess(this Exception ex, string messagePrefix)
        {
            var mess = ex.Message;
            ex.GetType().GetField("_message", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(ex, messagePrefix + " " + mess);
        }
    }
    public class ExceptionFormatDbEntityUtil
    {
        public static DbEntityValidationException GetFormated(DbEntityValidationException ex)
        {
            // Retrieve the error messages as a list of strings.
            var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

            // Join the list to a single string.
            var fullErrorMessage = string.Join("; ", errorMessages);

            // Combine the original exception message with the new one.
            var builder = new StringBuilder();
            builder.Append("DbEntityValidationException ").Append(ex.Message).Append(" The validation errors are: ").Append(fullErrorMessage);


            foreach (DbEntityValidationResult validationResult in ex.EntityValidationErrors)
            {
                string entityName = validationResult.Entry.Entity.GetType().Name;
                foreach (DbValidationError error in validationResult.ValidationErrors)
                {
                    builder.AppendLine(entityName + "." + error.PropertyName + ": " + error.ErrorMessage);
                }
                var e = validationResult.Entry;
                if (e != null && e.CurrentValues != null)
                {
                    foreach (var propertyName in e.CurrentValues.PropertyNames)
                    {
                        builder.AppendLine().AppendFormat("Property Name: {0}", propertyName);

                        if (e.State != EntityState.Added)
                        {
                            //get original value
                            var orgVal = e.OriginalValues[propertyName];
                            builder.AppendFormat("     Original Value: {0}", orgVal);
                        }
                        //get current values
                        var curVal = e.CurrentValues[propertyName];
                        builder.AppendFormat("     Current Value: {0}", curVal);
                    }
                }
            }
            // Throw a new DbEntityValidationException with the improved exception message.
            return new DbEntityValidationException(builder.ToString(), ex.EntityValidationErrors);
        }
    }

    public static class DbModelBuilderExt
    {


        public static void SetDefaultSchema(this System.Data.Entity.DbModelBuilder myDbModelBuilder,
            LogitudeDBSchema schema,
            string ConnSchemaUserId)
        {

          
            if (schema == LogitudeDBSchema.none)
            {
                throw new Exception("LogitudeDBSchema.none !!?????");
            }
            var config = Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance;
            //config.DatabaseScript.Schema.DeleteDatabaseBehaviour =  Devart.Data.Oracle.Entity.Configuration.DeleteDatabaseBehaviour.Schema;
            config.Workarounds.IgnoreSchemaName = false;
            var toSchema = ConnSchemaUserId;//schema.ToString();
            if (schema == LogitudeDBSchema.AMITAL_DB)
            {
                toSchema = DbContextBaseUtil.GetSchemaAMITAL_DB();
            }
            myDbModelBuilder.HasDefaultSchema(toSchema);//Not Work !!!!
            RewriteSchema(myDbModelBuilder, toSchema);//Work !!!!

        }



        const BindingFlags RewriteSchemaBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;

        static void RewriteSchema(DbModelBuilder modelBuilder, string schema)
        {
            var modelBuilderType = modelBuilder.GetType();
            var modelConfiguration = modelBuilderType.GetProperty("ModelConfiguration", RewriteSchemaBindingFlags).GetValue(modelBuilder);
            var activeEntityConfigurations = (IList)modelConfiguration.GetType().GetProperty("ActiveEntityConfigurations", RewriteSchemaBindingFlags).GetValue(modelConfiguration);
            foreach (var item in activeEntityConfigurations)
            {
                RewriteSchemaForEntityTypeConfiguration(item, schema);
            }
        }

        static void RewriteSchemaForEntityTypeConfiguration(object entityTypeConfiguration, string schema)
        {
            // not bulletproof, but better than nothing
            if (entityTypeConfiguration.GetType().Name != "EntityTypeConfiguration")
                throw new ArgumentException();

            var entityTypeConfigurationType = entityTypeConfiguration.GetType();
            var entityMappingConfigurations = ((IList)entityTypeConfigurationType.GetField("_entityMappingConfigurations", RewriteSchemaBindingFlags).GetValue(entityTypeConfiguration));
            foreach (var entityMappingConfiguration in entityMappingConfigurations)
            {
                var navigationPropertyConfigurations = (IDictionary)entityTypeConfigurationType.GetField("_navigationPropertyConfigurations", RewriteSchemaBindingFlags).GetValue(entityTypeConfiguration);
                foreach (var val in navigationPropertyConfigurations.Values)
                {
                    var associationMappingConfiguration = val.GetType().GetProperty("AssociationMappingConfiguration", RewriteSchemaBindingFlags).GetValue(val);
                    if (associationMappingConfiguration == null)
                        continue;
                    var tableNameAssociation = associationMappingConfiguration.GetType().GetField("_tableName", RewriteSchemaBindingFlags).GetValue(associationMappingConfiguration);
                    var schemaAssociation = (string)tableNameAssociation.GetType().GetProperty("Schema").GetValue(tableNameAssociation);
                    var nameAssociation = (string)tableNameAssociation.GetType().GetProperty("Name").GetValue(tableNameAssociation);
                    ToTableHelper(associationMappingConfiguration, nameAssociation, schemaAssociation ?? schema);
                }
                var tableNameEntity = entityMappingConfiguration.GetType().GetProperty("TableName").GetValue(entityMappingConfiguration);
                var schemaEntity = (string)tableNameEntity.GetType().GetProperty("Schema").GetValue(tableNameEntity);
                var nameEntity = (string)tableNameEntity.GetType().GetProperty("Name").GetValue(tableNameEntity);
                ToTableHelper(entityTypeConfiguration, nameEntity,
                    //schemaEntity ?? 
                    schema);
            }
        }

        static void ToTableHelper(object configuration, string name, string schema)
        {
            configuration.GetType().GetMethod("ToTable", new[] { typeof(string), typeof(string) }).Invoke(configuration, new[] { name, schema });
        }
    }
    public static class DbContextBaseUtil
    {
        //public const string Oracle_AmitalNetRoleConnStr = "Oracle_AmitalNetRoleConnStr";

        readonly static bool _UnifreightDataIncludedInMain_FeatureOn;
        public static bool UnifreightDataIncludedInMain_FeatureOn { get { return _UnifreightDataIncludedInMain_FeatureOn; } }
        static DbContextBaseUtil()
        {
            _UnifreightDataIncludedInMain_FeatureOn = true;
            return;
            if (Environment.MachineName.ToLower().Contains("itzik"))
            {
                AmitalDebuggerUtil.Break(AmitalDebuggerLevel.Critical);
                _UnifreightDataIncludedInMain_FeatureOn = true;
            }
            else
            {
                _UnifreightDataIncludedInMain_FeatureOn = false;
            }
        }
        public static string GetConnectionStringWithAmitalNetRole(string currentConnectionString)
        {

            //if (LogitudeSettings.DatabaseManagementSystem == "oracle" && DbContextBaseUtil.DefaultSchema_FeatureOn)
            //{

            //    try
            //    {
            //        currentConnectionString = ConfigurationManager.ConnectionStrings[DbContextBaseUtil.Oracle_AmitalNetRoleConnStr].ConnectionString;
            //    }
            //    catch (Exception)
            //    {

            //        throw new Exception("Please add to Config file Oracle_AmitalNetRoleConnStr");
            //    }

            //}
            return currentConnectionString;
        }
        public static bool? ToLog { get; set; }
        public static DateTime? MaxPoolSizeWasReachedWhileSave { get; set; }

        public static string GetStoredProcedureName(string StoredProcedure, LogitudeDBSchema myLogitudeDBSchema, string OracleConnectionStr)
        {
            if (DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
            {

                var oraCSB = new OracleConnectionStringBuilder(OracleConnectionStr);
                return oraCSB.UserId.ToString() + "." + StoredProcedure;
                //return myLogitudeDBSchema.ToString() + "." + StoredProcedure;
            }
            return StoredProcedure;

        }
        public static OracleConnectionStringBuilder GetOracleConStrBuilder(string dbConnectionInfo)
        {
            //dbConnectionInfo = "10.10.10.67,1521,amital,amitestm,amitestm";
            if (String.IsNullOrWhiteSpace(dbConnectionInfo))
            {
                throw new Exception("DevartAmitalDirect.ConnectionString is missing 'server,port,sid,userId,password'");
            }


            string[] information = dbConnectionInfo.Split(',');
            if (information.Count() != 5)
            {
                throw new Exception("DevartAmitalDirect.ConnectionString must be 'server,port,sid,userId,password'");
            }
            string server = information[0];
            string port = information[1];
            string sid = information[2];
            string userId = information[3];
            string password = information[4];
            int iPort = 1521;
            if (!int.TryParse(port, out iPort))
            {
                throw new Exception("DevartAmitalDirect.ConnectionString must be 'server,port,sid,userId,password'");
            }
            OracleConnectionStringBuilder oraCSB = new OracleConnectionStringBuilder();
            oraCSB.Direct = true;
            oraCSB.Server = server; //"10.10.10.67";
            oraCSB.Port = iPort;//1521;
            oraCSB.Sid = sid;//"amital";
            oraCSB.UserId = userId;//"amitestm";
            oraCSB.Password = password;//"amitestm";

            return oraCSB;
        }

        public static string GetSchemaAMITAL_DB(int tenantSeed = 1)
        {
            Devart.Data.Oracle.OracleConnectionStringBuilder csb = null;
            if (true)
            {
                //const int DEFAULT_CUSTOMS_axiom_Tenant = 1;
                string dbConnectionInfo = "";
                if (LogitudeSettings.GetLogitudeCustomsSettingsMInject != null)
                {
                    dbConnectionInfo = LogitudeSettings.GetLogitudeCustomsSettingsMInject(tenantSeed).UnfConnectionString;

                }
                else
                {
                    //using from filiing/OpenAccess service 
                    dbConnectionInfo = LogitudeSettings.GetUnfDBConnectionInfoFromTenantInject(tenantSeed);
                }
                csb = GetOracleConStrBuilder(dbConnectionInfo);
            }
            else
            {
                var connString = GetConnectionStringWithAmitalNetRole("");
                csb = new Devart.Data.Oracle.OracleConnectionStringBuilder(connString);
            }


            return csb.UserId.ToUpper();

        }
    }
    public class FixedSizedQueue<T> : ConcurrentQueue<T>
    {
        private readonly object syncObject = new object();

        public int Size { get; private set; }

        public FixedSizedQueue(int size)
        {
            Size = size;
        }

        public new void Enqueue(T obj)
        {
            base.Enqueue(obj);
            lock (syncObject)
            {
                while (base.Count > Size)
                {
                    T outObj;
                    base.TryDequeue(out outObj);
                }
            }
        }
    }


    public static class DbContextBaseSqlServerExt
    //4 Accounting streaming 
    {


        /// <summary> 
        /// Execute stored procedure with single table value parameter. 
        /// </summary> 
        /// <typeparam name="T">Type of object to store.</typeparam> 
        /// <param name="context">DbContext instance.</param> 
        /// <param name="data">Data to store</param> 
        /// <param name="procedureName">Procedure name</param> 
        /// <param name="paramName">Parameter name</param> 
        /// <param name="typeName">User table type name</param> 
        public static void ExecuteTableValueProcedure<T>(this DbContext context, IEnumerable<T> data, string procedureName, string paramName, string typeName)
        {
            //// convert source data to DataTable 
            DataTable table = data.ToDataTable();

            //// create parameter 
            SqlParameter parameter = new SqlParameter(paramName, table);
            parameter.SqlDbType = SqlDbType.Structured;
            parameter.TypeName = typeName;

            //// execute sp sql 
            string sql = String.Format("EXEC {0} {1};", procedureName, paramName);

            //// execute sql 
            context.Database.ExecuteSqlCommand(sql, parameter);
        }

        /// <summary> 
        /// Creates data table from source data. 
        /// </summary> 
        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            DataTable table = new DataTable();

            //// get properties of T 
            var binding = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty;
            var options = PropertyReflectionOptions.IgnoreEnumerable | PropertyReflectionOptions.IgnoreIndexer;

            var properties = GetProperties<T>(binding, options).ToList();

            //// create table schema based on properties 
            foreach (var property in properties)
            {
                table.Columns.Add(property.Name, property.PropertyType);
            }

            //// create table data from T instances 
            object[] values = new object[properties.Count];

            foreach (T item in source)
            {
                for (int i = 0; i < properties.Count; i++)
                {
                    values[i] = properties[i].GetValue(item, null);
                }

                table.Rows.Add(values);
            }

            return table;
        }

        /// <summary> 
        /// Gets properties of T 
        /// </summary> 
        public static IEnumerable<PropertyInfo> GetProperties<T>(BindingFlags binding, PropertyReflectionOptions options = PropertyReflectionOptions.All)
        {
            var properties = typeof(T).GetProperties(binding);

            bool all = (options & PropertyReflectionOptions.All) != 0;
            bool ignoreIndexer = (options & PropertyReflectionOptions.IgnoreIndexer) != 0;
            bool ignoreEnumerable = (options & PropertyReflectionOptions.IgnoreEnumerable) != 0;

            foreach (var property in properties)
            {
                if (!all)
                {
                    if (ignoreIndexer && IsIndexer(property))
                    {
                        continue;
                    }

                    if (ignoreIndexer && !property.PropertyType.Equals(typeof(string)) && IsEnumerable(property))
                    {
                        continue;
                    }
                }

                yield return property;
            }
        }

        /// <summary> 
        /// Check if property is indexer 
        /// </summary> 
        private static bool IsIndexer(PropertyInfo property)
        {
            var parameters = property.GetIndexParameters();

            if (parameters != null && parameters.Length > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary> 
        /// Check if property implements IEnumerable 
        /// </summary> 
        private static bool IsEnumerable(PropertyInfo property)
        {
            return property.PropertyType.GetInterfaces().Any(x => x.Equals(typeof(System.Collections.IEnumerable)));
        }
    }

    [Flags]
    public enum PropertyReflectionOptions : int
    {
        /// <summary> 
        /// Take all. 
        /// </summary> 
        All = 0,

        /// <summary> 
        /// Ignores indexer properties. 
        /// </summary> 
        IgnoreIndexer = 1,

        /// <summary> 
        /// Ignores all other IEnumerable properties 
        /// except strings. 
        /// </summary> 
        IgnoreEnumerable = 2
    }
}


