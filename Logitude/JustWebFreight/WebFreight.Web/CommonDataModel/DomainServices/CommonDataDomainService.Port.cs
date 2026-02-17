using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdatePortList(PortList currentEntity)
        {
        }

        public IQueryable<PortPM> GetPorts(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Port", "READ", tenant);

            portQuery = new PortQuery(tenant);
            return portQuery.GetPortPMsByTenant(0);
        }

        public PortPM GetPortsById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Port", "READ", tenant);

            portQuery = new PortQuery(tenant);
            return portQuery.GetSinglePM(id, tenant);
        }

        public List<PortPM> GetPortsWithInDistance(double distance, double lat, double lon, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Port", "READ", tenant);

            portQuery = new PortQuery(tenant);
            IQueryable<PortPM> portList = portQuery.GetPortPMsByTenant(0);
            List<PortPM> ports = new List<PortPM>();
            foreach (PortPM port in portList)
            {
                if (port.Latitude != 0 && port.Longtitude != 0)
                {
                    if (CalcDistance(lat, lon, port.Latitude, port.Longtitude) <= distance)
                    {
                        ports.Add(port);
                    }
                }
            }
            return ports;
        }

        public const double EarthRadiusInMiles = 3956.0;
        public const double EarthRadiusInKilometers = 6367.0;
        public static double ToRadian(double val) { return val * (Math.PI / 180); }
        public static double DiffRadian(double val1, double val2) { return ToRadian(val2) - ToRadian(val1); }

        /// <summary>
        /// Calculate the distance between two geocodes. Defaults to using Miles.
        /// </summary>
        /// 
        public static double CalcDistance(double lat1, double lng1, double lat2, double lng2)
        {
            return CalcDistance(lat1, lng1, lat2, lng2, GeoCodeCalcMeasurement.Kilometers);
        }

        /// <summary>
        /// Calculate the distance between two geocodes.
        /// </summary>
        /// 
        public static double CalcDistance(double lat1, double lng1, double lat2, double lng2, GeoCodeCalcMeasurement m)
        {
            double radius = EarthRadiusInMiles;
            if (m == GeoCodeCalcMeasurement.Kilometers) { radius = EarthRadiusInKilometers; }
            return radius * 2 * Math.Asin(Math.Min(1, Math.Sqrt((Math.Pow(Math.Sin((DiffRadian(lat1, lat2)) / 2.0), 2.0) + Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2)) * Math.Pow(Math.Sin((DiffRadian(lng1, lng2)) / 2.0), 2.0)))));
        }

        public enum GeoCodeCalcMeasurement : int
        {
            Miles = 0,
            Kilometers = 1
        }

        public IQueryable<PortPM> GetSinglePortByInput(string input, bool byCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Port", "READ", tenant);

            portQuery = new PortQuery(tenant);
            return portQuery.GetSinglePortPMByCode(input, byCode, tenant);
        }

        public IQueryable<Port> GetPortByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Port", "READ", tenant);

            portRepository = new PortRepository(tenant);
            return portRepository.GetPorts(tenant);
        }

        public IQueryable<PortPM> GetPortsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Port", "READ", tenant);

            portQuery = new PortQuery(tenant);
            if (tenant == 0)
            {
                return portQuery.GetPortPMsByTenant(tenant).Take(1000);
            }
            else
            {
                return portQuery.GetPortPMsByTenant(tenant);
            }
        }

        public PortList GetSinglePortList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Port", "READ", tenant);

            portRepository = new PortRepository(tenant);
            PortList portList = null;
            Port port = portRepository.GetSinglePort(tenant, id);
            
            if (port != null)
            {
                List<Port> singleEntityList = new List<Port>();
                singleEntityList.Add(port);

                portQuery = new PortQuery(portRepository);
                IQueryable<Port> iQueryable = singleEntityList.AsQueryable();
                IQueryable<PortList> iQueryableEntityList = portQuery.GetIQueryableEntityList(iQueryable);
                portList = iQueryableEntityList.FirstOrDefault();

            }
            return portList;
        }

        public PortList GetPortCopyToCurrentTenant(string entityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Port", "READ", tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            portRepository = new PortRepository(objectContext);
            portQuery = new PortQuery(portRepository);
            PortList myResult = portQuery.GetPortCopyToCurrentTenant(entityId, tenant);
            return myResult;
        }

        public List<PortList> GetPortLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Port", "READ", tenant);

            portRepository = new PortRepository(tenant);
            portQuery = new PortQuery(portRepository);

            IQueryable<Port> ports = null;
            if (tenant == 0)
            {
                ports = portRepository.GetPorts(tenant).Take(1000);
            }
            else
            {
                ports = portRepository.GetPorts(tenant);
            }
            IQueryable<PortList> query2 = portQuery.GetIQueryableEntityList(ports);

            return query2.ToList();
        }

        [Query(HasSideEffects = true)]
        public List<PortList> GetPortFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Port", "READ", tenant);
            portQuery = new PortQuery(tenant);
            return portQuery.GetPortFilters(xmlFilters, tenant);
         
        }

        public int GetPortFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Port", "READ", tenant);
            portQuery = new PortQuery(tenant);
            return portQuery.GetPortFiltersCount(xmlFilters, tenant);
          
        }

        [Query(HasSideEffects = true)]
        public List<PortList> GetPortCompactFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Port", "READ", tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }
            ICommonDataContext context = CommonDataContext.GetContext(tenant);
            portRepository = new PortRepository(objectContext);
            countryRepository = new CountryRepository(objectContext);
            portQuery = new PortQuery(portRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Port> ports = portRepository.GetPorts(tenant);
            ObjectTableRepository objectTableRep = new ObjectTableRepository(tenant);
            PortCustomFilter customfilters = new PortCustomFilter(tenant);
            QueryFilterItem item = queryOperations.QueryFilterItems.Where(f => f.FieldName == "CompactSearchField").FirstOrDefault();
            queryOperations.QueryFilterItems.Remove(item);
            object seachvalue = item != null ? item.FieldValue : null;

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            ports = customfilters.GetFilteredQuery(queryOperations, ports);
            ports = filter.GetFilteredQuery<Port>(nonListQueryOperation, ports);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<Country> countries = countryRepository.GetCountries(tenant);

            IQueryable<PortList> firstQuery = portQuery.GetIQueryableEntityList(ports);
            List<PortList> resultList;

            firstQuery = filter.GetFilteredQuery<PortList>(listQueryOperation, firstQuery);

            if (seachvalue != null)
            {
                listQueryOperation.SetFilter("Code", seachvalue, false, "StartsWith", null, false);
                queryOperations.SortByColumnName = "Code";
                queryOperations.SortDirectin = "Ascending";

                IQueryable<PortList> codeQueryResult = filter.GetFilteredQuery<PortList>(listQueryOperation, firstQuery).Take(queryOperations.PageSize);

                codeQueryResult = QuerySortClass.GetSortedQuery(queryOperations, codeQueryResult, "Port", tenant);
                resultList = codeQueryResult.ToList();

                if (resultList.Count() < queryOperations.PageSize)
                {
                    listQueryOperation.SetFilter("Code", null, false, "StartsWith", null, false);
                    listQueryOperation.SetFilter("EnglishName", seachvalue, false, "StartsWith", null, false);

                    IQueryable<PortList> nameQueryResult = filter.GetFilteredQuery<PortList>(listQueryOperation, firstQuery);

                    queryOperations.SortByColumnName = "EnglishName";
                    nameQueryResult = QuerySortClass.GetSortedQuery(queryOperations, nameQueryResult, "Port", tenant);

                    foreach (PortList port in nameQueryResult)
                    {
                        if (!resultList.Where(p => p.Code == port.Code && p.CountryCode == port.CountryCode).Any())
                        {
                            resultList.Add(port);

                        }
                        if (resultList.Count == queryOperations.PageSize)
                        {
                            break;
                        }
                    }

                    if (resultList.Count() < queryOperations.PageSize)
                    {
                        listQueryOperation.SetFilter("Code", null, false, "StartsWith", null, false);
                        listQueryOperation.SetFilter("EnglishName", null, false, "StartsWith", null, false);
                        listQueryOperation.SetFilter("SearchFields", seachvalue, false, "Contains", null, false);

                        IQueryable<PortList> searchFieldQueryResult = filter.GetFilteredQuery<PortList>(listQueryOperation, firstQuery);

                        foreach (PortList port in searchFieldQueryResult)
                        {
                            if (!resultList.Where(p => p.Code == port.Code && p.CountryCode == port.CountryCode).Any())
                            {
                                resultList.Add(port);
                            }
                            if (resultList.Count == queryOperations.PageSize)
                            {
                                break;
                            }
                        }
                    }
                }
                firstQuery = resultList.AsQueryable();
            }

            firstQuery = firstQuery.Take(queryOperations.PageSize);
            return firstQuery.ToList();
        }

        public IQueryable<PortPM> GetFirstPortsByTenant(string input, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Port", "READ", tenant);

            portQuery = new PortQuery(tenant);
            input = input.ToUpper();
            return portQuery.GetPortPMsByTenant(tenant).Where(d => d.Tenant == tenant).Where(p => p.Code.ToUpper().StartsWith(input) || p.EnglishName.ToUpper().StartsWith(input)).Take(50).OrderBy(p => p.EnglishName);
        }

        public int GetPortsByTenantCount(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Port", "READ", tenant);

            portRepository = new PortRepository(tenant);
            int count = portRepository.GetPorts(tenant).Count();
            return count;
        }

        public IQueryable<PortPM> GetFirstPortsByTenantInput(string input, int tenant, bool byCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Port", "READ", tenant);

            portQuery = new PortQuery(tenant);
            input = input.ToUpper().Trim();
            if (input != String.Empty)
            {
                if (byCode)
                {
                    return portQuery.GetPortPMsByTenant(tenant).Where(d => d.Tenant == tenant).Where(p => p.Code.ToUpper().StartsWith(input)).Take(10).OrderBy(p => p.Code);
                }
                else
                {
                    return portQuery.GetPortPMsByTenant(tenant).Where(d => d.Tenant == tenant).Where(p => p.EnglishName.ToUpper().StartsWith(input)).Take(10).OrderBy(p => p.EnglishName);
                }
            }
            else
            {
                return portQuery.GetPortPMsByTenant(tenant).Where(d => d.Tenant == tenant).Take(10).OrderBy(p => p.EnglishName);
            }
        }

        public List<PortPM> GetFirstPortsByTenant2(string input, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Port", "READ", tenant);

            portQuery = new PortQuery(tenant);
            input = input.ToUpper();
            return portQuery.GetPortPMsByTenant(tenant).Where(d => d.Tenant == tenant).Where(p => p.Code.ToUpper().StartsWith(input) || p.EnglishName.ToUpper().StartsWith(input)).Take(50).OrderBy(p => p.EnglishName).ToList();
        }

        public IQueryable<PortPM> GetPortsByTenantAndCountry(int tenant, string country)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Port", "READ", tenant);

            portQuery = new PortQuery(tenant);
            return portQuery.GetPortPMsByTenantAndCountry(tenant, country);
        }

        public IQueryable<PortPM> GetPortsByTenantAndCountrySearch(int tenant, string country, string id, string name)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Port", "READ", tenant);

            portQuery = new PortQuery(tenant);
            return portQuery.GetPortPMsByNameOrCode(id, name, tenant).Where(d => d.Tenant == tenant && d.CountryId == country);
        }

        public bool DoesPortCodeExist(string code, int tenant)
        {
            portRepository = new PortRepository(tenant);
            return (portRepository.GetPorts(tenant).Where(d => d.Code == code && d.Tenant == tenant)).Any();
        }

        public IQueryable<PortPM> GetPortsSearch(string code, string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Port", "READ", tenant);

            portQuery = new PortQuery(tenant);
            IQueryable<PortPM> q = portQuery.GetPortPMsByNameOrCode(code, name, tenant).Where(d => d.Tenant == tenant);
            return q;
        }

        //public void MapPortPortPM(PortPM portPM, Port port)
        //{
        //    Country country = null;
        //    if (portPM.CountryId != null)
        //    {
        //        country = CountryRepository.GetSingleCountry(portPM.CountryId, portPM.Tenant, true);
        //    }
        //    port.CountryId = portPM.CountryId;
        //    port.AddedManually = portPM.AddedManually;
        //    port.Code = portPM.Code;
        //    port.EnglishName = portPM.EnglishName;
        //    port.Field1 = portPM.Field1;
        //    port.Field2 = portPM.Field2;
        //    port.Field3 = portPM.Field3;
        //    port.Field4 = portPM.Field4;
        //    port.Field5 = portPM.Field5;
        //    port.Field6 = portPM.Field6;
        //    port.Field7 = portPM.Field7;
        //    port.Field8 = portPM.Field8;
        //    port.Field9 = portPM.Field9;
        //    port.Field10 = portPM.Field10;
        //    port.InActive = portPM.InActive;
        //    port.IsAir = portPM.IsAir;
        //    port.IsInland = portPM.IsInland;
        //    port.IsOcean = portPM.IsOcean;
        //    port.Latitude = portPM.Latitude;
        //    port.LocalName = portPM.LocalName;
        //    port.Longtitude = portPM.Longtitude;
        //    port.Notes = portPM.Notes;
        //    port.Tenant = portPM.Tenant;
        //    port.SearchFields = portPM.Code + "," + (country != null ? country.EnglishName : "") + "," + portPM.EnglishName + "," + portPM.LocalName + "," + (country != null ? country.Code : "");
        //    port.StateId = portPM.StateId;
        //}

        public void InsertPort(PortPM port)
        {
            SecurityUtility.CheckContactFeature("Port", "NEW", port.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(port.Tenant);
            }
            PortService service = new PortService(objectContext , port.Tenant);
            service.Create(port);
            TableLastUpdateClass.UpdateTableHistory(port.Tenant, "Port");
            //bool exist = (from a in portRepository.GetPorts(port.Tenant)
            //              where a.Code == port.Code && a.Tenant == port.Tenant && a.Country.Id == port.CountryId
            //              select a).Any();
            //if (!exist)
            //{
            //    Port newPort = new Port();
            //    newPort.Id = IdCounter.GetNumber("Port", port.Tenant).ToString();
            //    port.Id = newPort.Id;
            //    MapPortPortPM(port, newPort);
            //    portRepository.Add(newPort);
                
            //    create TraceEvent
            //    WebFreightDomainService webfreightService = new WebFreightDomainService();
            //    ContactRepository contactsRepositorypository = new ContactRepository(objectContext);
            //    ContactQuery contactQuery = new ContactQuery(contactsRepositorypository);
            //    ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), port.Tenant, true);
            //    if (contact != null) EventTracer.CreateTraceEvent(new TraceEvent(), "CRPO", port.Tenant, contact.Id, port.Id, null, "Port", null, null, false);
                
            //    
            //}

            //else
            //{
            //    string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", port.Tenant);
            //    msg = msg.Replace("%Entity", "Port");
            //    throw new Exception(msg);
            //}
        }

        public void UpdatePort(PortPM currentPort)
        {
            SecurityUtility.CheckContactFeature("Port", "UPDATE", currentPort.Tenant);

            DateTimeFormatInfo myDtfi = new DateTimeFormatInfo();

            String[] myPatternsArray = myDtfi.GetAllDateTimePatterns();
            string m = "";
            foreach (string pattern in myPatternsArray)
            {
                m += pattern + "," + System.Environment.NewLine;
            }
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentPort.Tenant);
            }
            PortService service = new PortService(objectContext, currentPort.Tenant);
            service.Update(currentPort);
            TableLastUpdateClass.UpdateTableHistory(currentPort.Tenant, "Port");

            //portRepository = new PortRepository(objectContext);
            //SecurityUtility.CheckContactFeature("Port", "UPDATE", currentPort.Tenant);
            //string entityName = "Port" + currentPort.Id + currentPort.Tenant;
            //string entityPmName = "PortPM" + currentPort.Id + currentPort.Tenant;
            //if (CacheManager.CacheWrapper.Get(entityName) != null)
            //{
            //    CacheManager.CacheWrapper.Remove(entityName);
            //}
            //if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            //{
            //    CacheManager.CacheWrapper.Remove(entityPmName);
            //}
          
            //Port port = portRepository.GetSinglePort(currentPort.Tenant, currentPort.Id);
            //MapPortPortPM(currentPort, port);
            //portRepository.Update(port);
            
            ////create TraceEvent
            //WebFreightDomainService webfreightService = new WebFreightDomainService();
            //ContactRepository contactsRepositorypository = new ContactRepository(objectContext);
            //ContactQuery contactQuery = new ContactQuery(contactsRepositorypository);
            //ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), currentPort.Tenant, true);
            //if (contact != null) EventTracer.CreateTraceEvent(new TraceEvent(), "UPPO", currentPort.Tenant, contact.Id, currentPort.Id, null, "Port", null, null, false);
            
            //TableLastUpdateClass.UpdateTableHistory(currentPort.Tenant, "Port");
        }

        public void DeletePort(PortPM port)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(port.Tenant);
            }
            portRepository = new PortRepository(objectContext);
            Port deletedport = portRepository.GetSinglePort(port.Tenant, port.Id);
            PortMapping.MapEntity(port, deletedport, true);
            portRepository.Remove(deletedport);
        }
    }
}