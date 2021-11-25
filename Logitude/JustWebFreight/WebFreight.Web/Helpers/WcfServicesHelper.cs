using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using WebFreight.Web.WcfApi;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Logitude.Server.Tools.Counters;
using Logitude.BL.Helpers;
using Logitude.BL.DataContracts;

namespace WebFreight.Web.Helpers
{
    public class WcfServicesHelper
    {
        public static void SetPortId(string code, int tenant, string propertyName, object entityPM, PortRepository portRepository, Response response)
        {

            if (!string.IsNullOrEmpty(code))
            {
                Port port = null;
                if (code.Length == 5)
                {
                    string countryCode = code.Substring(0, 2);
                    string portCode = code.Substring(2, 3);

                    port = portRepository.GetSinglePortByCodeCountryCode(tenant, portCode, countryCode, false);

                    if (port == null && tenant != 0)
                    {
                        Port tenantzeroPort = portRepository.GetSinglePortByCodeCountryCode(0, portCode, countryCode, false);
                        if (tenantzeroPort != null)
                        {
                            port = GetPortCopyToCurrentTenant(tenantzeroPort.Id, tenant);
                        }
                    }
                }
                else
                {
                    port = portRepository.GetSinglePortByCode(tenant, code, false);

                    if (port == null && tenant != 0)
                    {
                        Port tenantzeroPort = portRepository.GetSinglePortByCode(0, code, false);
                        if (tenantzeroPort != null)
                        {
                            port = GetPortCopyToCurrentTenant(tenantzeroPort.Id, tenant);
                        }
                    }
                }




                if (port != null)
                {
                    PropertyInfo propInfo = entityPM.GetType().GetProperty(propertyName);
                    propInfo.SetValue(entityPM, port.Id);
                }
                else
                {
                    response.HasError = true;
                    response.ErrorMessage = "- Property:" + propertyName + ", Error:" + "field doesn't exist in the database,Upsert this entity before using it." + Environment.NewLine;

                }
            }


        }


        public static Port GetPortOrCopyToTenant(string code, int tenant, PortRepository portRepository)
        {
            Port port = null;
            if (!string.IsNullOrEmpty(code))
            {

                if (code.Length == 5)
                {
                    string countryCode = code.Substring(0, 2);
                    string portCode = code.Substring(2, 3);

                    port = portRepository.GetSinglePortByCodeCountryCode(tenant, portCode, countryCode, false);

                    if (port == null && tenant != 0)
                    {
                        Port tenantzeroPort = portRepository.GetSinglePortByCodeCountryCode(0, portCode, countryCode, false);
                        if (tenantzeroPort != null)
                        {
                            port = GetPortCopyToCurrentTenant(tenantzeroPort.Id, tenant);
                        }
                    }
                }
                else
                {
                    port = portRepository.GetSinglePortByCode(tenant, code, false);

                    if (port == null && tenant != 0)
                    {
                        Port tenantzeroPort = portRepository.GetSinglePortByCode(0, code, false);
                        if (tenantzeroPort != null)
                        {
                            port = GetPortCopyToCurrentTenant(tenantzeroPort.Id, tenant);
                        }
                    }
                }

            }

            return port;
        }
    
        public static Port GetPortCopyToCurrentTenant(string entityId, int tenant)
        {


            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
            PortRepository portRepository = new PortRepository(objectContext);
            CountryRepository countryRepository = new CountryRepository(objectContext);
            GlobalZoneRepository globalZoneRepository = new GlobalZoneRepository(objectContext);

            Port newPort;
            Port port = portRepository.GetSinglePort(0, entityId);
            newPort = portRepository.GetSinglePortByCodeCountryCode(tenant, port.Code, port.Country.Code, false);
            Country country = null;

            if (newPort == null)
            {
                country = countryRepository.GetSingleCountryByCode(port.Country.Code, tenant, true);

                if (country == null)
                {
                    var zerocountry = countryRepository.GetSingleCountryByCode(port.Country.Code, 0, true);
                    GlobalZone globalzone = globalZoneRepository.GetSingleGlobalZoneByCode(zerocountry.Code, tenant);

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
                        IsNorthAmerica = oldCountry.IsNorthAmerica,
                        IsGreaterChina = oldCountry.IsGreaterChina,
                        HasCitiesList = oldCountry.HasCitiesList,
                    };

                    countryRepository.Add(country);
                    countryRepository.SubmitChanges();


                }

                newPort = new Port()
                {
                    Id = IdCounter.GetNumber("Port", tenant).ToString(),
                    Code = port.Code,
                    CombinedCode = port.CombinedCode,
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
                    CountryCode = port.CountryCode,
                    CountryName = port.CountryName,
                    StateCode = port.StateCode,
                    StateName = port.StateName,
                    PortTimeZoneCode = port.PortTimeZoneCode,
                };

                portRepository.Add(newPort);
                portRepository.SubmitChanges();
                RunStoredProcedureClass.UpdatePortSearcsFields(newPort.Id, newPort.Tenant);

                TableLastUpdateClass.UpdateTableHistory(tenant, "Port");
            }

            


            return newPort;
        }
    }
}