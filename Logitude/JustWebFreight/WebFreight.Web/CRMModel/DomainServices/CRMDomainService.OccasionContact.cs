using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using WebFreight.Web.Security;

namespace WebFreight.Web.CRMModel.DomainServices
{
    public partial class CRMDomainService
    {
        [System.ServiceModel.DomainServices.Server.Query(HasSideEffects = true)]
        public List<OccasionContactList> GetOccasionContactFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("OccasionContact", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }
            
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            OccasionContactArgs args = this.AnalyzeOccasionFilters(filters);

            ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
            IQueryable<Customer> customers = this.GetFilteredCustomers(args, commonDataContext, tenant);
            IQueryable<CardContact> contacts = this.GetCustomerContacts(customers, commonDataContext, tenant);

            if (!string.IsNullOrEmpty(args.OccasionId))
            {
                List<string> contactsIds = this.GetContactsIdsFromOccasion(args.OccasionId, tenant);
                if (contactsIds != null)
                {
                    contacts = contacts.Where(d => contactsIds.Contains(d.ContactId));
                }
            }

            if (!string.IsNullOrEmpty(args.ProductTypes))
            {
                List<string> myproductsTypesList = this.GetList(args.ProductTypes, commonDataContext, tenant);
                if (myproductsTypesList.Count() > 0)
                {
                    List<CardContactProduct> cardContactProducts = commonDataContext.CardContactProducts.Where(d => myproductsTypesList.Contains(d.ProductTypeCode)).ToList();
                    List<string> cardContactsIds = cardContactProducts.Select(s => s.CardContactId).ToList();
                    contacts = contacts.Where(d => cardContactsIds.Contains(d.Id));
                }
            }

            if (!string.IsNullOrEmpty(args.AdditionalServices))
            {
                List<string> myAdditionalServicesList = this.GetList(args.AdditionalServices, commonDataContext, tenant);
                if (myAdditionalServicesList.Count() > 0)
                {
                    List<CardContactAdditionalService> cardContactAdditionalServices = commonDataContext.CardContactAdditionalServices.Where(d => myAdditionalServicesList.Contains(d.AdditionalServiceId)).ToList();
                    List<string> cardContactsIds = cardContactAdditionalServices.Select(s => s.CardContactId).ToList();
                    contacts = contacts.Where(d => cardContactsIds.Contains(d.Id));
                }
            }

            List<OccasionContactList> myResult = new List<OccasionContactList>();
            if (contacts != null && contacts.Count() > 0)
            {
                myResult = this.BuildFilteredContacts(contacts, commonDataContext, tenant);
            }

            if (!string.IsNullOrEmpty(args.SearchText))
            {
                myResult = myResult.Where(f => f.Email != null && f.Email.ToLower().StartsWith(args.SearchText.ToLower())
                        || f.Name != null && f.Name.ToLower().StartsWith(args.SearchText.ToLower())
                        || f.Company != null && f.Company.ToLower().StartsWith(args.SearchText.ToLower())).ToList();
            }

            if (!queryOperations.GetAll)
            {
                myResult = myResult.Skip(queryOperations.PageIndex).ToList();
                myResult = myResult.Take(queryOperations.PageSize).ToList();
            }

            return myResult;
        }

