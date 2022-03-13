using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Base.Models.Api
{
    public class ApiQueryFiltersBuilder
    {
        private ApiQueryFilters _apiQueryFilters;

        public ApiQueryFiltersBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _apiQueryFilters = new ApiQueryFilters();
        }

        public ApiQueryFiltersBuilder Filter1Name(string Filter1Name)
        {
            _apiQueryFilters.Filter1Name = Filter1Name;
            return this;
        }
        public ApiQueryFiltersBuilder Filter1Value(string Filter1Value)
        {
            _apiQueryFilters.Filter1Value = Filter1Value;
            return this;
        }
        public ApiQueryFiltersBuilder Filter1Operator(string Filter1Operator)
        {
            _apiQueryFilters.Filter1Operator = Filter1Operator;
            return this;
        }

        public ApiQueryFiltersBuilder Filter2Name(string Filter2Name)
        {
            _apiQueryFilters.Filter2Name = Filter2Name;
            return this;
        }
        public ApiQueryFiltersBuilder Filter2Value(string Filter2Value)
        {
            _apiQueryFilters.Filter2Value = Filter2Value;
            return this;
        }
        public ApiQueryFiltersBuilder Filter2Operator(string Filter2Operator)
        {
            _apiQueryFilters.Filter2Operator = Filter2Operator;
            return this;
        }
        public ApiQueryFiltersBuilder PageSize(int pageSize)
        {
            _apiQueryFilters.PageSize = pageSize;
            return this;
        }
        public ApiQueryFiltersBuilder SortBy(string sortBy)
        {
            _apiQueryFilters.SortBy = sortBy;
            return this;
        }
        public ApiQueryFiltersBuilder SortDirection(string sortDirection)
        {
            _apiQueryFilters.SortDirection = sortDirection;
            return this;
        }

        public ApiQueryFilters Build()
        {
            ApiQueryFilters result = _apiQueryFilters;
            this.Reset();
            return result;
        }

        public ApiQueryFiltersBuilder WithModel(ApiQueryFilters apiQueryFilters)
        {
            _apiQueryFilters = apiQueryFilters;
            return this;
        }

        public ApiQueryFiltersBuilder WithDefualtValues()
        {
            _apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1
            };
            return this;
        }
    }
}
