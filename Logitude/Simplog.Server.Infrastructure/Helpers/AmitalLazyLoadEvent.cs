using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

//namespace Simplog.Server.Infrastructure.Helpers
//{
public static partial class AmitalPrimeNgUtil
{

    public static IQueryable<T> LazyOrderBy<T>(
            this IQueryable<T> qry, AmitalLazyLoadEvent lle)
    {
        if (string.IsNullOrWhiteSpace(lle.sortField) || lle.sortField == "undefined")
        {
            return qry;
        }
        ParameterExpression _parm = Expression.Parameter(typeof(T));
        MemberExpression memberAccess = Expression.PropertyOrField(_parm, lle.sortField);
        LambdaExpression keySelector = Expression.Lambda(memberAccess, _parm);
        //
        MethodCallExpression orderBy = Expression.Call(
            typeof(Queryable),
            (lle.sortOrder == 1 ? "OrderBy" : "OrderByDescending"),
            new Type[] { typeof(T), memberAccess.Type },
            qry.Expression,
            Expression.Quote(keySelector));
        //
        return qry.Provider.CreateQuery<T>(orderBy);
    }

    public static IQueryable<T> LazySkipTake<T>(
            this IQueryable<T> qry, AmitalLazyLoadEvent lle)
    {
        int first = (int)lle.first;
        int rows = (int)lle.rows;

        if (rows > 0)
        {
            qry = qry.Skip(first);
            qry = qry.Take(rows);
        }
        return qry;
    }

    public static IQueryable<T> LazyFilters<T>(
            this IQueryable<T> qry, AmitalLazyLoadEvent lle, Func<IQueryable<T>> getBasic)
    {
        if (lle.filters != null)
        {
            foreach (var filterField in lle.filters)
            {
                PropertyInfo _propertyInfo = typeof(T).GetProperty(filterField.key);
                Type _type = _propertyInfo.PropertyType;
                IQueryable<T> qry1 = getBasic();
                bool useQ = false;
                Expression<Func<T, bool>> expressionPerField = null;
                foreach (var item in filterField.value)
                {



                    dynamic filterMetadata = (item as dynamic);
                    string matchMode = (string)filterMetadata.matchMode;
                    string @operator = (string)filterMetadata.@operator;

                    string filtervalue = filterMetadata.value.ToString();
                    if (!string.IsNullOrWhiteSpace(filtervalue))
                    {
                        useQ = true;
                        var whereClause1 = LazyDynamicFilterExpression<T>(
                            filterField.key, matchMode, filtervalue, _type
                            );
                        if (expressionPerField == null)
                        {
                            expressionPerField = whereClause1;
                        }
                        else
                        {
                            if (@operator == "or")
                            {

                                expressionPerField = expressionPerField.Or(whereClause1);

                            }
                            else
                            {
                                expressionPerField = expressionPerField.And(whereClause1);

                            }

                        }

                        //qry1 =qry1.Where(whereClause1);




                    }


                }
                if (useQ)
                {
                    //qry = qry.Intersect(qry1);
                    qry = qry.Where(expressionPerField);
                }

                //Dictionary<string, Object> _value =
                //        ((Dictionary<string, Object>)filterField.value);
                //var whereClause = LazyDynamicFilterExpression<T>(filterField.key,
                //        (string)_value["matchMode"], _value["value"].ToString(), _type);


            }
        }
        return qry;
    }
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
    {

        ParameterExpression p = a.Parameters[0];

        SubstExpressionVisitor visitor = new SubstExpressionVisitor();
        visitor.subst[b.Parameters[0]] = p;

        Expression body = Expression.AndAlso(a.Body, visitor.Visit(b.Body));
        return Expression.Lambda<Func<T, bool>>(body, p);
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
    {

        ParameterExpression p = a.Parameters[0];

        SubstExpressionVisitor visitor = new SubstExpressionVisitor();
        visitor.subst[b.Parameters[0]] = p;

        Expression body = Expression.OrElse(a.Body, visitor.Visit(b.Body));
        return Expression.Lambda<Func<T, bool>>(body, p);
    }
    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
    {

        ParameterExpression p = a.Parameters[0];

        SubstExpressionVisitor visitor = new SubstExpressionVisitor();
        visitor.subst[b.Parameters[0]] = p;

        Expression body = Expression.Not(a.Body);
        return Expression.Lambda<Func<T, bool>>(body, p);
    }

    private static Expression<Func<TEntity, bool>>
        LazyDynamicFilterExpression<TEntity>(
            string propertyName, string op, string value, Type valueType)
    {
        Type type = typeof(TEntity);
        object asType = AsType(value, valueType);
        ParameterExpression p = Expression.Parameter(type, "x");
        MemberExpression member = Expression.Property(p, propertyName);
        string _stringValue = asType.ToString();
        //ConstantExpression valueExpression = Expression.Constant(asType);
        System.Linq.Expressions.Expression //right = System.Linq.Expressions.Expression.Constant(item.FieldValue, left.Type);
            valueExpression = Expression.Constant(asType, member.Type);
        //
        MethodInfo method;
        Expression q;
        //
        switch (op.ToLower())
        {
            case "gt":
            case "largerthan":
                q = Expression.GreaterThan(member, valueExpression);
                break;
            case "lt":
            case "lessthan":
                q = Expression.LessThan(member, valueExpression);
                break;
            case "equals":
                q = Expression.Equal(member, valueExpression);
                break;
            case "lte":
            case "lessthanorequal":
                q = Expression.LessThanOrEqual(member, valueExpression);
                break;
            case "gte":
            case "greaterthanorequal":
                
                q = Expression.GreaterThanOrEqual(member, valueExpression);
                break;
            case "notequals":
                q = Expression.NotEqual(member, valueExpression);
                break;
            case "notcontains":
            case "contains":
                method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                q = Expression.Call(member, method ?? throw new InvalidOperationException(),
                    Expression.Constant(_stringValue, typeof(string)));
                if (op.ToLower() == "notcontains")
                {
                    Expression notExpr = Expression.Not(q);
                    q = notExpr;


                }
                break;
            case "startswith":
                method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                q = Expression.Call(
                    member,
                    method ?? throw new InvalidOperationException(),
                    Expression.Constant(_stringValue, typeof(string)));
                break;
            case "endswith":
                method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                q = Expression.Call(member, method ?? throw new InvalidOperationException(),
                    Expression.Constant(_stringValue, typeof(string)));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(op), $"filter matchMode of: '{op}', not gt/lt/equals/lte/gte/notequals/contains/startswith/endswith");
        }
        //
        return Expression.Lambda<Func<TEntity, bool>>(q, p);
    }

