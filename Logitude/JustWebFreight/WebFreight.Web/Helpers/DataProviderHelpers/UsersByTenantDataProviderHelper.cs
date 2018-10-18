using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.Helpers.DataProviderHelpers
{
    public class UsersByTenantDataProviderHelper
    {
        public UsersByTenantDataProvider LoadUsersByTenantDataProvider(byte[] xmlFilters, int tenant)
        {
            UsersByTenantDataProvider dataProvider = new UsersByTenantDataProvider();
            #region Report Filters
            string distributorCode = "";
            string packageCode = "";
            string addOnPackageCode = "";
            bool includeInactiveTenants = false;
            bool includeInactiveUsers = false;

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            //DistributorCode
            QueryFilterItem queryFilterItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DistributorCode").FirstOrDefault();
            if (queryFilterItem != null && queryFilterItem.FieldValue != null) distributorCode = queryFilterItem.FieldValue.ToString();


            //PackageCode
            queryFilterItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "PackageCode").FirstOrDefault();
            if (queryFilterItem != null && queryFilterItem.FieldValue != null) packageCode = queryFilterItem.FieldValue.ToString();

            //AddOnPackageCode
            queryFilterItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "AddOnPackageCode").FirstOrDefault();
            if (queryFilterItem != null && queryFilterItem.FieldValue != null) addOnPackageCode = queryFilterItem.FieldValue.ToString();


            //IncludeInactiveTenants
            queryFilterItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeInactiveTenants").FirstOrDefault();
            if (queryFilterItem != null && queryFilterItem.FieldValue != null) includeInactiveTenants = queryFilterItem.FieldValue.ToString().ToLower() == "true" ? true : false;

            //IncludeInactiveUsers
            queryFilterItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeInactiveUsers").FirstOrDefault();
            if (queryFilterItem != null && queryFilterItem.FieldValue != null) includeInactiveUsers = queryFilterItem.FieldValue.ToString().ToLower() == "true" ? true : false;

            TenantQuery tenantQuery = new TenantQuery(tenant);
            dataProvider.UsersByTenantLists = tenantQuery.GetUsersByTenantLists(distributorCode, packageCode, addOnPackageCode, includeInactiveTenants, includeInactiveUsers, tenant);

            #endregion
            return dataProvider;
        }
    }
}