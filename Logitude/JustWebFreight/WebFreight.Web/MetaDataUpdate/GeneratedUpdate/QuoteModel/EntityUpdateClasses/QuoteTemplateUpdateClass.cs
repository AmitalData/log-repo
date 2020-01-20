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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.QuoteModel.EntityUpdateClasses
{
   public class QuoteTemplateUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "QuoteTemplate",
			      				    IsNew =  false,
			      				    DBTableName =  "QuoteTemplates",
			      				    OldDBTableName =  "QuoteTemplates",
			      				    ObjectTableSingular =  "Quote Template",
			      				    ObjectTablePlural =  "Quote Templates",
			      				    HasCustomFilter =  true,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  true,
			      				    IsNewWizard =  true,
			      				    LookUp1 =  "Name",
			      				    DependencyFilter1 =  "TemplateTypeCode",
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  true,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  true,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  true,
			      				    EnableEditFromLOV =  false,
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "Simplog.QuoteLib.NewQuoteTemplateCommand",
			      				    DefaultText =  "Quote Template",
			      				    Code =  "QUTE",
			      				    Name =  "QuoteTemplates",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Quote",
			      				    NewWizardComponentPath =  "./QuoteModules/QuoteTemplates/Components/NewQuoteTemplateComponent",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    SearchFields =  "QuoteTemplate,QuoteTemplates,Simplog.QuoteLib.NewQuoteTemplateCommand,Id,",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "HeaderDocId",
					  						OldFieldName =  "HeaderDocId",
					  						ObjectTableName =  "QuoteTemplate",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "HeaderDocId",
					  						ListPropertyPath =  "HeaderDocId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "HeaderDocId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "HeaderDocId",
					  						DefaultText =  "HeaderDocId",
					  						ListFieldLable =  "HeaderDocIdListLable",
					  						ListLableDefaultText =  "HeaderDocId",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "HeaderDocId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FooterDocId",
					  						OldFieldName =  "FooterDocId",
					  						ObjectTableName =  "QuoteTemplate",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FooterDocId",
					  						ListPropertyPath =  "FooterDocId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "FooterDocId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FooterDocId",
					  						DefaultText =  "FooterDocId",
					  						ListFieldLable =  "FooterDocIdListLable",
					  						ListLableDefaultText =  "FooterDocId",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "FooterDocId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "QuoteTemplateSettingId",
					  						OldFieldName =  "QuoteTemplateSettingId",
					  						ObjectTableName =  "QuoteTemplate",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "QuoteTemplateSettingId",
					  						ListPropertyPath =  "QuoteTemplateSettingId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "QuoteTemplateSettingId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "QuoteTemplateSettingId",
					  						DefaultText =  "QuoteTemplateSettingId",
					  						ListFieldLable =  "QuoteTemplateSettingIdListLable",
					  						ListLableDefaultText =  "QuoteTemplateSettingId",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "QuoteTemplateSettingId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Name",
					  						OldFieldName =  "Name",
					  						ObjectTableName =  "QuoteTemplate",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
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
					  						ValidForQuerySection1 =  "QuoteTemplate",
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
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsTemplate",
					  						OldFieldName =  "IsTemplate",
					  						ObjectTableName =  "QuoteTemplate",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsTemplate",
					  						ListPropertyPath =  "IsTemplate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsTemplate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsTemplate",
					  						DefaultText =  "Is Template",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsTemplate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OriginalQuoteTemplateId",
					  						OldFieldName =  "OriginalQuoteTemplateId",
					  						ObjectTableName =  "QuoteTemplate",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OriginalQuoteTemplateId",
					  						ListPropertyPath =  "OriginalQuoteTemplateId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "OriginalQuoteTemplateId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OriginalQuoteTemplateId",
					  						DefaultText =  "Original Quote Template Id",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "OriginalQuoteTemplateId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreateDate",
					  						OldFieldName =  "CreateDate",
					  						ObjectTableName =  "QuoteTemplate",
					  						FieldsDataType =  "DateTime",
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
					  						DisplayInList =  false,
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
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "CreateDate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdateDate",
					  						OldFieldName =  "UpdateDate",
					  						ObjectTableName =  "QuoteTemplate",
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
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "UpdateDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdateDate",
					  						DefaultText =  "Update Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "UpdateDate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByUserId",
					  						OldFieldName =  "CreatedByUserId",
					  						ObjectTableName =  "QuoteTemplate",
					  						FieldsDataType =  "Text",
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
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "CreatedByUserId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreatedByUserId",
					  						DefaultText =  "Created By UserId",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "CreatedByUserId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByUserId",
					  						OldFieldName =  "UpdatedByUserId",
					  						ObjectTableName =  "QuoteTemplate",
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
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "UpdatedByUserId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdatedByUserId",
					  						DefaultText =  "Updated By UserId",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "UpdatedByUserId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						OldFieldName =  "SearchFields",
					  						ObjectTableName =  "QuoteTemplate",
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
					  						SystemMaxLength =  0,
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
					  						DefaultText =  "Search..",
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
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TemplateTypeCode",
					  						OldFieldName =  "TemplateTypeCode",
					  						ObjectTableName =  "QuoteTemplate",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  true,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TemplateTypeCode",
					  						ListPropertyPath =  "TemplateTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "TemplateTypeCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TemplateTypeCode",
					  						DefaultText =  "Template Type Code",
					  						ListFieldLable =  "TemplateTypeCodeListLable",
					  						ListLableDefaultText =  "TemplateTypeCode",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "TemplateTypeCode",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TemplateTypeName",
					  						OldFieldName =  "TemplateTypeName",
					  						ObjectTableName =  "QuoteTemplate",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TemplateTypeName",
					  						ListPropertyPath =  "TemplateTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "QuoteTemplate",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "TemplateTypeName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TemplateTypeName",
					  						DefaultText =  "Template Type",
					  						ListFieldLable =  "TemplateTypeNameListLable",
					  						ListLableDefaultText =  "Template Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "TemplateTypeName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsDefault",
					  						OldFieldName =  "IsDefault",
					  						ObjectTableName =  "QuoteTemplate",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  100,
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
					  						PMPropertyPath =  "IsDefault",
					  						ListPropertyPath =  "IsDefault",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "QuoteTemplate",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsDefault",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsDefault",
					  						DefaultText =  "Is Default",
					  						ListFieldLable =  "IsDefaultListLable",
					  						ListLableDefaultText =  "Is Default",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsDefault",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ShowLocalLanguage",
					  						OldFieldName =  "ShowLocalLanguage",
					  						ObjectTableName =  "QuoteTemplate",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ShowLocalLanguage",
					  						ListPropertyPath =  "ShowLocalLanguage",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "QuoteTemplate",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ShowLocalLanguage",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ShowLocalLanguage",
					  						DefaultText =  "Show Local Language",
					  						ListFieldLable =  "ShowLocalLanguageListLable",
					  						ListLableDefaultText =  "Show Local Language",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ShowLocalLanguage",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InActive",
					  						OldFieldName =  "InActive",
					  						ObjectTableName =  "QuoteTemplate",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "Inactive",
					  						ListPropertyPath =  "Inactive",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "QuoteTemplate",
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
					  						DefaultText =  "In Active",
					  						ListFieldLable =  "InActiveListLable",
					  						ListLableDefaultText =  "In Active",
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
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsCopiedAtSignup",
					  						OldFieldName =  "IsCopiedAtSignup",
					  						ObjectTableName =  "QuoteTemplate",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsCopiedAtSignup",
					  						ListPropertyPath =  "IsCopiedAtSignup",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsCopiedAtSignup",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsCopiedAtSignup",
					  						DefaultText =  "Copy at Signup",
					  						ListFieldLable =  "IsCopiedAtSignupListLable",
					  						ListLableDefaultText =  "IsCopiedAtSignup",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsCopiedAtSignup",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsEnabledForCustomers",
					  						OldFieldName =  "IsEnabledForCustomers",
					  						ObjectTableName =  "QuoteTemplate",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsEnabledForCustomers",
					  						ListPropertyPath =  "IsEnabledForCustomers",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsEnabledForCustomers",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsEnabledForCustomers",
					  						DefaultText =  "Enabled for Customers",
					  						ListFieldLable =  "IsEnabledForCustomersListLable",
					  						ListLableDefaultText =  "IsEnabledForCustomers",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsEnabledForCustomers",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup QuoteTemplateQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "QUTE", Name = "QuoteTemplates" }, queryGroupRepository);
						QueryGroup QuoteTemplateQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "db63", Name = " Query Group" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable QuoteTemplateObjectTable = objectContext.ObjectTables.Where(d => d.Name == "QuoteTemplate" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> QuoteTemplateObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "QuoteTemplate").ToList();   

			   TextCode QuoteTemplateTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.Q.QuoteTemplates", DefaultText = @"Quote Templates",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature QuoteTemplateFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QUOTETEMPLATES", ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteTemplate.Features.QuoteTemplate", NameTextCodeDefaultText = "Quote Templates", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query QuoteTemplatesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = QuoteTemplateTextCode_0.Id, NameTextCodeCode = QuoteTemplateTextCode_0.Code, ObjectTableName = "QuoteTemplate", Code = "QuoteTemplates",  EditWizardName = "Simplog.QuoteLib.Views.QuoteTemplateViews.QuoteTemplateWizardEditControl",
			   QueryGroupCode = "QUTE", IndexOrder = 0, Tenant = 0, ObjectTableId = QuoteTemplateObjectTable.Id, QuerySection = "QuoteTemplate", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = QuoteTemplateFeature_0.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn QuoteTemplatesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = QuoteTemplatesQuery.Id,QueryCode = QuoteTemplatesQuery.Code, IndexOrder = 0, ObjectFieldId = QuoteTemplateObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == QuoteTemplateObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = QuoteTemplateObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == QuoteTemplateObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn QuoteTemplatesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = QuoteTemplatesQuery.Id,QueryCode = QuoteTemplatesQuery.Code, IndexOrder = 1, ObjectFieldId = QuoteTemplateObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == QuoteTemplateObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = QuoteTemplateObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == QuoteTemplateObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn QuoteTemplatesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = QuoteTemplatesQuery.Id,QueryCode = QuoteTemplatesQuery.Code, IndexOrder = 2, ObjectFieldId = QuoteTemplateObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == QuoteTemplateObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = QuoteTemplateObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == QuoteTemplateObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {    

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable QuoteTemplateObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "QuoteTemplate" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode QuoteTemplateGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature QuoteTemplateGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteTemplate.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode QuoteTemplateSettingsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.TH.Settings", DefaultText = "Settings",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature QuoteTemplateSettingsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SETTINGS", ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.Settings", NameTextCodeDefaultText = "Settings", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode QuoteTemplateEventsTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature QuoteTemplateEventsFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteTemplate.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "QEGC",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = QuoteTemplateGeneralFeature_TH0.Id, ControlPath = "Simplog.QuoteLib.Views.General.GeneralControl", ObjectTableId = QuoteTemplateObjectTable.Id, TabNameTextCodeId = QuoteTemplateGeneralTextCode_TH0.Id, TabNameTextCodeCode = QuoteTemplateGeneralTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "QESE",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = QuoteTemplateSettingsFeature_TH1.Id, ControlPath = "Simplog.QuoteLib.Views.Settings.SettingsControl", ObjectTableId = QuoteTemplateObjectTable.Id, TabNameTextCodeId = QuoteTemplateSettingsTextCode_TH1.Id, TabNameTextCodeCode = QuoteTemplateSettingsTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "QEEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = QuoteTemplateEventsFeature_TH2.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = QuoteTemplateObjectTable.Id, TabNameTextCodeId = QuoteTemplateEventsTextCode_TH2.Id, TabNameTextCodeCode = QuoteTemplateEventsTextCode_TH2.Code, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable QuoteTemplateObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "QuoteTemplate" && d.Tenant == 0).FirstOrDefault(); 

		   Feature QuoteTemplateFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteTemplate.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature QuoteTemplateFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteTemplate.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature QuoteTemplateFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteTemplate.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature QuoteTemplateFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteTemplate.Features.PackageFeature", NameTextCodeDefaultText = "QuoteTemplate Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 

		   		   //--------------> Additional Features <--------------\\

		   Feature QuoteTemplateFeature_COPYATSIGNUP = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "COPYATSIGNUP", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteTemplate.Features.CopyAtSignup", NameTextCodeDefaultText = @"Copy At Signup" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature QuoteTemplateFeature_ENABLEDFORCUSTOMERS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ENABLEDFORCUSTOMERS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteTemplate.Features.EnabledForCustomers", NameTextCodeDefaultText = @"Enabled For Customers" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature QuoteTemplateFeature_FROMLIBRARY = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "FROMLIBRARY", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteTemplate.Features.FromLibrary", NameTextCodeDefaultText = @"Add From Library" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable QuoteTemplateObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "QuoteTemplate" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = QuoteTemplateObjectTable.Id,
				 
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
                ObjectTableId = QuoteTemplateObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable QuoteTemplateObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "QuoteTemplate" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowTotalPerChargeGroup = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowTotalPerChargeGroup", DefaultText = "ShowTotalPerChargeGroup",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSSearch = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Search", DefaultText = "Search",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSPageHeader = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.PageHeader", DefaultText = "Page Header",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSQuoteHeader = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.QuoteHeader", DefaultText = "Quote Header",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSQuoteIntroduction = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.QuoteIntroduction", DefaultText = "Quote Introduction",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSQuoteDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.QuoteDetails", DefaultText = "Quote Details",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSPricingPackages = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.PricingPackages", DefaultText = "Pricing Packages",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSPricingContainers = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.PricingContainers", DefaultText = "Pricing Containers",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSPageFooter = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.PageFooter", DefaultText = "Page Footer",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSSampleText = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.SampleText", DefaultText = "Sample Text",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSSections = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Sections", DefaultText = "Sections",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSTranslation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Translation", DefaultText = "Translation",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSRighttoleft = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Righttoleft", DefaultText = "Right-to-left",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowLocalLanguage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowLocalLanguage", DefaultText = "Show Local Language",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSPricingTableDesign = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.PricingTableDesign", DefaultText = "Pricing Table Design",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSPreviewTemplate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.PreviewTemplate", DefaultText = "Preview Template",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSTotal = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Total", DefaultText = "Total",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSRows = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Rows", DefaultText = "Rows",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowPageBreakBeforeTable = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowPageBreakBeforeTable", DefaultText = "Show Page Break Before Table",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSBorderColor = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.BorderColor", DefaultText = "Border Color",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowFixedPrice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowFixedPrice", DefaultText = "Show Fixed Price",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowPriceByContainer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowPriceByContainer", DefaultText = "Show Price By Container",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSGeneral = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSTotalsDesign = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.TotalsDesign", DefaultText = "Totals  Design",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSTitleDesign = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.TitleDesign", DefaultText = "Title Design",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSPageHeaderSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.PageHeaderSettings", DefaultText = "Page Header Settings",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSQuoteDetailsSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.QuoteDetailsSettings", DefaultText = "Quote Details Settings",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSPricingPackagesSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.PricingPackagesSettings", DefaultText = "Pricing Packages Settings",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSQuoteHeaderSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.QuoteHeaderSettings", DefaultText = "Quote Header Settings",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSPageFooterSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.PageFooterSettings", DefaultText = "Page Footer Settings",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSPricingContainersSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.PricingContainersSettings", DefaultText = "Pricing Containers Settings",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSEditSection = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.EditSection", DefaultText = "Edit Section",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSAddSection = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.AddSection", DefaultText = "Add Section",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSBorderThickness = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.BorderThickness", DefaultText = "Border Thickness",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSColumn1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Column1", DefaultText = "Column 1",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSColumn2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Column2", DefaultText = "Column 2",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSColumn3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Column3", DefaultText = "Column 3",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSNone = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.None", DefaultText = "None",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSLogo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Logo", DefaultText = "Logo",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSText = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Text", DefaultText = "Text",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSColumnsWidthOptions = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ColumnsWidthOptions", DefaultText = "Columns Width Options",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSWidth = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Width", DefaultText = "Width",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSBorderType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.BorderType", DefaultText = "Border Type",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSAll = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.All", DefaultText = "All",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSAvaiLabelFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.AvaiLabelFields", DefaultText = "AvaiLabel Fields",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSBox = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Box", DefaultText = "Box",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSHorizontalOnly = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.HorizontalOnly", DefaultText = "Horizontal Only",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSVerticalOnly = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.VerticalOnly", DefaultText = "Vertical Only",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSCm = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Cm", DefaultText = "Cm",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSHeight = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Height", DefaultText = "Height",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSTextColor = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.TextColor", DefaultText = "Text Color",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSBackgroundColor = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.BackgroundColor", DefaultText = "Background Color",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSDesignTable = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.DesignTable", DefaultText = "Design Table",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSEditArea1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.EditArea1", DefaultText = "Edit Area 1",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSEditArea2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.EditArea2", DefaultText = "Edit Area 2",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSEditArea3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.EditArea3", DefaultText = "Edit Area 3",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSpx = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.px", DefaultText = "px",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSFont = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Font", DefaultText = "Font",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSTablecolumns = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Tablecolumns", DefaultText = "Table columns",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSLabels = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Labels", DefaultText = "Labels",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSTableDesign = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.TableDesign", DefaultText = "Table Design",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSColumnwidthType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ColumnwidthType", DefaultText = "Column width Type",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSFixed = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Fixed", DefaultText = "Fixed",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSAuto = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Auto", DefaultText = "Auto",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSColumn1Labels = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Column1Labels", DefaultText = "Column 1 Labels",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSColumn2Labels = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Column2Labels", DefaultText = "Column 2 Labels",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSPercentage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Percentage", DefaultText = "%",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSColumn1Values = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Column1Values", DefaultText = "Column 1 Values",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSColumn2Values = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Column2Values", DefaultText = "Column 2 Values",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSQuoteLabels = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.QuoteLabels", DefaultText = "Quote Labels",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSEnglishLabel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.EnglishLabel", DefaultText = "English Label",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSLocalLabel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.LocalLabel", DefaultText = "Local Label",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSLabel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Label", DefaultText = "Label",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSValue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Value", DefaultText = "Value",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSBorder = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Border", DefaultText = "Border",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowTitleGeneralDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowTitle(GeneralDetails)", DefaultText = "Show Title ( General Details )",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSNewQuoteTemplate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.NewQuoteTemplate", DefaultText = "New Quote Template",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Name", DefaultText = "Name",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSPricingTable = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.PricingTable", DefaultText = "Pricing Table",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSTotals = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Totals", DefaultText = "Totals",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowTitle = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowTitle", DefaultText = "Show Title",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowPricesTable = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowPricesTable", DefaultText = "Show Prices Table",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowChargeCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowChargeCode", DefaultText = "Show Charge Code",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowChargeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowChargeName", DefaultText = "Show Charge Name",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowMeasurement = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowMeasurement", DefaultText = "Show Measurement",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowUnits = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowUnits", DefaultText = "Show Units",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowUnitPrice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowUnitPrice", DefaultText = "Show Unit Price",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowSaleCurrencyColumn = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowSaleCurrencyColumn", DefaultText = "Show Sale Currency Column",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowLocalCurrencyColumn = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowLocalCurrencyColumn", DefaultText = "Show Local Currency Column",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowChargeDescription = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowChargeDescription", DefaultText = "Show Charge Description",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowChargeNote = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowChargeNote", DefaultText = "Show Charge Notes",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowSaleMinMax = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowSaleMinMax", DefaultText = "Show Sale Min/Max",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSSplitbychargegroup = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Splitbychargegroup", DefaultText = "Split by charge group",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowTotalInLocalCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowTotalInLocalCurrency", DefaultText = "Show Total In Local Currency",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowTotalInSaleCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowTotalInSaleCurrency", DefaultText = "Show Total In Sale Currency",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSHeader = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Header", DefaultText = "Header",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSTitle = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Title", DefaultText = "Title",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSGroupbyDesign = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.GroupbyDesign", DefaultText = "Group by Design",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSTotalPerContainerSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.TotalPerContainerSettings", DefaultText = "Total Per Container Settings",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSTotalPerContainerTable = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.TotalPerContainerTable", DefaultText = "Total Per Container Table",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSTotalPerContainerDesign = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.TotalPerContainerDesign", DefaultText = "Total Per Container Design",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSQuote = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.Quote", DefaultText = "Quote",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateFTemplate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.F.Template", DefaultText = "Template",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "F", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateFLastUpdate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.F.LastUpdate", DefaultText = "LastUpdate",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "F", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateBSection = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.B.Section", DefaultText = "Section",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateBAdd = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.B.Add", DefaultText = "Add",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateBPageBreak = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.B.PageBreak", DefaultText = "Page Break",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateBEdit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.B.Edit", DefaultText = "Edit",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateBSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.B.Settings", DefaultText = "Settings",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateBGeneralSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.B.GeneralSettings", DefaultText = "General Settings",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateBPreview = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.B.Preview", DefaultText = "Preview",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateBCancel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.B.Cancel", DefaultText = "Cancel",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateBSave = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.B.Save", DefaultText = "Save",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateBClose = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.B.Close", DefaultText = "Close",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateBUp = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.B.Up", DefaultText = "Up",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateBDown = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.B.Down", DefaultText = "Down",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateBNew = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.B.New", DefaultText = "New",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateBCopy = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.B.Copy", DefaultText = "Copy",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateBAdvanced = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.B.Advanced", DefaultText = "Advanced",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateBAddDataField = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.B.AddDataField", DefaultText = "Add Data Field",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateBNext = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.B.Next", DefaultText = "Next",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateBFromAllTenant = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.B.FromAllTenant", DefaultText = "From All Tenant",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateBAddtemplatefromlibrary = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.B.Addtemplatefromlibrary", DefaultText = "Add template from library",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateMClicktoaddthephoto = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.M.Clicktoaddthephoto", DefaultText = "Click to add the photo",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateMDeleteSectionConfirmMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.M.DeleteSectionConfirmMessage", DefaultText = "Are you sure you want to delete this section?",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateMPageHeaderDescriptionMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.M.PageHeaderDescriptionMessage", DefaultText = "the header of each page in the quote",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateMQuoteHeaderDescriptionMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.M.QuoteHeaderDescriptionMessage", DefaultText = "the header of the quote",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateMQuoteDetailsDescriptionMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.M.QuoteDetailsDescriptionMessage", DefaultText = "general details of the quote",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateMPricingTableDescriptionMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.M.PricingTableDescriptionMessage", DefaultText = "the sales prices for the quote",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateMPageFooterDescriptionMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.M.PageFooterDescriptionMessage", DefaultText = "the footer of each page in the quote",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateMSaving = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.M.Saving", DefaultText = "Saving...",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateMLoading = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.M.Loading", DefaultText = "Loading...",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateMNoActiveTemplatesFoundMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.M.NoActiveTemplatesFoundMessage", DefaultText = "No active templates found ",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateMValueEditedByUserMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.M.ValueEditedByUserMessage", DefaultText = "Value edited by user",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteQuotationMFileUploadedManuallyMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.Quotation.M.FileUploadedManuallyMessage", DefaultText = "File uploaded manually",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteQuotationMHaveAllTheQuoteTemplatesMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.Quotation.M.HaveAllTheQuoteTemplatesMessage", DefaultText = "You have all the Quote Templates",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowHeaderLabels = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowHeaderLabels", DefaultText = "Show Header Labels",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowTotalSplitToMultipleCurrencies = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowTotalSplitToMultipleCurrencies", DefaultText = "Show Total Split To Multiple Currencies",LocalDefaultText = @"", ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSSpaceLinesBefore = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.SpaceLinesBefore", DefaultText = "Space Lines Before",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSQuoteTemplatePDFMarginTop = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.QuoteTemplatePDFMarginTop", DefaultText = "Margin Top",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSQuoteTemplatePDFMarginBottom = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.QuoteTemplatePDFMarginBottom", DefaultText = "Margin Bottom",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTemplateTextCode_QuoteTemplateSShowIncludedCharges = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteTemplate.S.ShowIncludedCharges", DefaultText = "Show Included Charges",LocalDefaultText = null, ObjectTableId = QuoteTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 