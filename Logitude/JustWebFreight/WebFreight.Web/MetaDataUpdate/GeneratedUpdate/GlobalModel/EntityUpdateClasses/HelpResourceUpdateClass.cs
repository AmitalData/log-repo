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
using Logitude.CargoTracking.Data.Repositories;
using Logitude.CargoTracking.BL;
using Logitude.CargoTracking.Data.EntityPOCOs;

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.GlobalModel.EntityUpdateClasses
{
   public class HelpResourceUpdateClass
   {  		
		public const string HashString = "9039b0196e2371cf90c7b9524bcec787";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "HelpResource",
			      				    IsNew =  false,
			      				    DBTableName =  "HelpResources",
			      				    ObjectTableSingular =  "Help Resource",
			      				    ObjectTablePlural =  "Help Resources",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  true,
			      				    IsNewWizard =  true,
			      				    KeyPropertyPath =  "Code",
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
			      				    SortingByObjectField =  "Name",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "Help Center",
			      				    Code =  "e64b",
			      				    Name =  "HelpResource",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Infrastructure",
			      				    NewWizardComponentPath =  "./InfrastructureModules/InfrastructureHelpResource/Components/NewHelpResouceComponent",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    SearchFields =  "HelpResource,HelpResources,,Code,",
			      				    HashString =  HelpResourceUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Code",
					  						ObjectTableName =  "HelpResource",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
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
					  						PMPropertyPath =  "Code",
					  						ListPropertyPath =  "Code",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "HelpResource",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
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
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Name",
					  						ObjectTableName =  "HelpResource",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  true,
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
					  						ValidForQuerySection1 =  "HelpResource",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
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
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreateDate",
					  						ObjectTableName =  "HelpResource",
					  						FieldsDataType =  "DateTime",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
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
					  						ValidForQuerySection1 =  "HelpResource",
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
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdateDate",
					  						ObjectTableName =  "HelpResource",
					  						FieldsDataType =  "DateTime",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
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
					  						ValidForQuerySection1 =  "HelpResource",
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
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Language",
					  						ObjectTableName =  "HelpResource",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  true,
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
					  						PMPropertyPath =  "Language",
					  						ListPropertyPath =  "Language",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "HelpResource",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Language",
					  						DefaultText =  "Language",
					  						ListFieldLable =  "LanguageListLable",
					  						ListLableDefaultText =  "Language",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Type",
					  						ObjectTableName =  "HelpResource",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  3,
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
					  						PMPropertyPath =  "Type",
					  						ListPropertyPath =  "Type",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "HelpResource",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Type",
					  						DefaultText =  "Type",
					  						ListFieldLable =  "TypeListLable",
					  						ListLableDefaultText =  "Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Category",
					  						ObjectTableName =  "HelpResource",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  3,
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
					  						PMPropertyPath =  "Category",
					  						ListPropertyPath =  "Category",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "HelpResource",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Category",
					  						DefaultText =  "Category",
					  						ListFieldLable =  "CategoryListLable",
					  						ListLableDefaultText =  "Category",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VideoURL",
					  						ObjectTableName =  "HelpResource",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  250,
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
					  						PMPropertyPath =  "VideoURL",
					  						ListPropertyPath =  "VideoURL",
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
					  						FullFieldLable =  "VideoURL",
					  						DefaultText =  "Video URL",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Duration",
					  						ObjectTableName =  "HelpResource",
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
					  						PMPropertyPath =  "Duration",
					  						ListPropertyPath =  "Duration",
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
					  						FullFieldLable =  "Duration",
					  						DefaultText =  "Duration",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FileName",
					  						ObjectTableName =  "HelpResource",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  true,
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
					  						PMPropertyPath =  "FileName",
					  						ListPropertyPath =  "FileName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "HelpResource",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FileName",
					  						DefaultText =  "File Name",
					  						ListFieldLable =  "FileNameListLable",
					  						ListLableDefaultText =  "FileName",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "HelpResource",
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
					  						SystemMaxLength =  0,
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
					  						ValidForQuerySection1 =  "HelpResource",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search...",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "SearchFields",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsNew",
					  						ObjectTableName =  "HelpResource",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
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
					  						PMPropertyPath =  "IsNew",
					  						ListPropertyPath =  "IsNew",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "HelpResource",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsNew",
					  						DefaultText =  "Is New",
					  						ListFieldLable =  "IsNewListLable",
					  						ListLableDefaultText =  "IsNew",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FeatureCode",
					  						ObjectTableName =  "HelpResource",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FeatureCode",
					  						ListPropertyPath =  "FeatureCode",
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
					  						FullFieldLable =  "FeatureCode",
					  						DefaultText =  "Feature Code",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TypeName",
					  						ObjectTableName =  "HelpResource",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  50,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  50,
					  						DisplayInList =  true,
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
					  						ValidForQuerySection1 =  "HelpResource",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TypeName",
					  						DefaultText =  "Type",
					  						ListFieldLable =  "TypeNameListLable",
					  						ListLableDefaultText =  "Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CategoryName",
					  						ObjectTableName =  "HelpResource",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  50,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  50,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CategoryName",
					  						ListPropertyPath =  "CategoryName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "HelpResource",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CategoryName",
					  						DefaultText =  "Category",
					  						ListFieldLable =  "CategoryNameListLable",
					  						ListLableDefaultText =  "Category",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Inactive",
					  						ObjectTableName =  "HelpResource",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
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
					  						PMPropertyPath =  "Inactive",
					  						ListPropertyPath =  "Inactive",
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "HelpResource",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Inactive",
					  						DefaultText =  "Inactive",
					  						ListFieldLable =  "InactiveListLable",
					  						ListLableDefaultText =  "Inactive",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters,Dictionary<string, QueryGroup> tenantQueryGroups )
	    {  
	        //FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
	        QueryGroup HelpResourceQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "e64b", Name = "HelpResource" }, queryGroupRepository,tenantQueryGroups);
						QueryGroup HelpResourceQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "3bdc", Name = " Query Group" }, queryGroupRepository,tenantQueryGroups);
				        queryGroupRepository.SubmitChanges();
	        ObjectTable HelpResourceObjectTable = objectTables.ContainsKey("HelpResource") ? objectTables["HelpResource"] : null;
            if (HelpResourceObjectTable == null)
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(0);  

                HelpResourceObjectTable = objectContext.ObjectTables.Where(d => d.Name == "HelpResource" && d.Tenant == 0).FirstOrDefault();
            }

	         
			List<Feature> addedFeatures = new List<Feature>();
			List<TextCode> addedTextCodes = new List<TextCode>();
			List<Query> addedQueries = new List<Query>();
			List<QueryColumn> addedQueryColumns = new List<QueryColumn>();
			List<AdvancedQueryFilter> addedQueryFilters = new List<AdvancedQueryFilter>();
   

			   TextCode HelpResourceTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "HelpResource.Q.AllHelpResources", DefaultText = @"Help Resources",LocalDefaultText = "Help Resources", ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature HelpResourceFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "HelpResource.Q.AllHelpResources", ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResourceFeatures.AllHelpResources", NameTextCodeDefaultText = "All Help Resources", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,HelpResourceObjectTable, addedFeatures, addedTextCodes);

	        //TextCodeRepository.SubmitChanges();
	        //FeaturesRepository.SubmitChanges();    
	      

			  Query AllHelpResourcesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = HelpResourceTextCode_0.Id, NameTextCodeCode = HelpResourceTextCode_0.Code, ObjectTableName = "HelpResource", Code = "All Help Resources",  QueryGroupCode = "e64b", IndexOrder = 0, Tenant = 0, ObjectTableId = HelpResourceObjectTable.Id, QuerySection = "HelpResource", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = HelpResourceFeature_0.Id,FeatureUniqeCode= HelpResourceFeature_0.FeatureUniqeCode, DefaultSortName = "Code", DefaultSortDirection = "Ascending", Perspective = null }, addedQueries);
	
			 QueryColumn AllHelpResourcesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllHelpResourcesQuery.Id,QueryCode = AllHelpResourcesQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "HelpResource.Code" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AllHelpResourcesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllHelpResourcesQuery.Id,QueryCode = AllHelpResourcesQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "HelpResource.Name" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn AllHelpResourcesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllHelpResourcesQuery.Id,QueryCode = AllHelpResourcesQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "HelpResource.CreateDate" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn AllHelpResourcesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllHelpResourcesQuery.Id,QueryCode = AllHelpResourcesQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "HelpResource.UpdateDate" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn AllHelpResourcesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllHelpResourcesQuery.Id,QueryCode = AllHelpResourcesQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "HelpResource.Language" , ColumnWidth = 70 }, addedQueryColumns);

			 QueryColumn AllHelpResourcesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllHelpResourcesQuery.Id,QueryCode = AllHelpResourcesQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "HelpResource.FileName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn AllHelpResourcesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllHelpResourcesQuery.Id,QueryCode = AllHelpResourcesQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "HelpResource.TypeName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AllHelpResourcesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllHelpResourcesQuery.Id,QueryCode = AllHelpResourcesQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "HelpResource.CategoryName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AllHelpResourcesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllHelpResourcesQuery.Id,QueryCode = AllHelpResourcesQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "HelpResource.IsNew" , ColumnWidth = 70 }, addedQueryColumns);
			SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
			SqlBulkInsert.BulkInsert("Features", addedFeatures);
			SqlBulkInsert.BulkInsert("Queries", addedQueries);
			SqlBulkInsert.BulkInsert("QueryColumns", addedQueryColumns);
			SqlBulkInsert.BulkInsert("AdvancedQueryFilters", addedQueryFilters);	 
  
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable HelpResourceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "HelpResource" && d.Tenant == 0).FirstOrDefault();
		   //List<ObjectField> HelpResourceObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "HelpResource").ToList();
		       
	      

	         Screen HelpResourceHelpResourceHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "HelpResource.HeaderScreen", Name = "HelpResourceHeaderScreen", ObjectTableId = HelpResourceObjectTable.Id, NumberOfColumns = 3, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
             ScreenField HelpResourceHelpResourceHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ScreenId = HelpResourceHelpResourceHeaderScreenScreen0.Id,ScreenCode = HelpResourceHelpResourceHeaderScreenScreen0.Code, ObjectFieldCode = "HelpResource.Name", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField HelpResourceHelpResourceHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ScreenId = HelpResourceHelpResourceHeaderScreenScreen0.Id,ScreenCode = HelpResourceHelpResourceHeaderScreenScreen0.Code, ObjectFieldCode = "HelpResource.Language", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField HelpResourceHelpResourceHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ScreenId = HelpResourceHelpResourceHeaderScreenScreen0.Id,ScreenCode = HelpResourceHelpResourceHeaderScreenScreen0.Code, ObjectFieldCode = "HelpResource.FileName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          	
		    HelpResourceObjectTable.HeaderScreenId = HelpResourceHelpResourceHeaderScreenScreen0.Id;
		    HelpResourceObjectTable.HeaderScreenCode = HelpResourceHelpResourceHeaderScreenScreen0.Code;

	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable HelpResourceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "HelpResource" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode HelpResourceGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "HelpResource.TH.General", DefaultText = "General",LocalDefaultText = "General", ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature HelpResourceGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "HelpResource.Tab.General", ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResourceFeatures.HRGC", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,HelpResourceObjectTable);
 
                 
			   TextCode HelpResourceEventsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "HelpResource.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature HelpResourceEventsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "HelpResource.Tab.Events", ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResourceFeatures.HREV", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,HelpResourceObjectTable);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "HRGC",HtmlComponentName = "HelpResouceGeneralTabComponent",HtmlComponentUrl = "./InfrastructureModules/InfrastructureHelpResource/Components/HelpResouceGeneralTabComponent", FeatureId = HelpResourceGeneralFeature_TH0.Id,FeatureUniqeCode = HelpResourceGeneralFeature_TH0.FeatureUniqeCode, ControlPath = "./InfrastructureModules/InfrastructureHelpResource/Components/HelpResouceGeneralTabComponent", ObjectTableId = HelpResourceObjectTable.Id, TabNameTextCodeId = HelpResourceGeneralTextCode_TH0.Id, TabNameTextCodeCode = HelpResourceGeneralTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "HREV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = HelpResourceEventsFeature_TH1.Id,FeatureUniqeCode = HelpResourceEventsFeature_TH1.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = HelpResourceObjectTable.Id, TabNameTextCodeId = HelpResourceEventsTextCode_TH1.Id, TabNameTextCodeCode = HelpResourceEventsTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable HelpResourceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "HelpResource" && d.Tenant == 0).FirstOrDefault(); 

		   Feature HelpResourceFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);
		   Feature HelpResourceFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);
		   Feature HelpResourceFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);
		   Feature HelpResourceFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.PackageFeature", NameTextCodeDefaultText = "HelpResource Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable); 

		   		   //--------------> Additional Features <--------------\\

		   Feature HelpResourceFeature_CREATESIGNATURE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CREATESIGNATURE", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.CreateSignature", NameTextCodeDefaultText = @"Create Your Own Signature" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_BUILDCONSOLIDATION = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BUILDCONSOLIDATION", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.BuildConsolidation", NameTextCodeDefaultText = @"Build a Consolidation Shipment" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_GENERICINTERFACE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERICINTERFACE", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.GenericInterface", NameTextCodeDefaultText = @"Generic Invoice Interface" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_MANAGEUSERS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MANAGEUSERS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ManageUsers", NameTextCodeDefaultText = @"Manage Users" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_ADVANCEDWORKBOOK = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ADVANCEDWORKBOOK", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.AdvancedWorkbook", NameTextCodeDefaultText = @"Advanced Features Workbook" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_AWBQUICKTOUR = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AWBQUICKTOUR", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.AWBQuickTour", NameTextCodeDefaultText = @"e-AWB Quick Tour" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_MAILTEMPLATES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MAILTEMPLATES", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.MailTemplates", NameTextCodeDefaultText = @"Managing Mail Templates" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_MANAGECURRENCY = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MANAGECURRENCY", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.CurrencyManagement", NameTextCodeDefaultText = @"Currency Management" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_ANALYZINGCRM = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ANALYZINGCRM", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.AnalyzeCRMData", NameTextCodeDefaultText = @"Analyzing CRM Data" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_MANAGEOPPORTUNITY = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MANAGEOPPORTUNITY", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ManagingOpportunities", NameTextCodeDefaultText = @"Managing Opportunities" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_ACTIVITYWORK = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACTIVITYWORK", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.WorkingActivities", NameTextCodeDefaultText = @"Working With Activities" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_MEASUREMENT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MEASUREMENT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ChangeMeasurement", NameTextCodeDefaultText = @"Change the Units of Measurement" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_AWBTUTORIALSP = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AWBTUTORIALSP", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.AWBTutorialSpanish", NameTextCodeDefaultText = @"e-AWB Tutorial (Spanish)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_GETTINGAROUNDSP = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GETTINGAROUNDSP", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.GettingAroundSpanish", NameTextCodeDefaultText = @"Getting Around in Logitude (Spanish)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_MANAGECUSTOMERS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MANAGECUSTOMERS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ManagingCustomers", NameTextCodeDefaultText = @"Managing Customers" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_OUTLOOKCONNETION = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OUTLOOKCONNETION", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.OutlookConnection", NameTextCodeDefaultText = @"Logitude Outlook Connection" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_CUSTOMROLES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMROLES", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.CustomRoles", NameTextCodeDefaultText = @"Custom Roles" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_AIRLINEACCOUNT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AIRLINEACCOUNT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.AirlineAccountNumber", NameTextCodeDefaultText = @"Airline Account Number" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_MANAGEAWBSTOCK = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MANAGEAWBSTOCK", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ManageAWBStock", NameTextCodeDefaultText = @"Manage AWB Stock by Airlines" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_CANCELINVOICE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CANCELINVOICE", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.CancelInvoice", NameTextCodeDefaultText = @"Cancel an Invoice" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_SHAREDLOGISTICS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHAREDLOGISTICS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.SharedLogistics", NameTextCodeDefaultText = @"Activate Shared Logistics" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_GETTINGAROUND = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GETTINGAROUND", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.GettingAround", NameTextCodeDefaultText = @"Getting Around in Logitude" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_AWBTUTORIAL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AWBTUTORIAL", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.AWBTutorial", NameTextCodeDefaultText = @"e-AWB Tutorial" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_CONSOLINVOICE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CONSOLINVOICE", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ConsolidatedInvoice", NameTextCodeDefaultText = @"Issue a Consolidated Invoice" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_AWBTUTORIALFR = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AWBTUTORIALFR", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.AWBTutorialFrench", NameTextCodeDefaultText = @"e-AWB Tutorial (French)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_RELEASEDEC15 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RELEASEDEC15", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ReleaseDecember2015", NameTextCodeDefaultText = @"December 2015 - Version R5.15" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_RELEASEFEB16 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RELEASEFEB16", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ReleaseFebruary2016", NameTextCodeDefaultText = @"February 2016 - Version R1.16" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_LOGITUDEINTRO = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "LOGITUDEINTRO", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.LogitudeIntroduction", NameTextCodeDefaultText = @"Introduction to Logitude" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_BUILDSHIPMENT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BUILDSHIPMENT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.BuildingShipment", NameTextCodeDefaultText = @"Building a New Shipment" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_ISSUEINVOICE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ISSUEINVOICE", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.IssuingInvoice", NameTextCodeDefaultText = @"Issuing an Invoice" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_BUSINESSTOOLS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BUSINESSTOOLS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.BusinessTools", NameTextCodeDefaultText = @"Business Tools" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_BILLINGTOOLS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BILLINGTOOLS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.BillingTools", NameTextCodeDefaultText = @"Billing & Accounting Tools" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_AWBWORLD = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AWBWORLD", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.LogitudeWorldAWB", NameTextCodeDefaultText = @"Logitude World e-AWB" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_MANAGECUSTOMERSHE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MANAGECUSTOMERSHE", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ManagingCustomersHebrew", NameTextCodeDefaultText = @"Managing Customers (Hebrew)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_RELEASEMAY16 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RELEASEMAY16", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ReleaseMay2016", NameTextCodeDefaultText = @"May 2016 - Version R2.16" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_EBOOKWORK = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EBOOKWORK", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.EBookWork", NameTextCodeDefaultText = @"Working with eBooking" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_CHANGEPASSWORD = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CHANGEPASSWORD", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ChangePassword", NameTextCodeDefaultText = @"How to change Password" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_BLUESNAP = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BLUESNAP", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.BlueSnapSubscribep", NameTextCodeDefaultText = @"Subscribe to Logitude BlueSnap" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_UNIFREIGHTGUIDE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UNIFREIGHTGUIDE", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.UnifreightGuide", NameTextCodeDefaultText = @"Unifreight Mobile - User Guide (Hebrew)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_UNIFREIGHTSHARING = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UNIFREIGHTSHARING", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.UnifreightSharing", NameTextCodeDefaultText = @"Unifreight Mobile - Invitation and Sharing Data (Hebrew)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_OUTLOOKCONNECTIONHEBREW = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OUTLOOKCONNECTIONHEBREW", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.OutlookConnectionHebrew", NameTextCodeDefaultText = @"Logitude Outlook Connection (Hebrew)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_ANALYZINGCRMHEBREW = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ANALYZINGCRMHEBREW", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.AnalyzeCRMDataHebrew", NameTextCodeDefaultText = @"Analyzing CRM Data (Hebrew)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_OUTLOOKINSTALLATIONHEBREW = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OUTLOOKINSTALLATIONHEBREW", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.OutlookInstallationHebrew", NameTextCodeDefaultText = @"Outlook Installation (Hebrew)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_MANAGEOPPORTUNITYHEBREW = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MANAGEOPPORTUNITYHEBREW", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ManagingOpportunitiesHebrew", NameTextCodeDefaultText = @"Managing Opportunities (Hebrew)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_UNIFREIGHTCRMR52015 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UNIFREIGHTCRMR52015", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.UnifreightCRMR52015", NameTextCodeDefaultText = @"Unifreight CRM R5-2015 (Hebrew)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_UNIFREIGHTCRMR12016 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UNIFREIGHTCRMR12016", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.UnifreightCRMR12016", NameTextCodeDefaultText = @"Unifreight CRM R1-2016 (Hebrew)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_UNIFREIGHTCRMR22016 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UNIFREIGHTCRMR22016", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.UnifreightCRMR22016", NameTextCodeDefaultText = @"Unifreight CRM R2-2016 (Hebrew)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_OUTLOOKCONNECTIONSETUP = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OUTLOOKCONNECTIONSETUP", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.OutlookInstallationSetup", NameTextCodeDefaultText = @"Logitude Outlook Connection Setup Guide" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_RELEASEJUL16 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RELEASEJUL16", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ReleaseJul2016", NameTextCodeDefaultText = @"July 2016 - Version R3.16" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_RELEASEOCT16 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RELEASEOCT16", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ReleaseOct2016", NameTextCodeDefaultText = @"October 2016 - Version R4.16" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_LOGITUDEMOBILE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "LOGITUDEMOBILE", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.LogitudeMobile", NameTextCodeDefaultText = @"Logitude Mobile" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_SHAREDLOGISTICSANDMOBILESETUP = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHAREDLOGISTICSANDMOBILESETUP", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.SharedLogisticsMobileSetup", NameTextCodeDefaultText = @"Shared Logistics & Mobile Setup" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_ANALYZINGCRMFRENCH = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ANALYZINGCRMFRENCH", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.AnalyzeCRMDataFrench", NameTextCodeDefaultText = @"Analyzing CRM Data (French)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_MANAGEOPPORTUNITYFRENCH = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MANAGEOPPORTUNITYFRENCH", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ManagingOpportunitiesFrench", NameTextCodeDefaultText = @"Managing Opportunities (French)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_ACTIVITYWORKFRENCH = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACTIVITYWORKFRENCH", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.WorkingActivitiesFrench", NameTextCodeDefaultText = @"Working With Activities (French)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_QUOTESTUTORIAL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QUOTESTUTORIAL", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.QuotesTutorial", NameTextCodeDefaultText = @"Quotes Tutorial" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_RELEASEDEC16 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RELEASEDEC16", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ReleaseDec2016", NameTextCodeDefaultText = @"December 2016 - Version R5.16" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_RELEASEFEB17 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RELEASEFEB17", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ReleaseFeb2017", NameTextCodeDefaultText = @"Feb 2017 - Version R1.17" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_MANAGEUSERFRENCHTUTORIAL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MANAGEUSERFRENCHTUTORIAL", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ManageUserFrenchTutorial", NameTextCodeDefaultText = @"How to manage users (French)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_QUICKBOOKSCONNECTION = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QUICKBOOKSCONNECTION", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.QuickBooksConnection", NameTextCodeDefaultText = @"QuickBooks Online Connection Setup and Activation" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_VATTYPEMANAGEMENT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "VATTYPEMANAGEMENT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.VATTypeManagement", NameTextCodeDefaultText = @"VAT Type Management)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_BUILDCONSOLIDATIONSPANISH = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BUILDCONSOLIDATIONSPANISH", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.BuildConsolidationSpanish", NameTextCodeDefaultText = @"How to Build a Consolidation Shipment (Spanish)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_SHAREDLOGISTICSANDMOBILESPANISH = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHAREDLOGISTICSANDMOBILESPANISH", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.SharedLogisticsMobileSetupSpanish", NameTextCodeDefaultText = @"Shared Logistics & Mobile (Spanish)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_RELEASEMAY17 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RELEASEMAY17", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ReleaseMay2017", NameTextCodeDefaultText = @"May 2017 - Version R2.17" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_RELEASEJUL17 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RELEASEJUL17", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ReleaseJuly2017", NameTextCodeDefaultText = @"July 2017 - Version R3.17" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_FOLLOWUPSTORIAL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "FOLLOWUPSTORIAL", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.FollowUps", NameTextCodeDefaultText = @"Follow Ups" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

		   Feature HelpResourceFeature_HelpResource_M_HelpResources = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "HelpResource.M.HelpResources", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.HelpResource.M.HelpResources", NameTextCodeDefaultText = @"Help Resources" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,HelpResourceObjectTable);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable HelpResourceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "HelpResource" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRHR",
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
                ObjectTableId = HelpResourceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPHR",
                EnglishName =  "Updated",
                LocalName =  "Updated",
                IsManualEntry =  false,
                ShortView =  true,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = HelpResourceObjectTable.Id,
				 
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
	 