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
            };
            return this;
        }

        public ReportFliterItemBuilder Name(string name)
        {
            _reportFliterItem.Name = name;
            return this;
        }

        public ReportFliterItemBuilder Value(object value)
        {
            _reportFliterItem.Value = value;
            return this;
        }

        public ReportFliterItemBuilder Map(string map)
        {
            _reportFliterItem.Map = map;
            return this;
        }

        public ReportFliterItemBuilder EntityName(string entityName)
        {
            _reportFliterItem.EntityName = entityName;
            return this;
        }

        public ReportFliterItemBuilder SearchKeyValue(string searchKeyValue)
        {
            _reportFliterItem.SearchKeyValue = searchKeyValue;
            return this;
        }

        public ReportFliterItemBuilder Operator(string searchKeyName)
        {
            _reportFliterItem.SearchKeyName = searchKeyName;
            return this;
        }
    }
}
