using Logitude.ReportTests.Services.Asserts;
using System.Collections.Generic;

namespace Logitude.ReportTests.Models
{
    public class ReportAsserter<T> : ReportAsserterService<T> where T : BaseDataProvider
    {
        public ReportAsserter<T> Build(string fieldName, Operators @operator, List<object> values)
        {
            return new ReportAsserter<T>
            {
                FieldName = fieldName,
                Operator = @operator,
                Values = values
            };
        }

        public string FieldName { get; set; }
        public Operators Operator { get; set; }
        public List<object> Values { get; set; }


        internal string Assert(T dataProvider)
        {
            return Assert(dataProvider, this);
        }

    }
}
