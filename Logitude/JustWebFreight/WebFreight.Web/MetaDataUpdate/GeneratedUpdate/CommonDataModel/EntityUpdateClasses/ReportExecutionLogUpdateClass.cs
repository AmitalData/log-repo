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
using Logitude.Workflow.Data.Repositories;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.CLoseTable;
using Logitude.DashboardModule.Data.Repositories;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.BL;

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.CommonDataModel.EntityUpdateClasses
{
   public class ReportExecutionLogUpdateClass
   {  		
		public const string HashString = "adac4ca8ab37f134f774757f5c2baf3d";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "ReportExecutionLog",
			      				    IsNew =  true,
			      				    DBTableName =  "ReportExecutionLogs",
			      				    ObjectTableSingular =  "Report Execution Log",
			      				    ObjectTablePlural =  "Report Execution Logs",
			      				    HasCustomFilter =  true,
			      				    HasCustomFields =  false,
			      				    AvailableInCustomization =  true,
			      				    SupportSubEntity =  false,
			      				    ApplyGenericCustomFields =  false,
			      				    AvailableInDocumentTypes =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  true,
			      				    IsNewWizard =  false,
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  true,
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
			      				    DefaultText =  "Report Execution Log",
			      				    Code =  "4df5",
			      				    Name =  " Query Group",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Common",
			      				    NoTS =  false,
			      				    HasMenuButtons =  true,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    HashString =  ReportExecutionLogUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Id",
					  						ObjectTableName =  "ReportExecutionLog",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
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
					  						ValidForQuerySection1 =  "ReportExecutionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Id",
					  						DefaultText =  "Id",
					  						ListFieldLable =  "IdListLable",
					  						ListLableDefaultText =  "Id",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  true,
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
					 
					 						FieldName =  "Tenant",
					  						ObjectTableName =  "ReportExecutionLog",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
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
					  						ValidForQuerySection1 =  "ReportExecutionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Tenant",
					  						DefaultText =  "Tenant",
					  						ListFieldLable =  "TenantListLable",
					  						ListLableDefaultText =  "Tenant",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
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
					  						ObjectTableName =  "ReportExecutionLog",
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
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Between",
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
					  						ValidForQuerySection1 =  "ReportExecutionLog",
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
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
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
					 
					 						FieldName =  "CreatedByUserId",
					  						ObjectTableName =  "ReportExecutionLog",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
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
					  						ValidForQuerySection1 =  "ReportExecutionLog",
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
					  						IsForeignKey =  true,
					  						ForeignEntity =  "User",
					  						NavigationPropertyName =  "CreatedByUser",
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
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
					 
					 						FieldName =  "StatusCode",
					  						ObjectTableName =  "ReportExecutionLog",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "CommunicationStatusType",
					  						MinLength =  0,
					  						MaxLength =  4,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  4,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StatusCode",
					  						ListPropertyPath =  "StatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ReportExecutionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StatusCode",
					  						DefaultText =  "Status Code",
					  						ListFieldLable =  "StatusCodeListLable",
					  						ListLableDefaultText =  "Status Code",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "CommunicationStatusType",
					  						NavigationPropertyName =  "CommunicationStatusType",
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ExceptionMessage",
					  						ObjectTableName =  "ReportExecutionLog",
					  						FieldsDataType =  "nText",
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
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ExceptionMessage",
					  						ListPropertyPath =  "ExceptionMessage",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ReportExecutionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ExceptionMessage",
					  						DefaultText =  "Exception Message",
					  						ListFieldLable =  "ExceptionMessageListLable",
					  						ListLableDefaultText =  "Exception Message",
					  						IsForeignKey =  false,
					  						IsMaxLength =  true,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DoneDate",
					  						ObjectTableName =  "ReportExecutionLog",
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
					  						IsListFilter =  false,
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
					  						ValidForQuerySection1 =  "ReportExecutionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DoneDate",
					  						DefaultText =  "Done Date",
					  						ListFieldLable =  "DoneDateListLable",
					  						ListLableDefaultText =  "Done Date",
					  						ListLocalDefaultText =  " ",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ReportFilterXML",
					  						ObjectTableName =  "ReportExecutionLog",
					  						FieldsDataType =  "nText",
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
					  						IsListFilter =  false,
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ReportFilterXML",
					  						ListPropertyPath =  "ReportFilterXML",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ReportExecutionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ReportFilterXML",
					  						DefaultText =  "Report Filter XML",
					  						ListFieldLable =  "ReportFilterXMLListLable",
					  						ListLableDefaultText =  "ReportFilterXML",
					  						IsForeignKey =  false,
					  						IsMaxLength =  true,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ReportId",
					  						ObjectTableName =  "ReportExecutionLog",
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
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ReportId",
					  						ListPropertyPath =  "ReportId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ReportExecutionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ReportId",
					  						DefaultText =  "Report Id",
					  						ListFieldLable =  "ReportIdListLable",
					  						ListLableDefaultText =  "Report Id",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "Report",
					  						NavigationPropertyName =  "Report",
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ReportTemplateId",
					  						ObjectTableName =  "ReportExecutionLog",
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
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ReportTemplateId",
					  						ListPropertyPath =  "ReportTemplateId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ReportExecutionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ReportTemplateId",
					  						DefaultText =  "Report Template Id",
					  						ListFieldLable =  "ReportTemplateIdListLable",
					  						ListLableDefaultText =  "Report Template Id",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "ReportsTemplate",
					  						NavigationPropertyName =  "ReportsTemplate",
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RetryNumber",
					  						ObjectTableName =  "ReportExecutionLog",
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
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "RetryNumber",
					  						ListPropertyPath =  "RetryNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ReportExecutionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "RetryNumber",
					  						DefaultText =  "Retry Number",
					  						ListFieldLable =  "RetryNumberListLable",
					  						ListLableDefaultText =  "Retry Number",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StartDate",
					  						ObjectTableName =  "ReportExecutionLog",
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
					  						IsListFilter =  false,
					  						Operator =  "Between",
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
					  						ValidForQuerySection1 =  "ReportExecutionLog",
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
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DisablePreview",
					  						ObjectTableName =  "ReportExecutionLog",
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
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DisablePreview",
					  						ListPropertyPath =  "DisablePreview",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ReportExecutionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DisablePreview",
					  						DefaultText =  "Disable Preview",
					  						ListFieldLable =  "DisablePreviewListLable",
					  						ListLableDefaultText =  "Disable Preview",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ExecutedByServerName",
					  						ObjectTableName =  "ReportExecutionLog",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  200,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  200,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ExecutedByServerName",
					  						ListPropertyPath =  "ExecutedByServerName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ReportExecutionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ExecutedByServerName",
					  						DefaultText =  "Executed By Server Name",
					  						ListFieldLable =  "ExecutedByServerNameListLable",
					  						ListLableDefaultText =  "Executed By Server Name",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ReportName",
					  						ObjectTableName =  "ReportExecutionLog",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
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
					  						IsListFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ReportName",
					  						ListPropertyPath =  "ReportName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ReportExecutionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ReportName",
					  						DefaultText =  "Report Name",
					  						ListFieldLable =  "ReportNameListLable",
					  						ListLableDefaultText =  "Report Name",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StatusName",
					  						ObjectTableName =  "ReportExecutionLog",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StatusName",
					  						ListPropertyPath =  "StatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ReportExecutionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StatusName",
					  						DefaultText =  "Status",
					  						ListFieldLable =  "StatusNameListLable",
					  						ListLableDefaultText =  "Status",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
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
					  						ObjectTableName =  "ReportExecutionLog",
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
					  						SystemMaxLength =  100,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
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
					  						ValidForQuerySection1 =  "ReportExecutionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
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
					  						ObjectTableName =  "ReportExecutionLog",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "StartsWith",
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
					  						ValidForQuerySection1 =  "ReportExecutionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreatedByUserName",
					  						DefaultText =  "Created By User",
					  						ListFieldLable =  "CreatedByUserNameListLable",
					  						ListLableDefaultText =  "Created By User",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
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
	        QueryGroup ReportExecutionLogQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "4df5", Name = " Query Group" }, queryGroupRepository,tenantQueryGroups);
						QueryGroup ReportExecutionLogQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "2e27", Name = " Query Group" }, queryGroupRepository,tenantQueryGroups);
				        queryGroupRepository.SubmitChanges();
	        ObjectTable ReportExecutionLogObjectTable = objectTables.ContainsKey("ReportExecutionLog") ? objectTables["ReportExecutionLog"] : null;
            if (ReportExecutionLogObjectTable == null)
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(0);  

                ReportExecutionLogObjectTable = objectContext.ObjectTables.Where(d => d.Name == "ReportExecutionLog" && d.Tenant == 0).FirstOrDefault();
            }

	         
			List<Feature> addedFeatures = new List<Feature>();
			List<TextCode> addedTextCodes = new List<TextCode>();
			List<Query> addedQueries = new List<Query>();
			List<QueryColumn> addedQueryColumns = new List<QueryColumn>();
			List<AdvancedQueryFilter> addedQueryFilters = new List<AdvancedQueryFilter>();
   

			   TextCode ReportExecutionLogTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ReportExecutionLog.Q.Today", DefaultText = @"Today",LocalDefaultText = null, ObjectTableId = ReportExecutionLogObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature ReportExecutionLogFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ReportExecutionLog.Q.Today", ObjectTableId = ReportExecutionLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportExecutionLogFeatures.Today", NameTextCodeDefaultText = "Today", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,ReportExecutionLogObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode ReportExecutionLogTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ReportExecutionLog.Q.All", DefaultText = @"All",LocalDefaultText = null, ObjectTableId = ReportExecutionLogObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature ReportExecutionLogFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ReportExecutionLog.Q.All", ObjectTableId = ReportExecutionLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportExecutionLogFeatures.All", NameTextCodeDefaultText = "All", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,ReportExecutionLogObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode ReportExecutionLogTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ReportExecutionLog.Q.Failed", DefaultText = @"Failed",LocalDefaultText = null, ObjectTableId = ReportExecutionLogObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature ReportExecutionLogFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ReportExecutionLog.Q.Failed", ObjectTableId = ReportExecutionLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportExecutionLogFeatures.Failed", NameTextCodeDefaultText = "Failed", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,ReportExecutionLogObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode ReportExecutionLogTextCode_3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ReportExecutionLog.Q.InProcess", DefaultText = @"In Process",LocalDefaultText = null, ObjectTableId = ReportExecutionLogObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature ReportExecutionLogFeature_3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ReportExecutionLog.Q.InProcess", ObjectTableId = ReportExecutionLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportExecutionLogFeatures.InProcess", NameTextCodeDefaultText = "InProcess", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,ReportExecutionLogObjectTable, addedFeatures, addedTextCodes);

	        //TextCodeRepository.SubmitChanges();
	        //FeaturesRepository.SubmitChanges();    
	      

			  Query TodayQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ReportExecutionLogTextCode_0.Id, NameTextCodeCode = ReportExecutionLogTextCode_0.Code, ObjectTableName = "ReportExecutionLog", Code = "Today",  QueryGroupCode = "4df5", IndexOrder = 0, Tenant = 0, ObjectTableId = ReportExecutionLogObjectTable.Id, QuerySection = "ReportExecutionLog", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = ReportExecutionLogFeature_0.Id,FeatureUniqeCode= ReportExecutionLogFeature_0.FeatureUniqeCode, DefaultSortName = "CreateDate", DefaultSortDirection = "Desending", Perspective = null }, addedQueries);
	
			 QueryColumn TodayQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayQuery.Id,QueryCode = TodayQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "ReportExecutionLog.Tenant" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn TodayQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayQuery.Id,QueryCode = TodayQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "ReportExecutionLog.CreateDate" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn TodayQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayQuery.Id,QueryCode = TodayQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "ReportExecutionLog.ReportName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn TodayQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayQuery.Id,QueryCode = TodayQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "ReportExecutionLog.CreatedByUserName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn TodayQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayQuery.Id,QueryCode = TodayQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "ReportExecutionLog.ExecutedByServerName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn TodayQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayQuery.Id,QueryCode = TodayQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "ReportExecutionLog.StatusName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn TodayQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayQuery.Id,QueryCode = TodayQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "ReportExecutionLog.ExceptionMessage" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn TodayQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayQuery.Id,QueryCode = TodayQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "ReportExecutionLog.DoneDate" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn TodayQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayQuery.Id,QueryCode = TodayQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "ReportExecutionLog.ReportFilterXML" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn TodayQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayQuery.Id,QueryCode = TodayQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "ReportExecutionLog.ReportId" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn TodayQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayQuery.Id,QueryCode = TodayQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "ReportExecutionLog.ReportTemplateId" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn TodayQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayQuery.Id,QueryCode = TodayQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "ReportExecutionLog.RetryNumber" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn TodayQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayQuery.Id,QueryCode = TodayQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "ReportExecutionLog.DisablePreview" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn TodayQueryColumn_13 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayQuery.Id,QueryCode = TodayQuery.UniqueCode, IndexOrder = 13, ObjectFieldCode = "ReportExecutionLog.StartDate" , ColumnWidth = 130 }, addedQueryColumns);

             AdvancedQueryFilter TodayQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "ReportExecutionLog.CreateDate", PredefinedValue = "Today",PredefinedValue2 = null, CustomPredefined = false, QueryId = TodayQuery.Id,QueryCode = TodayQuery.UniqueCode, Tenant = 0}, addedQueryFilters);

  
	      

			  Query AllQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ReportExecutionLogTextCode_1.Id, NameTextCodeCode = ReportExecutionLogTextCode_1.Code, ObjectTableName = "ReportExecutionLog", Code = "All",  QueryGroupCode = "4df5", IndexOrder = 1, Tenant = 0, ObjectTableId = ReportExecutionLogObjectTable.Id, QuerySection = "ReportExecutionLog", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = ReportExecutionLogFeature_1.Id,FeatureUniqeCode= ReportExecutionLogFeature_1.FeatureUniqeCode, DefaultSortName = "CreateDate", DefaultSortDirection = "Desending", Perspective = null }, addedQueries);
	
			 QueryColumn AllQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuery.Id,QueryCode = AllQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "ReportExecutionLog.Tenant" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AllQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuery.Id,QueryCode = AllQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "ReportExecutionLog.CreateDate" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn AllQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuery.Id,QueryCode = AllQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "ReportExecutionLog.ReportName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn AllQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuery.Id,QueryCode = AllQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "ReportExecutionLog.CreatedByUserName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn AllQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuery.Id,QueryCode = AllQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "ReportExecutionLog.ExecutedByServerName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn AllQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuery.Id,QueryCode = AllQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "ReportExecutionLog.StatusName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AllQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuery.Id,QueryCode = AllQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "ReportExecutionLog.ExceptionMessage" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn AllQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuery.Id,QueryCode = AllQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "ReportExecutionLog.DoneDate" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn AllQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuery.Id,QueryCode = AllQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "ReportExecutionLog.ReportFilterXML" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn AllQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuery.Id,QueryCode = AllQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "ReportExecutionLog.ReportId" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AllQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuery.Id,QueryCode = AllQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "ReportExecutionLog.ReportTemplateId" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AllQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuery.Id,QueryCode = AllQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "ReportExecutionLog.RetryNumber" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AllQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuery.Id,QueryCode = AllQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "ReportExecutionLog.DisablePreview" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn AllQueryColumn_13 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuery.Id,QueryCode = AllQuery.UniqueCode, IndexOrder = 13, ObjectFieldCode = "ReportExecutionLog.StartDate" , ColumnWidth = 130 }, addedQueryColumns);
  
	      

			  Query FailedQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ReportExecutionLogTextCode_2.Id, NameTextCodeCode = ReportExecutionLogTextCode_2.Code, ObjectTableName = "ReportExecutionLog", Code = "Failed",  QueryGroupCode = "4df5", IndexOrder = 2, Tenant = 0, ObjectTableId = ReportExecutionLogObjectTable.Id, QuerySection = "ReportExecutionLog", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = ReportExecutionLogFeature_2.Id,FeatureUniqeCode= ReportExecutionLogFeature_2.FeatureUniqeCode, DefaultSortName = "CreateDate", DefaultSortDirection = "Desending", Perspective = null }, addedQueries);
	
			 QueryColumn FailedQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FailedQuery.Id,QueryCode = FailedQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "ReportExecutionLog.Tenant" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn FailedQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FailedQuery.Id,QueryCode = FailedQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "ReportExecutionLog.CreateDate" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn FailedQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FailedQuery.Id,QueryCode = FailedQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "ReportExecutionLog.StatusCode" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn FailedQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FailedQuery.Id,QueryCode = FailedQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "ReportExecutionLog.ExceptionMessage" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn FailedQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FailedQuery.Id,QueryCode = FailedQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "ReportExecutionLog.DoneDate" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn FailedQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FailedQuery.Id,QueryCode = FailedQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "ReportExecutionLog.ReportFilterXML" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn FailedQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FailedQuery.Id,QueryCode = FailedQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "ReportExecutionLog.ReportId" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn FailedQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FailedQuery.Id,QueryCode = FailedQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "ReportExecutionLog.ReportTemplateId" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn FailedQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FailedQuery.Id,QueryCode = FailedQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "ReportExecutionLog.RetryNumber" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn FailedQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FailedQuery.Id,QueryCode = FailedQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "ReportExecutionLog.StartDate" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn FailedQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FailedQuery.Id,QueryCode = FailedQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "ReportExecutionLog.DisablePreview" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn FailedQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FailedQuery.Id,QueryCode = FailedQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "ReportExecutionLog.ExecutedByServerName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn FailedQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FailedQuery.Id,QueryCode = FailedQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "ReportExecutionLog.ReportName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn FailedQueryColumn_13 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FailedQuery.Id,QueryCode = FailedQuery.UniqueCode, IndexOrder = 13, ObjectFieldCode = "ReportExecutionLog.StatusName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn FailedQueryColumn_14 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FailedQuery.Id,QueryCode = FailedQuery.UniqueCode, IndexOrder = 14, ObjectFieldCode = "ReportExecutionLog.CreatedByUserName" , ColumnWidth = 100 }, addedQueryColumns);

             AdvancedQueryFilter FailedQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "ReportExecutionLog.StatusCode", PredefinedValue = "F",PredefinedValue2 = null, CustomPredefined = false, QueryId = FailedQuery.Id,QueryCode = FailedQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query InProcessQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ReportExecutionLogTextCode_3.Id, NameTextCodeCode = ReportExecutionLogTextCode_3.Code, ObjectTableName = "ReportExecutionLog", Code = "InProcess",  QueryGroupCode = "4df5", IndexOrder = 3, Tenant = 0, ObjectTableId = ReportExecutionLogObjectTable.Id, QuerySection = "ReportExecutionLog", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = ReportExecutionLogFeature_3.Id,FeatureUniqeCode= ReportExecutionLogFeature_3.FeatureUniqeCode, DefaultSortName = "StatusCode", DefaultSortDirection = "Desending", Perspective = null }, addedQueries);
	
			 QueryColumn InProcessQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InProcessQuery.Id,QueryCode = InProcessQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "ReportExecutionLog.Tenant" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn InProcessQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InProcessQuery.Id,QueryCode = InProcessQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "ReportExecutionLog.CreateDate" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn InProcessQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InProcessQuery.Id,QueryCode = InProcessQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "ReportExecutionLog.StatusCode" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn InProcessQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InProcessQuery.Id,QueryCode = InProcessQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "ReportExecutionLog.ExceptionMessage" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn InProcessQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InProcessQuery.Id,QueryCode = InProcessQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "ReportExecutionLog.DoneDate" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn InProcessQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InProcessQuery.Id,QueryCode = InProcessQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "ReportExecutionLog.ReportFilterXML" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn InProcessQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InProcessQuery.Id,QueryCode = InProcessQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "ReportExecutionLog.ReportId" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn InProcessQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InProcessQuery.Id,QueryCode = InProcessQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "ReportExecutionLog.ReportTemplateId" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn InProcessQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InProcessQuery.Id,QueryCode = InProcessQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "ReportExecutionLog.RetryNumber" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn InProcessQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InProcessQuery.Id,QueryCode = InProcessQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "ReportExecutionLog.StartDate" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn InProcessQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InProcessQuery.Id,QueryCode = InProcessQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "ReportExecutionLog.DisablePreview" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn InProcessQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InProcessQuery.Id,QueryCode = InProcessQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "ReportExecutionLog.ExecutedByServerName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn InProcessQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InProcessQuery.Id,QueryCode = InProcessQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "ReportExecutionLog.ReportName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn InProcessQueryColumn_13 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InProcessQuery.Id,QueryCode = InProcessQuery.UniqueCode, IndexOrder = 13, ObjectFieldCode = "ReportExecutionLog.StatusName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn InProcessQueryColumn_14 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InProcessQuery.Id,QueryCode = InProcessQuery.UniqueCode, IndexOrder = 14, ObjectFieldCode = "ReportExecutionLog.CreatedByUserName" , ColumnWidth = 100 }, addedQueryColumns);

             AdvancedQueryFilter InProcessQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "ReportExecutionLog.StatusCode", PredefinedValue = "P",PredefinedValue2 = null, CustomPredefined = false, QueryId = InProcessQuery.Id,QueryCode = InProcessQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

			SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
			SqlBulkInsert.BulkInsert("Features", addedFeatures);
			SqlBulkInsert.BulkInsert("Queries", addedQueries);
			SqlBulkInsert.BulkInsert("QueryColumns", addedQueryColumns);
			SqlBulkInsert.BulkInsert("AdvancedQueryFilters", addedQueryFilters);	 
  
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable ReportExecutionLogObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ReportExecutionLog" && d.Tenant == 0).FirstOrDefault();
		   //List<ObjectField> ReportExecutionLogObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "ReportExecutionLog").ToList();
		       
	      

	         Screen ReportExecutionLogHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ReportExecutionLog.HeaderScreen", Name = "Header Screen", ObjectTableId = ReportExecutionLogObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
             ScreenField ReportExecutionLogReportExecutionLogHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ScreenId = ReportExecutionLogHeaderScreenScreen0.Id,ScreenCode = ReportExecutionLogHeaderScreenScreen0.Code, ObjectFieldCode = "ReportExecutionLog.ReportName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField ReportExecutionLogReportExecutionLogHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ScreenId = ReportExecutionLogHeaderScreenScreen0.Id,ScreenCode = ReportExecutionLogHeaderScreenScreen0.Code, ObjectFieldCode = "ReportExecutionLog.CreateDate", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          	
		    ReportExecutionLogObjectTable.HeaderScreenId = ReportExecutionLogHeaderScreenScreen0.Id;
		    ReportExecutionLogObjectTable.HeaderScreenCode = ReportExecutionLogHeaderScreenScreen0.Code;

	   		  
	      

	         Screen ReportExecutionLogGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ReportExecutionLog.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ReportExecutionLogObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 4, IsReadOnly = true }, screensRepository, tenantScreens);
      
             ScreenField ReportExecutionLogReportExecutionLogGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ScreenId = ReportExecutionLogGeneralTabScreenScreen1.Id,ScreenCode = ReportExecutionLogGeneralTabScreenScreen1.Code, ObjectFieldCode = "ReportExecutionLog.CreateDate", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField ReportExecutionLogReportExecutionLogGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ScreenId = ReportExecutionLogGeneralTabScreenScreen1.Id,ScreenCode = ReportExecutionLogGeneralTabScreenScreen1.Code, ObjectFieldCode = "ReportExecutionLog.StatusCode", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField ReportExecutionLogReportExecutionLogGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ScreenId = ReportExecutionLogGeneralTabScreenScreen1.Id,ScreenCode = ReportExecutionLogGeneralTabScreenScreen1.Code, ObjectFieldCode = "ReportExecutionLog.DoneDate", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField ReportExecutionLogReportExecutionLogGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ScreenId = ReportExecutionLogGeneralTabScreenScreen1.Id,ScreenCode = ReportExecutionLogGeneralTabScreenScreen1.Code, ObjectFieldCode = "ReportExecutionLog.StartDate", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField ReportExecutionLogReportExecutionLogGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ScreenId = ReportExecutionLogGeneralTabScreenScreen1.Id,ScreenCode = ReportExecutionLogGeneralTabScreenScreen1.Code, ObjectFieldCode = "ReportExecutionLog.ReportName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField ReportExecutionLogReportExecutionLogGeneralTabScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ScreenId = ReportExecutionLogGeneralTabScreenScreen1.Id,ScreenCode = ReportExecutionLogGeneralTabScreenScreen1.Code, ObjectFieldCode = "ReportExecutionLog.ReportId", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField ReportExecutionLogReportExecutionLogGeneralTabScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 2, ScreenId = ReportExecutionLogGeneralTabScreenScreen1.Id,ScreenCode = ReportExecutionLogGeneralTabScreenScreen1.Code, ObjectFieldCode = "ReportExecutionLog.ReportTemplateId", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField ReportExecutionLogReportExecutionLogGeneralTabScreenScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 3, ScreenId = ReportExecutionLogGeneralTabScreenScreen1.Id,ScreenCode = ReportExecutionLogGeneralTabScreenScreen1.Code, ObjectFieldCode = "ReportExecutionLog.ExecutedByServerName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	            

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable ReportExecutionLogObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ReportExecutionLog" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode ReportExecutionLogGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ReportExecutionLog.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = ReportExecutionLogObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ReportExecutionLogGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ReportExecutionLog.Tab.General", ObjectTableId = ReportExecutionLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportExecutionLogFeatures.RLGC", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,ReportExecutionLogObjectTable);
 
                 
			   TextCode ReportExecutionLogReportFilterXMLTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ReportExecutionLog.TH.MessageBody", DefaultText = "Report Filter XML",LocalDefaultText = null, ObjectTableId = ReportExecutionLogObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ReportExecutionLogReportFilterXMLFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ReportExecutionLog.Tab.ReportFilterXML", ObjectTableId = ReportExecutionLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportExecutionLogFeatures.RLMB", NameTextCodeDefaultText = "Report Filter XML", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,ReportExecutionLogObjectTable);
 
                 
			   TextCode ReportExecutionLogErrorTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ReportExecutionLog.TH.Error", DefaultText = "Error",LocalDefaultText = null, ObjectTableId = ReportExecutionLogObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ReportExecutionLogErrorFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ReportExecutionLog.Tab.Error", ObjectTableId = ReportExecutionLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportExecutionLogFeatures.RLER", NameTextCodeDefaultText = "Error", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,ReportExecutionLogObjectTable);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "RLGC",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = ReportExecutionLogGeneralFeature_TH0.Id,FeatureUniqeCode = ReportExecutionLogGeneralFeature_TH0.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = ReportExecutionLogObjectTable.Id, TabNameTextCodeId = ReportExecutionLogGeneralTextCode_TH0.Id, TabNameTextCodeCode = ReportExecutionLogGeneralTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "RLMB",HtmlComponentName = "ReportExecutionLogMessageBodyComponent",HtmlComponentUrl = "./InfrastructureModules/InfrastructureCommunications/Components/ReportExecutionLog/ReportExecutionLogMessageBodyComponent", FeatureId = ReportExecutionLogReportFilterXMLFeature_TH1.Id,FeatureUniqeCode = ReportExecutionLogReportFilterXMLFeature_TH1.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = ReportExecutionLogObjectTable.Id, TabNameTextCodeId = ReportExecutionLogReportFilterXMLTextCode_TH1.Id, TabNameTextCodeCode = ReportExecutionLogReportFilterXMLTextCode_TH1.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "RLER",HtmlComponentName = "ReportExecutionLogErrorComponent",HtmlComponentUrl = "./InfrastructureModules/InfrastructureCommunications/Components/ReportExecutionLog/ReportExecutionLogErrorComponent", FeatureId = ReportExecutionLogErrorFeature_TH2.Id,FeatureUniqeCode = ReportExecutionLogErrorFeature_TH2.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = ReportExecutionLogObjectTable.Id, TabNameTextCodeId = ReportExecutionLogErrorTextCode_TH2.Id, TabNameTextCodeCode = ReportExecutionLogErrorTextCode_TH2.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable ReportExecutionLogObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ReportExecutionLog" && d.Tenant == 0).FirstOrDefault(); 

		   Feature ReportExecutionLogFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = ReportExecutionLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportExecutionLog.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportExecutionLogObjectTable);
		   Feature ReportExecutionLogFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = ReportExecutionLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportExecutionLog.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportExecutionLogObjectTable);
		   Feature ReportExecutionLogFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = ReportExecutionLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportExecutionLog.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportExecutionLogObjectTable);
		   Feature ReportExecutionLogFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = false, ObjectTableId = ReportExecutionLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportExecutionLog.Features.PackageFeature", NameTextCodeDefaultText = "ReportExecutionLog Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportExecutionLogObjectTable);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable ReportExecutionLogObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ReportExecutionLog" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = ReportExecutionLogObjectTable.Id,
				 
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
                ObjectTableId = ReportExecutionLogObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   //FeatureRepository featureRepository = new FeatureRepository(0); 
		   //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable ReportExecutionLogObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ReportExecutionLog" && d.Tenant == 0).FirstOrDefault();       
    
			   Feature ReportExecutionLogFeature_MB0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Cancel", ObjectTableId = ReportExecutionLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportExecutionLog.Features.Cancel", NameTextCodeDefaultText = "Cancel", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,ReportExecutionLogObjectTable);

 

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup ReportExecutionLogMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "ReportExecutionLogEdit",
					Name = "ReportExecutionLogEditButtonsGroup",
					ObjectTableId = ReportExecutionLogObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton ReportExecutionLogMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Cancel",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "ReportExecutionLog.B.Cancel",
						LabelTextCodeDefaultText = "Cancel",
						Tenant = 0,
						MenuButtonGroupId = ReportExecutionLogMenuButtonGroup.Id,
						ObjectTableId = ReportExecutionLogObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = ReportExecutionLogFeature_MB0.Id,
						Style = null,
						LocalDefaultText = "ביטול",
						FeatureUniqeCode = ReportExecutionLogFeature_MB0.FeatureUniqeCode,
						HtmlComponentPath = null,
						Width = 0,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {     
	    
}

    

   }
    
}
	 