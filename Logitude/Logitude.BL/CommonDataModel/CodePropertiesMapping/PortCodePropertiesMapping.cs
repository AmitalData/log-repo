using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.CodePropertiesMapping
{
    public class PortCodePropertiesMapping
    {
        public static string GetPortIdFromPortProperties(int Tenant, CodeProperties CodeProp)
        {
            Port port;
            if (!string.IsNullOrEmpty(CodeProp.Id))
            {
                return CodeProp.Id;
            }
            else if (!string.IsNullOrEmpty(CodeProp.Code))
            {
                ICommonDataContext objectContext = CommonDataContext.GetContext(Tenant);
                PortRepository portRepository = new PortRepository(objectContext);
                port = portRepository.GetSinglePortByCodeCountryCode(Tenant, CodeProp.Code,CodeProp.CountryCode, true);
                if (port == null)
                {
                    port = GetPortCopyToCurrentTenant("", CodeProp.Code, CodeProp.CountryCode,Tenant);
                }
            }
            else if (!string.IsNullOrEmpty(CodeProp.ExternalCode))
            {
                throw new NotImplementedException();
                //ICommonDataContext objectContext = CommonDataContext.GetContext(Tenant);
                //ComputingPartnerTranslationQuery computingPartnerTranslationQuery = new ComputingPartnerTranslationQuery(Tenant);
                //port = computingPartnerTranslationQuery.GetTranslationsByPartnerAndTableId(Tenant, CodeProp.Code, true);
                //if (port == null)
                //{
                //    port = GetPortCopyToCurrentTenant("", CodeProp.Code, Tenant);
                //}
            }
            else
            {
                return "";
            }

            if (port != null)
            {
                return port.Id;
            }
            else
            {
                return "";
            }


        }

        private static Port GetPortCopyToCurrentTenant(string entityId, string Code,string CountryCode, int tenant)
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);

            PortRepository portRepository = new PortRepository(objectContext);
            CountryRepository countryRepository = new CountryRepository(objectContext);
            GlobalZoneRepository globalZoneRepository = new GlobalZoneRepository(objectContext);

            Port newPort;
            Port port;
            if (!string.IsNullOrEmpty(entityId))
            {
                port = portRepository.GetSinglePort(0, entityId);
            }
            else
            {
                port = portRepository.GetSinglePortByCodeCountryCode(0, Code, CountryCode, false);
            }

            newPort = portRepository.GetSinglePortByCodeCountryCode(tenant, port.Code, port.Country.Code, false);
            Country country = null;

            if (newPort == null)
            {
                country = countryRepository.GetSingleCountryByCode(port.Country.Code, tenant, false);

                if (country == null)
                {
                    GlobalZone globalzone = null;
                    if (port.Country.GlobalZone != null)
                    {
                        globalzone = globalZoneRepository.GetSingleGlobalZoneByCode(port.Country.GlobalZone.Code, tenant);
                    }
                    

                    if (globalzone == null)
                    {
                        GlobalZone oldZone = globalZoneRepository.GetSingleGlobalZone(port.Country.GlobalZoneId, 0);
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
                };

                portRepository.Add(newPort);
                portRepository.SubmitChanges();
            }

            if (country == null)
            {
                country = countryRepository.GetSingleCountryByCode(port.Country.Code, tenant, false);
            }

            return newPort;
        }
    }
}
