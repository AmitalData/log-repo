using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class CustomerDataSearchService
    {
        private CardDataSearchService cardDataSearchService = null;
        private List<CardSearchResult> allCardSearchResultLists = null;
        private CardSearchAdvanceArgs cardSearchAdvanceArgs = null;
        private CustomerSearchArgs customerSearchArgs = null;

        public CustomerDataSearchService()
        {
            allCardSearchResultLists = new List<CardSearchResult>();
            cardDataSearchService = new CardDataSearchService();
        }


        public List<CustomerList> Run(CustomerSearchArgs customerSearchArgs)
        {
            this.customerSearchArgs = customerSearchArgs;
            cardSearchAdvanceArgs = GetCardSearchAdvanceArgs();
            IQueryable<CardSearch> cardSearches = GetCardSearchEntities();
            List<CustomerList>  customerLists = GetCustomerLists(customerSearchArgs.EntityLists, cardSearches);
            SetCardSearchWeightToCustomerList(customerLists);
            customerLists = SortCustomerLists(customerLists);
            return customerLists;
        }

        private IQueryable<CardSearch> GetCardSearchEntities()
        {
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(customerSearchArgs.Tenant);
            IQueryable<CardSearch> cardSearches = (from a in commonDataContext.CardSearches where a.Tenant == customerSearchArgs.Tenant && a.PartnerTypeId != "AC" select a);
            if (customerSearchArgs.FilterItems != null && customerSearchArgs.FilterItems.Count() > 0)
            {
                var isCustomerFilter = customerSearchArgs.FilterItems.Where(d => d.FieldName == "IsCustomer").FirstOrDefault();
                if (isCustomerFilter != null && isCustomerFilter.FieldValue != null)
                {
                    bool isCustomerFilterValue = bool.Parse(isCustomerFilter.FieldValue.ToString());
                    cardSearches = cardSearches.Where(d => d.IsCustomer == isCustomerFilterValue);
                }
            }
            return cardSearches;
        }

        private List<CustomerList> GetCustomerLists(IQueryable<CustomerList> customers,  IQueryable<CardSearch> cardSearches)
        {
            List<CustomerList> customerLists = new List<CustomerList>();
            List<string> cardIds = new List<string>();
            bool isFirstTime = true;
            while ((customerLists.Count < customerSearchArgs.PageSize && cardIds.Count() >= customerSearchArgs.PageSize) || isFirstTime)
            {
                List<CardSearchResult> cardSearchResults = GetFilteredCardSearchResultList(cardSearches);
                cardIds = cardSearchResults.Select(d => d.CardId).ToList();
                customerLists = customerLists.Concat(customers.Where(d => cardIds.Contains(d.Id)).ToList()).ToList();
                isFirstTime = false;
            }
            return customerLists;
        }

        private void SetCardSearchWeightToCustomerList(List<CustomerList> customerLists)
        {
            foreach (CustomerList customerList in customerLists)
            {
                customerList.SearchWeight = allCardSearchResultLists.First(c => c.CardId == customerList.Id).Weight;
            }
        }

        private List<CustomerList> SortCustomerLists(List<CustomerList> customerLists)
        {
            var sortedList = customerLists.OrderByDescending(c => c.SearchWeight).ThenBy(d => d.EnglishName).Take(customerSearchArgs.PageSize).ToList();

            if (!string.IsNullOrEmpty(customerSearchArgs.SortByColumnName) && !string.IsNullOrEmpty(customerSearchArgs.SortDirectin))
            {
                var propertyInfo = typeof(CustomerList).GetProperty(customerSearchArgs.SortByColumnName);
                if (customerSearchArgs.SortDirectin.ToLower() == "ascending")
                {
                    sortedList = sortedList.OrderBy(c => propertyInfo.GetValue(c, null)).ToList();
                }
                else
                {
                    sortedList = sortedList.OrderByDescending(c => propertyInfo.GetValue(c, null)).ToList();
                }
            }
           
            return sortedList;
        }

        private List<CardSearchResult> GetFilteredCardSearchResultList(IQueryable<CardSearch> cardSearches)
        {
            var cardSearchResultLists = cardDataSearchService.GetCardSearchDataResults(cardSearchAdvanceArgs, cardSearches);
            cardSearchResultLists = cardSearchResultLists.Where(d => !allCardSearchResultLists.Select(a => a.CardId).ToList().Contains(d.CardId)).ToList();
            allCardSearchResultLists = allCardSearchResultLists.Concat(cardSearchResultLists).ToList();
            return cardSearchResultLists;
        }

        private CardSearchAdvanceArgs GetCardSearchAdvanceArgs()
        {
            return new CardSearchAdvanceArgs()
            {
                SeachText = customerSearchArgs.SearchText, 
                Take =(int) (customerSearchArgs.PageSize * 2.5), 
                Skip  = 0 ,
                Tenant = customerSearchArgs.Tenant,
            };
        }
      
    }

    public class CustomerSearchArgs
    {
        public string SearchText { get; set; }
        public int Tenant { get; set; }
        public int PageSize { get; set; }
        public IQueryable<CustomerList> EntityLists { get; set; }
        public string SortByColumnName { get; set; }
        public string SortDirectin { get; set; }
        public List<QueryFilterItem> FilterItems { get; set; }

    }
}
