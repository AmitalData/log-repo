using Logitude.ReportTests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ReportTests.Services.Asserts
{
    public class ReportAsserterOperatorsService<T> where T : BaseDataProvider
    {
        protected ReportAsserter<T> reportAsserter;
        protected string OperateAssertion(object fieldValue, Type fieldType)
        {
            switch (reportAsserter.Operator)
            {
                case Operators.Equal: return AssertEqual(fieldValue, fieldType);
                case Operators.NotEquals: return AssertNotEquals(fieldValue, fieldType);
                case Operators.LessThan: return AssertLessThan(fieldValue, fieldType);
                case Operators.LessThanOrEqual: return AssertLessThanOrEqual(fieldValue, fieldType);
                case Operators.GreaterOrEqual: return AssertGreaterOrEqual(fieldValue, fieldType);
                case Operators.GreaterThan: return AssertGreaterThan(fieldValue, fieldType);
                case Operators.StartsWith: return AssertStartsWith(fieldValue);
                case Operators.EndsWith: return AssertEndsWith(fieldValue);
                default: throw new InvalidOperationException("Operator not supported.");
            }
        }

        private string AssertEndsWith(object fieldValue)
        {
            var value = reportAsserter.Values[0] as string;

            return ((string)fieldValue).EndsWith(value) ? null : GetErrorMessage(fieldValue);
        }

        private string AssertStartsWith(object fieldValue)
        {
            var value = reportAsserter.Values[0] as string;

            return ((string)fieldValue).StartsWith(value) ? null : GetErrorMessage(fieldValue);
        }

        private string AssertGreaterThan(object fieldValue, Type fieldType)
        {
            var value = reportAsserter.Values[0].ChangeType(fieldType);

            IComparable comparable = (IComparable)fieldValue;
            return comparable.CompareTo(value) == 1 ? null : GetErrorMessage(fieldValue);
        }

        private string AssertGreaterOrEqual(object fieldValue, Type fieldType)
        {
            var value = reportAsserter.Values[0].ChangeType(fieldType);

            IComparable comparable = (IComparable)fieldValue;
            return (comparable.CompareTo(value) == 1 || comparable.CompareTo(value) == 0) ? null : GetErrorMessage(fieldValue);
        }

        private string AssertLessThanOrEqual(object fieldValue, Type fieldType)
        {
            var value = reportAsserter.Values[0].ChangeType(fieldType);

            IComparable comparable = (IComparable)fieldValue;
            return (comparable.CompareTo(value) == -1 || comparable.CompareTo(value) == 0) ? null : GetErrorMessage(fieldValue);
        }

        private string AssertLessThan(object fieldValue, Type fieldType)
        {
            var value = reportAsserter.Values[0].ChangeType(fieldType);

            IComparable comparable = (IComparable)fieldValue;
            return comparable.CompareTo(value) == -1 ? null : GetErrorMessage(fieldValue);
        }

        private string AssertNotEquals(object fieldValue, Type fieldType)
        {
            var value = reportAsserter.Values[0].ChangeType(fieldType);
            return value != fieldValue ? null : GetErrorMessage(fieldValue);
        }

        private string AssertEqual(object fieldValue, Type fieldType)
        {
            var value = reportAsserter.Values[0].ChangeType(fieldType);
            return value == fieldValue ? null : GetErrorMessage(fieldValue);
        }

        private string GetErrorMessage(object fieldValue)
        {
            return this.reportAsserter.FieldName + " has unexpected response with value " + fieldValue;
        }

    }
}
