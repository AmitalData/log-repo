using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.TreeFilterQuery.Expression
{
    public abstract class OperatorExpression
    {
        public System.Linq.Expressions.Expression LeftExpression { get; set; }
        public System.Linq.Expressions.Expression RightExpression { get; set; }
        public System.Linq.Expressions.Expression Expression { get; set; }

        public QueryFilterItem QueryFilterItem { get; set; }
        public System.Linq.Expressions.Expression  CreateExpression(System.Linq.Expressions.Expression expression, QueryFilterItem queryFilterItem)
        {
            Expression = expression;
            QueryFilterItem = queryFilterItem;
            if (!queryFilterItem.IsCustomField) SetLeftRightExpressions();
            else SetCustomFieldLeftRightExpressions();
            return Build();
        }

        private void SetLeftRightExpressions()
        {
           
            LeftExpression = System.Linq.Expressions.Expression.Property(Expression, WhereExpression.GetDeclaringProperty(Expression, QueryFilterItem.FieldName));
            RightExpression = GetRightExpression();

        }
        private void SetCustomFieldLeftRightExpressions()
        {
            if (QueryFilterItem.Operator == "IsEmpty" || QueryFilterItem.Operator == "IsNotEmpty")
            {
                QueryFilterItem.FieldValue = null;
                LeftExpression = System.Linq.Expressions.Expression.Property(Expression, WhereExpression.GetDeclaringProperty(Expression, QueryFilterItem.FieldName));
                RightExpression = GetRightExpression();
            }
            else
            {
                MethodInfo methodInfo = typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) });
                RightExpression = System.Linq.Expressions.Expression.Constant(0, typeof(int));
                if (QueryFilterItem.FieldValue == null || string.IsNullOrEmpty(QueryFilterItem.FieldValue.ToString())) return;

                if (IsValueTypeField())
                {
                    LeftExpression = System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression.PropertyOrField(Expression, QueryFilterItem.FieldName), methodInfo, System.Linq.Expressions.Expression.PropertyOrField(Expression, GetFieldName(QueryFilterItem.FieldValue)));
                }
                else
                {
                    LeftExpression = System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression.PropertyOrField(Expression, QueryFilterItem.FieldName), methodInfo, System.Linq.Expressions.Expression.Constant(QueryFilterItem.FieldValue));
                }
            }
           

        }
        private System.Linq.Expressions.Expression GetRightExpression()
        {
            if (QueryFilterItem.FieldValue == null || string.IsNullOrEmpty(QueryFilterItem.FieldValue.ToString())) return null;

            if (IsValueTypeField())
            {
                return System.Linq.Expressions.Expression.Property(Expression, WhereExpression.GetDeclaringProperty(Expression, GetFieldName(QueryFilterItem.FieldValue)));
            }
            return WhereExpression.ToStaticParameterExpressionOfType(WhereExpression.TryCastFieldValueType(QueryFilterItem.FieldValue, LeftExpression.Type), LeftExpression.Type);
        }

        public abstract System.Linq.Expressions.Expression Build();
        private string GetFieldName(object name)
        {
            if (name == null || string.IsNullOrEmpty(name.ToString())) return null;
            var names = name.ToString().Split('.');
            return names[names.Length - 1];
        }
        public bool IsValueTypeField()
        {
            return QueryFilterItem.Operator.ToString().Contains("Field") ? true : false;
        }


    }


    public class GreaterThan : OperatorExpression
    {
        public override System.Linq.Expressions.Expression Build()
        {
            return System.Linq.Expressions.Expression.GreaterThan(LeftExpression, RightExpression);
        }
    }

    public class LessThan : OperatorExpression
    {
        public override System.Linq.Expressions.Expression Build()
        {
            return System.Linq.Expressions.Expression.LessThan(LeftExpression, RightExpression);
        }
    }

    public class GreaterThanOrEqual : OperatorExpression
    {
        public override System.Linq.Expressions.Expression Build()
        {
            return System.Linq.Expressions.Expression.GreaterThanOrEqual(LeftExpression, RightExpression);
        }
    }

    public class LessThanOrEqual : OperatorExpression
    {
        public override System.Linq.Expressions.Expression Build()
        {
            return System.Linq.Expressions.Expression.LessThanOrEqual(LeftExpression, RightExpression);
        }
    }




    public class Equal : OperatorExpression
    {
        public override System.Linq.Expressions.Expression Build()
        {
            return System.Linq.Expressions.Expression.Equal(LeftExpression, RightExpression);
        }
    }

    public class NotEqual : OperatorExpression
    {
        public override System.Linq.Expressions.Expression  Build()
        {
            return System.Linq.Expressions.Expression.NotEqual(LeftExpression, RightExpression);
        }
    }


    public class Contains : OperatorExpression
    {
        public override System.Linq.Expressions.Expression Build()
        {
            if (IsValueTypeField())
            {
                return System.Linq.Expressions.Expression.Call(LeftExpression, WhereExpression.ContainsMethod, RightExpression);
            }

            MemberExpression field = System.Linq.Expressions.Expression.PropertyOrField(Expression, QueryFilterItem.FieldName);
            return System.Linq.Expressions.Expression.Call(field, WhereExpression.ContainsMethod, System.Linq.Expressions.Expression.Constant(QueryFilterItem.FieldValue, WhereExpression.StringType));

        }
    }

    public class NotContains : OperatorExpression
    {
        public override System.Linq.Expressions.Expression Build()
        {

            if (IsValueTypeField())
            {
                return System.Linq.Expressions.Expression.Not(System.Linq.Expressions.Expression.Call(LeftExpression, WhereExpression.ContainsMethod, RightExpression));
            }

            MemberExpression field = System.Linq.Expressions.Expression.PropertyOrField(Expression, QueryFilterItem.FieldName);
            return System.Linq.Expressions.Expression.Not(System.Linq.Expressions.Expression.Call(field, WhereExpression.ContainsMethod, System.Linq.Expressions.Expression.Constant(QueryFilterItem.FieldValue, WhereExpression.StringType)));

        }
    }


    public class IsEmpty : OperatorExpression
    {
        public override System.Linq.Expressions.Expression  Build()
        {
            //var isEmptyExpression = System.Linq.Expressions.Expression.Equal(LeftExpression, WhereExpression.ToStaticParameterExpressionOfType("", LeftExpression.Type));
            //var isNullExpression = System.Linq.Expressions.Expression.Equal(LeftExpression, WhereExpression.ToStaticParameterExpressionOfType(null, LeftExpression.Type));
            return System.Linq.Expressions.Expression.Equal(LeftExpression, WhereExpression.ToStaticParameterExpressionOfType(null, LeftExpression.Type));

        }
    }

    public class IsNotEmpty : OperatorExpression
    {
        public override System.Linq.Expressions.Expression  Build()
        {
            //var isEmptyExpression = System.Linq.Expressions.Expression.NotEqual(LeftExpression, WhereExpression.ToStaticParameterExpressionOfType("", LeftExpression.Type));
            //var isNullExpression = System.Linq.Expressions.Expression.NotEqual(LeftExpression, WhereExpression.ToStaticParameterExpressionOfType(null, LeftExpression.Type));
            return System.Linq.Expressions.Expression.NotEqual(LeftExpression, WhereExpression.ToStaticParameterExpressionOfType(null, LeftExpression.Type));
        }
    }


    public class StartsWith : OperatorExpression
    {
        public override System.Linq.Expressions.Expression  Build()
        {
            MemberExpression field = System.Linq.Expressions.Expression.PropertyOrField(Expression, QueryFilterItem.FieldName);
            return System.Linq.Expressions.Expression.Call(field, WhereExpression.StartsMethod, System.Linq.Expressions.Expression.Constant(QueryFilterItem.FieldValue));
        }
    }

    public class EndsWith : OperatorExpression
    {
        public override System.Linq.Expressions.Expression Build()
        {
            MemberExpression field = System.Linq.Expressions.Expression.PropertyOrField(Expression, QueryFilterItem.FieldName);
            return System.Linq.Expressions.Expression.Call(field, WhereExpression.EndsMethod, System.Linq.Expressions.Expression.Constant(QueryFilterItem.FieldValue));
        }
    }

    public class InList : OperatorExpression
    {
        public override System.Linq.Expressions.Expression  Build()
        {
            System.Linq.Expressions.Expression expression = null;
            foreach (string value in QueryFilterItem.FieldValue.ToString().Split(','))
            {
                expression = AddValueExpression(expression, value);
            }
            return expression;

        }

        private System.Linq.Expressions.Expression AddValueExpression(System.Linq.Expressions.Expression expression, string value)
        {
            MethodCallExpression contains = System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression.PropertyOrField(Expression, QueryFilterItem.FieldName), WhereExpression.ContainsMethod, System.Linq.Expressions.Expression.Constant(value));
            if (expression == null) return contains;
            return System.Linq.Expressions.Expression.Or(expression, contains); ;
        }
    }



    public class InListExact : OperatorExpression
    {
        public override System.Linq.Expressions.Expression  Build()
        {
            System.Linq.Expressions.Expression expression = null;
            foreach (string value in QueryFilterItem.FieldValue.ToString().Split(','))
            {
                expression = AddValueExpression(expression, value);

            }
            return expression;

        }

        private System.Linq.Expressions.Expression AddValueExpression(System.Linq.Expressions.Expression expression, string value)
        {
            MemberExpression field = System.Linq.Expressions.Expression.PropertyOrField(Expression, QueryFilterItem.FieldName);
            System.Linq.Expressions.Expression right = (value != null && value.ToLower() == "null") ? System.Linq.Expressions.Expression.Constant(null) : System.Linq.Expressions.Expression.Constant(value);
            expression = expression == null ? System.Linq.Expressions.Expression.Equal(LeftExpression, right) : System.Linq.Expressions.Expression.Or(expression, System.Linq.Expressions.Expression.Equal(LeftExpression, right));
            return expression;
        }
    }




    public class Exclude : OperatorExpression
    {
        public override System.Linq.Expressions.Expression Build()
        {
            System.Linq.Expressions.Expression expression = null;
            foreach (string value in QueryFilterItem.FieldValue.ToString().Split(new string[] { ",", "%2C" }, StringSplitOptions.None))
            {
                expression = AddValueExpression(expression, value);
            }

            return expression;
        }

        private System.Linq.Expressions.Expression AddValueExpression(System.Linq.Expressions.Expression expression, string value)
        {
            MemberExpression field = System.Linq.Expressions.Expression.PropertyOrField(expression, QueryFilterItem.FieldName);
            MethodCallExpression contains = System.Linq.Expressions.Expression.Call(field, WhereExpression.ContainsMethod, System.Linq.Expressions.Expression.Constant(value));
            if (expression == null) return System.Linq.Expressions.Expression.Not(contains);
            return System.Linq.Expressions.Expression.And(expression, System.Linq.Expressions.Expression.Not(contains)); ;
        }
    }



    public class InListInt : OperatorExpression
    {
        public override System.Linq.Expressions.Expression Build()
        {
            List<int> listOfInts = new List<int>();
            System.Linq.Expressions.Expression expression = null;

            foreach (string value in QueryFilterItem.FieldValue.ToString().Split(','))
            {
                listOfInts.Add(Convert.ToInt32(value));
            }

            foreach (int value in listOfInts)
            {
                expression = AddValueExpression(expression, value);

            }


            return expression;
        }

        private System.Linq.Expressions.Expression AddValueExpression(System.Linq.Expressions.Expression expression, int value)
        {
            System.Linq.Expressions.Expression right = System.Linq.Expressions.Expression.Constant(value, LeftExpression.Type);
            System.Linq.Expressions.Expression equals = System.Linq.Expressions.Expression.Equal(LeftExpression, right);
            if (expression == null) return  equals;
            return System.Linq.Expressions.Expression.Or(expression, equals);
        }

    }



    //    public class Between : OperatorExpression
    //{
    //    public override System.Linq.Expressions.Expression  Build()
    //    {

    //       System.Linq.Expressions.Expression grandExpression = null;
    //        string[] propertyinfo = QueryFilterItem.FieldName.Split('.');

    //       System.Linq.Expressions.Expression middle = LeftExpression;

    //        if (QueryFilterItem.FieldValue != null && QueryFilterItem.FieldValue2 != null)
    //        {
    //            if (QueryFilterItem.IsCustomField)
    //            {

    //               System.Linq.Expressions.Expression leftValue = System.Linq.Expressions.Expression.Constant(QueryFilterItem.FieldValue, middle.Type);
    //               System.Linq.Expressions.Expression rightValue = System.Linq.Expressions.Expression.Constant(QueryFilterItem.FieldValue2, middle.Type);

    //                MethodInfo mi = typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) });


    //                MethodCallExpression leftCompareExpression = System.Linq.Expressions.Expression.Call(middle, mi, System.Linq.Expressions.Expression.Constant(QueryFilterItem.FieldValue));
    //                MethodCallExpression rightCompareExpression = System.Linq.Expressions.Expression.Call(middle, mi, System.Linq.Expressions.Expression.Constant(QueryFilterItem.FieldValue2));
    //                System.Linq.Expressions.Expression zeroExpression = System.Linq.Expressions.Expression.Constant(0, typeof(int));


    //               System.Linq.Expressions.Expression e1 = System.Linq.Expressions.Expression.GreaterThanOrEqual(leftCompareExpression, zeroExpression);
    //               System.Linq.Expressions.Expression e2 = System.Linq.Expressions.Expression.LessThanOrEqual(rightCompareExpression, zeroExpression);
    //                grandExpression = System.Linq.Expressions.Expression.And(e1, e2);


    //            }
    //            else
    //            {
    //               System.Linq.Expressions.Expression left = System.Linq.Expressions.Expression.Constant(QueryFilterItem.FieldValue, middle.Type);
    //               System.Linq.Expressions.Expression rigt = System.Linq.Expressions.Expression.Constant(QueryFilterItem.FieldValue2, middle.Type);
    //               System.Linq.Expressions.Expression e1 = System.Linq.Expressions.Expression.GreaterThanOrEqual(middle, left);
    //               System.Linq.Expressions.Expression e2 = System.Linq.Expressions.Expression.LessThanOrEqual(middle, rigt);
    //                grandExpression = System.Linq.Expressions.Expression.And(e1, e2);



    //            }
    //        }
    //        else if (QueryFilterItem.FieldValue != null)
    //        {
    //            if (QueryFilterItem.IsCustomField)
    //            {
    //                System.Linq.Expressions.Expression left = LeftExpression;
    //                System.Linq.Expressions.Expression right = System.Linq.Expressions.Expression.Constant(QueryFilterItem.FieldValue, left.Type);
    //                MethodInfo mi = typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) });

    //                MemberExpression field = System.Linq.Expressions.Expression.PropertyOrField(Expression, QueryFilterItem.FieldName);

    //                MethodCallExpression compareExpression = System.Linq.Expressions.Expression.Call(field, mi, System.Linq.Expressions.Expression.Constant(QueryFilterItem.FieldValue));
    //                System.Linq.Expressions.Expression zeroExpression = System.Linq.Expressions.Expression.Constant(0, typeof(int));
    //                grandExpression = System.Linq.Expressions.Expression.GreaterThanOrEqual(compareExpression, zeroExpression);



    //            }
    //            else
    //            {
    //               System.Linq.Expressions.Expression left = LeftExpression;
    //               System.Linq.Expressions.Expression right = System.Linq.Expressions.Expression.Constant(QueryFilterItem.FieldValue, left.Type);
    //                grandExpression = System.Linq.Expressions.Expression.GreaterThanOrEqual(left, right);


    //            }
    //        }
    //        else
    //        {
    //            if (QueryFilterItem.IsCustomField)
    //            {
    //                System.Linq.Expressions.Expression left = LeftExpression;
    //                System.Linq.Expressions.Expression right = System.Linq.Expressions.Expression.Constant(QueryFilterItem.FieldValue2, left.Type);
    //                MethodInfo mi = typeof(String).GetMethod("CompareTo", new Type[] { typeof(String) });

    //                MemberExpression field = System.Linq.Expressions.Expression.PropertyOrField(Expression, QueryFilterItem.FieldName);

    //                MethodCallExpression compareExpression = System.Linq.Expressions.Expression.Call(field, mi, System.Linq.Expressions.Expression.Constant(QueryFilterItem.FieldValue2));
    //                System.Linq.Expressions.Expression zeroExpression = System.Linq.Expressions.Expression.Constant(0, typeof(int));
    //                grandExpression = System.Linq.Expressions.Expression.GreaterThanOrEqual(compareExpression, zeroExpression);



    //            }
    //            else
    //            {
    //               System.Linq.Expressions.Expression left = LeftExpression;
    //               System.Linq.Expressions.Expression right = System.Linq.Expressions.Expression.Constant(QueryFilterItem.FieldValue2, left.Type);
    //                grandExpression = System.Linq.Expressions.Expression.LessThanOrEqual(left, right);


    //            }
    //        }

    //        return grandExpression;
    //    }
    //}


}
