using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Domain.EntityLists;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using AmitalCloud.Infrastructure.Domain.Interfaces;

namespace AmitalCloud.Infrastructure.Data.Queries
{
    public class AddressQuery
    {
        IRepository<IAmitalCloudContext> repository;
        IAmitalCloudContext context;
        public AddressQuery() : this(0)
        {
        }
        public AddressQuery(int tenant)
        {
            context = AmitalCloudContext.GetContext(tenant);
            repository = (IRepository<IAmitalCloudContext>) new Repository< Address>(context);
        }
        public AddressQuery(IRepository<IAmitalCloudContext> addressRepository) => repository = addressRepository;
        #region Get Single AddressPM
        public AddressPM GetSinglePM(string id, int tenant) => GetAddressPMQuery().Where(a => a.Tenant == tenant && a.Id == id).FirstOrDefault(); 
        public AddressPM GetSinglePMByExternalId(string exteranlId, int tenant) =>  GetAddressPMQuery().Where(a => a.Tenant == tenant && a.Id == exteranlId).FirstOrDefault(); 
        public AddressPM GetAddressByExternalId(string exteranlId, int tenant) => GetSinglePMByExternalId(exteranlId, tenant);  
        public AddressPM GetAddressByCardId(string cardId, int tenant) =>GetAddressPMQuery().Where(a => a.Tenant == tenant  && a.CardId == cardId && (a.AddressTypeId == "B" || a.AddressTypeId == "M" || a.AddressTypeId == "O")).FirstOrDefault(); 
        public AddressPM GetSingleAddressPM(string id, int tenant, bool fromcache)
        {
            if (!fromcache)
            {
                return GetSingleAddressPM(id, tenant);
            }
            string cacheKey = $"GetSingleAddressPM_({id}_{tenant})";
            return CacheManager.GetOrInsertNewObject<AddressPM>(cacheKey, () =>
            {
                return GetSingleAddressPM(id, tenant);
            });
        }
        public AddressPM GetAddressPMByTypeAndCard(string cardId, string typeId, int tenant) => GetAddressPMQuery().Where(a => a.Tenant == tenant && a.CardId == cardId && a.AddressTypeId.ToUpper() == typeId.ToUpper()).FirstOrDefault(); 
        public AddressPM GetSingleAddressPM(string id, int tenant) => GetAddressPMQuery().Where(a => a.Tenant == tenant && a.Id == id).FirstOrDefault(); 
        private IQueryable<AddressPM> GetAddressPMQuery()
        {
            return (from a in context.Addresses.Include("Country").Include("State")
                    select new AddressPM()
                    {
                        Address1 = a.Address1,
                        Address2 = a.Address2,
                        AddressTypeId = a.AddressTypeId,
                        ATTN = a.ATTN,
                        CardId = a.CardId,
                        SearchFields = a.SearchFields,
                        City = a.City,
                        CountryId = a.CountryId,
                        Description = a.Description,
                        FaxNumber = a.FaxNumber,
                        Id = a.Id,
                        Name = a.Name,
                        PhoneNumber = a.PhoneNumber,
                        StateId = a.StateId,
                        Tenant = a.Tenant,
                        ZipCode = a.ZipCode,
                        InActive = a.InActive,
                        IsLocalLanguage = a.IsLocalLanguage,
                        CountryCode = a.Country != null ? a.Country.Code : null,
                        CountryEnglishName = a.Country != null ? a.Country.EnglishName : null,
                        CountryName = a.Country != null ? (a.IsLocalLanguage ? a.Country.LocalName : a.Country.EnglishName) : null,
                        StateCode = a.State != null ? a.State.Code : null,
                        StateEnglishName = a.State != null ? a.State.EnglishName : null,
                        HasStates = a.Country == null ? false : a.Country.HasStates,
                        IsStateRequired = a.Country == null ? false : a.Country.IsStateRequired,
                    });
        }
        #endregion Get Single AddressPM
        #region Get List<AddressList>
        private IQueryable<AddressList> GetAddressListQueryShort()
        {
            return (from a in context.Addresses
                    select new AddressList()
                    {
                        CardId = a.CardId,
                        ZipCode = a.ZipCode,
                        City = a.City,
                        CountryCode = a.Country != null ? a.Country.Code : null,
                        CountryName = a.Country != null ? a.IsLocalLanguage ? a.Country.LocalName : a.Country.EnglishName : null,
                        AddressTypeId = a.AddressTypeId,
                        Id = a.Id,
                        Address1 = a.Address1,
                        Address2 = a.Address2,
                        PhoneNumber = a.PhoneNumber,
                    });
        }
        public List<AddressList> GetAddressListsByCardIds(List<string> cardIds, string typeId, int tenant) => GetAddressListQueryShort().Where(a => a.Tenant == tenant && cardIds.Contains(a.CardId) && a.AddressTypeId.ToUpper() == typeId.ToUpper()).ToList();
        public List<AddressList> GetAddressesByCardIds(List<string> cardIds,  int tenant) => GetAddressListQueryShort().Where(a => a.Tenant == tenant && cardIds.Contains(a.CardId)).ToList();   
        public List<AddressPM> GetAddressesByCardId(string cardId, int tenant) => GetAddressPMQuery().Where(a => a.Tenant == tenant && a.CardId == cardId).ToList();
        public List<AddressList> GetAddressListsByIds(List<string> addressIds, int tenant) => GetAddressListQueryShort().Where(a => a.Tenant == tenant && addressIds.Contains(a.Id)).ToList();
        #endregion Get List<AddressList>
        #region Get Single AddressList
        private IQueryable<AddressList> GetAddressListQuery()
        {
            return (from a in context.Addresses.Include("Country").Include("State")
                         let list=new AddressList()
                         {
                             Address1 = a.Address1,
                             Address2 = a.Address2,
                             AddressTypeId = a.AddressTypeId,
                             ATTN = a.ATTN,
                             CardId = a.CardId,
                             SearchFields = a.SearchFields,
                             City = a.City,
                             Description = a.Description,
                             FaxNumber = a.FaxNumber,
                             Id = a.Id,
                             Name = a.Name,
                             PhoneNumber = a.PhoneNumber,
                             Tenant = a.Tenant,
                             ZipCode = a.ZipCode,
                             InActive = a.InActive,
                             IsLocalLanguage = a.IsLocalLanguage,
                             StateId = a.StateId,
                             StateCode = a.State != null ? a.State.Code : null,
                             StateName = a.State != null ? a.State.EnglishName : null,
                             CountryId = a.CountryId,
                             CountryCode = a.Country != null ? a.Country.Code : null,
                             CountryName = a.Country != null ? (a.IsLocalLanguage ? a.Country.LocalName : a.Country.EnglishName) : null,
                             CountryEC = a.Country == null ? false : a.Country.EC,
                         }
                         select list);
        }
        public AddressList GetAddressListByTypeAndCard(string cardId, string typeId, int tenant) => GetAddressListQuery().Where(a => a.Tenant == tenant && a.CardId == cardId && a.AddressTypeId.ToUpper() == typeId.ToUpper()).FirstOrDefault();   
        public AddressList GetSingleAddressList(string Id, int tenant) => GetAddressListQuery().Where(a => a.Tenant == tenant && a.Id == Id).FirstOrDefault();  
        #endregion
        public IQueryable<AddressPM> GetAddressePMsByTenant(int tenant)
        {
            IQueryable<AddressPM> addresses = from a in context.Addresses.Include("Country").Include("State")
                                              where a.Tenant == tenant
                                              select new AddressPM()
                                              {
                                                  Address1 = a.Address1,
                                                  Address2 = a.Address2,
                                                  AddressTypeId = a.AddressTypeId,
                                                  ATTN = a.ATTN,
                                                  CardId = a.CardId,
                                                  City = a.City,
                                                  CountryId = a.CountryId,
                                                  Description = a.Description,
                                                  FaxNumber = a.FaxNumber,
                                                  Id = a.Id,
                                                  Name = a.Name,
                                                  PhoneNumber = a.PhoneNumber,
                                                  StateId = a.StateId,
                                                  Tenant = a.Tenant,
                                                  ZipCode = a.ZipCode,
                                                  InActive = a.InActive,
                                                  IsLocalLanguage = a.IsLocalLanguage,
                                                  SearchFields = a.SearchFields,
                                                  CountryCode = a.Country != null ? a.Country.Code : null,
                                                  CountryEnglishName = a.Country != null ? a.Country.EnglishName : null,
                                                  CountryName = a.Country != null ? (a.IsLocalLanguage ? a.Country.LocalName : a.Country.EnglishName) : null,
                                                  StateCode = a.State != null ? a.State.Code : null,
                                                  StateEnglishName = a.State != null ? a.State.EnglishName : null,
                                                  HasStates = a.Country == null ? false : a.Country.HasStates,
                                                  IsStateRequired = a.Country == null ? false : a.Country.IsStateRequired,
                                              };
            return addresses;
        }
        public IQueryable<AddressList> GetIQueryableEntityList(IQueryable<Address> iQueryable)
        {
            IQueryable<AddressList> result = from entity in iQueryable.Include("Country").Include("State")
                                             select new AddressList()
                                             {
                                                 Id = entity.Id,
                                                 Tenant = entity.Tenant,
                                                 Address1 = entity.Address1,
                                                 Address2 = entity.Address2,
                                                 City = entity.City,
                                                 Description = entity.Description,
                                                 FaxNumber = entity.FaxNumber,
                                                 Name = entity.Name,
                                                 PhoneNumber = entity.PhoneNumber,
                                                 ZipCode = entity.ZipCode,
                                                 AddressTypeId = entity.AddressTypeId,
                                                 CountryId = entity.CountryId,
                                                 CardId = entity.CardId,
                                                 InActive = entity.InActive,
                                                 SearchFields = entity.SearchFields,
                                                 StateId = entity.StateId,
                                                 ATTN = entity.ATTN,
                                                 CountryCode = entity.Country != null ? entity.Country.Code : null,
                                                 CountryName = entity.Country != null ? (entity.IsLocalLanguage ? entity.Country.LocalName : entity.Country.EnglishName) : null,
                                                 StateCode = entity.State != null ? entity.State.Code : null,
                                                 StateName = entity.State != null ? entity.State.Code : null,
                                                 CountryEC = entity.Country == null ? false : entity.Country.EC,
                                             };
            return result;
        }
        public string GetZiPCodeByCard(string cardId, string typeId, int tenant)
        {
            string zipcode = (from a in context.Addresses
                              where a.Tenant == tenant
                              && a.CardId == cardId
                              && a.AddressTypeId.ToUpper() == typeId.ToUpper()
                              select a.ZipCode).FirstOrDefault();

            return zipcode;
        }
    }
}