using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ReportTests.Services
{
    public class ReportDataAssertService
    {
        protected object reportDataProvider;

        public void Initialize(object reportDataProvider)
        {
            this.reportDataProvider = reportDataProvider;
        }

        public void AssertFields(Table table)
        {
            table.CreateDynamicSet().ToList().ForEach(field =>
            {
                AssertField(
                    field.Get<string>("FieldName"),
                    field.Get<string>("Operation"),
                    field.Get<string>("ValueOne"),
                    field.Get<string>("ValueTwo"));
            });
        }

        private bool AssertField(string fieldName, string operation, string valueOne, string valueTwo)
        {
            return false;
        }
    }
}
