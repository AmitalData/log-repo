using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System.Text.RegularExpressions;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class AddressQuery
    {
        AddressRepository repository;

        public AddressQuery()
        {
            repository = new AddressRepository();
        }

        public AddressQuery(int tenant)
        {
            repository = new AddressRepository(tenant);
        }

        public AddressQuery(AddressRepository addressRepository)
        {
            repository = addressRepository;
        }

        public AddressPM GetSinglePM(string id, int tenant)
        {
            AddressPM entityPM = (from a in repository.context.Addresses.Include("Country").Include("State").Include("Card")
                                  where a.Tenant == tenant && a.Id == id
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
                                      CardCode = a.Card == null ? null : a.Card.Code,
                                      CardEnglishName = a.Card == null ? null : a.Card.EnglishName,
                                      VatNumber = a.Card == null ? null : a.Card.VatNumber,
                                      HasStates = a.Country == null ? false : a.Country.HasStates,
                                      IsStateRequired = a.Country == null ? false : a.Country.IsStateRequired,
                                      ExternalId = a.ExternalId
                                  }).FirstOrDefault();

            return entityPM;
        }

        public AddressPM GetSinglePMByExternalId(string exteranlId, int tenant)
        {
            return (from a in repository.context.Addresses.Include("Country").Include("State")
                    where a.Tenant == tenant
                    && a.Id == exteranlId
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
                    }).FirstOrDefault();

        }
        public AddressPM GetAddressByExternalId(string exteranlId, int tenant)
        {
            return (from a in repository.context.Addresses.Include("Country").Include("State")
                    where a.Tenant == tenant
                    && a.ExternalId == exteranlId
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
                    }).FirstOrDefault();

        }

        public AddressPM GetAddressByCardId(string cardId, int tenant)
        {
            return (from a in repository.context.Addresses.Include("Country").Include("State")
                    where a.Tenant == tenant
                    && a.CardId == cardId && (a.AddressTypeId == "B" || a.AddressTypeId == "M" || a.AddressTypeId == "O")
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
                    }).FirstOrDefault();

        }

        public AddressPM GetSingleAddressPM(string id, int tenant)
        {
            AddressPM instance = null;

            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "AddressPM" + id + tenant;

                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        ICommonDataContext context = CommonDataContext.GetContext(tenant);

                        instance = (from a in context.Addresses.Include("Country").Include("State")
                                    where a.Tenant == tenant && a.Id == id
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
                                    }).FirstOrDefault();

                        if (instance != null)
                        {
                            string name = "AddressPM" + instance.Id + tenant;

                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, instance, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                    }
                    else
                    {
                        instance = (AddressPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    ICommonDataContext context = CommonDataContext.GetContext(tenant);
                    instance = (from a in context.Addresses.Include("Country").Include("State")
                                where a.Tenant == tenant
                                && a.Id == id
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
                                }).FirstOrDefault();
                }
            }
            return instance;
        }
        public AddressPM GetAddressPMByTypeAndCard(string cardId, string typeId, int tenant)
        {
            AddressPM entityPM = null;

            Address entityPOCO = (from a in repository.context.Addresses.Include("Country").Include("State")
                                  where a.Tenant == tenant
                                  && a.CardId == cardId
                                  && a.AddressTypeId.ToUpper() == typeId.ToUpper()
                                  select a).FirstOrDefault();

            if (entityPOCO != null)
            {
                entityPM = new AddressPM()
                {
                    Address1 = entityPOCO.Address1,
                    Address2 = entityPOCO.Address2,
                    AddressTypeId = entityPOCO.AddressTypeId,
                    ATTN = entityPOCO.ATTN,
                    CardId = entityPOCO.CardId,
                    SearchFields = entityPOCO.SearchFields,
                    City = entityPOCO.City,
                    CountryId = entityPOCO.CountryId,
                    Description = entityPOCO.Description,
                    FaxNumber = entityPOCO.FaxNumber,
                    Id = entityPOCO.Id,
                    Name = entityPOCO.Name,
                    PhoneNumber = entityPOCO.PhoneNumber,
                    StateId = entityPOCO.StateId,
                    Tenant = entityPOCO.Tenant,
                    ZipCode = entityPOCO.ZipCode,
                    InActive = entityPOCO.InActive,
                    IsLocalLanguage = entityPOCO.IsLocalLanguage,
                    CountryCode = entityPOCO.Country != null ? entityPOCO.Country.Code : null,
                    CountryEnglishName = entityPOCO.Country != null ? entityPOCO.Country.EnglishName : null,
                    CountryName = entityPOCO.Country != null ? (entityPOCO.IsLocalLanguage ? entityPOCO.Country.LocalName : entityPOCO.Country.EnglishName) : null,
                    StateCode = entityPOCO.State != null ? entityPOCO.State.Code : null,
                    StateEnglishName = entityPOCO.State != null ? entityPOCO.State.EnglishName : null,
                    HasStates = entityPOCO.Country == null ? false : entityPOCO.Country.HasStates,
                    IsStateRequired = entityPOCO.Country == null ? false : entityPOCO.Country.IsStateRequired,
                };
            }

            return entityPM;
        }
        public string GetZiPCodeByCard(string cardId, string typeId, int tenant)
        {
            string zipcode = (from a in repository.context.Addresses
                              where a.Tenant == tenant
                              && a.CardId == cardId
                              && a.AddressTypeId.ToUpper() == typeId.ToUpper()
                              select a.ZipCode).FirstOrDefault();

            return zipcode;
        }



        public List<AddressList> GetAddressListsByCardIds(List<string> cardIds, string typeId, int tenant)
        {
            List<AddressList> addressLists = (from a in repository.context.Addresses
                                              where a.Tenant == tenant
                                              && cardIds.Contains(a.CardId)
                                              && a.AddressTypeId.ToUpper() == typeId.ToUpper()
                                              select new AddressList()
                                              {
                                                  CardId = a.CardId,
                                                  ZipCode = a.ZipCode,

                                              }).ToList();

            return addressLists;
        }

        public List<AddressList> GetAddressesByCardIds(List<string> cardIds,  int tenant)
        {
            List<AddressList> addressLists = (from a in repository.context.Addresses
                                              where a.Tenant == tenant
                                              && cardIds.Contains(a.CardId)
                                            
                                              select new AddressList()
                                              {
                                                  CardId = a.CardId,
                                                  ZipCode = a.ZipCode,
                                                  City = a.City,
                                                  CountryCode = a.Country != null ? a.Country.Code : null,
                                                  CountryName = a.Country != null ? a.IsLocalLanguage? a.Country.LocalName : a.Country.EnglishName  : null,
                                                  AddressTypeId = a.AddressTypeId,
                                                  Id = a.Id,
                                                  Address1 = a.Address1,
                                                  Address2= a.Address2,
                                                  PhoneNumber = a.PhoneNumber,
                                              }).ToList();

            return addressLists;
        }


        public AddressList GetAddressListByTypeAndCard(string cardId, string typeId, int tenant)
        {
            AddressList entityList = null;

            Address entityPOCO = (from a in repository.context.Addresses.Include("Country").Include("State")
                                  where a.Tenant == tenant
                                    && a.CardId == cardId
                                  && a.AddressTypeId.ToUpper() == typeId.ToUpper()
                                  &&!a.InActive
                                  select a).FirstOrDefault();

            if (entityPOCO != null)
            {
                entityList = new AddressList()
                {
                    Address1 = entityPOCO.Address1,
                    Address2 = entityPOCO.Address2,
                    AddressTypeId = entityPOCO.AddressTypeId,
                    ATTN = entityPOCO.ATTN,
                    CardId = entityPOCO.CardId,
                    SearchFields = entityPOCO.SearchFields,
                    City = entityPOCO.City,
                    Description = entityPOCO.Description,
                    FaxNumber = entityPOCO.FaxNumber,
                    Id = entityPOCO.Id,
                    Name = entityPOCO.Name,
                    PhoneNumber = entityPOCO.PhoneNumber,
                    Tenant = entityPOCO.Tenant,
                    ZipCode = entityPOCO.ZipCode,
                    InActive = entityPOCO.InActive,
                    IsLocalLanguage = entityPOCO.IsLocalLanguage,
                    StateId = entityPOCO.StateId,
                    StateCode = entityPOCO.State != null ? entityPOCO.State.Code : null,
                    StateName = entityPOCO.State != null ? entityPOCO.State.EnglishName : null,
                    CountryId = entityPOCO.CountryId,
                    CountryCode = entityPOCO.Country != null ? entityPOCO.Country.Code : null,
                    CountryName = entityPOCO.Country != null ? (entityPOCO.IsLocalLanguage ? entityPOCO.Country.LocalName : entityPOCO.Country.EnglishName) : null,
                    CountryEC = entityPOCO.Country == null ? false : entityPOCO.Country.EC,
                };
            }

            return entityList;
        }
        public AddressList GetSingleAddressList(string Id, int tenant)
        {
            AddressList entityList = null;
            Address entityPOCO = (from a in repository.context.Addresses.Include("Country").Include("State")
                                  where a.Tenant == tenant && a.Id == Id
                                  select a).FirstOrDefault();

            if (entityPOCO != null)
            {
                entityList = new AddressList()
                {
                    Address1 = entityPOCO.Address1,
                    Address2 = entityPOCO.Address2,
                    AddressTypeId = entityPOCO.AddressTypeId,
                    ATTN = entityPOCO.ATTN,
                    CardId = entityPOCO.CardId,
                    SearchFields = entityPOCO.SearchFields,
                    City = entityPOCO.City,
                    Description = entityPOCO.Description,
                    FaxNumber = entityPOCO.FaxNumber,
                    Id = entityPOCO.Id,
                    Name = entityPOCO.Name,
                    PhoneNumber = entityPOCO.PhoneNumber,
                    Tenant = entityPOCO.Tenant,
                    ZipCode = entityPOCO.ZipCode,
                    InActive = entityPOCO.InActive,
                    IsLocalLanguage = entityPOCO.IsLocalLanguage,
                    StateId = entityPOCO.StateId,
                    StateCode = entityPOCO.State != null ? entityPOCO.State.Code : null,
                    StateName = entityPOCO.State != null ? entityPOCO.State.EnglishName : null,
                    CountryId = entityPOCO.CountryId,
                    CountryCode = entityPOCO.Country != null ? entityPOCO.Country.Code : null,
                    CountryName = entityPOCO.Country != null ? (entityPOCO.IsLocalLanguage ? entityPOCO.Country.LocalName : entityPOCO.Country.EnglishName) : null,
                    CountryEC = entityPOCO.Country == null ? false : entityPOCO.Country.EC,
                };
            }
            return entityList;
        }

        public List<AddressPM> GetAddressesByCardId(string cardId, int tenant)
        {
            var addresses = (from a in repository.context.Addresses.Include("Country").Include("State")
                             where a.CardId == cardId && a.Tenant == tenant
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
                             }).ToList();

            return addresses;
        }

        public IQueryable<AddressPM> GetAddressePMsByTenant(int tenant)
        {
            IQueryable<AddressPM> addresses = from a in repository.context.Addresses.Include("Country").Include("State")
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

        public AddressPM GetSingleAddressPM(string id, int tenant, bool fromcache)
        {
            AddressPM instance = null;
            if (!string.IsNullOrEmpty(id))
            {
                if (fromcache)
                {
                    string entityName = "AddressPM" + id + tenant;

                    if (HttpContext.Current != null)
                    {
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            ICommonDataContext context = CommonDataContext.GetContext(tenant);
                            instance = (from a in context.Addresses.Include("Country").Include("State")
                                        where a.Tenant == tenant && a.Id == id
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
                                        }).FirstOrDefault();
                            if (instance != null)
                            {
                                string name = "AddressPM" + instance.Id + tenant;

                                if (CacheManager.CacheWrapper.Get(name) == null)
                                {
                                    CacheManager.CacheWrapper.Insert(name, instance, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                                }

                                instance = (AddressPM)CacheManager.CacheWrapper.Get(entityName);
                            }
                        }
                        else
                        {
                            instance = (AddressPM)CacheManager.CacheWrapper.Get(entityName);
                        }
                    }
                }
                else
                {
                    ICommonDataContext context = CommonDataContext.GetContext(tenant);
                    instance = (from a in context.Addresses.Include("Country").Include("State")
                                where a.Tenant == tenant
                                && a.Id == id
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
                                }).FirstOrDefault();
                }
            }
            return instance;
        }

        public List<AddressList> GetAddressListsByIds(List<string> addressIds, int tenant)
        {
            List<AddressList> addressLists = (from a in repository.context.Addresses
                                              where a.Tenant == tenant
                                              && addressIds.Contains(a.Id)
                                              select new AddressList()
                                              {
                                                  Id = a.Id,
                                                  ZipCode = a.ZipCode,
                                                  City = a.City,

                                              }).ToList();

            return addressLists;
        }

    }

}