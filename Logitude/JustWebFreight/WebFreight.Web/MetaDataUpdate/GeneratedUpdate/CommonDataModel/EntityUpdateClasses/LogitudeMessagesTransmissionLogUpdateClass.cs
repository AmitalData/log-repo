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
namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.CommonDataModel.EntityUpdateClasses
{
   public class LogitudeMessagesTransmissionLogUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "LogitudeMessagesTransmissionLog",
			      				    DBTableName =  "LogitudeMessagesTransmissionLogs",
			      				    ObjectTableSingular =  "Logitude Messages Transmission Log",
			      				    ObjectTablePlural =  "Logitude Messages Transmission Logs",
			      				    DefaultText =  "Logitude Messages Transmission Log",
			      				    Name =  "Transmission Logs",
			      				    IsNewWizard =  false,
			      				    HasCustomFilter =  false,
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableEditFromLOV =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  false,
			      				    CustomFieldsCount =  0,
			      				    HasCustomFields =  false,
			      				    InActive =  false,
			      				    SearchFields =  "LogitudeMessagesTransmissionLog,LogitudeMessagesTransmissionLogs,,Id,",
			      				    IsSaveButtonVisible =  true,
			      				    EnableSecurity =  true,
			      				    ObjectTableTypeCode =  "MD",
			      				    IsComposition =  false,
			      				    MaxNumberOfCustomFields =  0,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    HasDocuments =  true,
			      				    IsLookUp =  false,
			      				    IsEditable =  false,
			      				    AllowedForComputingPartners =  false,
			      				    DisableSearchBox =  false,
			      				    ClientModuleName =  "Common",
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasMenuButtons =  false,
			      				    HasFiltersMenu =  false,
			      				    AllowedInQueues =  false,
			      				    Code =  "LMTL",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CCS",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Text",
					  						Code =  "CCS",
					  						MaxLength =  10,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "CCS",
					  						ListPropertyPath =  "CCS",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
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
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  true,
					  						FullFieldLable =  "CCS",
					  						DefaultText =  @"CCS",
					  						ListFieldLable =  "CCSListLable",
					  						ListLableDefaultText =  @"CCS",
					  						HelpTextCode =  "CCS",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AirlineCode",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Text",
					  						Code =  "AirlineCode",
					  						MaxLength =  2,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "AirlineCode",
					  						ListPropertyPath =  "AirlineCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  true,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  true,
					  						FullFieldLable =  "AirlineCode",
					  						DefaultText =  @"Airline Code",
					  						ListFieldLable =  "AirlineCodeListLable",
					  						ListLableDefaultText =  @"Airline Code",
					  						HelpTextCode =  "AirlineCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MessageTypeCode",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Text",
					  						Code =  "MessageTypeCode",
					  						MaxLength =  3,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "MessageTypeCode",
					  						ListPropertyPath =  "MessageTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  true,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  true,
					  						FullFieldLable =  "MessageTypeCode",
					  						DefaultText =  @"Message Type",
					  						ListFieldLable =  "MessageTypeCodeListLable",
					  						ListLableDefaultText =  @"Message Type",
					  						HelpTextCode =  "MessageTypeCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Prefix",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Text",
					  						LookUpTableName =  "ShipmentLevel",
					  						Code =  "Prefix",
					  						MaxLength =  3,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "Prefix",
					  						ListPropertyPath =  "Prefix",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  true,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  true,
					  						FullFieldLable =  "Prefix",
					  						DefaultText =  @"Prefix",
					  						HelpTextCode =  "Prefix",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AWBNumber",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Text",
					  						Code =  "AWBNumber",
					  						MaxLength =  20,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "AWBNumber",
					  						ListPropertyPath =  "AWBNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
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
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "AWBNumber",
					  						DefaultText =  @"AWB Number",
					  						ListFieldLable =  "AWBNumberListLable",
					  						ListLableDefaultText =  @"AWB Number",
					  						HelpTextCode =  "AWBNumber",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "HWB",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Text",
					  						Code =  "HWB",
					  						MaxLength =  20,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "HWB",
					  						ListPropertyPath =  "HWB",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
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
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "HWB",
					  						DefaultText =  @"HWB",
					  						ListFieldLable =  "HWBListLable",
					  						ListLableDefaultText =  @"HWB",
					  						HelpTextCode =  "HWB",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SentDate",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "DateTime",
					  						Code =  "SentDate",
					  						MaxLength =  3,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "SentDate",
					  						ListPropertyPath =  "SentDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  true,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "SentDate",
					  						DefaultText =  @"Sent Date",
					  						ListFieldLable =  "SentDateListLable",
					  						ListLableDefaultText =  @"Sent Date",
					  						HelpTextCode =  "SentDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Participant",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Text",
					  						Code =  "Participant",
					  						MaxLength =  60,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "Participant",
					  						ListPropertyPath =  "Participant",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  true,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "Participant",
					  						DefaultText =  @"Participant",
					  						ListFieldLable =  "ParticipantListLable",
					  						ListLableDefaultText =  @"Participant",
					  						HelpTextCode =  "Participant",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IATACode",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Text",
					  						Code =  "IATACode",
					  						MaxLength =  7,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "IATACode",
					  						ListPropertyPath =  "IATACode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  true,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "IATACode",
					  						DefaultText =  @"IATA Code",
					  						ListFieldLable =  "IATACodeListLable",
					  						ListLableDefaultText =  @"IATA Code",
					  						HelpTextCode =  "IATACode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CASSCode",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Text",
					  						Code =  "CASSCode",
					  						MaxLength =  4,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "CASSCode",
					  						ListPropertyPath =  "CASSCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  true,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "CASSCode",
					  						DefaultText =  @"CASS Code",
					  						ListFieldLable =  "CASSCodeListLable",
					  						ListLableDefaultText =  @"CASS Code",
					  						HelpTextCode =  "CASSCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UserName",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Text",
					  						Code =  "UserName",
					  						MaxLength =  60,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "UserName",
					  						ListPropertyPath =  "UserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  true,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "UserName",
					  						DefaultText =  @"Sent by User",
					  						ListFieldLable =  "UserNameListLable",
					  						ListLableDefaultText =  @"Sent by User",
					  						HelpTextCode =  "UserName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Origin",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Text",
					  						Code =  "Origin",
					  						MaxLength =  3,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "Origin",
					  						ListPropertyPath =  "Origin",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  true,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "Origin",
					  						DefaultText =  @"Origin",
					  						ListFieldLable =  "OriginListLable",
					  						ListLableDefaultText =  @"Origin",
					  						HelpTextCode =  "Origin",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Destination",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Text",
					  						Code =  "Destination",
					  						MaxLength =  3,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "Destination",
					  						ListPropertyPath =  "Destination",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  true,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "Destination",
					  						DefaultText =  @"Destination",
					  						ListFieldLable =  "DestinationListLable",
					  						ListLableDefaultText =  @"Destination",
					  						HelpTextCode =  "Destination",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Pieces",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Integer",
					  						Code =  "Pieces",
					  						MaxLength =  3,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "Pieces",
					  						ListPropertyPath =  "Pieces",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  true,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "Pieces",
					  						DefaultText =  @"Pieces",
					  						ListFieldLable =  "PiecesListLable",
					  						ListLableDefaultText =  @"Pieces",
					  						HelpTextCode =  "Pieces",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "GrossWeight",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Decimal",
					  						Code =  "GrossWeight",
					  						MaxLength =  10,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "GrossWeight",
					  						ListPropertyPath =  "GrossWeight",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  true,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "GrossWeight",
					  						DefaultText =  @"Gross Weight",
					  						ListFieldLable =  "GrossWeightListLable",
					  						ListLableDefaultText =  @"Gross Weight",
					  						HelpTextCode =  "GrossWeight",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "GrossWeightUnitCode",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Text",
					  						Code =  "GrossWeightUnitCode",
					  						MaxLength =  3,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "GrossWeightUnitCode",
					  						ListPropertyPath =  "GrossWeightUnitCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
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
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "GrossWeightUnitCode",
					  						DefaultText =  @"Gross Weight Unit",
					  						HelpTextCode =  "GrossWeightUnitCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ChargeableWeight",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Decimal",
					  						Code =  "ChargeableWeight",
					  						MaxLength =  3,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "ChargeableWeight",
					  						ListPropertyPath =  "ChargeableWeight",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  true,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "ChargeableWeight",
					  						DefaultText =  @"Charg. Weight",
					  						ListFieldLable =  "ChargeableWeightListLable",
					  						ListLableDefaultText =  @"Charg. Weight",
					  						HelpTextCode =  "ChargeableWeight",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ChargeableWeightUnitCode",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Text",
					  						Code =  "ChargeableWeightUnitCode",
					  						MaxLength =  3,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "ChargeableWeightUnitCode",
					  						ListPropertyPath =  "ChargeableWeightUnitCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
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
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "ChargeableWeightUnitCode",
					  						DefaultText =  @"Chargeable Weight Unit",
					  						HelpTextCode =  "ChargeableWeightUnitCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Volume",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Decimal",
					  						Code =  "Volume",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "Volume",
					  						ListPropertyPath =  "Volume",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  true,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "Volume",
					  						DefaultText =  @"Volume",
					  						ListFieldLable =  "VolumeListLable",
					  						ListLableDefaultText =  @"Volume",
					  						HelpTextCode =  "Volume",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VolumeUnitCode",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Text",
					  						Code =  "VolumeUnitCode",
					  						MaxLength =  3,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "VolumeUnitCode",
					  						ListPropertyPath =  "VolumeUnitCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
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
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "VolumeUnitCode",
					  						DefaultText =  @"Volume Unit",
					  						HelpTextCode =  "VolumeUnitCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DescriptionOfGoods",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Text",
					  						Code =  "DescriptionOfGoods",
					  						MaxLength =  2000,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "DescriptionOfGoods",
					  						ListPropertyPath =  "DescriptionOfGoods",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  true,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "DescriptionOfGoods",
					  						DefaultText =  @"Description of Goods",
					  						HelpTextCode =  "DescriptionOfGoods",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DirectParticipant",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Boolean",
					  						Code =  "DirectParticipant",
					  						MaxLength =  60,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "DirectParticipant",
					  						ListPropertyPath =  "DirectParticipant",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  true,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "DirectParticipant",
					  						DefaultText =  @"Direct Participant",
					  						ListFieldLable =  "DirectParticipantListLable",
					  						ListLableDefaultText =  @"Direct Participant",
					  						HelpTextCode =  "DirectParticipant",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsUpdatedinAirlineTenant",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Boolean",
					  						Code =  "IsUpdatedinAirlineTenant",
					  						MaxLength =  1,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "IsUpdatedinAirlineTenant",
					  						ListPropertyPath =  "IsUpdatedinAirlineTenant",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "IsUpdatedinAirlineTenant",
					  						DefaultText =  @"Updated in Airline Tenant",
					  						ListFieldLable =  "IsUpdatedinAirlineTenantListLable",
					  						ListLableDefaultText =  @"Updated in Airline Tenant",
					  						HelpTextCode =  "IsUpdatedinAirlineTenant",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UserEmail",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Text",
					  						Code =  "UserEmail",
					  						MaxLength =  100,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "UserEmail",
					  						ListPropertyPath =  "UserEmail",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  true,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "UserEmail",
					  						DefaultText =  @"Sent by User e-mail",
					  						ListFieldLable =  "UserEmailListLable",
					  						ListLableDefaultText =  @"Sent by User e-mail",
					  						HelpTextCode =  "UserEmail",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "nText",
					  						Code =  "SearchFields",
					  						MaxLength =  1000,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "SearchFields",
					  						ListPropertyPath =  "SearchFields",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "LogitudeMessagesTransmissionLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  @"Search...",
					  						HelpTextCode =  "SearchFields",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FWBNotifyContacts",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Emails",
					  						Code =  "FWBNotifyContacts",
					  						MaxLength =  4000,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "FWBNotifyContacts",
					  						ListPropertyPath =  "FWBNotifyContacts",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  true,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "FWBNotifyContacts",
					  						DefaultText =  @"FWB Notify Contacts",
					  						HelpTextCode =  "FWBNotifyContacts",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FHLNotifyContacts",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Emails",
					  						Code =  "FHLNotifyContacts",
					  						MaxLength =  4000,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "FHLNotifyContacts",
					  						ListPropertyPath =  "FHLNotifyContacts",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  true,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "FHLNotifyContacts",
					  						DefaultText =  @"FHL Notify Contacts",
					  						HelpTextCode =  "FHLNotifyContacts",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FFRNotifyContacts",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Emails",
					  						Code =  "FFRNotifyContacts",
					  						MaxLength =  4000,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "FFRNotifyContacts",
					  						ListPropertyPath =  "FFRNotifyContacts",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  true,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "FFRNotifyContacts",
					  						DefaultText =  @"FFR Notify Contacts",
					  						HelpTextCode =  "FFRNotifyContacts",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SourceTenant",
					  						ObjectTableName =  "LogitudeMessagesTransmissionLog",
					  						FieldsDataType =  "Integer",
					  						Code =  "SourceTenant",
					  						MaxLength =  1,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "SourceTenant",
					  						ListPropertyPath =  "SourceTenant",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  true,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "SourceTenant",
					  						DefaultText =  @"Source Tenant",
					  						HelpTextCode =  "SourceTenant",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup LogitudeMessagesTransmissionLogQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "LMTL", Name = "Transmission Logs" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable LogitudeMessagesTransmissionLogObjectTable = objectContext.ObjectTables.Where(d => d.Name == "LogitudeMessagesTransmissionLog" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> LogitudeMessagesTransmissionLogObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "LogitudeMessagesTransmissionLog").ToList();   

			   TextCode LogitudeMessagesTransmissionLogTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "LogitudeMessagesTransmissionLog.Q.AllTransmissionLogs", DefaultText = @"All Logitude Messages Transmission Logs",LocalDefaultText = null, ObjectTableId = LogitudeMessagesTransmissionLogObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature LogitudeMessagesTransmissionLogFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLTRANSMISSIONLOG", ObjectTableId = LogitudeMessagesTransmissionLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "LogitudeMessagesTransmissionLog.Features.LogitudeMessagesTransmissionLogs", NameTextCodeDefaultText = "Logitude Messages Transmission Logs", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AllLogitudeTransmissionLogsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = LogitudeMessagesTransmissionLogTextCode_0.Id, Code = "All Logitude Transmission Logs",  QueryGroupCode = "LMTL", IndexOrder = 0, Tenant = 0, ObjectTableId = LogitudeMessagesTransmissionLogObjectTable.Id, QuerySection = "LogitudeMessagesTransmissionLog", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = LogitudeMessagesTransmissionLogFeature_0.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllLogitudeTransmissionLogsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllLogitudeTransmissionLogsQuery.Id, IndexOrder = 1, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "CCS" && d.ObjectTableId == LogitudeMessagesTransmissionLogObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 50 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllLogitudeTransmissionLogsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllLogitudeTransmissionLogsQuery.Id, IndexOrder = 2, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "AirlineCode" && d.ObjectTableId == LogitudeMessagesTransmissionLogObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllLogitudeTransmissionLogsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllLogitudeTransmissionLogsQuery.Id, IndexOrder = 3, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "Prefix" && d.ObjectTableId == LogitudeMessagesTransmissionLogObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 70 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllLogitudeTransmissionLogsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllLogitudeTransmissionLogsQuery.Id, IndexOrder = 4, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "MessageTypeCode" && d.ObjectTableId == LogitudeMessagesTransmissionLogObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 70 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllLogitudeTransmissionLogsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllLogitudeTransmissionLogsQuery.Id, IndexOrder = 5, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "AWBNumber" && d.ObjectTableId == LogitudeMessagesTransmissionLogObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllLogitudeTransmissionLogsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllLogitudeTransmissionLogsQuery.Id, IndexOrder = 6, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "SentDate" && d.ObjectTableId == LogitudeMessagesTransmissionLogObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 70 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllLogitudeTransmissionLogsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllLogitudeTransmissionLogsQuery.Id, IndexOrder = 7, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "Participant" && d.ObjectTableId == LogitudeMessagesTransmissionLogObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllLogitudeTransmissionLogsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllLogitudeTransmissionLogsQuery.Id, IndexOrder = 8, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "Origin" && d.ObjectTableId == LogitudeMessagesTransmissionLogObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 70 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllLogitudeTransmissionLogsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllLogitudeTransmissionLogsQuery.Id, IndexOrder = 9, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "Destination" && d.ObjectTableId == LogitudeMessagesTransmissionLogObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 70 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllLogitudeTransmissionLogsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllLogitudeTransmissionLogsQuery.Id, IndexOrder = 10, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "UserName" && d.ObjectTableId == LogitudeMessagesTransmissionLogObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllLogitudeTransmissionLogsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllLogitudeTransmissionLogsQuery.Id, IndexOrder = 11, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "DirectParticipant" && d.ObjectTableId == LogitudeMessagesTransmissionLogObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable LogitudeMessagesTransmissionLogObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "LogitudeMessagesTransmissionLog" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> LogitudeMessagesTransmissionLogObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "LogitudeMessagesTransmissionLog").ToList();
		       
	      

	         Screen LogitudeMessagesTransmissionLogHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "LogitudeMessagesTransmissionLog.HeaderScreen", Name = "Header Screen", ObjectTableId = LogitudeMessagesTransmissionLogObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField LogitudeMessagesTransmissionLogLogitudeMessagesTransmissionLogHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "AirlineCode").FirstOrDefault().Id, ScreenId = LogitudeMessagesTransmissionLogHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField LogitudeMessagesTransmissionLogLogitudeMessagesTransmissionLogHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "MessageTypeCode").FirstOrDefault().Id, ScreenId = LogitudeMessagesTransmissionLogHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField LogitudeMessagesTransmissionLogLogitudeMessagesTransmissionLogHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "AWBNumber").FirstOrDefault().Id, ScreenId = LogitudeMessagesTransmissionLogHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField LogitudeMessagesTransmissionLogLogitudeMessagesTransmissionLogHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "SentDate").FirstOrDefault().Id, ScreenId = LogitudeMessagesTransmissionLogHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    LogitudeMessagesTransmissionLogObjectTable.HeaderScreenId = LogitudeMessagesTransmissionLogHeaderScreenScreen0.Id;
	   		  
	      

	         Screen LogitudeMessagesTransmissionLogGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "LogitudeMessagesTransmissionLog.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = LogitudeMessagesTransmissionLogObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 12, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField LogitudeMessagesTransmissionLogLogitudeMessagesTransmissionLogGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "CCS").FirstOrDefault().Id, ScreenId = LogitudeMessagesTransmissionLogGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField LogitudeMessagesTransmissionLogLogitudeMessagesTransmissionLogGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "MessageTypeCode").FirstOrDefault().Id, ScreenId = LogitudeMessagesTransmissionLogGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField LogitudeMessagesTransmissionLogLogitudeMessagesTransmissionLogGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "AirlineCode").FirstOrDefault().Id, ScreenId = LogitudeMessagesTransmissionLogGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField LogitudeMessagesTransmissionLogLogitudeMessagesTransmissionLogGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "AWBNumber").FirstOrDefault().Id, ScreenId = LogitudeMessagesTransmissionLogGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField LogitudeMessagesTransmissionLogLogitudeMessagesTransmissionLogGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 4, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "SentDate").FirstOrDefault().Id, ScreenId = LogitudeMessagesTransmissionLogGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField LogitudeMessagesTransmissionLogLogitudeMessagesTransmissionLogGeneralTabScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 5, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "UserName").FirstOrDefault().Id, ScreenId = LogitudeMessagesTransmissionLogGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField LogitudeMessagesTransmissionLogLogitudeMessagesTransmissionLogGeneralTabScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 6, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "UserEmail").FirstOrDefault().Id, ScreenId = LogitudeMessagesTransmissionLogGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField LogitudeMessagesTransmissionLogLogitudeMessagesTransmissionLogGeneralTabScreenScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 7, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "DirectParticipant").FirstOrDefault().Id, ScreenId = LogitudeMessagesTransmissionLogGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField LogitudeMessagesTransmissionLogLogitudeMessagesTransmissionLogGeneralTabScreenScreenField8 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 8, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "IsUpdatedinAirlineTenant").FirstOrDefault().Id, ScreenId = LogitudeMessagesTransmissionLogGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField LogitudeMessagesTransmissionLogLogitudeMessagesTransmissionLogGeneralTabScreenScreenField9 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "Participant").FirstOrDefault().Id, ScreenId = LogitudeMessagesTransmissionLogGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField LogitudeMessagesTransmissionLogLogitudeMessagesTransmissionLogGeneralTabScreenScreenField10 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "Origin").FirstOrDefault().Id, ScreenId = LogitudeMessagesTransmissionLogGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField LogitudeMessagesTransmissionLogLogitudeMessagesTransmissionLogGeneralTabScreenScreenField11 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 2, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "Destination").FirstOrDefault().Id, ScreenId = LogitudeMessagesTransmissionLogGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField LogitudeMessagesTransmissionLogLogitudeMessagesTransmissionLogGeneralTabScreenScreenField12 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 3, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "Pieces").FirstOrDefault().Id, ScreenId = LogitudeMessagesTransmissionLogGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField LogitudeMessagesTransmissionLogLogitudeMessagesTransmissionLogGeneralTabScreenScreenField13 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 4, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "GrossWeight").FirstOrDefault().Id, ScreenId = LogitudeMessagesTransmissionLogGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField LogitudeMessagesTransmissionLogLogitudeMessagesTransmissionLogGeneralTabScreenScreenField14 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 5, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "ChargeableWeight").FirstOrDefault().Id, ScreenId = LogitudeMessagesTransmissionLogGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField LogitudeMessagesTransmissionLogLogitudeMessagesTransmissionLogGeneralTabScreenScreenField15 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 6, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "Volume").FirstOrDefault().Id, ScreenId = LogitudeMessagesTransmissionLogGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField LogitudeMessagesTransmissionLogLogitudeMessagesTransmissionLogGeneralTabScreenScreenField16 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 7, ObjectFieldId = LogitudeMessagesTransmissionLogObjectFields.Where(d => d.FieldName == "DescriptionOfGoods").FirstOrDefault().Id, ScreenId = LogitudeMessagesTransmissionLogGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {    
			 ObjectTable LogitudeMessagesTransmissionLogObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "LogitudeMessagesTransmissionLog" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode LogitudeMessagesTransmissionLogGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "LogitudeMessagesTransmissionLog.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = LogitudeMessagesTransmissionLogObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature LogitudeMessagesTransmissionLogGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = LogitudeMessagesTransmissionLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "LogitudeMessagesTransmissionLog.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode LogitudeMessagesTransmissionLogAuditTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "LogitudeMessagesTransmissionLog.TH.Audit", DefaultText = "Audit",LocalDefaultText = null, ObjectTableId = LogitudeMessagesTransmissionLogObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature LogitudeMessagesTransmissionLogAuditFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AUDIT", ObjectTableId = LogitudeMessagesTransmissionLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "LogitudeMessagesTransmissionLog.Features.Audit", NameTextCodeDefaultText = "Audit", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "MTGC",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "GENERAL" && d.ObjectTableId == LogitudeMessagesTransmissionLogObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = LogitudeMessagesTransmissionLogObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "LogitudeMessagesTransmissionLog.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "MTAD",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "AUDIT" && d.ObjectTableId == LogitudeMessagesTransmissionLogObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.InfrastructureExt.Views.Maintenance.TransmissionLog.TransmissionLogAuditTabControl", ObjectTableId = LogitudeMessagesTransmissionLogObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "LogitudeMessagesTransmissionLog.TH.Audit" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable LogitudeMessagesTransmissionLogObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "LogitudeMessagesTransmissionLog" && d.Tenant == 0).FirstOrDefault(); 
		   Feature LogitudeMessagesTransmissionLogFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = LogitudeMessagesTransmissionLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "LogitudeMessagesTransmissionLog.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature LogitudeMessagesTransmissionLogFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = LogitudeMessagesTransmissionLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "LogitudeMessagesTransmissionLog.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature LogitudeMessagesTransmissionLogFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = LogitudeMessagesTransmissionLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "LogitudeMessagesTransmissionLog.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature LogitudeMessagesTransmissionLogFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = LogitudeMessagesTransmissionLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "LogitudeMessagesTransmissionLog.Features.PackageFeature", NameTextCodeDefaultText = "LogitudeMessagesTransmissionLog Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 

		   		   //--------------> Additional Features <--------------\\

		   Feature LogitudeMessagesTransmissionLogFeature_AUTOMATION = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AUTOMATION", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = LogitudeMessagesTransmissionLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "LogitudeMessagesTransmissionLog.Features.Automation", NameTextCodeDefaultText = @"Automation" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable LogitudeMessagesTransmissionLogObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "LogitudeMessagesTransmissionLog" && d.Tenant == 0).FirstOrDefault(); 
	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable LogitudeMessagesTransmissionLogObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "LogitudeMessagesTransmissionLog" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode LogitudeMessagesTransmissionLogTextCode_LogitudeMessagesTransmissionLog = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "LogitudeMessagesTransmissionLog", DefaultText = "Logitude Messages Transmission Log",LocalDefaultText = null, ObjectTableId = LogitudeMessagesTransmissionLogObjectTable.Id, Tenant = 0, TextCodeTypeCode = "T", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 