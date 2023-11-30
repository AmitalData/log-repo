using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using System.Text.RegularExpressions;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.Server.Tools.TreeFilterQuery.Expression
{
    public static class WhereExpression
    {

        public static readonly Type StringType = typeof(string);


        private static readonly Type ExpType = typeof(System.Linq.Expressions.Expression);

        private static readonly MethodInfo AndExpMethod = ExpType.GetRuntimeMethod("AndAlso", new[] { ExpType, ExpType });


        private static readonly MethodInfo OrExpMethod = ExpType.GetRuntimeMethod("OrElse", new[] { ExpType, ExpType });


        public static readonly MethodInfo ContainsMethod = StringType.GetRuntimeMethod("Contains", new[] { StringType });
        public static readonly MethodInfo StartsMethod = StringType.GetRuntimeMethod("StartsWith", new[] { StringType });
        public static readonly MethodInfo EndsMethod = StringType.GetRuntimeMethod("EndsWith", new[] { StringType });
        public static readonly MethodInfo CompareToMethod = StringType.GetRuntimeMethod("CompareTo", new[] { StringType });

        private static readonly Type[] AvailableCastTypes =
        {
            typeof(DateTime),
            typeof(DateTime?),
            typeof(DateTimeOffset),
            typeof(DateTimeOffset?),
            typeof(TimeSpan),
            typeof(TimeSpan?),
            typeof(bool),
            typeof(bool?),
            typeof(byte?),
            typeof(sbyte?),
            typeof(short),
            typeof(short?),
            typeof(ushort),
            typeof(ushort?),
            typeof(int),
            typeof(int?),
            typeof(uint),
            typeof(uint?),
            typeof(long),
            typeof(long?),
            typeof(ulong),
            typeof(ulong?),
            typeof(Guid),
            typeof(Guid?),
            typeof(double),
            typeof(double?),
            typeof(float),
            typeof(float?),
            typeof(decimal),
            typeof(decimal?),
            typeof(char),
            typeof(char?),
            typeof(string)
        };


        public static Expression<Func<T, bool>> GetTreeExpression<T>(this QueryFilterItem filter, string suffix = "")
        {
            var expression = System.Linq.Expressions.Expression.Parameter(typeof(T), "e" + suffix);
            return System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(GetExpressionForTreeField(expression, filter, suffix), expression);
        }

        

        private static System.Linq.Expressions.Expression GetExpressionForTreeField(System.Linq.Expressions.Expression expression, QueryFilterItem filter, string suffix)
        {


            if (filter == null)
                throw new ArgumentNullException(nameof(filter));
            if (filter.FilterType == "None" || string.IsNullOrEmpty(filter.FilterType) || !string.IsNullOrEmpty(filter.Operator))
                return GetExpressionForField(expression, filter, suffix + "0");

            if (!(filter.QueryFilterItems?.Any() ?? false))
                throw new ArgumentException("Filter operands with operator type different from TreeFilterType.None cannot be empty.");



            var i = 0;
            var expressions = GetExpressionForTreeField(expression, filter.QueryFilterItems[i], suffix + i);
            var methodInfo = filter.FilterType == "And" ? AndExpMethod : OrExpMethod;
            for (i = 1; i < filter.QueryFilterItems.Count; i++)
            {
                expressions = (BinaryExpression)methodInfo.Invoke(null, new object[] { expressions, GetExpressionForTreeField(expression, filter.QueryFilterItems[i], suffix + i) });
            }
            return expressions;
          
        }

    
        private static System.Linq.Expressions.Expression GetExpressionForField(System.Linq.Expressions.Expression expression, QueryFilterItem filter, string suffix)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            if (filter.FilterType == "None" || string.IsNullOrWhiteSpace(filter.FieldName))
                throw new ArgumentException("Filter type cannot be None for single filter.");

            return GenerateExpressionOneField(expression, filter);
        }

        private static System.Linq.Expressions.Expression GenerateExpressionOneField(System.Linq.Expressions.Expression expression, QueryFilterItem item)
        {
            switch (item.Operator.Replace("Field", ""))
            {
                case "LessThan": return new LessThan().CreateExpression(expression, item);
                case "LessThanOrEqual": return new LessThanOrEqual().CreateExpression(expression, item);
                case "GreaterThanOrEqual": return new GreaterThanOrEqual().CreateExpression(expression, item);
                case "LargerThan":
                case "GreaterThan":
                return new GreaterThan().CreateExpression(expression, item);
                case "Contains": return new Contains().CreateExpression(expression, item);
                case "NotContains": return new NotContains().CreateExpression(expression, item);
                case "Equal": return new Equal().CreateExpression(expression, item);
                case "NotEqual": return new NotEqual().CreateExpression(expression, item);
                case "IsEmpty":
                case "IsNull": return new IsEmpty().CreateExpression(expression, item);
                case "IsNotEmpty":
                case "IsNotNull": return new IsNotEmpty().CreateExpression(expression, item);
                case "PartnerEntityExpression": return new PartnerEntityExpression().CreateExpression(expression, item);
                case "StartsWith": return new StartsWith().CreateExpression(expression, item);
                case "EndsWith": return new EndsWith().CreateExpression(expression, item);
                case "InList": return new InList().CreateExpression(expression, item);
                case "InListExact": return new InList().CreateExpression(expression, item);
                case "Exclude": return new Exclude().CreateExpression(expression, item);
                case "InListInt": return new Exclude().CreateExpression(expression, item);
                //case "Between": return new Between().CreateExpression(expression, item);
                default: throw new InvalidOperationException("Operator not supported.");
            }
            
        }

        public static object TryCastFieldValueType(object value, Type type)
        {
            if (value == null || (!AvailableCastTypes.Contains(type) && !type.GetTypeInfo().IsEnum))
                throw new InvalidCastException($"Cannot convert value to type {type.Name}.");


            if (value.GetType() == type)
                return value;

            if (type.GetTypeInfo().BaseType == typeof(Enum))
                return Enum.Parse(type, Convert.ToString(value));


            type = (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) ? type.GenericTypeArguments[0] : type;
           object res = (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) ? Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(type)) : Activator.CreateInstance(type);


            var argTypes = new[] { StringType, type.MakeByRefType() };
            object[] args = { Convert.ToString(value), res };
            var tryParse = type.GetRuntimeMethod("TryParse", argTypes);

            if (!(bool)(tryParse?.Invoke(null, args) ?? false))
                throw new InvalidCastException($"Cannot convert value to type {type.Name}.");

            return args[1];
        }

        public static System.Linq.Expressions.Expression ToStaticParameterExpressionOfType(object obj, Type type)
            => System.Linq.Expressions.Expression.Convert(
                System.Linq.Expressions.Expression.Property(
                    System.Linq.Expressions.Expression.Constant(new { obj }),
                    "obj"),
                type);





        public static PropertyInfo GetDeclaringProperty(System.Linq.Expressions.Expression expression, string name)
        {
            var type = expression.Type;
            var propertiy = type.GetRuntimeProperties().SingleOrDefault(d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (name == "PartnerEntityField") return type.GetRuntimeProperties().FirstOrDefault();
   
            if (propertiy == null) throw new InvalidOperationException(string.Format("Property '{0}' not found on type '{1}'", name, type));
   
            if (type != propertiy.DeclaringType)
            {
                propertiy = propertiy.DeclaringType.GetRuntimeProperties().SingleOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }
            return propertiy;
        }
    }
}
