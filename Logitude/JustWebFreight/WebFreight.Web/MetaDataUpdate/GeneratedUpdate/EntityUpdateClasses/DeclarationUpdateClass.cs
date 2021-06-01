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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses
{
   public class DeclarationUpdateClass
   {  		
		public const string HashString = "16ed35d97fca300054f6538af540cfda";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Customs.Declaration",
			      				    IsNew =  false,
			      				    DBTableName =  "Customs.Declarations",
			      				    ObjectTableSingular =  "Declaration",
			      				    ObjectTablePlural =  "Declarations",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  true,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  true,
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  true,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  false,
			      				    EnableEditFromLOV =  false,
			      				    SortingByObjectField =  "TaxationDateTime",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "Logitude.Customs.NewDeclarationControlCommand",
			      				    LocalDefaultText =  "הצהרות יבוא",
			      				    DefaultText =  "Declaration",
			      				    Code =  "DECL",
			      				    Name =  "Customs.CourierMaster",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Customs",
			      				    ServerModuleName =  "Customs",
			      				    NewWizardComponentPath =  "./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/NewEntity/NewDeclarationComponent",
			      				    NoTS =  false,
			      				    HasMenuButtons =  true,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    HashString =  DeclarationUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomFileNo",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  12,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  12,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomFileNo",
					  						ListPropertyPath =  "CustomFileNo",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomFileNo",
					  						DefaultText =  "Custom File No",
					  						FullLocalDefaultText =  "תיק עמילות",
					  						ListFieldLable =  "CustomFileNoListLable",
					  						ListLableDefaultText =  "Custom File No",
					  						ListLocalDefaultText =  "תיק עמילות",
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
					 
					 						FieldName =  "CustomerId",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomerId",
					  						ListPropertyPath =  "CustomerId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerId",
					  						DefaultText =  "Customer ",
					  						FullLocalDefaultText =  "לקוח",
					  						ListFieldLable =  "CustomerIdListLable",
					  						ListLableDefaultText =  "Customer ",
					  						ListLocalDefaultText =  "לקוח",
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
					 
					 						FieldName =  "ImporterId",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.Client",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ImporterId",
					  						ListPropertyPath =  "ImporterId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ImporterId",
					  						DefaultText =  "Importer Id",
					  						FullLocalDefaultText =  "מספר יבואן",
					  						ListFieldLable =  "ImporterIdListLable",
					  						ListLableDefaultText =  "Importer",
					  						ListLocalDefaultText =  "מספר יבואן",
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
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  40,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
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
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						ValidForQuerySection2 =  "Customs.DeclarationFollowUp",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search",
					  						FullLocalDefaultText =  "תיק/הצהרה/מזהה מטען/לקוח",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "SearchField",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  "Searching by :\n1: Tenant \n2: File No \n3: Custom File No",
					  						HelpLocalDefaultText =  "חיפוש על ידי: \n1: הדייר \n2: קובץ לא \n3: קובץ מותאם אישית לא",
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
					 
					 						FieldName =  "DeclarationNumber",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  35,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  35,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeclarationNumber",
					  						ListPropertyPath =  "DeclarationNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationNumber",
					  						DefaultText =  "Declaration Number",
					  						FullLocalDefaultText =  "מספר הצהרה",
					  						ListFieldLable =  "DeclarationNumberListLable",
					  						ListLableDefaultText =  "Declaration Number",
					  						ListLocalDefaultText =  "מספר הצהרה",
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
					 
					 						FieldName =  "CustomerName",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  60,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomerName",
					  						ListPropertyPath =  "CustomerName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerName",
					  						DefaultText =  "Customer Name",
					  						FullLocalDefaultText =  "לקוח",
					  						ListFieldLable =  "CustomerNameListLable",
					  						ListLableDefaultText =  "Customer Name",
					  						ListLocalDefaultText =  "לקוח",
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
					 
					 						FieldName =  "VersionId",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  9,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  9,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "VersionId",
					  						ListPropertyPath =  "VersionId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VersionId",
					  						DefaultText =  "Version ",
					  						FullLocalDefaultText =  "מספר גירסה להצהרה",
					  						ListFieldLable =  "VersionIdListLable",
					  						ListLableDefaultText =  "Version ",
					  						ListLocalDefaultText =  "מספר גירסה להצהרה",
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
					 
					 						FieldName =  "ExternalDeclarationNumber",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  35,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  35,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ExternalDeclarationNumber",
					  						ListPropertyPath =  "ExternalDeclarationNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ExternalDeclarationNumber",
					  						DefaultText =  "External Declaration Number",
					  						FullLocalDefaultText =  "מזהה רשומת סוכן",
					  						ListFieldLable =  "ExternalDeclarationNumberListLable",
					  						ListLableDefaultText =  "External Declaration Number",
					  						ListLocalDefaultText =  "מזהה רשומת סוכן",
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
					 
					 						FieldName =  "DeclarationOfficeCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CustomsHouseType",
					  						MinLength =  0,
					  						MaxLength =  17,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  17,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeclarationOfficeCode",
					  						ListPropertyPath =  "DeclarationOfficeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationOfficeCode",
					  						DefaultText =  "Declaration Office",
					  						FullLocalDefaultText =  "בית מכס",
					  						ListFieldLable =  "DeclarationOfficeCodeListLable",
					  						ListLableDefaultText =  "Declaration Office",
					  						ListLocalDefaultText =  "בית מכס",
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
					 
					 						FieldName =  "TaxationDateTime",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TaxationDateTime",
					  						ListPropertyPath =  "TaxationDateTime",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TaxationDateTime",
					  						DefaultText =  "Taxation Date Time",
					  						FullLocalDefaultText =  "תאריך חישוב מיסים  ",
					  						ListFieldLable =  "TaxationDateTimeListLable",
					  						ListLableDefaultText =  "Taxation Date Time",
					  						ListLocalDefaultText =  "תאריך חישוב מיסים  ",
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
					 
					 						FieldName =  "AgentId",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  9,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  9,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AgentId",
					  						ListPropertyPath =  "AgentId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AgentId",
					  						DefaultText =  "Agent ",
					  						FullLocalDefaultText =  "מספר סוכן",
					  						ListFieldLable =  "AgentIdListLable",
					  						ListLableDefaultText =  "Agent",
					  						ListLocalDefaultText =  "מספר סוכן",
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
					 
					 						FieldName =  "ProcedureCurrentCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.GovernmentProcedureType",
					  						MinLength =  0,
					  						MaxLength =  7,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  7,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ProcedureCurrentCode",
					  						ListPropertyPath =  "ProcedureCurrentCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ProcedureCurrentCode",
					  						DefaultText =  "Government Procedure Type",
					  						FullLocalDefaultText =  "סוג תהליך",
					  						ListFieldLable =  "ProcedureCurrentCodeListLable",
					  						ListLableDefaultText =  "Procedure Current ",
					  						ListLocalDefaultText =  "סוג תהליך",
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
					 
					 						FieldName =  "ProcedureCurrentName",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  300,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  300,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ProcedureCurrentName",
					  						ListPropertyPath =  "ProcedureCurrentName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ProcedureCurrentName",
					  						DefaultText =  "Government Procedure Type",
					  						FullLocalDefaultText =  "סוג תהליך",
					  						ListFieldLable =  "ProcedureCurrentNameListLable",
					  						ListLableDefaultText =  "Procedure Current Name",
					  						ListLocalDefaultText =  "סוג תהליך ",
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
					 
					 						FieldName =  "AutonomyRegionTypeCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.AutonomyType",
					  						MinLength =  0,
					  						MaxLength =  4,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  4,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AutonomyRegionTypeCode",
					  						ListPropertyPath =  "AutonomyRegionTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AutonomyRegionTypeCode",
					  						DefaultText =  "Autonomy Region Type",
					  						FullLocalDefaultText =  "קוד איזור אוטונומיה",
					  						ListFieldLable =  "AutonomyRegionTypeCodeListLable",
					  						ListLableDefaultText =  "Autonomy Region Type ",
					  						ListLocalDefaultText =  "קוד איזור אוטונומיה",
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
					 
					 						FieldName =  "AutonomyRegionTypeName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AutonomyRegionType",
					  						ListPropertyPath =  "AutonomyRegionType",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AutonomyRegionTypeName",
					  						DefaultText =  "Autonomy Region Type Name",
					  						FullLocalDefaultText =  "שם סוג אזור האוטונומיה",
					  						ListFieldLable =  "AutonomyRegionTypeNameListLable",
					  						ListLableDefaultText =  "Autonomy Region Type Name",
					  						ListLocalDefaultText =  "סוג אזור אוטונומיה",
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
					 
					 						FieldName =  "ImporterPassCountryCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CustomsCountry",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  2,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ImporterPassCountryCode",
					  						ListPropertyPath =  "ImporterPassCountryCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ImporterPassCountryCode",
					  						DefaultText =  "Importer Pass Country",
					  						FullLocalDefaultText =  "מדינת דרכון יבואן",
					  						ListFieldLable =  "ImporterPassCountryCodeListLable",
					  						ListLableDefaultText =  "Importer Pass Country ",
					  						ListLocalDefaultText =  "מדינת דרכון יבואן",
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
					 
					 						FieldName =  "ImporterPassCountryName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ImporterPassCountry",
					  						ListPropertyPath =  "ImporterPassCountry",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ImporterPassCountryName",
					  						DefaultText =  "Importer Pass Country Name",
					  						FullLocalDefaultText =  "מדינה שם יבואן יבואן",
					  						ListFieldLable =  "ImporterPassCountryNameListLable",
					  						ListLableDefaultText =  "Importer Pass Country Name",
					  						ListLocalDefaultText =  "המדינה יבואן",
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
					 
					 						FieldName =  "TransferImporterId",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.Client",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TransferImporterId",
					  						ListPropertyPath =  "TransferImporterId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransferImporterId",
					  						DefaultText =  "Transfer Importer Id",
					  						FullLocalDefaultText =  "מספר יבואן מעביר",
					  						ListFieldLable =  "TransferImporterIdListLable",
					  						ListLableDefaultText =  "Transfer Importer ",
					  						ListLocalDefaultText =  "מספר יבואן מעביר",
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
					 
					 						FieldName =  "TransferImporterCountryCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CustomsCountry",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  2,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TransferImporterCountryCode",
					  						ListPropertyPath =  "TransferImporterCountryCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransferImporterCountryCode",
					  						DefaultText =  "Transfer Importer Country",
					  						FullLocalDefaultText =  "מדינת דרכון יבואן מעביר",
					  						ListFieldLable =  "TransferImporterCountryCodeListLable",
					  						ListLableDefaultText =  "Transfer Importer Country ",
					  						ListLocalDefaultText =  "מדינת דרכון יבואן מעביר",
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
					 
					 						FieldName =  "TransferImporterCountryName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TransferImporterCountry",
					  						ListPropertyPath =  "TransferImporterCountry",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransferImporterCountryName",
					  						DefaultText =  "Transfer Importer Country Name",
					  						FullLocalDefaultText =  "מדינה שם יבואן ההעברה",
					  						ListFieldLable =  "TransferImporterCountryNameListLable",
					  						ListLableDefaultText =  "Transfer Importer Country Name",
					  						ListLocalDefaultText =  "העבר את המדינה יבואן",
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
					 
					 						FieldName =  "EntitleImporterId",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "EntitleImporterId",
					  						ListPropertyPath =  "EntitleImporterId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EntitleImporterId",
					  						DefaultText =  "Entitle Importer Id",
					  						FullLocalDefaultText =  "מספר יבואן זכאי",
					  						ListFieldLable =  "EntitleImporterIdListLable",
					  						ListLableDefaultText =  "Entitle Importer ",
					  						ListLocalDefaultText =  "מספר יבואן זכאי",
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
					 
					 						FieldName =  "ImporterEntitlementTypeCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.EntitlementType",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  3,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ImporterEntitlementTypeCode",
					  						ListPropertyPath =  "ImporterEntitlementTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ImporterEntitlementTypeCode",
					  						DefaultText =  "Importer Entitlement Type",
					  						FullLocalDefaultText =  "סוג זכאות יבואן זכאי",
					  						ListFieldLable =  "ImporterEntitlementTypeCodeListLable",
					  						ListLableDefaultText =  "Importer Entitlement Type ",
					  						ListLocalDefaultText =  "סוג זכאות יבואן זכאי",
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
					 
					 						FieldName =  "ImporterEntitlementTypeName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ImporterEntitlementType",
					  						ListPropertyPath =  "ImporterEntitlementType",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ImporterEntitlementTypeName",
					  						DefaultText =  "Importer Entitlement Type Name",
					  						FullLocalDefaultText =  "מדינה שם יבואן ההעברה",
					  						ListFieldLable =  "ImporterEntitlementTypeNameListLable",
					  						ListLableDefaultText =  "Importer Entitlement Type Name",
					  						ListLocalDefaultText =  "סוג זכאות יבואן",
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
					 
					 						FieldName =  "EntitleImporterCountryCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CustomsCountry",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  2,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "EntitleImporterCountryCode",
					  						ListPropertyPath =  "EntitleImporterCountryCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EntitleImporterCountryCode",
					  						DefaultText =  "Entitle Importer Country",
					  						FullLocalDefaultText =  "מדינת דרכון יבואן זכאי",
					  						ListFieldLable =  "EntitleImporterCountryCodeListLable",
					  						ListLableDefaultText =  "Entitle Importer Country ",
					  						ListLocalDefaultText =  "מדינת דרכון יבואן זכאי",
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
					 
					 						FieldName =  "EntitleImporterCountryName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "EntitleImporterCountry",
					  						ListPropertyPath =  "EntitleImporterCountry",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EntitleImporterCountryName",
					  						DefaultText =  "Entitle Importer Country Name",
					  						FullLocalDefaultText =  "מזכה את המדינה שם יבואן",
					  						ListFieldLable =  "EntitleImporterCountryNameListLable",
					  						ListLableDefaultText =  "Entitle Importer Country Name",
					  						ListLocalDefaultText =  "מזכה את המדינה יבואן",
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
					 
					 						FieldName =  "DeclarationDocumentId",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  35,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  35,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeclarationDocumentId",
					  						ListPropertyPath =  "DeclarationDocumentId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationDocumentId",
					  						DefaultText =  "Declaration Document ",
					  						FullLocalDefaultText =  "מספר הצהרה קשורה",
					  						ListFieldLable =  "DeclarationDocumentIdListLable",
					  						ListLableDefaultText =  "Declaration Document ",
					  						ListLocalDefaultText =  "מספר הצהרה קשורה",
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
					 
					 						FieldName =  "DeclarationDocumentTypeCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.LeadDocumentType",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  3,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeclarationDocumentTypeCode",
					  						ListPropertyPath =  "DeclarationDocumentTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationDocumentTypeCode",
					  						DefaultText =  "Declaration Document Type",
					  						FullLocalDefaultText =  "סוג הצהרה קשורה",
					  						ListFieldLable =  "DeclarationDocumentTypeCodeListLable",
					  						ListLableDefaultText =  "Declaration Document Type ",
					  						ListLocalDefaultText =  "סוג הצהרה קשורה",
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
					 
					 						FieldName =  "DeclarationDocumentTypeName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  40,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeclarationDocumentType",
					  						ListPropertyPath =  "DeclarationDocumentType",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationDocumentTypeName",
					  						DefaultText =  "Declaration Document Type Name",
					  						FullLocalDefaultText =  "שם סוג מסמך ההצהרה",
					  						ListFieldLable =  "DeclarationDocumentTypeNameListLable",
					  						ListLableDefaultText =  "Declaration Document Type Name",
					  						ListLocalDefaultText =  "סוג מסמך הצהרה",
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
					 
					 						FieldName =  "CreatedByUserId",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
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
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreatedByUserId",
					  						DefaultText =  "Created By User ",
					  						FullLocalDefaultText =  "משתמש פותח תיק",
					  						ListFieldLable =  "CreatedByUserIdListLable",
					  						ListLableDefaultText =  "CreatedByUser",
					  						ListLocalDefaultText =  "משתמש פותח תיק",
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
					 
					 						FieldName =  "IsChanged",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "IsChanged",
					  						ListPropertyPath =  "IsChanged",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsChanged",
					  						DefaultText =  "Is Changed",
					  						FullLocalDefaultText =  "בוצע שינוי",
					  						ListFieldLable =  "IsChangedListLable",
					  						ListLableDefaultText =  "Is Changed",
					  						ListLocalDefaultText =  "בוצע שינוי",
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
					 
					 						FieldName =  "PaymentDate",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToRoutingString",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PaymentDate",
					  						ListPropertyPath =  "PaymentDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PaymentDate",
					  						DefaultText =  "Payment Date",
					  						FullLocalDefaultText =  "תאריך תשלום",
					  						ListFieldLable =  "PaymentDateListLable",
					  						ListLableDefaultText =  "Payment Date",
					  						ListLocalDefaultText =  "תאריך תשלום",
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
					 
					 						FieldName =  "HatraDate",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToRoutingString",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "HatraDate",
					  						ListPropertyPath =  "HatraDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "HatraDate",
					  						DefaultText =  "Hatra Date",
					  						FullLocalDefaultText =  "תאריך התרה",
					  						ListFieldLable =  "HatraDateListLable",
					  						ListLableDefaultText =  "Hatra Date",
					  						ListLocalDefaultText =  "תאריך התרה",
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
					 
					 						FieldName =  "DeclarationStatusTypeCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.DeclarationStatusType",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  2,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeclarationStatusTypeCode",
					  						ListPropertyPath =  "DeclarationStatusTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationStatusTypeCode",
					  						DefaultText =  "Declaration Status Type ",
					  						FullLocalDefaultText =  "סטטוס הצהרה",
					  						ListFieldLable =  "DeclarationStatusTypeCodeListLable",
					  						ListLableDefaultText =  "Declaration Status Type",
					  						ListLocalDefaultText =  "סטטוס הצהרה",
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
					 
					 						FieldName =  "LoadingFactor",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Decimal",
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
					  						DataTemplateName =  "DeclarationLoadingFactorDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LoadingFactor",
					  						ListPropertyPath =  "LoadingFactor",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  18,
					  						DigitsAfterPoint =  10,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LoadingFactor",
					  						DefaultText =  "Loading Factor",
					  						FullLocalDefaultText =  "מקדם העמסה",
					  						ListFieldLable =  "LoadingFactorListLable",
					  						ListLableDefaultText =  "Loading Factor",
					  						ListLocalDefaultText =  "מקדם העמסה",
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
					 
					 						FieldName =  "DealValue",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Decimal",
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
					  						DataTemplateName =  "DeclarationDealValueDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DealValue",
					  						ListPropertyPath =  "DealValue",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  16,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DealValue",
					  						DefaultText =  "Deal Value",
					  						FullLocalDefaultText =  "סה\\\"כ ערך עסקה",
					  						ListFieldLable =  "DealValueListLable",
					  						ListLableDefaultText =  "Deal Value",
					  						ListLocalDefaultText =  "סה\\\"כ ערך עסקה",
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
					 
					 						FieldName =  "CIFValue",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Decimal",
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
					  						DataTemplateName =  "DeclarationCIFvalueDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CIFValue",
					  						ListPropertyPath =  "CIFValue",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  16,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CIFValue",
					  						DefaultText =  "CIF Value",
					  						FullLocalDefaultText =  "סה\\\"כ ערך CIF",
					  						ListFieldLable =  "CIFValueListLable",
					  						ListLableDefaultText =  "CIF Value",
					  						ListLocalDefaultText =  "סה\\\"כ ערך CIF",
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
					 
					 						FieldName =  "TotalTax",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Decimal",
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
					  						DataTemplateName =  "DeclarationTotalTaxDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TotalTax",
					  						ListPropertyPath =  "TotalTax",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  16,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TotalTax",
					  						DefaultText =  "Total Tax",
					  						FullLocalDefaultText =  "סה\\\"כ מיסים",
					  						ListFieldLable =  "TotalTaxListLable",
					  						ListLableDefaultText =  "Total Tax",
					  						ListLocalDefaultText =  "סה\\\"כ מיסים",
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
					 
					 						FieldName =  "DeclarationNumberandVersionId",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  false,
					  						DataTemplateName =  "DeclarationNumberandVersionIdTemplate",
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeclarationNumberandVersionId",
					  						ListPropertyPath =  "DeclarationNumberandVersionId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationNumberandVersionId",
					  						DefaultText =  "Declaration Number",
					  						FullLocalDefaultText =  "מספר הצהרה ",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Consignments",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "Consignments",
					  						ListPropertyPath =  "Consignments",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "Customs.Consignment",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Consignments",
					  						DefaultText =  "Consignments",
					  						ListFieldLable =  "ConsignmentsListLable",
					  						ListLableDefaultText =  "Consignments",
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
					 
					 						FieldName =  "SupplierInvoices",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "SupplierInvoices",
					  						ListPropertyPath =  "SupplierInvoices",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "Customs.SupplierInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SupplierInvoices",
					  						DefaultText =  "Supplier invoices",
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
					 
					 						FieldName =  "DeclarationTaxes",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "DeclarationTaxes",
					  						ListPropertyPath =  "DeclarationTaxes",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "Customs.DeclarationTax",
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
					  						FullFieldLable =  "DeclarationTaxes",
					  						DefaultText =  "Declaration Taxs",
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
					 
					 						FieldName =  "FileState",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  2,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FileState",
					  						ListPropertyPath =  "FileState",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FileState",
					  						DefaultText =  "File State",
					  						FullLocalDefaultText =  "קובץ מדינה",
					  						ListFieldLable =  "FileStateListLable",
					  						ListLableDefaultText =  "File State",
					  						ListLocalDefaultText =  "קובץ מדינה",
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
					 
					 						FieldName =  "TransportModeId",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CustomsTransportMode",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TransportModeId",
					  						ListPropertyPath =  "TransportModeId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransportModeId",
					  						DefaultText =  "Transport Mode ",
					  						FullLocalDefaultText =  "סוג הובלה",
					  						ListFieldLable =  "TransportModeIdListLable",
					  						ListLableDefaultText =  "Transport Mode ",
					  						ListLocalDefaultText =  "סוג הובלה",
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
					 
					 						FieldName =  "ErrosXml",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  4000,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  4000,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ErrosXml",
					  						ListPropertyPath =  "ErrosXml",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ErrosXml",
					  						DefaultText =  "Erros Xml",
					  						FullLocalDefaultText =  "שגיאות להצהרה",
					  						ListFieldLable =  "ErrosXmlListLable",
					  						ListLableDefaultText =  "Erros Xml",
					  						ListLocalDefaultText =  "שגיאות להצהרה",
					  						IsMaxLength =  true,
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
					 
					 						FieldName =  "DeclarationOfficeName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "DeclarationOfficeName",
					  						ListPropertyPath =  "DeclarationOfficeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationOfficeName",
					  						DefaultText =  "Declaration Office ",
					  						FullLocalDefaultText =  "בית מכס",
					  						ListFieldLable =  "DeclarationOfficeNameListLable",
					  						ListLableDefaultText =  "Declaration Office Name",
					  						ListLocalDefaultText =  "בית מכס",
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
					 
					 						FieldName =  "ImporterName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "ImporterName",
					  						ListPropertyPath =  "ImporterName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ImporterName",
					  						DefaultText =  "Importer Name",
					  						FullLocalDefaultText =  "שם יבואן",
					  						ListFieldLable =  "ImporterNameListLable",
					  						ListLableDefaultText =  "Importer Name",
					  						ListLocalDefaultText =  "שם יבואן",
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
					 
					 						FieldName =  "DepartmentId",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Department",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DepartmentId",
					  						ListPropertyPath =  "DepartmentId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DepartmentId",
					  						DefaultText =  "Department ",
					  						FullLocalDefaultText =  "מחלקה",
					  						ListFieldLable =  "DepartmentIdListLable",
					  						ListLableDefaultText =  "Department ",
					  						ListLocalDefaultText =  "מחלקה",
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
					 
					 						FieldName =  "DepartmentName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "DepartmentName",
					  						ListPropertyPath =  "DepartmentName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DepartmentName",
					  						DefaultText =  "Department",
					  						FullLocalDefaultText =  "חוליה",
					  						ListFieldLable =  "DepartmentNameListLable",
					  						ListLableDefaultText =  "Department",
					  						ListLocalDefaultText =  "חוליה",
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
					 
					 						FieldName =  "ReferentUserId",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ReferentUserId",
					  						ListPropertyPath =  "ReferentUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ReferentUserId",
					  						DefaultText =  "Referent User ",
					  						FullLocalDefaultText =  "רפרנט",
					  						ListFieldLable =  "ReferentUserIdListLable",
					  						ListLableDefaultText =  "Referent User ",
					  						ListLocalDefaultText =  "רפרנט",
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
					 
					 						FieldName =  "DeclarationStatusTypeName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  100,
					  						DisplayInList =  true,
					  						DataTemplateName =  "DeclarationStatusCodeTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeclarationStatusTypeName",
					  						ListPropertyPath =  "DeclarationStatusTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationStatusTypeName",
					  						DefaultText =  "Declaration Status Type",
					  						FullLocalDefaultText =  "סטטוס הצהרה",
					  						ListFieldLable =  "DeclarationStatusTypeNameListLable",
					  						ListLableDefaultText =  "Declaration Status Type Name",
					  						ListLocalDefaultText =  "סטטוס הצהרה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StorageSiteCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.DeliverySiteType",
					  						MinLength =  0,
					  						MaxLength =  20,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  17,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StorageSiteCode",
					  						ListPropertyPath =  "StorageSiteCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StorageSiteCode",
					  						DefaultText =  "Storage Site",
					  						FullLocalDefaultText =  "אתר אחסון",
					  						ListFieldLable =  "StorageSiteCodeListLable",
					  						ListLableDefaultText =  "Storage Site Code",
					  						ListLocalDefaultText =  "אתר אחסון",
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
					 
					 						FieldName =  "PlatformFee",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Decimal",
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
					  						DataTemplateName =  "DeclarationPlatformFeeDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PlatformFee",
					  						ListPropertyPath =  "PlatformFee",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  18,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PlatformFee",
					  						DefaultText =  "PlatformFee",
					  						FullLocalDefaultText =  "אגרת רציף",
					  						ListFieldLable =  "PlatformFeeListLable",
					  						ListLableDefaultText =  "PlatformFee",
					  						ListLocalDefaultText =  "אגרת רציף",
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
					 
					 						FieldName =  "DeclarationConstraints",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "DeclarationConstraints",
					  						ListPropertyPath =  "DeclarationConstraints",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "Customs.DeclarationConstraint",
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
					  						FullFieldLable =  "DeclarationConstraints",
					  						DefaultText =  "Declaration Constraints",
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
					 
					 						FieldName =  "CustomerCode",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  15,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomerCode",
					  						ListPropertyPath =  "CustomerCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerCode",
					  						DefaultText =  "Customer Code",
					  						ListFieldLable =  "CustomerCodeListLable",
					  						ListLableDefaultText =  "Customer Code",
					  						ListLocalDefaultText =  "קוד לקוח",
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
					 
					 						FieldName =  "CreateDateTime",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToRoutingString",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CreateDateTime",
					  						ListPropertyPath =  "CreateDateTime",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreateDateTime",
					  						DefaultText =  "Create Date Time",
					  						FullLocalDefaultText =  "תאריך יצירה",
					  						ListFieldLable =  "CreateDateTimeListLable",
					  						ListLableDefaultText =  "Create Date Time",
					  						ListLocalDefaultText =  "תאריך יצירה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdateDateTime",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToRoutingString",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UpdateDateTime",
					  						ListPropertyPath =  "UpdateDateTime",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdateDateTime",
					  						DefaultText =  "Update Date Time",
					  						FullLocalDefaultText =  "תאריך שעת עדכון",
					  						ListFieldLable =  "UpdateDateTimeListLable",
					  						ListLableDefaultText =  "Update Date Time",
					  						ListLocalDefaultText =  "תאריך שעת עדכון",
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
					 
					 						FieldName =  "IsCancelled",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "IsCancelled",
					  						ListPropertyPath =  "IsCancelled",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsCancelled",
					  						DefaultText =  "Is Cancelled",
					  						FullLocalDefaultText =  "תיק מבוטל",
					  						ListFieldLable =  "IsCancelledListLable",
					  						ListLableDefaultText =  "Is Cancelled",
					  						ListLocalDefaultText =  "תיק מבוטל",
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
					 
					 						FieldName =  "EntitleImporterName",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  55,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  55,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "EntitleImporterName",
					  						ListPropertyPath =  "EntitleImporterName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EntitleImporterName",
					  						DefaultText =  "Entitle Importer Name",
					  						FullLocalDefaultText =  "מספר יבואן זכאי",
					  						ListFieldLable =  "EntitleImporterNameListLable",
					  						ListLableDefaultText =  "Entitle Importer Name",
					  						ListLocalDefaultText =  "מספר יבואן זכאי",
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
					 
					 						FieldName =  "TransportModeName",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  10,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  10,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TransportModeName",
					  						ListPropertyPath =  "TransportModeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransportModeName",
					  						DefaultText =  "Transport Mode Name",
					  						FullLocalDefaultText =  "מצב תחבורה",
					  						ListFieldLable =  "TransportModeNameListLable",
					  						ListLableDefaultText =  "Transport Mode Name",
					  						ListLocalDefaultText =  "מצב תחבורה",
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
					 
					 						FieldName =  "TransferImporterName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "TransferImporterName",
					  						ListPropertyPath =  "TransferImporterName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransferImporterName",
					  						DefaultText =  "Transfer Importer Name",
					  						ListFieldLable =  "TransferImporterNameListLable",
					  						ListLableDefaultText =  "Transfer Importer Name",
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
					 
					 						FieldName =  "DealValueWithoutFactor",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Decimal",
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
					  						PMPropertyPath =  "DealValueWithoutFactor",
					  						ListPropertyPath =  "DealValueWithoutFactor",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  16,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DealValueWithoutFactor",
					  						DefaultText =  "Deal Value Without Factor",
					  						FullLocalDefaultText =  "ערך טובין בש”ח",
					  						ListFieldLable =  "DealValueWithoutFactorListLable",
					  						ListLableDefaultText =  "Deal Value Without Factor",
					  						ListLocalDefaultText =  "ערך טובין בש”ח",
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
					 
					 						FieldName =  "ImporterCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ImporterCode",
					  						ListPropertyPath =  "ImporterCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ImporterCode",
					  						DefaultText =  "Importer ",
					  						FullLocalDefaultText =  "מספר יבואן",
					  						ListFieldLable =  "ImporterCodeListLable",
					  						ListLableDefaultText =  "Importer",
					  						ListLocalDefaultText =  "מספר יבואן",
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
					 
					 						FieldName =  "TransferImporterCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TransferImporterCode",
					  						ListPropertyPath =  "TransferImporterCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransferImporterCode",
					  						DefaultText =  "Transfer Importer ",
					  						FullLocalDefaultText =  "מספר יבואן מעביר",
					  						ListFieldLable =  "TransferImporterCodeListLable",
					  						ListLableDefaultText =  "Transfer Importer",
					  						ListLocalDefaultText =  "מספר יבואן מעביר",
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
					 
					 						FieldName =  "EntitleImporterCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "EntitleImporterCode",
					  						ListPropertyPath =  "EntitleImporterCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EntitleImporterCode",
					  						DefaultText =  "Entitle Importer ",
					  						FullLocalDefaultText =  "מספר יבואן זכאי",
					  						ListFieldLable =  "EntitleImporterCodeListLable",
					  						ListLableDefaultText =  "Entitle Importer",
					  						ListLocalDefaultText =  "מספר יבואן זכאי",
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
					 
					 						FieldName =  "MarkAsChanged",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "MarkAsChanged",
					  						ListPropertyPath =  "MarkAsChanged",
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
					  						FullFieldLable =  "MarkAsChanged",
					  						DefaultText =  "Mark As Changed",
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
					 
					 						FieldName =  "CustomsTapagNumeral",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  100,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomsTapagNumeral",
					  						ListPropertyPath =  "CustomsTapagNumeral",
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
					  						FullFieldLable =  "CustomsTapagNumeral",
					  						DefaultText =  "Customs Tapag File/Customs Numeral",
					  						FullLocalDefaultText =  "מספר תיק תפ\\\"ג",
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
					 
					 						FieldName =  "CustomsTapagFile",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  25,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  25,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomsTapagFile",
					  						ListPropertyPath =  "CustomsTapagFile",
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
					  						FullFieldLable =  "CustomsTapagFile",
					  						DefaultText =  "Customs Tapag File",
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
					 
					 						FieldName =  "ConcurrencyGUID",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  40,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ConcurrencyGUID",
					  						ListPropertyPath =  "ConcurrencyGUID",
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
					  						FullFieldLable =  "ConcurrencyGUID",
					  						DefaultText =  "ConcurrencyGUID",
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
					 
					 						FieldName =  "NewConcurrencyGUID",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  40,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "NewConcurrencyGUID",
					  						ListPropertyPath =  "NewConcurrencyGUID",
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
					  						FullFieldLable =  "NewConcurrencyGUID",
					  						DefaultText =  "NewConcurrencyGUID",
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
					 
					 						FieldName =  "CreatedByUserName",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  60,
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
					  						ValidForQuerySection1 =  "Customs.Declaration",
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
					  						FullLocalDefaultText =  "משתמש פותח תיק",
					  						ListFieldLable =  "CreatedByUserNameListLable",
					  						ListLableDefaultText =  "Created By",
					  						ListLocalDefaultText =  "משתמש פותח תיק",
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
					 
					 						FieldName =  "ImporterTypeCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CustomerIdentifyType",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ImporterTypeCode",
					  						ListPropertyPath =  "ImporterTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ImporterTypeCode",
					  						DefaultText =  "Importer Type",
					  						FullLocalDefaultText =  "סוג יבואן",
					  						ListFieldLable =  "ImporterTypeCodeListLable",
					  						ListLableDefaultText =  "Importer Type Code",
					  						ListLocalDefaultText =  "סוג יבואן",
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
					 
					 						FieldName =  "TransferImporterTypeCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CustomerIdentifyType",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TransferImporterTypeCode",
					  						ListPropertyPath =  "TransferImporterTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransferImporterTypeCode",
					  						DefaultText =  "Transfer Importer Type",
					  						FullLocalDefaultText =  "סוג יבואן מעביר",
					  						ListFieldLable =  "TransferImporterTypeCodeListLable",
					  						ListLableDefaultText =  "Transfer Importer Type Code",
					  						ListLocalDefaultText =  "סוג יבואן מעביר",
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
					 
					 						FieldName =  "EntitleImporterTypeCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CustomerIdentifyType",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "EntitleImporterTypeCode",
					  						ListPropertyPath =  "EntitleImporterTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EntitleImporterTypeCode",
					  						DefaultText =  "Entitle Importer Type",
					  						FullLocalDefaultText =  "סוג יבואן זכאי",
					  						ListFieldLable =  "EntitleImporterTypeCodeListLable",
					  						ListLableDefaultText =  "Entitle Importer Type Code",
					  						ListLocalDefaultText =  "סוג יבואן זכאי",
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
					 
					 						FieldName =  "ImporterTypeName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "ImporterTypeName",
					  						ListPropertyPath =  "ImporterTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ImporterTypeName",
					  						DefaultText =  "Importer Type",
					  						FullLocalDefaultText =  "סוג יבואן",
					  						ListFieldLable =  "ImporterTypeNameListLable",
					  						ListLableDefaultText =  "Importer Type Name",
					  						ListLocalDefaultText =  "סוג יבואן",
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
					 
					 						FieldName =  "TransferImporterTypeName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "TransferImporterTypeName",
					  						ListPropertyPath =  "TransferImporterTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransferImporterTypeName",
					  						DefaultText =  "Transfer Importer Type",
					  						FullLocalDefaultText =  "סוג יבואן מעביר",
					  						ListFieldLable =  "TransferImporterTypeNameListLable",
					  						ListLableDefaultText =  "Transfer Importer Type Name",
					  						ListLocalDefaultText =  "סוג יבואן מעביר",
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
					 
					 						FieldName =  "EntitleImporterTypeName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "EntitleImporterTypeName",
					  						ListPropertyPath =  "EntitleImporterTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EntitleImporterTypeName",
					  						DefaultText =  "Entitle Importer Type",
					  						FullLocalDefaultText =  "סוג יבואן זכאי",
					  						ListFieldLable =  "EntitleImporterTypeNameListLable",
					  						ListLableDefaultText =  "Entitle Importer Type Name",
					  						ListLocalDefaultText =  "סוג יבואן זכאי",
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
					 
					 						FieldName =  "UserNotes",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  4000,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  4000,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UserNotes",
					  						ListPropertyPath =  "UserNotes",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UserNotes",
					  						DefaultText =  "User Notes",
					  						FullLocalDefaultText =  "הערות משתמש",
					  						ListFieldLable =  "UserNotesListLable",
					  						ListLableDefaultText =  "User Notes",
					  						ListLocalDefaultText =  "הערות משתמש",
					  						IsMaxLength =  true,
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
					 
					 						FieldName =  "FreightValuesFilled",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "FreightValuesFilled",
					  						ListPropertyPath =  "FreightValuesFilled",
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
					  						FullFieldLable =  "FreightValuesFilled",
					  						DefaultText =  "FreightValuesFilled",
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
					 
					 						FieldName =  "PaidDeclarationWithoutRelease",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  100,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PaidDeclarationWithoutRelease",
					  						ListPropertyPath =  "PaidDeclarationWithoutRelease",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PaidDeclarationWithoutRelease",
					  						DefaultText =  "PaidDeclarationWithoutRelease",
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
					 
					 						FieldName =  "DeclarationWithoutRelease",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  10,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  10,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeclarationWithoutRelease",
					  						ListPropertyPath =  "DeclarationWithoutRelease",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationWithoutRelease",
					  						DefaultText =  "Declaration Without Release",
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
					 
					 						FieldName =  "HasConstraint",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "HasConstraint",
					  						ListPropertyPath =  "HasConstraint",
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
					  						FullFieldLable =  "HasConstraint",
					  						DefaultText =  "HasConstraint",
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
					 
					 						FieldName =  "PrimaryInvoiceCounterKey",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PrimaryInvoiceCounterKey",
					  						ListPropertyPath =  "PrimaryInvoiceCounterKey",
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
					  						FullFieldLable =  "PrimaryInvoiceCounterKey",
					  						DefaultText =  "PrimaryInvoiceCounterKey",
					  						FullLocalDefaultText =  "חשבון עיקרי",
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
					 
					 						FieldName =  "PaymentOrderNumber",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  9,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  9,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PaymentOrderNumber",
					  						ListPropertyPath =  "PaymentOrderNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PaymentOrderNumber",
					  						DefaultText =  "Payment Order Number",
					  						FullLocalDefaultText =  "הוראת תשלום",
					  						ListFieldLable =  "PaymentOrderNumberListLable",
					  						ListLableDefaultText =  "Payment Order Number",
					  						ListLocalDefaultText =  "הוראת תשלום",
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
					 
					 						FieldName =  "PaymentStatusCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  2,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PaymentStatusCode",
					  						ListPropertyPath =  "PaymentStatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PaymentStatusCode",
					  						DefaultText =  "Payment Status Code",
					  						FullLocalDefaultText =  "סטטוס הוראת תשלום",
					  						ListFieldLable =  "PaymentStatusCodeListLable",
					  						ListLableDefaultText =  "Payment Status Code",
					  						ListLocalDefaultText =  "סטטוס הוראת תשלום",
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
					 
					 						FieldName =  "IsSignedVersion",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "IsSignedVersion",
					  						ListPropertyPath =  "IsSignedVersion",
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
					  						FullFieldLable =  "IsSignedVersion",
					  						DefaultText =  "Is Signed Version",
					  						FullLocalDefaultText =  "גרסה חתומה",
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
					 
					 						FieldName =  "SignedByUserId",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "SignedByUserId",
					  						ListPropertyPath =  "SignedByUserId",
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
					  						FullFieldLable =  "SignedByUserId",
					  						DefaultText =  "Signed By User",
					  						FullLocalDefaultText =  "משתמש חותם",
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
					 
					 						FieldName =  "StorageSiteName",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StorageSiteName",
					  						ListPropertyPath =  "StorageSiteName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StorageSiteName",
					  						DefaultText =  "Storage Site",
					  						FullLocalDefaultText =  "אתר אחסון",
					  						ListFieldLable =  "StorageSiteNameListLable",
					  						ListLableDefaultText =  "Storage Site Name",
					  						ListLocalDefaultText =  "שם אתר אחסון",
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
					 
					 						FieldName =  "HasDocument",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "HasDocument",
					  						ListPropertyPath =  "HasDocument",
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
					  						FullFieldLable =  "HasDocument",
					  						DefaultText =  "HasDocument",
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
					 
					 						FieldName =  "SignerPersonalId",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  20,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SignerPersonalId",
					  						ListPropertyPath =  "SignerPersonalId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SignerPersonalId",
					  						DefaultText =  "Signer Personal ID",
					  						FullLocalDefaultText =  "ת\\\"ז חותם",
					  						ListFieldLable =  "SignerPersonalIdListLable",
					  						ListLableDefaultText =  "Signer Personal ID",
					  						ListLocalDefaultText =  "ת\\\"ז חותם",
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
					 
					 						FieldName =  "CustomsNumeral",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "CustomsNumeral",
					  						ListPropertyPath =  "CustomsNumeral",
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
					  						FullFieldLable =  "CustomsNumeral",
					  						DefaultText =  "Customs Numeral",
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
					 
					 						FieldName =  "IsConvertedDeclaration",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "IsConvertedDeclaration",
					  						ListPropertyPath =  "IsConvertedDeclaration",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsConvertedDeclaration",
					  						DefaultText =  "Converted Declaration",
					  						FullLocalDefaultText =  "הצהרה מוסבת",
					  						ListFieldLable =  "IsConvertedDeclarationListLable",
					  						ListLableDefaultText =  "Converted Declaration",
					  						ListLocalDefaultText =  "הצהרה מוסבת",
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
					 
					 						FieldName =  "CorrectionsXml",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  1000,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1000,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CorrectionsXml",
					  						ListPropertyPath =  "CorrectionsXml",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CorrectionsXml",
					  						DefaultText =  "Corrections Xml",
					  						FullLocalDefaultText =  "תיקונים בהצהרה",
					  						ListFieldLable =  "CorrectionsXmlListLable",
					  						ListLableDefaultText =  "Corrections Xml",
					  						ListLocalDefaultText =  "תיקונים בהצהרה",
					  						IsMaxLength =  true,
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
					 
					 						FieldName =  "ResetDeclarationNumber",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "ResetDeclarationNumber",
					  						ListPropertyPath =  "ResetDeclarationNumber",
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
					  						FullFieldLable =  "ResetDeclarationNumber",
					  						DefaultText =  "ResetDeclarationNumber",
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
					 
					 						FieldName =  "IsCopiedFromOtherDeclaration",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "IsCopiedFromOtherDeclaration",
					  						ListPropertyPath =  "IsCopiedFromOtherDeclaration",
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
					  						FullFieldLable =  "IsCopiedFromOtherDeclaration",
					  						DefaultText =  "IsCopiedFromOtherDeclaration",
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
					 
					 						FieldName =  "RequestFileNumber",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  35,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  35,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "RequestFileNumber",
					  						ListPropertyPath =  "RequestFileNumber",
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
					  						FullFieldLable =  "RequestFileNumber",
					  						DefaultText =  "Request File Number",
					  						FullLocalDefaultText =  "מספר תיק בקשה",
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
					 
					 						FieldName =  "IsReleaseFile",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "IsReleaseFile",
					  						ListPropertyPath =  "IsReleaseFile",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsReleaseFile",
					  						DefaultText =  "Is Release File",
					  						FullLocalDefaultText =  "תיק שחרור",
					  						ListFieldLable =  "IsReleaseFileListLable",
					  						ListLableDefaultText =  "Is Release File",
					  						ListLocalDefaultText =  "תיק שחרור",
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
					 
					 						FieldName =  "VatChanged",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "VatChanged",
					  						ListPropertyPath =  "VatChanged",
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
					  						FullFieldLable =  "VatChanged",
					  						DefaultText =  "VatChanged",
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
					 
					 						FieldName =  "IsConnectedToUnifreight",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "IsConnectedToUnifreight",
					  						ListPropertyPath =  "IsConnectedToUnifreight",
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
					  						FullFieldLable =  "IsConnectedToUnifreight",
					  						DefaultText =  "Is Connected To Unifreight",
					  						FullLocalDefaultText =  "תיק יוניפרייט",
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
					 
					 						FieldName =  "MainImporterEntitlemntTypeCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.EntitlementType",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  3,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "MainImporterEntitlemntTypeCode",
					  						ListPropertyPath =  "MainImporterEntitlemntTypeCode",
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
					  						FullFieldLable =  "MainImporterEntitlemntTypeCode",
					  						DefaultText =  "Main Importer Entitlement Type Code",
					  						FullLocalDefaultText =  "סוג זכאות יבואן",
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
					 
					 						FieldName =  "TransImporterEntitleTypeCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.EntitlementType",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  3,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TransImporterEntitleTypeCode",
					  						ListPropertyPath =  "TransImporterEntitleTypeCode",
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
					  						FullFieldLable =  "TransImporterEntitleTypeCode",
					  						DefaultText =  "TransImporter Entitlemnt Type Code",
					  						FullLocalDefaultText =  "סוג זכאות יבואן מעביר",
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
					 
					 						FieldName =  "ImporterAddress",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  236,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  236,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ImporterAddress",
					  						ListPropertyPath =  "ImporterAddress",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ImporterAddress",
					  						DefaultText =  "Importer Address",
					  						FullLocalDefaultText =  "כתובת יבואן",
					  						ListFieldLable =  "ImporterAddressListLable",
					  						ListLableDefaultText =  "Importer Address",
					  						ListLocalDefaultText =  "כתובת יבואן",
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
					 
					 						FieldName =  "TransferImporterAddress",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  236,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  236,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TransferImporterAddress",
					  						ListPropertyPath =  "TransferImporterAddress",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransferImporterAddress",
					  						DefaultText =  "Transfer Importer Address",
					  						FullLocalDefaultText =  "כתובת יבואן מעביר",
					  						ListFieldLable =  "TransferImporterAddressListLable",
					  						ListLableDefaultText =  "Transfer Importer Address",
					  						ListLocalDefaultText =  "כתובת יבואן מעביר",
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
					 
					 						FieldName =  "EntitleImporterAddress",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  236,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  236,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "EntitleImporterAddress",
					  						ListPropertyPath =  "EntitleImporterAddress",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EntitleImporterAddress",
					  						DefaultText =  "Entitle Importer Address",
					  						FullLocalDefaultText =  "כתובת יבואן זכאי",
					  						ListFieldLable =  "EntitleImporterAddressListLable",
					  						ListLableDefaultText =  "Entitle Importer Address",
					  						ListLocalDefaultText =  "כתובת יבואן זכאי",
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
					 
					 						FieldName =  "ImporterPassportNumber",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ImporterPassportNumber",
					  						ListPropertyPath =  "ImporterPassportNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ImporterPassportNumber",
					  						DefaultText =  "Importer Passport Number",
					  						FullLocalDefaultText =  "מס' דרכון יבואן",
					  						ListFieldLable =  "ImporterPassportNumberListLable",
					  						ListLableDefaultText =  "Importer Passport Number",
					  						ListLocalDefaultText =  "מס' דרכון יבואן",
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
					 
					 						FieldName =  "TransferPassportNumber",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TransferPassportNumber",
					  						ListPropertyPath =  "TransferPassportNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransferPassportNumber",
					  						DefaultText =  "Transfer Passport Number",
					  						FullLocalDefaultText =  "מס' תעודה יבואן מעביר",
					  						ListFieldLable =  "TransferPassportNumberListLable",
					  						ListLableDefaultText =  "Transfer Passport Number",
					  						ListLocalDefaultText =  "מס' תעודה יבואן מעביר",
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
					 
					 						FieldName =  "EntitlePassportNumber",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "EntitlePassportNumber",
					  						ListPropertyPath =  "EntitlePassportNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EntitlePassportNumber",
					  						DefaultText =  "Entitle Passport Number",
					  						FullLocalDefaultText =  "מס' תעודה יבואן זכאי",
					  						ListFieldLable =  "EntitlePassportNumberListLable",
					  						ListLableDefaultText =  "Entitle Passport Number",
					  						ListLocalDefaultText =  "מס' תעודה יבואן זכאי",
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
					 
					 						FieldName =  "FacilityTypeName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FacilityTypeName",
					  						ListPropertyPath =  "FacilityTypeName",
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
					  						FullFieldLable =  "FacilityTypeName",
					  						DefaultText =  "Facility Type Name",
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
					 
					 						FieldName =  "CalculatedImporterName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CalculatedImporterName",
					  						ListPropertyPath =  "CalculatedImporterName",
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
					  						FullFieldLable =  "CalculatedImporterName",
					  						DefaultText =  "Importer Name",
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
					 
					 						FieldName =  "CalculatedTransferImporterName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CalculatedTransferImporterName",
					  						ListPropertyPath =  "CalculatedTransferImporterName",
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
					  						FullFieldLable =  "CalculatedTransferImporterName",
					  						DefaultText =  "Transfer Importer Name",
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
					 
					 						FieldName =  "CalculatedEntitleImporterName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CalculatedEntitleImporterName",
					  						ListPropertyPath =  "CalculatedEntitleImporterName",
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
					  						FullFieldLable =  "CalculatedEntitleImporterName",
					  						DefaultText =  "Entitle Importer Name",
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
					 
					 						FieldName =  "CustomerVatNo",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomerVatNo",
					  						ListPropertyPath =  "CustomerVatNo",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerVatNo",
					  						DefaultText =  "Customer Vat No.",
					  						FullLocalDefaultText =  "Customer Vat No.",
					  						ListFieldLable =  "CustomerVatNoListLable",
					  						ListLableDefaultText =  "Customer Vat No.",
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
					 
					 						FieldName =  "DeclarationErrorViews2",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "DeclarationErrorViews2",
					  						ListPropertyPath =  "DeclarationErrorViews2",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "DeclarationErrorView",
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
					  						FullFieldLable =  "DeclarationErrorViews2",
					  						DefaultText =  "DeclarationErrorViews",
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
					 
					 						FieldName =  "DeclarationPaymentChanged",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "DeclarationPaymentChanged",
					  						ListPropertyPath =  "DeclarationPaymentChanged",
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
					  						FullFieldLable =  "DeclarationPaymentChanged",
					  						DefaultText =  "DeclarationPaymentChanged",
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
					 
					 						FieldName =  "CustomsRequestsSheetId",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  40,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomsRequestsSheetId",
					  						ListPropertyPath =  "CustomsRequestsSheetId",
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
					  						FullFieldLable =  "CustomsRequestsSheetId",
					  						DefaultText =  "CustomsRequestsSheetId",
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
					 
					 						FieldName =  "DocumentDeclarationId",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  100,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DocumentDeclarationId",
					  						ListPropertyPath =  "DocumentDeclarationId",
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
					  						FullFieldLable =  "DocumentDeclarationId",
					  						DefaultText =  "DocumentDeclarationId",
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
					 
					 						FieldName =  "StorageStatusCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.StorageStatus",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StorageStatusCode",
					  						ListPropertyPath =  "StorageStatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StorageStatusCode",
					  						DefaultText =  "Storage Status",
					  						FullLocalDefaultText =  "סטטוס אחסנה",
					  						ListFieldLable =  "StorageStatusCodeListLable",
					  						ListLableDefaultText =  "Storage Status",
					  						ListLocalDefaultText =  "סטטוס אחסנה",
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
					 
					 						FieldName =  "CasualSupplierName",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CasualSupplierName",
					  						ListPropertyPath =  "CasualSupplierName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CasualSupplierName",
					  						DefaultText =  "Casual Supplier Name",
					  						ListFieldLable =  "CasualSupplierNameListLable",
					  						ListLableDefaultText =  "Casual Supplier Name",
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
					 
					 						FieldName =  "CasualSupplierAddress",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  250,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CasualSupplierAddress",
					  						ListPropertyPath =  "CasualSupplierAddress",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CasualSupplierAddress",
					  						DefaultText =  "Casual Supplier Address",
					  						ListFieldLable =  "CasualSupplierAddressListLable",
					  						ListLableDefaultText =  "Casual Supplier Address",
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
					 
					 						FieldName =  "IsCourierDeclaration",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "IsCourierDeclaration",
					  						ListPropertyPath =  "IsCourierDeclaration",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsCourierDeclaration",
					  						DefaultText =  "Is Courier Declaration",
					  						FullLocalDefaultText =  "הצהרת בלדר",
					  						ListFieldLable =  "IsCourierDeclarationListLable",
					  						ListLableDefaultText =  "Is Courier Declaration",
					  						ListLocalDefaultText =  "הצהרת בלדר",
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
					 
					 						FieldName =  "ManifestCargoStatusCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.ManifestCargoStatus",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  2,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ManifestCargoStatusCode",
					  						ListPropertyPath =  "ManifestCargoStatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ManifestCargoStatusCode",
					  						DefaultText =  "Manifest Cargo Status ",
					  						FullLocalDefaultText =  "קוד משוב למצהר",
					  						ListFieldLable =  "ManifestCargoStatusCodeListLable",
					  						ListLableDefaultText =  "Manifest Cargo Status",
					  						ListLocalDefaultText =  "קוד סטטוס מצהר",
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
					 
					 						FieldName =  "ManifestErrorXml",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  4000,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  4000,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ManifestErrorXml",
					  						ListPropertyPath =  "ManifestErrorXml",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ManifestErrorXml",
					  						DefaultText =  "Manifest Error Xml",
					  						FullLocalDefaultText =  "משוב למצהר",
					  						ListFieldLable =  "ManifestErrorXmlListLable",
					  						ListLableDefaultText =  "Manifest Error Xml",
					  						ListLocalDefaultText =  "משוב למצהר",
					  						IsMaxLength =  true,
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
					 
					 						FieldName =  "CourierHAWB",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  35,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  35,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CourierHAWB",
					  						ListPropertyPath =  "CourierHAWB",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CourierHAWB",
					  						DefaultText =  "Courier HAWB",
					  						FullLocalDefaultText =  "שטר מטען בלדר",
					  						ListFieldLable =  "CourierHAWBListLable",
					  						ListLableDefaultText =  "Courier HAWB",
					  						ListLocalDefaultText =  "שטר מטען בלדר",
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
					 
					 						FieldName =  "ManifestNumber",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  35,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  35,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ManifestNumber",
					  						ListPropertyPath =  "ManifestNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ManifestNumber",
					  						DefaultText =  "Manifest Number",
					  						FullLocalDefaultText =  "מזהה מטען",
					  						ListFieldLable =  "ManifestNumberListLable",
					  						ListLableDefaultText =  "Manifest Number",
					  						ListLocalDefaultText =  "מזהה מטען",
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
					 
					 						FieldName =  "StorageStatusName",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StorageStatusName",
					  						ListPropertyPath =  "StorageStatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StorageStatusName",
					  						DefaultText =  "Storage Status",
					  						FullLocalDefaultText =  "שם סטטוס אחסנה",
					  						ListFieldLable =  "StorageStatusNameListLable",
					  						ListLableDefaultText =  "Storage Status Name",
					  						ListLocalDefaultText =  "שם סטטוס אחסנה",
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
					 
					 						FieldName =  "IsAccumulated",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "DeclarationPaymentChanged",
					  						ListPropertyPath =  "IsAccumulated",
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
					  						FullFieldLable =  "IsAccumulated",
					  						DefaultText =  "Is Accumulated",
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
					 
					 						FieldName =  "ExcludeConsignment",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ExcludeConsignment",
					  						ListPropertyPath =  "ExcludeConsignment",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ExcludeConsignment",
					  						DefaultText =  "Exclude Consignment From Interface",
					  						FullLocalDefaultText =  "שדר ללא משגור",
					  						ListFieldLable =  "ExcludeConsignmentListLable",
					  						ListLableDefaultText =  "שדר ללא משגור",
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
					 
					 						FieldName =  "CourierCustomStatusCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CourierCustomStatus",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  3,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CourierCustomStatusCode",
					  						ListPropertyPath =  "CourierCustomStatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CourierCustomStatusCode",
					  						DefaultText =  "Courier Custom Status",
					  						FullLocalDefaultText =  " קוד סטטוס הצהרת בלדר",
					  						ListFieldLable =  "CourierCustomStatusCodeListLable",
					  						ListLableDefaultText =  "CourierCustomStatusCode",
					  						ListLocalDefaultText =  "קוד סטטוס הצהרת בלדר",
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
					 
					 						FieldName =  "CourierSuspentionReasonCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.AgentTalkBackType",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  3,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CourierSuspentionReasonCode",
					  						ListPropertyPath =  "CourierSuspentionReasonCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CourierSuspentionReasonCode",
					  						DefaultText =  "Courier Suspention Reason Code",
					  						FullLocalDefaultText =  "קוד סיבת עיכוב",
					  						ListFieldLable =  "CourierSuspentionReasonCodeListLable",
					  						ListLableDefaultText =  "Courier Suspention Reason Code",
					  						ListLocalDefaultText =  "קוד סיבת עיכוב",
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
					 
					 						FieldName =  "CourierReleaseStatusCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CourierReleaseStatusCode",
					  						ListPropertyPath =  "CourierReleaseStatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CourierReleaseStatusCode",
					  						DefaultText =  "Courier Release Status ",
					  						FullLocalDefaultText =  "סטטוס שחרור",
					  						ListFieldLable =  "CourierReleaseStatusCodeListLable",
					  						ListLableDefaultText =  "Courier Release Status Code",
					  						ListLocalDefaultText =  "סטטוס שחרור",
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
					 
					 						FieldName =  "CourierHataraStatusCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CourierHataraStatusCode",
					  						ListPropertyPath =  "CourierHataraStatusCode",
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
					  						FullFieldLable =  "CourierHataraStatusCode",
					  						DefaultText =  "Courier Hatara Status Code",
					  						FullLocalDefaultText =  "סטטוס התרה",
					  						ListFieldLable =  "CourierHataraStatusCodeListLable",
					  						ListLableDefaultText =  "Courier Hatara Status Code",
					  						ListLocalDefaultText =  "סטטוס התרה",
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
					 
					 						FieldName =  "CourierData",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  100,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CourierData",
					  						ListPropertyPath =  "CourierData",
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
					  						FullFieldLable =  "CourierData",
					  						DefaultText =  "Courier Data",
					  						FullLocalDefaultText =  "ש.מ.ר",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceHasFreight",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "InvoiceHasFreight",
					  						ListPropertyPath =  "InvoiceHasFreight",
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
					  						FullFieldLable =  "InvoiceHasFreight",
					  						DefaultText =  "InvoiceHasFreight",
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
					 
					 						FieldName =  "DealValueWithFactor",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Decimal",
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
					  						PMPropertyPath =  "DealValueWithFactor",
					  						ListPropertyPath =  "DealValueWithFactor",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  16,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DealValueWithFactor",
					  						DefaultText =  "Deal Value With Factor",
					  						FullLocalDefaultText =  "ערך טובין בש''ח",
					  						ListFieldLable =  "DealValueWithFactorListLable",
					  						ListLableDefaultText =  "Deal Value With Factor",
					  						ListLocalDefaultText =  "ערך טובין בש''ח",
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
					 
					 						FieldName =  "IsValueForCustomsOnly",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsValueForCustomsOnly",
					  						ListPropertyPath =  "IsValueForCustomsOnly",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsValueForCustomsOnly",
					  						DefaultText =  "Is value for customs only",
					  						FullLocalDefaultText =  "ערך לצרכי מכס",
					  						ListFieldLable =  "IsValueForCustomsOnlyListLable",
					  						ListLableDefaultText =  "Is value for customs only",
					  						ListLocalDefaultText =  "ערך לצרכי מכס",
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
					 
					 						FieldName =  "WeightValue",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.FreightPaymentMethod",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  3,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "WeightValue",
					  						ListPropertyPath =  "WeightValue",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.FreightPaymentMethod",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "WeightValue",
					  						DefaultText =  "Weight Value",
					  						FullLocalDefaultText =  "תנאי תשלום",
					  						ListFieldLable =  "WeightValueListLable",
					  						ListLableDefaultText =  "WeightValue",
					  						ListLocalDefaultText =  "תנאי תשלום",
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
					 
					 						FieldName =  "WeightValueName",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "WeightValueName",
					  						ListPropertyPath =  "WeightValueName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "WeightValueName",
					  						DefaultText =  "Weight Value Name",
					  						FullLocalDefaultText =  "תנאי תשלום",
					  						ListFieldLable =  "WeightValueNameListLable",
					  						ListLableDefaultText =  "Weight Value Name",
					  						ListLocalDefaultText =  "תנאי תשלום",
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
					 
					 						FieldName =  "DeclarationConsAcceptances",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "DeclarationConsAcceptances",
					  						ListPropertyPath =  "DeclarationConsAcceptances",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "Customs.DeclarationConsAcceptance",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationConsAcceptances",
					  						DefaultText =  "Declaration Cons Acceptances",
					  						ListFieldLable =  "DeclarationConsAcceptancesListLable",
					  						ListLableDefaultText =  "Declaration Cons Acceptances",
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
					 
					 						FieldName =  "CourierSearchFields",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  500,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CourierSearchFields",
					  						ListPropertyPath =  "CourierSearchFields",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CourierSearchFields",
					  						DefaultText =  "CourierSearchFields",
					  						FullLocalDefaultText =  "תיק/הצהרה/שטר מטען בלדר/לקוח",
					  						ListFieldLable =  "CourierSearchFieldsListLable",
					  						ListLableDefaultText =  "CourierSearchFields",
					  						ListLocalDefaultText =  "שדה חיפוש בלדרות",
					  						HelpTextCode =  "CourierSearchFields",
					  						HelpTextDefaultText =  "Courier Search By:",
					  						HelpLocalDefaultText =  "שדה חיפוש בלדרות",
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
					 
					 						FieldName =  "CourierCustomStatusName",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  true,
					  						DataTemplateName =  "DeclarationStatusCodeTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CourierCustomStatusName",
					  						ListPropertyPath =  "CourierCustomStatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CourierCustomStatusName",
					  						DefaultText =  "Courier Custom Status",
					  						FullLocalDefaultText =  "סטטוס הצהרת בלדר",
					  						ListFieldLable =  "CourierCustomStatusNameListLable",
					  						ListLableDefaultText =  "Courier Custom Status",
					  						ListLocalDefaultText =  "סטטוס הצהרת בלדר",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ManifestCargoStatusName",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ManifestCargoStatusName",
					  						ListPropertyPath =  "ManifestCargoStatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ManifestCargoStatusName",
					  						DefaultText =  "Manifest Cargo Status ",
					  						FullLocalDefaultText =  "משוב למצהר",
					  						ListFieldLable =  "ManifestCargoStatusNameListLable",
					  						ListLableDefaultText =  "Manifest Cargo Status ",
					  						ListLocalDefaultText =  "סטטוס מצהר",
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
					 
					 						FieldName =  "MAWBCourierMaster",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "MAWBCourierMaster",
					  						ListPropertyPath =  "MAWBCourierMaster",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "MAWBCourierMaster",
					  						DefaultText =  "MAWB Courier Master",
					  						FullLocalDefaultText =  "שטר מטען ראשי",
					  						ListFieldLable =  "MAWBCourierMasterListLable",
					  						ListLableDefaultText =  "MAWB Courier Master",
					  						ListLocalDefaultText =  "שטר מטען ראשי",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CourierSuspentionReasonName",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CourierSuspentionReasonName",
					  						ListPropertyPath =  "CourierSuspentionReasonName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CourierSuspentionReasonName",
					  						DefaultText =  "Courier Suspention Reason",
					  						FullLocalDefaultText =  "סיבת עיכוב",
					  						ListFieldLable =  "CourierSuspentionReasonNameListLable",
					  						ListLableDefaultText =  "Courier Suspention Reason",
					  						ListLocalDefaultText =  "סיבת עיכוב",
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
					 
					 						FieldName =  "AcceptanceStatusCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.AcceptanceStatus",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AcceptanceStatusCode",
					  						ListPropertyPath =  "AcceptanceStatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AcceptanceStatusCode",
					  						DefaultText =  "Acceptance Status",
					  						FullLocalDefaultText =  "סטטוס זמינות",
					  						ListFieldLable =  "AcceptanceStatusCodeListLable",
					  						ListLableDefaultText =  "Acceptance Status",
					  						ListLocalDefaultText =  "סטטוס זמינות",
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
					 
					 						FieldName =  "CasualImporterAddress1",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  35,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  35,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CasualImporterAddress1",
					  						ListPropertyPath =  "CasualImporterAddress1",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CasualImporterAddress1",
					  						DefaultText =  "Casual Importer Address",
					  						FullLocalDefaultText =  "כתובת לקוח",
					  						ListFieldLable =  "CasualImporterAddress1ListLable",
					  						ListLableDefaultText =  "Casual Importer Address",
					  						ListLocalDefaultText =  "כתובת לקוח",
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
					 
					 						FieldName =  "CasualImporterAddress2",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  35,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  35,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CasualImporterAddress2",
					  						ListPropertyPath =  "CasualImporterAddress2",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CasualImporterAddress2",
					  						DefaultText =  "Casual Importer Address 2",
					  						FullLocalDefaultText =  "כתובת לקוח 2",
					  						ListFieldLable =  "CasualImporterAddress2ListLable",
					  						ListLableDefaultText =  "Casual Importer Address 2",
					  						ListLocalDefaultText =  "כתובת לקוח 2",
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
					 
					 						FieldName =  "CasualImporterCity",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  17,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  17,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CasualImporterCity",
					  						ListPropertyPath =  "CasualImporterCity",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CasualImporterCity",
					  						DefaultText =  "Casual Importer City",
					  						FullLocalDefaultText =  "עיר",
					  						ListFieldLable =  "CasualImporterCityListLable",
					  						ListLableDefaultText =  "Casual Importer City",
					  						ListLocalDefaultText =  "עיר",
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
					 
					 						FieldName =  "CasualImporterZipCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  10,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "CasualImporterZipCode",
					  						ListPropertyPath =  "CasualImporterZipCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CasualImporterZipCode",
					  						DefaultText =  "Casual Importer Zip Code",
					  						FullLocalDefaultText =  "מיקוד",
					  						ListFieldLable =  "CasualImporterZipCodeListLable",
					  						ListLableDefaultText =  "Casual Importer Zip Code",
					  						ListLocalDefaultText =  "מיקוד",
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
					 
					 						FieldName =  "CasualImporterFax",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  30,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  30,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CasualImporterFax",
					  						ListPropertyPath =  "CasualImporterFax",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CasualImporterFax",
					  						DefaultText =  "Casual Importer Fax",
					  						FullLocalDefaultText =  "פקס",
					  						ListFieldLable =  "CasualImporterFaxListLable",
					  						ListLableDefaultText =  "Casual Importer Fax",
					  						ListLocalDefaultText =  "פקס",
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
					 
					 						FieldName =  "CasualImporterEmail",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  50,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  50,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CasualImporterEmail",
					  						ListPropertyPath =  "CasualImporterEmail",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CasualImporterEmail",
					  						DefaultText =  "Casual Importer Email",
					  						FullLocalDefaultText =  "מייל",
					  						ListFieldLable =  "CasualImporterEmailListLable",
					  						ListLableDefaultText =  "Casual Importer Email",
					  						ListLocalDefaultText =  "מייל",
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
					 
					 						FieldName =  "CasualImporterTel",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  30,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  30,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CasualImporterTel",
					  						ListPropertyPath =  "CasualImporterTel",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CasualImporterTel",
					  						DefaultText =  "Casual Importer Tel",
					  						FullLocalDefaultText =  "טלפון",
					  						ListFieldLable =  "CasualImporterTelListLable",
					  						ListLableDefaultText =  "Casual Importer Tel",
					  						ListLocalDefaultText =  "טלפון",
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
					 
					 						FieldName =  "CasualImporterContact",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  50,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  50,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CasualImporterContact",
					  						ListPropertyPath =  "CasualImporterContact",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CasualImporterContact",
					  						DefaultText =  "Casual Importer Contact",
					  						FullLocalDefaultText =  "איש קשר",
					  						ListFieldLable =  "CasualImporterContactListLable",
					  						ListLableDefaultText =  "Casual Importer Contact",
					  						ListLocalDefaultText =  "איש קשר",
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
					 
					 						FieldName =  "ItemsProcessTypesList",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  2000,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  2000,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ItemsProcessTypesList",
					  						ListPropertyPath =  "ItemsProcessTypesList",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ItemsProcessTypesList",
					  						DefaultText =  "Items Process Types List",
					  						FullLocalDefaultText =  "רשימת קודי תהליך",
					  						ListFieldLable =  "ItemsProcessTypesListListLable",
					  						ListLableDefaultText =  "Items Process Types List",
					  						ListLocalDefaultText =  "רשימת קודי תהליך",
					  						IsMaxLength =  true,
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
					 
					 						FieldName =  "IsClose",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsClose",
					  						ListPropertyPath =  "IsClose",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsClose",
					  						DefaultText =  "Is Close",
					  						FullLocalDefaultText =  "סגור",
					  						ListFieldLable =  "IsCloseListLable",
					  						ListLableDefaultText =  "Is Close",
					  						ListLocalDefaultText =  "סגור",
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
					 
					 						FieldName =  "AcceptanceStatusName",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AcceptanceStatusName",
					  						ListPropertyPath =  "AcceptanceStatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AcceptanceStatusName",
					  						DefaultText =  "Acceptance Status",
					  						FullLocalDefaultText =  "Customs.Declaration",
					  						ListFieldLable =  "AcceptanceStatusNameListLable",
					  						ListLableDefaultText =  "Acceptance Status",
					  						ListLocalDefaultText =  "סטטוס זמינות",
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
					 
					 						FieldName =  "CourierSuspentionCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.DeclarationStatusType",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  2,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CourierSuspentionCode",
					  						ListPropertyPath =  "CourierSuspentionCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CourierSuspentionCode",
					  						DefaultText =  "Courier Suspention Code",
					  						FullLocalDefaultText =  "קוד עיכוב מכס",
					  						ListFieldLable =  "CourierSuspentionCodeListLable",
					  						ListLableDefaultText =  "Courier Suspention Code",
					  						ListLocalDefaultText =  "קוד עיכוב מכס",
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
					 
					 						FieldName =  "CourierSuspentionName",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CourierSuspentionName",
					  						ListPropertyPath =  "CourierSuspentionName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CourierSuspentionName",
					  						DefaultText =  "Courier Suspention Name",
					  						FullLocalDefaultText =  "תאור קוד עיכוב מכס",
					  						ListFieldLable =  "CourierSuspentionNameListLable",
					  						ListLableDefaultText =  "Courier Suspention Name",
					  						ListLocalDefaultText =  "תאור קוד עיכוב מכס",
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
					 
					 						FieldName =  "DepositionStatusCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DepositionStatusCode",
					  						ListPropertyPath =  "DepositionStatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DepositionStatusCode",
					  						DefaultText =  "Deposition Status Code",
					  						FullLocalDefaultText =  "סטטוס תצהיר",
					  						ListFieldLable =  "DepositionStatusCodeListLable",
					  						ListLableDefaultText =  "Deposition Status Code",
					  						ListLocalDefaultText =  "סטטוס תצהיר",
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
					 
					 						FieldName =  "CourierMasterId",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CourierMasterId",
					  						ListPropertyPath =  "CourierMasterId",
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
					  						FullFieldLable =  "CourierMasterId",
					  						DefaultText =  "Courier Master Id",
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
					 
					 						FieldName =  "IsClosedForFollowUp",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsClosedForFollowUp",
					  						ListPropertyPath =  "IsClosedForFollowUp",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsClosedForFollowUp",
					  						DefaultText =  "Is Closed For Follow Up",
					  						FullLocalDefaultText =  "סגור/פתוח",
					  						ListFieldLable =  "IsClosedForFollowUpListLable",
					  						ListLableDefaultText =  "Is Closed For Follow Up",
					  						ListLocalDefaultText =  "סגור/פתוח",
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
					 
					 						FieldName =  "FastIndividualProcessCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  3,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FastIndividualProcessCode",
					  						ListPropertyPath =  "FastIndividualProcessCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FastIndividualProcessCode",
					  						DefaultText =  "Fast Individual Process",
					  						FullLocalDefaultText =  "מהיר/פרטני",
					  						ListFieldLable =  "FastIndividualProcessCodeListLable",
					  						ListLableDefaultText =  "Fast Individual Process",
					  						ListLocalDefaultText =  "מהיר/פרטני",
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
					 
					 						FieldName =  "TotalInvoiceAmountInUSD",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Decimal",
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
					  						PMPropertyPath =  "TotalInvoiceAmountInUSD",
					  						ListPropertyPath =  "TotalInvoiceAmountInUSD",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  16,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TotalInvoiceAmountInUSD",
					  						DefaultText =  "Total Invoice Amount",
					  						FullLocalDefaultText =  "ערך סחורה ב-$",
					  						ListFieldLable =  "TotalInvoiceAmountInUSDListLable",
					  						ListLableDefaultText =  "Total Invoice Amount",
					  						ListLocalDefaultText =  "ערך סחורה ב-$",
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
					 
					 						FieldName =  "IsPending902",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsPending902",
					  						ListPropertyPath =  "IsPending902",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsPending902",
					  						DefaultText =  "Is Pending 902",
					  						FullLocalDefaultText =  "Pending 902",
					  						ListFieldLable =  "IsPending902ListLable",
					  						ListLableDefaultText =  "Is Pending 902",
					  						ListLocalDefaultText =  "Pending 902",
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
					 
					 						FieldName =  "IsCourierMissingClassification",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsCourierMissingClassification",
					  						ListPropertyPath =  "IsCourierMissingClassification",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsCourierMissingClassification",
					  						DefaultText =  "Courier Missing Classification",
					  						FullLocalDefaultText =  "ללא סיווג",
					  						ListFieldLable =  "IsCourierMissingClassificationListLable",
					  						ListLableDefaultText =  "Courier Missing Classification",
					  						ListLocalDefaultText =  "ללא סיווג",
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
					 
					 						FieldName =  "MAWB",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  35,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  35,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "MAWB",
					  						ListPropertyPath =  "MAWB",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "MAWB",
					  						DefaultText =  "MAWB",
					  						FullLocalDefaultText =  "שטר מטען ראשי",
					  						ListFieldLable =  "MAWBListLable",
					  						ListLableDefaultText =  "MAWB",
					  						ListLocalDefaultText =  "שטר מטען ראשי",
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
					 
					 						FieldName =  "IsPending900",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsPending900",
					  						ListPropertyPath =  "IsPending900",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsPending900",
					  						DefaultText =  "Is Pending 900",
					  						FullLocalDefaultText =  "Pending 900",
					  						ListFieldLable =  "IsPending900ListLable",
					  						ListLableDefaultText =  "Is Pending 900",
					  						ListLocalDefaultText =  "Pending 900",
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
					 
					 						FieldName =  "CourierPendingReasonList",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  1000,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1000,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CourierPendingReasonList",
					  						ListPropertyPath =  "CourierPendingReasonList",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CourierPendingReasonList",
					  						DefaultText =  "Courier Pending Reason List",
					  						FullLocalDefaultText =  "קוד PENDING",
					  						ListFieldLable =  "CourierPendingReasonListListLable",
					  						ListLableDefaultText =  "Courier Pending Reason List",
					  						ListLocalDefaultText =  "קוד PENDING",
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
					 
					 						FieldName =  "CargoDescription",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  256,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  256,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CargoDescription",
					  						ListPropertyPath =  "CargoDescription",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CargoDescription",
					  						DefaultText =  "Cargo Description",
					  						FullLocalDefaultText =  "תאור טובין",
					  						ListFieldLable =  "CargoDescriptionListLable",
					  						ListLableDefaultText =  "Cargo Description",
					  						ListLocalDefaultText =  "תאור טובין",
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
					 
					 						FieldName =  "IsPaymentProtested",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "IsPaymentProtested",
					  						ListPropertyPath =  "IsPaymentProtested",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsPaymentProtested",
					  						DefaultText =  "Is Payment Protested",
					  						FullLocalDefaultText =  "בוצעה הגשה אגב מחאה",
					  						ListFieldLable =  "IsPaymentProtestedListLable",
					  						ListLableDefaultText =  "Is Payment Protested",
					  						ListLocalDefaultText =  "בוצעה הגשה אגב מחאה",
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
					 
					 						FieldName =  "FastIndividualProcessName",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  10,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  10,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FastIndividualProcessName",
					  						ListPropertyPath =  "FastIndividualProcessName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FastIndividualProcessName",
					  						DefaultText =  "Fast Individual Process Name",
					  						FullLocalDefaultText =  "מהיר/פרטני",
					  						ListFieldLable =  "FastIndividualProcessNameListLable",
					  						ListLableDefaultText =  "Fast Individual Process Name",
					  						ListLocalDefaultText =  "מהיר/פרטני",
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
					 
					 						FieldName =  "DecDangersContacts",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "List",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DecDangersContacts",
					  						ListPropertyPath =  "DecDangersContacts",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "Customs.DecDangersContact",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DecDangersContacts",
					  						DefaultText =  "Declaration Dangers Contacts",
					  						FullLocalDefaultText =  "אנשי קשר לחומרים מסוכנים",
					  						ListFieldLable =  "DecDangersContactsListLable",
					  						ListLableDefaultText =  "Declaration Dangers Contact",
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
					 
					 						FieldName =  "AmendmentRequestNumber",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  9,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  9,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AmendmentRequestNumber",
					  						ListPropertyPath =  "AmendmentRequestNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AmendmentRequestNumber",
					  						DefaultText =  "Amendment Request Number",
					  						FullLocalDefaultText =  "מס' בקשה",
					  						ListFieldLable =  "AmendmentRequestNumberListLable",
					  						ListLableDefaultText =  "Amendment Request Number",
					  						ListLocalDefaultText =  "מס' בקשה",
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
					 
					 						FieldName =  "AmendmentStatus",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.AmendmentStatus",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  2,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AmendmentStatus",
					  						ListPropertyPath =  "AmendmentStatus",
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
					  						FullFieldLable =  "AmendmentStatus",
					  						DefaultText =  "Amendment Status",
					  						FullLocalDefaultText =  "סטטוס תיקון הצהרה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AmendmentissueDate",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "AmendmentissueDate",
					  						ListPropertyPath =  "AmendmentissueDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AmendmentissueDate",
					  						DefaultText =  "Amendment Issue Date",
					  						FullLocalDefaultText =  "תאריך יצירת הבקשה",
					  						ListFieldLable =  "AmendmentissueDateListLable",
					  						ListLableDefaultText =  "Amendment Issue Date",
					  						ListLocalDefaultText =  "תאריך יצירת הבקשה",
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
					 
					 						FieldName =  "AmendmentRemarks",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  512,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  512,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AmendmentRemarks",
					  						ListPropertyPath =  "AmendmentRemarks",
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
					  						FullFieldLable =  "AmendmentRemarks",
					  						DefaultText =  "Amendment Remarks",
					  						FullLocalDefaultText =  "הערות לתיקון בקשה",
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
					 
					 						FieldName =  "AmendmentDeficitInitiated",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "AmendmentDeficitInitiated",
					  						ListPropertyPath =  "AmendmentDeficitInitiated",
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
					  						FullFieldLable =  "AmendmentDeficitInitiated",
					  						DefaultText =  "Amendment DeficitInitiated",
					  						FullLocalDefaultText =  "גרעון יזום",
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
					 
					 						FieldName =  "AmendDeficitInitiatedReasTo",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  512,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  512,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AmendDeficitInitiatedReasTo",
					  						ListPropertyPath =  "AmendDeficitInitiatedReasTo",
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
					  						FullFieldLable =  "AmendDeficitInitiatedReasTo",
					  						DefaultText =  "Amendment DeficitInitiated Reason To",
					  						FullLocalDefaultText =  "נימוקים לגרעון יזום",
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
					 
					 						FieldName =  "AmendmentCorrectedByUserId",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AmendmentCorrectedByUserId",
					  						ListPropertyPath =  "AmendmentCorrectedByUserId",
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
					  						FullFieldLable =  "AmendmentCorrectedByUserId",
					  						DefaultText =  "Amendment Corrected By User Id",
					  						FullLocalDefaultText =  "תיקון נפתח עי",
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
					 
					 						FieldName =  "AmendmentRejectionReason",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.AmendRequestRejectReasonType",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  3,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AmendmentRejectionReason",
					  						ListPropertyPath =  "AmendmentRejectionReason",
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
					  						FullFieldLable =  "AmendmentRejectionReason",
					  						DefaultText =  "Amendment Rejection Reason",
					  						FullLocalDefaultText =  "סיבת דחיה",
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
					 
					 						FieldName =  "IsAmendment",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "IsAmendment",
					  						ListPropertyPath =  "IsAmendment",
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
					  						FullFieldLable =  "IsAmendment",
					  						DefaultText =  "Is Amendment",
					  						FullLocalDefaultText =  "הצהרת תיקון",
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
					 
					 						FieldName =  "AmendmentOriginalDeclartation",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AmendmentOriginalDeclartation",
					  						ListPropertyPath =  "AmendmentOriginalDeclartation",
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
					  						FullFieldLable =  "AmendmentOriginalDeclartation",
					  						DefaultText =  "Amendment Original Declartation",
					  						FullLocalDefaultText =  "הצהרה מקורית",
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
					 
					 						FieldName =  "AmendmentCorrectedByUserName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AmendmentCorrectedByUserName",
					  						ListPropertyPath =  "AmendmentCorrectedByUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AmendmentCorrectedByUserName",
					  						DefaultText =  "Amendment Corrected By ",
					  						FullLocalDefaultText =  "תיקון נפתח עי",
					  						ListFieldLable =  "AmendmentCorrectedByUserNameListLable",
					  						ListLableDefaultText =  "Amendment Corrected By ",
					  						ListLocalDefaultText =  "תיקון נפתח עי",
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
					 
					 						FieldName =  "AmendmentStatusName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AmendmentStatusName",
					  						ListPropertyPath =  "AmendmentStatusName",
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
					  						FullFieldLable =  "AmendmentStatusName",
					  						DefaultText =  "Amendment Status Name",
					  						FullLocalDefaultText =  "סטטוס תיקון הצהרה",
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
					 
					 						FieldName =  "CourierManifestStatusCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CourierManifestStatusCode",
					  						ListPropertyPath =  "CourierManifestStatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationCourierStatus",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CourierManifestStatusCode",
					  						DefaultText =  "Courier Manifest Status",
					  						FullLocalDefaultText =  "מצהר",
					  						ListFieldLable =  "CourierManifestStatusCodeListLable",
					  						ListLableDefaultText =  "Courier Manifest Status",
					  						ListLocalDefaultText =  "מצהר",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  true,
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
					 
					 						FieldName =  "CourierPaymentStatusCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CourierPaymentStatusCode",
					  						ListPropertyPath =  "CourierPaymentStatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationCourierStatus",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CourierPaymentStatusCode",
					  						DefaultText =  "Courier Payment Status",
					  						FullLocalDefaultText =  "תשלום",
					  						ListFieldLable =  "CourierPaymentStatusCodeListLable",
					  						ListLableDefaultText =  "Courier Payment Status",
					  						ListLocalDefaultText =  "תשלום",
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
					 
					 						FieldName =  "IsPendingNotNull",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsPendingNotNull",
					  						ListPropertyPath =  "IsPendingNotNull",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsPendingNotNull",
					  						DefaultText =  "Pending",
					  						FullLocalDefaultText =  "Pending",
					  						ListFieldLable =  "IsPendingNotNullListLable",
					  						ListLableDefaultText =  "Pending",
					  						ListLocalDefaultText =  "Pending",
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
					 
					 						FieldName =  "IsDiamondDeclaration",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  1,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsDiamondDeclaration",
					  						ListPropertyPath =  "IsDiamondDeclaration",
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
					  						FullFieldLable =  "IsDiamondDeclaration",
					  						DefaultText =  "Is Diamond Declaration",
					  						FullLocalDefaultText =  "הצהרת יהלומים",
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
					 
					 						FieldName =  "AmendmentDontDisplayInList",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "AmendmentDontDisplayInList",
					  						ListPropertyPath =  "AmendmentDontDisplayInList",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AmendmentDontDisplayInList",
					  						DefaultText =  "Amendment Dont Display In List",
					  						FullLocalDefaultText =  "הצהרת תיקון לא להצגה",
					  						ListFieldLable =  "AmendmentDontDisplayInListListLable",
					  						ListLableDefaultText =  "Amendment Dont Display In List",
					  						ListLocalDefaultText =  "הצהרת תיקון לא להצגה",
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
					 
					 						FieldName =  "AmendmentMessage",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  200,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  200,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AmendmentMessage",
					  						ListPropertyPath =  "AmendmentMessage",
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
					  						FullFieldLable =  "AmendmentMessage",
					  						DefaultText =  "Amendment Message",
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
					 
					 						FieldName =  "IsAmendmentDisplayOnly",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "IsAmendmentDisplayOnly",
					  						ListPropertyPath =  "IsAmendmentDisplayOnly",
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
					  						FullFieldLable =  "IsAmendmentDisplayOnly",
					  						DefaultText =  "Is Amendment Display Only",
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
					 
					 						FieldName =  "IsMissMandatoryDiamond",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  10,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsMissMandatoryDiamond",
					  						ListPropertyPath =  "IsMissMandatoryDiamond",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsMissMandatoryDiamond",
					  						DefaultText =  "Is Missing Mandatory Fields",
					  						FullLocalDefaultText =  "שדות חובה בהצהרה",
					  						ListFieldLable =  "IsMissMandatoryDiamondListLable",
					  						ListLableDefaultText =  "Is Missing Mandatory Fields",
					  						ListLocalDefaultText =  "שדות חובה בהצהרה",
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
					 
					 						FieldName =  "IsValidTicketsDiamond",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  3,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsValidTicketsDiamond",
					  						ListPropertyPath =  "IsValidTicketsDiamond",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsValidTicketsDiamond",
					  						DefaultText =  "Document Status - Diamond",
					  						FullLocalDefaultText =  "סטטוס מסמכים - יהלומים",
					  						ListFieldLable =  "IsValidTicketsDiamondListLable",
					  						ListLableDefaultText =  "Document Status Diamond",
					  						ListLocalDefaultText =  "סטטוס מסמכים - יהלומים",
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
					 
					 						FieldName =  "CustomFileAmendment",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  12,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  12,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomFileAmendment",
					  						ListPropertyPath =  "CustomFileAmendment",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomFileAmendment",
					  						DefaultText =  "Custom File No.",
					  						FullLocalDefaultText =  "מספר תיק",
					  						ListFieldLable =  "CustomFileAmendmentListLable",
					  						ListLableDefaultText =  "Custom File No.",
					  						ListLocalDefaultText =  "מספר תיק",
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
					 
					 						FieldName =  "DeclarationNoAmendment",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  35,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  35,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeclarationNoAmendment",
					  						ListPropertyPath =  "DeclarationNoAmendment",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationNoAmendment",
					  						DefaultText =  "Declaration Number",
					  						FullLocalDefaultText =  "מספר הצהרה",
					  						ListFieldLable =  "DeclarationNoAmendmentListLable",
					  						ListLableDefaultText =  "Declaration Number",
					  						ListLocalDefaultText =  "מספר הצהרה",
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
					 
					 						FieldName =  "AvailabilityDate",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AvailabilityDate",
					  						ListPropertyPath =  "AvailabilityDate",
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
					  						FullFieldLable =  "AvailabilityDate",
					  						DefaultText =  "Availability Date",
					  						FullLocalDefaultText =  "תאריך זמינות",
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
					 
					 						FieldName =  "AmendmentNumber",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AmendmentNumber",
					  						ListPropertyPath =  "AmendmentNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AmendmentNumber",
					  						DefaultText =  "Amendment Number",
					  						FullLocalDefaultText =  "מספר תיקון",
					  						ListFieldLable =  "AmendmentNumberListLable",
					  						ListLableDefaultText =  "Amendment Number",
					  						ListLocalDefaultText =  "מספר תיקון",
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
					 
					 						FieldName =  "CourierPendingReasonName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CourierPendingReasonName",
					  						ListPropertyPath =  "CourierPendingReasonName",
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
					  						FullFieldLable =  "CourierPendingReasonName",
					  						DefaultText =  "Courier Pending Reason Name",
					  						FullLocalDefaultText =  "רשימת קודי עיכובים",
					  						ListFieldLable =  "CourierPendingReasonNameListLable",
					  						ListLableDefaultText =  "Courier Pending Reason Name",
					  						ListLocalDefaultText =  "רשימת קודי עיכובים",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HtmlListComponentUrl =  "./CustomsModules/CustomsListTemplates/Components/DeclarationListTemplate",
					  						HtmlListComponentName =  "DeclarationListTemplate",
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AutomaticPayment",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "AutomaticPayment",
					  						ListPropertyPath =  "AutomaticPayment",
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
					  						FullFieldLable =  "AutomaticPayment",
					  						DefaultText =  "Automatic Payment",
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
					 
					 						FieldName =  "LoadingDateTime",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "LoadingDateTime",
					  						ListPropertyPath =  "LoadingDateTime",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LoadingDateTime",
					  						DefaultText =  "Loading Date",
					  						FullLocalDefaultText =  "תאריך טעינה",
					  						ListFieldLable =  "LoadingDateTimeListLable",
					  						ListLableDefaultText =  "Loading Date",
					  						ListLocalDefaultText =  "תאריך טעינה",
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
					 
					 						FieldName =  "ShipCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CustomsShip",
					  						MinLength =  0,
					  						MaxLength =  25,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  25,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ShipCode",
					  						ListPropertyPath =  "ShipCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ShipCode",
					  						DefaultText =  "Ship Id",
					  						FullLocalDefaultText =  "כלי הובלה ימי",
					  						ListFieldLable =  "ShipCodeListLable",
					  						ListLableDefaultText =  "Ship Id ",
					  						ListLocalDefaultText =  "כלי הובלה ימי",
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
					 
					 						FieldName =  "IsExporterConfirmation",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsExporterConfirmation",
					  						ListPropertyPath =  "IsExporterConfirmation",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsExporterConfirmation",
					  						DefaultText =  "Exporter Confirmation",
					  						FullLocalDefaultText =  "אישור יצואן",
					  						ListFieldLable =  "IsExporterConfirmationListLable",
					  						ListLableDefaultText =  "Exporter Confirmation",
					  						ListLocalDefaultText =  "אישור יצואן",
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
					 
					 						FieldName =  "ShipName",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  300,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  300,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ShipName",
					  						ListPropertyPath =  "ShipName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ShipName",
					  						DefaultText =  "Ship",
					  						ListFieldLable =  "ShipNameListLable",
					  						ListLableDefaultText =  "Ship",
					  						ListLocalDefaultText =  "ספינה",
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
					 
					 						FieldName =  "DestinationCountryName",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  300,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  3000,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DestinationCountryName",
					  						ListPropertyPath =  "DestinationCountryName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DestinationCountryName",
					  						DefaultText =  "Destination Country",
					  						FullLocalDefaultText =  "ארץ יעד",
					  						ListFieldLable =  "DestinationCountryNameListLable",
					  						ListLableDefaultText =  "Destination Country",
					  						ListLocalDefaultText =  "ארץ יעד",
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
					 
					 						FieldName =  "DeclarationExportRecipients",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "DeclarationExportRecipients",
					  						ListPropertyPath =  "DeclarationExportRecipients",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "Customs.DeclarationExportRecipient",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationExportRecipients",
					  						DefaultText =  "Declaration Recipients",
					  						ListFieldLable =  "DeclarationExportRecipientsListLable",
					  						ListLableDefaultText =  "DeclarationExportRecipients",
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
					 
					 						FieldName =  "Direction",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Direction",
					  						ListPropertyPath =  "Direction",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Direction",
					  						DefaultText =  "Direction",
					  						FullLocalDefaultText =  "כיוון ",
					  						ListFieldLable =  "DirectionListLable",
					  						ListLableDefaultText =  "Direction",
					  						ListLocalDefaultText =  "כיוון ",
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
					 
					 						FieldName =  "AgentRoleCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1,
					  						DisplayInList =  true,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AgentRoleCode",
					  						ListPropertyPath =  "AgentRoleCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AgentRoleCode",
					  						DefaultText =  "Agent Role ",
					  						FullLocalDefaultText =  "תפקיד סוכן",
					  						ListFieldLable =  "AgentRoleCodeListLable",
					  						ListLableDefaultText =  "Agent Role ",
					  						ListLocalDefaultText =  "תפקיד סוכן",
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
					 
					 						FieldName =  "ExportFile",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ExportFile",
					  						ListPropertyPath =  "ExportFile",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ExportFile",
					  						DefaultText =  "Export File",
					  						FullLocalDefaultText =  "מס' תיק יצוא תפעולי",
					  						ListFieldLable =  "ExportFileListLable",
					  						ListLableDefaultText =  "Export File",
					  						ListLocalDefaultText =  "מס' תיק יצוא תפעולי",
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
					 
					 						FieldName =  "DestinationCountryCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CustomsCountry",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  2,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DestinationCountryCode",
					  						ListPropertyPath =  "DestinationCountryCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DestinationCountryCode",
					  						DefaultText =  "Destination Country",
					  						FullLocalDefaultText =  "ארץ יעד",
					  						ListFieldLable =  "DestinationCountryCodeListLable",
					  						ListLableDefaultText =  "Destination Country",
					  						ListLocalDefaultText =  "ארץ יעד",
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
					 
					 						FieldName =  "ExportAutonomyRegionTypeCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.AutonomyRegionType",
					  						MinLength =  0,
					  						MaxLength =  4,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  4,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ExportAutonomyRegionTypeCode",
					  						ListPropertyPath =  "ExportAutonomyRegionTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ExportAutonomyRegionTypeCode",
					  						DefaultText =  "Export Autonomy Region Type",
					  						FullLocalDefaultText =  "קוד איזור אוטונומיה",
					  						ListFieldLable =  "ExportAutonomyRegionTypeCodeListLable",
					  						ListLableDefaultText =  "Export Autonomy Region Type",
					  						ListLocalDefaultText =  "קוד איזור אוטונומיה",
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
					 
					 						FieldName =  "DeclarationTypeCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.LeadDocumentType",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  3,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeclarationTypeCode",
					  						ListPropertyPath =  "DeclarationTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationTypeCode",
					  						DefaultText =  "Declaration Type",
					  						FullLocalDefaultText =  "סוג הצהרה",
					  						ListFieldLable =  "DeclarationTypeCodeListLable",
					  						ListLableDefaultText =  "Declaration Type",
					  						ListLocalDefaultText =  "סוג הצהרה",
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
					 
					 						FieldName =  "CancelRequestReasonCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CancellationReasonRequestType",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  3,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CancelRequestReasonCode",
					  						ListPropertyPath =  "CancelRequestReasonCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CancelRequestReasonCode",
					  						DefaultText =  "Cancel Request Reason Code",
					  						FullLocalDefaultText =  "סיבת ביטול",
					  						ListFieldLable =  "CancelRequestReasonCodeListLable",
					  						ListLableDefaultText =  "Cancel Request Reason Code",
					  						ListLocalDefaultText =  "קוד סיבת ביטול",
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
					 
					 						FieldName =  "CancelRequestReasonExplanation",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  512,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  512,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CancelRequestReasonExplanation",
					  						ListPropertyPath =  "CancelRequestReasonExplanation",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CancellationReasonRequestType",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CancelRequestReasonExplanation",
					  						DefaultText =  "Cancel Request Reason Explanation",
					  						FullLocalDefaultText =  "נימוק ",
					  						ListFieldLable =  "CancelRequestReasonExplanationListLable",
					  						ListLableDefaultText =  "Cancel Request Reason Explanation",
					  						ListLocalDefaultText =  "נימוק",
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
					 
					 						FieldName =  "CancelRequestNumber",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Integer",
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
					  						PMPropertyPath =  "CancelRequestNumber",
					  						ListPropertyPath =  "CancelRequestNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CancelRequestNumber",
					  						DefaultText =  "Cancel Request Number",
					  						FullLocalDefaultText =  "מס' בקשה",
					  						ListFieldLable =  "CancelRequestNumberListLable",
					  						ListLableDefaultText =  "Cancel Request Number",
					  						ListLocalDefaultText =  "מס' בקשה",
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
					 
					 						FieldName =  "CustomCancelRequestRemarks",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  512,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  512,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomCancelRequestRemarks",
					  						ListPropertyPath =  "CustomCancelRequestRemarks",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomCancelRequestRemarks",
					  						DefaultText =  "Custom Cancel Request Remarks",
					  						FullLocalDefaultText =  "הערות מכס",
					  						ListFieldLable =  "CustomCancelRequestRemarksListLable",
					  						ListLableDefaultText =  "Custom Cancel Request Remarks",
					  						ListLocalDefaultText =  "הערות מכס לביטול",
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
					 
					 						FieldName =  "CancelRequestStatusCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CancellationRequestStatus",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  3,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CancelRequestStatusCode",
					  						ListPropertyPath =  "CancelRequestStatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CancelRequestStatusCode",
					  						DefaultText =  "Cancel Request Status Code",
					  						FullLocalDefaultText =  "סטטוס הבקשה",
					  						ListFieldLable =  "CancelRequestStatusCodeListLable",
					  						ListLableDefaultText =  "Cancel Request Status Code",
					  						ListLocalDefaultText =  "סטטוס ביטול הבקשה",
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
					 
					 						FieldName =  "CancelRequestRejectionReason",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CancelRequestRejectReasonType",
					  						MinLength =  0,
					  						MaxLength =  4,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  4,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CancelRequestRejectionReason",
					  						ListPropertyPath =  "CancelRequestRejectionReason",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CancelRequestRejectionReason",
					  						DefaultText =  "Cancel Request Rejection Reason",
					  						FullLocalDefaultText =  "סיבת דחיה",
					  						ListFieldLable =  "CancelRequestRejectionReasonListLable",
					  						ListLableDefaultText =  "Cancel Request Rejection Reason",
					  						ListLocalDefaultText =  "סיבת דחיה",
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
					 
					 						FieldName =  "CancelRequestApproveDate",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						PMPropertyPath =  "CancelRequestApproveDate",
					  						ListPropertyPath =  "CancelRequestApproveDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CancelRequestApproveDate",
					  						DefaultText =  "Cancel Request Approve Date",
					  						FullLocalDefaultText =  "תאריך ביטול",
					  						ListFieldLable =  "CancelRequestApproveDateListLable",
					  						ListLableDefaultText =  "Cancel Request Approve Date",
					  						ListLocalDefaultText =  "תאריך ביטול",
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
					 
					 						FieldName =  "IsClaimable",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsClaimable",
					  						ListPropertyPath =  "IsClaimable",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsClaimable",
					  						DefaultText =  "Is Claimable",
					  						FullLocalDefaultText =  "ניתן להגיש תביעה",
					  						ListFieldLable =  "IsClaimableListLable",
					  						ListLableDefaultText =  "Is Claimable",
					  						ListLocalDefaultText =  "ניתן להגיש תביעה",
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
					 
					 						FieldName =  "CancelRequestStatusName",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  200,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  200,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CancelRequestStatusName",
					  						ListPropertyPath =  "CancelRequestStatusName",
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
					  						FullFieldLable =  "CancelRequestStatusName",
					  						DefaultText =  "Cancel Request Status Code",
					  						FullLocalDefaultText =  "סטטוס ביטול הבקשה",
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
					 
					 						FieldName =  "ReplacingRepairRequest",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  9,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  9,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ReplacingRepairRequest",
					  						ListPropertyPath =  "ReplacingRepairRequest",
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
					  						FullFieldLable =  "ReplacingRepairRequest",
					  						DefaultText =  "Replacing Repair Request",
					  						FullLocalDefaultText =  "מס' בקשה מוחלפת",
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
					 
					 						FieldName =  "AmendmentErrorXml",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  4000,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  4000,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AmendmentErrorXml",
					  						ListPropertyPath =  "AmendmentErrorXml",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AmendmentErrorXml",
					  						DefaultText =  "Amendment Error Xml",
					  						FullLocalDefaultText =  "שגיאות - תיקון הצהרה",
					  						ListFieldLable =  "AmendmentErrorXmlListLable",
					  						ListLableDefaultText =  "Amendment Error Xml",
					  						ListLocalDefaultText =  "שגיאות - תיקון הצהרה",
					  						IsMaxLength =  true,
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
					 
					 						FieldName =  "FOBValueNIS",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Decimal",
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
					  						PMPropertyPath =  "FOBValueNIS",
					  						ListPropertyPath =  "FOBValueNIS",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  16,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FOBValueNIS",
					  						DefaultText =  "FOB Value NIS",
					  						FullLocalDefaultText =  "סה\\\"כ ערך FOB",
					  						ListFieldLable =  "FOBValueNISListLable",
					  						ListLableDefaultText =  "FOB Value NIS",
					  						ListLocalDefaultText =  "סה\\\"כ ערך FOB",
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
					 
					 						FieldName =  "FOBValueDollar",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Decimal",
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
					  						PMPropertyPath =  "FOBValueDollar",
					  						ListPropertyPath =  "FOBValueDollar",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  16,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FOBValueDollar",
					  						DefaultText =  "FOB Value Dollar",
					  						FullLocalDefaultText =  "סה\\\"כ ערך FOB בדולר",
					  						ListFieldLable =  "FOBValueDollarListLable",
					  						ListLableDefaultText =  "FOB Value Dollar",
					  						ListLocalDefaultText =  "סה\\\"כ ערך FOB בדולר",
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
					 
					 						FieldName =  "CreateDateForExport",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToRoutingString",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CreateDateForExport",
					  						ListPropertyPath =  "CreateDateForExport",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreateDateForExport",
					  						DefaultText =  "Create Date Time",
					  						FullLocalDefaultText =  "תאריך פתיחת הצהרה",
					  						ListFieldLable =  "CreateDateForExportListLable",
					  						ListLableDefaultText =  "Create Date Time",
					  						ListLocalDefaultText =  "תאריך פתיחת הצהרה",
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
					 
					 						FieldName =  "TransportModeForExport",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  10,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  10,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TransportModeForExport",
					  						ListPropertyPath =  "TransportModeForExport",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						ColumnHeaderTemplateName =  "TransportModeListHeaderTemplate",
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransportModeForExport",
					  						DefaultText =  "Transport Mode",
					  						ListFieldLable =  "TransportModeForExportListLable",
					  						ListLableDefaultText =  "Transport Mode",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomFileForExport",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  12,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  12,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomFileForExport",
					  						ListPropertyPath =  "CustomFileForExport",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomFileForExport",
					  						DefaultText =  "Custom File",
					  						FullLocalDefaultText =  "תיק מכס",
					  						ListFieldLable =  "CustomFileForExportListLable",
					  						ListLableDefaultText =  "Custom File",
					  						ListLocalDefaultText =  "תיק מכס",
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
					 
					 						FieldName =  "AmendmentRejectionReasonName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AmendmentRejectionReasonName",
					  						ListPropertyPath =  "AmendmentRejectionReasonName",
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
					  						FullFieldLable =  "AmendmentRejectionReasonName",
					  						DefaultText =  "Amendment Rejection Reason",
					  						FullLocalDefaultText =  "סיבת דחיה",
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
					 
					 						FieldName =  "TransshipmentApprovalDateTime",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TransshipmentApprovalDateTime",
					  						ListPropertyPath =  "TransshipmentApprovalDateTime",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransshipmentApprovalDateTime",
					  						DefaultText =  "Transshipment Approval Date",
					  						FullLocalDefaultText =  "תאריך אישור שטעון באתר הראשון",
					  						ListFieldLable =  "TransshipmentApprovalDateTimeListLable",
					  						ListLableDefaultText =  "Transshipment Approval Date",
					  						ListLocalDefaultText =  "תאריך אישור שטעון באתר הראשון",
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
					 
					 						FieldName =  "FinalLoadingSite",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.LoadingSiteType",
					  						MinLength =  0,
					  						MaxLength =  10,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  10,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FinalLoadingSite",
					  						ListPropertyPath =  "FinalLoadingSite",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FinalLoadingSite",
					  						DefaultText =  "Final Loading Site",
					  						FullLocalDefaultText =  "אתר טעינה אחרון בפועל",
					  						ListFieldLable =  "FinalLoadingSiteListLable",
					  						ListLableDefaultText =  "Final Loading Site",
					  						ListLocalDefaultText =  "אתר טעינה אחרון בפועל",
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
					 
					 						FieldName =  "PalestinianCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PalestinianCode",
					  						ListPropertyPath =  "PalestinianCode",
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
					  						FullFieldLable =  "PalestinianCode",
					  						DefaultText =  "Palestinian Code",
					  						FullLocalDefaultText =  "מס' יבואן פלסטינאי",
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
					 
					 						FieldName =  "RequestedCustomsDocId",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Integer",
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
					  						PMPropertyPath =  "RequestedCustomsDocId",
					  						ListPropertyPath =  "RequestedCustomsDocId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "RequestedCustomsDocId",
					  						DefaultText =  "Requested Customs DocId",
					  						FullLocalDefaultText =  "מסמך נדרש",
					  						ListFieldLable =  "RequestedCustomsDocIdListLable",
					  						ListLableDefaultText =  "מסמך נדרש",
					  						ListLocalDefaultText =  "Requested Customs DocId",
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
					 
					 						FieldName =  "ExportDeclarationOfficeCode",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  17,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  17,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ExportDeclarationOfficeCode",
					  						ListPropertyPath =  "ExportDeclarationOfficeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ExportDeclarationOfficeCode",
					  						DefaultText =  "Export Declaration Office Code",
					  						FullLocalDefaultText =  "בית מכס מייצא",
					  						ListFieldLable =  "ExportDeclarationOfficeCodeListLable",
					  						ListLableDefaultText =  "Export Declaration Office Code",
					  						ListLocalDefaultText =  "בית מכס מייצא",
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
					 
					 						FieldName =  "PhysicalCheck",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PhysicalCheck",
					  						ListPropertyPath =  "PhysicalCheck",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PhysicalCheck",
					  						DefaultText =  "Physical Check",
					  						FullLocalDefaultText =  "בדיקה פיזית",
					  						ListFieldLable =  "PhysicalCheckListLable",
					  						ListLableDefaultText =  "Physical Check",
					  						ListLocalDefaultText =  "בדיקה פיזית",
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
					 
					 						FieldName =  "FinalLoadingSiteName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FinalLoadingSiteName",
					  						ListPropertyPath =  "FinalLoadingSiteName",
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
					  						FullFieldLable =  "FinalLoadingSiteName",
					  						DefaultText =  "Final Loading Site Name",
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
					 
					 						FieldName =  "ShipCodeName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ShipCodeName",
					  						ListPropertyPath =  "ShipCodeName",
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
					  						FullFieldLable =  "ShipCodeName",
					  						DefaultText =  "ShipCodeName",
					  						FullLocalDefaultText =  "כלי הובלה ימי",
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
					 
					 						FieldName =  "CancelRejectionReasonName",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CancelRejectionReasonName",
					  						ListPropertyPath =  "CancelRejectionReasonName",
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
					  						FullFieldLable =  "CancelRejectionReasonName",
					  						DefaultText =  "Cancel Request Rejection Reason Name",
					  						FullLocalDefaultText =  "סיבת דחיה",
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
					 
					 						FieldName =  "ExportContainerizationID",
					  						ObjectTableName =  "Customs.Declaration",
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
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ExportContainerizationID",
					  						ListPropertyPath =  "ExportContainerizationID",
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
					  						FullFieldLable =  "ExportContainerizationID",
					  						DefaultText =  "Export Containerization ID",
					  						FullLocalDefaultText =  "מזהה ההמכלה",
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
					 
					 						FieldName =  "CargoTypeName",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CargoTypeName",
					  						ListPropertyPath =  "CargoTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CargoTypeName",
					  						DefaultText =  "Cargo Type Name",
					  						FullLocalDefaultText =  "סוג מזהה מטען",
					  						ListFieldLable =  "CargoTypeNameListLable",
					  						ListLableDefaultText =  "Cargo Type Name",
					  						ListLocalDefaultText =  "סוג מזהה מטען",
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
					 
					 						FieldName =  "SecondCargoID",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  35,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  35,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SecondCargoID",
					  						ListPropertyPath =  "SecondCargoID",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SecondCargoID",
					  						DefaultText =  "Second Cargo ID",
					  						FullLocalDefaultText =  "מזהה מטען שני",
					  						ListFieldLable =  "SecondCargoIDListLable",
					  						ListLableDefaultText =  "Second Cargo ID",
					  						ListLocalDefaultText =  "מזהה מטען שני",
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
					 
					 						FieldName =  "ThirdCargoID",
					  						ObjectTableName =  "Customs.Declaration",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  35,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  35,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ThirdCargoID",
					  						ListPropertyPath =  "ThirdCargoID",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ThirdCargoID",
					  						DefaultText =  "Third Cargo ID",
					  						FullLocalDefaultText =  "מזהה מטען שלישי",
					  						ListFieldLable =  "ThirdCargoIDListLable",
					  						ListLableDefaultText =  "Third Cargo ID",
					  						ListLocalDefaultText =  "מזהה מטען שלישי",
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
	        QueryGroup DeclarationQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "DECL", Name = "Customs.CourierMaster" }, queryGroupRepository,tenantQueryGroups);
						QueryGroup DeclarationQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "DECL", Name = "Customs.Declaration" }, queryGroupRepository,tenantQueryGroups);
				        queryGroupRepository.SubmitChanges();
	        ObjectTable DeclarationObjectTable = objectTables.ContainsKey("Customs.Declaration") ? objectTables["Customs.Declaration"] : null;
            if (DeclarationObjectTable == null)
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(0);  

                DeclarationObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Customs.Declaration" && d.Tenant == 0).FirstOrDefault();
            }

	         
			List<Feature> addedFeatures = new List<Feature>();
			List<TextCode> addedTextCodes = new List<TextCode>();
			List<Query> addedQueries = new List<Query>();
			List<QueryColumn> addedQueryColumns = new List<QueryColumn>();
			List<AdvancedQueryFilter> addedQueryFilters = new List<AdvancedQueryFilter>();
   

			   TextCode DeclarationTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.CourierMasterOpen", DefaultText = @"Courier Master Open",LocalDefaultText = "שטרי מטען בלדר פתוחים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Declaration.Q.OpenCourierMaster", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationFeatures.OpenCourierMaster", NameTextCodeDefaultText = "OpenCourierMaster", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.UnReleasedFastProcess", DefaultText = @"UnReleased Fast Process",LocalDefaultText = "לא שוחררו מכס מהיר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Declaration.Q.UnReleasedFastProcess", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationFeatures.UnReleasedFastProcess", NameTextCodeDefaultText = "UnReleasedFastProcess", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.CourierMasterOpenIndividual", DefaultText = @"Courier Master Open Individual",LocalDefaultText = "שטרי מטען פרטניים פתוחים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Declaration.Q.CourierMasterOpenIndividual", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationFeatures.CourierMasterOpenIndividual", NameTextCodeDefaultText = "CourierMasterOpenIndividual", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationTextCode_3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.UnReleasedIndividual", DefaultText = @"UnReleased Fast Process",LocalDefaultText = "לא שוחררו מכס פרטני", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationFeature_3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Declaration.Q.UnReleasedIndividual", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationFeatures.UnReleasedIndividual", NameTextCodeDefaultText = "UnReleasedIndividual", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationTextCode_4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.WithoutId", DefaultText = @"Without Id",LocalDefaultText = "ללא תעודת זהות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationFeature_4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Declaration.Q.WithoutId", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationFeatures.WithoutId", NameTextCodeDefaultText = "WithoutId", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationTextCode_5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.WithoutClassification", DefaultText = @"Without Classification",LocalDefaultText = "ללא סיווג", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationFeature_5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Declaration.Q.WithoutClassification", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationFeatures.WithoutClassification", NameTextCodeDefaultText = "WithoutClassification", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationTextCode_6 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.PendingPayment", DefaultText = @"Pending Payment",LocalDefaultText = "מעוכב גביה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationFeature_6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Declaration.Q.PendingPayment", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationFeatures.PendingPayment", NameTextCodeDefaultText = "PendingPayment", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationTextCode_7 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.PendingCustoms", DefaultText = @"Pending Customs",LocalDefaultText = "מעוכב מכס", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationFeature_7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Declaration.Q.PendingCustoms", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationFeatures.PendingCustoms", NameTextCodeDefaultText = "PendingCustoms", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationTextCode_8 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.Pending", DefaultText = @"Pending",LocalDefaultText = "Pending", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationFeature_8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Declaration.Q.Pending", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.CourierMaster.Features.CourierMasters", NameTextCodeDefaultText = "CourierMaster", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationTextCode_9 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.Q.DeclarationWithoutReleaseQuery", DefaultText = @"Open Declarations",LocalDefaultText = "הצהרות פתוחות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationFeature_9 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Declaration.Q.DeclarationWithoutRelease", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationFeatures.DeclarationWithoutRelease", NameTextCodeDefaultText = "DeclarationWithoutRelease", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationTextCode_10 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.Q.DeclaratioInConstraintQuery", DefaultText = @"Declarations in Constraint",LocalDefaultText = " אילוצים ללא תשובה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationFeature_10 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Declaration.Q.DeclarationInConstraint", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationFeatures.DeclarationInConstraint", NameTextCodeDefaultText = "DeclarationInConstraint", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationTextCode_11 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.Q.PaidDeclarationWithoutReleaseQuery", DefaultText = @"Paid Declarations Without Release",LocalDefaultText = "הגשות ללא תשובה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationFeature_11 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Declaration.Q.PaidDeclaration", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationFeatures.PaidDeclaration", NameTextCodeDefaultText = "PaidDeclaration", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationTextCode_12 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Declaration.Q.DeclarationAmendments", DefaultText = @"Declaration Amendments In Process",LocalDefaultText = "תיקוני הצהרה בתהליך", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationFeature_12 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Declaration.Q.DeclarationAmendments", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Declaration.Features.DeclarationAmendments", NameTextCodeDefaultText = "DeclarationAmendments", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationTextCode_13 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.Q.DeclarationQuery", DefaultText = @"All Declarations",LocalDefaultText = "כל ההצהרות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationFeature_13 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Declaration.Q.Declarations", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationFeatures.Declarations", NameTextCodeDefaultText = "Declarations", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationTextCode_14 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.Q.ExportDeclarationQuery", DefaultText = @"Export Declaration",LocalDefaultText = "הצהרות יצוא", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationFeature_14 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Declaration.Q.ExportDeclaration", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationFeatures.ExportDeclaration", NameTextCodeDefaultText = "ExportDeclaration", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationTextCode_15 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Declaration.Q.AllCourierDeclarations", DefaultText = @"All Courier Declarations",LocalDefaultText = "כל ההצהרות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationFeature_15 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Declaration.Q.AllCourierDeclarations", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationFeatures.AllCourierDeclarations", NameTextCodeDefaultText = "AllCourierDeclarations", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationObjectTable, addedFeatures, addedTextCodes);

	        //TextCodeRepository.SubmitChanges();
	        //FeaturesRepository.SubmitChanges();    
	      

			  Query OpenCourierMasterQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationTextCode_0.Id, NameTextCodeCode = DeclarationTextCode_0.Code, ObjectTableName = "Customs.Declaration", Code = "OpenCourierMaster",  QueryGroupCode = "VHQG", IndexOrder = 0, Tenant = 0, ObjectTableId = DeclarationObjectTable.Id, QuerySection = "Customs.Declaration", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationFeature_0.Id,FeatureUniqeCode= DeclarationFeature_0.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, addedQueries);
	
			 QueryColumn OpenCourierMasterQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenCourierMasterQuery.Id,QueryCode = OpenCourierMasterQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.Declaration.CustomFileNo" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn OpenCourierMasterQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenCourierMasterQuery.Id,QueryCode = OpenCourierMasterQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.Declaration.CourierHAWB" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn OpenCourierMasterQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenCourierMasterQuery.Id,QueryCode = OpenCourierMasterQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.Declaration.FastIndividualProcessCode" , ColumnWidth = 140 }, addedQueryColumns);

			 QueryColumn OpenCourierMasterQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenCourierMasterQuery.Id,QueryCode = OpenCourierMasterQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.Declaration.CustomerName" , ColumnWidth = 160 }, addedQueryColumns);

			 QueryColumn OpenCourierMasterQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenCourierMasterQuery.Id,QueryCode = OpenCourierMasterQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.Declaration.DeclarationNumber" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn OpenCourierMasterQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenCourierMasterQuery.Id,QueryCode = OpenCourierMasterQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.Declaration.ProcedureCurrentName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn OpenCourierMasterQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenCourierMasterQuery.Id,QueryCode = OpenCourierMasterQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.Declaration.TotalInvoiceAmountInUSD" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn OpenCourierMasterQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenCourierMasterQuery.Id,QueryCode = OpenCourierMasterQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.Declaration.DeclarationStatusTypeName" , ColumnWidth = 190 }, addedQueryColumns);

			 QueryColumn OpenCourierMasterQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenCourierMasterQuery.Id,QueryCode = OpenCourierMasterQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "Customs.Declaration.CourierCustomStatusName" , ColumnWidth = 180 }, addedQueryColumns);

			 QueryColumn OpenCourierMasterQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenCourierMasterQuery.Id,QueryCode = OpenCourierMasterQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "Customs.Declaration.CourierSuspentionName" , ColumnWidth = 180 }, addedQueryColumns);

             AdvancedQueryFilter OpenCourierMasterQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsClosedForFollowUp", PredefinedValue = "false",PredefinedValue2 = null, QueryId = OpenCourierMasterQuery.Id,QueryCode = OpenCourierMasterQuery.UniqueCode, Tenant = 0,Operator = "Equals"}, addedQueryFilters);


             AdvancedQueryFilter OpenCourierMasterQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsCourierDeclaration", PredefinedValue = "true",PredefinedValue2 = null, QueryId = OpenCourierMasterQuery.Id,QueryCode = OpenCourierMasterQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query UnReleasedFastProcessQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationTextCode_1.Id, NameTextCodeCode = DeclarationTextCode_1.Code, ObjectTableName = "Customs.Declaration", Code = "UnReleasedFastProcess",  QueryGroupCode = "VHQG", IndexOrder = 1, Tenant = 0, ObjectTableId = DeclarationObjectTable.Id, QuerySection = "Customs.Declaration", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationFeature_1.Id,FeatureUniqeCode= DeclarationFeature_1.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, addedQueries);
	
			 QueryColumn UnReleasedFastProcessQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnReleasedFastProcessQuery.Id,QueryCode = UnReleasedFastProcessQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.Declaration.CustomFileNo" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn UnReleasedFastProcessQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnReleasedFastProcessQuery.Id,QueryCode = UnReleasedFastProcessQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.Declaration.CourierHAWB" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn UnReleasedFastProcessQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnReleasedFastProcessQuery.Id,QueryCode = UnReleasedFastProcessQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.Declaration.CustomerName" , ColumnWidth = 160 }, addedQueryColumns);

			 QueryColumn UnReleasedFastProcessQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnReleasedFastProcessQuery.Id,QueryCode = UnReleasedFastProcessQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.Declaration.DeclarationNumber" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn UnReleasedFastProcessQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnReleasedFastProcessQuery.Id,QueryCode = UnReleasedFastProcessQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.Declaration.ProcedureCurrentName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn UnReleasedFastProcessQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnReleasedFastProcessQuery.Id,QueryCode = UnReleasedFastProcessQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.Declaration.TotalInvoiceAmountInUSD" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn UnReleasedFastProcessQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnReleasedFastProcessQuery.Id,QueryCode = UnReleasedFastProcessQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.Declaration.DeclarationStatusTypeName" , ColumnWidth = 190 }, addedQueryColumns);

			 QueryColumn UnReleasedFastProcessQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnReleasedFastProcessQuery.Id,QueryCode = UnReleasedFastProcessQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.Declaration.CourierCustomStatusName" , ColumnWidth = 180 }, addedQueryColumns);

			 QueryColumn UnReleasedFastProcessQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnReleasedFastProcessQuery.Id,QueryCode = UnReleasedFastProcessQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "Customs.Declaration.CourierSuspentionName" , ColumnWidth = 180 }, addedQueryColumns);

			 QueryColumn UnReleasedFastProcessQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnReleasedFastProcessQuery.Id,QueryCode = UnReleasedFastProcessQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "Customs.Declaration.AcceptanceStatusName" , ColumnWidth = 140 }, addedQueryColumns);

             AdvancedQueryFilter UnReleasedFastProcessQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.FastIndividualProcessCode", PredefinedValue = "F",PredefinedValue2 = null, QueryId = UnReleasedFastProcessQuery.Id,QueryCode = UnReleasedFastProcessQuery.UniqueCode, Tenant = 0,Operator = "Equals"}, addedQueryFilters);


             AdvancedQueryFilter UnReleasedFastProcessQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.HatraDate", PredefinedValue = "",PredefinedValue2 = null, QueryId = UnReleasedFastProcessQuery.Id,QueryCode = UnReleasedFastProcessQuery.UniqueCode, Tenant = 0,Operator = "Equals"}, addedQueryFilters);


             AdvancedQueryFilter UnReleasedFastProcessQueryFilter_2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsCourierDeclaration", PredefinedValue = "true",PredefinedValue2 = null, QueryId = UnReleasedFastProcessQuery.Id,QueryCode = UnReleasedFastProcessQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query CourierMasterOpenIndividualQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationTextCode_2.Id, NameTextCodeCode = DeclarationTextCode_2.Code, ObjectTableName = "Customs.Declaration", Code = "CourierMasterOpenIndividual",  QueryGroupCode = "VHQG", IndexOrder = 2, Tenant = 0, ObjectTableId = DeclarationObjectTable.Id, QuerySection = "Customs.Declaration", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationFeature_2.Id,FeatureUniqeCode= DeclarationFeature_2.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, addedQueries);
	
			 QueryColumn CourierMasterOpenIndividualQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CourierMasterOpenIndividualQuery.Id,QueryCode = CourierMasterOpenIndividualQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.Declaration.CustomFileNo" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn CourierMasterOpenIndividualQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CourierMasterOpenIndividualQuery.Id,QueryCode = CourierMasterOpenIndividualQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.Declaration.CourierHAWB" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn CourierMasterOpenIndividualQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CourierMasterOpenIndividualQuery.Id,QueryCode = CourierMasterOpenIndividualQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.Declaration.CustomerName" , ColumnWidth = 160 }, addedQueryColumns);

			 QueryColumn CourierMasterOpenIndividualQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CourierMasterOpenIndividualQuery.Id,QueryCode = CourierMasterOpenIndividualQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.Declaration.DeclarationNumber" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn CourierMasterOpenIndividualQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CourierMasterOpenIndividualQuery.Id,QueryCode = CourierMasterOpenIndividualQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.Declaration.ProcedureCurrentName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn CourierMasterOpenIndividualQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CourierMasterOpenIndividualQuery.Id,QueryCode = CourierMasterOpenIndividualQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.Declaration.TotalInvoiceAmountInUSD" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn CourierMasterOpenIndividualQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CourierMasterOpenIndividualQuery.Id,QueryCode = CourierMasterOpenIndividualQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.Declaration.DeclarationStatusTypeName" , ColumnWidth = 190 }, addedQueryColumns);

             AdvancedQueryFilter CourierMasterOpenIndividualQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsClosedForFollowUp", PredefinedValue = "false",PredefinedValue2 = null, QueryId = CourierMasterOpenIndividualQuery.Id,QueryCode = CourierMasterOpenIndividualQuery.UniqueCode, Tenant = 0,Operator = "Equals"}, addedQueryFilters);


             AdvancedQueryFilter CourierMasterOpenIndividualQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.FastIndividualProcessCode", PredefinedValue = "I",PredefinedValue2 = null, QueryId = CourierMasterOpenIndividualQuery.Id,QueryCode = CourierMasterOpenIndividualQuery.UniqueCode, Tenant = 0,Operator = "Equals"}, addedQueryFilters);


             AdvancedQueryFilter CourierMasterOpenIndividualQueryFilter_2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsCourierDeclaration", PredefinedValue = "true",PredefinedValue2 = null, QueryId = CourierMasterOpenIndividualQuery.Id,QueryCode = CourierMasterOpenIndividualQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query UnReleasedIndividualQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationTextCode_3.Id, NameTextCodeCode = DeclarationTextCode_3.Code, ObjectTableName = "Customs.Declaration", Code = "UnReleasedIndividual",  QueryGroupCode = "VHQG", IndexOrder = 3, Tenant = 0, ObjectTableId = DeclarationObjectTable.Id, QuerySection = "Customs.Declaration", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationFeature_3.Id,FeatureUniqeCode= DeclarationFeature_3.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, addedQueries);
	
			 QueryColumn UnReleasedIndividualQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnReleasedIndividualQuery.Id,QueryCode = UnReleasedIndividualQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.Declaration.CustomFileNo" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn UnReleasedIndividualQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnReleasedIndividualQuery.Id,QueryCode = UnReleasedIndividualQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.Declaration.CourierHAWB" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn UnReleasedIndividualQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnReleasedIndividualQuery.Id,QueryCode = UnReleasedIndividualQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.Declaration.CustomerName" , ColumnWidth = 160 }, addedQueryColumns);

			 QueryColumn UnReleasedIndividualQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnReleasedIndividualQuery.Id,QueryCode = UnReleasedIndividualQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.Declaration.DeclarationNumber" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn UnReleasedIndividualQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnReleasedIndividualQuery.Id,QueryCode = UnReleasedIndividualQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.Declaration.ProcedureCurrentName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn UnReleasedIndividualQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnReleasedIndividualQuery.Id,QueryCode = UnReleasedIndividualQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.Declaration.TotalInvoiceAmountInUSD" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn UnReleasedIndividualQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnReleasedIndividualQuery.Id,QueryCode = UnReleasedIndividualQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.Declaration.DeclarationStatusTypeName" , ColumnWidth = 190 }, addedQueryColumns);

			 QueryColumn UnReleasedIndividualQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnReleasedIndividualQuery.Id,QueryCode = UnReleasedIndividualQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.Declaration.CourierCustomStatusName" , ColumnWidth = 180 }, addedQueryColumns);

			 QueryColumn UnReleasedIndividualQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnReleasedIndividualQuery.Id,QueryCode = UnReleasedIndividualQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "Customs.Declaration.CourierSuspentionName" , ColumnWidth = 180 }, addedQueryColumns);

			 QueryColumn UnReleasedIndividualQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnReleasedIndividualQuery.Id,QueryCode = UnReleasedIndividualQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "Customs.Declaration.AcceptanceStatusName" , ColumnWidth = 140 }, addedQueryColumns);

             AdvancedQueryFilter UnReleasedIndividualQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.FastIndividualProcessCode", PredefinedValue = "I",PredefinedValue2 = null, QueryId = UnReleasedIndividualQuery.Id,QueryCode = UnReleasedIndividualQuery.UniqueCode, Tenant = 0,Operator = "Equals"}, addedQueryFilters);


             AdvancedQueryFilter UnReleasedIndividualQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.HatraDate", PredefinedValue = "",PredefinedValue2 = null, QueryId = UnReleasedIndividualQuery.Id,QueryCode = UnReleasedIndividualQuery.UniqueCode, Tenant = 0,Operator = "Equals"}, addedQueryFilters);


             AdvancedQueryFilter UnReleasedIndividualQueryFilter_2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsCourierDeclaration", PredefinedValue = "true",PredefinedValue2 = null, QueryId = UnReleasedIndividualQuery.Id,QueryCode = UnReleasedIndividualQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query WithoutIdQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationTextCode_4.Id, NameTextCodeCode = DeclarationTextCode_4.Code, ObjectTableName = "Customs.Declaration", Code = "WithoutId",  QueryGroupCode = "VHQG", IndexOrder = 4, Tenant = 0, ObjectTableId = DeclarationObjectTable.Id, QuerySection = "Customs.Declaration", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationFeature_4.Id,FeatureUniqeCode= DeclarationFeature_4.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, addedQueries);
	
			 QueryColumn WithoutIdQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WithoutIdQuery.Id,QueryCode = WithoutIdQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.Declaration.MAWB" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn WithoutIdQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WithoutIdQuery.Id,QueryCode = WithoutIdQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.Declaration.CustomFileNo" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn WithoutIdQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WithoutIdQuery.Id,QueryCode = WithoutIdQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.Declaration.CourierHAWB" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn WithoutIdQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WithoutIdQuery.Id,QueryCode = WithoutIdQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.Declaration.CustomerName" , ColumnWidth = 160 }, addedQueryColumns);

			 QueryColumn WithoutIdQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WithoutIdQuery.Id,QueryCode = WithoutIdQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.Declaration.DeclarationNumber" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn WithoutIdQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WithoutIdQuery.Id,QueryCode = WithoutIdQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.Declaration.ProcedureCurrentName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn WithoutIdQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WithoutIdQuery.Id,QueryCode = WithoutIdQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.Declaration.TotalInvoiceAmountInUSD" , ColumnWidth = 150 }, addedQueryColumns);

             AdvancedQueryFilter WithoutIdQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsClosedForFollowUp", PredefinedValue = "false",PredefinedValue2 = null, QueryId = WithoutIdQuery.Id,QueryCode = WithoutIdQuery.UniqueCode, Tenant = 0,Operator = "Equals"}, addedQueryFilters);


             AdvancedQueryFilter WithoutIdQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsPending902", PredefinedValue = "true",PredefinedValue2 = null, QueryId = WithoutIdQuery.Id,QueryCode = WithoutIdQuery.UniqueCode, Tenant = 0,Operator = "Equals"}, addedQueryFilters);


             AdvancedQueryFilter WithoutIdQueryFilter_2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsCourierDeclaration", PredefinedValue = "true",PredefinedValue2 = null, QueryId = WithoutIdQuery.Id,QueryCode = WithoutIdQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query WithoutClassificationQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationTextCode_5.Id, NameTextCodeCode = DeclarationTextCode_5.Code, ObjectTableName = "Customs.Declaration", Code = "WithoutClassification",  QueryGroupCode = "VHQG", IndexOrder = 5, Tenant = 0, ObjectTableId = DeclarationObjectTable.Id, QuerySection = "Customs.Declaration", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationFeature_5.Id,FeatureUniqeCode= DeclarationFeature_5.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, addedQueries);
	
			 QueryColumn WithoutClassificationQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WithoutClassificationQuery.Id,QueryCode = WithoutClassificationQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.Declaration.MAWB" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn WithoutClassificationQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WithoutClassificationQuery.Id,QueryCode = WithoutClassificationQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.Declaration.CustomFileNo" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn WithoutClassificationQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WithoutClassificationQuery.Id,QueryCode = WithoutClassificationQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.Declaration.CourierHAWB" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn WithoutClassificationQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WithoutClassificationQuery.Id,QueryCode = WithoutClassificationQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.Declaration.CustomerName" , ColumnWidth = 160 }, addedQueryColumns);

			 QueryColumn WithoutClassificationQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WithoutClassificationQuery.Id,QueryCode = WithoutClassificationQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.Declaration.DeclarationNumber" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn WithoutClassificationQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WithoutClassificationQuery.Id,QueryCode = WithoutClassificationQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.Declaration.ProcedureCurrentName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn WithoutClassificationQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WithoutClassificationQuery.Id,QueryCode = WithoutClassificationQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.Declaration.TotalInvoiceAmountInUSD" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn WithoutClassificationQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WithoutClassificationQuery.Id,QueryCode = WithoutClassificationQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.Declaration.CargoDescription" , ColumnWidth = 190 }, addedQueryColumns);

             AdvancedQueryFilter WithoutClassificationQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsCourierMissingClassification", PredefinedValue = "true",PredefinedValue2 = null, QueryId = WithoutClassificationQuery.Id,QueryCode = WithoutClassificationQuery.UniqueCode, Tenant = 0,Operator = "Equals"}, addedQueryFilters);


             AdvancedQueryFilter WithoutClassificationQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsCourierDeclaration", PredefinedValue = "true",PredefinedValue2 = null, QueryId = WithoutClassificationQuery.Id,QueryCode = WithoutClassificationQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query PendingPaymentQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationTextCode_6.Id, NameTextCodeCode = DeclarationTextCode_6.Code, ObjectTableName = "Customs.Declaration", Code = "PendingPayment",  QueryGroupCode = "VHQG", IndexOrder = 6, Tenant = 0, ObjectTableId = DeclarationObjectTable.Id, QuerySection = "Customs.Declaration", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationFeature_6.Id,FeatureUniqeCode= DeclarationFeature_6.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, addedQueries);
	
			 QueryColumn PendingPaymentQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingPaymentQuery.Id,QueryCode = PendingPaymentQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.Declaration.MAWB" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn PendingPaymentQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingPaymentQuery.Id,QueryCode = PendingPaymentQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.Declaration.CustomFileNo" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn PendingPaymentQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingPaymentQuery.Id,QueryCode = PendingPaymentQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.Declaration.CourierHAWB" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn PendingPaymentQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingPaymentQuery.Id,QueryCode = PendingPaymentQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.Declaration.CustomerName" , ColumnWidth = 160 }, addedQueryColumns);

			 QueryColumn PendingPaymentQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingPaymentQuery.Id,QueryCode = PendingPaymentQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.Declaration.DeclarationNumber" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn PendingPaymentQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingPaymentQuery.Id,QueryCode = PendingPaymentQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.Declaration.ProcedureCurrentName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn PendingPaymentQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingPaymentQuery.Id,QueryCode = PendingPaymentQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.Declaration.TotalInvoiceAmountInUSD" , ColumnWidth = 150 }, addedQueryColumns);

             AdvancedQueryFilter PendingPaymentQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsPending900", PredefinedValue = "true",PredefinedValue2 = null, QueryId = PendingPaymentQuery.Id,QueryCode = PendingPaymentQuery.UniqueCode, Tenant = 0,Operator = "Equals"}, addedQueryFilters);


             AdvancedQueryFilter PendingPaymentQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsCourierDeclaration", PredefinedValue = "true",PredefinedValue2 = null, QueryId = PendingPaymentQuery.Id,QueryCode = PendingPaymentQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query PendingCustomsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationTextCode_7.Id, NameTextCodeCode = DeclarationTextCode_7.Code, ObjectTableName = "Customs.Declaration", Code = "PendingCustoms",  QueryGroupCode = "VHQG", IndexOrder = 7, Tenant = 0, ObjectTableId = DeclarationObjectTable.Id, QuerySection = "Customs.Declaration", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationFeature_7.Id,FeatureUniqeCode= DeclarationFeature_7.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, addedQueries);
	
			 QueryColumn PendingCustomsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingCustomsQuery.Id,QueryCode = PendingCustomsQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.Declaration.MAWB" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn PendingCustomsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingCustomsQuery.Id,QueryCode = PendingCustomsQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.Declaration.CustomFileNo" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn PendingCustomsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingCustomsQuery.Id,QueryCode = PendingCustomsQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.Declaration.CourierHAWB" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn PendingCustomsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingCustomsQuery.Id,QueryCode = PendingCustomsQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.Declaration.CustomerName" , ColumnWidth = 160 }, addedQueryColumns);

			 QueryColumn PendingCustomsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingCustomsQuery.Id,QueryCode = PendingCustomsQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.Declaration.DeclarationNumber" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn PendingCustomsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingCustomsQuery.Id,QueryCode = PendingCustomsQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.Declaration.ProcedureCurrentName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn PendingCustomsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingCustomsQuery.Id,QueryCode = PendingCustomsQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.Declaration.TotalInvoiceAmountInUSD" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn PendingCustomsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingCustomsQuery.Id,QueryCode = PendingCustomsQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.Declaration.AcceptanceStatusName" , ColumnWidth = 190 }, addedQueryColumns);

			 QueryColumn PendingCustomsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingCustomsQuery.Id,QueryCode = PendingCustomsQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "Customs.Declaration.CourierSuspentionName" , ColumnWidth = 180 }, addedQueryColumns);

             AdvancedQueryFilter PendingCustomsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsClosedForFollowUp", PredefinedValue = "false",PredefinedValue2 = null, QueryId = PendingCustomsQuery.Id,QueryCode = PendingCustomsQuery.UniqueCode, Tenant = 0,Operator = "Equals"}, addedQueryFilters);


             AdvancedQueryFilter PendingCustomsQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.CourierCustomStatusCode", PredefinedValue = "2",PredefinedValue2 = null, QueryId = PendingCustomsQuery.Id,QueryCode = PendingCustomsQuery.UniqueCode, Tenant = 0,Operator = "Equals"}, addedQueryFilters);


             AdvancedQueryFilter PendingCustomsQueryFilter_2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsCourierDeclaration", PredefinedValue = "true",PredefinedValue2 = null, QueryId = PendingCustomsQuery.Id,QueryCode = PendingCustomsQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query PendingQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationTextCode_8.Id, NameTextCodeCode = DeclarationTextCode_8.Code, ObjectTableName = "Customs.Declaration", Code = "Pending",  QueryGroupCode = "VHQG", IndexOrder = 8, Tenant = 0, ObjectTableId = DeclarationObjectTable.Id, QuerySection = "Customs.Declaration", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationFeature_8.Id,FeatureUniqeCode= DeclarationFeature_8.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, addedQueries);
	
			 QueryColumn PendingQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingQuery.Id,QueryCode = PendingQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.Declaration.MAWB" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn PendingQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingQuery.Id,QueryCode = PendingQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.Declaration.CustomFileNo" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn PendingQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingQuery.Id,QueryCode = PendingQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.Declaration.CourierHAWB" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn PendingQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingQuery.Id,QueryCode = PendingQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.Declaration.CustomerName" , ColumnWidth = 160 }, addedQueryColumns);

			 QueryColumn PendingQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingQuery.Id,QueryCode = PendingQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.Declaration.DeclarationNumber" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn PendingQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingQuery.Id,QueryCode = PendingQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.Declaration.ProcedureCurrentName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn PendingQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingQuery.Id,QueryCode = PendingQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.Declaration.TotalInvoiceAmountInUSD" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn PendingQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingQuery.Id,QueryCode = PendingQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.Declaration.AcceptanceStatusName" , ColumnWidth = 190 }, addedQueryColumns);

			 QueryColumn PendingQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PendingQuery.Id,QueryCode = PendingQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "Customs.Declaration.CourierPendingReasonList" , ColumnWidth = 180 }, addedQueryColumns);

             AdvancedQueryFilter PendingQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsPendingNotNull", PredefinedValue = "true",PredefinedValue2 = null, QueryId = PendingQuery.Id,QueryCode = PendingQuery.UniqueCode, Tenant = 0,Operator = "Equals"}, addedQueryFilters);


             AdvancedQueryFilter PendingQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsCourierDeclaration", PredefinedValue = "true",PredefinedValue2 = null, QueryId = PendingQuery.Id,QueryCode = PendingQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query DeclarationWithoutReleaseQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationTextCode_9.Id, NameTextCodeCode = DeclarationTextCode_9.Code, ObjectTableName = "Customs.Declaration", Code = "DeclarationWithoutRelease",  QueryGroupCode = "DECL", IndexOrder = 9, Tenant = 0, ObjectTableId = DeclarationObjectTable.Id, QuerySection = "Customs.Declaration", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationFeature_9.Id,FeatureUniqeCode= DeclarationFeature_9.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, addedQueries);
	
			 QueryColumn DeclarationWithoutReleaseQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationWithoutReleaseQuery.Id,QueryCode = DeclarationWithoutReleaseQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.Declaration.TaxationDateTime" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn DeclarationWithoutReleaseQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationWithoutReleaseQuery.Id,QueryCode = DeclarationWithoutReleaseQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.Declaration.CustomFileNo" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn DeclarationWithoutReleaseQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationWithoutReleaseQuery.Id,QueryCode = DeclarationWithoutReleaseQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.Declaration.CustomerName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn DeclarationWithoutReleaseQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationWithoutReleaseQuery.Id,QueryCode = DeclarationWithoutReleaseQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.Declaration.DeclarationOfficeName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn DeclarationWithoutReleaseQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationWithoutReleaseQuery.Id,QueryCode = DeclarationWithoutReleaseQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.Declaration.DeclarationNumber" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn DeclarationWithoutReleaseQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationWithoutReleaseQuery.Id,QueryCode = DeclarationWithoutReleaseQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.Declaration.ProcedureCurrentName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn DeclarationWithoutReleaseQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationWithoutReleaseQuery.Id,QueryCode = DeclarationWithoutReleaseQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.Declaration.DeclarationStatusTypeName" , ColumnWidth = 200 }, addedQueryColumns);

             AdvancedQueryFilter DeclarationWithoutReleaseQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsCancelled", PredefinedValue = "false",PredefinedValue2 = null, QueryId = DeclarationWithoutReleaseQuery.Id,QueryCode = DeclarationWithoutReleaseQuery.UniqueCode, Tenant = 0}, addedQueryFilters);


             AdvancedQueryFilter DeclarationWithoutReleaseQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.DeclarationWithoutRelease", PredefinedValue = "1",PredefinedValue2 = null, QueryId = DeclarationWithoutReleaseQuery.Id,QueryCode = DeclarationWithoutReleaseQuery.UniqueCode, Tenant = 0}, addedQueryFilters);


             AdvancedQueryFilter DeclarationWithoutReleaseQueryFilter_2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.Direction", PredefinedValue = "E",PredefinedValue2 = null, QueryId = DeclarationWithoutReleaseQuery.Id,QueryCode = DeclarationWithoutReleaseQuery.UniqueCode, Tenant = 0,Operator = "NotEqual"}, addedQueryFilters);


             AdvancedQueryFilter DeclarationWithoutReleaseQueryFilter_3 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.AmendmentDontDisplayInList", PredefinedValue = "false",PredefinedValue2 = null, QueryId = DeclarationWithoutReleaseQuery.Id,QueryCode = DeclarationWithoutReleaseQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query DeclarationInConstraintQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationTextCode_10.Id, NameTextCodeCode = DeclarationTextCode_10.Code, ObjectTableName = "Customs.Declaration", Code = "DeclarationInConstraint",  QueryGroupCode = "DECL", IndexOrder = 10, Tenant = 0, ObjectTableId = DeclarationObjectTable.Id, QuerySection = "Customs.Declaration", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationFeature_10.Id,FeatureUniqeCode= DeclarationFeature_10.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, addedQueries);
	
			 QueryColumn DeclarationInConstraintQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationInConstraintQuery.Id,QueryCode = DeclarationInConstraintQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.Declaration.TaxationDateTime" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn DeclarationInConstraintQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationInConstraintQuery.Id,QueryCode = DeclarationInConstraintQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.Declaration.CustomFileNo" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn DeclarationInConstraintQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationInConstraintQuery.Id,QueryCode = DeclarationInConstraintQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.Declaration.CustomerName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn DeclarationInConstraintQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationInConstraintQuery.Id,QueryCode = DeclarationInConstraintQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.Declaration.DeclarationOfficeName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn DeclarationInConstraintQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationInConstraintQuery.Id,QueryCode = DeclarationInConstraintQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.Declaration.DeclarationNumber" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn DeclarationInConstraintQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationInConstraintQuery.Id,QueryCode = DeclarationInConstraintQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.Declaration.ProcedureCurrentName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn DeclarationInConstraintQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationInConstraintQuery.Id,QueryCode = DeclarationInConstraintQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.Declaration.DeclarationStatusTypeName" , ColumnWidth = 200 }, addedQueryColumns);

             AdvancedQueryFilter DeclarationInConstraintQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsCancelled", PredefinedValue = "false",PredefinedValue2 = null, QueryId = DeclarationInConstraintQuery.Id,QueryCode = DeclarationInConstraintQuery.UniqueCode, Tenant = 0}, addedQueryFilters);


             AdvancedQueryFilter DeclarationInConstraintQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.DeclarationStatusTypeCode", PredefinedValue = "11",PredefinedValue2 = null, QueryId = DeclarationInConstraintQuery.Id,QueryCode = DeclarationInConstraintQuery.UniqueCode, Tenant = 0}, addedQueryFilters);


             AdvancedQueryFilter DeclarationInConstraintQueryFilter_2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.AmendmentDontDisplayInList", PredefinedValue = "false",PredefinedValue2 = null, QueryId = DeclarationInConstraintQuery.Id,QueryCode = DeclarationInConstraintQuery.UniqueCode, Tenant = 0}, addedQueryFilters);


             AdvancedQueryFilter DeclarationInConstraintQueryFilter_3 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.Direction", PredefinedValue = "E",PredefinedValue2 = null, QueryId = DeclarationInConstraintQuery.Id,QueryCode = DeclarationInConstraintQuery.UniqueCode, Tenant = 0,Operator = "NotEqual"}, addedQueryFilters);

  
	      

			  Query PaidDeclarationQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationTextCode_11.Id, NameTextCodeCode = DeclarationTextCode_11.Code, ObjectTableName = "Customs.Declaration", Code = "PaidDeclaration",  QueryGroupCode = "DECL", IndexOrder = 11, Tenant = 0, ObjectTableId = DeclarationObjectTable.Id, QuerySection = "Customs.Declaration", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationFeature_11.Id,FeatureUniqeCode= DeclarationFeature_11.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, addedQueries);
	
			 QueryColumn PaidDeclarationQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PaidDeclarationQuery.Id,QueryCode = PaidDeclarationQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.Declaration.TaxationDateTime" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn PaidDeclarationQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PaidDeclarationQuery.Id,QueryCode = PaidDeclarationQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.Declaration.CustomFileNo" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn PaidDeclarationQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PaidDeclarationQuery.Id,QueryCode = PaidDeclarationQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.Declaration.CustomerName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn PaidDeclarationQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PaidDeclarationQuery.Id,QueryCode = PaidDeclarationQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.Declaration.DeclarationOfficeName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn PaidDeclarationQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PaidDeclarationQuery.Id,QueryCode = PaidDeclarationQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.Declaration.DeclarationNumber" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn PaidDeclarationQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PaidDeclarationQuery.Id,QueryCode = PaidDeclarationQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.Declaration.ProcedureCurrentName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn PaidDeclarationQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PaidDeclarationQuery.Id,QueryCode = PaidDeclarationQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.Declaration.DeclarationStatusTypeName" , ColumnWidth = 200 }, addedQueryColumns);

             AdvancedQueryFilter PaidDeclarationQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsCancelled", PredefinedValue = "false",PredefinedValue2 = null, QueryId = PaidDeclarationQuery.Id,QueryCode = PaidDeclarationQuery.UniqueCode, Tenant = 0}, addedQueryFilters);


             AdvancedQueryFilter PaidDeclarationQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.PaidDeclarationWithoutRelease", PredefinedValue = "",PredefinedValue2 = null, QueryId = PaidDeclarationQuery.Id,QueryCode = PaidDeclarationQuery.UniqueCode, Tenant = 0}, addedQueryFilters);


             AdvancedQueryFilter PaidDeclarationQueryFilter_2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.AmendmentDontDisplayInList", PredefinedValue = "false",PredefinedValue2 = null, QueryId = PaidDeclarationQuery.Id,QueryCode = PaidDeclarationQuery.UniqueCode, Tenant = 0}, addedQueryFilters);


             AdvancedQueryFilter PaidDeclarationQueryFilter_3 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.Direction", PredefinedValue = "E",PredefinedValue2 = null, QueryId = PaidDeclarationQuery.Id,QueryCode = PaidDeclarationQuery.UniqueCode, Tenant = 0,Operator = "NotEqual"}, addedQueryFilters);

  
	      

			  Query DeclarationAmendmentsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationTextCode_12.Id, NameTextCodeCode = DeclarationTextCode_12.Code, ObjectTableName = "Customs.Declaration", Code = "DeclarationAmendments",  QueryGroupCode = "DECL", IndexOrder = 12, Tenant = 0, ObjectTableId = DeclarationObjectTable.Id, QuerySection = "Customs.Declaration", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = DeclarationFeature_12.Id,FeatureUniqeCode= DeclarationFeature_12.FeatureUniqeCode, DefaultSortName = "AmendmentissueDate", DefaultSortDirection = "Desending", Perspective = null }, addedQueries);
	
			 QueryColumn DeclarationAmendmentsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationAmendmentsQuery.Id,QueryCode = DeclarationAmendmentsQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.Declaration.AmendmentissueDate" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn DeclarationAmendmentsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationAmendmentsQuery.Id,QueryCode = DeclarationAmendmentsQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.Declaration.CustomFileAmendment" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn DeclarationAmendmentsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationAmendmentsQuery.Id,QueryCode = DeclarationAmendmentsQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.Declaration.DeclarationNoAmendment" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn DeclarationAmendmentsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationAmendmentsQuery.Id,QueryCode = DeclarationAmendmentsQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.Declaration.CustomerName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn DeclarationAmendmentsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationAmendmentsQuery.Id,QueryCode = DeclarationAmendmentsQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.Declaration.ProcedureCurrentName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn DeclarationAmendmentsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationAmendmentsQuery.Id,QueryCode = DeclarationAmendmentsQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.Declaration.DeclarationOfficeName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn DeclarationAmendmentsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationAmendmentsQuery.Id,QueryCode = DeclarationAmendmentsQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.Declaration.AmendmentRequestNumber" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn DeclarationAmendmentsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationAmendmentsQuery.Id,QueryCode = DeclarationAmendmentsQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.Declaration.AmendmentCorrectedByUserName" , ColumnWidth = 100 }, addedQueryColumns);

             AdvancedQueryFilter DeclarationAmendmentsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsAmendment", PredefinedValue = "True",PredefinedValue2 = null, QueryId = DeclarationAmendmentsQuery.Id,QueryCode = DeclarationAmendmentsQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);


             AdvancedQueryFilter DeclarationAmendmentsQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.AmendmentStatus", PredefinedValue = "1",PredefinedValue2 = null, QueryId = DeclarationAmendmentsQuery.Id,QueryCode = DeclarationAmendmentsQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);


             AdvancedQueryFilter DeclarationAmendmentsQueryFilter_2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.Direction", PredefinedValue = "E",PredefinedValue2 = null, QueryId = DeclarationAmendmentsQuery.Id,QueryCode = DeclarationAmendmentsQuery.UniqueCode, Tenant = 0,Operator = "NotEqual"}, addedQueryFilters);

  
	      

			  Query DeclarationsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationTextCode_13.Id, NameTextCodeCode = DeclarationTextCode_13.Code, ObjectTableName = "Customs.Declaration", Code = "Declarations",  QueryGroupCode = "DECL", IndexOrder = 13, Tenant = 0, ObjectTableId = DeclarationObjectTable.Id, QuerySection = "Customs.Declaration", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationFeature_13.Id,FeatureUniqeCode= DeclarationFeature_13.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, addedQueries);
	
			 QueryColumn DeclarationsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationsQuery.Id,QueryCode = DeclarationsQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.Declaration.TaxationDateTime" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn DeclarationsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationsQuery.Id,QueryCode = DeclarationsQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.Declaration.CustomFileNo" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn DeclarationsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationsQuery.Id,QueryCode = DeclarationsQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.Declaration.CustomerName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn DeclarationsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationsQuery.Id,QueryCode = DeclarationsQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.Declaration.DeclarationOfficeName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn DeclarationsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationsQuery.Id,QueryCode = DeclarationsQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.Declaration.DeclarationNumber" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn DeclarationsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationsQuery.Id,QueryCode = DeclarationsQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.Declaration.ProcedureCurrentName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn DeclarationsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DeclarationsQuery.Id,QueryCode = DeclarationsQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.Declaration.DeclarationStatusTypeName" , ColumnWidth = 200 }, addedQueryColumns);

             AdvancedQueryFilter DeclarationsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsCancelled", PredefinedValue = "false",PredefinedValue2 = null, QueryId = DeclarationsQuery.Id,QueryCode = DeclarationsQuery.UniqueCode, Tenant = 0}, addedQueryFilters);


             AdvancedQueryFilter DeclarationsQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.AmendmentDontDisplayInList", PredefinedValue = "false",PredefinedValue2 = null, QueryId = DeclarationsQuery.Id,QueryCode = DeclarationsQuery.UniqueCode, Tenant = 0}, addedQueryFilters);


             AdvancedQueryFilter DeclarationsQueryFilter_2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.Direction", PredefinedValue = "E",PredefinedValue2 = null, QueryId = DeclarationsQuery.Id,QueryCode = DeclarationsQuery.UniqueCode, Tenant = 0,Operator = "NotEqual"}, addedQueryFilters);

  
	      

			  Query ExportDeclarationQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationTextCode_14.Id, NameTextCodeCode = DeclarationTextCode_14.Code, ObjectTableName = "Customs.Declaration", Code = "ExportDeclaration",  QueryGroupCode = "DECL", IndexOrder = 14, Tenant = 0, ObjectTableId = DeclarationObjectTable.Id, QuerySection = "Customs.Declaration", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationFeature_14.Id,FeatureUniqeCode= DeclarationFeature_14.FeatureUniqeCode, DefaultSortName = "TaxationDateTime", DefaultSortDirection = "Desending", Perspective = null }, addedQueries);
	
			 QueryColumn ExportDeclarationQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExportDeclarationQuery.Id,QueryCode = ExportDeclarationQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.Declaration.CreateDateForExport" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ExportDeclarationQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExportDeclarationQuery.Id,QueryCode = ExportDeclarationQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.Declaration.ExportFile" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ExportDeclarationQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExportDeclarationQuery.Id,QueryCode = ExportDeclarationQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.Declaration.TransportModeForExport" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ExportDeclarationQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExportDeclarationQuery.Id,QueryCode = ExportDeclarationQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.Declaration.CustomFileForExport" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ExportDeclarationQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExportDeclarationQuery.Id,QueryCode = ExportDeclarationQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.Declaration.CustomerName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn ExportDeclarationQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExportDeclarationQuery.Id,QueryCode = ExportDeclarationQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.Declaration.DeclarationOfficeName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ExportDeclarationQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExportDeclarationQuery.Id,QueryCode = ExportDeclarationQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.Declaration.ProcedureCurrentName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ExportDeclarationQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExportDeclarationQuery.Id,QueryCode = ExportDeclarationQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.Declaration.DeclarationStatusTypeName" , ColumnWidth = 200 }, addedQueryColumns);

             AdvancedQueryFilter ExportDeclarationQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.Direction", PredefinedValue = "E",PredefinedValue2 = null, QueryId = ExportDeclarationQuery.Id,QueryCode = ExportDeclarationQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query AllCourierDeclarationsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationTextCode_15.Id, NameTextCodeCode = DeclarationTextCode_15.Code, ObjectTableName = "Customs.Declaration", Code = "AllCourierDeclarations",  QueryGroupCode = "DECL", IndexOrder = 15, Tenant = 0, ObjectTableId = DeclarationObjectTable.Id, QuerySection = "Customs.Declaration", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationFeature_15.Id,FeatureUniqeCode= DeclarationFeature_15.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, addedQueries);
	
			 QueryColumn AllCourierDeclarationsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCourierDeclarationsQuery.Id,QueryCode = AllCourierDeclarationsQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.Declaration.MAWB" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn AllCourierDeclarationsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCourierDeclarationsQuery.Id,QueryCode = AllCourierDeclarationsQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.Declaration.CustomFileNo" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn AllCourierDeclarationsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCourierDeclarationsQuery.Id,QueryCode = AllCourierDeclarationsQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.Declaration.CourierHAWB" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn AllCourierDeclarationsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCourierDeclarationsQuery.Id,QueryCode = AllCourierDeclarationsQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.Declaration.FastIndividualProcessCode" , ColumnWidth = 140 }, addedQueryColumns);

			 QueryColumn AllCourierDeclarationsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCourierDeclarationsQuery.Id,QueryCode = AllCourierDeclarationsQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.Declaration.CustomerName" , ColumnWidth = 160 }, addedQueryColumns);

			 QueryColumn AllCourierDeclarationsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCourierDeclarationsQuery.Id,QueryCode = AllCourierDeclarationsQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.Declaration.DeclarationNumber" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn AllCourierDeclarationsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCourierDeclarationsQuery.Id,QueryCode = AllCourierDeclarationsQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.Declaration.ProcedureCurrentName" , ColumnWidth = 190 }, addedQueryColumns);

			 QueryColumn AllCourierDeclarationsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCourierDeclarationsQuery.Id,QueryCode = AllCourierDeclarationsQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.Declaration.DeclarationStatusTypeName" , ColumnWidth = 180 }, addedQueryColumns);

             AdvancedQueryFilter AllCourierDeclarationsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Declaration.IsCourierDeclaration", PredefinedValue = "true",PredefinedValue2 = null, QueryId = AllCourierDeclarationsQuery.Id,QueryCode = AllCourierDeclarationsQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

			SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
			SqlBulkInsert.BulkInsert("Features", addedFeatures);
			SqlBulkInsert.BulkInsert("Queries", addedQueries);
			SqlBulkInsert.BulkInsert("QueryColumns", addedQueryColumns);
			SqlBulkInsert.BulkInsert("AdvancedQueryFilters", addedQueryFilters);	 
  
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable DeclarationObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.Declaration" && d.Tenant == 0).FirstOrDefault();
		   //List<ObjectField> DeclarationObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.Declaration").ToList();
		       
	      

	         Screen DeclarationHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customs.Declaration.HeaderScreen", Name = "Header Screen", ObjectTableId = DeclarationObjectTable.Id, NumberOfColumns = 4, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
             ScreenField CustomsDeclarationCustomsDeclarationHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ScreenId = DeclarationHeaderScreenScreen0.Id,ScreenCode = DeclarationHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.Declaration.DeclarationNumberandVersionId", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsDeclarationCustomsDeclarationHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ScreenId = DeclarationHeaderScreenScreen0.Id,ScreenCode = DeclarationHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.Declaration.ProcedureCurrentName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsDeclarationCustomsDeclarationHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ScreenId = DeclarationHeaderScreenScreen0.Id,ScreenCode = DeclarationHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.Declaration.ExternalDeclarationNumber", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsDeclarationCustomsDeclarationHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ScreenId = DeclarationHeaderScreenScreen0.Id,ScreenCode = DeclarationHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.Declaration.DeclarationOfficeName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsDeclarationCustomsDeclarationHeaderScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ScreenId = DeclarationHeaderScreenScreen0.Id,ScreenCode = DeclarationHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.Declaration.TaxationDateTime", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsDeclarationCustomsDeclarationHeaderScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 1, ScreenId = DeclarationHeaderScreenScreen0.Id,ScreenCode = DeclarationHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.Declaration.HatraDate", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsDeclarationCustomsDeclarationHeaderScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ScreenId = DeclarationHeaderScreenScreen0.Id,ScreenCode = DeclarationHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.Declaration.DeclarationStatusTypeName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsDeclarationCustomsDeclarationHeaderScreenScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 1, ScreenId = DeclarationHeaderScreenScreen0.Id,ScreenCode = DeclarationHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.Declaration.DepartmentName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          	
		    DeclarationObjectTable.HeaderScreenId = DeclarationHeaderScreenScreen0.Id;
		    DeclarationObjectTable.HeaderScreenCode = DeclarationHeaderScreenScreen0.Code;

	   		  
	      

	         Screen DeclarationGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customs.Declaration.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = DeclarationObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 5, IsReadOnly = false }, screensRepository, tenantScreens);
      
             ScreenField CustomsDeclarationCustomsDeclarationGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ScreenId = DeclarationGeneralTabScreenScreen1.Id,ScreenCode = DeclarationGeneralTabScreenScreen1.Code, ObjectFieldCode = "Customs.Declaration.CustomFileNo", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsDeclarationCustomsDeclarationGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ScreenId = DeclarationGeneralTabScreenScreen1.Id,ScreenCode = DeclarationGeneralTabScreenScreen1.Code, ObjectFieldCode = "Customs.Declaration.CustomerId", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsDeclarationCustomsDeclarationGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ScreenId = DeclarationGeneralTabScreenScreen1.Id,ScreenCode = DeclarationGeneralTabScreenScreen1.Code, ObjectFieldCode = "Customs.Declaration.DeclarationOfficeCode", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsDeclarationCustomsDeclarationGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ScreenId = DeclarationGeneralTabScreenScreen1.Id,ScreenCode = DeclarationGeneralTabScreenScreen1.Code, ObjectFieldCode = "Customs.Declaration.ImporterId", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsDeclarationCustomsDeclarationGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 4, ScreenId = DeclarationGeneralTabScreenScreen1.Id,ScreenCode = DeclarationGeneralTabScreenScreen1.Code, ObjectFieldCode = "Customs.Declaration.ProcedureCurrentCode", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	            

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable DeclarationObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.Declaration" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode DeclarationCorrectionsTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.Corrections", DefaultText = "Corrections",LocalDefaultText = "תיקון הצהרה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationCorrectionsFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CORRECTIONS", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.Correction", NameTextCodeDefaultText = "Correction", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
 
                 
			   TextCode DeclarationGeneralTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.General", DefaultText = "General",LocalDefaultText = "כללי", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationGeneralFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
 
                 
			   TextCode DeclarationSupplierInvoicesTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.Invoices", DefaultText = "Supplier Invoices",LocalDefaultText = "חשבונות ספק", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationSupplierInvoicesFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "INVOICES", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.Invoices", NameTextCodeDefaultText = "Invoices", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
 
                 
			   TextCode DeclarationCertificatesTextCode_TH3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.Certificates", DefaultText = "Certificates",LocalDefaultText = "הזנת אישורים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationCertificatesFeature_TH3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CERTIFICATE", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.Certificates", NameTextCodeDefaultText = "Certificates", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
 
                 
			   TextCode DeclarationCustomsDocumentsTextCode_TH4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.CustomDocuments", DefaultText = "Customs Documents",LocalDefaultText = "צרופות מכס", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationCustomsDocumentsFeature_TH4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMDOCUMENTS", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.CustomDocument", NameTextCodeDefaultText = "Custom Documents", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
 
                 
			   TextCode DeclarationCustomsReplyTextCode_TH5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.CustomsAnswers", DefaultText = "Customs Reply",LocalDefaultText = "תשובה לתיק", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationCustomsReplyFeature_TH5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMSANSWERS", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.CustomsAnswers", NameTextCodeDefaultText = "Customs Answers", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
 
                 
			   TextCode DeclarationTaxesTextCode_TH6 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.Taxes", DefaultText = "Taxes",LocalDefaultText = "מסים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationTaxesFeature_TH6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TAXES", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.Taxes", NameTextCodeDefaultText = "Taxes", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
 
                 
			   TextCode DeclarationPaymentOrdersTextCode_TH7 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.PaymentOrder", DefaultText = "Payment Orders",LocalDefaultText = "הוראות תשלום", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationPaymentOrdersFeature_TH7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DECLARATIONPYORDER", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.PaymentOrder", NameTextCodeDefaultText = "Payment Order", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
 
                 
			   TextCode DeclarationPhysicalChecksTextCode_TH8 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.PhysicalCheck", DefaultText = "Physical Checks",LocalDefaultText = "בדיקה פיזית", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationPhysicalChecksFeature_TH8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DECLARATIONPHCHECK", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.PhysicalCheck", NameTextCodeDefaultText = "Physical Check", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
 
                 
			   TextCode DeclarationEventsTextCode_TH9 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.Events", DefaultText = "Events",LocalDefaultText = "אירועים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationEventsFeature_TH9 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
 
                 
			   TextCode DeclarationRequestSheetsTextCode_TH10 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.RequestSheet", DefaultText = "Request Sheets",LocalDefaultText = "גליון בקשות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationRequestSheetsFeature_TH10 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DECLARATIONSHEET", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.RequestSheet", NameTextCodeDefaultText = "Request Sheet", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
 
                 
			   TextCode DeclarationCommunicationsTextCode_TH11 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.Communications", DefaultText = "Communications",LocalDefaultText = "תקשורות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationCommunicationsFeature_TH11 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "COMMUNICATIONS", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.Communication", NameTextCodeDefaultText = "Communication", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
 
                 
			   TextCode DeclarationMoreFieldsTextCode_TH12 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.MoreFields", DefaultText = "More Fields",LocalDefaultText = "שדות נוספים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationMoreFieldsFeature_TH12 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MOREFIELDS", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.MoreFields", NameTextCodeDefaultText = "More Fields", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
 
                 
			   TextCode DeclarationTapagsTextCode_TH13 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.Tapags", DefaultText = "Tapags",LocalDefaultText = "תיקי תפ”ג", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationTapagsFeature_TH13 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TAPAGS", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.Tapags", NameTextCodeDefaultText = "Tapags", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
 
                 
			   TextCode DeclarationNotificationReplyTextCode_TH14 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.Notification", DefaultText = "Notification Reply",LocalDefaultText = "הודעות לסוכן", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationNotificationReplyFeature_TH14 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NOTIFICATION", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.NotificationReply", NameTextCodeDefaultText = "Notification Reply", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
 
                 
			   TextCode DeclarationNotificationsTextCode_TH15 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.Notifications", DefaultText = "Notifications",LocalDefaultText = "התראות לתיק", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationNotificationsFeature_TH15 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NOTIFICATIONS", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.Notifications", NameTextCodeDefaultText = "Notifications", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
 
                 
			   TextCode DeclarationCustomsCollateralTextCode_TH16 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.TH.CustomsCollateral", DefaultText = "Customs Collateral",LocalDefaultText = "בטוחות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationCustomsCollateralFeature_TH16 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "COLLATERAL", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.Collateral", NameTextCodeDefaultText = "Collateral", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
 
                 
			   TextCode DeclarationCargoSplitTextCode_TH17 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.CargoSplit", DefaultText = "Cargo Split",LocalDefaultText = "בקשות פיצול מטען", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationCargoSplitFeature_TH17 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DECLARATIONCASPLIT", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.CargoSplit", NameTextCodeDefaultText = "Cargo Split", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
 
                 
			   TextCode DeclarationClassificationTextCode_TH18 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.Classification", DefaultText = "Classification",LocalDefaultText = "סיווג", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationClassificationFeature_TH18 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DECLARATIONCLASSIFICATION", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.Classification", NameTextCodeDefaultText = "Declaration Classification", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
 
                 
			   TextCode DeclarationCargoSealTextCode_TH19 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.CargoSeal", DefaultText = "Cargo Seal",LocalDefaultText = "רשימת סגרים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationCargoSealFeature_TH19 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DECLARATIONCARGOSEAL", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.CargoSeal", NameTextCodeDefaultText = "Declaration CargoSeal", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
 
                 
			   TextCode DeclarationDeclarationAmendmentsTextCode_TH20 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.DeclarationAmendment", DefaultText = "Declaration Amendments",LocalDefaultText = "תיקוני הצהרה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationDeclarationAmendmentsFeature_TH20 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DECLARATIONAMENDMENT", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.DeclarationAmendment", NameTextCodeDefaultText = "Declaration Amendment", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
 
                 
			   TextCode DeclarationDocsInTextCode_TH21 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.DocsIn", DefaultText = "Docs In",LocalDefaultText = "טעינת מסמכים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature DeclarationDocsInFeature_TH21 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DOCSIN", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.DocsIn", NameTextCodeDefaultText = "DocsIn", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DCCR",HtmlComponentName = "DeclarationCorrectionsComponent",HtmlComponentUrl = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Corrections/DeclarationCorrectionsComponent", FeatureId = DeclarationCorrectionsFeature_TH0.Id,FeatureUniqeCode = DeclarationCorrectionsFeature_TH0.FeatureUniqeCode, ControlPath = " ", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationCorrectionsTextCode_TH0.Id, TabNameTextCodeCode = DeclarationCorrectionsTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DEGC",HtmlComponentName = "DeclarationGeneralComponent",HtmlComponentUrl = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/General/DeclarationGeneralComponent", FeatureId = DeclarationGeneralFeature_TH1.Id,FeatureUniqeCode = DeclarationGeneralFeature_TH1.FeatureUniqeCode, ControlPath = "Logitude.Customs.Views.Tabs.DeclarationGeneralTabControl", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationGeneralTextCode_TH1.Id, TabNameTextCodeCode = DeclarationGeneralTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DEIN",HtmlComponentName = "DeclarationSupplierInvoiceTabComponent",HtmlComponentUrl = "./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/DeclarationSupplierInvoiceTabComponent", FeatureId = DeclarationSupplierInvoicesFeature_TH2.Id,FeatureUniqeCode = DeclarationSupplierInvoicesFeature_TH2.FeatureUniqeCode, ControlPath = "Logitude.Customs.Views.Tabs.DeclarationInvoicesTabControl", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationSupplierInvoicesTextCode_TH2.Id, TabNameTextCodeCode = DeclarationSupplierInvoicesTextCode_TH2.Code, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DECR",HtmlComponentName = "CertificateTabComponent",HtmlComponentUrl = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Certificate/CertificateTabComponent", FeatureId = DeclarationCertificatesFeature_TH3.Id,FeatureUniqeCode = DeclarationCertificatesFeature_TH3.FeatureUniqeCode, ControlPath = "Logitude.Customs.Views.Tabs.DeclarationCertificatesTabControl", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationCertificatesTextCode_TH3.Id, TabNameTextCodeCode = DeclarationCertificatesTextCode_TH3.Code, Tenant = 0, IndexOrder = 3 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DCCD",HtmlComponentName = "CustomsDocumentsComponent",HtmlComponentUrl = "./CustomsModules/CustomsDocuments/Components/CustomsDocumentsComponent", FeatureId = DeclarationCustomsDocumentsFeature_TH4.Id,FeatureUniqeCode = DeclarationCustomsDocumentsFeature_TH4.FeatureUniqeCode, ControlPath = "Logitude.Customs.Views.Documents.DeclarationCustomDocumentsControl", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationCustomsDocumentsTextCode_TH4.Id, TabNameTextCodeCode = DeclarationCustomsDocumentsTextCode_TH4.Code, Tenant = 0, IndexOrder = 4 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DCCA",HtmlComponentName = "CustomsAnswersComponent",HtmlComponentUrl = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/CustomsAnswers/CustomsAnswersComponent", FeatureId = DeclarationCustomsReplyFeature_TH5.Id,FeatureUniqeCode = DeclarationCustomsReplyFeature_TH5.FeatureUniqeCode, ControlPath = "Logitude.Customs.Views.Tabs.DeclarationCustomsAnswersControl", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationCustomsReplyTextCode_TH5.Id, TabNameTextCodeCode = DeclarationCustomsReplyTextCode_TH5.Code, Tenant = 0, IndexOrder = 5 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DETX",HtmlComponentName = "DeclarationTaxesTabComponent",HtmlComponentUrl = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Taxes/DeclarationTaxesTabComponent", FeatureId = DeclarationTaxesFeature_TH6.Id,FeatureUniqeCode = DeclarationTaxesFeature_TH6.FeatureUniqeCode, ControlPath = "Logitude.Customs.Views.Tabs.DeclarationTaxesTabControl", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationTaxesTextCode_TH6.Id, TabNameTextCodeCode = DeclarationTaxesTextCode_TH6.Code, Tenant = 0, IndexOrder = 5 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DCPO",HtmlComponentName = "DeclarationPaymentOrderTabComponent",HtmlComponentUrl = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/PaymentOrder/DeclarationPaymentOrderTabComponent", FeatureId = DeclarationPaymentOrdersFeature_TH7.Id,FeatureUniqeCode = DeclarationPaymentOrdersFeature_TH7.FeatureUniqeCode, ControlPath = " ", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationPaymentOrdersTextCode_TH7.Id, TabNameTextCodeCode = DeclarationPaymentOrdersTextCode_TH7.Code, Tenant = 0, IndexOrder = 7 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DCPC",HtmlComponentName = "DeclarationPhysicalCheckTabComponent",HtmlComponentUrl = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/PhysicalCheck/DeclarationPhysicalCheckTabComponent", FeatureId = DeclarationPhysicalChecksFeature_TH8.Id,FeatureUniqeCode = DeclarationPhysicalChecksFeature_TH8.FeatureUniqeCode, ControlPath = " ", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationPhysicalChecksTextCode_TH8.Id, TabNameTextCodeCode = DeclarationPhysicalChecksTextCode_TH8.Code, Tenant = 0, IndexOrder = 8 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DEEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = DeclarationEventsFeature_TH9.Id,FeatureUniqeCode = DeclarationEventsFeature_TH9.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationEventsTextCode_TH9.Id, TabNameTextCodeCode = DeclarationEventsTextCode_TH9.Code, Tenant = 0, IndexOrder = 9 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DCRS",HtmlComponentName = "RequestSheetTabComponent",HtmlComponentUrl = "./CustomsModules/CustomsControls/Components/CustomsRequestsSheetsComponent", FeatureId = DeclarationRequestSheetsFeature_TH10.Id,FeatureUniqeCode = DeclarationRequestSheetsFeature_TH10.FeatureUniqeCode, ControlPath = " ", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationRequestSheetsTextCode_TH10.Id, TabNameTextCodeCode = DeclarationRequestSheetsTextCode_TH10.Code, Tenant = 0, IndexOrder = 10 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DECM",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = DeclarationCommunicationsFeature_TH11.Id,FeatureUniqeCode = DeclarationCommunicationsFeature_TH11.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Communications.CommunicationsControl", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationCommunicationsTextCode_TH11.Id, TabNameTextCodeCode = DeclarationCommunicationsTextCode_TH11.Code, Tenant = 0, IndexOrder = 11 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DCMF",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = DeclarationMoreFieldsFeature_TH12.Id,FeatureUniqeCode = DeclarationMoreFieldsFeature_TH12.FeatureUniqeCode, ControlPath = " ", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationMoreFieldsTextCode_TH12.Id, TabNameTextCodeCode = DeclarationMoreFieldsTextCode_TH12.Code, Tenant = 0, IndexOrder = 12 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DCTP",HtmlComponentName = "DeclarationTapagTabComponent",HtmlComponentUrl = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Tapag/DeclarationTapagTabComponent", FeatureId = DeclarationTapagsFeature_TH13.Id,FeatureUniqeCode = DeclarationTapagsFeature_TH13.FeatureUniqeCode, ControlPath = " ", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationTapagsTextCode_TH13.Id, TabNameTextCodeCode = DeclarationTapagsTextCode_TH13.Code, Tenant = 0, IndexOrder = 13 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DCNT",HtmlComponentName = "NotificationReplyTabComponent",HtmlComponentUrl = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/NotificationReplyTabComponent", FeatureId = DeclarationNotificationReplyFeature_TH14.Id,FeatureUniqeCode = DeclarationNotificationReplyFeature_TH14.FeatureUniqeCode, ControlPath = " ", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationNotificationReplyTextCode_TH14.Id, TabNameTextCodeCode = DeclarationNotificationReplyTextCode_TH14.Code, Tenant = 0, IndexOrder = 14 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DCNF",HtmlComponentName = "NotificationComponent",HtmlComponentUrl = "./CustomsModules/CustomsControls/Components/NotificationComponent", FeatureId = DeclarationNotificationsFeature_TH15.Id,FeatureUniqeCode = DeclarationNotificationsFeature_TH15.FeatureUniqeCode, ControlPath = " ", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationNotificationsTextCode_TH15.Id, TabNameTextCodeCode = DeclarationNotificationsTextCode_TH15.Code, Tenant = 0, IndexOrder = 15 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DCCL",HtmlComponentName = "DeclarationCollateralsComponent",HtmlComponentUrl = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Collateral/DeclarationCollateralsComponent", FeatureId = DeclarationCustomsCollateralFeature_TH16.Id,FeatureUniqeCode = DeclarationCustomsCollateralFeature_TH16.FeatureUniqeCode, ControlPath = " ", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationCustomsCollateralTextCode_TH16.Id, TabNameTextCodeCode = DeclarationCustomsCollateralTextCode_TH16.Code, Tenant = 0, IndexOrder = 16 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DCCS",HtmlComponentName = "DeclarationCargoSplitTabComponent",HtmlComponentUrl = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/CargoSplit/DeclarationCargoSplitTabComponent", FeatureId = DeclarationCargoSplitFeature_TH17.Id,FeatureUniqeCode = DeclarationCargoSplitFeature_TH17.FeatureUniqeCode, ControlPath = " ", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationCargoSplitTextCode_TH17.Id, TabNameTextCodeCode = DeclarationCargoSplitTextCode_TH17.Code, Tenant = 0, IndexOrder = 18 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DCCF",HtmlComponentName = "DeclarationClassificationComponent",HtmlComponentUrl = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Classification/DeclarationClassificationComponent", FeatureId = DeclarationClassificationFeature_TH18.Id,FeatureUniqeCode = DeclarationClassificationFeature_TH18.FeatureUniqeCode, ControlPath = " ", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationClassificationTextCode_TH18.Id, TabNameTextCodeCode = DeclarationClassificationTextCode_TH18.Code, Tenant = 0, IndexOrder = 19 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DCSE",HtmlComponentName = "DeclarationCargoSealTabComponent",HtmlComponentUrl = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/CargoSeal/DeclarationCargoSealTabComponent", FeatureId = DeclarationCargoSealFeature_TH19.Id,FeatureUniqeCode = DeclarationCargoSealFeature_TH19.FeatureUniqeCode, ControlPath = " ", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationCargoSealTextCode_TH19.Id, TabNameTextCodeCode = DeclarationCargoSealTextCode_TH19.Code, Tenant = 0, IndexOrder = 20 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DCDA",HtmlComponentName = "DeclarationAmendmentComponent",HtmlComponentUrl = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/DeclarationAmendment/DeclarationAmendmentComponent", FeatureId = DeclarationDeclarationAmendmentsFeature_TH20.Id,FeatureUniqeCode = DeclarationDeclarationAmendmentsFeature_TH20.FeatureUniqeCode, ControlPath = "Logitude.Customs.Views.Tabs.DeclarationAmendmentControl", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationDeclarationAmendmentsTextCode_TH20.Id, TabNameTextCodeCode = DeclarationDeclarationAmendmentsTextCode_TH20.Code, Tenant = 0, IndexOrder = 21 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DCDI",HtmlComponentName = "DeclarationDocsInTabComponent",HtmlComponentUrl = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/DocsIn/DeclarationDocsInTabComponent", FeatureId = DeclarationDocsInFeature_TH21.Id,FeatureUniqeCode = DeclarationDocsInFeature_TH21.FeatureUniqeCode, ControlPath = "Logitude.Customs.Views.Documents.DeclarationDocsInControl", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationDocsInTextCode_TH21.Id, TabNameTextCodeCode = DeclarationDocsInTextCode_TH21.Code, Tenant = 0, IndexOrder = 22 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DCCO",HtmlComponentName = "DeclarationCorrectionsComponent",HtmlComponentUrl = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Corrections/DeclarationCorrectionsComponent", FeatureId = DeclarationCorrectionsFeature_TH0.Id,FeatureUniqeCode = DeclarationCorrectionsFeature_TH0.FeatureUniqeCode, ControlPath = " ", ObjectTableId = DeclarationObjectTable.Id, TabNameTextCodeId = DeclarationCorrectionsTextCode_TH0.Id, TabNameTextCodeCode = DeclarationCorrectionsTextCode_TH0.Code, Tenant = 0, IndexOrder = 23 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable DeclarationObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.Declaration" && d.Tenant == 0).FirstOrDefault(); 

		   Feature DeclarationFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Declaration.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationObjectTable);
		   Feature DeclarationFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Declaration.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationObjectTable);
		   Feature DeclarationFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Declaration.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationObjectTable);
		   Feature DeclarationFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = false, ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Declaration.Features.PackageFeature", NameTextCodeDefaultText = "Declaration Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationObjectTable); 

		   		   //--------------> Additional Features <--------------\\

		   Feature DeclarationFeature_PAYMENTS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PAYMENTS", FeatureTypeCode = "AREA", Packagable = false, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.Payments", NameTextCodeDefaultText = @"Payments" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationObjectTable);

		   Feature DeclarationFeature_ITEMVEHICLES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ITEMVEHICLES", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.ItemVehicles", NameTextCodeDefaultText = @"Item Vehicle" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationObjectTable);

		   Feature DeclarationFeature_SPLIT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SPLIT", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.Split", NameTextCodeDefaultText = @"Document Split" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationObjectTable);

		   Feature DeclarationFeature_LOADVEHICLESFROMUNI = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "LOADVEHICLESFROMUNI", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.VehicleUnifreight", NameTextCodeDefaultText = @"Load Vehicle From Unifreight" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationObjectTable);

		   Feature DeclarationFeature_ACCUMULATION = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCUMULATION", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.Accumulation", NameTextCodeDefaultText = @"Accumulation" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationObjectTable);

		   Feature DeclarationFeature_IKEA = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "IKEA", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.IKEA", NameTextCodeDefaultText = @"IKEA" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationObjectTable);

		   Feature DeclarationFeature_IFRITZ = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "IFRITZ", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.IFRITZ", NameTextCodeDefaultText = @"IFritz Interface" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationObjectTable);

		   Feature DeclarationFeature_SpecialReplyToCustoms = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SpecialReplyToCustoms", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.SpecialReplyToCustoms", NameTextCodeDefaultText = @"Special Reply To Customs" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationObjectTable);

		   Feature DeclarationFeature_ItemPackageTab = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ItemPackageTab", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.ItemPackageTab", NameTextCodeDefaultText = @"Item Package Tab" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationObjectTable);

		   Feature DeclarationFeature_BTPA = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BTPA", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.BTPA", NameTextCodeDefaultText = @"Auto Filling Payment Filing Screen for BTL Disability Statement" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationObjectTable);

		   Feature DeclarationFeature_EXPORTDECLARATIONNEW2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EXPORTDECLARATIONNEW2", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.EXPORTDECLARATIONNEW2", NameTextCodeDefaultText = @"New Export Declaration" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationObjectTable);

		   Feature DeclarationFeature_EXPORTDECLARATIONPSCREEN = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EXPORTDECLARATIONPSCREEN", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.EXPORTDECLARATIONPSCREEN", NameTextCodeDefaultText = @"Export Declaration Pilot Screens" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationObjectTable);

		   Feature DeclarationFeature_EXPORTDECLARATIONPMENU = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EXPORTDECLARATIONPMENU", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.EXPORTDECLARATIONPMENU", NameTextCodeDefaultText = @"Export Declaration Screens" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationObjectTable);

		   Feature DeclarationFeature_AddNewClientFromManifest = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AddNewClientFromManifest", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.AddNewClientFromManifest", NameTextCodeDefaultText = @"Add New Client " }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationObjectTable);

		   Feature DeclarationFeature_ICL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ICL", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.ICL", NameTextCodeDefaultText = @"ICL Interface" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationObjectTable);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable DeclarationObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.Declaration" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPDT",
                EnglishName =  "Updated",
                LocalName =  "עודכן",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRET",
                EnglishName =  "Created",
                LocalName =  "חדש",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "RSG",
                EnglishName =  "Declaration Release",
                LocalName =  "הצהרה הותרה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "RSC",
                EnglishName =  "Declaration Relase Cancelation",
                LocalName =  "להצהרה בוטלה ההתרה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "DEC",
                EnglishName =  "Declaration Created",
                LocalName =  "נוצרה הצהרת יבוא",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "INR",
                EnglishName =  "Declaration Sent To Customs",
                LocalName =  "טיוטת הצהרה נשלחה למכס",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "PHF",
                EnglishName =  "Declaration Payment Sent",
                LocalName =  "הצהרה הוגשה לתשלום",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "DOK",
                EnglishName =  "Draft OK",
                LocalName =  "טיוטה תקינה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "BRA",
                EnglishName =  "Storage Approval",
                LocalName =  "אישור אחסון",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "BRD",
                EnglishName =  "Storage Denial",
                LocalName =  "דחיית אחסון",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CGN",
                EnglishName =  "Custom Guarantee Notification",
                LocalName =  "הודעה על ערבות חדשה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "DCA",
                EnglishName =  "Deficit Customs Answer",
                LocalName =  "תשובת מכס בגין גרעון עצמי",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "DFP",
                EnglishName =  "Declaration Future Payment ",
                LocalName =  "הגשה עתידית",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "LIC",
                EnglishName =  "ProceduralFault Cancelled ",
                LocalName =  "בוטל ליקוי להצהרה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "LIK",
                EnglishName =  "New ProceduralFault",
                LocalName =  "התקבל ליקוי להצהרה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRD",
                EnglishName =  "Document Request By Customs",
                LocalName =  "מסמך נדרש על ידי המכס",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "DCN",
                EnglishName =  "Declaration Cancellation",
                LocalName =  "ביטול הצהרה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CAS",
                EnglishName =  "Custom Documents Que",
                LocalName =  "הודעה על המצאת מסמכים",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRC",
                EnglishName =  "Document requested Cancelled by Customs",
                LocalName =  "בוטלה דרישת מסמך",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "VCD",
                EnglishName =  "Constraint Declined by Customs",
                LocalName =  "ממתין לבדיקת יסמ ובקרת מסמכים",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "RAM",
                EnglishName =  "Constraint Approved by Customs",
                LocalName =  "אילוץ אושר במכס",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "VCC",
                EnglishName =  "Custom Check",
                LocalName =  "הצהרה נותבה לתור בחינה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "RCA",
                EnglishName =  "Constraint Sent to Customs",
                LocalName =  "אילוץ נקלט במחשב",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CDC",
                EnglishName =  "Constraint Declined by Customs",
                LocalName =  "אילוץ נדחה עי המכס",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "RSH",
                EnglishName =  "Reshimon received",
                LocalName =  "קבלת רשימון",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "PRS",
                EnglishName =  "Pre Clearancen",
                LocalName =  "הודעה מוקדמת לסוכן מכס",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "DCH",
                EnglishName =  "Declaration Changed By Customs",
                LocalName =  "בוצע תיקון הצהרה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "RPD",
                EnglishName =  "Declaration Re-Payment",
                LocalName =  "הגשה חוזרת של הצהרה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "INP",
                EnglishName =  "Signed Declaration Sent",
                LocalName =  "נשלחה הצהרה חתומה אישית",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "LPA",
                EnglishName =  "Logistic Permit Approved",
                LocalName =  "היתר לוגיסטי אושר",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "LPC",
                EnglishName =  "Logistic Permit Cancelled",
                LocalName =  "היתר לוגיסטי בוטל",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "DNR",
                EnglishName =  "Declaration Number Reset",
                LocalName =  "אופס מספר הצהרה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "VATC",
                EnglishName =  "Vat Changed",
                LocalName =  "חפ השתנה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CREV",
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
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPEV",
                EnglishName =  "Updated",
                LocalName =  "Updated",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "VCI",
                EnglishName =  "Custom Documents Check",
                LocalName =  "הצהרה נותבה לבקרת מסמכים",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "VCR",
                EnglishName =  "Custom Check",
                LocalName =  "הצהרה נותבה לתור רשות",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "VCB",
                EnglishName =  "Custom Security Check",
                LocalName =  "בדיקה בטחונית להצהרה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "VCM",
                EnglishName =  "BOL",
                LocalName =  "התקבל מסר שטר מטען מאסטר",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "VCP",
                EnglishName =  "Loading/Unloading Confirmation",
                LocalName =  "אישור פריקה/טעינה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CDA",
                EnglishName =  "Constraint Conditional Approval",
                LocalName =  "אילוץ מאושר בתנאי",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "PRA",
                EnglishName =  "Release When Arrived",
                LocalName =  "תיק מאושר להתרה לאחר הגשת טובין",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "VAN",
                EnglishName =  "Agent Notification",
                LocalName =  "הודעות לסוכן",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "MNS",
                EnglishName =  "Manifest Sent",
                LocalName =  "מסר מצהר נשלח",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "MNE",
                EnglishName =  "Manifest Error",
                LocalName =  "מסר מצהר שגוי",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "MNC",
                EnglishName =  "Manifest Correct",
                LocalName =  "מסר מצהר תקין",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "MPOA",
                EnglishName =  "Missing Power Of Attorney",
                LocalName =  "חסר יפוי כח",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "MID",
                EnglishName =  "Missing Impoter Declaration",
                LocalName =  "חסר תצהיר יבואן",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "IDE",
                EnglishName =  "Impoter Declaration about to expire",
                LocalName =  "תצהיר יבואן עומד לפוג",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "DCS",
                EnglishName =  "Declaration Close",
                LocalName =  "הצהרה נסגרה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CDCS",
                EnglishName =  "Cancel Declaration Close",
                LocalName =  "ביטול סגירת הצהרה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CSA",
                EnglishName =  "Cargo Split Approved",
                LocalName =  "בקשת פיצול מטען אושרה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CSJ",
                EnglishName =  "Cargo Split Rejected",
                LocalName =  "בקשת פיצול מטען נדחתה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CSC",
                EnglishName =  "Cargo Split Canceled",
                LocalName =  "בקשת פיצול מטען בוטלה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CSD",
                EnglishName =  "Cargo Split Done",
                LocalName =  "בוצע פיצול מטען",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "VCS",
                EnglishName =  "Custom Documents Check",
                LocalName =  "ממתין לבדיקת יסמ",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "VCE",
                EnglishName =  "Custom Documents Check",
                LocalName =  "ממתין ליחידת בטחון",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "VCA",
                EnglishName =  "Custom Documents Check",
                LocalName =  "ממתין ליחידת בטחון ובקרת מסמכים",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "VCG",
                EnglishName =  "Custom Documents Check",
                LocalName =  "ממתין ליחידת הבטחון וליסמ",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "VCT",
                EnglishName =  "Custom Documents Check",
                LocalName =  "ממתין ליחידת הבטחון,ליסמ ולבקרת מסמכים",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "DMA",
                EnglishName =  "Declaration Amendment Approved",
                LocalName =  "תיקון הצהרה אושרה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "DMP",
                EnglishName =  "Declaration Amendment Partial Approval",
                LocalName =  "תיקון הצהרה אושרה חלקית",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "DMD",
                EnglishName =  "Declaration Amendment Denial",
                LocalName =  "תיקון הצהרה נדחתה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "DMC",
                EnglishName =  "Declaration Amendment Cancelled",
                LocalName =  "תיקון הצהרה בוטלה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "DWR",
                EnglishName =  "Amendment Waiting for customs",
                LocalName =  "תיקון הצהרה ממתינה לטיפול מכס",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CAP",
                EnglishName =  "Declaration Canceled",
                LocalName =  "הצהרה בוטלה",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRJ",
                EnglishName =  "Declaration Cancel Denial",
                LocalName =  "ביטול הצהרה נדחה",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CWR",
                EnglishName =  "Declaration Cancel Waiting for customs",
                LocalName =  "ביטול הצהרה ממתין לטיפול המכס",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CPO",
                EnglishName =  "Declaration Cancel Sent",
                LocalName =  "נשלחה בקשה לביטול הצהרה",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "SCH",
                EnglishName =  "נדרש לטפל בהזנת סגר",
                LocalName =  "נדרש לטפל בהזנת סגר",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "RDA",
                EnglishName =  "Required Document Verified",
                LocalName =  "מסמך נדרש אומת",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "RDR",
                EnglishName =  "Required Document Rejected",
                LocalName =  "מסמך נדרש נדחה",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "RDC",
                EnglishName =  "Required Document Verified By Customer",
                LocalName =  "מסמך נדרש אומת בנוכחות לקוח",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = DeclarationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   //FeatureRepository featureRepository = new FeatureRepository(0); 
		   //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable DeclarationObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.Declaration" && d.Tenant == 0).FirstOrDefault();       
    
			   Feature DeclarationFeature_MB0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SENDDECLARATION", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.SendDeclaration", NameTextCodeDefaultText = "Send Declarations", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);

      
    
			   Feature DeclarationFeature_MB1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SENDMANIFEST", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.SendManifest", NameTextCodeDefaultText = "Send Manifest", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);

      
    
			   Feature DeclarationFeature_MB2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DECLARATIONPAYMENT", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.Payment", NameTextCodeDefaultText = "Declaration Payment", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);

      
    
			   Feature DeclarationFeature_MB3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "FORMS", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.Forms", NameTextCodeDefaultText = "Forms", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);

			   Feature DeclarationFeature_MB30 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PRINTTZRUFA", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.PrintTzrufa", NameTextCodeDefaultText = "Print Tzrufa", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
             			   Feature DeclarationFeature_MB31 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PRINTTAZRUFA", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.PrintATzrufa", NameTextCodeDefaultText = "Print Accumulated Tzrufa", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
             			   Feature DeclarationFeature_MB32 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PRINTTDECLARATIONFORM", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.PrintDeclarationForm", NameTextCodeDefaultText = "Print Declaration Form", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
             			   Feature DeclarationFeature_MB33 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PRINTRELEASE", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.PrintRelease", NameTextCodeDefaultText = "Print Release", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
                   
    
			   Feature DeclarationFeature_MB4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CloseDeclaration", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Declaration.Features.CloseDeclaration", NameTextCodeDefaultText = "Close Declaration", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);

      
    
			   Feature DeclarationFeature_MB5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACTIONS", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.Actions", NameTextCodeDefaultText = "Actions", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);

			   Feature DeclarationFeature_MB50 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "STATUSREQUEST", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.StatusRequest", NameTextCodeDefaultText = "Status Request", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
             			   Feature DeclarationFeature_MB51 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DECLARATIONRESTORE", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.DeclarationRestore", NameTextCodeDefaultText = "Declaration Restore", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
             			   Feature DeclarationFeature_MB52 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RESETDECLARATIONNUMBER", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.ResetDeclarationNumber", NameTextCodeDefaultText = "Reset Delaration Number", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
             			   Feature DeclarationFeature_MB53 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "COPY", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.Copy", NameTextCodeDefaultText = "Copy", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
             			   Feature DeclarationFeature_MB54 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TRANSFERTOCOLLECTOR", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.TransferToCollector", NameTextCodeDefaultText = "Transfer To Collector", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
             			   Feature DeclarationFeature_MB55 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "VEHICLEMODIFICATION", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.VehicleModification", NameTextCodeDefaultText = "Vehicle Modification", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
             			   Feature DeclarationFeature_MB56 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DECLARATIONSPECIALACTION", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.SpecialActionRequest", NameTextCodeDefaultText = "Special Action Request", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
             			   Feature DeclarationFeature_MB57 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CourierPendingReason", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Declaration.Features.CourierPendingReason", NameTextCodeDefaultText = "Courier Pending Reason", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
             			   Feature DeclarationFeature_MB58 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CourierPendingReasonDel", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Declaration.Features.CourierPendingReasonDel", NameTextCodeDefaultText = "Courier Pending Reason Del", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
             			   Feature DeclarationFeature_MB59 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DeclarationClosure", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Declaration.Features.DeclarationClosure", NameTextCodeDefaultText = "Declaration Closure", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
             			   Feature DeclarationFeature_MB510 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Cancel Declaration Closure", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Declaration.Features.CancelDeclarationClosure", NameTextCodeDefaultText = "Cancel Declaration Closure", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
             			   Feature DeclarationFeature_MB511 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DeclarationCustomsRequests", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.DeclarationCustomsRequests", NameTextCodeDefaultText = "Declaration Customs Requests", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
             			   Feature DeclarationFeature_MB512 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DeclarationCancellation", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Declaration.Features.DeclarationCancellation", NameTextCodeDefaultText = "Declaration Cancellation", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);
                   
    
			   Feature DeclarationFeature_MB6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DOCUMENTSPANEL", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.Declaration.Features.DocumentsPanel", NameTextCodeDefaultText = "Declaration Documents Panel", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,DeclarationObjectTable);

 

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup DeclarationMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "Customs.DeclarationEdit",
					Name = "Customs.DeclarationEditButtonsGroup",
					ObjectTableId = DeclarationObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton DeclarationMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "SendDeclaration",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "Customs.Declaration.B.SendDeclaration",
						LabelTextCodeDefaultText = "Send Declaration",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "control",
						FeatureId = DeclarationFeature_MB0.Id,
						Style = null,
						LocalDefaultText = "שלח הצהרה",
						FeatureUniqeCode = DeclarationFeature_MB0.FeatureUniqeCode,
						HtmlComponentPath = "./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/SendDeclaration/SendDeclarationComponent",
						Width = 0,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton DeclarationMenuButton1 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "SendManifest",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "Customs.Declaration.B.SendManifest",
						LabelTextCodeDefaultText = "Send Manifest",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "control",
						FeatureId = DeclarationFeature_MB1.Id,
						Style = null,
						LocalDefaultText = "שלח מצהר",
						FeatureUniqeCode = DeclarationFeature_MB1.FeatureUniqeCode,
						HtmlComponentPath = "./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/SendDeclaration/SendManifestComponent",
						Width = 0,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton DeclarationMenuButton2 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "DeclarationPayment",
						Index = 2, 
						IsActive = true,
						LabelTextCodeCode = "Customs.Declaration.B.DeclarationPayment",
						LabelTextCodeDefaultText = "Declaration Payment",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = DeclarationFeature_MB2.Id,
						Style = null,
						LocalDefaultText = "הגשת תשלום",
						FeatureUniqeCode = DeclarationFeature_MB2.FeatureUniqeCode,
						HtmlComponentPath = null,
						Width = 0,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton DeclarationMenuButton3 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Forms",
						Index = 3, 
						IsActive = false,
						LabelTextCodeCode = "Customs.Declaration.B.Forms",
						LabelTextCodeDefaultText = "Forms",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "dropdownbutton",
						FeatureId = DeclarationFeature_MB3.Id,
						Style = null,
						LocalDefaultText = "טפסים",
						FeatureUniqeCode = DeclarationFeature_MB3.FeatureUniqeCode,
						HtmlComponentPath = null,
						Width = 0,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

			   MenuButton DeclarationMenuButton30 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "PrintTzrufa",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "Customs.Declaration.B.PrintTzrufa",
						LabelTextCodeDefaultText = "Print Tzrufa",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ParentMenuButtonId = DeclarationMenuButton3.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  DeclarationFeature_MB30.Id,
						Style = null,
						LocalDefaultText = "צרופה",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  DeclarationFeature_MB30.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton DeclarationMenuButton31 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "PrintAccumaltedTzrufa",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "Customs.Declaration.B.PrintAccumaltedTzrufa",
						LabelTextCodeDefaultText = "Print Accumalted Tzrufa",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ParentMenuButtonId = DeclarationMenuButton3.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  DeclarationFeature_MB31.Id,
						Style = null,
						LocalDefaultText = "צרופה צבורה",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  DeclarationFeature_MB31.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton DeclarationMenuButton32 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "PrintDeclarationForm",
						Index = 2, 
						IsActive = true,
						LabelTextCodeCode = "Customs.Declaration.B.PrintDeclarationForm",
						LabelTextCodeDefaultText = "Print Declaration Form",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ParentMenuButtonId = DeclarationMenuButton3.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  DeclarationFeature_MB32.Id,
						Style = null,
						LocalDefaultText = "טופס הצהרה",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  DeclarationFeature_MB32.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton DeclarationMenuButton33 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "PrintRelease",
						Index = 3, 
						IsActive = true,
						LabelTextCodeCode = "Customs.Declaration.B.PrintRelease",
						LabelTextCodeDefaultText = "Print Release",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ParentMenuButtonId = DeclarationMenuButton3.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  DeclarationFeature_MB33.Id,
						Style = null,
						LocalDefaultText = "שחרור חלקי",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  DeclarationFeature_MB33.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	          
   
			   MenuButton DeclarationMenuButton4 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CloseDeclaration",
						Index = 3, 
						IsActive = true,
						LabelTextCodeCode = "Declaration.B.CloseDeclaration",
						LabelTextCodeDefaultText = "Close Declaration",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = DeclarationFeature_MB4.Id,
						Style = null,
						LocalDefaultText = "סגירת הצהרה",
						FeatureUniqeCode = DeclarationFeature_MB4.FeatureUniqeCode,
						HtmlComponentPath = null,
						Width = 0,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton DeclarationMenuButton5 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Actions",
						Index = 4, 
						IsActive = false,
						LabelTextCodeCode = "Customs.Declaration.B.Actions",
						LabelTextCodeDefaultText = "Actions",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "dropdownbutton",
						FeatureId = DeclarationFeature_MB5.Id,
						Style = null,
						LocalDefaultText = "פעולות",
						FeatureUniqeCode = DeclarationFeature_MB5.FeatureUniqeCode,
						HtmlComponentPath = null,
						Width = 0,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

			   MenuButton DeclarationMenuButton50 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "DeclarationsStatusRequest",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "Customs.Declaration.B.DeclarationsStatusRequest",
						LabelTextCodeDefaultText = "Declarations Status Request",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ParentMenuButtonId = DeclarationMenuButton5.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  DeclarationFeature_MB50.Id,
						Style = null,
						LocalDefaultText = "בדיקת סטטוס הצהרה",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  DeclarationFeature_MB50.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton DeclarationMenuButton51 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "DeclarationRestore",
						Index = 2, 
						IsActive = true,
						LabelTextCodeCode = "Customs.Declaration.B.DeclarationRestore",
						LabelTextCodeDefaultText = "Declaration Restore",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ParentMenuButtonId = DeclarationMenuButton5.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  DeclarationFeature_MB51.Id,
						Style = null,
						LocalDefaultText = "שחזור נתוני הצהרה",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  DeclarationFeature_MB51.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton DeclarationMenuButton52 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "ResetDeclarationNumber",
						Index = 3, 
						IsActive = true,
						LabelTextCodeCode = "Customs.Declaration.B.ResetDeclarationNumber",
						LabelTextCodeDefaultText = "Reset Declaration Number",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ParentMenuButtonId = DeclarationMenuButton5.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  DeclarationFeature_MB52.Id,
						Style = null,
						LocalDefaultText = "איפוס מספר הצהרה",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  DeclarationFeature_MB52.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton DeclarationMenuButton53 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Copy",
						Index = 4, 
						IsActive = true,
						LabelTextCodeCode = "Customs.Declaration.B.Copy",
						LabelTextCodeDefaultText = "Copy",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ParentMenuButtonId = DeclarationMenuButton5.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  DeclarationFeature_MB53.Id,
						Style = null,
						LocalDefaultText = "העתקת הצהרה",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  DeclarationFeature_MB53.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton DeclarationMenuButton54 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "TransferToCollector",
						Index = 5, 
						IsActive = true,
						LabelTextCodeCode = "Customs.Declaration.B.TransferToCollector",
						LabelTextCodeDefaultText = "Transfer To Collector",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ParentMenuButtonId = DeclarationMenuButton5.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  DeclarationFeature_MB54.Id,
						Style = null,
						LocalDefaultText = "העברה לגובה",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  DeclarationFeature_MB54.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton DeclarationMenuButton55 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Vehicle Modifications",
						Index = 6, 
						IsActive = true,
						LabelTextCodeCode = "Customs.Declaration.B.VehicleModifications",
						LabelTextCodeDefaultText = "Vehicle Modifications",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ParentMenuButtonId = DeclarationMenuButton5.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  DeclarationFeature_MB55.Id,
						Style = null,
						LocalDefaultText = "הפחתות לשילדה",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  DeclarationFeature_MB55.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton DeclarationMenuButton56 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "SpecialActionRequest",
						Index = 7, 
						IsActive = true,
						LabelTextCodeCode = "Customs.General.O.SpecialActivityRequest",
						LabelTextCodeDefaultText = "Special Action Request",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ParentMenuButtonId = DeclarationMenuButton5.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  DeclarationFeature_MB56.Id,
						Style = null,
						LocalDefaultText = "בקשה לפעולה מיוחדת",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  DeclarationFeature_MB56.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton DeclarationMenuButton57 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CourierPendingReason",
						Index = 8, 
						IsActive = true,
						LabelTextCodeCode = "Customs.Declaration.B.CourierPendingReason",
						LabelTextCodeDefaultText = "Courier Pending Reason",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ParentMenuButtonId = DeclarationMenuButton5.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  DeclarationFeature_MB57.Id,
						Style = null,
						LocalDefaultText = "Pending",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  DeclarationFeature_MB57.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton DeclarationMenuButton58 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CourierPendingReasonDel",
						Index = 10, 
						IsActive = true,
						LabelTextCodeCode = "Declaration.B.CourierPendingReasonDel",
						LabelTextCodeDefaultText = "Courier Pending Reason Del",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ParentMenuButtonId = DeclarationMenuButton5.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  DeclarationFeature_MB58.Id,
						Style = null,
						LocalDefaultText = "מחיקת Pending",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  DeclarationFeature_MB58.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton DeclarationMenuButton59 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Declaration Closure",
						Index = 11, 
						IsActive = true,
						LabelTextCodeCode = "Customs.Declaration.B.DeclarationClosure",
						LabelTextCodeDefaultText = "Declaration Closure",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ParentMenuButtonId = DeclarationMenuButton5.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  DeclarationFeature_MB59.Id,
						Style = null,
						LocalDefaultText = "סגירת הצהרה",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  DeclarationFeature_MB59.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton DeclarationMenuButton510 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Cancel Declaration Closure",
						Index = 12, 
						IsActive = true,
						LabelTextCodeCode = "Customs.Declaration.B.CancelDeclarationClosure",
						LabelTextCodeDefaultText = "Cancel Declaration Closure",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ParentMenuButtonId = DeclarationMenuButton5.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  DeclarationFeature_MB510.Id,
						Style = null,
						LocalDefaultText = "ביטול סגירת הצהרה",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  DeclarationFeature_MB510.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton DeclarationMenuButton511 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Declaration Customs Requests",
						Index = 13, 
						IsActive = true,
						LabelTextCodeCode = "Customs.Declaration.B.DeclarationCustomsRequests",
						LabelTextCodeDefaultText = "Declaration Customs Requests",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ParentMenuButtonId = DeclarationMenuButton5.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  DeclarationFeature_MB511.Id,
						Style = null,
						LocalDefaultText = "בקשות מכס",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  DeclarationFeature_MB511.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton DeclarationMenuButton512 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "DeclarationCancellation",
						Index = 14, 
						IsActive = true,
						LabelTextCodeCode = "Declaration.B.DeclarationCancellation",
						LabelTextCodeDefaultText = "Declaration Cancellation",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ParentMenuButtonId = DeclarationMenuButton5.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  DeclarationFeature_MB512.Id,
						Style = null,
						LocalDefaultText = "ביטול הצהרה",
                        HtmlComponentPath="./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/DeclarationCancellation/DeclarationCancellationComponent",
                        Width=0,
						FeatureUniqeCode=  DeclarationFeature_MB512.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	          
   
			   MenuButton DeclarationMenuButton6 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "DocumentsPanel",
						Index = 99, 
						IsActive = true,
						LabelTextCodeCode = "Customs.Declaration.B.DocumentsPanel",
						LabelTextCodeDefaultText = "Documents Panel",
						Tenant = 0,
						MenuButtonGroupId = DeclarationMenuButtonGroup.Id,
						ObjectTableId = DeclarationObjectTable.Id,
						MenuButtonType = "control",
						FeatureId = DeclarationFeature_MB6.Id,
						Style = null,
						LocalDefaultText = "רשימת מסמכים",
						FeatureUniqeCode = DeclarationFeature_MB6.FeatureUniqeCode,
						HtmlComponentPath = "./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/DocumentsPanel/DocumentsPanelComponent",
						Width = 30,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable DeclarationObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.Declaration" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode DeclarationTextCode_CustomsDeclarationORequestedDocument = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.RequestedDocument", DefaultText = "Requested Document",LocalDefaultText = @"מסמך נדרש", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSendAmendmentDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SendAmendmentDeclaration", DefaultText = "Send Amendment",LocalDefaultText = @"שלח תיקון", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOChangeAmendment = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ChangeAmendment", DefaultText = "Change Amendment",LocalDefaultText = @"החלפת בקשה לתיקון", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOConsignmentPackages = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ConsignmentPackages", DefaultText = "Cargo Serial Data",LocalDefaultText = @"נתוני סידורי במטען", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOConsignmentDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ConsignmentDetails", DefaultText = "Consignment Details",LocalDefaultText = @"פרטי משלוח", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODeletePackage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DeletePackage", DefaultText = "Delete this Package?",LocalDefaultText = @"מחק את החבילה הזו?", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODeleteConsignment = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DeleteConsignment", DefaultText = "Delete this Consignment?",LocalDefaultText = @"מחק את המשלוח הזה?", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationONewPackage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.NewPackage", DefaultText = "New Package",LocalDefaultText = @"חבילה חדשה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOPaymentMethod = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.PaymentMethod", DefaultText = "Payment Method",LocalDefaultText = @"פירוט תשלום", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsPaymentOrderOProtest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentOrder.O.Protest", DefaultText = "Protest",LocalDefaultText = @"אגב מחאה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSend = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Send", DefaultText = "Send",LocalDefaultText = @"שלח", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOEditInvoiceItem = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.EditInvoiceItem", DefaultText = "Edit Invoice Item",LocalDefaultText = @"עריכת פריט חשבונית", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODeclarations = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Declarations", DefaultText = "Declarations",LocalDefaultText = @"הצהרות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOItems = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Items", DefaultText = "Items",LocalDefaultText = @"פרטי חשבון ספק", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Details", DefaultText = "Details",LocalDefaultText = @"פרטים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOModifications = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Modifications", DefaultText = "Modifications",LocalDefaultText = @"שינויים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationONewInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.NewInvoice", DefaultText = "New Invoice",LocalDefaultText = @"חשבונית חדשה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOEditInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.EditInvoice", DefaultText = "Edit Invoice",LocalDefaultText = @"חשבון ספק", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODeleteInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DeleteInvoice", DefaultText = "Delete this Invoice?",LocalDefaultText = @"האם למחוק את החשבון ושורות פרטי המכס", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOTotalmustbeequaltototaltax = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Totalmustbeequaltototaltax", DefaultText = "Total must be equal to total tax.",LocalDefaultText = "סה''כ חייב להיות שווה למס הכולל.", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCertificates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Certificates", DefaultText = "Certificates",LocalDefaultText = @"תעודות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOFreightAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.FreightAmount", DefaultText = "Freight Amount",LocalDefaultText = @"נתוני הובלה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOInsurance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Insurance", DefaultText = "Insurance",LocalDefaultText = @"נתוני ביטוח", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Amount", DefaultText = "Amount",LocalDefaultText = @"סכום", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOPercentage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Percentage", DefaultText = "Percentage",LocalDefaultText = @"אחוז", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOAddCustomsDocument = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.AddCustomsDocument", DefaultText = "Add Customs Document",LocalDefaultText = @"הוסף מסמך המכס", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOEditCustomsDocument = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.EditCustomsDocument", DefaultText = "Edit Customs Document",LocalDefaultText = @"מסמך מכס עריכה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationONumbersAreOnlyAllowed = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.NumbersAreOnlyAllowed", DefaultText = "Numbers are only allowed",LocalDefaultText = @"מספרים מותר רק", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODuplicatevaluesarenotAllowed = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DuplicatevaluesarenotAllowed", DefaultText = "Duplicate values are not Allowed",LocalDefaultText = @"ערכים כפולים אינם מורשים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationONumbersAndCommasAreOnlyAllowed = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.NumbersAndCommasAreOnlyAllowed", DefaultText = "Numbers and Commas are only allowed",LocalDefaultText = @"מספרים והפסיקים מותר רק", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSearchDeclarationStatus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SearchDeclarationStatus", DefaultText = "Search Declaration Status",LocalDefaultText = @"סטטוס הכרזת חיפוש", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODeclarationStatusRequest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DeclarationStatusRequest", DefaultText = "Declaration Status Request",LocalDefaultText = @"בקשת סטטוס הצהרה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODeclarationMamanRequest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DeclarationMamanRequest", DefaultText = "Declaration Maman Request",LocalDefaultText = @"מסר תת מצהר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSendRequest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SendRequest", DefaultText = "Send Request",LocalDefaultText = @"בדיקת סטטוס הצהרה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSearchByDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SearchByDeclaration", DefaultText = "Search By Declaration",LocalDefaultText = @"חפש לפי הצהרה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODeclarationStatus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DeclarationStatus", DefaultText = "Declaration Status:",LocalDefaultText = @"סטטוס הצהרה:", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSearchByCargo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SearchByCargo", DefaultText = "Search By Cargo",LocalDefaultText = @"חפש לפי מטענים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOOldReshimonRadio = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.OldReshimonRadio", DefaultText = "Search By Old Reshimon",LocalDefaultText = @"חפש לפי רשימון ישן", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOOldReshimon = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.OldReshimon", DefaultText = "Old Reshimon Number",LocalDefaultText = @"מספר רשימון ישן", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOOldReshimonIsMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.OldReshimonIsMandatory", DefaultText = "Old Reshimon is missing",LocalDefaultText = @"מספר רשימון ישן חסר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSendDeclarationConstraint = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SendDeclarationConstraint", DefaultText = "Send Declaration Constraint",LocalDefaultText = @"שלח אילוץ הצהרה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODescription = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Description", DefaultText = "Description",LocalDefaultText = @"תיאור", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOErrorType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ErrorType", DefaultText = "Error Type",LocalDefaultText = @"קוד השגיאה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOField = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Field", DefaultText = "Field",LocalDefaultText = @"שדה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOLine = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Line", DefaultText = "Line",LocalDefaultText = @"קו", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOScreen = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Screen", DefaultText = "Screen",LocalDefaultText = @"מסך", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOLink = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Link", DefaultText = "Link",LocalDefaultText = @"קשר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOConstraintIndication = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ConstraintIndication", DefaultText = "Constraint Indication",LocalDefaultText = @"הוריה אילוץ", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOConstraintData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ConstraintData", DefaultText = "Constraint Data",LocalDefaultText = @"נתונים אילוץ", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOEditDocumentMetaData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.EditDocumentMetaData", DefaultText = "Edit Document MetaData",LocalDefaultText = @"עריכת מסמך", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOConnectedToDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ConnectedToDeclaration", DefaultText = "Connected To Declaration",LocalDefaultText = @"מחובר להכרזה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODisconnectedDocument = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DisconnectedDocument", DefaultText = "Disconnected  Document",LocalDefaultText = @"מסמך מנותק", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODocumentPreview = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DocumentPreview", DefaultText = "Document Preview",LocalDefaultText = @"מסמך מקדימה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODocumentMetaData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DocumentMetaData", DefaultText = "Document MetaData",LocalDefaultText = @"נתוני מטה דאטה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationORelatedDocuments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.RelatedDocuments", DefaultText = "Related Documents",LocalDefaultText = @"מסמכים מקושרים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODocuments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Documents", DefaultText = "Documents",LocalDefaultText = @"מסמכים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODocumentAndCustomDocumentType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DocumentAndCustomDocumentType", DefaultText = "Document and custom Document must be the same type!",LocalDefaultText = @"מסמך ומסמך מותאם אישית חייב להיות מאותו הסוג!", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOMetaDataEdit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.MetaDataEdit", DefaultText = "You must edit the metadata for the document first!",LocalDefaultText = @"עליך לערוך מטה של המסמך ראשון!", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOGeneralData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.GeneralData", DefaultText = "GeneralData",LocalDefaultText = @"נתונים כלליים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODeclarationTaxesLines = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DeclarationTaxesLines", DefaultText = "Declaration Taxes Lines",LocalDefaultText = @"מיסים ברמת הצהרה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOItemTaxes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ItemTaxes", DefaultText = "Item Taxes",LocalDefaultText = @"מיסים ברמת פרט מכס", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCollateralData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CollateralData", DefaultText = "Collateral Data",LocalDefaultText = @"נתוני הבטוחה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCollateralAnswer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CollateralAnswer", DefaultText = "Collateral Answer",LocalDefaultText = @"מענה לדרישה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCollateralCondition = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CollateralCondition", DefaultText = "Collateral Condition",LocalDefaultText = @"פירוט הסכום המבוקש", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOAddAnswer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.AddAnswer", DefaultText = "Add Answer",LocalDefaultText = @"הוסף מענה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOMultiCollateralsAnswer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.MultiCollateralsAnswer", DefaultText = "Multi Answers",LocalDefaultText = @"מענה מרוכז", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOEditCustomsCollateral = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.EditCustomsCollateral", DefaultText = "Edit Customs Collateral",LocalDefaultText = @"בטוחות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCreateNewFileRequest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CreateNewFileRequest", DefaultText = "New File Request",LocalDefaultText = @"בקשה לפתיחת תיק תפ”ג", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOOpenDeclarationAmendment = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.OpenDeclarationAmendment", DefaultText = "Open Declaration Amendment",LocalDefaultText = @"פתיחת בקשה לתיקון", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOInvoiceDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.InvoiceDetails", DefaultText = "Invoice Details",LocalDefaultText = @"פרטי חשבונית", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOEntity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Entity", DefaultText = "Entity",LocalDefaultText = @"מקור השגיאה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOListVersion = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ListVersion", DefaultText = "List Version",LocalDefaultText = @"סוג שגיאה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODifference = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Difference", DefaultText = "Difference",LocalDefaultText = @"הפרש", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOTotalForeignCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.TotalForeignCurrency", DefaultText = "Total Foreign Currency",LocalDefaultText = "סה''כ מט''ח", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOExistingType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ExistingType", DefaultText = "Sorry you can't choose an existing type",LocalDefaultText = @"לא ניתן לבחור אותו קוד סוג יותר מפעם אחת", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOFillAgentExplanation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.FillAgentExplanation", DefaultText = "Fill agent explanation field first.",LocalDefaultText = @"מלא שדה הסבר הסוכן ראשון.", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOHasMetaData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.HasMetaData", DefaultText = "This document has metadata",LocalDefaultText = @"מסמך זה יש מידע נוסף על הקובץ", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOEmptyConsignmentPackage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.EmptyConsignmentPackage", DefaultText = "You can't add an empty consignment package!",LocalDefaultText = @"אתה לא יכול להוסיף חבילת משלוח ריקה!", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOTotalAllocatedAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.TotalAllocatedAmount", DefaultText = "Total Allocated Amount",LocalDefaultText = "סה''כ סכום מענה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOTotalAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.TotalAmount", DefaultText = "Total  Amount",LocalDefaultText = "סה''כ סכום שהוזן", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOTaxesModifications = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.TaxesModifications", DefaultText = "Taxes Modifications",LocalDefaultText = @"שינויים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCantAddToConnected = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CantAddToConnected", DefaultText = "Document cannot be added to a connected Pointer!",LocalDefaultText = @"מסמך לא ניתן להוסיף לכרטיס מחובר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOByDeclarationOrFile = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ByDeclarationOrFile", DefaultText = "By File/Declaration",LocalDefaultText = @"לפי תיק/הצהרה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOByStorageSite = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ByStorageSite", DefaultText = "By Storage Site and Warehouse Block",LocalDefaultText = @"לפי אתר אחסון וגוש", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCodeShort = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CodeShort", DefaultText = "Classification Code Too Short",LocalDefaultText = @"פרט מכס קצר מידי", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCodeLong = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CodeLong", DefaultText = "Classification Code Too Long",LocalDefaultText = @"פרט המכס ארוך מדי", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCorrectDigit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CorrectDigit", DefaultText = "Check digit is incorrect ,the correct digit is ",LocalDefaultText = @" ספרת הביקורת שגויה , הספרה הנכונה היא ", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOAmountsNotCompatableToIncoterm = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.AmountsNotCompatableToIncoterm", DefaultText = "Insurance are not compitable to Incoterms ,Continue?",LocalDefaultText = @" אין התאמה לתנאי המכר , האם להמשיך ? ", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODifferentTotals = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DifferentTotals", DefaultText = "Tax to pay is different than File taxes , screen is display only",LocalDefaultText = @"המס לתשלום שונה מהמיסים לתיק , המסך לתצוגה בלבד", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOPaidDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.PaidDeclaration", DefaultText = "Declaration was already paid , screen is display only",LocalDefaultText = @"הצהרה כבר שולמה , המסך לתצוגה בלבד", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOWaitingApproval = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.WaitingApproval", DefaultText = "Declaration Paid , waiting for constraint approval",LocalDefaultText = @"טיוטה הוגשה , ממתינה לאילוץ הגשה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOIsAmendmentDontDisplay = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.IsAmendmentDontDisplay", DefaultText = "Amendment Declaration , screen is display only",LocalDefaultText = @"לתצוגה בלבד - הצהרת תיקון", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOIsAmendment = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.IsAmendment", DefaultText = "Amendment Declaration",LocalDefaultText = @"הצהרת תיקון", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOFuturePayment = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.FuturePayment", DefaultText = "Future payment was done",LocalDefaultText = @"בוצעה הגשה עתידית", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOChangedDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ChangedDeclaration", DefaultText = "Declaration data was changed , please send again before trying to pay",LocalDefaultText = @"בוצעו שינויים בהצהרה , יש לשדר שוב לפני הגשת תשלום", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationONewDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.NewDeclaration", DefaultText = "New Declaration",LocalDefaultText = @"הצהרה חדשה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCustomFileIsAlreadyEntered = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CustomFileIsAlreadyEntered", DefaultText = "Custom file is already entered, can’t create new declaration",LocalDefaultText = @"הוזן תיק עמילות , לא ניתן לפתוח הצהרה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOClientIsMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ClientIsMandatory", DefaultText = "Client is mandatory",LocalDefaultText = @"יש להזין לקוח", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOMatch = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Match", DefaultText = "Transport Mode not match to DeclarationOfficeCode",LocalDefaultText = @"סוג ההובלה לא תואם לתחנת המכס", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODeclarationOfficeCodeMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DeclarationOfficeCodeMandatory", DefaultText = "DeclarationOfficeCode is mandatory",LocalDefaultText = @"יש להזין בית מכס", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOTransportModeIdMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.TransportModeIdMandatory", DefaultText = "TransportModeId  is mandatory",LocalDefaultText = @"יש להזין סוג הובלה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODidntfindcustomfile = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Didntfindcustomfile", DefaultText = "Didn't find custom file",LocalDefaultText = @"לא נמצא תיק עמילות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODeclarationTaxChanged = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DeclarationTaxChanged", DefaultText = "Changes were made in declaration , Taxes are not up to date",LocalDefaultText = @"בוצעו שינויים בהצהרה , מסך מיסים אינו עדכני", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODidntfindDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DidntfindDeclaration", DefaultText = "Didn't find Declaration",LocalDefaultText = @"לא נמצא מס' הצהרה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOMetaDataReady = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.MetaDataReady", DefaultText = "Metadata is ready",LocalDefaultText = @"נתוני מטה-דאטה מוכנים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOMetaDataNotReady = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.MetaDataNotReady", DefaultText = "Metadata is not ready",LocalDefaultText = @"נתוני מטה-דאטה אינם מוכנים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOConstraintType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ConstraintType", DefaultText = "Constraint Type",LocalDefaultText = @"אילוץ", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCustomConstraint = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CustomConstraint", DefaultText = "Custom Constraints",LocalDefaultText = @"אילוצים מותאמים אישית", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODeclarationNumberIsMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DeclarationNumberIsMandatory", DefaultText = "Declaration Number is mandatory",LocalDefaultText = @"מספר הצהרה הוא חובה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOTotal = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Total", DefaultText = "Total:",LocalDefaultText = @"סה”כ:", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOPaymentMethodFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.PaymentMethodFields", DefaultText = "All fields must be filled",LocalDefaultText = @"יש למלא את כל השדות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSendPaymentOrder = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SendPaymentOrder", DefaultText = "Send Payment Order",LocalDefaultText = @"שלח להזמין תשלום", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationONo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.No", DefaultText = "No",LocalDefaultText = @"לא", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOYes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Yes", DefaultText = "Yes",LocalDefaultText = @"כן", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOLevies = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Levies", DefaultText = "Levies",LocalDefaultText = @"היטלים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOPackages = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Packages", DefaultText = "Packages",LocalDefaultText = @"נתוני אחסנה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationORestoreMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.RestoreMessage", DefaultText = "Restore Messages Request",LocalDefaultText = @"שיחזור מסרים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationORestoreByCorrelation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.RestoreByCorrelation", DefaultText = "Restore By Correlation",LocalDefaultText = @"לפי קורולציה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationORestoreByDates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.RestoreByDates", DefaultText = "Restore By Dates",LocalDefaultText = @"לפי תאריכים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCorrelationNo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CorrelationNo", DefaultText = "Correlation No.",LocalDefaultText = @"מספר קורולציה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationORestoreInterfaceName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.RestoreInterfaceName", DefaultText = "Interface Name",LocalDefaultText = @"שם השירות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCorrelationNumberIsMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CorrelationNumberIsMandatory", DefaultText = "Corrlation Number is missing",LocalDefaultText = @"מספר הקורולציה חסר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOInterfaceManagementsCodeIsMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.InterfaceManagementsCodeIsMandatory", DefaultText = "Interface Name is missing",LocalDefaultText = @"שם השירות חסר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOFromDateIsMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.FromDateIsMandatory", DefaultText = "From Date is missing",LocalDefaultText = @"מ-תאריך חסר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOToDateIsMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ToDateIsMandatory", DefaultText = "To Date is missing",LocalDefaultText = @"עד-תאריך חסר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOAddNewCustomer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.AddNewCustomer", DefaultText = "Add New Customer",LocalDefaultText = @"להוסיף לקוחות חדשים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationONoPaymentDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.NoPaymentDate", DefaultText = "Declaration was already paid , can’t send",LocalDefaultText = @"הצהרה כבר שולמה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationONoImporterId = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.NoImporterId", DefaultText = "Need to retrieve client before sending",LocalDefaultText = @"יש לשלוף לקוח מהמכס לפני שליחה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOConstraintsInProgress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ConstraintsInProgress", DefaultText = "Declaration Paid , waiting for constraint approval",LocalDefaultText = @"טיוטה ממתינה לאישור אילוץ הגשה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOFuturePaymentDone = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.FuturePaymentDone", DefaultText = "Future payment was done",LocalDefaultText = @"בוצעה הגשה עתידית", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOTaxationDateTimeNotToday = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.TaxationDateTimeNotToday", DefaultText = "Taxes date is different from today , continue ?",LocalDefaultText = @"תאריך חישוב מיסים שונה מהיום , האם לעדכן לתאריך של היום?", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOTaxationDateTimeCheck = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.TaxationDateTimeCheck", DefaultText = "Taxation date validation",LocalDefaultText = @"בדיקת תאריך חישוב מיסים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODocumetsUploaded = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DocumetsUploaded", DefaultText = "Not all documets were uploaded , continue ?",LocalDefaultText = @"לא כל המסמכים הועלו למכס  האם להמשיך ?", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationONewFileRequest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.NewFileRequest", DefaultText = "New File Request",LocalDefaultText = @"תנאי הבטוחה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationONewFileExist = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.NewFileExist", DefaultText = "New file data exist , delete it ?",LocalDefaultText = @"קיימים נתוני בקשה לתיק חדש , למחוק אותם?", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCustomsFileNoExists = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CustomsFileNoExists", DefaultText = "File already Exist",LocalDefaultText = @"תיק כבר קיים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSentToDCA = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SentToDCA", DefaultText = "Successfully sent to Customs. Answer'll arrive via DCA",LocalDefaultText = @"נשלח למכס בהצלחה , משוב יתקבל בכספת", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationORefreshConsignment = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.RefreshConsignment", DefaultText = "Refresh Consignment",LocalDefaultText = @"שאילתא למצהר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSerialNumbers = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SerialNumbers", DefaultText = "Serial Numbers",LocalDefaultText = @"נתוני מוצר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSerialNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SerialNumber", DefaultText = "Serial Numbers",LocalDefaultText = @"סיראליים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODescriptions = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Descriptions", DefaultText = "Descriptions",LocalDefaultText = @"תיאור סחורה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOProductIdentifications = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ProductIdentifications", DefaultText = "Product Identifications",LocalDefaultText = @"זיהוי סחורה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOProcessTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ProcessTypes", DefaultText = "Process Types",LocalDefaultText = @"סוגי תהליכים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCargoTypeCodeIsMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CargoTypeCodeIsMandatory", DefaultText = "Cargo Type is missing",LocalDefaultText = @"מזהה מטען חסר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOFirstCargoIdIsMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.FirstCargoIdIsMandatory", DefaultText = "First Cargo ID is missing",LocalDefaultText = @"מזהה מטען ראשון חסר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOTotalTaxes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.TotalTaxes", DefaultText = "Total Taxes",LocalDefaultText = @"סה”כ מיסים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Currency", DefaultText = "Currency",LocalDefaultText = @"מטבע", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCustomsDocumentRemarks = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CustomsDocumentRemarks", DefaultText = "Remarks",LocalDefaultText = @"הערות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOMissingFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.MissingFields", DefaultText = "Missing fields",LocalDefaultText = @"שדות חסרים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODragHere = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DragHere", DefaultText = "Drag Here",LocalDefaultText = @"גרור לכאן", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOOr = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Or", DefaultText = "Or",LocalDefaultText = @"או", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOViewDocumentsQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ViewDocumentsQuery", DefaultText = "View Documents",LocalDefaultText = @"שאילתא למסמכים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCalculatedFee = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CalculatedFee", DefaultText = "Please Choose Calculated Fee",LocalDefaultText = @"יש להשתמש בסוג אגרת נמל מוצהרת", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODisconnectNotAllowed = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DisconnectNotAllowed", DefaultText = "Can't disconnect a ticket with a request in progress.",LocalDefaultText = @".לא ניתן לנתק מסמך עם בקשה בתהליך", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOOneInvoiceSelected = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.OneInvoiceSelected", DefaultText = "One invoice must be selected",LocalDefaultText = @"ניתן לסמן חשבונית עיקרית אחת בלבד", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSendDocument = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SendDocument", DefaultText = "Send",LocalDefaultText = @"שלח מסמך", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODeleteSite = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DeleteSite", DefaultText = "Delete this internal site?",LocalDefaultText = @"למחוק את המעבר הפנימי ?", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCertificateMandatoryFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CertificateMandatoryFields", DefaultText = "Some lines are without Mandatory fields , Continue ?",LocalDefaultText = @"קיימים אישורים שלא הוזן בהם שדות חובה , להמשיך ?", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Date", DefaultText = "Date",LocalDefaultText = @"תאריך", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOResetDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ResetDeclaration", DefaultText = "Are you sure you want to reset declaration number ?",LocalDefaultText = @"האם בטוח שברצונך לאפס את מספר ההצהרה ?", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODeclarationReset = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DeclarationReset", DefaultText = "Declaration Number Was reset",LocalDefaultText = @"מספר הצהרה אופס בהצלחה - יש לעדכן נתונים ולשדר מחדש", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSearchItems = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SearchItems", DefaultText = "Search",LocalDefaultText = @"פרט/פריט/סכום", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCantCopy = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CantCopy", DefaultText = "Supplier invoice exist can't copy",LocalDefaultText = @"קיימים חשבונות ספק , לא ניתן לבצע העתקה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCopyData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CopyData", DefaultText = "Copy Data from File",LocalDefaultText = @"העתק נתוני מתיק עמילות ", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOToFile = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ToFile", DefaultText = "To File",LocalDefaultText = @"לתיק עמילות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODeleteAmounts = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DeleteAmounts", DefaultText = "Freight And Insurance Values will be deleted",LocalDefaultText = @"נתוני ביטוח ימחקו, להמשיך ?", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCorrectionGeneral = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CorrectionGeneral", DefaultText = "General Data",LocalDefaultText = @"תיקון הצהרה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCorrectionStatement = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CorrectionStatement", DefaultText = "Statement",LocalDefaultText = @"נתוני תיקון", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOAmendments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Amendments", DefaultText = "Amendments",LocalDefaultText = @"שינויים שבוצעו", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOExistsAmendments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ExistsAmendments", DefaultText = "Exists declaration amendment in status ",LocalDefaultText = @"קיים תיקון הצהרה בסטטוס", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOTime = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Time", DefaultText = "Time",LocalDefaultText = @"שעה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOVersion = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Version", DefaultText = "Version",LocalDefaultText = @"גרסה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOStatement = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Statement", DefaultText = "Statement",LocalDefaultText = @"תיאור", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOContent = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Content", DefaultText = "Content",LocalDefaultText = @"ערך", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOVatChanged = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.VatChanged", DefaultText = "Customer VAT is different than Importer VAT , Continue anyway ?",LocalDefaultText = @"מספר החפ לא תואם ללקוח בתיק , להמשיך בכל זאת ?", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSystemMessages = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SystemMessages", DefaultText = "System Messages",LocalDefaultText = @"הודעות מערכת", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationONoAmendments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.NoAmendments", DefaultText = "There are no amendments in this declaration",LocalDefaultText = @"לא בוצעו תיקונים בהצהרה זו", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSaveDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SaveDeclaration", DefaultText = "Data will be saved, continue?",LocalDefaultText = @"?יש לשמור את נתוני ההצהרה , המשך ", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCertificateNotMandatoryFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CertificateNotMandatoryFields", DefaultText = "Entered data fields are not mandatory, continue?",LocalDefaultText = @"?הוזנו נתונים בשדות שאינם חובה , האם להמשיך", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCopyDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CopyDeclaration", DefaultText = "Copy Declaration",LocalDefaultText = @"העתקת הצהרה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOTransferToCollector = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.TransferToCollector", DefaultText = "Aprove Transfer To Collector",LocalDefaultText = @"אשר העברה לגובה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsGeneralOSendClaim = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.SendClaim", DefaultText = "SendClaim",LocalDefaultText = @"מסר תביעה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsGeneralOAnswerSent = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.AnswerSent", DefaultText = "Answer Sent to Customs - display Only”",LocalDefaultText = @"לתצוגה בלבד - נשלח מענה לבטוחה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsGeneralOEmptyVehicle = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.EmptyVehicle", DefaultText = "You can't add an empty vehicle",LocalDefaultText = @"אתה לא יכול להוסיף רכב ריקה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsGeneralOMessageError = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.MessageError", DefaultText = "Message Error",LocalDefaultText = @"מידע נוסף", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsGeneralOOldValue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.OldValue", DefaultText = "Old Value",LocalDefaultText = @"ערך ישן", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsGeneralONewValue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.NewValue", DefaultText = "New Value",LocalDefaultText = @"ערך חדש", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsGeneralOCancelMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CancelMessage", DefaultText = "Are you sure you want to cancel? Your data may lost",LocalDefaultText = @"בוצעו שינויים שלא נשמרו , האם ברצונך לשמור אותם ?", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsGeneralONext = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.Next", DefaultText = "Next",LocalDefaultText = @"הבא", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsGeneralOPrevious = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.Previous", DefaultText = "Previous",LocalDefaultText = @"קודם", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOProtest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Protest", DefaultText = "Protest",LocalDefaultText = @"אגב מחאה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOEnterAtLeastInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.EnterAtLeastInvoice", DefaultText = "Please enter at least one invoice and item",LocalDefaultText = @"יש להזין לפחות חשבון ספק ופרט מכס אחד", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCertificateMultiEntry = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CertificateMultiEntry", DefaultText = "Certificate Multi Entry",LocalDefaultText = @"הזנת אישורים מרוכזת", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODemandState = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DemandState", DefaultText = "Demand State",LocalDefaultText = @"סטטוס דרישה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOLevel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Level", DefaultText = "Level",LocalDefaultText = @"רמה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Invoice", DefaultText = "Invoice",LocalDefaultText = @"חשבון", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOWithResponse = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.WithResponse", DefaultText = "With response",LocalDefaultText = @"עם תגובה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOWithoutResponse = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.WithoutResponse", DefaultText = "Without response",LocalDefaultText = @"ללא מענה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationORequestedCerticate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.RequestedCerticate", DefaultText = "Requested Certicate",LocalDefaultText = @"נדרש", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODigital = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Digital", DefaultText = "Digital",LocalDefaultText = @"ממוחשב", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOExempt = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Exempt", DefaultText = "Exempt",LocalDefaultText = @"פטור", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOManual = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Manual", DefaultText = "Manual",LocalDefaultText = @"ידני", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCertificateResponse = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CertificateResponse", DefaultText = "Certificate Response for Demand",LocalDefaultText = @"הזנת מענה לדרישה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOTicketAlreadyExist = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.TicketAlreadyExist", DefaultText = "This Ticket already exist , do you want to move the items to the existing ticket ?",LocalDefaultText = @"המענה שהזנת כבר קיים , האם להעביר את פרטי המכס למענה הקיים ?", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCreate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Create", DefaultText = "Create",LocalDefaultText = @"יצירה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOMove = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Move", DefaultText = "Move",LocalDefaultText = @"לזוז ", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOMoreData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.MoreData", DefaultText = "More Data",LocalDefaultText = @"נוספים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOVehiclesModifications = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.VehiclesModifications", DefaultText = "Vehicles Modifications",LocalDefaultText = @"התאמות ברמת שילדה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOChassisNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ChassisNumber", DefaultText = "Chassis Number",LocalDefaultText = @"סנן לפי שילדה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOAdjustmentType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.AdjustmentType", DefaultText = "Adjustment Type",LocalDefaultText = @"סנן לפי התאמה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCertificateNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CertificateNumber", DefaultText = "Certificate Number:",LocalDefaultText = @"מספר אישור:", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsClientTHMoreData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Client.TH.MoreData", DefaultText = "MoreData",LocalDefaultText = @"נתונים נוספים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODeletingDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DeletingDetails", DefaultText = "All Certificate data will be deleted , continue?",LocalDefaultText = @"נתוני האישור ימחקו , להמשיך ?", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCancel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Cancel", DefaultText = "This supplier invoice has unsaved changes, do you want to save it?",LocalDefaultText = @"בחשבון ספק זה בוצעו שינויים שלא נשמרו, האם ברצונך לשמור אותם?  ", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOTooLongCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.TooLongCode", DefaultText = "Importer code is too long",LocalDefaultText = @"מספר יבואן ארוך מדי", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOImporterDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ImporterDetails", DefaultText = "Importer Details",LocalDefaultText = @"נתונים נוספים ליבואן", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOFillAgentObjection = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.FillAgentObjection", DefaultText = "You must fill objection",LocalDefaultText = @"יש למלא ערעור לתשובה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSendPaymentSucceeded = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SendPaymentSucceeded", DefaultText = "Send Payment Succeeded",LocalDefaultText = @"הגשת תשלום בוצעה בהצלחה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSendPaymentFailed = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SendPaymentFailed", DefaultText = "Send Payment Failed",LocalDefaultText = @"הגשת תשלום נכשלה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSendTransferRequestSucceeded = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SendTransferRequestSucceeded", DefaultText = "Send Transfer Request Succeeded",LocalDefaultText = @"העברה לגובה בוצעה בהצלחה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSendTransferRequestFailed = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SendTransferRequestFailed", DefaultText = "Send Transfer Request Failed",LocalDefaultText = @"העברה לגובה נכשלה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSendReTransferRequestSucceeded = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SendReTransferRequestSucceeded", DefaultText = "Send ReTransfer Request Succeeded",LocalDefaultText = @"העברה חוזרת לגובה בוצעה בהצלחה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSendReTransferRequestFailed = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SendReTransferRequestFailed", DefaultText = "Send ReTransfer Request Failed",LocalDefaultText = @"העברה חוזרת לגובה נכשלה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOServicereturnedanullresponse = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Servicereturnedanullresponse", DefaultText = "Service returned a null response",LocalDefaultText = @"התקבלה בסרוויס תשובה ריקה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOIsAccumulated = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.IsAccumulated", DefaultText = "Items are accumulated",LocalDefaultText = @"פרטי המכס בחשבון זה צבורים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOAccumulated = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Accumulated", DefaultText = "Accumulated",LocalDefaultText = @"צבור", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationONotAccumulated = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.NotAccumulated", DefaultText = "Not Accumulated",LocalDefaultText = @"לא צבור", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOOther = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Other", DefaultText = "Other",LocalDefaultText = @"אחר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOUpdateProcessCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.UpdateProcessCode", DefaultText = "Update Process Code",LocalDefaultText = @"עדכון קוד תהליך", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOProcessTypeRequired = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ProcessTypeRequired", DefaultText = "Process type is empty",LocalDefaultText = @"קוד תהליך שדה חובה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSelectItems = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SelectItems", DefaultText = "Please select items to update",LocalDefaultText = @"נא לבחור פריטים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOMultiProcessCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.MultiProcessCode", DefaultText = "This screen allows to multi update process code",LocalDefaultText = @"מסך זה מאפשר לעדכן את קוד התהליך באופן גורף לכל שורות פרטי המכס או לחלקן . אנא בחר בקוד התהליך ובפעולה הרצויה.", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOUpdateAll = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.UpdateAll", DefaultText = "Update all items",LocalDefaultText = @"עדכן את כל הפריטים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOUpdateSelected = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.UpdateSelected", DefaultText = "Update select item lines from ",LocalDefaultText = @" עדכן את הפריטים הנבחרים מ", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOfuturedatecantbepast = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.futuredatecantbepast", DefaultText = "The field future date can't be past date",LocalDefaultText = @"השדה תאריך עתידי לא יכול להיות תאריך עבר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSendDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SendDeclaration", DefaultText = "Send Declaration",LocalDefaultText = @"שלח הצהרה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Declaration", DefaultText = "Declaration",LocalDefaultText = @"הצהרה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOManifest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Manifest", DefaultText = "Manifest",LocalDefaultText = @"מצהר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOconsignmentShoudlnotSent = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.consignmentShoudlnotSent", DefaultText = "This field is for cases when consignment shoudln’t be sent has part of declaration message.",LocalDefaultText = @"שדה זה מיועד למצבים בהם נדרש לשדה את ההצהרה ללא נתוני המשגור", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOConsignmentwillnotsent = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Consignmentwillnotsent", DefaultText = "Consignment data will not be sent to customs",LocalDefaultText = @"נתוני משגור לא ישלחו במסר הצהרה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOMoveTo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.MoveTo", DefaultText = "Move to line",LocalDefaultText = @"עבור לשורה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOUpdateCommision = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.UpdateCommision", DefaultText = "Commision changed , update ?",LocalDefaultText = @"נתוני עמלה השתנו , האם לעדכן ?", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOUpdateCertificates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.UpdateCertificates", DefaultText = "Update certificates",LocalDefaultText = @"עדכן אישורים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOMultiCertificateUpdate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.MultiCertificateUpdate", DefaultText = "Multi Certificate Update",LocalDefaultText = @"עדכון אישורים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOSearchBy = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.SearchBy", DefaultText = "Search By",LocalDefaultText = @"חפש לפי", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOUpdateCertificateSubTitle = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.UpdateCertificateSubTitle", DefaultText = "Update Certificate",LocalDefaultText = @"עדכן אישור", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOMultiCertificateUpdateDescription = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.MultiCertificateUpdateDescription", DefaultText = "This screen allows to multi update certificates",LocalDefaultText = @"מסך זה מאפשר לעדכן את נתוני האישור לפי מספר הבקשה וקוד האישור", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOUpdate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Update", DefaultText = "Update",LocalDefaultText = @"עדכן", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCancelButton = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CancelButton", DefaultText = "Cancel",LocalDefaultText = @"ביטול", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationONomatchinglineswerefound = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Nomatchinglineswerefound", DefaultText = "No matching lines were found",LocalDefaultText = @"לא נמצאו שורות שתואמות לנתוני הבקשה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOitemswereupdated = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.itemswereupdated", DefaultText = "#Number items were updated",LocalDefaultText = @"עודכנו #Number שורות פרטי מכס", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOAllFieldsAreRequired = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.AllFieldsAreRequired", DefaultText = "All fields are required!",LocalDefaultText = @"כל השדות דרושים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODecCargoSplitCargoIdentifiers = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DecCargoSplitCargoIdentifiers", DefaultText = "Cargo Split Identifier",LocalDefaultText = @"מזהה מטען מפוצל", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCommissionChangedFromTo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CommissionChangedFromTo", DefaultText = "Commission changed, old value: #oldValue , new value: #newValue, change?",LocalDefaultText = @"#typeCode עודכן מערך קודם #oldValue לערך עדכני #newValue", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCargoSealsQueryFCargoIdentifierKey3Mandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoSealsQuery.F.CargoIdentifierKey3Mandatory", DefaultText = "Cargo IdentifierKey 3 field is mandatory",LocalDefaultText = @"מזהה מטען שלישי הוא שדה חובה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCargoSealsQueryFContainerNumberMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoSealsQuery.F.ContainerNumberMandatory", DefaultText = "Container Number field is mandatory",LocalDefaultText = @"מספר מכולה הוא שדה חובה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCargoSealsQueryFUpdateDateMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoSealsQuery.F.UpdateDateMandatory", DefaultText = "Update Date field is mandatory",LocalDefaultText = @"תאריך עדכון הוא שדה חובה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCargoSealsQueryFCargoSealItemsItemsMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoSealsQuery.F.CargoSealItemsItemsMandatory", DefaultText = "Seals is mandatory",LocalDefaultText = @"חובה להזין פרטי סגר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCargoSealsQueryFSealNumberMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoSealsQuery.F.SealNumberMandatory", DefaultText = "Seal Number field is mandatory",LocalDefaultText = @"מספר סגר הוא שדה חובה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCargoSealsQueryFSealTypeCodeMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoSealsQuery.F.SealTypeCodeMandatory", DefaultText = "Seal Type field is mandatory",LocalDefaultText = @"סוג הסגר הוא שדה חובה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCargoSealsQueryFSealCompletenessStateCodeMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoSealsQuery.F.SealCompletenessStateCodeMandatory", DefaultText = "Seal CompletenessState is mandatory",LocalDefaultText = @"מצב שלמות הסגר הוא שדה חובה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCargoSealsQueryFUpdateReasonCodeMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoSealsQuery.F.UpdateReasonCodeMandatory", DefaultText = "Update Reason field is mandatory",LocalDefaultText = @"סיבת עדכון הסגר הוא שדה חובה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCargoSealsQueryFUpdateTypeCodeMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoSealsQuery.F.UpdateTypeCodeMandatory", DefaultText = "Update Type field is mandatory",LocalDefaultText = @"סוג עדכון של הסגר הוא שדה חובה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOUpdateCountryOfOrigin = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.UpdateCountryOfOrigin", DefaultText = "Update Country of Origin",LocalDefaultText = @"עדכון ארץ מקור", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOMultiCountryOfOrigin = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.MultiCountryOfOrigin", DefaultText = "This screen allows to multi update Country of Origin",LocalDefaultText = @"מסך זה מאפשר לעדכן את ארץ המקור באופן גורף לכל שורות פרטי המכס או לחלקן . אנא בחר בקוד התהליך ובפעולה הרצויה.", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOItemsWithNoCountrOfOrigin = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ItemsWithNoCountrOfOrigin", DefaultText = "Update Items with no Country of Origin",LocalDefaultText = @"עדכן פריטים ללא ארץ מקור", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentsMetaDataDifference = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocuments.MetaDataDifference", DefaultText = "There is a difference between the meta data, do you wish to continue?",LocalDefaultText = @"קיים הבדלים בין נתוני המסמכים , האם לקשר בכל מקרה ?", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentsDisconnectConfirm = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocuments.DisconnectConfirm", DefaultText = "a document is connected to this ticket , delete anyway ?",LocalDefaultText = @"קיים מסמך מקושר , למחוק ?", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentsNotReady = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocuments.NotReady", DefaultText = "Not Ready",LocalDefaultText = @"לא מוכן", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentsReady = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocuments.Ready", DefaultText = "Ready",LocalDefaultText = @"מוכן", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentsDocumentStatusNameLabel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocuments.DocumentStatusNameLabel", DefaultText = "Status",LocalDefaultText = @"סטטוס מסמך", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentsCustomsDocIdLabel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocuments.CustomsDocIdLabel", DefaultText = "Customs Doc Id",LocalDefaultText = @"סימוכין", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentsExternalId = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocuments.ExternalId", DefaultText = "ExternalId",LocalDefaultText = @"מספרנו", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentsDocMetadataWarning = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocuments.DocMetadataWarning", DefaultText = "Irelavent Meta Data Values will be delete , continue ?",LocalDefaultText = @"נתוני מטה דאטה שאינם רלוונטים לסוג המסמך החדש ימחקו , להמשיך ?", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentExternalAttachmentIdLabel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocument.ExternalAttachmentIdLabel", DefaultText = "External Attachment",LocalDefaultText = @"מס פנימי", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentAddDocumentsTicket = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocument.AddDocumentsTicket", DefaultText = "Add Ticket",LocalDefaultText = @"הוסף מסמך חדש", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentAllTicketsLabel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocument.AllTicketsLabel", DefaultText = "All",LocalDefaultText = @"הכל", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentNotSentToCustoms = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocument.NotSentToCustoms", DefaultText = "Not Uploaded",LocalDefaultText = @"לא עלו למכס", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentUploadedToCustoms = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocument.UploadedToCustoms", DefaultText = "Uploaded",LocalDefaultText = @"עלו למכס", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentRequiredDocument = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocument.RequiredDocument", DefaultText = "Required Document",LocalDefaultText = @"מסמך נדרש", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentMetaDataMissing = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocument.MetaDataMissing", DefaultText = "Meta Data Missing",LocalDefaultText = @"מטה דאטה חסר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentSearchDocTypeWaterMark = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocument.SearchDocTypeWaterMark", DefaultText = "Search By Doc Type...",LocalDefaultText = @"בחר סוג מסמך", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentSupplierInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocument.SupplierInvoice", DefaultText = "Supplier Invoice",LocalDefaultText = @"חשבון ספק", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentSupplierInvoiceItem = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocument.SupplierInvoiceItem", DefaultText = "Supplier Invoice Item",LocalDefaultText = @"פרט מכס", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentConnectTo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocument.ConnectTo", DefaultText = "Connect To",LocalDefaultText = @"קשור ל", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentsCustomsDocNotSentYet = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocuments.CustomsDocNotSentYet", DefaultText = "Not send to customs yet",LocalDefaultText = @"טרם בוצעה שליחה למכס", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentsCustomsDocSendInProgress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocuments.CustomsDocSendInProgress", DefaultText = "Send in progress",LocalDefaultText = @"בתהליך שליחה למכס", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentsCustomsDocInVerificationProgress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocuments.CustomsDocInVerificationProgress", DefaultText = "In Verification Progress",LocalDefaultText = @"בתהליך אימות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsCustomsDocumentsNewVersionWarning = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsDocuments.NewVersionWarning", DefaultText = "Delete Customs reference and create a new version ?",LocalDefaultText = @" (מחיקת סימוכין המכס ויצירת גרסה חדשה ?)", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationTHMore = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.More", DefaultText = "More",LocalDefaultText = @"נוספים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsImporterDeclarationQueryODeclarationConectDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.DeclarationConect.Declaration", DefaultText = "Declaration",LocalDefaultText = @"הצהרה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationTHDeclarationCancellation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.DeclarationCancellation", DefaultText = "Declaration Cancellation",LocalDefaultText = @"ביטול הצהרה במכס", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationFExporterCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.F.ExporterCode", DefaultText = "Exporter Code",LocalDefaultText = @"מספר יצואן", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "F", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationFTransferExporterCode2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.F.TransferExporterCode2", DefaultText = "Transfer Exporter Code",LocalDefaultText = @"סוג יצואן מעביר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "F", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationFExporterTypeCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.F.ExporterTypeCode", DefaultText = "Exporter Type Code",LocalDefaultText = @"קוד סוג יצואן", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "F", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationFExporterPassportNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.F.ExporterPassportNumber", DefaultText = "Exporter Passport Number",LocalDefaultText = @"מס דרכון יצואן", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "F", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationFExporterCountryCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.F.ExporterCountryCode", DefaultText = "ImporterCountryCode",LocalDefaultText = @"מדינת דרכון יצואן", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "F", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationFExporterName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.F.ExporterName", DefaultText = "Exporter Name",LocalDefaultText = @"שם יצואן", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "F", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationFExporterAddress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.F.ExporterAddress", DefaultText = "Exporter Address",LocalDefaultText = @"כתובת יצואן", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "F", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationFTransferExporterCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.F.TransferExporterCode", DefaultText = "Transfer Exporter Code",LocalDefaultText = @"קוד סוג  יצואן מעביר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "F", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationFTransferExporterPassportNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.F.TransferExporterPassportNumber", DefaultText = "Transfer Exporter Passport Number",LocalDefaultText = @"מס תעודה יצואן מעביר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "F", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationFTransferExporterCountryCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.F.TransferExporterCountryCode", DefaultText = "Transfer Exporter Country Code",LocalDefaultText = @"מדינת דרכון יצואן מעביר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "F", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationFTransferExporterName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.F.TransferExporterName", DefaultText = "Transfer Exporter Name",LocalDefaultText = @"שם יצואן מעביר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "F", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationFTransferExporterAddress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.F.TransferExporterAddress", DefaultText = "Transfer Exporter Address",LocalDefaultText = @"כתובת יצואן מעביר", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "F", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOExporterDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ExporterDetails", DefaultText = "Exporter Details",LocalDefaultText = @"נתונים נוספים ליצואן", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOIsExporterConfirmation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.IsExporterConfirmation", DefaultText = "שדה זה מיועד לאפשר סימון במידה והיצואן מאשר להעביר את נתוני ההצהרה למדינת היעד",LocalDefaultText = @"שדה זה מיועד לאפשר סימון במידה והיצואן מאשר להעביר את נתוני ההצהרה למדינת היעד", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationODeleteExportRecipient = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DeleteExportRecipient", DefaultText = "Delete Recipient?",LocalDefaultText = @"למחוק פרטי מקבל?", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationTHExporterInvoices = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.ExporterInvoices", DefaultText = "Exporter Invoices",LocalDefaultText = @"חשבונות יצואן", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOExporterInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ExporterInvoice", DefaultText = "Exporter Invoice",LocalDefaultText = @"חשבון יצואן", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOModificationsExport = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ModificationsExport", DefaultText = "Modifications",LocalDefaultText = @"הפחתות / התאמות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOUCR = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.UCR", DefaultText = "UCR",LocalDefaultText = @"זיהוי מטען", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOPaymentDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.PaymentDetails", DefaultText = "Payment Details",LocalDefaultText = @"פרטי תשלום", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOPrices = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Prices", DefaultText = "Prices",LocalDefaultText = @"מחירים", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOAbachStatement = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.AbachStatement", DefaultText = "Abach Declaration",LocalDefaultText = @"הצהרות אב'כ", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOConnectedDeclarations = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ConnectedDeclarations", DefaultText = "Connected Declarations",LocalDefaultText = @"הצהרות מקושרות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOAllCourierDeclarations = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.AllCourierDeclarations", DefaultText = "All Courier Declarations ",LocalDefaultText = @"כל ההצהרות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOErrors = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Errors", DefaultText = "Errors",LocalDefaultText = @"שגיאות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOReferences = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.References", DefaultText = "References",LocalDefaultText = @"אסמכתאות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOReferenceType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ReferenceType", DefaultText = "Reference Type",LocalDefaultText = @"סוג אסמכתא", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationORefernceStatus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.RefernceStatus", DefaultText = "Refernce Status",LocalDefaultText = @"סטטוס אסמכתא", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationORefernceID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.RefernceID", DefaultText = "RefernceID",LocalDefaultText = @"מזהה אסמכתא", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationORemarks = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Remarks", DefaultText = "Remarks",LocalDefaultText = @"הערות", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationORefernceInputType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.RefernceInputType", DefaultText = "Refernce Input Type",LocalDefaultText = @"תהליך הזנת אסמכתא", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationsOUnSavedRemarks = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declarations.O.UnSavedRemarks", DefaultText = "Remarks wont be saved",LocalDefaultText = @"ביציאה מהמסך לא ישמרו הערות לחשבונית שהוזנו במסך", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOAmendmentFieldStatus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.AmendmentFieldStatus", DefaultText = "Status",LocalDefaultText = @"סטטוס", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOAmendmentRequestInitiatorType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.AmendmentRequestInitiatorType", DefaultText = "Amendment Request Initiator Type",LocalDefaultText = @"יוזם התיקון", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOFieldAmendmentRejectReasonRemarks = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.FieldAmendmentRejectReasonRemarks", DefaultText = "Amendment Reject Reason Remarks",LocalDefaultText = @"סיבת דחיה", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOExport = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Export", DefaultText = "Export Declaration",LocalDefaultText = @"הצהרת יצוא", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationTextCode_CustomsDeclarationOCargoTypeCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CargoTypeCode", DefaultText = "Cargo Type Code",LocalDefaultText = @"סוג מזהה מטען", ObjectTableId = DeclarationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 