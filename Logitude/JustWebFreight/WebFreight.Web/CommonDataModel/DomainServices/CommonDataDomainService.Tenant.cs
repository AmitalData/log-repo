using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataContracts;
using WebFreight.Web.GlobalModel;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.TariffModule.Data.Repositories;
using Logitude.TariffModule.Data.EntityPOCOs;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        [RequiresAuthentication]
        [Query(IsDefault = true)]
        public IQueryable<Tenant> GetTenants()
        {
            SecurityUtility.AuthenticationOnTenant(0);
            //SecurityUtility.CheckContactFeature("Tenant", "READ", 0);

            tenantRepository = new TenantRepository(0);
            return tenantRepository.GetTenants();
        }

        public TenantPM GetSingleTenantPM(int id)
        {
            return TenantQuery.GetSingleTenantPM(id, false);
        }

        public int GetTenantsCount()
        {
            return tenantRepository.GetTenantsCount();
        }

        [Invoke]
        public TenantPM CreateTenant(TenantPM tenant)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant.Id);
            }

            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            if (authToken == null || (authToken != null && authToken.Tenant != tenant.Id))
            {
                throw new ApplicationException("You are not authorized to do this operation");
            }

            tenantRepository = new TenantRepository(objectContext);
            passwordPolicyRepository = new PasswordPolicyRepository(objectContext);
            tenant.Id = CodeCounter.GetNumber("Tenant", 0);
            Tenant newTenant = new Tenant();
            newTenant.Id = tenant.Id;

            TenantMapping.MapEntity(tenant, newTenant, true);

            if (newTenant.PasswordPolicyCode == null)
            {
                PasswordPolicy policy = passwordPolicyRepository.GetSinglePasswordPolicy("MEDU");
                newTenant.PasswordPolicyCode = policy.Code;
            }

            tenantRepository.Add(newTenant);
            string database = GetConnectionString(tenant.Id);

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
            {
                GlobalTenantRepository globaltenantRep = new GlobalTenantRepository();
                GlobalDBRepository globaldbRep = new GlobalDBRepository();
                GlobalDB db = globaldbRep.GetSingleGlobalDB(database);
                GlobalTenant newGlobalTenant = new GlobalTenant() { Id = tenant.Id, GlobalDBId = database, CompanyName = tenant.Company };
                globaltenantRep.Add(newGlobalTenant);
                globaltenantRep.SubmitChanges();
                scope.Complete();
            }

            this.objectContext.SaveChanges();
            return tenant;
        }

        public bool CheckIfDatabaseBackupIsBuilt(int id)
        {
            tenantRepository = new TenantRepository(id);
            return tenantRepository.CheckIfDatabaseBackupBuilt(id);
        }

        [Invoke]
        public void SetDatabaseDataBackupNotReady(int id)
        {
            tenantRepository = new TenantRepository(id);
            Tenant tenant = tenantRepository.GetSingleTenant(id);
            tenant.IsDataBackupBuilt = false;
            tenantRepository.Update(tenant);
            tenantRepository.SubmitChanges();
        }

        [Invoke]
        public void SetDatabaseDataBackupReady(int id)
        {
            tenantRepository = new TenantRepository(id);
            Tenant tenant = tenantRepository.GetSingleTenant(id);
            tenant.IsDataBackupBuilt = true;
            tenantRepository.Update(tenant);
            tenantRepository.SubmitChanges();
        }

        private string BuildConnectionString(string dbConnectionInfo)
        {
            string[] information = dbConnectionInfo.Split(',');
            string dbName = information[0];
            string userName = information[1];
            string pass = information[2];
            SqlConnectionStringBuilder sqlBuilder =
               new SqlConnectionStringBuilder();

            // Set the properties for the data source.
            sqlBuilder.DataSource = ".";
            sqlBuilder.InitialCatalog = dbName;
            sqlBuilder.IntegratedSecurity = false;
            sqlBuilder.UserID = userName;
            sqlBuilder.Password = pass;

            // Build the SqlConnection connection string.
            string providerString = sqlBuilder.ToString();

            // Initialize the EntityConnectionStringBuilder.
            EntityConnectionStringBuilder entityBuilder =
                new EntityConnectionStringBuilder();

            //Set the provider name.
            entityBuilder.Provider = "System.Data.SqlClient";

            // Set the provider-specific connection string.
            entityBuilder.ProviderConnectionString = providerString;

            // Set the Metadata location.
            entityBuilder.Metadata = string.Format(@"res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl",
                "InfrastructureModel");

            return entityBuilder.ToString();
        }

        public string GetConnectionString(int tenant)
        {
            GlobalDBRepository globaldbRep = new GlobalDBRepository();
            int count = globaldbRep.GetDataBasesCount();
            string databaseName = "";
            int rem = (tenant % count);
            databaseName = rem.ToString();
            return databaseName;
        }

    

        public void UpdateTenantPM(TenantPM currentTenant)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentTenant.Id);
            }

            tenantRepository = new TenantRepository(objectContext);

            string entityName = "Tenant" + currentTenant.Id;
            string entityPmName = "TenantPM" + currentTenant.Id;
            string datetimeoffset = "datetimeoffset" + currentTenant.Id;

            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            if (authToken == null || (authToken != null && authToken.Tenant != currentTenant.Id))
            {
                throw new ApplicationException("You are not authorized to do this operation");
            }



            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }

            if (CacheManager.CacheWrapper.Get(datetimeoffset) != null)
            {
                CacheManager.CacheWrapper.Invalidate(datetimeoffset);
            }


            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            if (!string.IsNullOrEmpty(currentTenant.CurrencyId))
            {
                CommonDataDomainService service = new CommonDataDomainService();
                currentTenant.CurrencyId = service.GetTenantCurrency(currentTenant.CurrencyId, currentTenant.Id);
            }

            if (!string.IsNullOrEmpty(currentTenant.ProfitCurrencyId))
            {
                CommonDataDomainService service = new CommonDataDomainService();
                currentTenant.ProfitCurrencyId = service.GetTenantCurrency(currentTenant.ProfitCurrencyId, currentTenant.Id);

                TariffSettingRepository tariffSettingRepository = new TariffSettingRepository(currentTenant.Id);
                IQueryable<TariffSetting> tariffSettings = tariffSettingRepository.GetAll(currentTenant.Id);
                if(tariffSettings != null && tariffSettings.Count() > 0)
                {
                    TariffSetting myTariffSetting = tariffSettings.FirstOrDefault();
                    if(myTariffSetting != null)
                    {
                        if (string.IsNullOrEmpty(myTariffSetting.DefaultCurrencyId))
                        {
                            myTariffSetting.DefaultCurrencyId = currentTenant.ProfitCurrencyId;
                            tariffSettingRepository.Update(myTariffSetting);
                            tariffSettingRepository.SubmitChanges();
                        }
                    }
                }
            }

            AddressPM address = null;
            if (currentTenant.AddressId == null)
            {
                currentTenant.CountryCode = null;
                currentTenant.CountryName = null;
            }

            else
            {
                addressRepository = new AddressRepository(objectContext);
                addressQuery = new AddressQuery(addressRepository);
                address = addressQuery.GetSinglePM(currentTenant.AddressId, currentTenant.Id);
                if (address != null)
                {
                    currentTenant.CountryCode = address.CountryCode;
                    currentTenant.CountryName = address.CountryEnglishName;
                }
            }

            Tenant entity = tenantRepository.GetSingleTenant(currentTenant.Id);
            var value = currentTenant.Company + Environment.NewLine;

            if (currentTenant.AddressId != null && entity.AddressId == null)
            {
                if (address != null)
                {
                    if (address.Address1 != null && address.Address2 != null)
                        value = value + address.Address1 + "," + address.Address2 + Environment.NewLine;
                    else
                        if (address.Address1 != null && address.Address2 == null)
                        value = value + address.Address1 + Environment.NewLine;
                    else
                            if (address.Address1 == null && address.Address2 != null)
                        value = value + address.Address2 + Environment.NewLine;

                    if (address.City != null || address.StateCode != null || address.CountryCode != null || address.ZipCode != null)
                    {
                        if (address.City != null && address.CountryCode == null)
                            value = value + address.City;
                        else
                            if (address.CountryCode != null && address.City == null)
                            value = value + address.CountryCode;
                        else
                                if (address.City != null && address.CountryCode != null)
                            value = value + address.City + "-" + address.CountryCode;
                        else
                                    if (address.StateCode != null)
                            value = value + "(" + address.StateCode + ")";
                        else
                                        if (address.ZipCode != null)
                            value = value + address.ZipCode;

                        value = value + Environment.NewLine;
                    }
                    if (address.PhoneNumber != null && address.FaxNumber != null)

                        value = value + "Tel:" + " " + address.PhoneNumber + " " + "Fax:" + " " + address.FaxNumber + Environment.NewLine;
                    else
                        if (address.PhoneNumber != null && address.FaxNumber == null)

                        value = value + "Tel:" + " " + address.PhoneNumber + " " + Environment.NewLine;
                    else
                            if (address.PhoneNumber == null && address.FaxNumber != null)

                        value = value + "Fax:" + " " + address.FaxNumber + Environment.NewLine;

                    currentTenant.InvoiceSection1 = value;
                    currentTenant.InvoiceSection2 = currentTenant.Company;
                }
            }

            TenantMapping.MapEntity(currentTenant, entity, false);

            if (entity.PasswordPolicyCode == null)
            {
                PasswordPolicy policy = passwordPolicyRepository.GetSinglePasswordPolicy("MEDU");
                entity.PasswordPolicyCode = policy.Code;
            }

            tenantRepository.Update(entity);
            tenantRepository.SubmitChanges();

            UpdateLogboxTenantSettings(currentTenant);

            using (TransactionScope scop = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
            {
                GlobalTenantRepository globalTenantRep = new GlobalTenantRepository();
                GlobalTenant gtenant = globalTenantRep.GetGlobalTenantsByTenant(currentTenant.Id);
                gtenant.CompanyName = currentTenant.Company;
                globalTenantRep.Update(gtenant);
                globalTenantRep.SubmitChanges();

                TenantManagementRepository tenantMngmentRep = new TenantManagementRepository();
                TenantManagement tenantMngment = tenantMngmentRep.GetSingleTenantManagement(currentTenant.Id);
                tenantMngment.Name = currentTenant.Company;
                tenantMngmentRep.Update(tenantMngment);
                tenantMngmentRep.SubmitChanges();

                scop.Complete();
            }
        }

        private static void UpdateLogboxTenantSettings(TenantPM currentTenant)
        {
            LogBoxTenantSettingRepository logBoxTenantSettingRepository = new LogBoxTenantSettingRepository(currentTenant.Id);
            LogBoxTenantSetting logBoxTenantSetting = logBoxTenantSettingRepository.GetSingleLogBoxTenantSetting(currentTenant.Id);
            logBoxTenantSetting.CustomerTenantShareImportFile = currentTenant.CustomerTenantShareImportFile;
            logBoxTenantSetting.AutoArchiveOnInvoice = currentTenant.AutoArchiveOnInvoice;
            logBoxTenantSetting.DocumentShareAsDefault = currentTenant.DocumentShareAsDefault;
            logBoxTenantSetting.LogBoxAdminUserId = currentTenant.LogBoxAdminUserId;

            logBoxTenantSettingRepository.Update(logBoxTenantSetting);
            logBoxTenantSettingRepository.SubmitChanges();
        }

        public void DeleteTenantPM(TenantPM tenant)
        {
            tenantRepository = new TenantRepository(tenant.Id);
            Tenant entity = TenantRepository.GetSingleTenant(tenant.Id, false);
            tenantRepository.Remove(entity);
        }

        [Query(HasSideEffects = true)]
        public IQueryable<TenantList> GetTenantFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("Tenant", "READ", tenant);

            tenantRepository = new TenantRepository(tenant);
            tenantQuery = new TenantQuery(tenantRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Tenant> iQueryable = tenantRepository.GetTenants();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
            iQueryable = filter.GetFilteredQuery<Tenant>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<TenantList> query2 = tenantQuery.GetIQueryableEntityList(iQueryable);

            IGlobalContext globalObjectContext = GlobalContext.GetContext();
            IQueryable<TenantManagement> tenantmanagements = globalObjectContext.TenantManagements;
            foreach (TenantList t in query2)
            {
                t.PackageCode = tenantmanagements.Where(d => d.Id == t.Id).FirstOrDefault().PackageCode;
            }

            query2 = filter.GetFilteredQuery<TenantList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(TenantList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Tenant", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<TenantList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<TenantList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<TenantList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<TenantList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<TenantList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Id);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Id);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetTenantFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("Tenant", "READ", tenant);

            tenantRepository = new TenantRepository(tenant);
            tenantQuery = new TenantQuery(tenantRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Tenant> iQueryable = tenantRepository.GetTenants();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Tenant>(nonListQueryOperation, iQueryable);

            IQueryable<TenantList> query2 = tenantQuery.GetIQueryableEntityList(iQueryable);

            IGlobalContext globalObjectContext = GlobalContext.GetContext();
            IQueryable<TenantManagement> tenantmanagements = globalObjectContext.TenantManagements;
            foreach (TenantList t in query2)
            {
                t.PackageCode = tenantmanagements.Where(d => d.Id == t.Id).FirstOrDefault().PackageCode;
            }
           
            query2 = filter.GetFilteredQuery<TenantList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public IQueryable<TenantList> GetTenantLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Tenant", "READ", tenant);

            tenantRepository = new TenantRepository(tenant);
            tenantQuery = new TenantQuery(tenantRepository);

            IQueryable<Tenant> tenants = tenantRepository.GetTenants();
            IQueryable<TenantList> query2 = tenantQuery.GetIQueryableEntityList(tenants);

            IGlobalContext globalObjectContext = GlobalContext.GetContext();
            IQueryable<TenantManagement> tenantmanagements = globalObjectContext.TenantManagements;
            foreach (TenantList t in query2)
            {
                t.PackageCode = tenantmanagements.Where(d => d.Id == t.Id).FirstOrDefault().PackageCode;
            }
            return query2;
        }

        public IQueryable<TenantPM> GetTenantPMs(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("Tenant", "READ", tenant);

            tenantQuery = new TenantQuery(tenant);
            return tenantQuery.GetTenantPMs();
        }


    }
}