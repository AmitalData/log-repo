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
    public class CustomerSearchFilter
    {
        private CardSearchFilter cardSearchFilter = null;
        private List<CardSearchResult> allCardSearchResultLists = null;
        private CustomerSearchFilterArgs customerSearchFilterArgs = null;
        private CardSearchResultArgs cardSearchResultArgs = null;
        public CustomerSearchFilter(CustomerSearchFilterArgs customerSearchFilterArgs)
        {
            this.customerSearchFilterArgs = customerSearchFilterArgs;
            allCardSearchResultLists = new List<CardSearchResult>();
            cardSearchFilter = new CardSearchFilter();
            cardSearchResultArgs = GetCardSearchResultArgs();
        }


        public List<CustomerList> Run()
        {
            IQueryable<CardSearch> cardSearches = GetCardSearches();
            List<CustomerList>  customerLists = GetCustomerLists(customerSearchFilterArgs.Customers, cardSearches);
            customerLists = SortCustomerListBySearchWeight( customerLists);
            return customerLists;
        }

        private IQueryable<CardSearch> GetCardSearches()
        {
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(customerSearchFilterArgs.Tenant);
            IQueryable<CardSearch> cardSearches = (from a in commonDataContext.CardSearches where a.Tenant == customerSearchFilterArgs.Tenant && a.PartnerTypeId != "AC" select a);
            return cardSearches;
        }

        private List<CustomerList> GetCustomerLists(IQueryable<CustomerList> customers,  IQueryable<CardSearch> cardSearches)
        {
            List<CustomerList> customerLists = new List<CustomerList>();
            List<string> cardIds = new List<string>();
            bool isFirstTime = true;
            while ((customerLists.Count < customerSearchFilterArgs.PageSize && cardIds.Count() >= customerSearchFilterArgs.PageSize) || isFirstTime)
            {
                List<CardSearchResult> cardSearchResults = GetCardSearchResultLists(cardSearches);
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

            sortedList = sortedList.OrderByDescending(c => c.SearchWeight).ThenBy(d => d.EnglishName).Take(customerSearchFilterArgs.PageSize).ToList();
            return customerLists;
        }

        private List<CardSearchResult> GetCardSearchResultLists(IQueryable<CardSearch> cardSearches)
        {
            var cardSearchResultLists = cardSearchFilter.GetCardSearchDataResults(cardSearchResultArgs, cardSearches);
            cardSearchResultLists = cardSearchResultLists.Where(d => !allCardSearchResultLists.Select(a => a.CardId).ToList().Contains(d.CardId)).ToList();
            return cardSearchResultLists;
        }

        private CardSearchResultArgs GetCardSearchResultArgs()
        {
            return new CardSearchResultArgs()
            {
                SeachText = customerSearchFilterArgs.SearchText, 
                Take =(int) (customerSearchFilterArgs.PageSize * 2.5), 
                Skip  = 0 ,
                Tenant = customerSearchFilterArgs.Tenant,
            };
        }

    }
    public class CustomerSearchFilterArgs
    {
        public string SearchText { get; set; }
        public int Tenant { get; set; }
        public int PageSize { get; set; }
        public IQueryable<CustomerList> Customers{ get; set; }
        public string SortByColumnName { get; set; }


    }


}
