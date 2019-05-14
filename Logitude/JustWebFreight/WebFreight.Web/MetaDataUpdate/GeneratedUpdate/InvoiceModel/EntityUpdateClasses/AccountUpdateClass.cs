using System;
using System.Collections.Generic;
using System.Linq;
 
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.GlobalModel;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using Logitude.BL.InfrastructureModel.EntityPMs;
using WebFreight.Web.InvoiceModel;
using WebFreight.Web.MetaDataUpdate.AddClasses;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using WebFreight.Web.QuoteModel;
using WebFreight.Web.ShipmentsModel;
using WebFreight.Web.CommonDataModel.DomainServices;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System.Data.Entity.Core.EntityClient;
using System.Configuration;
using Simplog.Server.Infrastructure;
using System.Data.Common;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL;
using Logitude.CRM.Data.Repsitories;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.BL;
using Logitude.Accounting.Data.Repositories;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.BL;
using Logitude.BookingLib.Data.Repositories;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.BL;
using Logitude.Customs.Data.Repsitories;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.BL;
using Logitude.Social.Data.Repsitories;
using Logitude.Server.Tools.CloseTablesClasses;
using Logitude.Customs.BL.ClosedTable;
using Logitude.CRM.BL.CLoseTable;
using Logitude.BookingLib.BL.CLoseTable;
using Logitude.WarehouseLib.Data.Repositories;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.BL.CLoseTable;
using Logitude.TimeManagement.Data.Repositories;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.BL.CLoseTable;
using Logitude.BL.ShipmentsModel.CloseTables;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.GlobalModel;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL;
using Logitude.TariffModule.Data.Repositories;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.CLoseTable;

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.InvoiceModel.EntityUpdateClasses
{
   public class AccountUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Account",
			      				    DBTableName =  "Accounts",
			      				    ObjectTableSingular =  "Account",
			      				    ObjectTablePlural =  "Accounts",
			      				    DefaultText =  "Account",
			      				    Name =  "Accounts",
			      				    IsNewWizard =  false,
			      				    LookUp1 =  "Code",
			      				    LookUp2 =  "Name",
			      				    DependencyFilter1 =  "AccountTypeCode",
			      				    HasCustomFilter =  false,
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  true,
			      				    IsClosed =  false,
			      				    CacheOnClient =  true,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableEditFromLOV =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  true,
			      				    CustomFieldsCount =  0,
			      				    HasCustomFields =  false,
			      				    InActive =  false,
			      				    SearchFields =  "Account,Accounts,,Id,",
			      				    IsSaveButtonVisible =  true,
			      				    EnableSecurity =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    IsComposition =  false,
			      				    MaxNumberOfCustomFields =  0,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  true,
			      				    IsEditable =  true,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasMenuButtons =  false,
			      				    HasFiltersMenu =  false,
			      				    Code =  "ACCT",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Code",
					  						ObjectTableName =  "Account",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  true,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						DisplayInSearchWindowFilters =  true,
					  						PMPropertyPath =  "Code",
					  						ListPropertyPath =  "Code",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "Account",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "Code",
					  						DefaultText =  "Code",
					  						ListFieldLable =  "CodeListLable",
					  						ListLableDefaultText =  "Code",
					  						HelpTextCode =  "Code",
					  						Code =  "Code",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Name",
					  						ObjectTableName =  "Account",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  30,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  true,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						DisplayInSearchWindowFilters =  true,
					  						PMPropertyPath =  "Name",
					  						ListPropertyPath =  "Name",
					  						DisplayInLookUpIndex =  1,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  1,
					  						DisplayInSearchWindowFiltersIndex =  1,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "Account",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "Name",
					  						DefaultText =  "Name",
					  						ListFieldLable =  "NameListLable",
					  						ListLableDefaultText =  "Name",
					  						HelpTextCode =  "Name",
					  						Code =  "Name",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AccountTypeCode",
					  						ObjectTableName =  "Account",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "AccountType",
					  						DataTypeCode =  "LookUp",
					  						MaxLength =  2,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "AccountTypeCode",
					  						ListPropertyPath =  "AccountTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "Account",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "AccountTypeCode",
					  						DefaultText =  "Account Type",
					  						ListFieldLable =  "AccountTypeCodeListLable",
					  						ListLableDefaultText =  "Account Type Code",
					  						HelpTextCode =  "AccountTypeCode",
					  						Code =  "AccountTypeCode",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ExternalAccountingCard",
					  						ObjectTableName =  "Account",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  25,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "ExternalAccountingCard",
					  						ListPropertyPath =  "ExternalAccountingCard",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "Account",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "ExternalAccountingCard",
					  						DefaultText =  "External Accounting Card",
					  						ListFieldLable =  "ExternalAccountingCardListLable",
					  						ListLableDefaultText =  "External Accounting Card",
					  						HelpTextCode =  "ExternalAccountingCard",
					  						Code =  "ExternalAccountingCard",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InActive",
					  						ObjectTableName =  "Account",
					  						FieldsDataType =  "Boolean",
					  						DataTypeCode =  "Boolean",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "InActive",
					  						ListPropertyPath =  "InActive",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "Account",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "InActive",
					  						DefaultText =  "Inactive Account",
					  						ListFieldLable =  "InActiveListLable",
					  						ListLableDefaultText =  "Inactive",
					  						HelpTextCode =  "InActive",
					  						Code =  "InActive",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AddedManually",
					  						ObjectTableName =  "Account",
					  						FieldsDataType =  "Boolean",
					  						DataTypeCode =  "Boolean",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "AddedManually",
					  						ListPropertyPath =  "AddedManually",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "Account",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "AddedManually",
					  						DefaultText =  "Added manually",
					  						ListFieldLable =  "AddedManuallyListLable",
					  						ListLableDefaultText =  "Added Manually",
					  						HelpTextCode =  "AddedManually",
					  						Code =  "AddedManually",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "Account",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  1000,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "Account",
					  						ValidForQuerySection2 =  "AccountFollowUp",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search codes/ names",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  "Searching by :\n1: code\n2: name",
					  						Code =  "SearchFields",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup AccountQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "ACCT", Name = "Accounts" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable AccountObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Account" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> AccountObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Account").ToList();   

			   TextCode AccountTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Account.Q.AllAccounts", DefaultText = @"All Accounts",LocalDefaultText = null, ObjectTableId = AccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature AccountFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLACCOUNTS", ObjectTableId = AccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "Account.Features.AllAccounts", NameTextCodeDefaultText = "All Accounts", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AllAccountsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = AccountTextCode_0.Id, Code = "All Accounts",  QueryGroupCode = "ACCT", IndexOrder = 0, Tenant = 0, ObjectTableId = AccountObjectTable.Id, QuerySection = "Account", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = AccountFeature_0.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllAccountsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAccountsQuery.Id, IndexOrder = 0, ObjectFieldId = AccountObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == AccountObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllAccountsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAccountsQuery.Id, IndexOrder = 1, ObjectFieldId = AccountObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == AccountObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllAccountsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAccountsQuery.Id, IndexOrder = 2, ObjectFieldId = AccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == AccountObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 50 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllAccountsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAccountsQuery.Id, IndexOrder = 3, ObjectFieldId = AccountObjectFields.Where(d => d.FieldName == "ExternalAccountingCard" && d.ObjectTableId == AccountObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 50 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllAccountsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAccountsQuery.Id, IndexOrder = 4, ObjectFieldId = AccountObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == AccountObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 50 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllAccountsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAccountsQuery.Id, IndexOrder = 5, ObjectFieldId = AccountObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTableId == AccountObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 50 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable AccountObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Account" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> AccountObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Account").ToList();
		       
	      

	         Screen AccountGeneralTabScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Account.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = AccountObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 6, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField AccountAccountGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = AccountObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = AccountGeneralTabScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField AccountAccountGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = AccountObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().Id, ScreenId = AccountGeneralTabScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField AccountAccountGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = AccountObjectFields.Where(d => d.FieldName == "AccountTypeCode").FirstOrDefault().Id, ScreenId = AccountGeneralTabScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField AccountAccountGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = AccountObjectFields.Where(d => d.FieldName == "ExternalAccountingCard").FirstOrDefault().Id, ScreenId = AccountGeneralTabScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField AccountAccountGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = AccountObjectFields.Where(d => d.FieldName == "InActive").FirstOrDefault().Id, ScreenId = AccountGeneralTabScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField AccountAccountGeneralTabScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = AccountObjectFields.Where(d => d.FieldName == "AddedManually").FirstOrDefault().Id, ScreenId = AccountGeneralTabScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           
	      

	         Screen AccountHeaderScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Account.HeaderScreen", Name = "Header Screen", ObjectTableId = AccountObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField AccountAccountHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = AccountObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = AccountHeaderScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField AccountAccountHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = AccountObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().Id, ScreenId = AccountHeaderScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField AccountAccountHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = AccountObjectFields.Where(d => d.FieldName == "AccountTypeCode").FirstOrDefault().Id, ScreenId = AccountHeaderScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField AccountAccountHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = AccountObjectFields.Where(d => d.FieldName == "AddedManually").FirstOrDefault().Id, ScreenId = AccountHeaderScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    AccountObjectTable.HeaderScreenId = AccountHeaderScreenScreen1.Id;
	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable AccountObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Account" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode AccountGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Account.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = AccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature AccountGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Account.Tab.General", ObjectTableId = AccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountFeatures.ACGC", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode AccountEventsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Account.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = AccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature AccountEventsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Account.Tab.Events", ObjectTableId = AccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountFeatures.ACEV", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ACGC",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = AccountGeneralFeature_TH0.Id, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = AccountObjectTable.Id, TabNameTextCodeId = AccountGeneralTextCode_TH0.Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ACEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = AccountEventsFeature_TH1.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = AccountObjectTable.Id, TabNameTextCodeId = AccountEventsTextCode_TH1.Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable AccountObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Account" && d.Tenant == 0).FirstOrDefault(); 

		   Feature AccountFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = AccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "Account.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature AccountFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = AccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "Account.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature AccountFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = AccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "Account.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature AccountFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = AccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "Account.Features.PackageFeature", NameTextCodeDefaultText = "Account Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable AccountObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Account" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPAC",
                ShortView =  false,
                IsManualEntry =  false,
                LocalName =  "Account Updated",
                EnglishName =  "Account Updated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = AccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRAC",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Created",
                EnglishName =  "Created",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = AccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {     
	    
}

    

   }
    
}
	 