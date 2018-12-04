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
   public class ParticipantUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Participant",
			      				    DBTableName =  "Participants",
			      				    ObjectTableSingular =  "Participant",
			      				    ObjectTablePlural =  "Participants",
			      				    DefaultText =  "Participant",
			      				    Name =  "Participants",
			      				    IsNewWizard =  true,
			      				    NewWizardControlName =  "Simplog.FreightLib.NewParticipantCommand",
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  true,
			      				    HasCounter =  false,
			      				    EnableEditFromLOV =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  false,
			      				    CustomFieldsCount =  0,
			      				    HasCustomFields =  false,
			      				    InActive =  false,
			      				    SearchFields =  "Participant,Participants,Simplog.FreightLib.NewParticipantCommand,Id,",
			      				    IsSaveButtonVisible =  true,
			      				    EnableSecurity =  true,
			      				    ObjectTableTypeCode =  "MD",
			      				    IsComposition =  false,
			      				    MaxNumberOfCustomFields =  0,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsEditable =  false,
			      				    HasCustomFilter =  false,
			      				    AllowedForComputingPartners =  false,
			      				    DisableSearchBox =  false,
			      				    ClientModuleName =  "Common",
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasMenuButtons =  false,
			      				    HasFiltersMenu =  false,
			      				    AllowedInQueues =  false,
			      				    Code =  "PPCT",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AirlineTenant",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Integer",
					  						Code =  "AirlineTenant",
					  						MaxLength =  100,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "AirlineTenant",
					  						ListPropertyPath =  "AirlineTenant",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "AirlineTenant",
					  						DefaultText =  @"Airline Tenant",
					  						ListFieldLable =  "AirlineTenantListLable",
					  						ListLableDefaultText =  @"Airline Tenant",
					  						HelpTextCode =  "AirlineTenant",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TTY",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Text",
					  						Code =  "TTY",
					  						MaxLength =  33,
					  						IsCustom =  false,
					  						MinLength =  7,
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
					  						PMPropertyPath =  "TTY",
					  						ListPropertyPath =  "TTY",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "TTY",
					  						DefaultText =  @"TTY",
					  						ListFieldLable =  "TTYListLable",
					  						ListLableDefaultText =  @"TTY",
					  						HelpTextCode =  "TTY",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Registered",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Boolean",
					  						Code =  "Registered",
					  						MaxLength =  100,
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
					  						PMPropertyPath =  "Registered",
					  						ListPropertyPath =  "Registered",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "Registered",
					  						DefaultText =  @"Registered",
					  						ListFieldLable =  "RegisteredListLable",
					  						ListLableDefaultText =  @"Registered",
					  						HelpTextCode =  "Registered",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RegistrationRequested",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Boolean",
					  						Code =  "RegistrationRequested",
					  						MaxLength =  100,
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
					  						PMPropertyPath =  "RegistrationRequested",
					  						ListPropertyPath =  "RegistrationRequested",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "RegistrationRequested",
					  						DefaultText =  @"Registration Requested",
					  						ListFieldLable =  "RegistrationRequestedListLable",
					  						ListLableDefaultText =  @"Registration Requested",
					  						HelpTextCode =  "RegistrationRequested",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Website",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Text",
					  						Code =  "Website",
					  						MaxLength =  100,
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
					  						PMPropertyPath =  "Website",
					  						ListPropertyPath =  "Website",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "Website",
					  						DefaultText =  @"Website",
					  						ListFieldLable =  "WebsiteListLable",
					  						ListLableDefaultText =  @"Website",
					  						HelpTextCode =  "Website",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Code",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Text",
					  						Code =  "Code",
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
					  						AutomaticField =  true,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  1,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "Code",
					  						DefaultText =  @"Code",
					  						ListFieldLable =  "CodeListLable",
					  						ListLableDefaultText =  @"Code",
					  						HelpTextCode =  "Code",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EnglishName",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Text",
					  						Code =  "EnglishName",
					  						MaxLength =  70,
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
					  						PMPropertyPath =  "EnglishName",
					  						ListPropertyPath =  "EnglishName",
					  						DisplayInLookUpIndex =  1,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  1,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "English Name",
					  						DefaultText =  @"Name",
					  						ListFieldLable =  "EnglishNameListLable",
					  						ListLableDefaultText =  @"Name",
					  						HelpTextCode =  "English Name",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LocalName",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "nText",
					  						Code =  "LocalName",
					  						MaxLength =  100,
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
					  						PMPropertyPath =  "LocalName",
					  						ListPropertyPath =  "LocalName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "LocalName",
					  						DefaultText =  @"Local Name",
					  						ListFieldLable =  "LocalNameListLable",
					  						ListLableDefaultText =  @"Local Name",
					  						HelpTextCode =  "LocalName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ComputedLocalName",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "nText",
					  						Code =  "ComputedLocalName",
					  						MaxLength =  100,
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
					  						PMPropertyPath =  "ComputedLocalName",
					  						ListPropertyPath =  "ComputedLocalName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "ComputedLocalName",
					  						DefaultText =  @"Local Name",
					  						HelpTextCode =  "ComputedLocalName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InActive",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Boolean",
					  						Code =  "InActive",
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
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "InActive",
					  						DefaultText =  @"Inactive",
					  						ListFieldLable =  "InActiveListLable",
					  						ListLableDefaultText =  @"Inactive",
					  						HelpTextCode =  "InActive",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VatNumber",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Text",
					  						Code =  "VatNumber",
					  						MaxLength =  20,
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
					  						PMPropertyPath =  "VatNumber",
					  						ListPropertyPath =  "VatNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "VatNumber",
					  						DefaultText =  @"VAT No.",
					  						ListFieldLable =  "VatNumberListLable",
					  						ListLableDefaultText =  @"VAT Number",
					  						HelpTextCode =  "VatNumber",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentTermId",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "PaymentTerm",
					  						Code =  "PaymentTermId",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "PaymentTermId",
					  						ListPropertyPath =  "PaymentTermId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "PaymentTermId",
					  						DefaultText =  @"Payment Term",
					  						HelpTextCode =  "PaymentTermId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentTermEnglishName",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Text",
					  						LookUpTableName =  "PaymentTerm",
					  						Code =  "PaymentTermEnglishName",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "Card.PaymentTerm.EnglishName",
					  						ListPropertyPath =  "PaymentTermEnglishName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "PaymentTermEnglishName",
					  						DefaultText =  @"Payment Term Name",
					  						ListFieldLable =  "PaymentTermListLable",
					  						ListLableDefaultText =  @"Payment Term",
					  						HelpTextCode =  "PaymentTermEnglishName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Notes",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Text",
					  						Code =  "Notes",
					  						MaxLength =  1000,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "Notes",
					  						ListPropertyPath =  "Notes",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						DataTemplateName =  "NotesDataTemplate",
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "Notes",
					  						DefaultText =  @"Notes",
					  						ListFieldLable =  "NotesListLable",
					  						ListLableDefaultText =  @"Notes",
					  						HelpTextCode =  "Notes",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceCurrencyId",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Currency",
					  						Code =  "InvoiceCurrencyId",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "InvoiceCurrencyId",
					  						ListPropertyPath =  "InvoiceCurrencyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "InvoiceCurrencyId",
					  						DefaultText =  @"Invoice Currency",
					  						HelpTextCode =  "InvoiceCurrencyId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VatTypeId",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "VatType",
					  						Code =  "VatTypeId",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "VatTypeId",
					  						ListPropertyPath =  "VatTypeId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "VatTypeId",
					  						DefaultText =  @"VAT Type",
					  						HelpTextCode =  "VatTypeId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BankName",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Text",
					  						Code =  "BankName",
					  						MaxLength =  40,
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
					  						PMPropertyPath =  "BankName",
					  						ListPropertyPath =  "BankName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "BankName",
					  						DefaultText =  @"Bank Name",
					  						HelpTextCode =  "BankName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BankAddress",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Text",
					  						Code =  "BankAddress",
					  						MaxLength =  100,
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
					  						PMPropertyPath =  "BankAddress",
					  						ListPropertyPath =  "BankAddress",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "BankAddress",
					  						DefaultText =  @"Bank Address",
					  						HelpTextCode =  "BankAddress",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Swift",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Text",
					  						Code =  "Swift",
					  						MaxLength =  20,
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
					  						PMPropertyPath =  "Swift",
					  						ListPropertyPath =  "Swift",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "Swift",
					  						DefaultText =  @"Swift",
					  						HelpTextCode =  "Swift",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AccountNumber",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Text",
					  						Code =  "AccountNumber",
					  						MaxLength =  25,
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
					  						PMPropertyPath =  "AccountNumber",
					  						ListPropertyPath =  "AccountNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "AccountNumber",
					  						DefaultText =  @"Bank Account Number",
					  						HelpTextCode =  "AccountNumber",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IBANNumber",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Text",
					  						Code =  "IBANNumber",
					  						MaxLength =  30,
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
					  						PMPropertyPath =  "IBANNumber",
					  						ListPropertyPath =  "IBANNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "IBANNumber",
					  						DefaultText =  @"IBAN No.",
					  						HelpTextCode =  "IBANNumber",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ForwarderTenant",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Integer",
					  						Code =  "ForwarderTenant",
					  						MaxLength =  100,
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
					  						PMPropertyPath =  "ForwarderTenant",
					  						ListPropertyPath =  "ForwarderTenant",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "ForwarderTenant",
					  						DefaultText =  @"Forwarder Tenant",
					  						ListFieldLable =  "ForwarderTenantListLable",
					  						ListLableDefaultText =  @"Forwarder Tenant",
					  						HelpTextCode =  "ForwarderTenant",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsDirect",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Boolean",
					  						Code =  "IsDirect",
					  						MaxLength =  100,
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
					  						PMPropertyPath =  "IsDirect",
					  						ListPropertyPath =  "IsDirect",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "IsDirect",
					  						DefaultText =  @"Is Direct",
					  						ListFieldLable =  "IsDirectListLable",
					  						ListLableDefaultText =  @"Is Direct",
					  						HelpTextCode =  "IsDirect",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FWBNotifyContacts",
					  						ObjectTableName =  "Participant",
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
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "FWBNotifyContacts",
					  						DefaultText =  @"FWB Notify Contacts",
					  						HelpTextCode =  "FWBNotifyContacts",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FHLNotifyContacts",
					  						ObjectTableName =  "Participant",
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
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "FHLNotifyContacts",
					  						DefaultText =  @"FHL Notify Contacts",
					  						HelpTextCode =  "FHLNotifyContacts",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FFRNotifyContacts",
					  						ObjectTableName =  "Participant",
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
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "FFRNotifyContacts",
					  						DefaultText =  @"FFR Notify Contacts",
					  						HelpTextCode =  "FFRNotifyContacts",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ReceivablesAccountingCard",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Text",
					  						Code =  "ReceivablesAccountingCard",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "ReceivablesAccountingCard",
					  						ListPropertyPath =  "ReceivablesAccountingCard",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "ReceivablesAccountingCard",
					  						DefaultText =  @"Receivables External ID",
					  						ListFieldLable =  "ReceivablesAccountingCardListLable",
					  						ListLableDefaultText =  @"Receivables External ID",
					  						HelpTextCode =  "ReceivablesAccountingCard",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PayablesAccountingCard",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Text",
					  						Code =  "PayablesAccountingCard",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "PayablesAccountingCard",
					  						ListPropertyPath =  "PayablesAccountingCard",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "PayablesAccountingCard",
					  						DefaultText =  @"Payables External ID",
					  						ListFieldLable =  "PayablesAccountingCardListLable",
					  						ListLableDefaultText =  @"Payables External ID",
					  						HelpTextCode =  "PayablesAccountingCard",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrimaryContactName",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Text",
					  						Code =  "PrimaryContactName",
					  						MaxLength =  60,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
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
					  						PMPropertyPath =  "PrimaryContactName",
					  						ListPropertyPath =  "PrimaryContactName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "PrimaryContactName",
					  						DefaultText =  @"Primary Contact Name",
					  						ListFieldLable =  "PrimaryContactNameLabel",
					  						ListLableDefaultText =  @"Primary Contact",
					  						HelpTextCode =  "PrimaryContactName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrimaryContactEmail",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Text",
					  						Code =  "PrimaryContactEmail",
					  						MaxLength =  70,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
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
					  						PMPropertyPath =  "PrimaryContactEmail",
					  						ListPropertyPath =  "PrimaryContactEmail",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "PrimaryContactEmail",
					  						DefaultText =  @"Primary Contact E-mail",
					  						ListFieldLable =  "PrimaryContactEmailLabel",
					  						ListLableDefaultText =  @"Primary Contact E-mail",
					  						HelpTextCode =  "PrimaryContactEmail",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrimaryContactPhone",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Text",
					  						Code =  "PrimaryContactPhone",
					  						MaxLength =  25,
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
					  						PMPropertyPath =  "PrimaryContactPhone",
					  						ListPropertyPath =  "PrimaryContactPhone",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "PrimaryContactPhone",
					  						DefaultText =  @"Primary Contact Phone",
					  						HelpTextCode =  "PrimaryContactPhone",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CountryId",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Country",
					  						Code =  "CountryId",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "CountryId",
					  						ListPropertyPath =  "CountryId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "CountryId",
					  						DefaultText =  @"Country",
					  						HelpTextCode =  "CountryId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CountryName",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Text",
					  						Code =  "CountryName",
					  						MaxLength =  120,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "CountryName",
					  						ListPropertyPath =  "CountryName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "CountryName",
					  						DefaultText =  @"Country",
					  						ListFieldLable =  "CountryNameListLabel",
					  						ListLableDefaultText =  @"Country",
					  						HelpTextCode =  "CountryName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CityName",
					  						ObjectTableName =  "Participant",
					  						FieldsDataType =  "Text",
					  						Code =  "CityName",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "CityName",
					  						ListPropertyPath =  "CityName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Participant",
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
					  						FullFieldLable =  "CityName",
					  						DefaultText =  @"City",
					  						ListFieldLable =  "CityNameListLable",
					  						ListLableDefaultText =  @"City",
					  						HelpTextCode =  "CityName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup ParticipantQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "PPCT", Name = "Participants" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable ParticipantObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Participant" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> ParticipantObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Participant").ToList();   

			   TextCode ParticipantTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Participant.Q.Participants", DefaultText = @"Participants",LocalDefaultText = null, ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ParticipantFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PARTICIPANTS", ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, NameTextCodeCode = "Participant.Features.Participants", NameTextCodeDefaultText = "Participants", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query ParticipantsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ParticipantTextCode_0.Id, Code = "Participants",  QueryGroupCode = "PPCT", IndexOrder = 0, Tenant = 0, ObjectTableId = ParticipantObjectTable.Id, QuerySection = "Participant", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = ParticipantFeature_0.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn ParticipantsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ParticipantsQuery.Id, IndexOrder = 0, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == ParticipantObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ParticipantsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ParticipantsQuery.Id, IndexOrder = 1, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == ParticipantObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ParticipantsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ParticipantsQuery.Id, IndexOrder = 2, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == ParticipantObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ParticipantsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ParticipantsQuery.Id, IndexOrder = 3, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "AirlineTenant" && d.ObjectTableId == ParticipantObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ParticipantsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ParticipantsQuery.Id, IndexOrder = 3, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "ForwarderTenant" && d.ObjectTableId == ParticipantObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ParticipantsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ParticipantsQuery.Id, IndexOrder = 4, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "TTY" && d.ObjectTableId == ParticipantObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ParticipantsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ParticipantsQuery.Id, IndexOrder = 5, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == ParticipantObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ParticipantsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ParticipantsQuery.Id, IndexOrder = 6, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "ReceivablesAccountingCard" && d.ObjectTableId == ParticipantObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ParticipantsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ParticipantsQuery.Id, IndexOrder = 7, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "PaymentTermEnglishName" && d.ObjectTableId == ParticipantObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ParticipantsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ParticipantsQuery.Id, IndexOrder = 8, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "Registered" && d.ObjectTableId == ParticipantObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ParticipantsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ParticipantsQuery.Id, IndexOrder = 9, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "RegistrationRequested" && d.ObjectTableId == ParticipantObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ParticipantsQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ParticipantsQuery.Id, IndexOrder = 10, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == ParticipantObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ParticipantsQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ParticipantsQuery.Id, IndexOrder = 11, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "Notes" && d.ObjectTableId == ParticipantObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable ParticipantObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Participant" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> ParticipantObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Participant").ToList();
		       
	      

	         Screen ParticipantHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Participant.HeaderScreen", Name = "Header Screen", ObjectTableId = ParticipantObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField ParticipantParticipantHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = ParticipantHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ParticipantParticipantHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().Id, ScreenId = ParticipantHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ParticipantParticipantHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "ForwarderTenant").FirstOrDefault().Id, ScreenId = ParticipantHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ParticipantParticipantHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "TTY").FirstOrDefault().Id, ScreenId = ParticipantHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    ParticipantObjectTable.HeaderScreenId = ParticipantHeaderScreenScreen0.Id;
	   		  
	      

	         Screen ParticipantGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Participant.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ParticipantObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 7, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField ParticipantParticipantGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = ParticipantGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ParticipantParticipantGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().Id, ScreenId = ParticipantGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ParticipantParticipantGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().Id, ScreenId = ParticipantGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ParticipantParticipantGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "AirlineTenant").FirstOrDefault().Id, ScreenId = ParticipantGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ParticipantParticipantGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 4, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "TTY").FirstOrDefault().Id, ScreenId = ParticipantGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ParticipantParticipantGeneralTabScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 5, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "Website").FirstOrDefault().Id, ScreenId = ParticipantGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ParticipantParticipantGeneralTabScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "Registered").FirstOrDefault().Id, ScreenId = ParticipantGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ParticipantParticipantGeneralTabScreenScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "RegistrationRequested").FirstOrDefault().Id, ScreenId = ParticipantGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ParticipantParticipantGeneralTabScreenScreenField8 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 2, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "InActive").FirstOrDefault().Id, ScreenId = ParticipantGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           
	      

	         Screen ParticipantBillingTabScreenScreen2 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Participant.BillingTabScreen", Name = "Billing Tab Screen", ObjectTableId = ParticipantObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 6, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField ParticipantParticipantBillingTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "VatNumber").FirstOrDefault().Id, ScreenId = ParticipantBillingTabScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ParticipantParticipantBillingTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "PaymentTermId").FirstOrDefault().Id, ScreenId = ParticipantBillingTabScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ParticipantParticipantBillingTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "InvoiceCurrencyId").FirstOrDefault().Id, ScreenId = ParticipantBillingTabScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ParticipantParticipantBillingTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "VatTypeId").FirstOrDefault().Id, ScreenId = ParticipantBillingTabScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ParticipantParticipantBillingTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "BankName").FirstOrDefault().Id, ScreenId = ParticipantBillingTabScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ParticipantParticipantBillingTabScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "BankAddress").FirstOrDefault().Id, ScreenId = ParticipantBillingTabScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ParticipantParticipantBillingTabScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 2, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "Swift").FirstOrDefault().Id, ScreenId = ParticipantBillingTabScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ParticipantParticipantBillingTabScreenScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 3, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "AccountNumber").FirstOrDefault().Id, ScreenId = ParticipantBillingTabScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ParticipantParticipantBillingTabScreenScreenField8 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 4, ObjectFieldId = ParticipantObjectFields.Where(d => d.FieldName == "IBANNumber").FirstOrDefault().Id, ScreenId = ParticipantBillingTabScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {    
			 ObjectTable ParticipantObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Participant" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode ParticipantGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Participant.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ParticipantGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, NameTextCodeCode = "Participant.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode ParticipantBillingTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Participant.TH.Billing", DefaultText = "Billing",LocalDefaultText = null, ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ParticipantBillingFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BILLING", ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, NameTextCodeCode = "Participant.Features.Billing", NameTextCodeDefaultText = "Billing", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode ParticipantAccountingTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Participant.TH.Accounting", DefaultText = "Accounting",LocalDefaultText = null, ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ParticipantAccountingFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCOUNTINGTRANSFER", ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.AccountingTransfer", NameTextCodeDefaultText = "Accounting Transfer", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode ParticipantAddressesTextCode_TH3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Participant.TH.Addresses", DefaultText = "Addresses",LocalDefaultText = null, ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ParticipantAddressesFeature_TH3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ADDRESSES", ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, NameTextCodeCode = "Participant.Features.Addresses", NameTextCodeDefaultText = "Addresses", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode ParticipantContactsTextCode_TH4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Participant.TH.Contacts", DefaultText = "Contacts",LocalDefaultText = null, ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ParticipantContactsFeature_TH4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CONTACTS", ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, NameTextCodeCode = "Participant.Features.Contacts", NameTextCodeDefaultText = "Contacts", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode ParticipantNotifyTextCode_TH5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Participant.TH.Notify", DefaultText = "Notify",LocalDefaultText = null, ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ParticipantNotifyFeature_TH5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NOTIFY", ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, NameTextCodeCode = "Participant.Features.Notify", NameTextCodeDefaultText = "Notify", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode ParticipantDocsInTextCode_TH6 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Participant.TH.DocsIn", DefaultText = "Docs In",LocalDefaultText = null, ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ParticipantDocsInFeature_TH6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DOCSIN", ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, NameTextCodeCode = "Participant.Features.DocsIn", NameTextCodeDefaultText = "Docs In", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PCGC",HtmlComponentName = "",HtmlComponentUrl = "./CommonPartners/Components/EditTabs/Participant//ParticipantGeneralTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "GENERAL" && d.ObjectTableId == ParticipantObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.ParticipantGeneralTabControl", ObjectTableId = ParticipantObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Participant.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PCBL",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "BILLING" && d.ObjectTableId == ParticipantObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.GeneralControls.BillingTabControl", ObjectTableId = ParticipantObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Participant.TH.Billing" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PCAC",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "ACCOUNTINGTRANSFER" && d.ObjectTableId == ParticipantObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.ParticipantAccountingTabControl", ObjectTableId = ParticipantObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Participant.TH.Accounting" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PCAD",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "ADDRESSES" && d.ObjectTableId == ParticipantObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.PartnerAddressesTab", ObjectTableId = ParticipantObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Participant.TH.Addresses" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 3 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PCCO",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "CONTACTS" && d.ObjectTableId == ParticipantObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.PartnerContactsTab", ObjectTableId = ParticipantObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Participant.TH.Contacts" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 4 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PCNO",HtmlComponentName = "",HtmlComponentUrl = "./CommonPartners/Components/EditTabs/Participant/ParticipantNotifyTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "NOTIFY" && d.ObjectTableId == ParticipantObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.ParticipantNotifyTabControl", ObjectTableId = ParticipantObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Participant.TH.Notify" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 5 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PTDI",HtmlComponentName = "",HtmlComponentUrl = "./CommonModules/CommonPartners/Components/EditTabs/Participant/ParticipantDocsInTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "DOCSIN" && d.ObjectTableId == ParticipantObjectTable.Id).FirstOrDefault().Id, ControlPath = "", ObjectTableId = ParticipantObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Participant.TH.DocsIn" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 6 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable ParticipantObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Participant" && d.Tenant == 0).FirstOrDefault(); 
		   Feature ParticipantFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, NameTextCodeCode = "Participant.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature ParticipantFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, NameTextCodeCode = "Participant.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature ParticipantFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, NameTextCodeCode = "Participant.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature ParticipantFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, NameTextCodeCode = "Participant.Features.PackageFeature", NameTextCodeDefaultText = "Participant Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 

		   		   //--------------> Additional Features <--------------\\

		   Feature ParticipantFeature_EVENTS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", FeatureTypeCode = "AREA", Packagable = false, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, NameTextCodeCode = "Participant.Features.Events", NameTextCodeDefaultText = @"Events" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable ParticipantObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Participant" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPPC",
                ShortView =  false,
                IsManualEntry =  false,
                LocalName =  "Participant Updated",
                EnglishName =  "Participant Updated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ParticipantObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRPC",
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
                ObjectTableId = ParticipantObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable ParticipantObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Participant" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode ParticipantTextCode_Participant = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Participant", DefaultText = "Participant",LocalDefaultText = null, ObjectTableId = ParticipantObjectTable.Id, Tenant = 0, TextCodeTypeCode = "T", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 