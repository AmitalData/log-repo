using AmitalCloud.Infrastructure.Data.DBHelpers;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Domain.Enums;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using Devart.Data.Oracle;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;
namespace AmitalCloud.Infrastructure.Data.BaseClasses
{
    public abstract class DbContextBase : DbContext
    {
        FixedSizedQueue<string> _MyLogQueue = new FixedSizedQueue<string>(30);
        abstract public AmitalCloudDBSchema AmitalCloudDBSchema { get; }
        public DbContextBase(DbConnection connection, DbCompiledModel model)
: base(connection, model, contextOwnsConnection: false)
        {
            Database.Connection.StateChange += Connection_StateChange;
        }
        public DbContextBase()
            : base()
        {
            if (AmitalCloudSettings.DatabaseManagementSystem.Equals("oracle", StringComparison.OrdinalIgnoreCase))
            {
                (this as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 
            }
            Database.Connection.StateChange += Connection_StateChange;
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
                if (AmitalCloudSettings.DatabaseManagementSystem == "oracle")
                {
                    this.Database.CommandTimeout = to;
                }
                else if (ApplicationAppInfo.WorkerRoleCall)
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
                AmitalCloudDebuggerUtil.Break(AmitalDebuggerLevel.Information);
                if (suppressThrow)
                {
                    return -999;
                }
                throw e1;
            }
            catch (DbUpdateException dbu)
            {
                FormatDbUpdateException(dbu);
                if (this.AmitalCloudDBSchema == AmitalCloudDBSchema.LOGITUDE_MAIN)
                {
                    if (AmitalCloudSettings.HandleDbExceptionInject != null)
                    {
                        AmitalCloudSettings.HandleDbExceptionInject(dbu, "SaveChanges:DbUpdateException", GetString4LogGroupBy(saveChangeLogger, Environment.StackTrace.ToString()));
                    }
                }
                if (suppressThrow)
                {
                    return -999;
                }
                throw dbu;
            }
            catch (Exception e)
            {
                if (this.AmitalCloudDBSchema != AmitalCloudDBSchema.LOGITUDE_LOGS && AmitalCloudSettings.HandleDbExceptionInject != null)
                {
                    AmitalCloudSettings.HandleDbExceptionInject(e, "SaveChanges:Exception", GetString4LogGroupBy(saveChangeLogger, Environment.StackTrace.ToString()));
                }
                else
                {
                    AmitalCloudDebuggerUtil.Break(AmitalDebuggerLevel.Critical);
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
                                var orgVal = result.OriginalValues[propertyName];
                                builder.AppendFormat("     Original Value: {0}", orgVal);
                            }
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
        }
        public IDbContextLogger CreateLogger() => new DbContextLogger(this.Database) as IDbContextLogger;
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
            if (AmitalCloudSettings.DatabaseManagementSystem != "oracle")
            {
                return;
            }
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                return;
            }
            if (DBHelpers.DbContextBaseUtil.ToLog == null)
            {
                DBHelpers.DbContextBaseUtil.ToLog = false;
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(@"DbContextBase:ToLog:(Default:False due Memory Leak if not Disposed)Any time any place u can set: 
                        DBHelpers.DbContextBaseUtil.ToLog =true;");
                AmitalCloudDebuggerUtil.Break();
            }
            DBHelpers.DbContextBaseUtil.ToLog = DBHelpers.DbContextBaseUtil.ToLog;
            if (DBHelpers.DbContextBaseUtil.ToLog.GetValueOrDefault())
            {
                TransactionFactory.RegisterTransactionCompleted();
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
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(mess);
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
            if (AmitalCloudSettings.System2RedirectFraction == 0)
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
            return ((LuckyNumber % AmitalCloudSettings.System2RedirectFraction) == 0);
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
            var UserSlashPass = _UserSlashPass ?? GetConnetionStringFunc();
            var UserSlashPassList = new List<string>(UserSlashPass.Split(new char[] { '/' }));
            var main_ocsb = new OracleConnectionStringBuilder(mainConnectionString);
            var sys = new OracleConnectionStringBuilder()
            {
                Direct = main_ocsb.Direct,
                Server = main_ocsb.Server,
                Port = main_ocsb.Port,
                Sid = main_ocsb.Sid,
                UserId = UserSlashPassList[0],
                Password = UserSlashPassList[1],
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
                        NetCommonHelper.Logger.DevLog.Instance.WriteDebug(line + " ;");
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
        }
        public List<string> GetTableNames(string pocoNamespace)
        {
            List<string> tableNameList = new List<string>();
            IObjectContextAdapter adapter = this as IObjectContextAdapter;
            var objectContext = adapter.ObjectContext;
            ReadOnlyCollection<EntityType> allTypes = objectContext.MetadataWorkspace.GetItems<EntityType>(DataSpace.CSpace);
            Regex objectRegex = new Regex(@"^OBJECT:\(\[(?<database>[^\]]+)\]\.\[(?<schema>[^\]]+)\]\.\[(?<table>[^\]]+)\]\.\[(?<field>[^\]]+)\]\)$", RegexOptions.ExplicitCapture);
            foreach (EntityType item in allTypes)
            {
                string typeName =
                    pocoNamespace
                    + "." + item.Name;
                Type type =  this.GetType().Assembly.GetType(typeName);
                if (type == null)
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
            }

            return tableNameList;
        }
        const bool FeatureRemoveCaseInsensitive = true;
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (AmitalCloudSettings.DatabaseManagementSystem != "oracle")
            {
                return;
            }
            var connStr = this.Database.Connection.ConnectionString;
            var oraCSB = new OracleConnectionStringBuilder(connStr);
            var ConnSchemaUserId = oraCSB.UserId;
            if (DBHelpers.DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
            {
                modelBuilder.SetDefaultSchema(AmitalCloudDBSchema, ConnSchemaUserId);
            }
            base.OnModelCreating(modelBuilder);
        }
        public Nullable<returnType> ExecuteReaderSingleResult<returnType>(string sqlReturn1Row, Func<DbDataReader, Nullable<returnType>> GetReturnTypeFromReader)
        where returnType : struct
        {
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug(sqlReturn1Row);
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




            NetCommonHelper.Logger.DevLog.Instance.WriteDebug(sqlReturn1Row);



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
}
