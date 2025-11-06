
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.Interfaces;
using Logitude.BL.Resolvers;
using Logitude.BL.Security;
using Logitude.Customs.BL.StimulReport;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
    public class AutomaticReconcileService
    ///PILOT 
    ///http://stackoverflow.com/questions/23280535/maybe-a-really-simple-dynamic-linq-to-entities-select-statement
    {
        public static string fifoAccountingDate = "FIFOAccountingDate";
         public static string fifoDueDate = "FIFODueDate";
         public static string accountingDate = "AccountingDate";
        public static string dueDate = "DueDate";

        public void AutomaticReconcile(string gLAccountId, int tenant, FilteredReconciliation myFilteredReconciliation)
        {
            var accountingContext = AccountingContext.GetContext(tenant);
            var repo = new LedgerTransactionRepository(tenant);
            LedgerTransactionListQueryService listService = new LedgerTransactionListQueryService(accountingContext);



            var repoGLAccountRepository = new GLAccountRepository(tenant);
            var acc = repoGLAccountRepository.GetGLAccountByIdTenant(gLAccountId, tenant);

            //acc.ReconcileMethod.
            //var repoAutomaticReconcile = new AutomaticReconcileRepository(tenant);

            AutomaticReconcileMethod pocoAutomaticReconcileMethod = new AutomaticReconcileMethod();

            if (String.IsNullOrWhiteSpace(acc.AutomaticReconcileId))
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("No valid Method");
                //return;
            }
            else
            {
                var repoAutomaticReconcileM = new AutomaticReconcileMethodRepository(tenant);
                pocoAutomaticReconcileMethod = repoAutomaticReconcileM.GetSingle(acc.AutomaticReconcileId, tenant);


                if (pocoAutomaticReconcileMethod == null)
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug("No valid Method");
                }
            }


            AutomaticReconcilePM.AutomaticReconcileEnum method1 = AutomaticReconcilePM.AutomaticReconcileEnum.AccountingDate;
            AutomaticReconcilePM.AutomaticReconcileEnum method2 = AutomaticReconcilePM.AutomaticReconcileEnum.none;
            AutomaticReconcilePM.AutomaticReconcileEnum method3 = AutomaticReconcilePM.AutomaticReconcileEnum.none;
            Enum.TryParse<AutomaticReconcilePM.AutomaticReconcileEnum>(pocoAutomaticReconcileMethod.AutomaticReconcile1, out method1);
            Enum.TryParse<AutomaticReconcilePM.AutomaticReconcileEnum>(pocoAutomaticReconcileMethod.AutomaticReconcile2, out method2);
            Enum.TryParse<AutomaticReconcilePM.AutomaticReconcileEnum>(pocoAutomaticReconcileMethod.AutomaticReconcile3, out method3);

            var fields = new List<string>();


            var qNotReconciled = repo.GetNotReconciled(gLAccountId, tenant);

            var filter = new GenericFilter();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = myFilteredReconciliation.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            qNotReconciled = filter.GetFilteredQuery<LedgerTransaction>(nonListQueryOperation, qNotReconciled);


            if (myFilteredReconciliation.ClientChooseAutoMethod)
            {
                qNotReconciled = AddMethod(qNotReconciled, myFilteredReconciliation.method1, fields);
                qNotReconciled = AddMethod(qNotReconciled, myFilteredReconciliation.method2, fields);
                qNotReconciled = AddMethod(qNotReconciled, myFilteredReconciliation.method3, fields);

            }
            else
            {
                qNotReconciled = AddMethod(qNotReconciled, method1, fields);
                qNotReconciled = AddMethod(qNotReconciled, method2, fields);
                qNotReconciled = AddMethod(qNotReconciled, method3, fields);
            }
            fields = fields.Distinct().ToList();
            if (fields.Count == 0)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("No valid Method");



                qNotReconciled = AddMethod(qNotReconciled, myFilteredReconciliation.method1, fields);
                qNotReconciled = AddMethod(qNotReconciled, myFilteredReconciliation.method2, fields);
                qNotReconciled = AddMethod(qNotReconciled, myFilteredReconciliation.method3, fields);
                fields = fields.Distinct().ToList();
                if (fields.Count == 0)
                {

                    //bool useLocal = !(GetLoggedContact(tenant).DontShowLocal);
                    bool useLocal = true;
                    var user = GetLoggedContact(tenant);
                    if (user != null) useLocal = !(GetLoggedContact(tenant).DontShowLocal);

                    string msg = TranslateTextsClass.Translate("Accounting.General.O.NoAutoRecoMethodDefined4GLAccount", 0, useLocal);

                    throw new ApplicationException(msg);
                    //No GlAccount AutomaticReconcile and no Screen AutomaticReconcile defintion // WI26460

                }

            }
            var qNotReconciledGroupByHaveValues = qNotReconciled;



            //var qg= (from  r in q 
            //         group r by q.SelectDynamic( new List<string>() {"OpenAmount","DocumentDate"}
            var qNotReconciledGroupByHaveValuesMapDTO = qNotReconciledGroupByHaveValues.Select(rec => new LedgerTransactionDto()
            {
                Id = rec.Id,
                OpenAmount = rec.OpenAmount,

                //<<<<<GROUP
                OpenAmountCurrencyId = rec.OpenAmountCurrencyId,
                OpenAmountABS = Math.Abs(rec.OpenAmount),
                DocumentDate = DbFunctions.TruncateTime(rec.DocumentDate).Value,
                DueDate = DbFunctions.TruncateTime(rec.DueDate).Value,
                AccountingDate = DbFunctions.TruncateTime(rec.AccountingDate).Value,
                Reference1 = rec.Reference1,
                Reference2 = rec.Reference2,
                Reference3 = rec.Reference3,
                //>>>>GROUP

            });

            var takeFirst250000 = true;
            if (takeFirst250000)
            {
                qNotReconciledGroupByHaveValuesMapDTO = qNotReconciledGroupByHaveValuesMapDTO
                    .OrderBy(rec => rec.AccountingDate)
                    .Take(250000);
            }
            //expresion tree
            var fieldList = fields.ToArray();
            
            if (fields.FirstOrDefault() == fifoAccountingDate || fields.FirstOrDefault() == fifoDueDate)
            {
                string propertyName = fields[0] == fifoAccountingDate ? accountingDate : dueDate;
                var fifoResult = DoFifoReconcile(qNotReconciledGroupByHaveValuesMapDTO, propertyName);

                AllLedgerTransactionList = listService.GetDtoAsList(fifoResult);
                return;
            }
            var lambada = GroupByExpression<LedgerTransactionDto>(fieldList);
            var qNotReconciledGroupByHaveValuesMapDTOHavingZeroSum = qNotReconciledGroupByHaveValuesMapDTO.GroupBy(lambada.Compile()).Where(g => g.Sum(r => r.OpenAmount) == 0);
            var only10000Match = true;
            if (only10000Match)// 10000 Match with most rows 
            {
                qNotReconciledGroupByHaveValuesMapDTOHavingZeroSum =
                    qNotReconciledGroupByHaveValuesMapDTOHavingZeroSum
                    .OrderByDescending(g => g.Count())
                    .Take(10000); ;
            }

            var resGroupBy = qNotReconciledGroupByHaveValuesMapDTOHavingZeroSum.ToList();
            var allDto = new List<LedgerTransactionDto>();

            int GroupMatch = 1;
            foreach (var group in resGroupBy)
            {

                //group.Key
                foreach (var item in group)
                {
                    item.GroupHash = group.Key.GetHashCode();
                    item.GroupHash = GroupMatch;
                    //item.GroupMatch = GroupMatch;
                    allDto.Add(item);
                }
                GroupMatch++;

            }

            var listLedger = listService.GetDtoAsList(allDto);

            AllLedgerTransactionList = listLedger;


        }

        private static IQueryable<LedgerTransaction> AddMethod(IQueryable<LedgerTransaction> qNotReconciled, AutomaticReconcilePM.AutomaticReconcileEnum method, List<string> fields)
        {
            switch (method)
            {
                case AutomaticReconcilePM.AutomaticReconcileEnum.none:
                    break;
                case AutomaticReconcilePM.AutomaticReconcileEnum.OpenAmountABS:
                    fields.Add("OpenAmountABS");
                    fields.Add("OpenAmountCurrencyId");

                    break;
                case AutomaticReconcilePM.AutomaticReconcileEnum.ReferenceDate:
                    //qNotReconciled = qNotReconciled.Where( rec=> rec.DocumentDate
                    fields.Add("DocumentDate");
                    break;
                case AutomaticReconcilePM.AutomaticReconcileEnum.DueDate:
                    //qNotReconciled = qNotReconciled.Where( rec=> rec.DueDate

                    fields.Add("DueDate");
                    break;
                case AutomaticReconcilePM.AutomaticReconcileEnum.AccountingDate:

                    fields.Add("AccountingDate");
                    break;
                case AutomaticReconcilePM.AutomaticReconcileEnum.Reference1:
                    qNotReconciled = qNotReconciled
                        //.Where(rec => !string.IsNullOrWhiteSpace(rec.Reference1));
                        .Where(rec => !(rec.Reference1 == null || rec.Reference1.Trim() == string.Empty));
                    fields.Add("Reference1");//t.Reference1
                    break;
                case AutomaticReconcilePM.AutomaticReconcileEnum.Reference2:
                    qNotReconciled = qNotReconciled
                        .Where(rec => !(rec.Reference2 == null || rec.Reference2.Trim() == string.Empty));
                    fields.Add("Reference2");
                    break;
                case AutomaticReconcilePM.AutomaticReconcileEnum.Reference3:
                    qNotReconciled = qNotReconciled
                        //.Where(rec => !string.IsNullOrWhiteSpace(rec.Reference3));
                        .Where(rec => !(rec.Reference3 == null || rec.Reference3.Trim() == string.Empty));

                    fields.Add("Reference3");
                    break;
                case AutomaticReconcilePM.AutomaticReconcileEnum.FIFOAccountingDate:
                    fields.Add(fifoAccountingDate);
                    break;
                case AutomaticReconcilePM.AutomaticReconcileEnum.FIFODueDate:
                    fields.Add(fifoDueDate);
                    break;
                default:
                    break;
            }
            return qNotReconciled;
        }

        private static void testNewMethod()
        {
            string[] fields = { "Name", "Test_Result" };
            Type ledgerTransactionDTOType = typeof(LedgerTransactionDTO);

            var itemParam = Expression.Parameter(ledgerTransactionDTOType, "x");

            var addMethod = typeof(Dictionary<string, object>).GetMethod(
                "Add", new[] { typeof(string), typeof(object) });
            var selector = Expression.ListInit(
                    Expression.New(typeof(Dictionary<string, object>)),
                    fields.Select(field => Expression.ElementInit(addMethod,
                        Expression.Constant(field),
                        Expression.Convert(
                            Expression.PropertyOrField(itemParam, field),
                            typeof(object)
                        )
                    )));
            var lambda = Expression.Lambda<Func<LedgerTransactionDTO, Dictionary<string, object>>>(
                selector, itemParam);
        }

        private List<LedgerTransactionDto> DoFifoReconcile(IEnumerable<LedgerTransactionDto> transactions, string dateField)
        {
            if (transactions == null || !transactions.Any())
                return new List<LedgerTransactionDto>();

            if (string.IsNullOrWhiteSpace(dateField))
                throw new ArgumentException("Date field name is required.", nameof(dateField));

            var propInfo = typeof(LedgerTransactionDto).GetProperty(dateField);
            if (propInfo == null)
                throw new ArgumentException($"Property '{dateField}' not found on LedgerTransactionDto.");

            var ordered = transactions
                .Where(t => propInfo.GetValue(t) != null)
                .OrderBy(t => (DateTime)propInfo.GetValue(t))
                .ThenBy(t => t.Id)
                .ToList();

            if (!ordered.Any())
                return new List<LedgerTransactionDto>();

            var plusList = ordered.Where(t => t.OpenAmount > 0).ToList();
            var minusList = ordered.Where(t => t.OpenAmount < 0).ToList();

            if (!plusList.Any() || !minusList.Any())
                return new List<LedgerTransactionDto>();

            decimal plusSum = plusList.Sum(x => x.OpenAmount);
            decimal minusSum = minusList.Sum(x => x.OpenAmount); 

            var reconciled = new List<LedgerTransactionDto>();
            decimal balance;

            if (plusSum >= Math.Abs(minusSum))
            {
                balance = -minusSum; 
                foreach (var payment in minusList)
                {
                    payment.GroupHash = 1;
                    reconciled.Add(payment);
                }

                foreach (var invoice in plusList)
                {
                    if (balance <= 0)
                        break;

                    if (invoice.OpenAmount <= balance)
                    {
                        invoice.AmountToReconcile = invoice.OpenAmount;
                        balance -= invoice.OpenAmount;
                    }
                    else
                    {
                        invoice.AmountToReconcile = balance;
                        balance = 0;
                    }

                    invoice.GroupHash = 1;
                    reconciled.Add(invoice);
                }
            }
            else
            {
                balance = plusSum;
                foreach (var invoice in plusList)
                {
                    invoice.GroupHash = 1;
                    reconciled.Add(invoice);
                }

                foreach (var payment in minusList)
                {
                    if (balance <= 0)
                        break;

                    decimal paymentAbs = Math.Abs(payment.OpenAmount);
                    if (paymentAbs <= balance)
                    {
                        balance -= paymentAbs;
                    }
                    else
                    {
                        payment.AmountToReconcile =-balance;
                        balance = 0;
                    }

                    payment.GroupHash = 1;
                    reconciled.Add(payment);
                }
            }

            return reconciled;
        }





        public Expression<Func<TItem, object>> GroupByExpression<TItem>(string[] propertyNames)
        {
            var properties = propertyNames.Select(name => typeof(TItem).GetProperty(name)).ToArray();
            var propertyTypes = properties.Select(p => p.PropertyType).ToArray();
            var tupleTypeDefinition = typeof(Tuple).Assembly.GetType("System.Tuple`" + properties.Length);
            var tupleType = tupleTypeDefinition.MakeGenericType(propertyTypes);
            var constructor = tupleType.GetConstructor(propertyTypes);
            var param = Expression.Parameter(typeof(TItem), "item");
            var body = Expression.New(constructor, properties.Select(p => Expression.Property(param, p)));
            var expr = Expression.Lambda<Func<TItem, object>>(body, param);
            return expr;
        }


        private static Expression<Func<LedgerTransactionDTO, string>> testGetColumnName(string property)
        {
            var parameter = Expression.Parameter(typeof(LedgerTransactionDTO), "LedgerTransactionDTO");
            var LedgerTransactionDTOProperty = Expression.PropertyOrField(parameter, property);
            var lambda = Expression.Lambda<Func<LedgerTransactionDTO, string>>(LedgerTransactionDTOProperty, parameter);

            return lambda;
        }
        private static Expression<Func<LedgerTransactionDTO, string>> testGetGroupKey(string property)
        {
            var parameter = Expression.Parameter(typeof(LedgerTransactionDTO));
            var body = Expression.Property(parameter, property);
            return Expression.Lambda<Func<LedgerTransactionDTO, string>>(body, parameter);
        }

        /*
        public static IEnumerable<IGrouping<object, TElement>> GroupByMany<TElement>(
         this IEnumerable<TElement> elements, params string[] groupSelectors)
        {
            var selectors = new List<Func<TElement, object>>(groupSelectors.Length);
            selectors.AddRange(groupSelectors.Select(selector => DynamicExpression.ParseLambda(typeof(TElement), typeof(object), selector)).Select(l => (Func<TElement, object>)l.Compile()));
            return elements.GroupByMany(selectors.ToArray());
        }

        public static IEnumerable<IGrouping<object, TElement>> GroupByMany<TElement>(
                this IEnumerable<TElement> elements, params Func<TElement, object>[] groupSelectors)
        {
            if (groupSelectors.Length > 0)
            {
                Func<TElement, object> selector = groupSelectors.First();
                return elements.GroupBy(selector);
            }
            return null;
        }
       
         */
        public void GetAll(string gLAccountId, int tenant)
        {


            //var q = (from record in context.LedgerTransactions
            //         where record.Tenant == tenant && record.AccountId == gLAccountId && record.IsReconciled == false
            //         select record);
            var repo = new LedgerTransactionRepository(tenant);

            var q = repo.GetNotReconciled(gLAccountId, tenant);

            var ledgerTransactionsEqualityComparer = new LedgerTransactionsEqualityComparer();

            var method1 = "1";
            var method2 = "4";
            ledgerTransactionsEqualityComparer._FuncKeyStrin = ledgerTransactionList =>
            {
                var sb = new StringBuilder();

                GetIt(method1, ledgerTransactionList, sb);
                GetIt(method2, ledgerTransactionList, sb);

                return sb.ToString();
            };
            //var qg= (from  r in q 
            //         group r by q.SelectDynamic( new List<string>() {"OpenAmount","DocumentDate"}
            var l = q.Select(rec => new LedgerTransactionDTO()
            {
                Id = rec.Id,
                OpenAmount = rec.OpenAmount,
                DocumentDate = //EntityFunctions.TruncateTime(
                rec.DocumentDate,
                AccountingDate = rec.AccountingDate,
                Reference1 = rec.Reference1

            }).ToList();
            var res = l.GroupBy(r => r, ledgerTransactionsEqualityComparer).Where(g => g.Sum(r => r.OpenAmount) == 0).ToList();
        }
        private static void GetIt(string method, LedgerTransactionDTO ledgerTransactionList, StringBuilder sb)
        {
            switch (method)
            {
                case "1":
                    //if (ledgerTransactionList.OpenAmount >= 0)
                    //    xDecimal = ledgerTransactionList.OpenAmount;
                    //else
                    //    xDecimal = -ledgerTransactionList.OpenAmount;
                    //break;
                    sb.Append(Math.Abs(ledgerTransactionList.OpenAmount).ToString());
                    break;
                case "2":
                    sb.Append(ledgerTransactionList.DocumentDate.Date.ToString());
                    break;
                case "3":
                    sb.Append(ledgerTransactionList.DueDate.ToString());
                    break;
                case "4":
                    sb.Append(ledgerTransactionList.AccountingDate.ToString());
                    break;
                case "5":
                    sb.Append(ledgerTransactionList.Reference1);
                    break;
                case "6":
                    sb.Append(ledgerTransactionList.Reference2);
                    break;
                case "7":
                    sb.Append(ledgerTransactionList.Reference3);
                    break;
                default:
                    sb.Append("");
                    break;
            }
        }



        public List<Data.EntityLists.LedgerTransactionList> AllLedgerTransactionList { get; set; }


        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }



        private static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }

            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }



    }
    public class LedgerTransactionsEqualityComparer : IEqualityComparer<LedgerTransactionDTO>
    {

        public Func<LedgerTransactionDTO, string> _FuncKeyStrin { get; set; }
        public bool Equals(LedgerTransactionDTO x, LedgerTransactionDTO y)
        {
            return _FuncKeyStrin(x) == _FuncKeyStrin(y);
        }

        public int GetHashCode(LedgerTransactionDTO obj)
        {
            var ss = _FuncKeyStrin(obj);
            return _FuncKeyStrin(obj).GetHashCode();
        }




    }


    public static class IQueryableExt
    {

        public static IQueryable SelectDynamic(this IQueryable source, IEnumerable<string> fieldNames)
        {
            Dictionary<string, PropertyInfo> sourceProperties = fieldNames.ToDictionary(name => name, name => source.ElementType.GetProperty(name));
            Type dynamicType = LinqRuntimeTypeBuilder.GetDynamicType(sourceProperties.Values);

            ParameterExpression sourceItem = Expression.Parameter(source.ElementType, "t");
            IEnumerable<MemberBinding> bindings = dynamicType.GetFields().Select(p => Expression.Bind(p, Expression.Property(sourceItem, sourceProperties[p.Name]))).OfType<MemberBinding>();

            Expression selector = Expression.Lambda(Expression.MemberInit(
                Expression.New(dynamicType.GetConstructor(Type.EmptyTypes)), bindings), sourceItem);

            return source.Provider.CreateQuery(Expression.Call(typeof(Queryable), "Select", new Type[] { source.ElementType, dynamicType },
                         Expression.Constant(source), selector));
        }



        public static class LinqRuntimeTypeBuilder
        {
            //private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            private static AssemblyName assemblyName = new AssemblyName() { Name = "DynamicLinqTypes" };
            private static ModuleBuilder moduleBuilder = null;
            private static Dictionary<string, Type> builtTypes = new Dictionary<string, Type>();

            static LinqRuntimeTypeBuilder()
            {
                moduleBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run).DefineDynamicModule(assemblyName.Name);
            }

            private static string GetTypeKey(Dictionary<string, Type> fields)
            {
                //TODO: optimize the type caching -- if fields are simply reordered, that doesn't mean that they're actually different types, so this needs to be smarter
                string key = string.Empty;
                foreach (var field in fields)
                    key += field.Key + ";" + field.Value.Name + ";";

                return key;
            }

            public static Type GetDynamicType(Dictionary<string, Type> fields)
            {
                if (null == fields)
                    throw new ArgumentNullException("fields");
                if (0 == fields.Count)
                    throw new ArgumentOutOfRangeException("fields", "fields must have at least 1 field definition");

                try
                {
                    Monitor.Enter(builtTypes);
                    string className = GetTypeKey(fields);

                    if (builtTypes.ContainsKey(className))
                        return builtTypes[className];

                    TypeBuilder typeBuilder = moduleBuilder.DefineType(className, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable);

                    foreach (var field in fields)
                        typeBuilder.DefineField(field.Key, field.Value, FieldAttributes.Public);

                    builtTypes[className] = typeBuilder.CreateType();

                    return builtTypes[className];
                }
                catch (Exception ex)
                {
                    //log.Error(ex);
                }
                finally
                {
                    Monitor.Exit(builtTypes);
                }

                return null;
            }


            private static string GetTypeKey(IEnumerable<PropertyInfo> fields)
            {
                return GetTypeKey(fields.ToDictionary(f => f.Name, f => f.PropertyType));
            }

            public static Type GetDynamicType(IEnumerable<PropertyInfo> fields)
            {
                return GetDynamicType(fields.ToDictionary(f => f.Name, f => f.PropertyType));
            }
        }
    }
    public class LedgerTransactionDTO
    {



        public string Id { get; set; }
        public string ControlAccountId { get; set; }

        public DateTime AccountingDate { get; set; }

        public DateTime DocumentDate { get; set; }

        public DateTime DueDate { get; set; }

        public decimal LocalAmountDebit { get; set; }

        public decimal LocalAmountCredit { get; set; }
        public string CurrencyId { get; set; }

        public decimal ForeignAmountDebit { get; set; }

        public decimal ForeignAmountCredit { get; set; }

        public decimal ExchangeRate { get; set; }

        public string Reference1 { get; set; }

        public string Reference2 { get; set; }

        public string Reference3 { get; set; }

        public decimal OpenAmountABS { get; set; }
        public decimal OpenAmount { get; set; }
        public string OppositeAccountId { get; set; }

        public string SearchFields { get; set; }
        public string OpenAmountCurrencyId { get; set; }

        public string Notes { get; set; }
    }

    public class FilteredReconciliation : QueryOperations
    {
        public bool IsFilteredReconciliation { get; set; }//Unique not 4 use 
        //public List<QueryFilterItem> QueryFilterItems { get; set; }

        public AutomaticReconcilePM.AutomaticReconcileEnum method1 { get; set; }
        public AutomaticReconcilePM.AutomaticReconcileEnum method2 { get; set; }
        public AutomaticReconcilePM.AutomaticReconcileEnum method3 { get; set; }
        public GenericCallBack CallBack { get; set; }
        public bool ClientChooseAutoMethod { get; set; }
    }

}

