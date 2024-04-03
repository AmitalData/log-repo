using Logitude.ReportTests.Models;
using System.Collections;
using System.Linq;

namespace Logitude.ReportTests.Services.Asserts
{
    public class ReportAsserterService<T> : ReportAsserterOperatorsService<T> where T : BaseDataProvider
    {
        protected string Assert(T dataProvider, ReportAsserter<T> reportAsserter)
        {
            this.reportAsserter = reportAsserter;
            string[] fieldParts = this.reportAsserter.FieldName.Split('.');
            if (fieldParts.Length == 1) return AssertField(fieldParts[0], dataProvider);
            return AssertNestedObject(fieldParts, dataProvider);
        }

        private string AssertNestedObject(string[] fieldParts, object myObject)
        {
            var isClass = myObject.GetType().GetProperty(fieldParts[0]).IsClassProperty();
            var isList = myObject.GetType().GetProperty(fieldParts[0]).IsListProperty();

            if (!isClass && !isList) return "invalid field name";
            if (isClass) return AssertObject(fieldParts, myObject);
            return AssertList(fieldParts, myObject);
        }

        private string AssertObject(string[] fieldParts, object parentObject)
        {
            if (fieldParts.Length == 2) return OperateAssertion(parentObject.GetNestedPropertyValue(string.Join(".", fieldParts)), parentObject.GetNestedPropertyType(string.Join(".", fieldParts)));
            var myObject = parentObject.GetPropertyValue(fieldParts[0]);
            return AssertNestedObject(fieldParts.Skip(1).ToArray(), myObject);
        }

        private string AssertList(string[] fieldParts, object parentObject)
        {
            if (fieldParts.Length == 2) return AssertListValues(fieldParts, parentObject.GetPropertyValue(fieldParts[0]));
            return AssertNestedListValues(fieldParts, parentObject);
        }

        private string AssertNestedListValues(string[] fieldParts, object parentObject)
        {
            IList collection = (IList)parentObject.GetPropertyValue(fieldParts[0]);
            string error = null;
            foreach (var item in collection)
            {
                error = AssertNestedObject(fieldParts.Skip(1).ToArray(), item);
                if (error != null) break;
            }
            return error;
        }

        private string AssertListValues(string[] fieldParts, object list)
        {
            IList collection = (IList)list;
            string error = null;
            foreach (var item in collection)
            {
                error = AssertField(fieldParts[1], item);
                if (error != null) break;
            }
            return error;
        }

        private string AssertField(string fieldName, object myObject)
        {
            var fieldValue = myObject.GetPropertyValue(fieldName);
            var fieldType = myObject.GetPropertyType(fieldName);
            return OperateAssertion(fieldValue, fieldType);
        }

    }
}