    private static object AsType(string value, Type type)
    {
        //TODO: This method needs to be expanded to include all appropriate use cases
        string v = value;
        if (value.StartsWith("'") && value.EndsWith("'"))
            v = value.Substring(1, value.Length - 2);
        if (value.StartsWith("\"") && value.EndsWith("\""))
            v = value.Substring(1, value.Length - 2);
        //
        if (type == typeof(string)) return v;
        if (type == typeof(DateTime)) return DateTime.Parse(v);
        if (type == typeof(DateTime?)) return DateTime.Parse(v);
        if (type == typeof(int)) return int.Parse(v);
        if (type == typeof(int?)) return int.Parse(v);
        if (type == typeof(long) || type == typeof(long?)) return long.Parse(v);
        if (type == typeof(decimal) || type == typeof(decimal?)) return decimal.Parse(v);
        if (type == typeof(short) || type == typeof(short?)) return short.Parse(v);
        if (type == typeof(byte) || type == typeof(byte?)) return byte.Parse(v);
        if (type == typeof(bool) || type == typeof(bool?)) return bool.Parse(v);
        //
        throw new ArgumentException("ItzikPrimeNG.LazyLoading.Helpers.AsType: " +
            "A filter was attempted for a field with value '" + value + "' and type '" +
            type + "' however this type is not currently supported");
    }
}



    public class AmitaFilterMetadata
    {
        public string key;
        public Object[] value;

    }
    public class FilterMetadata
    {
        public string @value;
        public string @matchMode;
        public string @operator;
    }

    public class AmitalLazyLoadEvent
    {
        public bool GetCount;

        public long first;

        public long rows;

        public string sortField;

        public int sortOrder;

        public object multiSortMeta;

        public Dictionary<string, Dictionary<string, Object>> filters_old;
        public AmitaFilterMetadata[] filters;

        public object globalFilter;

        public override string ToString()
        {
            StringBuilder _return = new StringBuilder("record:[");
            _return.AppendFormat("first: {0}, rows: {1}, ", first, rows);
            _return.AppendFormat("sortField: {0}, sortOrder: {1}, ", sortField, sortOrder);
            _return.AppendFormat("multiSortMeta: {0}, ", multiSortMeta.ToString());
            _return.AppendFormat("filters: {0}, ", filters.ToString());
            _return.AppendFormat("globalFilter: {0}]", globalFilter.ToString());
            return _return.ToString();
        }
    }

    internal class SubstExpressionVisitor : System.Linq.Expressions.ExpressionVisitor
    {
        public Dictionary<Expression, Expression> subst = new Dictionary<Expression, Expression>();

        protected override Expression VisitParameter(ParameterExpression node)
        {
            Expression newValue;
            if (subst.TryGetValue(node, out newValue))
            {
                return newValue;
            }
            return node;
        }
    }
    public enum SortingEnumeration
    {
        OrderByAsc = 1,
        OrderByDesc = -1
    }

    public enum OperatorEnumeration
    {
        And = 1,
        Or = 2,
        None = 3
    }

    public static class OperatorConstant
    {
        private const string And = "and";
        private const string Or = "or";

        public static OperatorEnumeration ConvertOperatorEnumeration(string value)
        {
            switch (value.ToLower())
            {
                case And:
                    return OperatorEnumeration.And;
                case Or:
                    return OperatorEnumeration.Or;
                default:
                    return OperatorEnumeration.None;
            }
        }
    }
//}
