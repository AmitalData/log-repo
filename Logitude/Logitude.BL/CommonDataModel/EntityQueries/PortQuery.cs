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
using Logitude.BL.CommonDataModel.CustomFilters;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Counters;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class PortQuery
    {
        PortRepository repository;

        public PortQuery()
        {
            repository = new PortRepository();
        }

        public PortQuery(int tenant)
        {
            repository = new PortRepository(tenant);
        }

        public PortQuery(PortRepository repository)
        {
            this.repository = repository;
        }

        public PortPM GetSinglePM(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "PortPM" + id + tenant;
                PortPM entity;

                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        entity = (from a in repository.context.Ports.Include("Country")
                                  where a.Tenant == tenant && a.Id == id
                                  select new PortPM()
                                  {
                                      AddedManually = a.AddedManually,
                                      Code = a.Code,
                                      CountryId = a.CountryId,
                                      EnglishName = a.EnglishName,
                                      Field1 = a.Field1,
                                      Field2 = a.Field2,
                                      Field3 = a.Field3,
                                      Field4 = a.Field4,
                                      Field5 = a.Field5,
                                      Field6 = a.Field6,
                                      Field7 = a.Field7,
                                      Field8 = a.Field8,
                                      Field9 = a.Field9,
                                      Field10 = a.Field10,
                                      Id = a.Id,
                                      InActive = a.InActive,
                                      IsAir = a.IsAir,
                                      IsInland = a.IsInland,
                                      IsOcean = a.IsOcean,
                                      Latitude = a.Latitude,
                                      LocalName = a.LocalName,
                                      Longtitude = a.Longtitude,
                                      Notes = a.Notes,
                                      Tenant = a.Tenant,
                                      CountryName = a.CountryName,
                                      CountryCode = a.CountryCode,
                                      ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                      SearchFields = a.SearchFields,
                                      CountryEC = a.Country.EC,
                                      StateId = a.StateId,
                                      CombinedCode = a.CombinedCode,
                                      StateCode = a.StateCode,
                                      StateName = a.StateName,
                                      PortTimeZoneCode = a.PortTimeZoneCode,
                                  }).FirstOrDefault();

                        if (entity != null)
                        {
                            string name = "PortPM" + entity.Id + tenant;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, entity, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                    }
                    else
                    {
                        entity = (PortPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.Ports.Include("Country")
                              where a.Id == id && a.Tenant == tenant
                              select new PortPM()
                              {
                                  AddedManually = a.AddedManually,
                                  Code = a.Code,
                                  CountryId = a.CountryId,
                                  EnglishName = a.EnglishName,
                                  Field1 = a.Field1,
                                  Field2 = a.Field2,
                                  Field3 = a.Field3,
                                  Field4 = a.Field4,
                                  Field5 = a.Field5,
                                  Field6 = a.Field6,
                                  Field7 = a.Field7,
                                  Field8 = a.Field8,
                                  Field9 = a.Field9,
                                  Field10 = a.Field10,
                                  Id = a.Id,
                                  InActive = a.InActive,
                                  IsAir = a.IsAir,
                                  IsInland = a.IsInland,
                                  IsOcean = a.IsOcean,
                                  Latitude = a.Latitude,
                                  LocalName = a.LocalName,
                                  Longtitude = a.Longtitude,
                                  Notes = a.Notes,
                                  Tenant = a.Tenant,
                                  CountryName = a.CountryName,
                                  CountryCode = a.CountryCode,
                                  ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                  SearchFields = a.SearchFields,
                                  StateId = a.StateId,
                                  CombinedCode = a.CombinedCode,
                                  CountryEC = a.Country.EC,
                                  StateName = a.StateName,
                                  StateCode = a.StateCode,
                                  PortTimeZoneCode = a.PortTimeZoneCode,
                              }).FirstOrDefault();
                }

                PortPM securedPm = new PortPM();
                SecuredMapping.GetMappedPM(entity, securedPm, "Port", tenant);

                return securedPm;
            }
            return null;
        }

        public static PortPM GetSinglePort(int tenant, string id, bool getFromCache)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "PortPM" + id + tenant;
                PortPM entity;

                if (getFromCache)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        PortRepository myRepository = new PortRepository(tenant);

                        entity = (from a in myRepository.context.Ports.Include("Country")
                                  where a.Tenant == tenant && a.Id == id
                                  select new PortPM()
                                  {
                                      AddedManually = a.AddedManually,
                                      Code = a.Code,
                                      CountryId = a.CountryId,
                                      EnglishName = a.EnglishName,
                                      Field1 = a.Field1,
                                      Field2 = a.Field2,
                                      Field3 = a.Field3,
                                      Field4 = a.Field4,
                                      Field5 = a.Field5,
                                      Field6 = a.Field6,
                                      Field7 = a.Field7,
                                      Field8 = a.Field8,
                                      Field9 = a.Field9,
                                      Field10 = a.Field10,
                                      Id = a.Id,
                                      InActive = a.InActive,
                                      IsAir = a.IsAir,
                                      IsInland = a.IsInland,
                                      IsOcean = a.IsOcean,
                                      Latitude = a.Latitude,
                                      LocalName = a.LocalName,
                                      Longtitude = a.Longtitude,
                                      Notes = a.Notes,
                                      Tenant = a.Tenant,
                                      CountryName = a.CountryName,
                                      CountryCode = a.CountryCode,
                                      ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                      SearchFields = a.SearchFields,
                                      CountryEC = a.Country.EC,
                                      StateId = a.StateId,
                                      StateCode = a.StateCode,
                                      CombinedCode = a.CombinedCode,
                                      StateName = a.StateName,
                                      CountryIsNorthAmerica = a.Country.IsNorthAmerica,
                                      CountryIsGreaterChinese = a.Country.IsGreaterChina,
                                      PortTimeZoneCode = a.PortTimeZoneCode,
                                  }).FirstOrDefault();

                        if (CacheManager.CacheWrapper.Get(entityName) == null && entity != null)
                        {
                            CacheManager.CacheWrapper.Insert(entityName, entity, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }

                    else
                    {
                        entity = (PortPM)CacheManager.CacheWrapper.Get(entityName);
                    }


                }

                else
                {
                    PortRepository myRepository = new PortRepository(tenant);

                    entity = (from a in myRepository.context.Ports.Include("Country")
                              where a.Tenant == tenant && a.Id == id
                              select new PortPM()
                              {
                                  AddedManually = a.AddedManually,
                                  Code = a.Code,
                                  CountryId = a.CountryId,
                                  EnglishName = a.EnglishName,
                                  Field1 = a.Field1,
                                  Field2 = a.Field2,
                                  Field3 = a.Field3,
                                  Field4 = a.Field4,
                                  Field5 = a.Field5,
                                  Field6 = a.Field6,
                                  Field7 = a.Field7,
                                  Field8 = a.Field8,
                                  Field9 = a.Field9,
                                  Field10 = a.Field10,
                                  Id = a.Id,
                                  InActive = a.InActive,
                                  IsAir = a.IsAir,
                                  IsInland = a.IsInland,
                                  IsOcean = a.IsOcean,
                                  Latitude = a.Latitude,
                                  LocalName = a.LocalName,
                                  Longtitude = a.Longtitude,
                                  Notes = a.Notes,
                                  Tenant = a.Tenant,
                                  CountryName = a.CountryName,
                                  CountryCode = a.CountryCode,
                                  ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                  SearchFields = a.SearchFields,
                                  CountryEC = a.Country.EC,
                                  StateId = a.StateId,
                                  StateCode = a.StateCode,
                                  CombinedCode = a.CombinedCode,
                                  StateName = a.StateName,
                                  CountryIsNorthAmerica = a.Country.IsNorthAmerica,
                                  CountryIsGreaterChinese = a.Country.IsGreaterChina,
                                  PortTimeZoneCode = a.PortTimeZoneCode,
                              }).FirstOrDefault();
                }

                return entity;
            }

            return null;
        }

        public IQueryable<PortPM> GetPortPMsByTenant(int tenant)
        {
            IQueryable<PortPM> ports = (from a in repository.context.Ports.Include("Country")
                                        where a.Tenant == tenant
                                        select new PortPM()
                                        {
                                            AddedManually = a.AddedManually,
                                            Code = a.Code,
                                            CountryId = a.CountryId,
                                            EnglishName = a.EnglishName,
                                            Field1 = a.Field1,
                                            Field2 = a.Field2,
                                            Field3 = a.Field3,
                                            Field4 = a.Field4,
                                            Field5 = a.Field5,
                                            Field6 = a.Field6,
                                            Field7 = a.Field7,
                                            Field8 = a.Field8,
                                            Field9 = a.Field9,
                                            Field10 = a.Field10,
                                            Id = a.Id,
                                            InActive = a.InActive,
                                            IsAir = a.IsAir,
                                            IsInland = a.IsInland,
                                            IsOcean = a.IsOcean,
                                            Latitude = a.Latitude,
                                            LocalName = a.LocalName,
                                            Longtitude = a.Longtitude,
                                            Notes = a.Notes,
                                            Tenant = a.Tenant,
                                            CountryName = a.CountryName,
                                            CountryCode = a.CountryCode,
                                            SearchFields = a.SearchFields,
                                            CountryEC = a.Country.EC,
                                            StateId = a.StateId,
                                            CombinedCode = a.CombinedCode,
                                            StateName = a.StateName,
                                            StateCode = a.StateCode,
                                            PortTimeZoneCode = a.PortTimeZoneCode,
                                        });
            return ports;
        }


        public IQueryable<Port> GetAllPorts()
        {
            IQueryable<Port> ports = (from a in repository.context.Ports select a);

            return ports;
        }


        public IQueryable<PortPM> GetPortPMsByTenantAndCountry(int tenant, string country)
        {
            IQueryable<PortPM> ports = (from a in repository.context.Ports.Include("Country")
                                        where a.Tenant == tenant && a.Country.EnglishName == country
                                        select new PortPM()
                                        {
                                            AddedManually = a.AddedManually,
                                            Code = a.Code,
                                            CountryId = a.CountryId,
                                            EnglishName = a.EnglishName,
                                            Field1 = a.Field1,
                                            Field2 = a.Field2,
                                            Field3 = a.Field3,
                                            Field4 = a.Field4,
                                            Field5 = a.Field5,
                                            Field6 = a.Field6,
                                            Field7 = a.Field7,
                                            Field8 = a.Field8,
                                            Field9 = a.Field9,
                                            Field10 = a.Field10,
                                            Id = a.Id,
                                            InActive = a.InActive,
                                            IsAir = a.IsAir,
                                            IsInland = a.IsInland,
                                            IsOcean = a.IsOcean,
                                            Latitude = a.Latitude,
                                            LocalName = a.LocalName,
                                            Longtitude = a.Longtitude,
                                            Notes = a.Notes,
                                            Tenant = a.Tenant,
                                            CountryName = a.CountryName,
                                            SearchFields = a.SearchFields,
                                            CountryCode = a.CountryCode,
                                            CountryEC = a.Country.EC,
                                            StateId = a.StateId,
                                            CombinedCode = a.CombinedCode,
                                            StateName = a.StateName,
                                            StateCode = a.StateCode,
                                            PortTimeZoneCode = a.PortTimeZoneCode,
                                        });
            return ports;
        }

        public IQueryable<PortPM> GetSinglePortPMByCode(string input, bool byCode, int tenant)
        {
            if (byCode)
            {
                IQueryable<PortPM> ports = (from a in repository.context.Ports.Include("Country")
                                            where a.Tenant == tenant && a.Code.ToUpper() == input.ToUpper().Trim()
                                            select new PortPM()
                                            {
                                                AddedManually = a.AddedManually,
                                                Code = a.Code,
                                                CountryId = a.CountryId,
                                                EnglishName = a.EnglishName,
                                                Field1 = a.Field1,
                                                Field2 = a.Field2,
                                                Field3 = a.Field3,
                                                Field4 = a.Field4,
                                                Field5 = a.Field5,
                                                Field6 = a.Field6,
                                                Field7 = a.Field7,
                                                Field8 = a.Field8,
                                                Field9 = a.Field9,
                                                Field10 = a.Field10,
                                                Id = a.Id,
                                                InActive = a.InActive,
                                                IsAir = a.IsAir,
                                                IsInland = a.IsInland,
                                                IsOcean = a.IsOcean,
                                                Latitude = a.Latitude,
                                                LocalName = a.LocalName,
                                                Longtitude = a.Longtitude,
                                                Notes = a.Notes,
                                                Tenant = a.Tenant,
                                                CountryName = a.CountryName,
                                                SearchFields = a.SearchFields,
                                                CountryCode = a.CountryCode,
                                                CountryEC = a.Country.EC,
                                                StateId = a.StateId,
                                                CombinedCode = a.CombinedCode,
                                                StateName = a.StateName,
                                                StateCode = a.StateCode,
                                                PortTimeZoneCode = a.PortTimeZoneCode,
                                            });
                return ports;
            }
            else
            {
                IQueryable<PortPM> ports = (from a in repository.context.Ports.Include("Country")
                                            where a.Tenant == tenant && a.EnglishName.ToUpper() == input.ToUpper().Trim()
                                            select new PortPM()
                                            {
                                                AddedManually = a.AddedManually,
                                                Code = a.Code,
                                                CountryId = a.CountryId,
                                                EnglishName = a.EnglishName,
                                                Field1 = a.Field1,
                                                Field2 = a.Field2,
                                                Field3 = a.Field3,
                                                Field4 = a.Field4,
                                                Field5 = a.Field5,
                                                Field6 = a.Field6,
                                                Field7 = a.Field7,
                                                Field8 = a.Field8,
                                                Field9 = a.Field9,
                                                Field10 = a.Field10,
                                                Id = a.Id,
                                                InActive = a.InActive,
                                                IsAir = a.IsAir,
                                                IsInland = a.IsInland,
                                                IsOcean = a.IsOcean,
                                                Latitude = a.Latitude,
                                                LocalName = a.LocalName,
                                                Longtitude = a.Longtitude,
                                                Notes = a.Notes,
                                                Tenant = a.Tenant,
                                                CountryName = a.CountryName,
                                                SearchFields = a.SearchFields,
                                                CountryCode = a.CountryCode,
                                                CountryEC = a.Country.EC,
                                                StateId = a.StateId,
                                                CombinedCode = a.CombinedCode,
                                                StateName = a.StateName,
                                                StateCode = a.StateCode,
                                                PortTimeZoneCode = a.PortTimeZoneCode,
                                            });
                return ports;
            }
        }

        public IQueryable<PortPM> GetPortPMsByNameOrCode(string code, string name, int tenant)
        {
            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = (from a in repository.context.Ports.Include("Country")
                         where a.Tenant == tenant
                         select new PortPM()
                         {
                             AddedManually = a.AddedManually,
                             Code = a.Code,
                             CountryId = a.CountryId,
                             EnglishName = a.EnglishName,
                             Field1 = a.Field1,
                             Field2 = a.Field2,
                             Field3 = a.Field3,
                             Field4 = a.Field4,
                             Field5 = a.Field5,
                             Field6 = a.Field6,
                             Field7 = a.Field7,
                             Field8 = a.Field8,
                             Field9 = a.Field9,
                             Field10 = a.Field10,
                             Id = a.Id,
                             InActive = a.InActive,
                             IsAir = a.IsAir,
                             IsInland = a.IsInland,
                             IsOcean = a.IsOcean,
                             Latitude = a.Latitude,
                             LocalName = a.LocalName,
                             Longtitude = a.Longtitude,
                             Notes = a.Notes,
                             Tenant = a.Tenant,
                             CountryName = a.CountryName,
                             SearchFields = a.SearchFields,
                             CountryCode = a.CountryCode,
                             CountryEC = a.Country.EC,
                             StateId = a.StateId,
                             CombinedCode = a.CombinedCode,
                             StateName = a.StateName,
                             StateCode = a.StateCode,
                             PortTimeZoneCode = a.PortTimeZoneCode,
                         }).AsQueryable();

            IQueryable<PortPM> query2 = null;
            if (!string.IsNullOrEmpty(code))
            {
                query2 = query.Where(d => d.Code.ToUpper().StartsWith(code.ToUpper()));
            }
            if (!string.IsNullOrEmpty(name))
            {
                if (query2 != null)
                {
                    if (query2.Count() == 0)
                    {
                        query2 = query.Where(d => d.EnglishName.ToUpper().StartsWith(name.ToUpper()));
                    }
                }
                else
                {
                    query2 = query.Where(d => d.EnglishName.ToUpper().StartsWith(name.ToUpper()));
                }
            }
            if (query2 != null)
            {
                return query2;
            }
            else
                return query;
        }

        public IQueryable<PortList> GetIQueryableEntityList(IQueryable<Port> iQueryable)
        {
            IQueryable<PortList> result = from f in iQueryable.Include("Country")
                                          select new PortList()
                                          {
                                              Code = f.Code,
                                              EnglishName = f.EnglishName,
                                              Id = f.Id,
                                              Tenant = f.Tenant,
                                              IsAir = f.IsAir,
                                              IsInland = f.IsInland,
                                              IsOcean = f.IsOcean,
                                              Notes = f.Notes,
                                              InActive = f.InActive,
                                              CountryId = f.CountryId,
                                              CountryCode = f.CountryCode,
                                              CountryName = f.CountryName,
                                              CountryEC = f.Country.EC,
                                              AddedManually = f.AddedManually,
                                              SearchFields = f.SearchFields,
                                              TransportModeId = (f.IsAir ? "A" : "") + (f.IsInland ? "I" : "") + (f.IsOcean ? "O" : ""),
                                              StateId = f.StateId,
                                              CombinedCode = f.CombinedCode,
                                              StateName = f.StateName,
                                              StateCode = f.StateCode,
                                          };
            return result;
        }

        public List<PortList> GetPortFilters(byte[] xmlFilters, int tenant)
        {
            IQueryable<Port> iQueryable = repository.GetPorts(tenant);
            PortCustomFilter customfilters = new PortCustomFilter(tenant);

            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            QueryFilterItem item = queryOperations.QueryFilterItems.Where(f => f.FieldName == "CompactSearchField").FirstOrDefault();
            if (item != null)
            {
                queryOperations.QueryFilterItems.Remove(item);
            }

            iQueryable = EntityListFilter.ApplyEntityNonListFilters(queryOperations, iQueryable);
            iQueryable = customfilters.GetFilteredQuery(queryOperations, iQueryable);

            IQueryable<PortList> query2 = GetIQueryableEntityList(iQueryable);

            query2 = EntityListFilter.ApplyEntityListFilters(queryOperations, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                query2 = QuerySortClass.GetSortedQuery(queryOperations, query2, "Port", tenant);
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            int skippedInvoices = queryOperations.PageIndex;
            query2 = query2.Skip(skippedInvoices);
            query2 = query2.Take(queryOperations.PageSize);
            return query2.ToList();
        }

        public int GetPortFiltersCount(byte[] xmlFilters, int tenant)
        {
            IQueryable<Port> iQueryable = from record in repository.context.Ports where record.Tenant == tenant select record;
            PortCustomFilter customfilters = new PortCustomFilter(tenant);

            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);


            QueryFilterItem item = queryOperations.QueryFilterItems.Where(f => f.FieldName == "CompactSearchField").FirstOrDefault();
            if (item != null)
            {
                queryOperations.QueryFilterItems.Remove(item);
            }

            iQueryable = EntityListFilter.ApplyEntityNonListFilters(queryOperations, iQueryable);
            iQueryable = customfilters.GetFilteredQuery(queryOperations, iQueryable);

            IQueryable<PortList> query2 = GetIQueryableEntityList(iQueryable);

            query2 = EntityListFilter.ApplyEntityListFilters(queryOperations, query2);

            return query2.Count();
        }

        public PortList GetPortCopyToCurrentTenant(string zeroPortId, int tenant)
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);

            PortRepository portRepository = new PortRepository(objectContext);
            CountryRepository countryRepository = new CountryRepository(objectContext);
            GlobalZoneRepository globalZoneRepository = new GlobalZoneRepository(objectContext);

            Port newPort;
            Port port = portRepository.GetSinglePort(0, zeroPortId);
            newPort = portRepository.GetSinglePortByCodeCountryCode(tenant, port.Code, port.Country.Code, false);
            Country country = null;

            if (newPort == null)
            {
                country = countryRepository.GetSingleCountryByCode(port.Country.Code, tenant, true);

                if (country == null)
                {
                    GlobalZone oldZone = globalZoneRepository.GetSingleGlobalZone(port.Country.GlobalZoneId, 0);

                    GlobalZone globalzone = globalZoneRepository.GetSingleGlobalZoneByCode(oldZone.Code, tenant);

                    if (globalzone == null)
                    {
                        globalzone = new GlobalZone()
                        {
                            Id = IdCounter.GetNumber("GlobalZone", tenant).ToString(),
                            Code = oldZone.Code,
                            EnglishName = oldZone.EnglishName,
                            LocalName = oldZone.LocalName,
                            Notes = oldZone.Notes,
                            SearchFields = oldZone.SearchFields,
                            Tenant = tenant,
                        };

                        globalZoneRepository.Add(globalzone);
                        globalZoneRepository.SubmitChanges();
                    }

                    Country oldCountry = CountryRepository.GetSingleCountry(port.CountryId, 0, false);
                    country = new Country()
                    {
                        Id = IdCounter.GetNumber("Country", tenant).ToString(),
                        Tenant = tenant,
                        GlobalZoneId = oldCountry.GlobalZoneId,
                        EC = oldCountry.EC,
                        EnglishName = oldCountry.EnglishName,
                        Code = oldCountry.Code,
                        InActive = oldCountry.InActive,
                        Notes = oldCountry.Notes,
                        LocalName = oldCountry.LocalName,
                        SearchFields = oldCountry.SearchFields,
                    };

                    countryRepository.Add(country);
                    countryRepository.SubmitChanges();
                }
                newPort = new Port()
                {
                    Id = IdCounter.GetNumber("Port", tenant).ToString(),
                    Code = port.Code,
                    EnglishName = port.EnglishName,
                    LocalName = port.LocalName,
                    Tenant = tenant,
                    AddedManually = false,
                    InActive = false,
                    CountryId = country.Id,
                    IsAir = port.IsAir,
                    IsInland = port.IsInland,
                    IsOcean = port.IsOcean,
                    Latitude = port.Latitude,
                    Longtitude = port.Longtitude,
                    SearchFields = port.SearchFields,
                    Notes = port.Notes,
                    CombinedCode = port.CombinedCode,
                    StateName = port.StateName,
                    StateCode = port.StateCode,
                    CountryCode = port.CountryCode,
                    CountryName = port.CountryName,
                    PortTimeZoneCode = port.PortTimeZoneCode,
                };

                portRepository.Add(newPort);
                portRepository.SubmitChanges();

                TableLastUpdateClass.UpdateTableHistory(tenant, "Port");
            }

            if (country == null)
            {
                country = countryRepository.GetSingleCountryByCode(port.Country.Code, tenant, true);
            }

            PortList portList = new PortList()
            {
                Code = newPort.Code,
                EnglishName = newPort.EnglishName,
                Id = newPort.Id,
                Tenant = newPort.Tenant,
                CountryCode = newPort.CountryCode,
                CountryName = newPort.CountryName,
                CountryEC = country.EC,
                IsAir = newPort.IsAir,
                IsOcean = newPort.IsOcean,
                IsInland = newPort.IsInland,
                AddedManually = newPort.AddedManually,
                SearchFields = newPort.SearchFields,
                Notes = newPort.Notes,
                InActive = newPort.InActive,
                TransportModeId = (newPort.IsAir ? "A" : "") + (newPort.IsInland ? "I" : "") + (newPort.IsOcean ? "O" : ""),
                CombinedCode = newPort.CombinedCode,
                StateName = newPort.StateName,
                StateCode = newPort.StateCode,
            };

            return portList;
        }

        public PortPM GetSinglePortPMByCodeCountryCode(string Code, string CountryCode, int tenant)
        {
            if (Code == "---")
            {
                return GetNotAssignedPortPM(tenant);
            }
            else
            {
                IQueryable<PortPM> ports = (from a in repository.context.Ports.Include("Country")
                                            where a.Tenant == tenant && a.Code.ToUpper() == Code.ToUpper().Trim() && a.Country.Code.ToUpper() == CountryCode.ToUpper()
                                            select new PortPM()
                                            {
                                                AddedManually = a.AddedManually,
                                                Code = a.Code,
                                                CountryId = a.CountryId,
                                                EnglishName = a.EnglishName,
                                                Field1 = a.Field1,
                                                Field2 = a.Field2,
                                                Field3 = a.Field3,
                                                Field4 = a.Field4,
                                                Field5 = a.Field5,
                                                Field6 = a.Field6,
                                                Field7 = a.Field7,
                                                Field8 = a.Field8,
                                                Field9 = a.Field9,
                                                Field10 = a.Field10,
                                                Id = a.Id,
                                                InActive = a.InActive,
                                                IsAir = a.IsAir,
                                                IsInland = a.IsInland,
                                                IsOcean = a.IsOcean,
                                                Latitude = a.Latitude,
                                                LocalName = a.LocalName,
                                                Longtitude = a.Longtitude,
                                                Notes = a.Notes,
                                                Tenant = a.Tenant,
                                                CountryName = a.CountryName,
                                                SearchFields = a.SearchFields,
                                                CountryCode = a.CountryCode,
                                                CountryEC = a.Country.EC,
                                                StateId = a.StateId,
                                                CombinedCode = a.CombinedCode,
                                                StateName = a.StateName,
                                                StateCode = a.StateCode,
                                            });
                var port = ports.FirstOrDefault();
                if (port == null)
                {
                    var zeroport = (from a in repository.context.Ports.Include("Country")
                                    where a.Tenant == 0 && a.Code.ToUpper() == Code.ToUpper().Trim() && a.Country.Code.ToUpper() == CountryCode.ToUpper()
                                    select new PortPM()
                                    {
                                        AddedManually = a.AddedManually,
                                        Code = a.Code,
                                        CountryId = a.CountryId,
                                        EnglishName = a.EnglishName,
                                        Field1 = a.Field1,
                                        Field2 = a.Field2,
                                        Field3 = a.Field3,
                                        Field4 = a.Field4,
                                        Field5 = a.Field5,
                                        Field6 = a.Field6,
                                        Field7 = a.Field7,
                                        Field8 = a.Field8,
                                        Field9 = a.Field9,
                                        Field10 = a.Field10,
                                        Id = a.Id,
                                        InActive = a.InActive,
                                        IsAir = a.IsAir,
                                        IsInland = a.IsInland,
                                        IsOcean = a.IsOcean,
                                        Latitude = a.Latitude,
                                        LocalName = a.LocalName,
                                        Longtitude = a.Longtitude,
                                        Notes = a.Notes,
                                        Tenant = a.Tenant,
                                        CountryName = a.CountryName,
                                        SearchFields = a.SearchFields,
                                        CountryCode = a.CountryCode,
                                        CountryEC = a.Country.EC,
                                        StateId = a.StateId,
                                        CombinedCode = a.CombinedCode,
                                        StateName = a.StateName,
                                        StateCode = a.StateCode,
                                    }).FirstOrDefault();

                    if (zeroport != null)
                    {
                        var newport = GetPortCopyToCurrentTenant(zeroport.Id, tenant);
                        port = new PortPM()
                        {
                            AddedManually = newport.AddedManually,
                            Code = newport.Code,
                            CountryId = newport.CountryId,
                            EnglishName = newport.EnglishName,
                            Id = newport.Id,
                            InActive = newport.InActive,
                            IsAir = newport.IsAir,
                            IsInland = newport.IsInland,
                            IsOcean = newport.IsOcean,
                            Notes = newport.Notes,
                            Tenant = newport.Tenant,
                            CountryName = newport.CountryName,
                            SearchFields = newport.SearchFields,
                            CountryCode = newport.CountryCode,
                            StateId = newport.StateId,
                            CombinedCode = newport.CombinedCode,
                            StateName = newport.StateName,
                            StateCode = newport.StateCode,
                        };
                    }
                }
                return port;
            }
        }

        public PortPM GetNotAssignedPortPM(int tenant)
        {
            IQueryable<PortPM> ports = (from a in repository.context.Ports.Include("Country")
                                        where a.Tenant == tenant && a.Code.ToUpper() == "---"
                                        select new PortPM()
                                        {
                                            AddedManually = a.AddedManually,
                                            Code = a.Code,
                                            CountryId = a.CountryId,
                                            EnglishName = a.EnglishName,
                                            Field1 = a.Field1,
                                            Field2 = a.Field2,
                                            Field3 = a.Field3,
                                            Field4 = a.Field4,
                                            Field5 = a.Field5,
                                            Field6 = a.Field6,
                                            Field7 = a.Field7,
                                            Field8 = a.Field8,
                                            Field9 = a.Field9,
                                            Field10 = a.Field10,
                                            Id = a.Id,
                                            InActive = a.InActive,
                                            IsAir = a.IsAir,
                                            IsInland = a.IsInland,
                                            IsOcean = a.IsOcean,
                                            Latitude = a.Latitude,
                                            LocalName = a.LocalName,
                                            Longtitude = a.Longtitude,
                                            Notes = a.Notes,
                                            Tenant = a.Tenant,
                                            CountryName = a.CountryName,
                                            SearchFields = a.SearchFields,
                                            CountryCode = a.CountryCode,
                                            CountryEC = a.Country.EC,
                                            StateId = a.StateId,
                                            CombinedCode = a.CombinedCode,
                                            StateName = a.StateName,
                                            StateCode = a.StateCode,
                                        });

            var port = ports.FirstOrDefault();
            if (port == null)
            {
                var zeroport = (from a in repository.context.Ports.Include("Country")
                                where a.Tenant == 0 && a.Code.ToUpper() == "---"
                                select new PortPM()
                                {
                                    AddedManually = a.AddedManually,
                                    Code = a.Code,
                                    CountryId = a.CountryId,
                                    EnglishName = a.EnglishName,
                                    Field1 = a.Field1,
                                    Field2 = a.Field2,
                                    Field3 = a.Field3,
                                    Field4 = a.Field4,
                                    Field5 = a.Field5,
                                    Field6 = a.Field6,
                                    Field7 = a.Field7,
                                    Field8 = a.Field8,
                                    Field9 = a.Field9,
                                    Field10 = a.Field10,
                                    Id = a.Id,
                                    InActive = a.InActive,
                                    IsAir = a.IsAir,
                                    IsInland = a.IsInland,
                                    IsOcean = a.IsOcean,
                                    Latitude = a.Latitude,
                                    LocalName = a.LocalName,
                                    Longtitude = a.Longtitude,
                                    Notes = a.Notes,
                                    Tenant = a.Tenant,
                                    CountryName = a.CountryName,
                                    SearchFields = a.SearchFields,
                                    CountryCode = a.CountryCode,
                                    CountryEC = a.Country.EC,
                                    StateId = a.StateId,
                                    CombinedCode = a.CombinedCode,
                                    StateName = a.StateName,
                                    StateCode = a.StateCode,
                                }).FirstOrDefault();

                if (zeroport != null)
                {
                    var newport = GetPortCopyToCurrentTenant(zeroport.Id, tenant);
                    port = new PortPM()
                    {
                        AddedManually = newport.AddedManually,
                        Code = newport.Code,
                        CountryId = newport.CountryId,
                        EnglishName = newport.EnglishName,
                        Id = newport.Id,
                        InActive = newport.InActive,
                        IsAir = newport.IsAir,
                        IsInland = newport.IsInland,
                        IsOcean = newport.IsOcean,
                        Notes = newport.Notes,
                        Tenant = newport.Tenant,
                        CountryName = newport.CountryName,
                        SearchFields = newport.SearchFields,
                        CountryCode = newport.CountryCode,
                        StateId = newport.StateId,
                        CombinedCode = newport.CombinedCode,
                        StateName = newport.StateName,
                        StateCode = newport.StateCode,
                    };
                }
            }
            return port;
        }

        public PortPM GetSinglePMByCombinedCode(string CombindCode, int Tenant)
        {
            var CountryCode = CombindCode.Substring(0, 2);
            var PortCode = CombindCode.Substring(2);
            return GetSinglePortPMByCodeCountryCode(PortCode, CountryCode, Tenant);
        }
        public PortPM GetSinglePMByCode(string Code, int Tenant)
        {
            return GetSinglePortPMByCode(Code, Tenant);
        }

        public PortPM GetSinglePortPMByCode(string Code, int tenant)
        {
            if (Code == "---")
            {
                return GetNotAssignedPortPM(tenant);
            }
            PortPM port = GetSinglePortByCode(Code, tenant);
            if (port != null)
            {
                return port;
            }

            port = GetSinglePortByCode(Code, 0);
            if (port == null)
            {
                return null;
            }

            var newport = GetPortCopyToCurrentTenant(port.Id, tenant);
            port = new PortPM()
            {
                AddedManually = newport.AddedManually,
                Code = newport.Code,
                CountryId = newport.CountryId,
                EnglishName = newport.EnglishName,
                Id = newport.Id,
                InActive = newport.InActive,
                IsAir = newport.IsAir,
                IsInland = newport.IsInland,
                IsOcean = newport.IsOcean,
                Notes = newport.Notes,
                Tenant = newport.Tenant,
                CountryName = newport.CountryName,
                SearchFields = newport.SearchFields,
                CountryCode = newport.CountryCode,
                StateId = newport.StateId,
                CombinedCode = newport.CombinedCode,
                StateName = newport.StateName,
                StateCode = newport.StateCode,
            };

            return port;

        }

        private PortPM GetSinglePortByCode(string Code, int tenant)
        {
            IQueryable<PortPM> ports = (from a in repository.context.Ports.Include("Country")
                                        where a.Tenant == tenant && a.Code.ToUpper() == Code.ToUpper().Trim()
                                        select new PortPM()
                                        {
                                            AddedManually = a.AddedManually,
                                            Code = a.Code,
                                            CountryId = a.CountryId,
                                            EnglishName = a.EnglishName,
                                            Field1 = a.Field1,
                                            Field2 = a.Field2,
                                            Field3 = a.Field3,
                                            Field4 = a.Field4,
                                            Field5 = a.Field5,
                                            Field6 = a.Field6,
                                            Field7 = a.Field7,
                                            Field8 = a.Field8,
                                            Field9 = a.Field9,
                                            Field10 = a.Field10,
                                            Id = a.Id,
                                            InActive = a.InActive,
                                            IsAir = a.IsAir,
                                            IsInland = a.IsInland,
                                            IsOcean = a.IsOcean,
                                            Latitude = a.Latitude,
                                            LocalName = a.LocalName,
                                            Longtitude = a.Longtitude,
                                            Notes = a.Notes,
                                            Tenant = a.Tenant,
                                            CountryName = a.CountryName,
                                            SearchFields = a.SearchFields,
                                            CountryCode = a.CountryCode,
                                            CountryEC = a.Country.EC,
                                            StateId = a.StateId,
                                            CombinedCode = a.CombinedCode,
                                            StateName = a.StateName,
                                            StateCode = a.StateCode,
                                        });
            return ports.FirstOrDefault();
        }

        public List<PortList> GetPortListsByListIds(List<string> PortIds, int tenant)
        {
            List<PortList> PortLists = (from a in repository.context.Ports
                                        where PortIds.Contains(a.Id) && a.Tenant == tenant
                                        select new PortList()
                                        {
                                            Id = a.Id,
                                            Tenant = a.Tenant,
                                            EnglishName = a.EnglishName,
                                            Code = a.Code,
                                        }).ToList();
            return PortLists;
        }

        public IQueryable<PortPM> GetPortPMsByCombinedCode(string combinedCode)
        {
            IQueryable<PortPM> ports = (from a in repository.context.Ports
                                        where a.CombinedCode == combinedCode
                                        select new PortPM()
                                        {
                                            AddedManually = a.AddedManually,
                                            Code = a.Code,
                                            CountryId = a.CountryId,
                                            EnglishName = a.EnglishName,
                                            Field1 = a.Field1,
                                            Field2 = a.Field2,
                                            Field3 = a.Field3,
                                            Field4 = a.Field4,
                                            Field5 = a.Field5,
                                            Field6 = a.Field6,
                                            Field7 = a.Field7,
                                            Field8 = a.Field8,
                                            Field9 = a.Field9,
                                            Field10 = a.Field10,
                                            Id = a.Id,
                                            InActive = a.InActive,
                                            IsAir = a.IsAir,
                                            IsInland = a.IsInland,
                                            IsOcean = a.IsOcean,
                                            Latitude = a.Latitude,
                                            LocalName = a.LocalName,
                                            Longtitude = a.Longtitude,
                                            Notes = a.Notes,
                                            Tenant = a.Tenant,
                                            CountryName = a.CountryName,
                                            SearchFields = a.SearchFields,
                                            CountryCode = a.CountryCode,
                                            CountryEC = a.Country.EC,
                                            StateId = a.StateId,
                                            CombinedCode = a.CombinedCode,
                                            StateName = a.StateName,
                                            StateCode = a.StateCode,
                                            PortTimeZoneCode = a.PortTimeZoneCode,
                                        });
            return ports;
        }
    }
}
