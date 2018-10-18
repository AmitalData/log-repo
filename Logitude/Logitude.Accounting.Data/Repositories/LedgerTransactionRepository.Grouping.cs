#if false


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Reflection.Emit;
using System.Data.Entity;


namespace Logitude.Accounting.Data.Repositories
{
    public partial class LedgerTransactionRepository : IRepository<LedgerTransaction>
    ///PILOT 
    ///http://stackoverflow.com/questions/23280535/maybe-a-really-simple-dynamic-linq-to-entities-select-statement
    {
        public void Get(string gLAccountId, int tenant)
        {


            var q = (from record in context.LedgerTransactions
                     where record.Tenant == tenant && record.AccountId == gLAccountId && record.IsReconciled == false
                     select record);

            var ledgerTransactionsEqualityComparer = new LedgerTransactionsEqualityComparer();

            AutomaticReconcileEnum method1 = "1";
            var method2 = "4";
            var fields = new List<string>();
            
            
            //var qg= (from  r in q 
            //         group r by q.SelectDynamic( new List<string>() {"OpenAmount","DocumentDate"}
            var l = q.Select(rec => new LedgerTransactionDTO()
            {
                Id = rec.Id,
                OpenAmount = rec.OpenAmount,
                OpenAmountABS = Math.Abs( rec.OpenAmount),
                DocumentDate = DbFunctions.TruncateTime(rec.DocumentDate).Value ,
                AccountingDate= //EntityFunctions.TruncateTime(rec.AccountingDate)
                DbFunctions.TruncateTime(
                rec.AccountingDate).Value ,
                
                Reference1 = rec.Reference1

            });
            //expresion tree
            var fieldList=(new List<string>() {"OpenAmountABS","DocumentDate"}).ToArray();
            var lambada = GroupByExpression<LedgerTransactionDTO>(fieldList);
            var resGroupBy =l.GroupBy(lambada.Compile()).Where(g => g.Sum(r => r.OpenAmount) == 0).ToList();
            
            foreach (var item in resGroupBy)
            {
                //item.Sum( r=>r.
            }
            //var res = l.GroupBy(r => r., ledgerTransactionsEqualityComparer).Where(g => g.Sum(r => r.OpenAmount) == 0).ToList();

            //NewMethod();

        }

        private static void NewMethod()
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
        private static Expression<Func<LedgerTransactionDTO, string>> GetColumnName(string property)
        {
            var parameter = Expression.Parameter(typeof(LedgerTransactionDTO), "LedgerTransactionDTO");
            var LedgerTransactionDTOProperty = Expression.PropertyOrField(parameter, property);
            var lambda = Expression.Lambda<Func<LedgerTransactionDTO, string>>(LedgerTransactionDTOProperty, parameter);

            return lambda;
        }
        private static Expression<Func<LedgerTransactionDTO, string>> GetGroupKey(string property)
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


            var q = (from record in context.LedgerTransactions
                     where record.Tenant == tenant && record.AccountId == gLAccountId && record.IsReconciled == false
                     select record);

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
        string dbms;


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
}


#endif