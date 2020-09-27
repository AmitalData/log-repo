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
   public class AnalyzeQueueUpdateClass
   {  		
		public const string HashString = "ebcfa31b402186424ded02d5e7ba10ca";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "AnalyzeQueue",
			      				    IsNew =  false,
			      				    DBTableName =  "AnalyzeQueues",
			      				    ObjectTableSingular =  "Analyze Queue",
			      				    ObjectTablePlural =  "AnalyzeQueues",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  true,
			      				    IsNewWizard =  false,
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
			      				    SortingByObjectField =  "CreateDate",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "Analyze Queue",
			      				    Code =  "ANQU",
			      				    Name =  "Analyze Queues",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Infrastructure",
			      				    NoTS =  false,
			      				    HasMenuButtons =  true,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    SearchFields =  "AnalyzeQueue,AnalyzeQueues,,,",
			      				    HashString =  AnalyzeQueueUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Id",
					  						ObjectTableName =  "AnalyzeQueue",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Id",
					  						ListPropertyPath =  "Id",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AnalyzeQueue",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Id",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Id",
					  						DefaultText =  "ID",
					  						ListFieldLable =  "IdListLable",
					  						ListLableDefaultText =  "ID",
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
					  						HelpTextCode =  "Id",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CommunicationLogId",
					  						ObjectTableName =  "AnalyzeQueue",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "CommunicationLog",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CommunicationLog",
					  						ListPropertyPath =  "CommunicationLog",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AnalyzeQueue",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "CommunicationLogId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CommunicationLogId",
					  						DefaultText =  "Communication Log",
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
					  						HelpTextCode =  "CommunicationLogId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EntityReference",
					  						ObjectTableName =  "AnalyzeQueue",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "EntityReference",
					  						ListPropertyPath =  "EntityReference",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AnalyzeQueue",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "EntityReference",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EntityReference",
					  						DefaultText =  "EntityReference",
					  						ListFieldLable =  "EntityReferenceListLable",
					  						ListLableDefaultText =  "Entity Reference",
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
					  						HelpTextCode =  "EntityReference",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ObjectTableName",
					  						ObjectTableName =  "AnalyzeQueue",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  25,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ObjectTableName",
					  						ListPropertyPath =  "ObjectTableName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AnalyzeQueue",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ObjectTableName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ObjectTableName",
					  						DefaultText =  "Object Table Name",
					  						ListFieldLable =  "ObjectTableNameListLable",
					  						ListLableDefaultText =  "ObjectTableName",
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
					  						HelpTextCode =  "ObjectTableName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "From",
					  						ObjectTableName =  "AnalyzeQueue",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "From",
					  						ListPropertyPath =  "From",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AnalyzeQueue",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "From",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "From",
					  						DefaultText =  "From",
					  						ListFieldLable =  "FromListLable",
					  						ListLableDefaultText =  "From",
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
					  						HelpTextCode =  "From",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreateDate",
					  						ObjectTableName =  "AnalyzeQueue",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
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
					  						ValidForQuerySection1 =  "AnalyzeQueue",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "CreateDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
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
					  						HelpTextCode =  "CreateDate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FileSize",
					  						ObjectTableName =  "AnalyzeQueue",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FileSize",
					  						ListPropertyPath =  "FileSize",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AnalyzeQueue",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "FileSize",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FileSize",
					  						DefaultText =  "File Size (KB)",
					  						ListFieldLable =  "FileSizeListLable",
					  						ListLableDefaultText =  "File Size (KB)",
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
					  						HelpTextCode =  "FileSize",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Status",
					  						ObjectTableName =  "AnalyzeQueue",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  4,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Status",
					  						ListPropertyPath =  "Status",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AnalyzeQueue",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Status",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Status",
					  						DefaultText =  "Status Name",
					  						ListFieldLable =  "StatusListLable",
					  						ListLableDefaultText =  "Status",
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
					  						HelpTextCode =  "Status",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ErrorMessage",
					  						ObjectTableName =  "AnalyzeQueue",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  8000,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ErrorMessage",
					  						ListPropertyPath =  "ErrorMessage",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AnalyzeQueue",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ErrorMessage",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ErrorMessage",
					  						DefaultText =  "Error Message",
					  						ListFieldLable =  "ErrorMessageListLable",
					  						ListLableDefaultText =  "Error Message",
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
					  						HelpTextCode =  "ErrorMessage",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Tenant",
					  						ObjectTableName =  "AnalyzeQueue",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Tenant",
					  						ListPropertyPath =  "Tenant",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AnalyzeQueue",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Tenant",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Tenant",
					  						DefaultText =  "Tenant",
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
					  						HelpTextCode =  "Tenant",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Subject",
					  						ObjectTableName =  "AnalyzeQueue",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Subject",
					  						ListPropertyPath =  "Subject",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AnalyzeQueue",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Subject",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Subject",
					  						DefaultText =  "Subject",
					  						ListFieldLable =  "SubjectListLable",
					  						ListLableDefaultText =  "Subject",
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
					  						HelpTextCode =  "Subject",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Retries",
					  						ObjectTableName =  "AnalyzeQueue",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Retries",
					  						ListPropertyPath =  "Retries",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AnalyzeQueue",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Retries",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Retries",
					  						DefaultText =  "Retries",
					  						ListFieldLable =  "RetriesListLable",
					  						ListLableDefaultText =  "Retries",
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
					  						HelpTextCode =  "Retries",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ConnectedToTenant",
					  						ObjectTableName =  "AnalyzeQueue",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ConnectedToTenant",
					  						ListPropertyPath =  "ConnectedToTenant",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AnalyzeQueue",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ConnectedToTenant",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ConnectedToTenant",
					  						DefaultText =  "Connected To Tenant",
					  						ListFieldLable =  "ConnectedToTenantListLable",
					  						ListLableDefaultText =  "Connected To Tenant",
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
					  						HelpTextCode =  "ConnectedToTenant",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ConnectedToEntity",
					  						ObjectTableName =  "AnalyzeQueue",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ConnectedToEntity",
					  						ListPropertyPath =  "ConnectedToEntity",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AnalyzeQueue",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ConnectedToEntity",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ConnectedToEntity",
					  						DefaultText =  "Connected To Entity",
					  						ListFieldLable =  "ConnectedToEntityListLable",
					  						ListLableDefaultText =  "Connected To Entity",
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
					  						HelpTextCode =  "ConnectedToEntity",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "AnalyzeQueue",
					  						FieldsDataType =  "Text",
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
					  						ValidForQuerySection1 =  "AnalyzeQueue",
					  						ValidForQuerySection2 =  "AnalyzeQueueFollowUp",
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
					  						DefaultText =  "Search EntityReference / From / Create Date / Tenant / Status / Subject",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  "Searching by :\n1: Entity References \n2: Tenants \n3: From \n4: Subjects \n5: Statuses \n6: Create Date",
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
					 
					 						FieldName =  "TenantName",
					  						ObjectTableName =  "AnalyzeQueue",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TenantName",
					  						ListPropertyPath =  "TenantName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AnalyzeQueue",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "TenantName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TenantName",
					  						DefaultText =  "Tenant Name",
					  						ListFieldLable =  "TenantNameListLable",
					  						ListLableDefaultText =  "Tenant Name",
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
					  						HelpTextCode =  "TenantName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AWBNumber",
					  						ObjectTableName =  "AnalyzeQueue",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  50,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AWBNumber",
					  						ListPropertyPath =  "AWBNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AnalyzeQueue",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "AWBNumber",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AWBNumber",
					  						DefaultText =  "AWB #",
					  						ListFieldLable =  "AWBNumberListLable",
					  						ListLableDefaultText =  "AWB #",
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
					  						HelpTextCode =  "AWBNumber",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AckReason",
					  						ObjectTableName =  "AnalyzeQueue",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  256,
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AckReason",
					  						ListPropertyPath =  "AckReason",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AnalyzeQueue",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "AckReason",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AckReason",
					  						DefaultText =  "Ack Reason",
					  						ListFieldLable =  "AckReasonListLable",
					  						ListLableDefaultText =  "Ack Reason",
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
					  						HelpTextCode =  "AckReason",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DoneDate",
					  						ObjectTableName =  "AnalyzeQueue",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DoneDate",
					  						ListPropertyPath =  "DoneDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AnalyzeQueue",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "DoneDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DoneDate",
					  						DefaultText =  "Done Date",
					  						ListFieldLable =  "DoneDateListLable",
					  						ListLableDefaultText =  "Done Date",
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
					  						HelpTextCode =  "DoneDate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Log",
					  						ObjectTableName =  "AnalyzeQueue",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  500,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Log",
					  						ListPropertyPath =  "Log",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AnalyzeQueue",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Log",
					  						DefaultText =  "Log",
					  						ListFieldLable =  "LogListLable",
					  						ListLableDefaultText =  "Log",
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
	        QueryGroup AnalyzeQueueQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "ANQU", Name = "Analyze Queues" }, queryGroupRepository,tenantQueryGroups);
						QueryGroup AnalyzeQueueQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "8b20", Name = " Query Group" }, queryGroupRepository,tenantQueryGroups);
				        queryGroupRepository.SubmitChanges();
	        ObjectTable AnalyzeQueueObjectTable = objectTables.ContainsKey("AnalyzeQueue") ? objectTables["AnalyzeQueue"] : null;
            if (AnalyzeQueueObjectTable == null)
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(0);  

                AnalyzeQueueObjectTable = objectContext.ObjectTables.Where(d => d.Name == "AnalyzeQueue" && d.Tenant == 0).FirstOrDefault();
            }

	         
			List<Feature> addedFeatures = new List<Feature>();
			List<TextCode> addedTextCodes = new List<TextCode>();
			List<Query> addedQueries = new List<Query>();
			List<QueryColumn> addedQueryColumns = new List<QueryColumn>();
			List<AdvancedQueryFilter> addedQueryFilters = new List<AdvancedQueryFilter>();
   

			   TextCode AnalyzeQueueTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AnalyzeQueue.Q.TodayAnalyzeQueues", DefaultText = @"Today",LocalDefaultText = null, ObjectTableId = AnalyzeQueueObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature AnalyzeQueueFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TODAYANALYZEQUS", ObjectTableId = AnalyzeQueueObjectTable.Id, Tenant = 0, NameTextCodeCode = "AnalyzeQueueObjectTable.Features.TodayAnalyzeQueues", NameTextCodeDefaultText = "Today Analyze Queues", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,AnalyzeQueueObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode AnalyzeQueueTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AnalyzeQueue.Q.AllAnalyzeQueues", DefaultText = @"All Analyze Queues",LocalDefaultText = null, ObjectTableId = AnalyzeQueueObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature AnalyzeQueueFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLANALYZEQUS", ObjectTableId = AnalyzeQueueObjectTable.Id, Tenant = 0, NameTextCodeCode = "AnalyzeQueueObjectTable.Features.AllAnalyzeQueues", NameTextCodeDefaultText = "All Analyze Queues", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,AnalyzeQueueObjectTable, addedFeatures, addedTextCodes);

	        //TextCodeRepository.SubmitChanges();
	        //FeaturesRepository.SubmitChanges();    
	      

			  Query TodayAnalyzeQueuesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = AnalyzeQueueTextCode_0.Id, NameTextCodeCode = AnalyzeQueueTextCode_0.Code, ObjectTableName = "AnalyzeQueue", Code = "Today Analyze Queues",  QueryGroupCode = "ANQU", IndexOrder = 0, Tenant = 0, ObjectTableId = AnalyzeQueueObjectTable.Id, QuerySection = "AnalyzeQueue", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = AnalyzeQueueFeature_0.Id,FeatureUniqeCode= AnalyzeQueueFeature_0.FeatureUniqeCode, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, addedQueries);
	
			 QueryColumn TodayAnalyzeQueuesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayAnalyzeQueuesQuery.Id,QueryCode = TodayAnalyzeQueuesQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "AnalyzeQueue.Id" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn TodayAnalyzeQueuesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayAnalyzeQueuesQuery.Id,QueryCode = TodayAnalyzeQueuesQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "AnalyzeQueue.From" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn TodayAnalyzeQueuesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayAnalyzeQueuesQuery.Id,QueryCode = TodayAnalyzeQueuesQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "AnalyzeQueue.EntityReference" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn TodayAnalyzeQueuesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayAnalyzeQueuesQuery.Id,QueryCode = TodayAnalyzeQueuesQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "AnalyzeQueue.CreateDate" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn TodayAnalyzeQueuesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayAnalyzeQueuesQuery.Id,QueryCode = TodayAnalyzeQueuesQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "AnalyzeQueue.FileSize" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn TodayAnalyzeQueuesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayAnalyzeQueuesQuery.Id,QueryCode = TodayAnalyzeQueuesQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "AnalyzeQueue.Status" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn TodayAnalyzeQueuesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayAnalyzeQueuesQuery.Id,QueryCode = TodayAnalyzeQueuesQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "AnalyzeQueue.ErrorMessage" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn TodayAnalyzeQueuesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayAnalyzeQueuesQuery.Id,QueryCode = TodayAnalyzeQueuesQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "AnalyzeQueue.Subject" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn TodayAnalyzeQueuesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayAnalyzeQueuesQuery.Id,QueryCode = TodayAnalyzeQueuesQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "AnalyzeQueue.Retries" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn TodayAnalyzeQueuesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayAnalyzeQueuesQuery.Id,QueryCode = TodayAnalyzeQueuesQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "AnalyzeQueue.ConnectedToTenant" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn TodayAnalyzeQueuesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayAnalyzeQueuesQuery.Id,QueryCode = TodayAnalyzeQueuesQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "AnalyzeQueue.ConnectedToEntity" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn TodayAnalyzeQueuesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayAnalyzeQueuesQuery.Id,QueryCode = TodayAnalyzeQueuesQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "AnalyzeQueue.DoneDate" , ColumnWidth = 130 }, addedQueryColumns);

             AdvancedQueryFilter TodayAnalyzeQueuesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "AnalyzeQueue.CreateDate", PredefinedValue = "Today",PredefinedValue2 = null, QueryId = TodayAnalyzeQueuesQuery.Id,QueryCode = TodayAnalyzeQueuesQuery.UniqueCode, Tenant = 0}, addedQueryFilters);

  
	      

			  Query AllAnalyzeQueuesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = AnalyzeQueueTextCode_1.Id, NameTextCodeCode = AnalyzeQueueTextCode_1.Code, ObjectTableName = "AnalyzeQueue", Code = "All Analyze Queues",  QueryGroupCode = "ANQU", IndexOrder = 1, Tenant = 0, ObjectTableId = AnalyzeQueueObjectTable.Id, QuerySection = "AnalyzeQueue", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = AnalyzeQueueFeature_1.Id,FeatureUniqeCode= AnalyzeQueueFeature_1.FeatureUniqeCode, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, addedQueries);
	
			 QueryColumn AllAnalyzeQueuesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAnalyzeQueuesQuery.Id,QueryCode = AllAnalyzeQueuesQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "AnalyzeQueue.Id" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn AllAnalyzeQueuesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAnalyzeQueuesQuery.Id,QueryCode = AllAnalyzeQueuesQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "AnalyzeQueue.From" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn AllAnalyzeQueuesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAnalyzeQueuesQuery.Id,QueryCode = AllAnalyzeQueuesQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "AnalyzeQueue.EntityReference" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn AllAnalyzeQueuesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAnalyzeQueuesQuery.Id,QueryCode = AllAnalyzeQueuesQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "AnalyzeQueue.CreateDate" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn AllAnalyzeQueuesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAnalyzeQueuesQuery.Id,QueryCode = AllAnalyzeQueuesQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "AnalyzeQueue.FileSize" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn AllAnalyzeQueuesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAnalyzeQueuesQuery.Id,QueryCode = AllAnalyzeQueuesQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "AnalyzeQueue.Status" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn AllAnalyzeQueuesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAnalyzeQueuesQuery.Id,QueryCode = AllAnalyzeQueuesQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "AnalyzeQueue.ErrorMessage" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn AllAnalyzeQueuesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAnalyzeQueuesQuery.Id,QueryCode = AllAnalyzeQueuesQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "AnalyzeQueue.Subject" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn AllAnalyzeQueuesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAnalyzeQueuesQuery.Id,QueryCode = AllAnalyzeQueuesQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "AnalyzeQueue.Retries" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn AllAnalyzeQueuesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAnalyzeQueuesQuery.Id,QueryCode = AllAnalyzeQueuesQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "AnalyzeQueue.ConnectedToTenant" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn AllAnalyzeQueuesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAnalyzeQueuesQuery.Id,QueryCode = AllAnalyzeQueuesQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "AnalyzeQueue.ConnectedToEntity" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn AllAnalyzeQueuesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAnalyzeQueuesQuery.Id,QueryCode = AllAnalyzeQueuesQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "AnalyzeQueue.DoneDate" , ColumnWidth = 130 }, addedQueryColumns);
			SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
			SqlBulkInsert.BulkInsert("Features", addedFeatures);
			SqlBulkInsert.BulkInsert("Queries", addedQueries);
			SqlBulkInsert.BulkInsert("QueryColumns", addedQueryColumns);
			SqlBulkInsert.BulkInsert("AdvancedQueryFilters", addedQueryFilters);	 
  
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable AnalyzeQueueObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AnalyzeQueue" && d.Tenant == 0).FirstOrDefault();
		   //List<ObjectField> AnalyzeQueueObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "AnalyzeQueue").ToList();
		       
	      

	         Screen AnalyzeQueueHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "AnalyzeQueue.HeaderScreen", Name = "Header Screen", ObjectTableId = AnalyzeQueueObjectTable.Id, NumberOfColumns = 4, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
             ScreenField AnalyzeQueueAnalyzeQueueHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ScreenId = AnalyzeQueueHeaderScreenScreen0.Id,ScreenCode = AnalyzeQueueHeaderScreenScreen0.Code, ObjectFieldCode = "AnalyzeQueue.Id", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField AnalyzeQueueAnalyzeQueueHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ScreenId = AnalyzeQueueHeaderScreenScreen0.Id,ScreenCode = AnalyzeQueueHeaderScreenScreen0.Code, ObjectFieldCode = "AnalyzeQueue.From", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField AnalyzeQueueAnalyzeQueueHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ScreenId = AnalyzeQueueHeaderScreenScreen0.Id,ScreenCode = AnalyzeQueueHeaderScreenScreen0.Code, ObjectFieldCode = "AnalyzeQueue.CreateDate", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField AnalyzeQueueAnalyzeQueueHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ScreenId = AnalyzeQueueHeaderScreenScreen0.Id,ScreenCode = AnalyzeQueueHeaderScreenScreen0.Code, ObjectFieldCode = "AnalyzeQueue.Subject", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          	
		    AnalyzeQueueObjectTable.HeaderScreenId = AnalyzeQueueHeaderScreenScreen0.Id;
		    AnalyzeQueueObjectTable.HeaderScreenCode = AnalyzeQueueHeaderScreenScreen0.Code;

	   		  
	      

	         Screen AnalyzeQueueGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "AnalyzeQueue.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = AnalyzeQueueObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 8, IsReadOnly = false }, screensRepository, tenantScreens);
      
             ScreenField AnalyzeQueueAnalyzeQueueGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ScreenId = AnalyzeQueueGeneralTabScreenScreen1.Id,ScreenCode = AnalyzeQueueGeneralTabScreenScreen1.Code, ObjectFieldCode = "AnalyzeQueue.From", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField AnalyzeQueueAnalyzeQueueGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ScreenId = AnalyzeQueueGeneralTabScreenScreen1.Id,ScreenCode = AnalyzeQueueGeneralTabScreenScreen1.Code, ObjectFieldCode = "AnalyzeQueue.CreateDate", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField AnalyzeQueueAnalyzeQueueGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ScreenId = AnalyzeQueueGeneralTabScreenScreen1.Id,ScreenCode = AnalyzeQueueGeneralTabScreenScreen1.Code, ObjectFieldCode = "AnalyzeQueue.FileSize", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField AnalyzeQueueAnalyzeQueueGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ScreenId = AnalyzeQueueGeneralTabScreenScreen1.Id,ScreenCode = AnalyzeQueueGeneralTabScreenScreen1.Code, ObjectFieldCode = "AnalyzeQueue.Status", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField AnalyzeQueueAnalyzeQueueGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 4, ScreenId = AnalyzeQueueGeneralTabScreenScreen1.Id,ScreenCode = AnalyzeQueueGeneralTabScreenScreen1.Code, ObjectFieldCode = "AnalyzeQueue.Subject", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField AnalyzeQueueAnalyzeQueueGeneralTabScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 5, ScreenId = AnalyzeQueueGeneralTabScreenScreen1.Id,ScreenCode = AnalyzeQueueGeneralTabScreenScreen1.Code, ObjectFieldCode = "AnalyzeQueue.Retries", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField AnalyzeQueueAnalyzeQueueGeneralTabScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 6, ScreenId = AnalyzeQueueGeneralTabScreenScreen1.Id,ScreenCode = AnalyzeQueueGeneralTabScreenScreen1.Code, ObjectFieldCode = "AnalyzeQueue.TenantName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField AnalyzeQueueAnalyzeQueueGeneralTabScreenScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 7, ScreenId = AnalyzeQueueGeneralTabScreenScreen1.Id,ScreenCode = AnalyzeQueueGeneralTabScreenScreen1.Code, ObjectFieldCode = "AnalyzeQueue.AWBNumber", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField AnalyzeQueueAnalyzeQueueGeneralTabScreenScreenField8 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ScreenId = AnalyzeQueueGeneralTabScreenScreen1.Id,ScreenCode = AnalyzeQueueGeneralTabScreenScreen1.Code, ObjectFieldCode = "AnalyzeQueue.ConnectedToTenant", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField AnalyzeQueueAnalyzeQueueGeneralTabScreenScreenField9 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ScreenId = AnalyzeQueueGeneralTabScreenScreen1.Id,ScreenCode = AnalyzeQueueGeneralTabScreenScreen1.Code, ObjectFieldCode = "AnalyzeQueue.ConnectedToEntity", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	            

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable AnalyzeQueueObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AnalyzeQueue" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode AnalyzeQueueMessageBodyTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AnalyzeQueue.TH.MessageBody", DefaultText = "Message Body",LocalDefaultText = null, ObjectTableId = AnalyzeQueueObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature AnalyzeQueueMessageBodyFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MESSAGEBODY", ObjectTableId = AnalyzeQueueObjectTable.Id, Tenant = 0, NameTextCodeCode = "AnalyzeQueueObjectTable.Features.MessageBody", NameTextCodeDefaultText = "Message Body", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,AnalyzeQueueObjectTable);
 
                 
			   TextCode AnalyzeQueueGeneralTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AnalyzeQueue.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = AnalyzeQueueObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature AnalyzeQueueGeneralFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = AnalyzeQueueObjectTable.Id, Tenant = 0, NameTextCodeCode = "AnalyzeQueueObjectTable.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,AnalyzeQueueObjectTable);
 
                 
			   TextCode AnalyzeQueueErrorsTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AnalyzeQueue.TH.AnalyzeQueueErrors", DefaultText = "Errors",LocalDefaultText = null, ObjectTableId = AnalyzeQueueObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
 
                 
			   TextCode AnalyzeQueueEventsTextCode_TH3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AnalyzeQueue.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = AnalyzeQueueObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature AnalyzeQueueEventsFeature_TH3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = AnalyzeQueueObjectTable.Id, Tenant = 0, NameTextCodeCode = "AnalyzeQueueObjectTable.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,AnalyzeQueueObjectTable);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "AQMB",HtmlComponentName = "",HtmlComponentUrl = "./InfrastructureModules/InfrastructureCommunications/Components/AnalyzeQueue/MessageBodyTabComponent", FeatureId = AnalyzeQueueMessageBodyFeature_TH0.Id,FeatureUniqeCode = AnalyzeQueueMessageBodyFeature_TH0.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.MaintenanceControls.MessageBody", ObjectTableId = AnalyzeQueueObjectTable.Id, TabNameTextCodeId = AnalyzeQueueMessageBodyTextCode_TH0.Id, TabNameTextCodeCode = AnalyzeQueueMessageBodyTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "AQGC",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = AnalyzeQueueGeneralFeature_TH1.Id,FeatureUniqeCode = AnalyzeQueueGeneralFeature_TH1.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = AnalyzeQueueObjectTable.Id, TabNameTextCodeId = AnalyzeQueueGeneralTextCode_TH1.Id, TabNameTextCodeCode = AnalyzeQueueGeneralTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "AQER",HtmlComponentName = "",HtmlComponentUrl = "./InfrastructureModules/InfrastructureCommunications/Components/AnalyzeQueue/AnalyzeQueueErrorsTabComponent", FeatureId = AnalyzeQueueMessageBodyFeature_TH0.Id,FeatureUniqeCode = AnalyzeQueueMessageBodyFeature_TH0.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.MaintenanceControls.AnalyzeQueueErrors", ObjectTableId = AnalyzeQueueObjectTable.Id, TabNameTextCodeId = AnalyzeQueueErrorsTextCode_TH2.Id, TabNameTextCodeCode = AnalyzeQueueErrorsTextCode_TH2.Code, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "AQEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = AnalyzeQueueEventsFeature_TH3.Id,FeatureUniqeCode = AnalyzeQueueEventsFeature_TH3.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = AnalyzeQueueObjectTable.Id, TabNameTextCodeId = AnalyzeQueueEventsTextCode_TH3.Id, TabNameTextCodeCode = AnalyzeQueueEventsTextCode_TH3.Code, Tenant = 0, IndexOrder = 3 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable AnalyzeQueueObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AnalyzeQueue" && d.Tenant == 0).FirstOrDefault(); 


		   		   //--------------> Additional Features <--------------\\

		   Feature AnalyzeQueueFeature_ANALYZEQUEUEERRORS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ANALYZEQUEUEERRORS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = AnalyzeQueueObjectTable.Id, Tenant = 0, NameTextCodeCode = "AnalyzeQueueObjectTable.Features.AnalyzeQueueErrors", NameTextCodeDefaultText = @"Error" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,AnalyzeQueueObjectTable);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable AnalyzeQueueObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AnalyzeQueue" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPAQ",
                EnglishName =  "Analyze Queue Updated",
                LocalName =  "Analyze Queue Updated",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = AnalyzeQueueObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   //FeatureRepository featureRepository = new FeatureRepository(0); 
		   //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable AnalyzeQueueObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AnalyzeQueue" && d.Tenant == 0).FirstOrDefault();       
    
			   Feature AnalyzeQueueFeature_MB0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RESEND", ObjectTableId = AnalyzeQueueObjectTable.Id, Tenant = 0, NameTextCodeCode = "AnalyzeQueue.Features.Resend", NameTextCodeDefaultText = "Resend", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,AnalyzeQueueObjectTable);

 

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup AnalyzeQueueMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "AnalyzeQueueEdit",
					Name = "AnalyzeQueueEditButtonsGroup",
					ObjectTableId = AnalyzeQueueObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton AnalyzeQueueMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Resend",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "AnalyzeQueue.B.Resend",
						LabelTextCodeDefaultText = "Resend",
						Tenant = 0,
						MenuButtonGroupId = AnalyzeQueueMenuButtonGroup.Id,
						ObjectTableId = AnalyzeQueueObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = AnalyzeQueueFeature_MB0.Id,
						Style = null,
						LocalDefaultText = null,
						FeatureUniqeCode = AnalyzeQueueFeature_MB0.FeatureUniqeCode,
						HtmlComponentPath = null,
						Width = 0,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {     
	    
}

    

   }
    
}
	 