        public int GetOccasionContactFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("OccasionContact", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            OccasionContactListQueryService queryService = new OccasionContactListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        private OccasionContactArgs AnalyzeOccasionFilters(ApiQueryFilters filters)
        {
            OccasionContactArgs args = new OccasionContactArgs();

            List<PropertyInfo> filterProperties = filters.GetType().GetProperties().ToList();
            for (int i = 1; i <= 10; i++)
            {
                object filterNameProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Name")).GetValue(filters);
                object filterValue1 = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Value")).GetValue(filters);

                if (filterNameProp != null)
                {
                    string filterName = filterNameProp.ToString();
                    string filterValue = filterValue1 != null ? filterValue1.ToString() : null;

                    switch (filterName)
                    {
                        case "SearchText":
                            {
                                args.SearchText = filterValue;
                                break;
                            }

                        case "CustomerSizeId":
                            {
                                args.CustomerSizeId = filterValue;
                                break;
                            }

                        case "RegionId":
                            {
                                args.RegionId = filterValue;
                                break;
                            }

                        case "IndustryId":
                            {
                                args.IndustryId = filterValue;
                                break;
                            }

                        case "OccasionId":
                            {
                                args.OccasionId = filterValue;
                                break;
                            }

                        case "Products":
                            {
                                args.ProductTypes = filterValue;
                                break;
                            }

                        case "AdditionalServices":
                            {
                                args.AdditionalServices = filterValue;
                                break;
                            }
                    }
                }
            }

            return args;
        }
        private IQueryable<Customer> GetFilteredCustomers(OccasionContactArgs args, ICommonDataContext commonDataContext, int tenant)
        {
            CustomerRepository customerRepository = new CustomerRepository(commonDataContext);
            IQueryable<Customer> customers = customerRepository.GetCustomers(tenant);
            customers = customers.Where(d => d.IsCustomer);

            if (!string.IsNullOrEmpty(args.CustomerSizeId))
            {
                customers = customers.Where(d => d.CustomerSizeId == args.CustomerSizeId);
            }

            if (!string.IsNullOrEmpty(args.RegionId))
            {
                customers = customers.Where(d => d.RegionId == args.RegionId);
            }

            if (!string.IsNullOrEmpty(args.IndustryId))
            {
                customers = customers.Where(d => d.IndustryId == args.IndustryId);
            }

            return customers;
        }
        private IQueryable<CardContact> GetCustomerContacts(IQueryable<Customer> customers, ICommonDataContext commonDataContext, int tenant)
        {
            List<string> customersIds = customers.Select(s => s.Id).ToList();

            CardContactRepository cardContactRepository = new CardContactRepository(commonDataContext);
            IQueryable<CardContact> contacts = cardContactRepository.GetCardsContactsForCustomerIds(customersIds, tenant);

            return contacts;
        }
        private List<string> GetContactsIdsFromOccasion(string occasionId, int tenant)
        {
            ICRMContext cRMContext = CRMContext.GetContext(tenant);
            OccasionRepository occasionRepository = new OccasionRepository(cRMContext);
            OccasionInviteeRepository occasionInviteeRepository = new OccasionInviteeRepository(cRMContext);
            IQueryable<OccasionInvitee> occasionInvitees = occasionInviteeRepository.GetOccasionInviteesByOccasion(occasionId, tenant);
            List<string> contactsIds = occasionInvitees.Select(s => s.ContactId).ToList();
            return contactsIds;
        }
        private List<string> GetList(string myString, ICommonDataContext commonDataContext, int tenant)
        {
            List<string> myList = new List<string>();

            myString = myString.Replace(" ", "");

            if (myString.ToLower() == "all")
            {
            }

            else
            {
                myString = myString.Trim(',');
                string[] mySplitString = myString.Split(',');
                myList = mySplitString.ToList();
            }

            return myList;
        }
        private List<OccasionContactList> BuildFilteredContacts(IQueryable<CardContact> contacts, ICommonDataContext commonDataContext, int tenant)
        {
            List<OccasionContactList> myResult = new List<OccasionContactList>();

            foreach (CardContact cardContact in contacts)
            {
                string regionName = "";
                string industryName = "";
                string customerSizeName = "";
                if (cardContact.Card != null && cardContact.Card.Customer != null)
                {
                    if (!string.IsNullOrEmpty(cardContact.Card.Customer.RegionId))
                    {
                        Region region = commonDataContext.Regions.Where(d => d.Id == cardContact.Card.Customer.RegionId && d.Tenant == tenant).FirstOrDefault();
                        if (region != null)
                        {
                            regionName = region.Name;
                        }
                    }

                    if (!string.IsNullOrEmpty(cardContact.Card.Customer.IndustryId))
                    {
                        Industry industry = commonDataContext.Industries.Where(d => d.Id == cardContact.Card.Customer.IndustryId && d.Tenant == tenant).FirstOrDefault();
                        if (industry != null)
                        {
                            industryName = industry.Name;
                        }
                    }

                    if (!string.IsNullOrEmpty(cardContact.Card.Customer.CustomerSizeId))
                    {
                        CustomerSize customerSize = commonDataContext.CustomerSizes.Where(d => d.Id == cardContact.Card.Customer.CustomerSizeId && d.Tenant == tenant).FirstOrDefault();
                        if (customerSize != null)
                        {
                            customerSizeName = customerSize.Name;
                        }
                    }
                }

                string productsNames = "";
                CardContactProductRepository cardContactProductRepository = new CardContactProductRepository(commonDataContext);
                IQueryable<CardContactProduct> products = cardContactProductRepository.GetProductsByCardContactIdd(cardContact.Id, tenant);
                if (products != null && products.Count() > 0)
                {
                    foreach (CardContactProduct item in products)
                    {
                        if (item.ProductType != null)
                        {
                            if (string.IsNullOrEmpty(productsNames))
                            {
                                productsNames = item.ProductType.Name;
                            }

                            else
                            {
                                productsNames = productsNames + ", " + item.ProductType.Name;
                            }
                        }
                    }
                }

                myResult.Add(new OccasionContactList()
                {
                    Id = cardContact.ContactId,
                    Name = cardContact.Contact == null ? null : cardContact.Contact.EnglishName,
                    Email = cardContact.Contact == null ? null : cardContact.Contact.Email,
                    Company = cardContact.Card == null ? null : cardContact.Card.EnglishName,
                    Region = regionName,
                    Industry = industryName,
                    Product = productsNames,
                    CustomerSize = customerSizeName,
                    ContactMobile = cardContact.Contact == null ? null : cardContact.Contact.Mobile,
                    ContactPhone = cardContact.Contact == null ? null : cardContact.Contact.BusinessPhone,
                    ContactPosition = cardContact.Contact == null ? null : cardContact.Contact.Position,
                    ContactTel = cardContact.Contact == null ? null : cardContact.Contact.BusinessPhone,
                });
            }

            return myResult;
        }
    }

    public class OccasionContactArgs
    {
        public string CustomerSizeId { get; set; }
        public string RegionId { get; set; }
        public string IndustryId { get; set; }
        public string OccasionId { get; set; }
        public string ProductTypes { get; set; }
        public string AdditionalServices { get; set; }
        public string SearchText { get; set; }
    }

    //public class OccasionContactSearchresult
    //{
    //    public string ContactId { get; set; }
    //    public string Name { get; set; }
    //    public string Email { get; set; }
    //    public string Company { get; set; }
    //    public string Region { get; set; }
    //    public string Industry { get; set; }
    //    public string Product { get; set; }
    //    public string CustomerSize { get; set; }
    //    public string ContactPhone { get; set; }
    //    public string ContactPosition { get; set; }
    //    public string ContactMobile { get; set; }
    //    public string ContactTel { get; set; }
    //}
}