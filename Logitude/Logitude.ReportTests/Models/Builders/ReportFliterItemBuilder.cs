using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ReportTests.Models.Builders
{
    public class ReportFliterItemBuilder
    {
        private ReportFliterItem _reportFliterItem;
        public ReportFliterItemBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _reportFliterItem = new ReportFliterItem();
        }

        public ReportFliterItem Build()
        {
            ReportFliterItem result = _reportFliterItem;
            this.Reset();
            return result;
        }

        public ReportFliterItemBuilder WithModel(ReportFliterItem reportFliter)
        {
            _reportFliterItem = reportFliter;
            return this;
        }

        public ReportFliterItemBuilder WithDefualtValues()
        {
            _reportFliterItem = new ReportFliterItem
            {
                Operator = "Equals",
            };
            return this;
        }

        public ReportFliterItemBuilder FieldName(string fieldName)
        {
            _reportFliterItem.FieldName = fieldName;
            return this;
        }

        public ReportFliterItemBuilder FieldValue(object fieldValue)
        {
            _reportFliterItem.FieldValue = fieldValue;
            return this;
        }

        public ReportFliterItemBuilder FieldDataType(string fieldDataType)
        {
            _reportFliterItem.FieldDataType = fieldDataType;
            return this;
        }

        public ReportFliterItemBuilder Operator(string operatr)
        {
            _reportFliterItem.Operator = operatr;
            return this;
        }
    }
}
