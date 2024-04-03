using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.InfrastructureModel.EntityLists;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class ObjectTableQuery
    {
        ObjectTableRepository repository;
        public ObjectTableQuery()
        {
            repository = new ObjectTableRepository(); 
        }
        public ObjectTableQuery(int tenant)
        {
            repository = new ObjectTableRepository(tenant);
        }
        public ObjectTableQuery(ObjectTableRepository objectTabelRepository)
        {
            repository = objectTabelRepository;
        }
        
        public IQueryable<ObjectTablePM> GetLastUpdatedTables(int tenant, DateTime sinceDate)
        {
            List<ObjectTablePM> currentLastUpdates = new List<ObjectTablePM>();
            List<ObjectTablePM> zeroLastUpdates = new List<ObjectTablePM>();

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
               WebFreightContext webFreightContext = (WebFreightContext)WebFreightContext.GetContext(tenant);

               currentLastUpdates = (from a in repository.context.ObjectTables.Include("HeaderScreen").Include("DescriptionTextCode").Include("NewButtonTextCode").Include("FullNameTextCode")
                                     where a.Tenant == tenant && a.LastUpdateDate > sinceDate
                                     select new ObjectTablePM()
                                     {
                                         LookUp1 = a.LookUp1,
                                         LookUp2 = a.LookUp2,
                                         AutoCompleteSearchWindow = a.AutoCompleteSearchWindow,
                                         DependencyFilter1 = a.DependencyFilter1,
                                         DependencyFilter2 = a.DependencyFilter2,
                                         DependencyFilter3 = a.DependencyFilter3,
                                         EditableFromAutoCompleteWindow = a.EditableFromAutoCompleteWindow,
                                         HeaderScreenId = a.HeaderScreenId,
                                         Id = a.Id,
                                         IsClosed = a.IsClosed,
                                         IsNewWizard = a.IsNewWizard,
                                         NewWizardControlName = a.NewWizardControlName,
                                         KeyPropertyPath = a.KeyPropertyPath,
                                         LastUpdateDate = a.LastUpdateDate,
                                         Name = a.Name,
                                         Tenant = a.Tenant,
                                         CacheOnClient = a.CacheOnClient,
                                         HeaderScreenCode = a.HeaderScreenCode,
                                         HasCounter = a.HasCounter,
                                         EnableAddFromLOV = a.EnableAddFromLOV,
                                         EnableEditFromLOV = a.EnableEditFromLOV,
                                         IsMain = a.IsMain,
                                         IsRestrictable = a.IsRestrictable,
                                         IsAutoComplete = a.IsAutoComplete,
                                         SortingByObjectField = a.SortingByObjectField,
                                         HasCustomFields = a.HasCustomFields,
                                         CustomFieldsCount = a.CustomFieldsCount,
                                         DescriptionTextCodeId = a.DescriptionTextCodeId,
                                         DescriptionTextCodeCode = a.DescriptionTextCodeCode,
                                         SearchFields = a.SearchFields,
                                         IsSaveButtonVisible = a.IsSaveButtonVisible,
                                         MainTipCode = a.MainTipCode,
                                         EnableSecurity = a.EnableSecurity,
                                         ObjectTableTypeCode = a.ObjectTableTypeCode,
                                         IsComposition = a.IsComposition,
                                         AllowCustomFields = a.AllowCustomFields,
                                         MaxNumberOfCustomFields = a.MaxNumberOfCustomFields,
                                         DBTableName = a.DBTableName,
                                         HasDynamicHeader = a.HasDynamicHeader,
                                         IsLookUp = a.IsLookUp,
                                         HasDocuments = a.HasDocuments,
                                         NewButtonTextCodeCode = a.NewButtonTextCodeCode,
                                         HasCustomValidator = a.HasCustomValidator,
                                         ClientModuleName = a.ClientModuleName,
                                         ServerModuleName = a.ServerModuleName,
                                         NewWizardComponentPath = a.NewWizardComponentPath,
                                         HasHelper = a.HasHelper,
                                         HasShortTitle = a.HasShortTitle,
                                         HasMenuButtons = a.HasMenuButtons,
                                         HasFiltersMenu = a.HasFiltersMenu,
                                         EntityResourceLastUpdate = a.EntityResourceLastUpdate,
                                         DownloadToExcelFeatureCode  = a.DownloadToExcelFeatureCode,
                                         SplitComponentPath = a.SplitComponentPath,
                                         AllowedForComputingPartners=a.AllowedForComputingPartners,
                                         CodeField = a.CodeField,
                                         NameField = a.NameField,
                                         DisableSearchBox = a.DisableSearchBox,
                                         AllowedInTicket = a.AllowedInTicket,
                                         LovDisplayMemberPath=a.LovDisplayMemberPath,
                                         LovDisplayMemberPathLocal=a.LovDisplayMemberPathLocal,
                                         IsTabsHidden = a.IsTabsHidden,
                                         ParentObjectTableName = a.ParentObjectTableName,
                                         AvailableInCustomization = a.AvailableInCustomization,
                                         ParentObjectTableId = a.ParentObjectTableId,
                                         IsCustom = a.IsCustom,
                                         SupportSubEntity = a.SupportSubEntity,
                                         ApplyGenericCustomFields = a.ApplyGenericCustomFields,
                                         FullNameTextCodeId = a.FullNameTextCodeId,
                                         FullNameTextCodeCode = a.FullNameTextCodeCode,
                                         FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : a.Name,
                                         AvailableInDocumentTypes = a.AvailableInDocumentTypes,
                                     }).ToList();
            }
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
             WebFreightContext   webFreightContext = (WebFreightContext)WebFreightContext.GetContext(0);

             zeroLastUpdates = (from a in repository.context.ObjectTables.Include("HeaderScreen").Include("DescriptionTextCode").Include("NewButtonTextCode").Include("FullNameTextCode")
                                where a.Tenant == 0 && a.LastUpdateDate > sinceDate
                                select new ObjectTablePM()
                                {
                                    NewButtonTextCodeCode = a.NewButtonTextCodeCode,
                                    LookUp1 = a.LookUp1,
                                    LookUp2 = a.LookUp2,
                                    AutoCompleteSearchWindow = a.AutoCompleteSearchWindow,
                                    DependencyFilter1 = a.DependencyFilter1,
                                    DependencyFilter2 = a.DependencyFilter2,
                                    DependencyFilter3 = a.DependencyFilter3,
                                    EditableFromAutoCompleteWindow = a.EditableFromAutoCompleteWindow,
                                    HeaderScreenId = a.HeaderScreenId,
                                    Id = a.Id,
                                    IsClosed = a.IsClosed,
                                    IsNewWizard = a.IsNewWizard,
                                    NewWizardControlName = a.NewWizardControlName,
                                    KeyPropertyPath = a.KeyPropertyPath,
                                    LastUpdateDate = a.LastUpdateDate,
                                    Name = a.Name,
                                    Tenant = a.Tenant,
                                    CacheOnClient = a.CacheOnClient,
                                    HeaderScreenCode = a.HeaderScreenCode,
                                    HasCounter = a.HasCounter,
                                    EnableAddFromLOV = a.EnableAddFromLOV,
                                    EnableEditFromLOV = a.EnableEditFromLOV,
                                    IsMain = a.IsMain,
                                    IsRestrictable = a.IsRestrictable,
                                    IsAutoComplete = a.IsAutoComplete,
                                    SortingByObjectField = a.SortingByObjectField,
                                    HasCustomFields = a.HasCustomFields,
                                    CustomFieldsCount = a.CustomFieldsCount,
                                    DescriptionTextCodeId = a.DescriptionTextCodeId,
                                    DescriptionTextCodeCode = a.DescriptionTextCodeCode,
                                    SearchFields = a.SearchFields,
                                    IsSaveButtonVisible = a.IsSaveButtonVisible,
                                    MainTipCode = a.MainTipCode,
                                    EnableSecurity = a.EnableSecurity,
                                    ObjectTableTypeCode = a.ObjectTableTypeCode,
                                    IsComposition = a.IsComposition,
                                    AllowCustomFields = a.AllowCustomFields,
                                    MaxNumberOfCustomFields = a.MaxNumberOfCustomFields,
                                    DBTableName = a.DBTableName,
                                    HasDynamicHeader = a.HasDynamicHeader,
                                    IsLookUp = a.IsLookUp,
                                    HasDocuments = a.HasDocuments,
                                    HasCustomValidator = a.HasCustomValidator,
                                    ClientModuleName = a.ClientModuleName,
                                    ServerModuleName = a.ServerModuleName,
                                    NewWizardComponentPath = a.NewWizardComponentPath,
                                    HasHelper = a.HasHelper,
                                    HasShortTitle = a.HasShortTitle,
                                    HasMenuButtons = a.HasMenuButtons,
                                    HasFiltersMenu = a.HasFiltersMenu,
                                    EntityResourceLastUpdate = a.EntityResourceLastUpdate,
                                    DownloadToExcelFeatureCode = a.DownloadToExcelFeatureCode,
                                    AllowedForComputingPartners = a.AllowedForComputingPartners,
                                    CodeField = a.CodeField,
                                    NameField = a.NameField,
                                    SplitComponentPath = a.SplitComponentPath,
                                    DisableSearchBox = a.DisableSearchBox,
                                    AllowedInTicket = a.AllowedInTicket,
                                    LovDisplayMemberPath = a.LovDisplayMemberPath,
                                    LovDisplayMemberPathLocal = a.LovDisplayMemberPathLocal,
                                    IsTabsHidden = a.IsTabsHidden,
                                    ParentObjectTableName = a.ParentObjectTableName,
                                    AvailableInCustomization = a.AvailableInCustomization,
                                    ParentObjectTableId = a.ParentObjectTableId,
                                    IsCustom = a.IsCustom,
                                    SupportSubEntity = a.SupportSubEntity,
                                    ApplyGenericCustomFields = a.ApplyGenericCustomFields,
                                    FullNameTextCodeId = a.FullNameTextCodeId,
                                    FullNameTextCodeCode = a.FullNameTextCodeCode,
                                    FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : a.Name,
                                    AvailableInDocumentTypes = a.AvailableInDocumentTypes,
                                }).ToList();
            }

            return currentLastUpdates.Concat(zeroLastUpdates).AsQueryable<ObjectTablePM>();
        }        
        public IQueryable<ObjectTablePM> GetObjectPMsByTenant(int tenant)
        {
            List<ObjectTablePM> currentObjectTables = new List<ObjectTablePM>();
            List<ObjectTablePM> zeroObjectTables = new List<ObjectTablePM>();

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
               WebFreightContext webFreightContext = (WebFreightContext)WebFreightContext.GetContext(tenant);

               currentObjectTables = (from a in repository.context.ObjectTables.Include("HeaderScreen").Include("DescriptionTextCode").Include("NewButtonTextCode").Include("FullNameTextCode")
                                       where a.Tenant == tenant
                                       select new ObjectTablePM()
                                       {
                                           NewButtonTextCodeCode = a.NewButtonTextCodeCode,
                                           LookUp1 = a.LookUp1,
                                           LookUp2 = a.LookUp2,
                                           AutoCompleteSearchWindow = a.AutoCompleteSearchWindow,
                                           DependencyFilter1 = a.DependencyFilter1,
                                           DependencyFilter2 = a.DependencyFilter2,
                                           DependencyFilter3 = a.DependencyFilter3,
                                           EditableFromAutoCompleteWindow = a.EditableFromAutoCompleteWindow,
                                           HeaderScreenId = a.HeaderScreenId,
                                           Id = a.Id,
                                           IsClosed = a.IsClosed,
                                           IsNewWizard = a.IsNewWizard,
                                           NewWizardControlName = a.NewWizardControlName,
                                           KeyPropertyPath = a.KeyPropertyPath,
                                           LastUpdateDate = a.LastUpdateDate,
                                           Name = a.Name,
                                           Tenant = a.Tenant,
                                           CacheOnClient = a.CacheOnClient,
                                           HeaderScreenCode = a.HeaderScreenCode,
                                           HasCounter = a.HasCounter,
                                           EnableAddFromLOV = a.EnableAddFromLOV,
                                           EnableEditFromLOV = a.EnableEditFromLOV,
                                           IsMain = a.IsMain,
                                           IsRestrictable = a.IsRestrictable,
                                           IsAutoComplete = a.IsAutoComplete,
                                           SortingByObjectField = a.SortingByObjectField,
                                           HasCustomFields = a.HasCustomFields,
                                           CustomFieldsCount = a.CustomFieldsCount,
                                           DescriptionTextCodeId = a.DescriptionTextCodeId,
                                           DescriptionTextCodeCode = a.DescriptionTextCodeCode,
                                           SearchFields = a.SearchFields,
                                           IsSaveButtonVisible = a.IsSaveButtonVisible,
                                           MainTipCode = a.MainTipCode,
                                           EnableSecurity = a.EnableSecurity,
                                           ObjectTableTypeCode = a.ObjectTableTypeCode,
                                           IsComposition = a.IsComposition,
                                           AllowCustomFields = a.AllowCustomFields,
                                           MaxNumberOfCustomFields = a.MaxNumberOfCustomFields,
                                           DBTableName = a.DBTableName,
                                           HasDynamicHeader = a.HasDynamicHeader,
                                           IsLookUp = a.IsLookUp,
                                           HasDocuments = a.HasDocuments,
                                           HasCustomValidator = a.HasCustomValidator,
                                           ClientModuleName = a.ClientModuleName,
                                           ServerModuleName = a.ServerModuleName,
                                           NewWizardComponentPath = a.NewWizardComponentPath,
                                           HasHelper = a.HasHelper,
                                           HasShortTitle = a.HasShortTitle,
                                           HasMenuButtons = a.HasMenuButtons,
                                           HasFiltersMenu = a.HasFiltersMenu,
                                           EntityResourceLastUpdate = a.EntityResourceLastUpdate,
                                           DownloadToExcelFeatureCode = a.DownloadToExcelFeatureCode,
                                           AllowedForComputingPartners = a.AllowedForComputingPartners,
                                           CodeField = a.CodeField,
                                           NameField = a.NameField,
                                           SplitComponentPath = a.SplitComponentPath,
                                           DisableSearchBox = a.DisableSearchBox,
                                           AllowedInTicket = a.AllowedInTicket,
                                           LovDisplayMemberPath = a.LovDisplayMemberPath,
                                           LovDisplayMemberPathLocal = a.LovDisplayMemberPathLocal,
                                           IsTabsHidden = a.IsTabsHidden,
                                           ParentObjectTableName = a.ParentObjectTableName,
                                           AvailableInCustomization = a.AvailableInCustomization,
                                           ParentObjectTableId = a.ParentObjectTableId,
                                           IsCustom = a.IsCustom,
                                           SupportSubEntity = a.SupportSubEntity,
                                           ApplyGenericCustomFields = a.ApplyGenericCustomFields,
                                           FullNameTextCodeId = a.FullNameTextCodeId,
                                           FullNameTextCodeCode = a.FullNameTextCodeCode,
                                           FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : a.Name,
                                           AvailableInDocumentTypes = a.AvailableInDocumentTypes,
                                       }).ToList();
            }
            if (tenant != 0)
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    zeroObjectTables = GetTenantZeroObjectTables();
                }
            }

            return currentObjectTables.Concat(zeroObjectTables).AsQueryable<ObjectTablePM>();
        }

        private List<ObjectTablePM> GetTenantZeroObjectTables()
        {
            string tenantZeroObjectTablesCacheKeyName = "tenantZeroObjectTables";


            if (HttpContext.Current != null && CacheManager.CacheWrapper.Get(tenantZeroObjectTablesCacheKeyName) != null)
            {
                return (List<ObjectTablePM>)CacheManager.CacheWrapper.Get(tenantZeroObjectTablesCacheKeyName);
            }


            List<ObjectTablePM> zeroObjectTables = (from a in repository.context.ObjectTables.Include("HeaderScreen").Include("DescriptionTextCode").Include("NewButtonTextCode").Include("FullNameTextCode")
                                                    where a.Tenant == 0
                                        select new ObjectTablePM()
                                        {
                                            NewButtonTextCodeCode = a.NewButtonTextCodeCode,
                                            LookUp1 = a.LookUp1,
                                            LookUp2 = a.LookUp2,
                                            AutoCompleteSearchWindow = a.AutoCompleteSearchWindow,
                                            DependencyFilter1 = a.DependencyFilter1,
                                            DependencyFilter2 = a.DependencyFilter2,
                                            DependencyFilter3 = a.DependencyFilter3,
                                            EditableFromAutoCompleteWindow = a.EditableFromAutoCompleteWindow,
                                            HeaderScreenId = a.HeaderScreenId,
                                            Id = a.Id,
                                            IsClosed = a.IsClosed,
                                            IsNewWizard = a.IsNewWizard,
                                            NewWizardControlName = a.NewWizardControlName,
                                            KeyPropertyPath = a.KeyPropertyPath,
                                            LastUpdateDate = a.LastUpdateDate,
                                            Name = a.Name,
                                            Tenant = a.Tenant,
                                            CacheOnClient = a.CacheOnClient,
                                            HeaderScreenCode = a.HeaderScreenCode,
                                            HasCounter = a.HasCounter,
                                            EnableAddFromLOV = a.EnableAddFromLOV,
                                            EnableEditFromLOV = a.EnableEditFromLOV,
                                            IsMain = a.IsMain,
                                            IsRestrictable = a.IsRestrictable,
                                            IsAutoComplete = a.IsAutoComplete,
                                            SortingByObjectField = a.SortingByObjectField,
                                            HasCustomFields = a.HasCustomFields,
                                            CustomFieldsCount = a.CustomFieldsCount,
                                            DescriptionTextCodeId = a.DescriptionTextCodeId,
                                            DescriptionTextCodeCode = a.DescriptionTextCodeCode,
                                            SearchFields = a.SearchFields,
                                            IsSaveButtonVisible = a.IsSaveButtonVisible,
                                            MainTipCode = a.MainTipCode,
                                            EnableSecurity = a.EnableSecurity,
                                            ObjectTableTypeCode = a.ObjectTableTypeCode,
                                            IsComposition = a.IsComposition,
                                            AllowCustomFields = a.AllowCustomFields,
                                            MaxNumberOfCustomFields = a.MaxNumberOfCustomFields,
                                            DBTableName = a.DBTableName,
                                            HasDynamicHeader = a.HasDynamicHeader,
                                            IsLookUp = a.IsLookUp,
                                            HasDocuments = a.HasDocuments,
                                            HasCustomValidator = a.HasCustomValidator,
                                            ClientModuleName = a.ClientModuleName,
                                            ServerModuleName = a.ServerModuleName,
                                            NewWizardComponentPath = a.NewWizardComponentPath,
                                            HasHelper = a.HasHelper,
                                            HasShortTitle = a.HasShortTitle,
                                            HasMenuButtons = a.HasMenuButtons,
                                            HasFiltersMenu = a.HasFiltersMenu,
                                            EntityResourceLastUpdate = a.EntityResourceLastUpdate,
                                            DownloadToExcelFeatureCode = a.DownloadToExcelFeatureCode,
                                            SplitComponentPath = a.SplitComponentPath,
                                            AllowedForComputingPartners = a.AllowedForComputingPartners,
                                            CodeField = a.CodeField,
                                            NameField = a.NameField,
                                            DisableSearchBox = a.DisableSearchBox,
                                            AllowedInTicket = a.AllowedInTicket,
                                            LovDisplayMemberPath = a.LovDisplayMemberPath,
                                            LovDisplayMemberPathLocal = a.LovDisplayMemberPathLocal,
                                            IsTabsHidden = a.IsTabsHidden,
                                            ParentObjectTableName = a.ParentObjectTableName,
                                            AvailableInCustomization = a.AvailableInCustomization,
                                            ParentObjectTableId = a.ParentObjectTableId,
                                            IsCustom = a.IsCustom,
                                            SupportSubEntity = a.SupportSubEntity,
                                            ApplyGenericCustomFields = a.ApplyGenericCustomFields,
                                            FullNameTextCodeId = a.FullNameTextCodeId,
                                            FullNameTextCodeCode = a.FullNameTextCodeCode,
                                            FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : a.Name,
                                            AvailableInDocumentTypes = a.AvailableInDocumentTypes,
                                        }).ToList();

            CacheManager.CacheWrapper.Insert(tenantZeroObjectTablesCacheKeyName, zeroObjectTables, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);


            return zeroObjectTables;
        }

        public static List<ObjectTablePM> GetObjectTablesWithTenantZero(int tenant)
        {
            string entityKeyString = $"GetObjectTablesWithTenantZero({tenant})";
            List<ObjectTablePM> myres = CacheManager.GetOrInsertNewObject<List<ObjectTablePM>>(entityKeyString, () => {

                
                return GetObjectTablesWithTenantZeroBadCache(tenant);
            });
            return myres;
        }
        static List<ObjectTablePM> GetObjectTablesWithTenantZeroBadCache(int tenant)
        {
            
            string listName = "tenantzerotextobjecttablepms";
            string tenantListName = "tenantobjecttablepms" + tenant;

            List<ObjectTablePM> result = new List<ObjectTablePM>();
            List<ObjectTablePM> currentTenantTables = new List<ObjectTablePM>();
            List<ObjectTablePM> zeroTenantTables = new List<ObjectTablePM>();
             
            if (tenant != 0)
            {
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(tenantListName) == null)
                    {
                        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        {
                            IWebFreightContext context = WebFreightContext.GetContext(tenant);
                            currentTenantTables = (from a in context.ObjectTables.Include("HeaderScreen").Include("DescriptionTextCode").Include("NewButtonTextCode").Include("FullNameTextCode")
                                                   where (a.Tenant == tenant && a.InActive == false)
                                                   select new ObjectTablePM()
                                                   {
                                                       NewButtonTextCodeCode = a.NewButtonTextCodeCode,
                                                       LookUp1 = a.LookUp1,
                                                       LookUp2 = a.LookUp2,
                                                       AutoCompleteSearchWindow = a.AutoCompleteSearchWindow,
                                                       DependencyFilter1 = a.DependencyFilter1,
                                                       DependencyFilter2 = a.DependencyFilter2,
                                                       DependencyFilter3 = a.DependencyFilter3,
                                                       EditableFromAutoCompleteWindow = a.EditableFromAutoCompleteWindow,
                                                       HeaderScreenId = a.HeaderScreenId,
                                                       Id = a.Id,
                                                       IsClosed = a.IsClosed,
                                                       IsNewWizard = a.IsNewWizard,
                                                       NewWizardControlName = a.NewWizardControlName,
                                                       KeyPropertyPath = a.KeyPropertyPath,
                                                       LastUpdateDate = a.LastUpdateDate,
                                                       Name = a.Name,
                                                       Tenant = a.Tenant,
                                                       CacheOnClient = a.CacheOnClient,
                                                       HeaderScreenCode = a.HeaderScreenCode,
                                                       HasCounter = a.HasCounter,
                                                       EnableAddFromLOV = a.EnableAddFromLOV,
                                                       EnableEditFromLOV = a.EnableEditFromLOV,
                                                       IsMain = a.IsMain,
                                                       IsRestrictable = a.IsRestrictable,
                                                       IsAutoComplete = a.IsAutoComplete,
                                                       SortingByObjectField = a.SortingByObjectField,
                                                       HasCustomFields = a.HasCustomFields,
                                                       CustomFieldsCount = a.CustomFieldsCount,
                                                       DescriptionTextCodeId = a.DescriptionTextCodeId,
                                                       DescriptionTextCodeCode = a.DescriptionTextCodeCode,
                                                       SearchFields = a.SearchFields,
                                                       IsSaveButtonVisible = a.IsSaveButtonVisible,
                                                       MainTipCode = a.MainTipCode,
                                                       EnableSecurity = a.EnableSecurity,
                                                       ObjectTableTypeCode = a.ObjectTableTypeCode,
                                                       IsComposition = a.IsComposition,
                                                       AllowCustomFields = a.AllowCustomFields,
                                                       MaxNumberOfCustomFields = a.MaxNumberOfCustomFields,
                                                       DBTableName = a.DBTableName,
                                                       HasDynamicHeader = a.HasDynamicHeader,
                                                       IsLookUp = a.IsLookUp,
                                                       HasDocuments = a.HasDocuments,
                                                       HasCustomValidator = a.HasCustomValidator,
                                                       ClientModuleName = a.ClientModuleName,
                                                       ServerModuleName = a.ServerModuleName,
                                                       NewWizardComponentPath = a.NewWizardComponentPath,
                                                       HasHelper = a.HasHelper,
                                                       HasShortTitle = a.HasShortTitle,
                                                       HasMenuButtons = a.HasMenuButtons,
                                                       HasFiltersMenu = a.HasFiltersMenu,
                                                       EntityResourceLastUpdate = a.EntityResourceLastUpdate,
                                                       DownloadToExcelFeatureCode = a.DownloadToExcelFeatureCode,
                                                       AllowedForComputingPartners = a.AllowedForComputingPartners,
                                                       CodeField = a.CodeField,
                                                       NameField = a.NameField,
                                                       SplitComponentPath = a.SplitComponentPath,
                                                       DisableSearchBox = a.DisableSearchBox,
                                                       AllowedInTicket = a.AllowedInTicket,
                                                       LovDisplayMemberPath = a.LovDisplayMemberPath,
                                                       LovDisplayMemberPathLocal = a.LovDisplayMemberPathLocal,
                                                       IsTabsHidden = a.IsTabsHidden,
                                                       ParentObjectTableName = a.ParentObjectTableName,
                                                       AvailableInCustomization = a.AvailableInCustomization,
                                                       SupportSubEntity = a.SupportSubEntity,
                                                       ApplyGenericCustomFields = a.ApplyGenericCustomFields,
                                                       FullNameTextCodeId = a.FullNameTextCodeId,
                                                       FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                       FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : a.Name,
                                                       AvailableInDocumentTypes = a.AvailableInDocumentTypes,
                                                   }).ToList();
                            scope.Complete();
                        }

                        CacheManager.CacheWrapper.Insert(tenantListName, currentTenantTables, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                    }
                    else
                    {
                        currentTenantTables = (List<ObjectTablePM>)CacheManager.CacheWrapper.Get(tenantListName);
                    }
                }
                else
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IWebFreightContext context = WebFreightContext.GetContext(tenant);
                        currentTenantTables = (from a in context.ObjectTables.Include("HeaderScreen").Include("DescriptionTextCode").Include("NewButtonTextCode").Include("FullNameTextCode")
                                               where (a.Tenant == tenant && a.InActive == false)
                                               select new ObjectTablePM()
                                               {
                                                   NewButtonTextCodeCode = a.NewButtonTextCodeCode,
                                                   LookUp1 = a.LookUp1,
                                                   LookUp2 = a.LookUp2,
                                                   AutoCompleteSearchWindow = a.AutoCompleteSearchWindow,
                                                   DependencyFilter1 = a.DependencyFilter1,
                                                   DependencyFilter2 = a.DependencyFilter2,
                                                   DependencyFilter3 = a.DependencyFilter3,
                                                   EditableFromAutoCompleteWindow = a.EditableFromAutoCompleteWindow,
                                                   HeaderScreenId = a.HeaderScreenId,
                                                   Id = a.Id,
                                                   IsClosed = a.IsClosed,
                                                   IsNewWizard = a.IsNewWizard,
                                                   NewWizardControlName = a.NewWizardControlName,
                                                   KeyPropertyPath = a.KeyPropertyPath,
                                                   LastUpdateDate = a.LastUpdateDate,
                                                   Name = a.Name,
                                                   Tenant = a.Tenant,
                                                   CacheOnClient = a.CacheOnClient,
                                                   HeaderScreenCode = a.HeaderScreenCode,
                                                   HasCounter = a.HasCounter,
                                                   EnableAddFromLOV = a.EnableAddFromLOV,
                                                   EnableEditFromLOV = a.EnableEditFromLOV,
                                                   IsMain = a.IsMain,
                                                   IsRestrictable = a.IsRestrictable,
                                                   IsAutoComplete = a.IsAutoComplete,
                                                   SortingByObjectField = a.SortingByObjectField,
                                                   HasCustomFields = a.HasCustomFields,
                                                   CustomFieldsCount = a.CustomFieldsCount,
                                                   DescriptionTextCodeId = a.DescriptionTextCodeId,
                                                   DescriptionTextCodeCode = a.DescriptionTextCodeCode,
                                                   SearchFields = a.SearchFields,
                                                   IsSaveButtonVisible = a.IsSaveButtonVisible,
                                                   MainTipCode = a.MainTipCode,
                                                   EnableSecurity = a.EnableSecurity,
                                                   ObjectTableTypeCode = a.ObjectTableTypeCode,
                                                   IsComposition = a.IsComposition,
                                                   AllowCustomFields = a.AllowCustomFields,
                                                   MaxNumberOfCustomFields = a.MaxNumberOfCustomFields,
                                                   DBTableName = a.DBTableName,
                                                   HasDynamicHeader = a.HasDynamicHeader,
                                                   IsLookUp = a.IsLookUp,
                                                   HasDocuments = a.HasDocuments,
                                                   HasCustomValidator = a.HasCustomValidator,
                                                   ClientModuleName = a.ClientModuleName,
                                                   ServerModuleName = a.ServerModuleName,
                                                   NewWizardComponentPath = a.NewWizardComponentPath,
                                                   HasHelper = a.HasHelper,
                                                   HasShortTitle = a.HasShortTitle,
                                                   HasMenuButtons = a.HasMenuButtons,
                                                   HasFiltersMenu = a.HasFiltersMenu,
                                                   EntityResourceLastUpdate = a.EntityResourceLastUpdate,
                                                   DownloadToExcelFeatureCode = a.DownloadToExcelFeatureCode,
                                                   AllowedForComputingPartners = a.AllowedForComputingPartners,
                                                   CodeField = a.CodeField,
                                                   NameField = a.NameField,
                                                   SplitComponentPath = a.SplitComponentPath,
                                                   DisableSearchBox = a.DisableSearchBox,
                                                   AllowedInTicket = a.AllowedInTicket,
                                                   LovDisplayMemberPath = a.LovDisplayMemberPath,
                                                   LovDisplayMemberPathLocal = a.LovDisplayMemberPathLocal,
                                                   IsTabsHidden = a.IsTabsHidden,
                                                   ParentObjectTableName = a.ParentObjectTableName,
                                                   AvailableInCustomization = a.AvailableInCustomization,
                                                   SupportSubEntity = a.SupportSubEntity,
                                                   ApplyGenericCustomFields = a.ApplyGenericCustomFields,
                                                   FullNameTextCodeId = a.FullNameTextCodeId,
                                                   FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                   FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : a.Name,
                                                   AvailableInDocumentTypes = a.AvailableInDocumentTypes,
                                               }).ToList();
                        scope.Complete();
                    }
                }
            }

            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(listName) == null)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IWebFreightContext context = WebFreightContext.GetContext(0);
                        zeroTenantTables = (from a in context.ObjectTables.Include("HeaderScreen").Include("DescriptionTextCode").Include("NewButtonTextCode").Include("FullNameTextCode")
                                            where (a.Tenant == 0 && a.InActive == false)
                                               select new ObjectTablePM()
                                               {
                                                   NewButtonTextCodeCode = a.NewButtonTextCodeCode,
                                                   LookUp1 = a.LookUp1,
                                                   LookUp2 = a.LookUp2,
                                                   AutoCompleteSearchWindow = a.AutoCompleteSearchWindow,
                                                   DependencyFilter1 = a.DependencyFilter1,
                                                   DependencyFilter2 = a.DependencyFilter2,
                                                   DependencyFilter3 = a.DependencyFilter3,
                                                   EditableFromAutoCompleteWindow = a.EditableFromAutoCompleteWindow,
                                                   HeaderScreenId = a.HeaderScreenId,
                                                   Id = a.Id,
                                                   IsClosed = a.IsClosed,
                                                   IsNewWizard = a.IsNewWizard,
                                                   NewWizardControlName = a.NewWizardControlName,
                                                   KeyPropertyPath = a.KeyPropertyPath,
                                                   LastUpdateDate = a.LastUpdateDate,
                                                   Name = a.Name,                                                   
                                                   Tenant = a.Tenant,
                                                   CacheOnClient = a.CacheOnClient,
                                                   HeaderScreenCode = a.HeaderScreenCode,
                                                   HasCounter = a.HasCounter,
                                                   EnableAddFromLOV = a.EnableAddFromLOV,
                                                   EnableEditFromLOV = a.EnableEditFromLOV,
                                                   IsMain = a.IsMain,
                                                   IsRestrictable = a.IsRestrictable,
                                                   IsAutoComplete = a.IsAutoComplete,
                                                   SortingByObjectField = a.SortingByObjectField,
                                                   HasCustomFields = a.HasCustomFields,
                                                   CustomFieldsCount = a.CustomFieldsCount,
                                                   DescriptionTextCodeId = a.DescriptionTextCodeId,
                                                   DescriptionTextCodeCode = a.DescriptionTextCodeCode,
                                                   SearchFields = a.SearchFields,
                                                   IsSaveButtonVisible = a.IsSaveButtonVisible,
                                                   MainTipCode = a.MainTipCode,
                                                   EnableSecurity = a.EnableSecurity,
                                                   ObjectTableTypeCode = a.ObjectTableTypeCode,
                                                   IsComposition = a.IsComposition,
                                                   AllowCustomFields = a.AllowCustomFields,
                                                   MaxNumberOfCustomFields = a.MaxNumberOfCustomFields,
                                                   DBTableName = a.DBTableName,
                                                   HasDynamicHeader = a.HasDynamicHeader,
                                                   IsLookUp = a.IsLookUp,
                                                   HasDocuments = a.HasDocuments,
                                                   HasCustomValidator = a.HasCustomValidator,
                                                   ClientModuleName = a.ClientModuleName,
                                                   ServerModuleName = a.ServerModuleName,
                                                   NewWizardComponentPath = a.NewWizardComponentPath,
                                                   HasHelper = a.HasHelper,
                                                   HasShortTitle = a.HasShortTitle,
                                                   HasMenuButtons = a.HasMenuButtons,
                                                   HasFiltersMenu = a.HasFiltersMenu,
                                                   EntityResourceLastUpdate = a.EntityResourceLastUpdate,
                                                   DownloadToExcelFeatureCode = a.DownloadToExcelFeatureCode,
                                                   AllowedForComputingPartners = a.AllowedForComputingPartners,
                                                   CodeField = a.CodeField,
                                                   NameField = a.NameField,
                                                   SplitComponentPath = a.SplitComponentPath,
                                                   DisableSearchBox = a.DisableSearchBox,
                                                   AllowedInTicket = a.AllowedInTicket,
                                                   LovDisplayMemberPath = a.LovDisplayMemberPath,
                                                   LovDisplayMemberPathLocal = a.LovDisplayMemberPathLocal,
                                                   IsTabsHidden = a.IsTabsHidden,
                                                   ParentObjectTableName = a.ParentObjectTableName,
                                                   AvailableInCustomization = a.AvailableInCustomization,
                                                   ParentObjectTableId = a.ParentObjectTableId,
                                                   IsCustom = a.IsCustom,
                                                   SupportSubEntity = a.SupportSubEntity,
                                                   ApplyGenericCustomFields = a.ApplyGenericCustomFields,
                                                   FullNameTextCodeId = a.FullNameTextCodeId,
                                                   FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                   FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : a.Name,
                                                   AvailableInDocumentTypes = a.AvailableInDocumentTypes,
                                               }).ToList();

                        scope.Complete();
                    }


                    CacheManager.CacheWrapper.Insert(listName, zeroTenantTables, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    zeroTenantTables = (List<ObjectTablePM>)CacheManager.CacheWrapper.Get(listName);
                }
            }
            else
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    IWebFreightContext context = WebFreightContext.GetContext(0);
                    zeroTenantTables = (from a in context.ObjectTables.Include("HeaderScreen").Include("DescriptionTextCode").Include("NewButtonTextCode").Include("FullNameTextCode")
                                        where (a.Tenant == 0 && a.InActive == false)
                                        select new ObjectTablePM()
                                        {
                                            NewButtonTextCodeCode = a.NewButtonTextCodeCode,
                                            LookUp1 = a.LookUp1,
                                            LookUp2 = a.LookUp2,
                                            AutoCompleteSearchWindow = a.AutoCompleteSearchWindow,
                                            DependencyFilter1 = a.DependencyFilter1,
                                            DependencyFilter2 = a.DependencyFilter2,
                                            DependencyFilter3 = a.DependencyFilter3,
                                            EditableFromAutoCompleteWindow = a.EditableFromAutoCompleteWindow,
                                            HeaderScreenId = a.HeaderScreenId,
                                            Id = a.Id,
                                            IsClosed = a.IsClosed,
                                            IsNewWizard = a.IsNewWizard,
                                            NewWizardControlName = a.NewWizardControlName,
                                            KeyPropertyPath = a.KeyPropertyPath,
                                            LastUpdateDate = a.LastUpdateDate,
                                            Name = a.Name,
                                            Tenant = a.Tenant,
                                            CacheOnClient = a.CacheOnClient,
                                            HeaderScreenCode = a.HeaderScreenCode,
                                            HasCounter = a.HasCounter,
                                            EnableAddFromLOV = a.EnableAddFromLOV,
                                            EnableEditFromLOV = a.EnableEditFromLOV,
                                            IsMain = a.IsMain,
                                            IsRestrictable = a.IsRestrictable,
                                            IsAutoComplete = a.IsAutoComplete,
                                            SortingByObjectField = a.SortingByObjectField,
                                            HasCustomFields = a.HasCustomFields,
                                            CustomFieldsCount = a.CustomFieldsCount,
                                            DescriptionTextCodeId = a.DescriptionTextCodeId,
                                            DescriptionTextCodeCode = a.DescriptionTextCodeCode,
                                            SearchFields = a.SearchFields,
                                            IsSaveButtonVisible = a.IsSaveButtonVisible,
                                            MainTipCode = a.MainTipCode,
                                            EnableSecurity = a.EnableSecurity,
                                            ObjectTableTypeCode = a.ObjectTableTypeCode,
                                            IsComposition = a.IsComposition,
                                            AllowCustomFields = a.AllowCustomFields,
                                            MaxNumberOfCustomFields = a.MaxNumberOfCustomFields,
                                            DBTableName = a.DBTableName,
                                            HasDynamicHeader = a.HasDynamicHeader,
                                            IsLookUp = a.IsLookUp,
                                            HasDocuments = a.HasDocuments,
                                            HasCustomValidator = a.HasCustomValidator,
                                            ClientModuleName = a.ClientModuleName,
                                            ServerModuleName = a.ServerModuleName,
                                            NewWizardComponentPath = a.NewWizardComponentPath,
                                            HasHelper = a.HasHelper,
                                            HasShortTitle = a.HasShortTitle,
                                            HasMenuButtons = a.HasMenuButtons,
                                            HasFiltersMenu = a.HasFiltersMenu,
                                            EntityResourceLastUpdate = a.EntityResourceLastUpdate,
                                            DownloadToExcelFeatureCode = a.DownloadToExcelFeatureCode,
                                            AllowedForComputingPartners = a.AllowedForComputingPartners,
                                            CodeField = a.CodeField,
                                            NameField = a.NameField,
                                            SplitComponentPath = a.SplitComponentPath,
                                            DisableSearchBox = a.DisableSearchBox,
                                            AllowedInTicket = a.AllowedInTicket,
                                            LovDisplayMemberPath = a.LovDisplayMemberPath,
                                            LovDisplayMemberPathLocal = a.LovDisplayMemberPathLocal,
                                            IsTabsHidden = a.IsTabsHidden,
                                            ParentObjectTableName = a.ParentObjectTableName,
                                            AvailableInCustomization = a.AvailableInCustomization,
                                            ParentObjectTableId = a.ParentObjectTableId,
                                            IsCustom = a.IsCustom,
                                            SupportSubEntity = a.SupportSubEntity,
                                            ApplyGenericCustomFields = a.ApplyGenericCustomFields,
                                            FullNameTextCodeId = a.FullNameTextCodeId,
                                            FullNameTextCodeCode = a.FullNameTextCodeCode,
                                            FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : a.Name,
                                            AvailableInDocumentTypes = a.AvailableInDocumentTypes,
                                        }).ToList();


                    scope.Complete();
                }

            }
            zeroTenantTables = zeroTenantTables == null ? new List<ObjectTablePM>() : zeroTenantTables;
            currentTenantTables = currentTenantTables == null ? new List<ObjectTablePM>() : currentTenantTables;

            result = zeroTenantTables.Concat(currentTenantTables).ToList();

            return result;
        }
        public static ObjectTablePM GetObjectTableByCode(string name, int tenant)
        {
            ObjectTablePM table = null;
            if (!string.IsNullOrEmpty(name))
            {
                table = GetObjectTablesWithTenantZero(tenant).Where(t => t.Name.ToLower() == name.ToLower()).FirstOrDefault();
            }

            return table;
        }
        public  ObjectTablePM GetSinglePM(string id, int tenant)
        {
            ObjectTablePM table = null;
            if (!string.IsNullOrEmpty(id))
            {
                table = GetObjectTablesWithTenantZero(tenant).Where(t => t.Id == id).FirstOrDefault();
            }

            return table;
        }        
        public static ObjectTablePM GetSingleObjectTableById(string id, int tenant)
        {
            ObjectTablePM table = null;
            if (!string.IsNullOrEmpty(id))
            {
                table = GetObjectTablesWithTenantZero(tenant).Where(t => t.Id == id).FirstOrDefault();
            }

            return table;
        }
        public ObjectTablePM GetObjectTableByName(string name, int tenant)
        {
            ObjectTablePM table = null;
            if (!string.IsNullOrEmpty(name))
            {
                table = GetObjectTablesWithTenantZero(tenant).Where(t => t.Name.ToLower() == name.ToLower()).FirstOrDefault();
            }

            return table;
        }        
        public ObjectTablePM GetObjectTablePMById(string id, int tenant)
        {
            ObjectTablePM table = null;
            if (!string.IsNullOrEmpty(id))
            {
                table = GetObjectTablesWithTenantZero(tenant).Where(t => t.Id == id).FirstOrDefault();
            }

            return table;
        }        
        public  List<ObjectTablePM> GetSomeObjectTables(int tenant)
        {
            List<ObjectTablePM> tables = null;

          
            tables = GetObjectTablesWithTenantZero(tenant).Where(t => t.Name == "Customs.Declaration" ||
                t.Name == "Customs.Consignment" ||
                t.Name == "Customs.ConsignmentPackage" ||
                t.Name == "Customs.SupplierInvioceItemCertificat" ||
                t.Name == "Customs.SupplierInvoice" ||
                t.Name == "Customs.SupplierInvoiceItem" ||
                t.Name == "Customs.SupplierInvoiceItemsConDeclar" ||
                t.Name == "Customs.SupplierInvoiceItemsDescript" ||
                t.Name == "Customs.SupplierInvoiceItemsMod" ||
                t.Name == "Customs.SupplierInvoiceItemsProdIdent" ||
                t.Name == "Customs.SupplierInvoiceItemsQuantity" ||
                t.Name == "Customs.SupplierInvoiceItemsSerialNum" ||
                t.Name == "Customs.SupplierInvoiceModification" ||
                t.Name == "Customs.ConsignmentInternalTransition" ||
                t.Name == "Customs.DeclarationPayment" ||
                t.Name == "Customs.DeclarationPaymentMethod" ||
                t.Name == "Customs.DeclarationPaymentProtest" ||
                t.Name == "Customs.SupplierInvoiceFreightAmount" ||
                t.Name == "Customs.Claim" || 
                t.Name == "Customs.ClaimsRelatedEntity" ||
                t.Name == "Customs.CourierMaster"||
                 t.Name == "Customs.SupplierInvoicePayment" ||
                   t.Name == "Customs.DeclarationExportRecipient"
                ).ToList();           


            return tables;
        }

        public List<ObjectTablePM> GetSomeExportObjectTables(int tenant)
        {
            List<ObjectTablePM> tables = null;


            tables = GetObjectTablesWithTenantZero(tenant).Where(t => t.Name == "Customs.Declaration" ||
                t.Name == "Customs.Consignment" ||
                t.Name == "Customs.ConsignmentPackage" ||
                t.Name == "Customs.SupplierInvoice" ||
                t.Name == "Customs.SupplierInvoiceItem" ||
                t.Name == "Customs.DeclarationExportRecipient"
                ).ToList();


            return tables;
        }
        public IQueryable<ObjectTableList> GetIQueryableEntityList(IQueryable<ObjectTable> iQueryable)
        {
            IQueryable<ObjectTableList> result = from a in iQueryable.Include("FullNameTextCode")
                                                 select new ObjectTableList()
                                                 {
                                                     AutoCompleteSearchWindow = a.AutoCompleteSearchWindow,
                                                     DependencyFilter1 = a.DependencyFilter1,
                                                     DependencyFilter2 = a.DependencyFilter2,
                                                     DependencyFilter3 = a.DependencyFilter3,
                                                     HeaderScreenId = a.HeaderScreenId,
                                                     HeaderScreenCode=a.HeaderScreenCode,
                                                     Id = a.Id,
                                                     IsClosed = a.IsClosed,
                                                     IsNewWizard = a.IsNewWizard,
                                                     NewWizardControlName = a.NewWizardControlName,
                                                     KeyPropertyPath = a.KeyPropertyPath,
                                                     Name = a.Name,
                                                     Tenant = a.Tenant,
                                                     HasCounter = a.HasCounter,
                                                     EnableAddFromLOV = a.EnableAddFromLOV,
                                                     EnableEditFromLOV = a.EnableEditFromLOV,
                                                     IsMain = a.IsMain,
                                                     IsRestrictable = a.IsRestrictable,
                                                     DescriptionTextCodeId = a.DescriptionTextCodeId,
                                                     DescriptionTextCodeCode = a.DescriptionTextCodeCode,
                                                     SearchFields = a.SearchFields,
                                                     IsSaveButtonVisible = a.IsSaveButtonVisible,
                                                     MainTipCode = a.MainTipCode,
                                                     EnableSecurity = a.EnableSecurity,
                                                     ObjectTableTypeCode = a.ObjectTableTypeCode,
                                                     IsComposition = a.IsComposition,
                                                     AllowCustomFields = a.AllowCustomFields,
                                                     MaxNumberOfCustomFields = a.MaxNumberOfCustomFields,
                                                     DBTableName = a.DBTableName,
                                                     IsLookUp = a.IsLookUp,
                                                     HasDocuments = a.HasDocuments,
                                                     HasCustomValidator = a.HasCustomValidator,
                                                     ClientModuleName = a.ClientModuleName,
                                                     ServerModuleName = a.ServerModuleName,
                                                     NewWizardComponentPath = a.NewWizardComponentPath,
                                                     HasHelper = a.HasHelper,
                                                     HasShortTitle = a.HasShortTitle,
                                                     HasMenuButtons = a.HasMenuButtons,
                                                     HasFiltersMenu = a.HasFiltersMenu,
                                                     SplitComponentPath = a.SplitComponentPath,
                                                     AllowedForComputingPartners = a.AllowedForComputingPartners,
                                                     DisableSearchBox = a.DisableSearchBox,
                                                     AllowedInTicket = a.AllowedInTicket,
                                                     CodeField = a.CodeField,
                                                     NameField = a.NameField,
                                                     LovDisplayMemberPath = a.LovDisplayMemberPath,
                                                     LovDisplayMemberPathLocal = a.LovDisplayMemberPathLocal,
                                                     IsTabsHidden = a.IsTabsHidden,
                                                     ParentObjectTableName = a.ParentObjectTableName,
                                                     AvailableInCustomization = a.AvailableInCustomization,
                                                     ParentObjectTableId = a.ParentObjectTableId,
                                                     IsCustom = a.IsCustom,
                                                     SupportSubEntity = a.SupportSubEntity,
                                                     ApplyGenericCustomFields = a.ApplyGenericCustomFields,
                                                     LookUp1 = a.LookUp1,
                                                     LookUp2 = a.LookUp2,
                                                     FullNameTextCodeId = a.FullNameTextCodeId,
                                                     FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                     FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : a.Name,
                                                     AvailableInDocumentTypes = a.AvailableInDocumentTypes,
                                                 };
            return result;
        }  
        public ObjectTableList GetObjectTableList(string id, int tenant)
        {
            ObjectTableList ObjectTableList = (from a in repository.context.ObjectTables.Include("FullNameTextCode")
                                               where (a.Tenant == tenant || a.Tenant == 0)
                                               && a.Id == id
                                               && a.InActive == false
                                               select new ObjectTableList()
                                               {
                                                   AutoCompleteSearchWindow = a.AutoCompleteSearchWindow,
                                                   DependencyFilter1 = a.DependencyFilter1,
                                                   DependencyFilter2 = a.DependencyFilter2,
                                                   DependencyFilter3 = a.DependencyFilter3,
                                                   HeaderScreenId = a.HeaderScreenId,
                                                   HeaderScreenCode = a.HeaderScreenCode,
                                                   Id = a.Id,
                                                   IsClosed = a.IsClosed,
                                                   IsNewWizard = a.IsNewWizard,
                                                   NewWizardControlName = a.NewWizardControlName,
                                                   KeyPropertyPath = a.KeyPropertyPath,
                                                   Name = a.Name,
                                                   Tenant = a.Tenant,
                                                   HasCounter = a.HasCounter,
                                                   EnableAddFromLOV = a.EnableAddFromLOV,
                                                   EnableEditFromLOV = a.EnableEditFromLOV,
                                                   IsMain = a.IsMain,
                                                   IsRestrictable = a.IsRestrictable,
                                                   DescriptionTextCodeId = a.DescriptionTextCodeId,
                                                   DescriptionTextCodeCode = a.DescriptionTextCodeCode,
                                                   SearchFields = a.SearchFields,
                                                   IsSaveButtonVisible = a.IsSaveButtonVisible,
                                                   MainTipCode = a.MainTipCode,
                                                   EnableSecurity = a.EnableSecurity,
                                                   ObjectTableTypeCode = a.ObjectTableTypeCode,
                                                   IsComposition = a.IsComposition,
                                                   AllowCustomFields = a.AllowCustomFields,
                                                   MaxNumberOfCustomFields = a.MaxNumberOfCustomFields,
                                                   DBTableName = a.DBTableName,
                                                   AllowedForComputingPartners = a.AllowedForComputingPartners,
                                                   CodeField = a.CodeField,
                                                   NameField = a.NameField,
                                                   IsLookUp = a.IsLookUp,
                                                   HasDocuments = a.HasDocuments,
                                                   HasCustomValidator = a.HasCustomValidator,
                                                   ClientModuleName = a.ClientModuleName,
                                                   ServerModuleName = a.ServerModuleName,
                                                   NewWizardComponentPath = a.NewWizardComponentPath,
                                                   HasHelper = a.HasHelper,
                                                   HasShortTitle = a.HasShortTitle,
                                                   HasMenuButtons = a.HasMenuButtons,
                                                   HasFiltersMenu = a.HasFiltersMenu,
                                                   SplitComponentPath = a.SplitComponentPath,
                                                   DisableSearchBox = a.DisableSearchBox,
                                                   AllowedInTicket = a.AllowedInTicket,
                                                   LovDisplayMemberPath = a.LovDisplayMemberPath,
                                                   LovDisplayMemberPathLocal = a.LovDisplayMemberPathLocal,
                                                   IsTabsHidden = a.IsTabsHidden,
                                                   ParentObjectTableName = a.ParentObjectTableName,
                                                   AvailableInCustomization = a.AvailableInCustomization,
                                                   ParentObjectTableId = a.ParentObjectTableId,
                                                   IsCustom = a.IsCustom,
                                                   SupportSubEntity = a.SupportSubEntity,
                                                   ApplyGenericCustomFields = a.ApplyGenericCustomFields,
                                                   FullNameTextCodeId = a.FullNameTextCodeId,
                                                   FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                   FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : a.Name,
                                                   AvailableInDocumentTypes = a.AvailableInDocumentTypes,
                                               }).FirstOrDefault();



            return ObjectTableList;
        } 
        
        public string GetObjectTableIdByName(string tableName)
        {
            return repository.GetObjectTableIdByName(tableName);
        }
    }
}
