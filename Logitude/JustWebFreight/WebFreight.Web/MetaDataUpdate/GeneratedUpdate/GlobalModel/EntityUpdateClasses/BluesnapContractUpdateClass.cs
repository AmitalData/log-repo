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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.GlobalModel.EntityUpdateClasses
{
   public class BluesnapContractUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "BluesnapContract",
			      				    IsNew =  false,
			      				    DBTableName =  "BluesnapContracts",
			      				    OldDBTableName =  "BluesnapContracts",
			      				    ObjectTableSingular =  "Bluesnap Contract",
			      				    ObjectTablePlural =  "Bluesnap Contracts",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  true,
			      				    IsNewWizard =  false,
			      				    LookUp1 =  "Name",
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  true,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  true,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  true,
			      				    EnableEditFromLOV =  true,
			      				    SortingByObjectField =  "Name",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "Bluesnap Contract",
			      				    Code =  "BSCR",
			      				    Name =  "Bluesnap Contract",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Infrastructure",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  true,
			      				    SearchFields =  "BluesnapContract,BluesnapContracts,,Code,Name",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Code",
					  						OldFieldName =  "Code",
					  						ObjectTableName =  "BluesnapContract",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  10,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  10,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Code",
					  						ListPropertyPath =  "Code",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BluesnapContract",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Code",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Code",
					  						DefaultText =  "Code",
					  						ListFieldLable =  "CodeListLable",
					  						ListLableDefaultText =  "Code",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Code",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Name",
					  						OldFieldName =  "Name",
					  						ObjectTableName =  "BluesnapContract",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  50,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  true,
					  						SystemMaxLength =  50,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "Name",
					  						ListPropertyPath =  "Name",
					  						DisplayInLookUpIndex =  1,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  1,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BluesnapContract",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Name",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Name",
					  						DefaultText =  "Name",
					  						ListFieldLable =  "NameListLable",
					  						ListLableDefaultText =  "Name",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Name",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						OldFieldName =  "SearchFields",
					  						ObjectTableName =  "BluesnapContract",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  1000,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1000,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SearchFields",
					  						ListPropertyPath =  "SearchFields",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BluesnapContract",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "SearchFields",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search...",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "SearchFields",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ContractId",
					  						OldFieldName =  "ContractId",
					  						ObjectTableName =  "BluesnapContract",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  true,
					  						SystemMaxLength =  15,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "ContractId",
					  						ListPropertyPath =  "ContractId",
					  						DisplayInLookUpIndex =  2,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  2,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BluesnapContract",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ContractId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContractId",
					  						DefaultText =  "Contract Id",
					  						ListFieldLable =  "ContractIdListLable",
					  						ListLableDefaultText =  "Contract Id",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ContractId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InActive",
					  						OldFieldName =  "InActive",
					  						ObjectTableName =  "BluesnapContract",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
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
					  						PMPropertyPath =  "InActive",
					  						ListPropertyPath =  "InActive",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BluesnapContract",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "InActive",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InActive",
					  						DefaultText =  "Inactive",
					  						ListFieldLable =  "InActiveListLable",
					  						ListLableDefaultText =  "Inactive",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "InActive",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BluesnapContractTypeCode",
					  						OldFieldName =  "BluesnapContractTypeCode",
					  						ObjectTableName =  "BluesnapContract",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "BluesnapContractType",
					  						MinLength =  0,
					  						MaxLength =  4,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "BluesnapContractTypeCode",
					  						ListPropertyPath =  "BluesnapContractTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BluesnapContract",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BluesnapContractTypeCode",
					  						DefaultText =  "Bluesnap Contract Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BluesnapContractTypeName",
					  						OldFieldName =  "BluesnapContractTypeName",
					  						ObjectTableName =  "BluesnapContract",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "BluesnapContractTypeName",
					  						ListPropertyPath =  "BluesnapContractTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BluesnapContract",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BluesnapContractTypeName",
					  						DefaultText =  "Bluesnap Contract Type",
					  						ListFieldLable =  "BluesnapContractTypeNameListLable",
					  						ListLableDefaultText =  "Bluesnap Contract Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup BluesnapContractQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "BSCR", Name = "Bluesnap Contract" }, queryGroupRepository);
						QueryGroup BluesnapContractQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "c493", Name = " Query Group" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable BluesnapContractObjectTable = objectContext.ObjectTables.Where(d => d.Name == "BluesnapContract" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> BluesnapContractObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "BluesnapContract").ToList();   

			   TextCode BluesnapContractTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BluesnapContract.Q.AllBluesnapContracts", DefaultText = @"Bluesnap Contracts",LocalDefaultText = null, ObjectTableId = BluesnapContractObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature BluesnapContractFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLBLUESNAPCONTRACTS", ObjectTableId = BluesnapContractObjectTable.Id, Tenant = 0, NameTextCodeCode = "BluesnapContract.Features.AllBluesnapContracts", NameTextCodeDefaultText = "All Bluesnap Contracts", FeatureTypeCode = "QUER", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AllBluesnapContractQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = BluesnapContractTextCode_0.Id, NameTextCodeCode = BluesnapContractTextCode_0.Code, Code = "All Bluesnap Contract",  QueryGroupCode = "BSCR", IndexOrder = 0, Tenant = 0, ObjectTableId = BluesnapContractObjectTable.Id, QuerySection = "BluesnapContract", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = BluesnapContractFeature_0.Id,FeatureUniqeCode= BluesnapContractFeature_0.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllBluesnapContractQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllBluesnapContractQuery.Id, IndexOrder = 0, ObjectFieldId = BluesnapContractObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == BluesnapContractObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BluesnapContractObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == BluesnapContractObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllBluesnapContractQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllBluesnapContractQuery.Id, IndexOrder = 1, ObjectFieldId = BluesnapContractObjectFields.Where(d => d.FieldName == "BluesnapContractTypeName" && d.ObjectTableId == BluesnapContractObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BluesnapContractObjectFields.Where(d => d.FieldName == "BluesnapContractTypeName" && d.ObjectTableId == BluesnapContractObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllBluesnapContractQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllBluesnapContractQuery.Id, IndexOrder = 2, ObjectFieldId = BluesnapContractObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == BluesnapContractObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BluesnapContractObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == BluesnapContractObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllBluesnapContractQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllBluesnapContractQuery.Id, IndexOrder = 3, ObjectFieldId = BluesnapContractObjectFields.Where(d => d.FieldName == "ContractId" && d.ObjectTableId == BluesnapContractObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BluesnapContractObjectFields.Where(d => d.FieldName == "ContractId" && d.ObjectTableId == BluesnapContractObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllBluesnapContractQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllBluesnapContractQuery.Id, IndexOrder = 4, ObjectFieldId = BluesnapContractObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == BluesnapContractObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BluesnapContractObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == BluesnapContractObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable BluesnapContractObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "BluesnapContract" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> BluesnapContractObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "BluesnapContract").ToList();
		       
	      

	         Screen BluesnapContractHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "BluesnapContract.HeaderScreen", Name = "Header Screen", ObjectTableId = BluesnapContractObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField BluesnapContractBluesnapContractHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = BluesnapContractObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = BluesnapContractHeaderScreenScreen0.Id,ScreenCode = BluesnapContractHeaderScreenScreen0.Code, ObjectFieldCode = BluesnapContractObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField BluesnapContractBluesnapContractHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = BluesnapContractObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().Id, ScreenId = BluesnapContractHeaderScreenScreen0.Id,ScreenCode = BluesnapContractHeaderScreenScreen0.Code, ObjectFieldCode = BluesnapContractObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    BluesnapContractObjectTable.HeaderScreenId = BluesnapContractHeaderScreenScreen0.Id;
		    BluesnapContractObjectTable.HeaderScreenCode = BluesnapContractHeaderScreenScreen0.Code;

	   		  
	      

	         Screen BluesnapContractGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "BluesnapContract.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = BluesnapContractObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 5, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField BluesnapContractBluesnapContractGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = BluesnapContractObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = BluesnapContractGeneralTabScreenScreen1.Id,ScreenCode = BluesnapContractGeneralTabScreenScreen1.Code, ObjectFieldCode = BluesnapContractObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField BluesnapContractBluesnapContractGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = BluesnapContractObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().Id, ScreenId = BluesnapContractGeneralTabScreenScreen1.Id,ScreenCode = BluesnapContractGeneralTabScreenScreen1.Code, ObjectFieldCode = BluesnapContractObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField BluesnapContractBluesnapContractGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = BluesnapContractObjectFields.Where(d => d.FieldName == "ContractId").FirstOrDefault().Id, ScreenId = BluesnapContractGeneralTabScreenScreen1.Id,ScreenCode = BluesnapContractGeneralTabScreenScreen1.Code, ObjectFieldCode = BluesnapContractObjectFields.Where(d => d.FieldName == "ContractId").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField BluesnapContractBluesnapContractGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = BluesnapContractObjectFields.Where(d => d.FieldName == "InActive").FirstOrDefault().Id, ScreenId = BluesnapContractGeneralTabScreenScreen1.Id,ScreenCode = BluesnapContractGeneralTabScreenScreen1.Code, ObjectFieldCode = BluesnapContractObjectFields.Where(d => d.FieldName == "InActive").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField BluesnapContractBluesnapContractGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 4, ObjectFieldId = BluesnapContractObjectFields.Where(d => d.FieldName == "BluesnapContractTypeCode").FirstOrDefault().Id, ScreenId = BluesnapContractGeneralTabScreenScreen1.Id,ScreenCode = BluesnapContractGeneralTabScreenScreen1.Code, ObjectFieldCode = BluesnapContractObjectFields.Where(d => d.FieldName == "BluesnapContractTypeCode").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable BluesnapContractObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "BluesnapContract" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode BluesnapContractGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BluesnapContract.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = BluesnapContractObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature BluesnapContractGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BluesnapContract.Tab.General", ObjectTableId = BluesnapContractObjectTable.Id, Tenant = 0, NameTextCodeCode = "BluesnapContract.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode BluesnapContractEventsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BluesnapContract.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = BluesnapContractObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature BluesnapContractEventsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BluesnapContract.Tab.Events", ObjectTableId = BluesnapContractObjectTable.Id, Tenant = 0, NameTextCodeCode = "BluesnapContract.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "BSGN",HtmlComponentName = "GeneralTabComponent",HtmlComponentUrl = "./Infrastructure/GenericComponents/GeneralTabComponent", FeatureId = BluesnapContractGeneralFeature_TH0.Id, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = BluesnapContractObjectTable.Id, TabNameTextCodeId = BluesnapContractGeneralTextCode_TH0.Id, TabNameTextCodeCode = BluesnapContractGeneralTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "BSEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = BluesnapContractEventsFeature_TH1.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = BluesnapContractObjectTable.Id, TabNameTextCodeId = BluesnapContractEventsTextCode_TH1.Id, TabNameTextCodeCode = BluesnapContractEventsTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable BluesnapContractObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "BluesnapContract" && d.Tenant == 0).FirstOrDefault(); 

		   Feature BluesnapContractFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = BluesnapContractObjectTable.Id, Tenant = 0, NameTextCodeCode = "BluesnapContract.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature BluesnapContractFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = BluesnapContractObjectTable.Id, Tenant = 0, NameTextCodeCode = "BluesnapContract.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature BluesnapContractFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = BluesnapContractObjectTable.Id, Tenant = 0, NameTextCodeCode = "BluesnapContract.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature BluesnapContractFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = BluesnapContractObjectTable.Id, Tenant = 0, NameTextCodeCode = "BluesnapContract.Features.PackageFeature", NameTextCodeDefaultText = "BluesnapContract Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 

		   		   //--------------> Additional Features <--------------\\

		   Feature BluesnapContractFeature_PaymentSettingButton = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PaymentSettingButton", FeatureTypeCode = "ACT", Packagable = false, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = BluesnapContractObjectTable.Id, Tenant = 0, NameTextCodeCode = "BluesnapContract.Features.PaymentSettingButton", NameTextCodeDefaultText = @"Payment Settings Button" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable BluesnapContractObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "BluesnapContract" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPBC",
                EnglishName =  "Bluesnap Contract Updated",
                LocalName =  "Bluesnap Contract Updated",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = BluesnapContractObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRBC",
                EnglishName =  "Created",
                LocalName =  "Created",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = BluesnapContractObjectTable.Id,
				 
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
	 