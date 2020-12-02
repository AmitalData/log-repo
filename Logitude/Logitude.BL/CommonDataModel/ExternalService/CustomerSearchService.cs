using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class CustomerSearchService
    {
        private CardSearchFilter cardSearchFilter = null;
        private List<CardSearchResult> allCardSearchResultLists = null;
        private CustomerSearchArgs customerSearchArgs = null;
        private CardSearchResultArgs cardSearchResultArgs = null;
        public CustomerSearchService(CustomerSearchArgs customerSearchArgs)
        {
            this.customerSearchArgs = customerSearchArgs;
            allCardSearchResultLists = new List<CardSearchResult>();
            cardSearchFilter = new CardSearchFilter();
            cardSearchResultArgs = GetCardSearchResultArgs();
        }


        public List<CustomerList> Run()
        {
            IQueryable<CardSearch> cardSearches = GetCardSearchEntities();
            List<CustomerList>  customerLists = GetCustomerLists(customerSearchArgs.Customers, cardSearches);
            customerLists = SortCustomerListBySearchWeight( customerLists);
            return customerLists;
        }

        private IQueryable<CardSearch> GetCardSearchEntities()
        {
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(customerSearchArgs.Tenant);
            IQueryable<CardSearch> cardSearches = (from a in commonDataContext.CardSearches where a.Tenant == customerSearchArgs.Tenant && a.PartnerTypeId != "AC" && a.IsCustomer  select a);
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
                allCardSearchResultLists = allCardSearchResultLists.Concat(cardSearchResults).ToList();
                isFirstTime = false;
            }
            return customerLists;
        }

 
        private List<CustomerList> SortCustomerListBySearchWeight(List<CustomerList> customerLists)
        {
            var sortedList = customerLists;
            foreach (CustomerList customerList in customerLists)
            {
                customerList.SearchWeight = allCardSearchResultLists.First(c => c.CardId == customerList.Id).Weight;
            }

            sortedList = sortedList.OrderByDescending(c => c.SearchWeight).ThenBy(d => d.EnglishName).Take(customerSearchArgs.PageSize).ToList();
            return customerLists;
        }

        private List<CardSearchResult> GetFilteredCardSearchResultList(IQueryable<CardSearch> cardSearches)
        {
            var cardSearchResultLists = cardSearchFilter.GetCardSearchDataResults(cardSearchResultArgs, cardSearches);
            cardSearchResultLists = cardSearchResultLists.Where(d => !allCardSearchResultLists.Select(a => a.CardId).ToList().Contains(d.CardId)).ToList();
            return cardSearchResultLists;
        }

        private CardSearchResultArgs GetCardSearchResultArgs()
        {
            return new CardSearchResultArgs()
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
        public IQueryable<CustomerList> Customers{ get; set; }
        public string SortByColumnName { get; set; }


    }


}
