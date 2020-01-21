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
   public class IncotermUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Incoterm",
			      				    IsNew =  false,
			      				    DBTableName =  "Incoterms",
			      				    OldDBTableName =  "Incoterms",
			      				    ObjectTableSingular =  "Incoterm",
			      				    ObjectTablePlural =  "Incoterms",
			      				    DescriptionDefaultText =  "Define delivery terms specifying the default method – collect or prepaid – for freight and other charges.",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  true,
			      				    IsNewWizard =  false,
			      				    LookUp1 =  "Code",
			      				    LookUp2 =  "Name",
			      				    CodeField =  "Code",
			      				    NameField =  "Name",
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  true,
			      				    EditableFromAutoCompleteWindow =  true,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  true,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  true,
			      				    EnableEditFromLOV =  true,
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "Incoterm",
			      				    Code =  "INCO",
			      				    Name =  "Incoterms",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Common",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  true,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    SearchFields =  "Incoterm,Incoterms,,Id,",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Code",
					  						OldFieldName =  "Code",
					  						ObjectTableName =  "Incoterm",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  3,
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
					  						PMPropertyPath =  "Code",
					  						ListPropertyPath =  "Code",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Incoterm",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Code",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  true,
					  						DisplayInSearchWindowFiltersIndex =  1,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
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
					  						HelpTextCode =  "Code",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Name",
					  						OldFieldName =  "Name",
					  						ObjectTableName =  "Incoterm",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
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
					  						DisplayInLookUpIndex =  1,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  1,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Incoterm",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Name",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  true,
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
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Name",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LocalName",
					  						OldFieldName =  "LocalName",
					  						ObjectTableName =  "Incoterm",
					  						FieldsDataType =  "nText",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LocalName",
					  						ListPropertyPath =  "LocalName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Incoterm",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						Code =  "LocalName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LocalName",
					  						DefaultText =  "Local Name",
					  						ListFieldLable =  "LocalNameListLable",
					  						ListLableDefaultText =  "Local Name",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "LocalName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ComputedLocalName",
					  						OldFieldName =  "ComputedLocalName",
					  						ObjectTableName =  "Incoterm",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ComputedLocalName",
					  						ListPropertyPath =  "ComputedLocalName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Incoterm",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ComputedLocalName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ComputedLocalName",
					  						DefaultText =  "Local Name",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ComputedLocalName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Freight",
					  						OldFieldName =  "Freight",
					  						ObjectTableName =  "Incoterm",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "PrepaidCollect",
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
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Freight",
					  						ListPropertyPath =  "Freight",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1Value =  "True",
					  						DependencyFilter1Type =  "Constant",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Incoterm",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Freight",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Freight",
					  						DefaultText =  "Freight",
					  						ListFieldLable =  "FreightListLable",
					  						ListLableDefaultText =  "Freight",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Freight",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OtherCharges",
					  						OldFieldName =  "OtherCharges",
					  						ObjectTableName =  "Incoterm",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "PrepaidCollect",
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
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OtherCharges",
					  						ListPropertyPath =  "OtherCharges",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1Value =  "True",
					  						DependencyFilter1Type =  "Constant",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Incoterm",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "OtherCharges",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OtherCharges",
					  						DefaultText =  "Other Charges",
					  						ListFieldLable =  "OtherChargesListLable",
					  						ListLableDefaultText =  "Other Charges",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "OtherCharges",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AddedManually",
					  						OldFieldName =  "AddedManually",
					  						ObjectTableName =  "Incoterm",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  15,
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
					  						PMPropertyPath =  "AddedManually",
					  						ListPropertyPath =  "AddedManually",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Incoterm",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "AddedManually",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AddedManually",
					  						DefaultText =  "Added Manually",
					  						ListFieldLable =  "AddedManuallyListLable",
					  						ListLableDefaultText =  "Added Manually",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "AddedManually",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InActive",
					  						OldFieldName =  "InActive",
					  						ObjectTableName =  "Incoterm",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  15,
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
					  						ValidForQuerySection1 =  "Incoterm",
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
					  						DefaultText =  "Inactive Incoterm",
					  						ListFieldLable =  "InActiveListLable",
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
					  						HelpTextCode =  "InActive",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Notes",
					  						OldFieldName =  "Notes",
					  						ObjectTableName =  "Incoterm",
					  						FieldsDataType =  "Text",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Notes",
					  						ListPropertyPath =  "Notes",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Incoterm",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Notes",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Notes",
					  						DefaultText =  "Notes",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Notes",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						OldFieldName =  "SearchFields",
					  						ObjectTableName =  "Incoterm",
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
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Incoterm",
					  						ValidForQuerySection2 =  "IncotermFollowUp",
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
					  						DefaultText =  "Search codes/ names",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  "Searching by :\n1: code\n2: english and local names",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PartnerCode",
					  						OldFieldName =  "PartnerCode",
					  						ObjectTableName =  "Incoterm",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PartnerCode",
					  						ListPropertyPath =  "PartnerCode",
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
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PartnerCode",
					  						DefaultText =  "PartnerCode",
					  						ListFieldLable =  "PartnerCodeListLable",
					  						ListLableDefaultText =  "PartnerCode",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
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
	        QueryGroup IncotermQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "INCO", Name = "Incoterms" }, queryGroupRepository);
						QueryGroup IncotermQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "0926", Name = " Query Group" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable IncotermObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Incoterm" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> IncotermObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Incoterm").ToList();   

			   TextCode IncotermTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Incoterm.Q.Incoterms", DefaultText = @"Incoterms",LocalDefaultText = null, ObjectTableId = IncotermObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature IncotermFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "INCOTERMS", ObjectTableId = IncotermObjectTable.Id, Tenant = 0, NameTextCodeCode = "Incoterm.Features.Incoterms", NameTextCodeDefaultText = "Incoterms", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query IncotermsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = IncotermTextCode_0.Id, NameTextCodeCode = IncotermTextCode_0.Code, ObjectTableName = "Incoterm", Code = "Incoterms",  QueryGroupCode = "INCO", IndexOrder = 0, Tenant = 0, ObjectTableId = IncotermObjectTable.Id, QuerySection = "Incoterm", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = IncotermFeature_0.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn IncotermsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = IncotermsQuery.Id,QueryCode = IncotermsQuery.UniqueCode, IndexOrder = 0, ObjectFieldId = IncotermObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == IncotermObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = IncotermObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == IncotermObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn IncotermsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = IncotermsQuery.Id,QueryCode = IncotermsQuery.UniqueCode, IndexOrder = 1, ObjectFieldId = IncotermObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == IncotermObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = IncotermObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == IncotermObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn IncotermsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = IncotermsQuery.Id,QueryCode = IncotermsQuery.UniqueCode, IndexOrder = 2, ObjectFieldId = IncotermObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == IncotermObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = IncotermObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == IncotermObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn IncotermsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = IncotermsQuery.Id,QueryCode = IncotermsQuery.UniqueCode, IndexOrder = 3, ObjectFieldId = IncotermObjectFields.Where(d => d.FieldName == "Freight" && d.ObjectTableId == IncotermObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = IncotermObjectFields.Where(d => d.FieldName == "Freight" && d.ObjectTableId == IncotermObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn IncotermsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = IncotermsQuery.Id,QueryCode = IncotermsQuery.UniqueCode, IndexOrder = 4, ObjectFieldId = IncotermObjectFields.Where(d => d.FieldName == "OtherCharges" && d.ObjectTableId == IncotermObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = IncotermObjectFields.Where(d => d.FieldName == "OtherCharges" && d.ObjectTableId == IncotermObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn IncotermsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = IncotermsQuery.Id,QueryCode = IncotermsQuery.UniqueCode, IndexOrder = 5, ObjectFieldId = IncotermObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTableId == IncotermObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = IncotermObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTableId == IncotermObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn IncotermsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = IncotermsQuery.Id,QueryCode = IncotermsQuery.UniqueCode, IndexOrder = 6, ObjectFieldId = IncotermObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == IncotermObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = IncotermObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == IncotermObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable IncotermObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Incoterm" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> IncotermObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Incoterm").ToList();
		       
	      

	         Screen IncotermGeneralTabScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Incoterm.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = IncotermObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 5, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField IncotermIncotermGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = IncotermObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = IncotermGeneralTabScreenScreen0.Id, ObjectFieldCode = IncotermObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField IncotermIncotermGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = IncotermObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().Id, ScreenId = IncotermGeneralTabScreenScreen0.Id, ObjectFieldCode = IncotermObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField IncotermIncotermGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = IncotermObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().Id, ScreenId = IncotermGeneralTabScreenScreen0.Id, ObjectFieldCode = IncotermObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField IncotermIncotermGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = IncotermObjectFields.Where(d => d.FieldName == "Freight").FirstOrDefault().Id, ScreenId = IncotermGeneralTabScreenScreen0.Id, ObjectFieldCode = IncotermObjectFields.Where(d => d.FieldName == "Freight").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField IncotermIncotermGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 4, ObjectFieldId = IncotermObjectFields.Where(d => d.FieldName == "InActive").FirstOrDefault().Id, ScreenId = IncotermGeneralTabScreenScreen0.Id, ObjectFieldCode = IncotermObjectFields.Where(d => d.FieldName == "InActive").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField IncotermIncotermGeneralTabScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = IncotermObjectFields.Where(d => d.FieldName == "OtherCharges").FirstOrDefault().Id, ScreenId = IncotermGeneralTabScreenScreen0.Id, ObjectFieldCode = IncotermObjectFields.Where(d => d.FieldName == "OtherCharges").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           
	      

	         Screen IncotermHeaderScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Incoterm.HeaderScreen", Name = "Header Screen", ObjectTableId = IncotermObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 3, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField IncotermIncotermHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = IncotermObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = IncotermHeaderScreenScreen1.Id, ObjectFieldCode = IncotermObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField IncotermIncotermHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = IncotermObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().Id, ScreenId = IncotermHeaderScreenScreen1.Id, ObjectFieldCode = IncotermObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField IncotermIncotermHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = IncotermObjectFields.Where(d => d.FieldName == "AddedManually").FirstOrDefault().Id, ScreenId = IncotermHeaderScreenScreen1.Id, ObjectFieldCode = IncotermObjectFields.Where(d => d.FieldName == "AddedManually").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    IncotermObjectTable.HeaderScreenId = IncotermHeaderScreenScreen1.Id;
	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable IncotermObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Incoterm" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode IncotermGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Incoterm.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = IncotermObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature IncotermGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = IncotermObjectTable.Id, Tenant = 0, NameTextCodeCode = "Incoterm.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode IncotermEventsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Incoterm.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = IncotermObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature IncotermEventsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = IncotermObjectTable.Id, Tenant = 0, NameTextCodeCode = "Incoterm.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ITGC",HtmlComponentName = "GeneralTabComponent",HtmlComponentUrl = "./Infrastructure/GenericComponents/GeneralTabComponent", FeatureId = IncotermGeneralFeature_TH0.Id,FeatureUniqeCode = IncotermGeneralFeature_TH0.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = IncotermObjectTable.Id, TabNameTextCodeId = IncotermGeneralTextCode_TH0.Id, TabNameTextCodeCode = IncotermGeneralTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ITEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = IncotermEventsFeature_TH1.Id,FeatureUniqeCode = IncotermEventsFeature_TH1.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = IncotermObjectTable.Id, TabNameTextCodeId = IncotermEventsTextCode_TH1.Id, TabNameTextCodeCode = IncotermEventsTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable IncotermObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Incoterm" && d.Tenant == 0).FirstOrDefault(); 

		   Feature IncotermFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = IncotermObjectTable.Id, Tenant = 0, NameTextCodeCode = "Incoterm.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature IncotermFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = IncotermObjectTable.Id, Tenant = 0, NameTextCodeCode = "Incoterm.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature IncotermFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = IncotermObjectTable.Id, Tenant = 0, NameTextCodeCode = "Incoterm.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature IncotermFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = IncotermObjectTable.Id, Tenant = 0, NameTextCodeCode = "Incoterm.Features.PackageFeature", NameTextCodeDefaultText = "Incoterm Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable IncotermObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Incoterm" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPIT",
                EnglishName =  "Incoterm Updated",
                LocalName =  "Incoterm Updated",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = IncotermObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRIT",
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
                ObjectTableId = IncotermObjectTable.Id,
				 
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
	 