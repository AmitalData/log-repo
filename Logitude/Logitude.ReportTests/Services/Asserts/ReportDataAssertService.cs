using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Logitude.ReportTests.Models;
using System.Dynamic;
using System.Collections.Generic;
using System;

namespace Logitude.ReportTests.Services
{
    public class ReportDataAssertService<T> where T : BaseDataProvider
    {
        protected T reportDataProvider;
        private List<string> errors;

        public void Initialize(T reportDataProvider)
        {
            this.reportDataProvider = reportDataProvider;
        }

        public void AssertFields(Table table)
        {
            errors = new List<string>();
            table.CreateDynamicSet().ToList().ForEach(field => { AssertField(field); });
            if (errors.Any()) throw new System.Exception("Unexpected Response\r\n" + string.Join("\r\n", errors));
        }

        private void AssertField(ExpandoObject reportFliterItemObject)
        {
            ReportAsserter<T> reportAsserter = BuildReportAsserter(reportFliterItemObject);
            var error = reportAsserter.Assert(reportDataProvider);
            if (error != null) errors.Add(error);
        }

        private ReportAsserter<T> BuildReportAsserter(ExpandoObject reportFliterItemObject)
        {
            var values = new List<object>
            {
                reportFliterItemObject.Get<string>("ValueOne"),
                reportFliterItemObject.Get<string>("ValueTwo")
            };
            return new ReportAsserter<T>().Build(reportFliterItemObject.Get<string>("FieldName"), ToEnum(reportFliterItemObject.Get<string>("Operation"), Operators.Equal), values);
        }

        public S ToEnum<S>(string value, S defaultValue) where S : struct
        {
            return string.IsNullOrEmpty(value) ? defaultValue : Enum.TryParse<S>(value, true, out S result) ? result : defaultValue;
        }
    }
}
