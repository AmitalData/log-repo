using System;
using System.Collections.Generic;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Server.Infrastructure.DataContracts;
using System.Reflection;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    // TODO: Create methods containing your application logic.
   // [RequiresAuthentication]
    [EnableClientAccess()]
    public partial class CommonDataDomainService : LogitudeDomainService
    {
        private ICommonDataContext objectContext;
        private AddressTypeRepository addressTypesRepository;
        private PackageRepository packageRepository;
        private FeatureTypeRepository featureTypeRepository;
        private RoleTypeRepository roleTypeRepository;
        private AccountingSystemRepository accountingSystemRepository;
        private AccountingSettingRepository accountingSettingRepository;
        private PasswordPolicyRepository passwordPolicyRepository;
        private UserLoginLogRepository userLoginLogRepository;
        private GlobalZoneRepository globalZoneRepository;
        private CountryRepository countryRepository;
        private StateRepository stateRepository;
        private AddressRepository addressRepository;
        private PortRepository portRepository;
        private RankRepository rankRepository;
        private PartnerTypeRepository partnerTypeRepository;
        private DepartmentRepository departmentRepository;
        private BranchRepository branchRepository;
        private CurrencyRepository currencyRepository;
        private TenantRepository tenantRepository;
        private IncotermRepository incotermRepository;
        private VatTypeRepository vatTypeRepository;
        private VatTypePercentageRepository vatTypePercentageRepository;
        private ChargesTypeRepository chargesTypeRepository;
        private EntityDateRepository entityDateRepository;
        private DocumentTypeRepository documentTypeRepository;
        private DocumentOutRepository documentOutRepository;
        //private DocumentInRepository documentInRepository;
        private DocumentRepository documentRepository;
        private PackageTypeRepository packageTypeRepository;
        private DocumentTypeCustomFieldRepository documentTypeCustomFieldRepository;
        private FormCustomFieldRepository formCustomFieldRepository;
        private RateClassRepository rateClassRepository;
        private WeightUnitRepository weightUnitRepository;
        private DimensionsUnitRepository dimensionsUnitRepository;
        private CommunicationAttachmentRepository communicationAttachmentRepository;
        private CommunicationLogRepository communicationLogRepository;
        private MeasurementRepository measurementRepository;
        private DueTypeRepository dueTypeRepository;
        private DocumentTypeTemplateRepository documentTypeTemplateRepository;
        private CommunicationStatusTypeRepository communicationStatusTypeRepository;
        private CommunicationLogTypeRepository communicationLogTypeRepository;
        private TemplateFormatRepository templateFormatRepository;
        private VesselRepository vesselRepository;
        private DocumentTypeCopyRepository documentTypeCopyRepository;
        private DocumentOutCopyRepository documentOutCopyRepository;
        private ReportRepository reportRepository;
        private ReportGroupRepository reportGroupRepository;
        private LeadSourceRepository leadSourceRepository;
        private IndustryRepository industryRepository;
        private ProductTypeRepository productTypeRepository;
        private ProductPeriodRepository productPeriodRepository;
        private CustomerProductRepository customerProductRepository;
        private CompetitorRepository competitorRepository;
        private ContactDoneMethodRepository ContactDoneMethodRepository;
        private AdditionalServiceRepository additionalServiceRepository;
        private CustomerStatusRepository customerStatusRepository;
        private CustomerSalesmanByProductRepository customerSalesmanByProductRepository;
        private RegionRepository regionRepository;
        private CustomerSizeRepository customerSizeRepository;
        private DistributorRepository distributorRepository;
        private CommunicationLogStepRepository communicationLogStepRepository;
        private DocumentsFilingRepository documentsFilingRepository;
        private DocumentTypeCategoryRepository documentTypeCategoryRepository;
        private DocumentTypeMetaDataRepository documentTypeMetaDataRepository;
        private DocumentsFilingMetaDataValueRepository documentsFilingMetaDataValueRepository;
        private CustomerTenantAccessStatusTypeRepository customerTenantAccessStatusTypeRepository;
        private CustomerTenantAccessRepository customerTenantAccessRepository;
        private CustomerTenantAccessCardsBatchRepository customerTenantAccessCardsBatchRepository;
        private CustomerTenantAccessRequestRepository customerTenantAccessRequestRepository;
        private HybridPartnerRepository hybridPartnerRepository;
        private AirlineStatisticsRepository airlineStatisticsRepository;
        private LogitudeMessagesTransmissionLogRepository logitudeMessagesTransmissionLogRepository;

        private DocumentsFilingQuery documentsFilingQuery;
        private AccountingSettingQuery accountingSettingQuery;
        private AccountingSystemQuery accountingSystemQuery;
        private AddressQuery addressQuery;
        private ChargesTypeQuery chargesTypeQuery;
        private CommunicationAttachmentQuery communicationAttachmentQuery;
        private CommunicationLogQuery communicationLogQuery;
        private CommunicationLogTypeQuery communicationLogTypeQuery;
        private CommunicationStatusTypeQuery communicationStatusTypeQuery;
        private CountryQuery countryQuery;
        private CurrencyQuery currencyQuery;
        private VatTypeQuery vatTypeQuery;
        private IncotermQuery incotermQuery;
        private DimensionsUnitQuery dimensionsUnitQuery;
        //private DocumentInQuery documentInQuery;
        private DocumentOutQuery documentOutQuery;
        private DocumentTypeCopyQuery documentTypeCopyQuery;
        private DocumentTypeCustomFieldQuery documentTypeCustomFieldQuery;
        private DocumentTypeQuery documentTypeQuery;
        private DocumentTypeTemplateQuery documentTypeTemplateQuery;
        private DueTypeQuery dueTypeQuery;
        private FormCustomFieldQuery formCustomFieldQuery;
        private GlobalZoneQuery globalZoneQuery;
        private MeasurementQuery measurementQuery;
        private PackageQuery packageQuery;
        private PackageTypeQuery packageTypeQuery;
        private TenantQuery tenantQuery;
        private PortQuery portQuery;
        private PartnerTypeQuery partnerTypeQuery;
        private PasswordPolicyQuery passwordPolicyQuery;
        private RankQuery rankQuery;
        private RateClassQuery rateClassQuery;
        private StateQuery stateQuery;
        private TemplateFormatQuery templateFormatQuery;
        private UserLoginLogQuery userLoginLogQuery;
        private VesselQuery vesselQuery;
        private WeightUnitQuery weightUnitQuery;
        private ReportQuery reportQuery;
        private ReportGroupQuery reportGroupQuery;
        private LeadSourceQuery leadSourceQuery;
        private IndustryQuery industryQuery;
        private ProductTypeQuery productTypeQuery;
        private ProductPeriodQuery productPeriodQuery;
        private CustomerProductQuery customerProductQuery;
        private CompetitorQuery competitorQuery;
        private ContactDoneMethodQuery ContactDoneMethodQuery;
        private AdditionalServiceQuery additionalServiceQuery;
        private CustomerStatusQuery customerStatusQuery;
        private CustomerSalesmanByProductQuery customerSalesmanByProductQuery;
        private RegionQuery regionQuery;
        private CustomerSizeQuery customerSizeQuery;
        private DistributorQuery distributorQuery;
        private CommunicationLogStepQuery communicationLogStepQuery;
        private DocumentTypeCategoryQuery documentTypeCategoryQuery;
        private DocumentTypeMetaDataQuery documentTypeMetaDataQuery;
        private DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery;
        private CustomerTenantAccessStatusTypeQuery customerTenantAccessStatusTypeQuery;
        private CustomerTenantAccessQuery customerTenantAccessQuery;
        private CustomerTenantAccessCardBatchQuery customerTenantAccessCardBatchQuery;
        private CustomerTenantAccessRequestQuery customerTenantAccessRequestQuery;
        private HybridPartnerQuery hybridPartnerQuery;
        private AirlineStatisticsQuery airlineStatisticsQuery;
        private LogitudeMessagesTransmissionLogQuery logitudeMessagesTransmissionLogQuery;

        public CommonDataDomainService(UserData currentUser)
        {

            
        }

        public CommonDataDomainService()
        {

            
        }

        public CommonDataDomainService(ICommonDataContext context)
        {
           
        }


        [Query(HasSideEffects = true)]
        public IQueryable<LoginPolicyList> GetLoginPolicyFilters(byte[] xmlFilters, int tenant)
        {
            LoginPolicyRepository LoginPolicyRepository = new LoginPolicyRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<LoginPolicy> iQueryable = LoginPolicyRepository.GetLoginPolicies();

            //PortCustomFilter customfilters = new PortCustomFilter(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<LoginPolicy>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new LoginPolicyList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<LoginPolicyList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(LoginPolicyList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("LoginPolicy", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<LoginPolicyList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<LoginPolicyList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<LoginPolicyList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<LoginPolicyList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<LoginPolicyList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Name);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetLoginPolicyFiltersCount(byte[] xmlFilters, int tenant)
        {
            LoginPolicyRepository LoginPolicyRepository = new LoginPolicyRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<LoginPolicy> iQueryable = LoginPolicyRepository.GetLoginPolicies();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<LoginPolicy>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new LoginPolicyList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<LoginPolicyList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }


        [Query(HasSideEffects = true)]
        public IQueryable<MetodoPagoList> GetMetodoPagoFilters(byte[] xmlFilters, int tenant)
        {
            MetodoPagoRepository metodoPagoRepository = new MetodoPagoRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<MetodoPago> iQueryable = metodoPagoRepository.GetMetodoPagos();

            //PortCustomFilter customfilters = new PortCustomFilter(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<MetodoPago>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new MetodoPagoList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<MetodoPagoList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(LoginPolicyList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("MetodoPago", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<MetodoPagoList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<MetodoPagoList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<MetodoPagoList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<MetodoPagoList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<MetodoPagoList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Name);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetMetodoPagoFiltersCount(byte[] xmlFilters, int tenant)
        {
            MetodoPagoRepository metodoPagoRepository = new MetodoPagoRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<MetodoPago> iQueryable = metodoPagoRepository.GetMetodoPagos();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<MetodoPago>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new MetodoPagoList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<MetodoPagoList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<RegimenFiscalList> GetRegimenFiscalFilters(byte[] xmlFilters, int tenant)
        {
            RegimenFiscalRepository RegimenFiscalRepository = new RegimenFiscalRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<RegimenFiscal> iQueryable = RegimenFiscalRepository.GetRegimenFiscals();

            //PortCustomFilter customfilters = new PortCustomFilter(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<RegimenFiscal>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new RegimenFiscalList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<RegimenFiscalList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(LoginPolicyList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("MetodoPago", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<RegimenFiscalList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<RegimenFiscalList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<RegimenFiscalList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<RegimenFiscalList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<RegimenFiscalList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Name);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetRegimenFiscalFiltersCount(byte[] xmlFilters, int tenant)
        {
            RegimenFiscalRepository RegimenFiscalRepository = new RegimenFiscalRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<RegimenFiscal> iQueryable = RegimenFiscalRepository.GetRegimenFiscals();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<RegimenFiscal>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new RegimenFiscalList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<RegimenFiscalList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<PostalCodeList> GetPostalCodeFilters(byte[] xmlFilters, int tenant)
        {
            PostalCodeRepository PostalCodeRepository = new PostalCodeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<PostalCode> iQueryable = PostalCodeRepository.GetPostalCodes();

            //PortCustomFilter customfilters = new PortCustomFilter(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PostalCode>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new PostalCodeList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                             CountryCode = entity.CountryCode,
                         };

            query2 = filter.GetFilteredQuery<PostalCodeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(LoginPolicyList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("MetodoPago", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<PostalCodeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<PostalCodeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<PostalCodeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<PostalCodeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<PostalCodeList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Name);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetPostalCodeFiltersCount(byte[] xmlFilters, int tenant)
        {
            PostalCodeRepository PostalCodeRepository = new PostalCodeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<PostalCode> iQueryable = PostalCodeRepository.GetPostalCodes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<PostalCode>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new PostalCodeList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                             CountryCode = entity.CountryCode,
                         };

            query2 = filter.GetFilteredQuery<PostalCodeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<UsoCFDIList> GetUsoCFDIFilters(byte[] xmlFilters, int tenant)
        {
            UsoCFDIRepository UsoCFDIRepository = new UsoCFDIRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<UsoCFDI> iQueryable = UsoCFDIRepository.GetUsoCFDIs();

            //PortCustomFilter customfilters = new PortCustomFilter(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<UsoCFDI>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new UsoCFDIList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<UsoCFDIList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(LoginPolicyList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("UsoCFDI", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<UsoCFDIList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<UsoCFDIList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<UsoCFDIList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<UsoCFDIList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<UsoCFDIList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Name);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetUsoCFDIFiltersCount(byte[] xmlFilters, int tenant)
        {
            UsoCFDIRepository UsoCFDIRepository = new UsoCFDIRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<UsoCFDI> iQueryable = UsoCFDIRepository.GetUsoCFDIs();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<UsoCFDI>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new UsoCFDIList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<UsoCFDIList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }



        [Query(HasSideEffects = true)]
        public IQueryable<NumberFormatList> GetNumberFormatFilters(byte[] xmlFilters, int tenant)
        {
            NumberFormatRepository NumberFormatRepository = new NumberFormatRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<NumberFormat> iQueryable = NumberFormatRepository.GetNumberFormats();

            //PortCustomFilter customfilters = new PortCustomFilter(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<NumberFormat>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new NumberFormatList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<NumberFormatList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(LoginPolicyList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("NumberFormat", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<NumberFormatList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<NumberFormatList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<NumberFormatList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<NumberFormatList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<NumberFormatList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Name);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetNumberFormatFiltersCount(byte[] xmlFilters, int tenant)
        {
            NumberFormatRepository NumberFormatRepository = new NumberFormatRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<NumberFormat> iQueryable = NumberFormatRepository.GetNumberFormats();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<NumberFormat>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new NumberFormatList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<NumberFormatList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }



        public GettingStartedData GetGettingStartedData(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            GettingStartedData result = new GettingStartedData()
            {
                Id = 1
            };

            if (SecurityUtility.CheckTableContactFeature("Port", "READ", tenant))
            {
                PortRepository thePortRepository = new PortRepository(tenant);
                result.PortsCount = thePortRepository.GetPortsCount(tenant);
            }

            if (SecurityUtility.CheckTableContactFeature("Airline", "READ", tenant))
            {
                AirlineRepository airlineRepository = new AirlineRepository(tenant);
                result.AirlinesCount = airlineRepository.GetAirlinesCount(tenant);
            }

            if (SecurityUtility.CheckTableContactFeature("ShippingLine", "READ", tenant))
            {
                ShippingLineRepository shippingLineRepository = new ShippingLineRepository(tenant);
                result.ShippinglinesCount = shippingLineRepository.GetShippinngLinesCount(tenant);
            }

            if (SecurityUtility.CheckTableContactFeature("User", "READ", tenant))
            {
                UserRepository userRepository = new UserRepository(tenant);
                result.UsersCount = userRepository.GetUsersCount(tenant);
            }

            if (SecurityUtility.CheckTableContactFeature("Agent", "READ", tenant))
            {
                AgentRepository agentRepository = new AgentRepository(tenant);
                result.AgentsCount = agentRepository.GetAgentsCount(tenant);
            }

            if (SecurityUtility.CheckTableContactFeature("Customer", "READ", tenant))
            {
                CustomerRepository customerRepository = new CustomerRepository(tenant);
                result.CustomersCount = customerRepository.GetCustomerCount(tenant);
            }

            if (SecurityUtility.CheckTableContactFeature("Shipment", "READ", tenant))
            {
                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                result.ShipmentsCount = shipmentRepository.GetAllShipmentsCount(tenant);
            }

            if (SecurityUtility.CheckTableContactFeature("Master", "READ", tenant))
            {
                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                result.MastersCount = shipmentRepository.GetAllMastersCount(tenant);
            }

            if (SecurityUtility.CheckTableContactFeature("Quote", "READ", tenant))
            {
                QuoteRepository quoteRepository = new QuoteRepository(tenant);
                result.QuotesCount = quoteRepository.GetQuotesCount(tenant);
            }
            return result;
        }
        
        public override void Initialize(System.ServiceModel.DomainServices.Server.DomainServiceContext context)
        {
            base.Initialize(context);
        }

        public override System.Collections.IEnumerable Query(QueryDescription queryDescription, out IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> validationErrors, out int totalCount)
        {
            return base.Query(queryDescription, out validationErrors, out totalCount);
        }
        
        protected override bool PersistChangeSet()
        {
            if (objectContext != null)
            {
                objectContext.SaveChanges();
            }

            return base.PersistChangeSet();
        }

        protected override bool ExecuteChangeSet()
        {
            return base.ExecuteChangeSet();
        }        
    }
}


