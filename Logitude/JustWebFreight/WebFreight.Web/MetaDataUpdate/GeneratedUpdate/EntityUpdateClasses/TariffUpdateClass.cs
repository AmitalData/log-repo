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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses
{
   public class TariffUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Tariff",
			      				    IsNew =  true,
			      				    DBTableName =  "Tariffs",
			      				    OldDBTableName =  "Sprints",
			      				    ObjectTableSingular =  "Tariff",
			      				    ObjectTablePlural =  "Tariffs",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  true,
			      				    LookUp1 =  "Name",
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  true,
			      				    IsClosed =  false,
			      				    CacheOnClient =  true,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  true,
			      				    EnableEditFromLOV =  false,
			      				    SortingByObjectField =  "Id",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "BR",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "NewAirFreightCostComponent",
			      				    DefaultText =  "Tariff",
			      				    Code =  "d341",
			      				    Name =  " Query Group",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "TariffModule",
			      				    ServerModuleName =  "TariffModule",
			      				    NewWizardComponentPath =  "./TariffModule/Components/NewEntity/NewAirFreightCostComponent",
			      				    NoTS =  false,
			      				    HasMenuButtons =  true,
			      				    AllowedForComputingPartners =  true,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByUserId",
					  						OldFieldName =  "CreatedByUserId",
					  						ObjectTableName =  "Tariff",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CreatedByUserId",
					  						ListPropertyPath =  "CreatedByUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Tariff",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreatedByUserId",
					  						DefaultText =  "Created By",
					  						ListFieldLable =  "CreatedByUserIdListLable",
					  						ListLableDefaultText =  "Created By",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  true,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByUserId",
					  						OldFieldName =  "UpdatedByUserId",
					  						ObjectTableName =  "Tariff",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UpdatedByUserId",
					  						ListPropertyPath =  "UpdatedByUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Tariff",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdatedByUserId",
					  						DefaultText =  "Updated By",
					  						ListFieldLable =  "UpdatedByUserIdListLable",
					  						ListLableDefaultText =  "Updated By",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  true,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						OldFieldName =  "SearchFields",
					  						ObjectTableName =  "Tariff",
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
					  						ValidForQuerySection1 =  "Tariff",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search Name, Seller, Description and contract #",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "Search ...",
					  						IsMaxLength =  true,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StartDate",
					  						OldFieldName =  "FromDate",
					  						ObjectTableName =  "Tariff",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeNewFormatConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StartDate",
					  						ListPropertyPath =  "StartDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Tariff",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StartDate",
					  						DefaultText =  "Start Date",
					  						ListFieldLable =  "StartDateListLable",
					  						ListLableDefaultText =  "Start Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ExpirationDate",
					  						OldFieldName =  "ToDate",
					  						ObjectTableName =  "Tariff",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeNewFormatConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ExpirationDate",
					  						ListPropertyPath =  "ExpirationDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Tariff",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ExpirationDate",
					  						DefaultText =  "Expiration Date",
					  						ListFieldLable =  "ExpirationDateListLable",
					  						ListLableDefaultText =  "Expiration Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Name",
					  						OldFieldName =  "Name",
					  						ObjectTableName =  "Tariff",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "Name",
					  						ListPropertyPath =  "Name",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Tariff",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
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
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InActive",
					  						OldFieldName =  "InActive",
					  						ObjectTableName =  "Tariff",
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
					  						ValidForQuerySection1 =  "Tariff",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InActive",
					  						DefaultText =  "InActive",
					  						ListFieldLable =  "InActiveListLable",
					  						ListLableDefaultText =  "InActive",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Description",
					  						OldFieldName =  "Description",
					  						ObjectTableName =  "Tariff",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  250,
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
					  						Operator =  "StartsWith",
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Description",
					  						ListPropertyPath =  "Description",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Tariff",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Description",
					  						DefaultText =  "Description",
					  						ListFieldLable =  "DescriptionListLable",
					  						ListLableDefaultText =  "Description",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SellerId",
					  						OldFieldName =  "SellerId",
					  						ObjectTableName =  "Tariff",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SellerId",
					  						ListPropertyPath =  "SellerId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Tariff",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SellerId",
					  						DefaultText =  "Seller ",
					  						ListFieldLable =  "SellerIdListLable",
					  						ListLableDefaultText =  "Seller",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CurrencyId",
					  						OldFieldName =  "CurrencyId",
					  						ObjectTableName =  "Tariff",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Currency",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CurrencyId",
					  						ListPropertyPath =  "CurrencyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Tariff",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CurrencyId",
					  						DefaultText =  "Currency",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreateDate",
					  						OldFieldName =  "CreateDate",
					  						ObjectTableName =  "Tariff",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CreateDate",
					  						ListPropertyPath =  "CreateDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Tariff",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreateDate",
					  						DefaultText =  "Create Date",
					  						ListFieldLable =  "CreateDateListLable",
					  						ListLableDefaultText =  "Create Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdateDate",
					  						OldFieldName =  "UpdateDate",
					  						ObjectTableName =  "Tariff",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UpdateDate",
					  						ListPropertyPath =  "UpdateDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Tariff",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdateDate",
					  						DefaultText =  "Update Date",
					  						ListFieldLable =  "UpdateDateListLable",
					  						ListLableDefaultText =  "Update Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastExpirationDate",
					  						OldFieldName =  "LastExpirationDate",
					  						ObjectTableName =  "Tariff",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LastExpirationDate",
					  						ListPropertyPath =  "LastExpirationDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Tariff",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LastExpirationDate",
					  						DefaultText =  "Last Expiration Date",
					  						ListFieldLable =  "LastExpirationDateListLable",
					  						ListLableDefaultText =  "Last Expiration Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PriceSteps",
					  						OldFieldName =  "PriceSteps",
					  						ObjectTableName =  "Tariff",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PriceSteps",
					  						ListPropertyPath =  "PriceSteps",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Tariff",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PriceSteps",
					  						DefaultText =  "Price Steps",
					  						ListFieldLable =  "PriceStepsListLable",
					  						ListLableDefaultText =  "Price Steps",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TypeCode",
					  						OldFieldName =  "TypeCode",
					  						ObjectTableName =  "Tariff",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "TariffType",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TypeCode",
					  						ListPropertyPath =  "TypeCode",
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
					  						FullFieldLable =  "TypeCode",
					  						DefaultText =  "Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TypeName",
					  						OldFieldName =  "TypeName",
					  						ObjectTableName =  "Tariff",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TypeName",
					  						ListPropertyPath =  "TypeName",
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
					  						FullFieldLable =  "TypeName",
					  						DefaultText =  "Type Name",
					  						ListFieldLable =  "TypeNameListLable",
					  						ListLableDefaultText =  "Type Name",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastStartDate",
					  						OldFieldName =  "LastStartDate",
					  						ObjectTableName =  "Tariff",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LastStartDate",
					  						ListPropertyPath =  "LastStartDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Tariff",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LastStartDate",
					  						DefaultText =  "Last Start Date",
					  						ListFieldLable =  "LastStartDateListLable",
					  						ListLableDefaultText =  "Last Start Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastVersion",
					  						OldFieldName =  "LastVersion",
					  						ObjectTableName =  "Tariff",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LastVersion",
					  						ListPropertyPath =  "LastVersion",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Tariff",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LastVersion",
					  						DefaultText =  "Last Version",
					  						ListFieldLable =  "LastVersionListLable",
					  						ListLableDefaultText =  "Last Version",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ContractNumber",
					  						OldFieldName =  "ContractNumber",
					  						ObjectTableName =  "Tariff",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ContractNumber",
					  						ListPropertyPath =  "ContractNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Tariff",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContractNumber",
					  						DefaultText =  "Contract#",
					  						ListFieldLable =  "ContractNumberListLable",
					  						ListLableDefaultText =  "Contract#",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByUserName",
					  						OldFieldName =  "CreatedByUserName",
					  						ObjectTableName =  "Tariff",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CreatedByUserName",
					  						ListPropertyPath =  "CreatedByUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Tariff",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreatedByUserName",
					  						DefaultText =  "Created By",
					  						ListFieldLable =  "CreatedByUserNameListLable",
					  						ListLableDefaultText =  "Created By",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SellerName",
					  						OldFieldName =  "SellerName",
					  						ObjectTableName =  "Tariff",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SellerName",
					  						ListPropertyPath =  "SellerName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Tariff",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SellerName",
					  						DefaultText =  "Seller",
					  						ListFieldLable =  "SellerNameListLable",
					  						ListLableDefaultText =  "Seller",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByUserName",
					  						OldFieldName =  "UpdatedByUserName",
					  						ObjectTableName =  "Tariff",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UpdatedByUserName",
					  						ListPropertyPath =  "UpdatedByUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Tariff",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdatedByUserName",
					  						DefaultText =  "Updated By",
					  						ListFieldLable =  "UpdatedByUserNameListLable",
					  						ListLableDefaultText =  "Updated By",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SetAsInActive",
					  						OldFieldName =  "SetAsInActive",
					  						ObjectTableName =  "Tariff",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SetAsInActive",
					  						ListPropertyPath =  "SetAsInActive",
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
					  						FullFieldLable =  "SetAsInActive",
					  						DefaultText =  "Set as inactive",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SetAsReActive",
					  						OldFieldName =  "SetAsReActive",
					  						ObjectTableName =  "Tariff",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SetAsReActive",
					  						ListPropertyPath =  "SetAsReActive",
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
					  						FullFieldLable =  "SetAsReActive",
					  						DefaultText =  "Set as reactive",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TariffNumber",
					  						OldFieldName =  "TariffNumber",
					  						ObjectTableName =  "Tariff",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  20,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  20,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TariffNumber",
					  						ListPropertyPath =  "TariffNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Tariff",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TariffNumber",
					  						DefaultText =  "Tariff No.",
					  						ListFieldLable =  "TariffNumberListLable",
					  						ListLableDefaultText =  "Tariff No.",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TariffVersions",
					  						OldFieldName =  "TariffVersions",
					  						ObjectTableName =  "Tariff",
					  						FieldsDataType =  "List",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TariffVersions",
					  						ListPropertyPath =  "TariffVersions",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "TariffVersion",
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
					  						FullFieldLable =  "TariffVersions",
					  						DefaultText =  "Tariff Versions",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TariffLinesAdded",
					  						OldFieldName =  "TariffLinesAdded",
					  						ObjectTableName =  "Tariff",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TariffLinesAdded",
					  						ListPropertyPath =  "TariffLinesAdded",
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
					  						FullFieldLable =  "TariffLinesAdded",
					  						DefaultText =  "TariffLinesAdded",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup TariffQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "d341", Name = " Query Group" }, queryGroupRepository);
						QueryGroup TariffQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "de93", Name = " Query Group" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable TariffObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Tariff" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> TariffObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Tariff").ToList();   

			   TextCode TariffTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Tariff.Q.AirFreightCostTariffs", DefaultText = @"Air Freight Cost Tariffs",LocalDefaultText = "Air Freight Cost Tariffs", ObjectTableId = TariffObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature TariffFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Tariff.Q.AirFreightCostTariffs", ObjectTableId = TariffObjectTable.Id, Tenant = 0, NameTextCodeCode = "TariffFeatures.AirFreightCostTariffs", NameTextCodeDefaultText = "Air Freight Cost Tariffs", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode TariffTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Tariff.Q.AirSurchargesCostTariffs", DefaultText = @"Air Surcharges Cost Tariffs",LocalDefaultText = "Air Surcharges Cost Tariffs", ObjectTableId = TariffObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature TariffFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Tariff.Q.AirSurchargesCostTariffs", ObjectTableId = TariffObjectTable.Id, Tenant = 0, NameTextCodeCode = "TariffFeatures.AirSurchargesCostTariffs", NameTextCodeDefaultText = "Air Surcharges Cost Tariffs", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AirFreightCostTariffsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = TariffTextCode_0.Id, Code = "Air Freight Cost Tariffs",  QueryGroupCode = "d341", IndexOrder = 0, Tenant = 0, ObjectTableId = TariffObjectTable.Id, QuerySection = "Tariff", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = TariffFeature_0.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Desending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AirFreightCostTariffsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirFreightCostTariffsQuery.Id, IndexOrder = 0, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "TariffNumber" && d.ObjectTableId == TariffObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirFreightCostTariffsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirFreightCostTariffsQuery.Id, IndexOrder = 1, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == TariffObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirFreightCostTariffsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirFreightCostTariffsQuery.Id, IndexOrder = 2, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "SellerName" && d.ObjectTableId == TariffObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirFreightCostTariffsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirFreightCostTariffsQuery.Id, IndexOrder = 3, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "StartDate" && d.ObjectTableId == TariffObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirFreightCostTariffsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirFreightCostTariffsQuery.Id, IndexOrder = 4, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "ExpirationDate" && d.ObjectTableId == TariffObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirFreightCostTariffsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirFreightCostTariffsQuery.Id, IndexOrder = 5, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "Description" && d.ObjectTableId == TariffObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirFreightCostTariffsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirFreightCostTariffsQuery.Id, IndexOrder = 6, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == TariffObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirFreightCostTariffsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirFreightCostTariffsQuery.Id, IndexOrder = 7, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == TariffObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirFreightCostTariffsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirFreightCostTariffsQuery.Id, IndexOrder = 8, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "UpdatedByUserName" && d.ObjectTableId == TariffObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirFreightCostTariffsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirFreightCostTariffsQuery.Id, IndexOrder = 9, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == TariffObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter AirFreightCostTariffsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "TypeCode" && d.ObjectTableId == TariffObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "AFC",PredefinedValue2 = null, QueryId = AirFreightCostTariffsQuery.Id, Tenant = 0,Operator = "Equal"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query AirSurchargesCostTariffsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = TariffTextCode_1.Id, Code = "Air Surcharges Cost Tariffs",  QueryGroupCode = "d341", IndexOrder = 1, Tenant = 0, ObjectTableId = TariffObjectTable.Id, QuerySection = "Tariff", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = TariffFeature_1.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Desending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AirSurchargesCostTariffsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirSurchargesCostTariffsQuery.Id, IndexOrder = 0, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == TariffObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirSurchargesCostTariffsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirSurchargesCostTariffsQuery.Id, IndexOrder = 1, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "SellerName" && d.ObjectTableId == TariffObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirSurchargesCostTariffsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirSurchargesCostTariffsQuery.Id, IndexOrder = 2, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "StartDate" && d.ObjectTableId == TariffObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirSurchargesCostTariffsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirSurchargesCostTariffsQuery.Id, IndexOrder = 3, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "ExpirationDate" && d.ObjectTableId == TariffObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirSurchargesCostTariffsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirSurchargesCostTariffsQuery.Id, IndexOrder = 4, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "Description" && d.ObjectTableId == TariffObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirSurchargesCostTariffsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirSurchargesCostTariffsQuery.Id, IndexOrder = 5, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == TariffObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirSurchargesCostTariffsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirSurchargesCostTariffsQuery.Id, IndexOrder = 6, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == TariffObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirSurchargesCostTariffsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirSurchargesCostTariffsQuery.Id, IndexOrder = 7, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "UpdatedByUserName" && d.ObjectTableId == TariffObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirSurchargesCostTariffsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirSurchargesCostTariffsQuery.Id, IndexOrder = 8, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == TariffObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter AirSurchargesCostTariffsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "TypeCode" && d.ObjectTableId == TariffObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "ASC",PredefinedValue2 = null, QueryId = AirSurchargesCostTariffsQuery.Id, Tenant = 0,Operator = "Equal"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable TariffObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Tariff" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> TariffObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Tariff").ToList();
		       
	      

	         Screen TariffTariffHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Tariff.HeaderScreen", Name = "TariffHeaderScreen", ObjectTableId = TariffObjectTable.Id, NumberOfColumns = 4, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField TariffTariffHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().Id, ScreenId = TariffTariffHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField TariffTariffHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "StartDate").FirstOrDefault().Id, ScreenId = TariffTariffHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField TariffTariffHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "SellerId").FirstOrDefault().Id, ScreenId = TariffTariffHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField TariffTariffHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "ExpirationDate").FirstOrDefault().Id, ScreenId = TariffTariffHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField TariffTariffHeaderScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "CurrencyId").FirstOrDefault().Id, ScreenId = TariffTariffHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField TariffTariffHeaderScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 1, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "ContractNumber").FirstOrDefault().Id, ScreenId = TariffTariffHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField TariffTariffHeaderScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = TariffObjectFields.Where(d => d.FieldName == "TariffNumber").FirstOrDefault().Id, ScreenId = TariffTariffHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    TariffObjectTable.HeaderScreenId = TariffTariffHeaderScreenScreen0.Id;
	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable TariffObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Tariff" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode TariffDetailsTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Tariff.TH.Details", DefaultText = "Details",LocalDefaultText = "Details", ObjectTableId = TariffObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature TariffDetailsFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Tariff.Tab.Details", ObjectTableId = TariffObjectTable.Id, Tenant = 0, NameTextCodeCode = "TariffFeatures.TRDT", NameTextCodeDefaultText = "Details", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode TariffGeneralTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Tariff.TH.General", DefaultText = "General",LocalDefaultText = "General", ObjectTableId = TariffObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature TariffGeneralFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Tariff.Tab.General", ObjectTableId = TariffObjectTable.Id, Tenant = 0, NameTextCodeCode = "TariffFeatures.TFGT", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode TariffEventsTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Tariff.TH.Events", DefaultText = "Events",LocalDefaultText = "Events", ObjectTableId = TariffObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature TariffEventsFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Tariff.Tab.Events", ObjectTableId = TariffObjectTable.Id, Tenant = 0, NameTextCodeCode = "TariffFeatures.EVTM", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TRDT",HtmlComponentName = "",HtmlComponentUrl = "./TariffModule/Components/EditTabs/Tariff/TariffDetailsTabComponent", FeatureId = TariffDetailsFeature_TH0.Id, ControlPath = "./TariffModule/Components/EditTabs/Tariff/TariffDetailsTabComponent", ObjectTableId = TariffObjectTable.Id, TabNameTextCodeId = TariffDetailsTextCode_TH0.Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TFGT",HtmlComponentName = "",HtmlComponentUrl = "./TariffModule/Components/EditTabs/Tariff/TariffGeneralTabComponent", FeatureId = TariffGeneralFeature_TH1.Id, ControlPath = "./TariffModule/Components/EditTabs/Tariff/TariffGeneralTabComponent", ObjectTableId = TariffObjectTable.Id, TabNameTextCodeId = TariffGeneralTextCode_TH1.Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "EVTM",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = TariffEventsFeature_TH2.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = TariffObjectTable.Id, TabNameTextCodeId = TariffEventsTextCode_TH2.Id, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable TariffObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Tariff" && d.Tenant == 0).FirstOrDefault(); 

		   Feature TariffFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = TariffObjectTable.Id, Tenant = 0, NameTextCodeCode = "Tariff.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature TariffFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = TariffObjectTable.Id, Tenant = 0, NameTextCodeCode = "Tariff.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature TariffFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = TariffObjectTable.Id, Tenant = 0, NameTextCodeCode = "Tariff.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature TariffFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = TariffObjectTable.Id, Tenant = 0, NameTextCodeCode = "Tariff.Features.PackageFeature", NameTextCodeDefaultText = "Tariff Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 

		   		   //--------------> Additional Features <--------------\\

		   Feature TariffFeature_TARRIFAPPROVEVERSION = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TARRIFAPPROVEVERSION", FeatureTypeCode = "ACT", Packagable = false, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = TariffObjectTable.Id, Tenant = 0, NameTextCodeCode = "Tariff.Features.TARRIFAPPROVEVERSION", NameTextCodeDefaultText = @"Approve Version" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable TariffObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Tariff" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = TariffObjectTable.Id,
				 
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
                ObjectTableId = TariffObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "SAIN",
                EnglishName =  "Set as Inactive",
                LocalName =  "Set as Inactive",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = TariffObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "REAC",
                EnglishName =  "Reactivated",
                LocalName =  "Reactivated",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = TariffObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "TLAD",
                EnglishName =  "Tariff Lines Added",
                LocalName =  "Tariff Lines Added",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = TariffObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "VNAP",
                EnglishName =  "Version Approved",
                LocalName =  "Version Approved",
                IsManualEntry =  false,
                ShortView =  true,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = TariffObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   FeatureRepository featureRepository = new FeatureRepository(0); 
		   //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable TariffObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Tariff" && d.Tenant == 0).FirstOrDefault(); 			   Feature TariffFeature_MB00 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Inactive", ObjectTableId = TariffObjectTable.Id, Tenant = 0, NameTextCodeCode = "Tariff.Features.Inactive", NameTextCodeDefaultText = "Inactive", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature TariffFeature_MB01 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Reactivate", ObjectTableId = TariffObjectTable.Id, Tenant = 0, NameTextCodeCode = "Tariff.Features.Reactivate", NameTextCodeDefaultText = "Reactivate", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
              

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup TariffMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "TariffEdit",
					Name = "TariffEditButtonsGroup",
					ObjectTableId = TariffObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton TariffMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Actions",
						Index = 100, 
						IsActive = false,
						LabelTextCodeCode = "Tariff.B.More",
						LabelTextCodeDefaultText = "More",
						Tenant = 0,
						MenuButtonGroupId = TariffMenuButtonGroup.Id,
						ObjectTableId = TariffObjectTable.Id,
						MenuButtonType = "dropdownbutton",
						FeatureId = null,
						Style = null,
						LocalDefaultText = "More",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

			   MenuButton TariffMenuButton00 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Inactive",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "Tariff.B.Inactive",
						LabelTextCodeDefaultText = "Inactive",
						Tenant = 0,
						MenuButtonGroupId = TariffMenuButtonGroup.Id,
						ParentMenuButtonId = TariffMenuButton0.Id,
						ObjectTableId = TariffObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  TariffFeature_MB00.Id,
						Style = null,
						LocalDefaultText = "Inactive",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton TariffMenuButton01 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Reactivate",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "Tariff.B.Reactivate",
						LabelTextCodeDefaultText = "Reactivate",
						Tenant = 0,
						MenuButtonGroupId = TariffMenuButtonGroup.Id,
						ParentMenuButtonId = TariffMenuButton0.Id,
						ObjectTableId = TariffObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  TariffFeature_MB01.Id,
						Style = null,
						LocalDefaultText = "Reactivate",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {     
	    
}

    

   }
    
}
	 