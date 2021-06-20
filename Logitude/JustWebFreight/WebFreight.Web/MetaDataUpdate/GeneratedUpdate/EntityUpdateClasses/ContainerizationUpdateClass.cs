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
   public class ContainerizationUpdateClass
   {  		
		public const string HashString = "55a72db9c271fb28694b84448e8c6bb6";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Customs.Containerization",
			      				    IsNew =  true,
			      				    DBTableName =  "Customs.Containerizations",
			      				    ObjectTableSingular =  "Containerization",
			      				    ObjectTablePlural =  "Containerizations",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
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
			      				    SortingByObjectField =  "Id",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "Logitude.Customs.NewContainerizationControlCommand",
			      				    LocalDefaultText =  "המכלה",
			      				    DefaultText =  "Containerization",
			      				    Code =  "4ba2",
			      				    Name =  " Query Group",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Customs",
			      				    ServerModuleName =  "Customs",
			      				    NewWizardComponentPath =  "./CustomsModules/CustomsContainerization/Components/NewEntity/NewContainerizationComponent",
			      				    NoTS =  false,
			      				    HasMenuButtons =  true,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    HashString =  ContainerizationUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "Customs.Containerization",
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
					  						ValidForQuerySection1 =  "Containerization",
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
					  						FullLocalDefaultText =  "תיק יצוא / מזהה מטען",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "Search ...",
					  						ShortFieldLable =  "SearchFields",
					  						ShortFieldLableDefaultText =  "תיק יצוא / מזהה מטען",
					  						ShortLocalDefaultText =  "תיק יצוא / מזהה מטען",
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
					 
					 						FieldName =  "AgentDeclaration",
					  						ObjectTableName =  "Customs.Containerization",
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
					  						PMPropertyPath =  "AgentDeclaration",
					  						ListPropertyPath =  "AgentDeclaration",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Containerization",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AgentDeclaration",
					  						DefaultText =  "Agent Declaration",
					  						FullLocalDefaultText =  "הצהרת סוכן",
					  						ListFieldLable =  "AgentDeclarationListLable",
					  						ListLableDefaultText =  "Agent Declaration",
					  						ListLocalDefaultText =  "הצהרת סוכן",
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
					 
					 						FieldName =  "ContainerizationDate",
					  						ObjectTableName =  "Customs.Containerization",
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
					  						PMPropertyPath =  "ContainerizationDate",
					  						ListPropertyPath =  "ContainerizationDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Containerization",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContainerizationDate",
					  						DefaultText =  "Containerization Date",
					  						FullLocalDefaultText =  "תאריך המכלה",
					  						ListFieldLable =  "ContainerizationDateListLable",
					  						ListLableDefaultText =  "Containerization Date",
					  						ListLocalDefaultText =  "תאריך המכלה",
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
					 
					 						FieldName =  "ContainerizationNumber",
					  						ObjectTableName =  "Customs.Containerization",
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
					  						PMPropertyPath =  "ContainerizationNumber",
					  						ListPropertyPath =  "ContainerizationNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Containerization",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContainerizationNumber",
					  						DefaultText =  "Containerization Number",
					  						FullLocalDefaultText =  "מספר המכלה",
					  						ListFieldLable =  "ContainerizationNumberListLable",
					  						ListLableDefaultText =  "Containerization Number",
					  						ListLocalDefaultText =  "מספר המכלה",
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
					 
					 						FieldName =  "ContainerizationStatus",
					  						ObjectTableName =  "Customs.Containerization",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.ContainerizationStatusCode",
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
					  						PMPropertyPath =  "ContainerizationStatus",
					  						ListPropertyPath =  "ContainerizationStatus",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Containerization",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  3,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContainerizationStatus",
					  						DefaultText =  "Containerization Status",
					  						FullLocalDefaultText =  "קוד סטטוס המכלה",
					  						ListFieldLable =  "ContainerizationStatusListLable",
					  						ListLableDefaultText =  "Containerization Status",
					  						ListLocalDefaultText =  "קוד סטטוס המכלה",
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
					 
					 						FieldName =  "HataraStatus",
					  						ObjectTableName =  "Customs.Containerization",
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
					  						PMPropertyPath =  "HataraStatus",
					  						ListPropertyPath =  "HataraStatus",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Containerization",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  3,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "HataraStatus",
					  						DefaultText =  "Hatara Status",
					  						FullLocalDefaultText =  "קוד סטטוס התרה",
					  						ListFieldLable =  "HataraStatusListLable",
					  						ListLableDefaultText =  "Hatara Status",
					  						ListLocalDefaultText =  "קוד סטטוס התרה",
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
					 
					 						FieldName =  "OperationMode",
					  						ObjectTableName =  "Customs.Containerization",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.NDMessageActionCode",
					  						MinLength =  0,
					  						MaxLength =  2,
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
					  						PMPropertyPath =  "OperationMode",
					  						ListPropertyPath =  "OperationMode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Containerization",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OperationMode",
					  						DefaultText =  "Operation Mode",
					  						ListFieldLable =  "OperationModeListLable",
					  						ListLableDefaultText =  "Operation Mode",
					  						ListLocalDefaultText =  "קוד פעולה",
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
					  						ObjectTableName =  "Customs.Containerization",
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
					  						ValidForQuerySection1 =  "Customs.Containerization",
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
					 
					 						FieldName =  "TransportModeForExport",
					  						ObjectTableName =  "Customs.Containerization",
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
					  						ValidForQuerySection1 =  "Customs.Containerization",
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
					  						FullLocalDefaultText =  "סוג משלוח",
					  						ListFieldLable =  "TransportModeForExportListLable",
					  						ListLableDefaultText =  "Transport Mode",
					  						ListLocalDefaultText =  "סוג משלוח",
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
					 
					 						FieldName =  "ImporterName",
					  						ObjectTableName =  "Customs.Containerization",
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
					  						Operator =  "Contains",
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
					  						ValidForQuerySection1 =  "Customs.Containerization",
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
					 
					 						FieldName =  "ContainerizationStatusName",
					  						ObjectTableName =  "Customs.Containerization",
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
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ContainerizationStatusName",
					  						ListPropertyPath =  "ContainerizationStatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Containerization",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContainerizationStatusName",
					  						DefaultText =  "Containerization Status",
					  						FullLocalDefaultText =  "סטטוס המכלה",
					  						ListFieldLable =  "ContainerizationStatusNameListLable",
					  						ListLableDefaultText =  "Containerization Status",
					  						ListLocalDefaultText =  "סטטוס המכלה",
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
					 
					 						FieldName =  "HataraStatusName",
					  						ObjectTableName =  "Customs.Containerization",
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
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "HataraStatusName",
					  						ListPropertyPath =  "HataraStatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Containerization",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "HataraStatusName",
					  						DefaultText =  "Hatara Status",
					  						FullLocalDefaultText =  "סטטוס התרה",
					  						ListFieldLable =  "HataraStatusNameListLable",
					  						ListLableDefaultText =  "Hatara Status",
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
					 
					 						FieldName =  "HataraStatusIsNull",
					  						ObjectTableName =  "Customs.Containerization",
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
					  						PMPropertyPath =  "HataraStatusIsNull",
					  						ListPropertyPath =  "HataraStatusIsNull",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Containerization",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "HataraStatusIsNull",
					  						DefaultText =  "HataraStatusIsNull",
					  						FullLocalDefaultText =  "התקבלה התרה",
					  						ListFieldLable =  "HataraStatusIsNullListLable",
					  						ListLableDefaultText =  "HataraStatusIsNull",
					  						ListLocalDefaultText =  "התקבלה התרה",
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
					 
					 						FieldName =  "ConnectedDeclarations",
					  						ObjectTableName =  "Customs.Containerization",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  5000,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  5000,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ConnectedDeclarations",
					  						ListPropertyPath =  "ConnectedDeclarations",
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
					  						FullFieldLable =  "ConnectedDeclarations",
					  						DefaultText =  "ConnectedDeclarations",
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
					 
					 						FieldName =  "NotConnectedDeclarations",
					  						ObjectTableName =  "Customs.Containerization",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  5000,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  5000,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "NotConnectedDeclarations",
					  						ListPropertyPath =  "NotConnectedDeclarations",
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
					  						FullFieldLable =  "NotConnectedDeclarations",
					  						DefaultText =  "NotConnectedDeclarations",
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
					 
					 						FieldName =  "IsChange",
					  						ObjectTableName =  "Customs.Containerization",
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
					  						PMPropertyPath =  "IsChange",
					  						ListPropertyPath =  "IsChange",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Containerization",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsChange",
					  						DefaultText =  "Is Change",
					  						FullLocalDefaultText =  "האם יש שינוי בהמכלה",
					  						ListFieldLable =  "IsChangeListLable",
					  						ListLableDefaultText =  "Is Change",
					  						ListLocalDefaultText =  "האם יש שינוי בהמכלה",
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
	        QueryGroup ContainerizationQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "4ba2", Name = " Query Group" }, queryGroupRepository,tenantQueryGroups);
						QueryGroup ContainerizationQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "73f5", Name = " Query Group" }, queryGroupRepository,tenantQueryGroups);
				        queryGroupRepository.SubmitChanges();
	        ObjectTable ContainerizationObjectTable = objectTables.ContainsKey("Customs.Containerization") ? objectTables["Customs.Containerization"] : null;
            if (ContainerizationObjectTable == null)
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(0);  

                ContainerizationObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Customs.Containerization" && d.Tenant == 0).FirstOrDefault();
            }

	         
			List<Feature> addedFeatures = new List<Feature>();
			List<TextCode> addedTextCodes = new List<TextCode>();
			List<Query> addedQueries = new List<Query>();
			List<QueryColumn> addedQueryColumns = new List<QueryColumn>();
			List<AdvancedQueryFilter> addedQueryFilters = new List<AdvancedQueryFilter>();
   

			   TextCode ContainerizationTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Containerization.Q.OpenContainerization", DefaultText = @"Open Containerization",LocalDefaultText = "המכלות פתוחות", ObjectTableId = ContainerizationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature ContainerizationFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Containerization.Q.OpenContainerization", ObjectTableId = ContainerizationObjectTable.Id, Tenant = 0, NameTextCodeCode = "ContainerizationFeatures.OpenContainerization", NameTextCodeDefaultText = "OpenContainerization", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,ContainerizationObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode ContainerizationTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Containerization.Q.ContainerizationWithRelease", DefaultText = @"Containerization With Release",LocalDefaultText = "המכלות שהותרו", ObjectTableId = ContainerizationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature ContainerizationFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Containerization.Q.ContainerizationWithRelease", ObjectTableId = ContainerizationObjectTable.Id, Tenant = 0, NameTextCodeCode = "ContainerizationFeatures.ContainerizationWithRelease", NameTextCodeDefaultText = "ContainerizationWithRelease", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,ContainerizationObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode ContainerizationTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Containerization.Q.CancelledContainerization", DefaultText = @"Cancelled Containerization",LocalDefaultText = "המכלות מבוטלות", ObjectTableId = ContainerizationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature ContainerizationFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Containerization.Q.CancelledContainerization", ObjectTableId = ContainerizationObjectTable.Id, Tenant = 0, NameTextCodeCode = "ContainerizationFeatures.CancelledContainerization", NameTextCodeDefaultText = "CancelledContainerization", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,ContainerizationObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode ContainerizationTextCode_3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Containerization.Q.AllContainerization", DefaultText = @"All Containerization",LocalDefaultText = "כל ההמכלות", ObjectTableId = ContainerizationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature ContainerizationFeature_3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Containerization.Q.AllContainerization", ObjectTableId = ContainerizationObjectTable.Id, Tenant = 0, NameTextCodeCode = "ContainerizationFeatures.AllContainerization", NameTextCodeDefaultText = "AllContainerization", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,ContainerizationObjectTable, addedFeatures, addedTextCodes);

	        //TextCodeRepository.SubmitChanges();
	        //FeaturesRepository.SubmitChanges();    
	      

			  Query OpenContainerizationQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ContainerizationTextCode_0.Id, NameTextCodeCode = ContainerizationTextCode_0.Code, ObjectTableName = "Customs.Containerization", Code = "OpenContainerization",  QueryGroupCode = "4ba2", IndexOrder = 0, Tenant = 0, ObjectTableId = ContainerizationObjectTable.Id, QuerySection = "Customs.Containerization", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = ContainerizationFeature_0.Id,FeatureUniqeCode= ContainerizationFeature_0.FeatureUniqeCode, DefaultSortName = "ContainerizationDate", DefaultSortDirection = "Desending", Perspective = null }, addedQueries);
	
			 QueryColumn OpenContainerizationQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenContainerizationQuery.Id,QueryCode = OpenContainerizationQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.Containerization.ContainerizationDate" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn OpenContainerizationQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenContainerizationQuery.Id,QueryCode = OpenContainerizationQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.Containerization.ContainerizationNumber" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn OpenContainerizationQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenContainerizationQuery.Id,QueryCode = OpenContainerizationQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.Containerization.ExportFile" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn OpenContainerizationQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenContainerizationQuery.Id,QueryCode = OpenContainerizationQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.Containerization.TransportModeForExport" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn OpenContainerizationQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenContainerizationQuery.Id,QueryCode = OpenContainerizationQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.Containerization.ImporterName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn OpenContainerizationQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenContainerizationQuery.Id,QueryCode = OpenContainerizationQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.Containerization.ContainerizationStatusName" , ColumnWidth = 200 }, addedQueryColumns);

			 QueryColumn OpenContainerizationQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenContainerizationQuery.Id,QueryCode = OpenContainerizationQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.Containerization.HataraStatusName" , ColumnWidth = 100 }, addedQueryColumns);

             AdvancedQueryFilter OpenContainerizationQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Containerization.ContainerizationStatus", PredefinedValue = "3",PredefinedValue2 = null, QueryId = OpenContainerizationQuery.Id,QueryCode = OpenContainerizationQuery.UniqueCode, Tenant = 0,Operator = "NotEqual"}, addedQueryFilters);


             AdvancedQueryFilter OpenContainerizationQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Containerization.HataraStatusIsNull", PredefinedValue = "true",PredefinedValue2 = null, QueryId = OpenContainerizationQuery.Id,QueryCode = OpenContainerizationQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query ContainerizationWithReleaseQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ContainerizationTextCode_1.Id, NameTextCodeCode = ContainerizationTextCode_1.Code, ObjectTableName = "Customs.Containerization", Code = "ContainerizationWithRelease",  QueryGroupCode = "4ba2", IndexOrder = 1, Tenant = 0, ObjectTableId = ContainerizationObjectTable.Id, QuerySection = "Customs.Containerization", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = ContainerizationFeature_1.Id,FeatureUniqeCode= ContainerizationFeature_1.FeatureUniqeCode, DefaultSortName = "ContainerizationDate", DefaultSortDirection = "Desending", Perspective = null }, addedQueries);
	
			 QueryColumn ContainerizationWithReleaseQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ContainerizationWithReleaseQuery.Id,QueryCode = ContainerizationWithReleaseQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.Containerization.ContainerizationDate" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ContainerizationWithReleaseQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ContainerizationWithReleaseQuery.Id,QueryCode = ContainerizationWithReleaseQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.Containerization.ContainerizationNumber" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn ContainerizationWithReleaseQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ContainerizationWithReleaseQuery.Id,QueryCode = ContainerizationWithReleaseQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.Containerization.ExportFile" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn ContainerizationWithReleaseQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ContainerizationWithReleaseQuery.Id,QueryCode = ContainerizationWithReleaseQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.Containerization.TransportModeForExport" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn ContainerizationWithReleaseQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ContainerizationWithReleaseQuery.Id,QueryCode = ContainerizationWithReleaseQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.Containerization.ImporterName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn ContainerizationWithReleaseQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ContainerizationWithReleaseQuery.Id,QueryCode = ContainerizationWithReleaseQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.Containerization.ContainerizationStatusName" , ColumnWidth = 200 }, addedQueryColumns);

			 QueryColumn ContainerizationWithReleaseQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ContainerizationWithReleaseQuery.Id,QueryCode = ContainerizationWithReleaseQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.Containerization.HataraStatusName" , ColumnWidth = 100 }, addedQueryColumns);

             AdvancedQueryFilter ContainerizationWithReleaseQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Containerization.HataraStatus", PredefinedValue = "1",PredefinedValue2 = null, QueryId = ContainerizationWithReleaseQuery.Id,QueryCode = ContainerizationWithReleaseQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query CancelledContainerizationQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ContainerizationTextCode_2.Id, NameTextCodeCode = ContainerizationTextCode_2.Code, ObjectTableName = "Customs.Containerization", Code = "CancelledContainerization",  QueryGroupCode = "4ba2", IndexOrder = 2, Tenant = 0, ObjectTableId = ContainerizationObjectTable.Id, QuerySection = "Customs.Containerization", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = ContainerizationFeature_2.Id,FeatureUniqeCode= ContainerizationFeature_2.FeatureUniqeCode, DefaultSortName = "ContainerizationDate", DefaultSortDirection = "Desending", Perspective = null }, addedQueries);
	
			 QueryColumn CancelledContainerizationQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledContainerizationQuery.Id,QueryCode = CancelledContainerizationQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.Containerization.ContainerizationDate" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn CancelledContainerizationQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledContainerizationQuery.Id,QueryCode = CancelledContainerizationQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.Containerization.ContainerizationNumber" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn CancelledContainerizationQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledContainerizationQuery.Id,QueryCode = CancelledContainerizationQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.Containerization.ExportFile" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn CancelledContainerizationQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledContainerizationQuery.Id,QueryCode = CancelledContainerizationQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.Containerization.TransportModeForExport" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn CancelledContainerizationQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledContainerizationQuery.Id,QueryCode = CancelledContainerizationQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.Containerization.ImporterName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn CancelledContainerizationQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledContainerizationQuery.Id,QueryCode = CancelledContainerizationQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.Containerization.ContainerizationStatusName" , ColumnWidth = 200 }, addedQueryColumns);

			 QueryColumn CancelledContainerizationQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledContainerizationQuery.Id,QueryCode = CancelledContainerizationQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.Containerization.HataraStatusName" , ColumnWidth = 100 }, addedQueryColumns);

             AdvancedQueryFilter CancelledContainerizationQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.Containerization.ContainerizationStatus", PredefinedValue = "3",PredefinedValue2 = null, QueryId = CancelledContainerizationQuery.Id,QueryCode = CancelledContainerizationQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query AllContainerizationQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ContainerizationTextCode_3.Id, NameTextCodeCode = ContainerizationTextCode_3.Code, ObjectTableName = "Customs.Containerization", Code = "AllContainerization",  QueryGroupCode = "4ba2", IndexOrder = 3, Tenant = 0, ObjectTableId = ContainerizationObjectTable.Id, QuerySection = "Customs.Containerization", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = ContainerizationFeature_3.Id,FeatureUniqeCode= ContainerizationFeature_3.FeatureUniqeCode, DefaultSortName = "ContainerizationDate", DefaultSortDirection = "Desending", Perspective = null }, addedQueries);
	
			 QueryColumn AllContainerizationQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllContainerizationQuery.Id,QueryCode = AllContainerizationQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.Containerization.ContainerizationDate" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AllContainerizationQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllContainerizationQuery.Id,QueryCode = AllContainerizationQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.Containerization.ContainerizationNumber" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn AllContainerizationQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllContainerizationQuery.Id,QueryCode = AllContainerizationQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.Containerization.ExportFile" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn AllContainerizationQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllContainerizationQuery.Id,QueryCode = AllContainerizationQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.Containerization.TransportModeForExport" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn AllContainerizationQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllContainerizationQuery.Id,QueryCode = AllContainerizationQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.Containerization.ImporterName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn AllContainerizationQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllContainerizationQuery.Id,QueryCode = AllContainerizationQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.Containerization.ContainerizationStatusName" , ColumnWidth = 200 }, addedQueryColumns);

			 QueryColumn AllContainerizationQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllContainerizationQuery.Id,QueryCode = AllContainerizationQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.Containerization.HataraStatusName" , ColumnWidth = 100 }, addedQueryColumns);
			SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
			SqlBulkInsert.BulkInsert("Features", addedFeatures);
			SqlBulkInsert.BulkInsert("Queries", addedQueries);
			SqlBulkInsert.BulkInsert("QueryColumns", addedQueryColumns);
			SqlBulkInsert.BulkInsert("AdvancedQueryFilters", addedQueryFilters);	 
  
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable ContainerizationObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.Containerization" && d.Tenant == 0).FirstOrDefault();
		   //List<ObjectField> ContainerizationObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.Containerization").ToList();
		       
	      

	         Screen ContainerizationCustomsContainerizationHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customs.Containerization.HeaderScreen", Name = "Customs.ContainerizationHeaderScreen", ObjectTableId = ContainerizationObjectTable.Id, NumberOfColumns = 3, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
             ScreenField CustomsContainerizationCustomsContainerizationHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ScreenId = ContainerizationCustomsContainerizationHeaderScreenScreen0.Id,ScreenCode = ContainerizationCustomsContainerizationHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.Containerization.ContainerizationNumber", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsContainerizationCustomsContainerizationHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ScreenId = ContainerizationCustomsContainerizationHeaderScreenScreen0.Id,ScreenCode = ContainerizationCustomsContainerizationHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.Containerization.ImporterName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsContainerizationCustomsContainerizationHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ScreenId = ContainerizationCustomsContainerizationHeaderScreenScreen0.Id,ScreenCode = ContainerizationCustomsContainerizationHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.Containerization.ContainerizationDate", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsContainerizationCustomsContainerizationHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ScreenId = ContainerizationCustomsContainerizationHeaderScreenScreen0.Id,ScreenCode = ContainerizationCustomsContainerizationHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.Containerization.ContainerizationStatusName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsContainerizationCustomsContainerizationHeaderScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ScreenId = ContainerizationCustomsContainerizationHeaderScreenScreen0.Id,ScreenCode = ContainerizationCustomsContainerizationHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.Containerization.HataraStatusName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          	
		    ContainerizationObjectTable.HeaderScreenId = ContainerizationCustomsContainerizationHeaderScreenScreen0.Id;
		    ContainerizationObjectTable.HeaderScreenCode = ContainerizationCustomsContainerizationHeaderScreenScreen0.Code;

	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable ContainerizationObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.Containerization" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode ContainerizationGeneralDetailsTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Containerization.TH.GeneralDetails", DefaultText = "General Details",LocalDefaultText = "פרטים כללים", ObjectTableId = ContainerizationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ContainerizationGeneralDetailsFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Containerization.Tab.GeneralDetails", ObjectTableId = ContainerizationObjectTable.Id, Tenant = 0, NameTextCodeCode = "ContainerizationFeatures.CNGN", NameTextCodeDefaultText = "General Details", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,ContainerizationObjectTable);
 
                 
			   TextCode ContainerizationRequestSheetTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Containerization.TH.RequestSheet", DefaultText = "Request Sheet",LocalDefaultText = "גיליון בקשה", ObjectTableId = ContainerizationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ContainerizationRequestSheetFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Containerization.Tab.RequestSheet", ObjectTableId = ContainerizationObjectTable.Id, Tenant = 0, NameTextCodeCode = "ContainerizationFeatures.CNRS", NameTextCodeDefaultText = "Request Sheet", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,ContainerizationObjectTable);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CNGN",HtmlComponentName = "ContainerizationGeneralComponent",HtmlComponentUrl = "./CustomsModules/CustomsContainerization/Components/EditTabs/ContainerizationGeneralComponent", FeatureId = ContainerizationGeneralDetailsFeature_TH0.Id,FeatureUniqeCode = ContainerizationGeneralDetailsFeature_TH0.FeatureUniqeCode, ControlPath = "", ObjectTableId = ContainerizationObjectTable.Id, TabNameTextCodeId = ContainerizationGeneralDetailsTextCode_TH0.Id, TabNameTextCodeCode = ContainerizationGeneralDetailsTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CNRS",HtmlComponentName = "RequestSheetTabComponent",HtmlComponentUrl = "./CustomsModules/CustomsControls/Components/CustomsRequestsSheetsComponent", FeatureId = ContainerizationRequestSheetFeature_TH1.Id,FeatureUniqeCode = ContainerizationRequestSheetFeature_TH1.FeatureUniqeCode, ControlPath = "", ObjectTableId = ContainerizationObjectTable.Id, TabNameTextCodeId = ContainerizationRequestSheetTextCode_TH1.Id, TabNameTextCodeCode = ContainerizationRequestSheetTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable ContainerizationObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.Containerization" && d.Tenant == 0).FirstOrDefault(); 

		   Feature ContainerizationFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = ContainerizationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Containerization.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ContainerizationObjectTable);
		   Feature ContainerizationFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = ContainerizationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Containerization.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ContainerizationObjectTable);
		   Feature ContainerizationFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = ContainerizationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Containerization.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ContainerizationObjectTable);
		   Feature ContainerizationFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = false, ObjectTableId = ContainerizationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Containerization.Features.PackageFeature", NameTextCodeDefaultText = "Containerization Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ContainerizationObjectTable);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable ContainerizationObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.Containerization" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = ContainerizationObjectTable.Id,
				 
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
                ObjectTableId = ContainerizationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   //FeatureRepository featureRepository = new FeatureRepository(0); 
		   //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable ContainerizationObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.Containerization" && d.Tenant == 0).FirstOrDefault(); 			   Feature ContainerizationFeature_MB00 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AddDeclaration", ObjectTableId = ContainerizationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Containerization.Features.AddDeclaration", NameTextCodeDefaultText = "Add Declaration", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,ContainerizationObjectTable);
             			   Feature ContainerizationFeature_MB01 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CancelContainerization", ObjectTableId = ContainerizationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Containerization.Features.CancelContainerization", NameTextCodeDefaultText = "Cancel Containerization", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,ContainerizationObjectTable);
             			   Feature ContainerizationFeature_MB02 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DeclarationsStatusRequest", ObjectTableId = ContainerizationObjectTable.Id, Tenant = 0, NameTextCodeCode = "Containerization.Features.DeclarationsStatusRequest", NameTextCodeDefaultText = "Declarations Status Request", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,ContainerizationObjectTable);
              

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup ContainerizationMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "Customs.ContainerizationEdit",
					Name = "Customs.ContainerizationEditButtonsGroup",
					ObjectTableId = ContainerizationObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton ContainerizationMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Actions",
						Index = 0, 
						IsActive = false,
						LabelTextCodeCode = "Containerization.B.Actions",
						LabelTextCodeDefaultText = "Actions",
						Tenant = 0,
						MenuButtonGroupId = ContainerizationMenuButtonGroup.Id,
						ObjectTableId = ContainerizationObjectTable.Id,
						MenuButtonType = "dropdownbutton",
						FeatureId = null,
						Style = null,
						LocalDefaultText = "פעולות",
						FeatureUniqeCode = null,
						HtmlComponentPath = null,
						Width = 0,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

			   MenuButton ContainerizationMenuButton00 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "AddDeclaration",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "Containerization.B.AddDeclaration",
						LabelTextCodeDefaultText = "Add Declaration",
						Tenant = 0,
						MenuButtonGroupId = ContainerizationMenuButtonGroup.Id,
						ParentMenuButtonId = ContainerizationMenuButton0.Id,
						ObjectTableId = ContainerizationObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  ContainerizationFeature_MB00.Id,
						Style = null,
						LocalDefaultText = "הוסף הצהרה",
                        HtmlComponentPath="./CustomsModules/CustomsContainerization/Components/NewEntity/NewContainerizationComponent",
                        Width=0,
						FeatureUniqeCode=  ContainerizationFeature_MB00.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton ContainerizationMenuButton01 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CancelContainerization",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "Containerization.B.CancelContainerization",
						LabelTextCodeDefaultText = "Cancel Containerization",
						Tenant = 0,
						MenuButtonGroupId = ContainerizationMenuButtonGroup.Id,
						ParentMenuButtonId = ContainerizationMenuButton0.Id,
						ObjectTableId = ContainerizationObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  ContainerizationFeature_MB01.Id,
						Style = null,
						LocalDefaultText = "ביטול המכלה",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  ContainerizationFeature_MB01.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton ContainerizationMenuButton02 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "DeclarationsStatusRequest",
						Index = 2, 
						IsActive = true,
						LabelTextCodeCode = "Containerization.B.DeclarationsStatusRequest",
						LabelTextCodeDefaultText = "Declarations Status Request",
						Tenant = 0,
						MenuButtonGroupId = ContainerizationMenuButtonGroup.Id,
						ParentMenuButtonId = ContainerizationMenuButton0.Id,
						ObjectTableId = ContainerizationObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  ContainerizationFeature_MB02.Id,
						Style = null,
						LocalDefaultText = "סטטוס הצהרות בהמכלה",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  ContainerizationFeature_MB02.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {     
	    
}

    

   }
    
}
	 