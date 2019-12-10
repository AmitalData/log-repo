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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.CommonDataModel.EntityUpdateClasses
{
   public class CustomsShipperUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "CustomsShipper",
			      				    IsNew =  true,
			      				    DBTableName =  "CustomsShippers",
			      				    OldDBTableName =  "CustomsShippers",
			      				    ObjectTableSingular =  "Customs Shipper",
			      				    ObjectTablePlural =  "CustomsShippers",
			      				    HasCustomFilter =  true,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  true,
			      				    IsEditable =  true,
			      				    IsNewWizard =  false,
			      				    LookUp1 =  "Id",
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  false,
			      				    EnableEditFromLOV =  false,
			      				    SortingByObjectField =  "Id",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  false,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "Customs Shipper",
			      				    NewButtonLocalDefaultText =  "",
			      				    NewButtonDefaultText =  "",
			      				    Code =  "3852",
			      				    Name =  "CustomsShipper",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Common",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  true,
			      				    SearchFields =  "CustomsShipper,CustomsShippers,,Id,Id",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						OldFieldName =  "SearchFields",
					  						ObjectTableName =  "CustomsShipper",
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
					  						ValidForQuerySection1 =  "CustomsShipper",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search ...",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "Deposition Number / Shipper Code / English Name",
					  						IsMaxLength =  true,
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
					 
					 						FieldName =  "CustomsShipperCode",
					  						OldFieldName =  "CustomsShipperCode",
					  						ObjectTableName =  "CustomsShipper",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomsShipperCode",
					  						ListPropertyPath =  "CustomsShipperCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsShipper",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomsShipperCode",
					  						DefaultText =  "Shipper Code",
					  						ListFieldLable =  "CustomsShipperCodeListLable",
					  						ListLableDefaultText =  "Shipper Code",
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
					 
					 						FieldName =  "ValidDepositionNumber",
					  						OldFieldName =  "ValidDepositionNumber",
					  						ObjectTableName =  "CustomsShipper",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  20,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ValidDepositionNumber",
					  						ListPropertyPath =  "ValidDepositionNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsShipper",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ValidDepositionNumber",
					  						DefaultText =  "Deposition Number",
					  						ListFieldLable =  "ValidDepositionNumberListLable",
					  						ListLableDefaultText =  "Deposition Number",
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
					 
					 						FieldName =  "ValidityStartDate",
					  						OldFieldName =  "ValidityStartDate",
					  						ObjectTableName =  "CustomsShipper",
					  						FieldsDataType =  "DateTime",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ValidityStartDate",
					  						ListPropertyPath =  "ValidityStartDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsShipper",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ValidityStartDate",
					  						DefaultText =  "Validity Start Date",
					  						ListFieldLable =  "ValidityStartDateListLable",
					  						ListLableDefaultText =  "Start Date",
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
					 
					 						FieldName =  "ValidityEndDate",
					  						OldFieldName =  "ValidityEndDate",
					  						ObjectTableName =  "CustomsShipper",
					  						FieldsDataType =  "DateTime",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ValidityEndDate",
					  						ListPropertyPath =  "ValidityEndDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsShipper",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ValidityEndDate",
					  						DefaultText =  "ValidityEndDate",
					  						ListFieldLable =  "ValidityEndDateListLable",
					  						ListLableDefaultText =  "End Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EnglishName",
					  						OldFieldName =  "EnglishName",
					  						ObjectTableName =  "CustomsShipper",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  70,
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "EnglishName",
					  						ListPropertyPath =  "EnglishName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsShipper",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EnglishName",
					  						DefaultText =  "Name",
					  						ListFieldLable =  "EnglishNameListLable",
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
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ShipperVAT",
					  						OldFieldName =  "VatNumber",
					  						ObjectTableName =  "CustomsShipper",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  20,
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ShipperVAT",
					  						ListPropertyPath =  "ShipperVAT",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsShipper",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ShipperVAT",
					  						DefaultText =  "Vat Number",
					  						ListFieldLable =  "ShipperVATListLable",
					  						ListLableDefaultText =  "Vat Number",
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
					 
					 						FieldName =  "DepositionsDateFilter",
					  						OldFieldName =  "DepositionsDateFilter",
					  						ObjectTableName =  "CustomsShipper",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  150,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DepositionsDateFilter",
					  						ListPropertyPath =  "DepositionsDateFilter",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DepositionsDateFilter",
					  						DefaultText =  "Depositions Date Filter",
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
	        QueryGroup CustomsShipperQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "3852", Name = "CustomsShipper" }, queryGroupRepository);
						QueryGroup CustomsShipperQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "cde1", Name = " Query Group" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable CustomsShipperObjectTable = objectContext.ObjectTables.Where(d => d.Name == "CustomsShipper" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> CustomsShipperObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "CustomsShipper").ToList();   

			   TextCode CustomsShipperTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomsShipper.Q.AllDepositionsQuery", DefaultText = @"All Depositions",LocalDefaultText = "All Depositions", ObjectTableId = CustomsShipperObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CustomsShipperFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CustomsShipper.Q.AllDepositionsQuery", ObjectTableId = CustomsShipperObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsShipperFeatures.AllDepositionsQuery", NameTextCodeDefaultText = "AllDepositionsQuery", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode CustomsShipperTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomsShipper.Q.EndsNext30DaysQuery", DefaultText = @"Ends Next 30 Days",LocalDefaultText = "Ends Next 30 Days", ObjectTableId = CustomsShipperObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CustomsShipperFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CustomsShipper.Q.EndsNext30DaysQuery", ObjectTableId = CustomsShipperObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsShipperFeatures.EndsNext30DaysQuery", NameTextCodeDefaultText = "EndsNext30DaysQuery", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode CustomsShipperTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomsShipper.Q.InValidDepositionsQuery", DefaultText = @"InValid Depositions",LocalDefaultText = "InValid Depositions", ObjectTableId = CustomsShipperObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CustomsShipperFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CustomsShipper.Q.InValidDepositionsQuery", ObjectTableId = CustomsShipperObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsShipperFeatures.InValidDepositionsQuery", NameTextCodeDefaultText = "InValidDepositionsQuery", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AllDepositionsQueryQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CustomsShipperTextCode_0.Id, Code = "AllDepositionsQuery",  QueryGroupCode = "3852", IndexOrder = 0, Tenant = 0, ObjectTableId = CustomsShipperObjectTable.Id, QuerySection = "CustomsShipper", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = CustomsShipperFeature_0.Id, DefaultSortName = "ValidityStartDate", DefaultSortDirection = "Desending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllDepositionsQueryQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllDepositionsQueryQuery.Id, IndexOrder = 0, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "CustomsShipperCode" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "CustomsShipperCode" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllDepositionsQueryQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllDepositionsQueryQuery.Id, IndexOrder = 1, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 300 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllDepositionsQueryQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllDepositionsQueryQuery.Id, IndexOrder = 2, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "ShipperVAT" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "ShipperVAT" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllDepositionsQueryQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllDepositionsQueryQuery.Id, IndexOrder = 3, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "ValidDepositionNumber" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "ValidDepositionNumber" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllDepositionsQueryQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllDepositionsQueryQuery.Id, IndexOrder = 4, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "ValidityStartDate" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "ValidityStartDate" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllDepositionsQueryQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllDepositionsQueryQuery.Id, IndexOrder = 5, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "ValidityEndDate" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "ValidityEndDate" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
  
	      

			  Query EndsNext30DaysQueryQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CustomsShipperTextCode_1.Id, Code = "EndsNext30DaysQuery",  QueryGroupCode = "3852", IndexOrder = 1, Tenant = 0, ObjectTableId = CustomsShipperObjectTable.Id, QuerySection = "CustomsShipper", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = CustomsShipperFeature_1.Id, DefaultSortName = "ValidityStartDate", DefaultSortDirection = "Desending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn EndsNext30DaysQueryQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = EndsNext30DaysQueryQuery.Id, IndexOrder = 0, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "CustomsShipperCode" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "CustomsShipperCode" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn EndsNext30DaysQueryQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = EndsNext30DaysQueryQuery.Id, IndexOrder = 1, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 300 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn EndsNext30DaysQueryQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = EndsNext30DaysQueryQuery.Id, IndexOrder = 2, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "ShipperVAT" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "ShipperVAT" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn EndsNext30DaysQueryQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = EndsNext30DaysQueryQuery.Id, IndexOrder = 3, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "ValidDepositionNumber" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "ValidDepositionNumber" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn EndsNext30DaysQueryQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = EndsNext30DaysQueryQuery.Id, IndexOrder = 4, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "ValidityStartDate" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "ValidityStartDate" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn EndsNext30DaysQueryQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = EndsNext30DaysQueryQuery.Id, IndexOrder = 5, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "ValidityEndDate" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "ValidityEndDate" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter EndsNext30DaysQueryQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "DepositionsDateFilter" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "DepositionsDateFilter" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "EndNext30Days",PredefinedValue2 = null, QueryId = EndsNext30DaysQueryQuery.Id, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query InValidDepositionsQueryQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CustomsShipperTextCode_2.Id, Code = "InValidDepositionsQuery",  QueryGroupCode = "3852", IndexOrder = 2, Tenant = 0, ObjectTableId = CustomsShipperObjectTable.Id, QuerySection = "CustomsShipper", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = CustomsShipperFeature_2.Id, DefaultSortName = "ValidityStartDate", DefaultSortDirection = "Desending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn InValidDepositionsQueryQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InValidDepositionsQueryQuery.Id, IndexOrder = 0, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "CustomsShipperCode" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "CustomsShipperCode" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InValidDepositionsQueryQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InValidDepositionsQueryQuery.Id, IndexOrder = 1, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 300 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InValidDepositionsQueryQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InValidDepositionsQueryQuery.Id, IndexOrder = 2, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "ShipperVAT" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "ShipperVAT" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InValidDepositionsQueryQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InValidDepositionsQueryQuery.Id, IndexOrder = 3, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "ValidDepositionNumber" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "ValidDepositionNumber" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InValidDepositionsQueryQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InValidDepositionsQueryQuery.Id, IndexOrder = 4, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "ValidityStartDate" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "ValidityStartDate" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InValidDepositionsQueryQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InValidDepositionsQueryQuery.Id, IndexOrder = 5, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "ValidityEndDate" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "ValidityEndDate" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter InValidDepositionsQueryQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "DepositionsDateFilter" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "DepositionsDateFilter" && d.ObjectTableId == CustomsShipperObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "InValidDepositions",PredefinedValue2 = null, QueryId = InValidDepositionsQueryQuery.Id, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable CustomsShipperObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomsShipper" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> CustomsShipperObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "CustomsShipper").ToList();
		       
	      

	         Screen CustomsShipperCustomsShipperHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CustomsShipper.HeaderScreen", Name = "CustomsShipperHeaderScreen", ObjectTableId = CustomsShipperObjectTable.Id, NumberOfColumns = 3, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField CustomsShipperCustomsShipperHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "CustomsShipperCode").FirstOrDefault().Id, ScreenId = CustomsShipperCustomsShipperHeaderScreenScreen0.Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "CustomsShipperCode").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomsShipperCustomsShipperHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().Id, ScreenId = CustomsShipperCustomsShipperHeaderScreenScreen0.Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomsShipperCustomsShipperHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = CustomsShipperObjectFields.Where(d => d.FieldName == "ValidDepositionNumber").FirstOrDefault().Id, ScreenId = CustomsShipperCustomsShipperHeaderScreenScreen0.Id, ObjectFieldCode = CustomsShipperObjectFields.Where(d => d.FieldName == "ValidDepositionNumber").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    CustomsShipperObjectTable.HeaderScreenId = CustomsShipperCustomsShipperHeaderScreenScreen0.Id;
	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable CustomsShipperObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomsShipper" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode CustomsShipperGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomsShipper.TH.General", DefaultText = "General",LocalDefaultText = "General", ObjectTableId = CustomsShipperObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomsShipperGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CustomsShipper.Tab.General", ObjectTableId = CustomsShipperObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsShipperFeatures.CSGE", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CSGE",HtmlComponentName = "CustomsShipperGeneralTabComponent",HtmlComponentUrl = "./CommonModules/CommonOthers/Components/Depositions/EditTab/CustomsShipperGeneralTabComponent", FeatureId = CustomsShipperGeneralFeature_TH0.Id, ControlPath = "./CommonModules/CommonOthers/Components/Depositions/EditTab/CustomsShipperGeneralTabComponent", ObjectTableId = CustomsShipperObjectTable.Id, TabNameTextCodeId = CustomsShipperGeneralTextCode_TH0.Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable CustomsShipperObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomsShipper" && d.Tenant == 0).FirstOrDefault(); 

		   Feature CustomsShipperFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = CustomsShipperObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsShipper.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CustomsShipperFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = CustomsShipperObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsShipper.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CustomsShipperFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = CustomsShipperObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsShipper.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CustomsShipperFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = CustomsShipperObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsShipper.Features.PackageFeature", NameTextCodeDefaultText = "CustomsShipper Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable CustomsShipperObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomsShipper" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CREV",
                EnglishName =  "Created",
                LocalName =  "Created",
                IsManualEntry =  false,
                ShortView =  true,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomsShipperObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPEV",
                EnglishName =  "Updated",
                LocalName =  "Updated",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomsShipperObjectTable.Id,
				 
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
	 