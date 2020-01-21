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
   public class OpportunityUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Opportunity",
			      				    IsNew =  false,
			      				    DBTableName =  "Opportunities",
			      				    OldDBTableName =  "Opportunities",
			      				    ObjectTableSingular =  "Opportunity",
			      				    ObjectTablePlural =  "Opportunities",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  true,
			      				    HasHelper =  true,
			      				    HasShortTitle =  true,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  true,
			      				    KeyPropertyPath =  "Id",
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
			      				    SortingByObjectField =  "Id",
			      				    InActive =  false,
			      				    ShortTitleControlPath =  "",
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  true,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "BR",
			      				    MaxNumberOfCustomFields =  10,
			      				    NewWizardControlName =  "Logitude.CRM.NewOpportunityCommand",
			      				    DefaultText =  "Opportunity",
			      				    Code =  "APPO",
			      				    Name =  "Opportunity",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "CRM",
			      				    NewWizardComponentPath =  "./CRMModules/CRMOpportunity/Components/NewEntity/NewOpportunityComponent",
			      				    NoTS =  false,
			      				    HasMenuButtons =  true,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OwnerId",
					  						OldFieldName =  "OwnerId",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  true,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OwnerId",
					  						ListPropertyPath =  "OwnerId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OwnerId",
					  						DefaultText =  "Owner",
					  						ListFieldLable =  "OwnerIdListLable",
					  						ListLableDefaultText =  "Owner",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Subject",
					  						OldFieldName =  "Subject",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  250,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  true,
					  						SystemMaxLength =  250,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Subject",
					  						ListPropertyPath =  "Subject",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerId",
					  						OldFieldName =  "CustomerId",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  true,
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
					  						DependencyFilter1Value =  "PO,CS",
					  						DependencyFilter2Value =  "True",
					  						DependencyFilter1Type =  "Constant",
					  						DependencyFilter2Type =  "Constant",
					  						DependencyFilter1IsList =  true,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerId",
					  						DefaultText =  "Customer",
					  						ListFieldLable =  "CustomerIdListLable",
					  						ListLableDefaultText =  "Customer",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LeadSourceId",
					  						OldFieldName =  "LeadSourceId",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "LeadSource",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
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
					  						PMPropertyPath =  "LeadSourceId",
					  						ListPropertyPath =  "LeadSourceId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LeadSourceId",
					  						DefaultText =  "Source",
					  						ListFieldLable =  "LeadSourceIdListLable",
					  						ListLableDefaultText =  "Lead Source",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ContactId",
					  						OldFieldName =  "ContactId",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Contact",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
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
					  						PMPropertyPath =  "ContactId",
					  						ListPropertyPath =  "ContactId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1Value =  "CustomerId",
					  						DependencyFilter1Type =  "Path",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContactId",
					  						DefaultText =  "Contact",
					  						ListFieldLable =  "ContactIdListLable",
					  						ListLableDefaultText =  "Contact",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EstimatedClosingDate",
					  						OldFieldName =  "EstimatedClosingDate",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "EstimatedClosingDateDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "EstimatedClosingDate",
					  						ListPropertyPath =  "EstimatedClosingDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EstimatedClosingDate",
					  						DefaultText =  "Est. Closing Date",
					  						ListFieldLable =  "EstimatedClosingDateListLable",
					  						ListLableDefaultText =  "Est. Closing Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StageId",
					  						OldFieldName =  "StageId",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Stage",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  true,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StageId",
					  						ListPropertyPath =  "StageId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StageId",
					  						DefaultText =  "Stage",
					  						ListFieldLable =  "StageIdListLable",
					  						ListLableDefaultText =  "Stage",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Probability",
					  						OldFieldName =  "Probability",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "Probability",
					  						ListPropertyPath =  "Probability",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Probability",
					  						DefaultText =  "Probability (%)",
					  						ListFieldLable =  "ProbabilityListLable",
					  						ListLableDefaultText =  "Probability (%)",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreateDate",
					  						OldFieldName =  "CreateDate",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  true,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "CreateDateDataTemplate",
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
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
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
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByUserId",
					  						OldFieldName =  "CreatedByUserId",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  true,
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
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
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
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdateDate",
					  						OldFieldName =  "UpdateDate",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  true,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "OpportunityUpdateDateDataTemplate",
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
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
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
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByUserId",
					  						OldFieldName =  "UpdatedByUserId",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  true,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
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
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
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
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RatingCode",
					  						OldFieldName =  "RatingCode",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Rating",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  true,
					  						SystemMaxLength =  1,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "RatingCode",
					  						ListPropertyPath =  "RatingCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "RatingCode",
					  						DefaultText =  "Rating",
					  						ListFieldLable =  "RatingCodeListLable",
					  						ListLableDefaultText =  "Rating",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsClosed",
					  						OldFieldName =  "IsClosed",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
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
					  						PMPropertyPath =  "IsClosed",
					  						ListPropertyPath =  "IsClosed",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsClosed",
					  						DefaultText =  "Is Closed",
					  						ListFieldLable =  "IsClosedListLable",
					  						ListLableDefaultText =  "Is Closed",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ActualClosingDate",
					  						OldFieldName =  "ActualClosingDate",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "ActualClosingDate",
					  						ListPropertyPath =  "ActualClosingDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ActualClosingDate",
					  						DefaultText =  "Actual Closing Date",
					  						ListFieldLable =  "ActualClosingDateListLable",
					  						ListLableDefaultText =  "Actual Closing Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ClosingDescription",
					  						OldFieldName =  "ClosingDescription",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  1000,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1000,
					  						DisplayInList =  true,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.RemarkStringConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ClosingDescription",
					  						ListPropertyPath =  "ClosingDescription",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ClosingDescription",
					  						DefaultText =  "Closing Description",
					  						ListFieldLable =  "ClosingDescriptionListLable",
					  						ListLableDefaultText =  "Closing Description",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerName",
					  						OldFieldName =  "CustomerName",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  60,
					  						DisplayInList =  true,
					  						DataTemplateName =  "CustomerNameDataTemplate",
					  						IsCustomFilter =  false,
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
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerName",
					  						DefaultText =  "Customer",
					  						ListFieldLable =  "CustomerNameListLable",
					  						ListLableDefaultText =  "Customer",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						OldFieldName =  "SearchFields",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  1000,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
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
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search Subject / Customer / Contact",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "Search...",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OwnerName",
					  						OldFieldName =  "OwnerName",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  60,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OwnerName",
					  						ListPropertyPath =  "OwnerName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OwnerName",
					  						DefaultText =  "Owner",
					  						ListFieldLable =  "OwnerNameListLable",
					  						ListLableDefaultText =  "Owner",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StageName",
					  						OldFieldName =  "StageName",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  60,
					  						DisplayInList =  true,
					  						DataTemplateName =  "OpportunityStageAgeTemplate",
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StageName",
					  						ListPropertyPath =  "StageName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StageName",
					  						DefaultText =  "Stage",
					  						ListFieldLable =  "StageNameListLable",
					  						ListLableDefaultText =  "Stage",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RatingName",
					  						OldFieldName =  "RatingName",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  60,
					  						DisplayInList =  true,
					  						DataTemplateName =  "OpportunityRatingNameTemplate",
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "RatingName",
					  						ListPropertyPath =  "RatingName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "RatingName",
					  						DefaultText =  "Rating",
					  						ListFieldLable =  "RatingNameListLable",
					  						ListLableDefaultText =  "Rating",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ClosedToCompetitorId",
					  						OldFieldName =  "ClosedToCompetitorId",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
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
					  						PMPropertyPath =  "ClosedToCompetitorId",
					  						ListPropertyPath =  "ClosedToCompetitorId",
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
					  						FullFieldLable =  "ClosedToCompetitorId",
					  						DefaultText =  "ClosedToCompetitorId",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MyOpenOpportunities",
					  						OldFieldName =  "MyOpenOpportunities",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "MyOpenOpportunities",
					  						ListPropertyPath =  "MyOpenOpportunities",
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
					  						FullFieldLable =  "MyOpenOpportunities",
					  						DefaultText =  "My Open Opportunities",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AllOpenOpportunities",
					  						OldFieldName =  "AllOpenOpportunities",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AllOpenOpportunities",
					  						ListPropertyPath =  "AllOpenOpportunities",
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
					  						FullFieldLable =  "AllOpenOpportunities",
					  						DefaultText =  "All Open Opportunities",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MyClosedOpportunities",
					  						OldFieldName =  "MyClosedOpportunities",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "MyClosedOpportunities",
					  						ListPropertyPath =  "MyClosedOpportunities",
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
					  						FullFieldLable =  "MyClosedOpportunities",
					  						DefaultText =  "My Closed Opportunities",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AllClosedOpportunities",
					  						OldFieldName =  "AllClosedOpportunities",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AllClosedOpportunities",
					  						ListPropertyPath =  "AllClosedOpportunities",
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
					  						FullFieldLable =  "AllClosedOpportunities",
					  						DefaultText =  "All Closed Opportunities",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "NumberOfShipments",
					  						OldFieldName =  "NumberOfShipments",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "OpportunityNumberOfShipmentsTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "NumberOfShipments",
					  						ListPropertyPath =  "NumberOfShipments",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "NumberOfShipments",
					  						DefaultText =  "No. Of Shipments",
					  						ListFieldLable =  "NumberOfShipmentsListLable",
					  						ListLableDefaultText =  "No. Of Shipments",
					  						ShortFieldLable =  "NumberOfShipments",
					  						ShortFieldLableDefaultText =  "Shipments",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OpportunityProducts",
					  						OldFieldName =  "OpportunityProducts",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "List",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OpportunityProducts",
					  						ListPropertyPath =  "OpportunityProducts",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "OpportunityProduct",
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
					  						FullFieldLable =  "OpportunityProducts",
					  						DefaultText =  "Opportunity Products",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ValueField",
					  						OldFieldName =  "ValueField",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Decimal",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "ValueField",
					  						ListPropertyPath =  "ValueField",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  18,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ValueField",
					  						DefaultText =  "Value",
					  						ListFieldLable =  "ValueFieldListLable",
					  						ListLableDefaultText =  "Value",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OpportunityCompetitors",
					  						OldFieldName =  "OpportunityCompetitors",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "List",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OpportunityCompetitors",
					  						ListPropertyPath =  "OpportunityCompetitors",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "OpportunityCompetitor",
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
					  						FullFieldLable =  "OpportunityCompetitors",
					  						DefaultText =  "Opportunity Competitors",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastStageDate",
					  						OldFieldName =  "LastStageDate",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
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
					  						PMPropertyPath =  "LastStageDate",
					  						ListPropertyPath =  "LastStageDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LastStageDate",
					  						DefaultText =  "Last Stage Date",
					  						ListFieldLable =  "LastStageDateListLable",
					  						ListLableDefaultText =  "Last Stage Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastStageIdBeforeClosure",
					  						OldFieldName =  "LastStageIdBeforeClosure",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Stage",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
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
					  						PMPropertyPath =  "LastStageIdBeforeClosure",
					  						ListPropertyPath =  "LastStageIdBeforeClosure",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LastStageIdBeforeClosure",
					  						DefaultText =  "Last stage before closure",
					  						ListFieldLable =  "LastStageIdBeforeClosureListLable",
					  						ListLableDefaultText =  "Last stage before closure",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CountryName",
					  						OldFieldName =  "CountryName",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  120,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  120,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CountryName",
					  						ListPropertyPath =  "CountryName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CountryName",
					  						DefaultText =  "Country",
					  						ListFieldLable =  "CountryNameListLable",
					  						ListLableDefaultText =  "Country",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastCompletedActivityDate",
					  						OldFieldName =  "LastCompletedActivityDate",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
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
					  						PMPropertyPath =  "LastCompletedActivityDate",
					  						ListPropertyPath =  "LastCompletedActivityDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LastCompletedActivityDate",
					  						DefaultText =  "Last Activity Date",
					  						ListFieldLable =  "LastCompletedActivityDateListLable",
					  						ListLableDefaultText =  "Last Activity Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OpenByStageOpp",
					  						OldFieldName =  "OpenByStageOpp",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OpenByStageOpp",
					  						ListPropertyPath =  "OpenByStageOpp",
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
					  						FullFieldLable =  "OpenByStageOpp",
					  						DefaultText =  "Open By Stage Opp",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LeadDescription",
					  						OldFieldName =  "LeadDescription",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
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
					  						PMPropertyPath =  "LeadDescription",
					  						ListPropertyPath =  "LeadDescription",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LeadDescription",
					  						DefaultText =  "Description",
					  						ListFieldLable =  "LeadDescriptionListLable",
					  						ListLableDefaultText =  "Lead Description",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastCompletedActivityTypeCode",
					  						OldFieldName =  "LastCompletedActivityTypeCode",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "ActivityType",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  2,
					  						DisplayInList =  true,
					  						DataTemplateName =  "CompletedActivityTypeDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LastCompletedActivityTypeCode",
					  						ListPropertyPath =  "LastCompletedActivityTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LastCompletedActivityTypeCode",
					  						DefaultText =  "Last Activity",
					  						ListFieldLable =  "LastCompletedActivityTypeCodeListLable",
					  						ListLableDefaultText =  "Last Activity",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastActivitySubject",
					  						OldFieldName =  "LastActivitySubject",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  255,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  255,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LastActivitySubject",
					  						ListPropertyPath =  "LastActivitySubject",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LastActivitySubject",
					  						DefaultText =  "Last Activity Subject",
					  						ListFieldLable =  "LastActivitySubjectListLable",
					  						ListLableDefaultText =  "Last Activity Subject",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "NextActivityDate",
					  						OldFieldName =  "NextActivityDate",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						DataTemplateName =  "NextActivityDateTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "NextActivityDate",
					  						ListPropertyPath =  "NextActivityDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "NextActivityDate",
					  						DefaultText =  "Next Activity Date",
					  						ListFieldLable =  "NextActivityDateListLable",
					  						ListLableDefaultText =  "Next Activity Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastCompletedActivityTypeName",
					  						OldFieldName =  "LastCompletedActivityTypeName",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  20,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  20,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LastCompletedActivityTypeName",
					  						ListPropertyPath =  "LastCompletedActivityTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LastCompletedActivityTypeName",
					  						DefaultText =  "Last Completed Activity Type",
					  						ListFieldLable =  "LastCompletedActivityTypeNameListLable",
					  						ListLableDefaultText =  "Last Completed Activity Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IndustryName",
					  						OldFieldName =  "IndustryName",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  60,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IndustryName",
					  						ListPropertyPath =  "IndustryName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IndustryName",
					  						DefaultText =  "Industry",
					  						ListFieldLable =  "IndustryNameListLable",
					  						ListLableDefaultText =  "Industry",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "NextActivityTypeCode",
					  						OldFieldName =  "NextActivityTypeCode",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "ActivityType",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  2,
					  						DisplayInList =  true,
					  						DataTemplateName =  "NextActivityTypeDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "NextActivityTypeCode",
					  						ListPropertyPath =  "NextActivityTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "NextActivityTypeCode",
					  						DefaultText =  "Next Activity",
					  						ListFieldLable =  "NextActivityTypeCodeListLable",
					  						ListLableDefaultText =  "Next Activity",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "NextActivitySubject",
					  						OldFieldName =  "NextActivitySubject",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  255,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  255,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "NextActivitySubject",
					  						ListPropertyPath =  "NextActivitySubject",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "NextActivitySubject",
					  						DefaultText =  "Next Activity Subject",
					  						ListFieldLable =  "NextActivitySubjectListLable",
					  						ListLableDefaultText =  "Next Activity Subject",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "NextActivityTypeName",
					  						OldFieldName =  "NextActivityTypeName",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  20,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  20,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "NextActivityTypeName",
					  						ListPropertyPath =  "NextActivityTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "NextActivityTypeName",
					  						DefaultText =  "Next Activity",
					  						ListFieldLable =  "NextActivityTypeNameListLable",
					  						ListLableDefaultText =  "Next Activity",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Notes",
					  						OldFieldName =  "Notes",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  4000,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  4000,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  true,
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
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Notes",
					  						DefaultText =  "Notes",
					  						ListFieldLable =  "NotesListLable",
					  						ListLableDefaultText =  "Notes",
					  						IsMaxLength =  true,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StageDueDate",
					  						OldFieldName =  "StageDueDate",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "StageDueDateTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StageDueDate",
					  						ListPropertyPath =  "StageDueDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StageDueDate",
					  						DefaultText =  "Stage Due Date",
					  						ListFieldLable =  "StageDueDateListLable",
					  						ListLableDefaultText =  "Stage Due Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BusinessUnitId",
					  						OldFieldName =  "BusinessUnitId",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "BusinessUnit",
					  						MinLength =  0,
					  						MaxLength =  50,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  50,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "BusinessUnitId",
					  						ListPropertyPath =  "BusinessUnitId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BusinessUnitId",
					  						DefaultText =  "Business Unit",
					  						ListFieldLable =  "BusinessUnitIdListLable",
					  						ListLableDefaultText =  "Business Unit",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BusinessUnitName",
					  						OldFieldName =  "BusinessUnitName",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "BusinessUnitName",
					  						ListPropertyPath =  "BusinessUnitName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BusinessUnitName",
					  						DefaultText =  "Business Unit",
					  						ListFieldLable =  "BusinessUnitNameListLable",
					  						ListLableDefaultText =  "Business Unit",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StageProbability",
					  						OldFieldName =  "StageProbability",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StageProbability",
					  						ListPropertyPath =  "StageProbability",
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
					  						FullFieldLable =  "StageProbability",
					  						DefaultText =  "Stage Probability",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RatingIndexOrder",
					  						OldFieldName =  "RatingIndexOrder",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "RatingIndexOrder",
					  						ListPropertyPath =  "RatingIndexOrder",
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
					  						FullFieldLable =  "RatingIndexOrder",
					  						DefaultText =  "Rating Index Order",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LeadUserId",
					  						OldFieldName =  "LeadUserId",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
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
					  						PMPropertyPath =  "LeadUserId",
					  						ListPropertyPath =  "LeadUserId",
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
					  						FullFieldLable =  "LeadUserId",
					  						DefaultText =  "User",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LeadPartnerId",
					  						OldFieldName =  "LeadPartnerId",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
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
					  						PMPropertyPath =  "LeadPartnerId",
					  						ListPropertyPath =  "LeadPartnerId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LeadPartnerId",
					  						DefaultText =  "Partner",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AgentId",
					  						OldFieldName =  "AgentId",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
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
					  						PMPropertyPath =  "AgentId",
					  						ListPropertyPath =  "AgentId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1Value =  "AG",
					  						DependencyFilter1Type =  "Constant",
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
					  						FullFieldLable =  "AgentId",
					  						DefaultText =  "Agent",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ForeignClientId",
					  						OldFieldName =  "ForeignClientId",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
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
					  						PMPropertyPath =  "ForeignClientId",
					  						ListPropertyPath =  "ForeignClientId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1Value =  "CS",
					  						DependencyFilter1Type =  "Constant",
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
					  						FullFieldLable =  "ForeignClientId",
					  						DefaultText =  "Foreign Client",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OpportunityAdditionalServices",
					  						OldFieldName =  "OpportunityAdditionalServices",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "List",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OpportunityAdditionalServices",
					  						ListPropertyPath =  "OpportunityAdditionalServices",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "OpportunityAdditionalService",
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
					  						FullFieldLable =  "OpportunityAdditionalServices",
					  						DefaultText =  "Opportunity Additional Services",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ContactName",
					  						OldFieldName =  "ContactName",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  60,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ContactName",
					  						ListPropertyPath =  "ContactName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContactName",
					  						DefaultText =  "Contact",
					  						ListFieldLable =  "ContactNameListLable",
					  						ListLableDefaultText =  "Contact",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ContactPhone",
					  						OldFieldName =  "ContactPhone",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  25,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  25,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ContactPhone",
					  						ListPropertyPath =  "ContactPhone",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContactPhone",
					  						DefaultText =  "Tel",
					  						ListFieldLable =  "ContactPhoneListLable",
					  						ListLableDefaultText =  "Tel",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ConcurrencyGUID",
					  						OldFieldName =  "ConcurrencyGUID",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ClosingReasonId",
					  						OldFieldName =  "ClosingReasonId",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "OpportunityClosingReason",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
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
					  						PMPropertyPath =  "ClosingReasonId",
					  						ListPropertyPath =  "ClosingReasonId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ClosingReasonId",
					  						DefaultText =  "Closing Reason",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsCancelled",
					  						OldFieldName =  "IsCancelled",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  true,
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
					  						ValidForQuerySection1 =  "Opportunity",
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
					  						ListFieldLable =  "IsCancelledListLable",
					  						ListLableDefaultText =  "Is Cancelled",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CancelledOpportunities",
					  						OldFieldName =  "CancelledOpportunities",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CancelledOpportunities",
					  						ListPropertyPath =  "CancelledOpportunities",
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
					  						FullFieldLable =  "CancelledOpportunities",
					  						DefaultText =  "Cancelled Opportunities",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ClosingReasonName",
					  						OldFieldName =  "ClosingReasonName",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  60,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ClosingReasonName",
					  						ListPropertyPath =  "ClosingReasonName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ClosingReasonName",
					  						DefaultText =  "Closing Reason",
					  						ListFieldLable =  "ClosingReasonNameListLable",
					  						ListLableDefaultText =  "Closing Reason",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LeadSourceName",
					  						OldFieldName =  "LeadSourceName",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  60,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LeadSourceName",
					  						ListPropertyPath =  "LeadSourceName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LeadSourceName",
					  						DefaultText =  "Source",
					  						ListFieldLable =  "LeadSourceNameListLable",
					  						ListLableDefaultText =  "Source",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ContactEmail",
					  						OldFieldName =  "ContactEmail",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  70,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  70,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ContactEmail",
					  						ListPropertyPath =  "ContactEmail",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContactEmail",
					  						DefaultText =  "Contact Email",
					  						ListFieldLable =  "ContactEmailListLable",
					  						ListLableDefaultText =  "Contact Email",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ActivityWatch",
					  						OldFieldName =  "ActivityWatch",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "CustomerActivityWatchDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ActivityWatch",
					  						ListPropertyPath =  "ActivityWatch",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ActivityWatch",
					  						DefaultText =  "Activity Watch",
					  						ListFieldLable =  "ActivityWatchListLable",
					  						ListLableDefaultText =  "Activity Watch",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OpportunityTypeId",
					  						OldFieldName =  "OpportunityTypeId",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "OpportunityType",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  true,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OpportunityTypeId",
					  						ListPropertyPath =  "OpportunityTypeId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OpportunityTypeId",
					  						DefaultText =  "Opportunity Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OpportunityTypeName",
					  						OldFieldName =  "OpportunityTypeName",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OpportunityTypeName",
					  						ListPropertyPath =  "OpportunityTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OpportunityTypeName",
					  						DefaultText =  "Opportunity Type",
					  						ListFieldLable =  "OpportunityTypeNameListLable",
					  						ListLableDefaultText =  "Opportunity Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LeadPartnerName",
					  						OldFieldName =  "LeadPartnerName",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  60,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LeadPartnerName",
					  						ListPropertyPath =  "LeadPartnerName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Opportunity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LeadPartnerName",
					  						DefaultText =  "Partner",
					  						ListFieldLable =  "LeadPartnerNameListLable",
					  						ListLableDefaultText =  "Partner",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsClosedLost",
					  						OldFieldName =  "IsClosedLost",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsClosedLost",
					  						ListPropertyPath =  "IsClosedLost",
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
					  						FullFieldLable =  "IsClosedLost",
					  						DefaultText =  "IsClosedLost",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerRankCode",
					  						OldFieldName =  "CustomerRankCode",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomerRankCode",
					  						ListPropertyPath =  "CustomerRankCode",
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
					  						FullFieldLable =  "CustomerRankCode",
					  						DefaultText =  "CustomerRankCode",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerRankName",
					  						OldFieldName =  "CustomerRankName",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomerRankName",
					  						ListPropertyPath =  "CustomerRankName",
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
					  						FullFieldLable =  "CustomerRankName",
					  						DefaultText =  "CustomerRankName",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PostToFollowersAsWon",
					  						OldFieldName =  "PostToFollowersAsWon",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PostToFollowersAsWon",
					  						ListPropertyPath =  "PostToFollowersAsWon",
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
					  						FullFieldLable =  "PostToFollowersAsWon",
					  						DefaultText =  "PostToFollowersAsWon",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ClosingReasonCode",
					  						OldFieldName =  "ClosingReasonCode",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ClosingReasonCode",
					  						ListPropertyPath =  "ClosingReasonCode",
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
					  						FullFieldLable =  "ClosingReasonCode",
					  						DefaultText =  "ClosingReasonCode",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerExternalId",
					  						OldFieldName =  "CustomerExternalId",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomerExternalId",
					  						ListPropertyPath =  "CustomerExternalId",
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
					  						FullFieldLable =  "CustomerExternalId",
					  						DefaultText =  "CustomerExternalId",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsCopy",
					  						OldFieldName =  "IsCopy",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsCopy",
					  						ListPropertyPath =  "IsCopy",
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
					  						FullFieldLable =  "IsCopy",
					  						DefaultText =  "IsCopy",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CopyFromEntityId",
					  						OldFieldName =  "CopyFromEntityId",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CopyFromEntityId",
					  						ListPropertyPath =  "CopyFromEntityId",
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
					  						FullFieldLable =  "CopyFromEntityId",
					  						DefaultText =  "CopyFromEntityId",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsCustomerBlockedBusinessUnit",
					  						OldFieldName =  "IsCustomerBlockedBusinessUnit",
					  						ObjectTableName =  "Opportunity",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsCustomerBlockedBusinessUnit",
					  						ListPropertyPath =  "IsCustomerBlockedBusinessUnit",
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
					  						FullFieldLable =  "IsCustomerBlockedBusinessUnit",
					  						DefaultText =  "IsCustomerBlockedBusinessUnit",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup OpportunityQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "APPO", Name = "Opportunity" }, queryGroupRepository);
						QueryGroup OpportunityQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "f5c0", Name = " Query Group" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable OpportunityObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Opportunity" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> OpportunityObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Opportunity").ToList();   

			   TextCode OpportunityTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.Q.MyOpenOpportunities", DefaultText = @"My Open Opportunities",LocalDefaultText = null, ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature OpportunityFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Q.MyOpenOpportunities", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.MyOpenOpportunities", NameTextCodeDefaultText = "My Open Opportunities", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode OpportunityTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.Q.AllOpenOpportunities", DefaultText = @"All Open Opportunities",LocalDefaultText = null, ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature OpportunityFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Q.AllOpenOpportunities", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.AllOpenOpportunities", NameTextCodeDefaultText = "All Open Opportunities", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode OpportunityTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.Q.MyClosedOpportunities", DefaultText = @"My Closed Opportunities",LocalDefaultText = null, ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature OpportunityFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Q.MyClosedOpportunities", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.MyClosedOpportunities", NameTextCodeDefaultText = "My Closed Opportunities", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode OpportunityTextCode_3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.Q.AllClosedOpportunities", DefaultText = @"All Closed Opportunities",LocalDefaultText = null, ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature OpportunityFeature_3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Q.AllClosedOpportunities", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.AllClosedOpportunities", NameTextCodeDefaultText = "All Closed Opportunities", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode OpportunityTextCode_4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.Q.AllOpportunities", DefaultText = @"All Opportunities",LocalDefaultText = null, ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature OpportunityFeature_4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Q.AllOpportunities", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.AllOpportunities", NameTextCodeDefaultText = "All Opportunities", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode OpportunityTextCode_5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.Q.CancelledOpportunities", DefaultText = @"Cancelled Opportunities",LocalDefaultText = null, ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature OpportunityFeature_5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Q.CancelledOpportunities", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.CancelledOpportunities", NameTextCodeDefaultText = "Cancelled Opportunities", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode OpportunityTextCode_6 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.Q.OpenByStage", DefaultText = @"Open Opportunities by Stage",LocalDefaultText = null, ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature OpportunityFeature_6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.OpenByStage", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.OpenByStage", NameTextCodeDefaultText = "Open Opportunities by Stage", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query MyOpenOpportunitiesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = OpportunityTextCode_0.Id, NameTextCodeCode = OpportunityTextCode_0.Code, ObjectTableName = "Opportunity", Code = "My Open Opportunities",  QueryGroupCode = "APPO", IndexOrder = 0, Tenant = 0, ObjectTableId = OpportunityObjectTable.Id, QuerySection = "Opportunity", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = OpportunityFeature_0.Id, DefaultSortName = "EstimatedClosingDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn MyOpenOpportunitiesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyOpenOpportunitiesQuery.Id,QueryCode = MyOpenOpportunitiesQuery.UniqueCode, IndexOrder = 0, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyOpenOpportunitiesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyOpenOpportunitiesQuery.Id,QueryCode = MyOpenOpportunitiesQuery.UniqueCode, IndexOrder = 1, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyOpenOpportunitiesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyOpenOpportunitiesQuery.Id,QueryCode = MyOpenOpportunitiesQuery.UniqueCode, IndexOrder = 2, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "EstimatedClosingDate" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "EstimatedClosingDate" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyOpenOpportunitiesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyOpenOpportunitiesQuery.Id,QueryCode = MyOpenOpportunitiesQuery.UniqueCode, IndexOrder = 3, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "ValueField" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "ValueField" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyOpenOpportunitiesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyOpenOpportunitiesQuery.Id,QueryCode = MyOpenOpportunitiesQuery.UniqueCode, IndexOrder = 4, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "Probability" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "Probability" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyOpenOpportunitiesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyOpenOpportunitiesQuery.Id,QueryCode = MyOpenOpportunitiesQuery.UniqueCode, IndexOrder = 5, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "RatingName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "RatingName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyOpenOpportunitiesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyOpenOpportunitiesQuery.Id,QueryCode = MyOpenOpportunitiesQuery.UniqueCode, IndexOrder = 6, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter MyOpenOpportunitiesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "MyOpenOpportunities" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "MyOpenOpportunities" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = MyOpenOpportunitiesQuery.Id,QueryCode = MyOpenOpportunitiesQuery.UniqueCode, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query AllOpenOpportunitiesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = OpportunityTextCode_1.Id, NameTextCodeCode = OpportunityTextCode_1.Code, ObjectTableName = "Opportunity", Code = "All Open Opportunities",  QueryGroupCode = "APPO", IndexOrder = 1, Tenant = 0, ObjectTableId = OpportunityObjectTable.Id, QuerySection = "Opportunity", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = OpportunityFeature_1.Id, DefaultSortName = "EstimatedClosingDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllOpenOpportunitiesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllOpenOpportunitiesQuery.Id,QueryCode = AllOpenOpportunitiesQuery.UniqueCode, IndexOrder = 0, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllOpenOpportunitiesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllOpenOpportunitiesQuery.Id,QueryCode = AllOpenOpportunitiesQuery.UniqueCode, IndexOrder = 1, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllOpenOpportunitiesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllOpenOpportunitiesQuery.Id,QueryCode = AllOpenOpportunitiesQuery.UniqueCode, IndexOrder = 2, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "EstimatedClosingDate" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "EstimatedClosingDate" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllOpenOpportunitiesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllOpenOpportunitiesQuery.Id,QueryCode = AllOpenOpportunitiesQuery.UniqueCode, IndexOrder = 3, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "ValueField" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "ValueField" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllOpenOpportunitiesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllOpenOpportunitiesQuery.Id,QueryCode = AllOpenOpportunitiesQuery.UniqueCode, IndexOrder = 4, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "Probability" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "Probability" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllOpenOpportunitiesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllOpenOpportunitiesQuery.Id,QueryCode = AllOpenOpportunitiesQuery.UniqueCode, IndexOrder = 5, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "RatingName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "RatingName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllOpenOpportunitiesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllOpenOpportunitiesQuery.Id,QueryCode = AllOpenOpportunitiesQuery.UniqueCode, IndexOrder = 6, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter AllOpenOpportunitiesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "AllOpenOpportunities" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "AllOpenOpportunities" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = AllOpenOpportunitiesQuery.Id,QueryCode = AllOpenOpportunitiesQuery.UniqueCode, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query MyClosedOpportunitiesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = OpportunityTextCode_2.Id, NameTextCodeCode = OpportunityTextCode_2.Code, ObjectTableName = "Opportunity", Code = "My Closed Opportunities",  QueryGroupCode = "APPO", IndexOrder = 2, Tenant = 0, ObjectTableId = OpportunityObjectTable.Id, QuerySection = "Opportunity", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = OpportunityFeature_2.Id, DefaultSortName = "EstimatedClosingDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn MyClosedOpportunitiesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyClosedOpportunitiesQuery.Id,QueryCode = MyClosedOpportunitiesQuery.UniqueCode, IndexOrder = 0, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyClosedOpportunitiesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyClosedOpportunitiesQuery.Id,QueryCode = MyClosedOpportunitiesQuery.UniqueCode, IndexOrder = 1, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyClosedOpportunitiesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyClosedOpportunitiesQuery.Id,QueryCode = MyClosedOpportunitiesQuery.UniqueCode, IndexOrder = 2, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "EstimatedClosingDate" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "EstimatedClosingDate" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyClosedOpportunitiesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyClosedOpportunitiesQuery.Id,QueryCode = MyClosedOpportunitiesQuery.UniqueCode, IndexOrder = 3, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "ValueField" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "ValueField" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyClosedOpportunitiesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyClosedOpportunitiesQuery.Id,QueryCode = MyClosedOpportunitiesQuery.UniqueCode, IndexOrder = 4, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "Probability" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "Probability" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyClosedOpportunitiesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyClosedOpportunitiesQuery.Id,QueryCode = MyClosedOpportunitiesQuery.UniqueCode, IndexOrder = 5, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "RatingName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "RatingName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyClosedOpportunitiesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyClosedOpportunitiesQuery.Id,QueryCode = MyClosedOpportunitiesQuery.UniqueCode, IndexOrder = 6, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter MyClosedOpportunitiesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "MyClosedOpportunities" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "MyClosedOpportunities" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = MyClosedOpportunitiesQuery.Id,QueryCode = MyClosedOpportunitiesQuery.UniqueCode, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query AllClosedOpportunitiesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = OpportunityTextCode_3.Id, NameTextCodeCode = OpportunityTextCode_3.Code, ObjectTableName = "Opportunity", Code = "All Closed Opportunities",  QueryGroupCode = "APPO", IndexOrder = 3, Tenant = 0, ObjectTableId = OpportunityObjectTable.Id, QuerySection = "Opportunity", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = OpportunityFeature_3.Id, DefaultSortName = "EstimatedClosingDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllClosedOpportunitiesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllClosedOpportunitiesQuery.Id,QueryCode = AllClosedOpportunitiesQuery.UniqueCode, IndexOrder = 0, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllClosedOpportunitiesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllClosedOpportunitiesQuery.Id,QueryCode = AllClosedOpportunitiesQuery.UniqueCode, IndexOrder = 1, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllClosedOpportunitiesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllClosedOpportunitiesQuery.Id,QueryCode = AllClosedOpportunitiesQuery.UniqueCode, IndexOrder = 2, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "EstimatedClosingDate" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "EstimatedClosingDate" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllClosedOpportunitiesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllClosedOpportunitiesQuery.Id,QueryCode = AllClosedOpportunitiesQuery.UniqueCode, IndexOrder = 3, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "ValueField" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "ValueField" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllClosedOpportunitiesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllClosedOpportunitiesQuery.Id,QueryCode = AllClosedOpportunitiesQuery.UniqueCode, IndexOrder = 4, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "Probability" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "Probability" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllClosedOpportunitiesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllClosedOpportunitiesQuery.Id,QueryCode = AllClosedOpportunitiesQuery.UniqueCode, IndexOrder = 5, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "RatingName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "RatingName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllClosedOpportunitiesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllClosedOpportunitiesQuery.Id,QueryCode = AllClosedOpportunitiesQuery.UniqueCode, IndexOrder = 6, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter AllClosedOpportunitiesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "AllClosedOpportunities" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "AllClosedOpportunities" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = AllClosedOpportunitiesQuery.Id,QueryCode = AllClosedOpportunitiesQuery.UniqueCode, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query AllOpportunitiesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = OpportunityTextCode_4.Id, NameTextCodeCode = OpportunityTextCode_4.Code, ObjectTableName = "Opportunity", Code = "All Opportunities",  QueryGroupCode = "APPO", IndexOrder = 4, Tenant = 0, ObjectTableId = OpportunityObjectTable.Id, QuerySection = "Opportunity", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = OpportunityFeature_4.Id, DefaultSortName = "EstimatedClosingDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllOpportunitiesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllOpportunitiesQuery.Id,QueryCode = AllOpportunitiesQuery.UniqueCode, IndexOrder = 0, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllOpportunitiesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllOpportunitiesQuery.Id,QueryCode = AllOpportunitiesQuery.UniqueCode, IndexOrder = 1, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllOpportunitiesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllOpportunitiesQuery.Id,QueryCode = AllOpportunitiesQuery.UniqueCode, IndexOrder = 2, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "EstimatedClosingDate" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "EstimatedClosingDate" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllOpportunitiesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllOpportunitiesQuery.Id,QueryCode = AllOpportunitiesQuery.UniqueCode, IndexOrder = 3, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "ValueField" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "ValueField" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllOpportunitiesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllOpportunitiesQuery.Id,QueryCode = AllOpportunitiesQuery.UniqueCode, IndexOrder = 4, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "Probability" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "Probability" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllOpportunitiesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllOpportunitiesQuery.Id,QueryCode = AllOpportunitiesQuery.UniqueCode, IndexOrder = 5, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "RatingName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "RatingName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllOpportunitiesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllOpportunitiesQuery.Id,QueryCode = AllOpportunitiesQuery.UniqueCode, IndexOrder = 6, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllOpportunitiesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllOpportunitiesQuery.Id,QueryCode = AllOpportunitiesQuery.UniqueCode, IndexOrder = 7, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "IsClosed" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "IsClosed" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
  
	      

			  Query CancelledOpportunitiesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = OpportunityTextCode_5.Id, NameTextCodeCode = OpportunityTextCode_5.Code, ObjectTableName = "Opportunity", Code = "Cancelled Opportunities",  QueryGroupCode = "APPO", IndexOrder = 5, Tenant = 0, ObjectTableId = OpportunityObjectTable.Id, QuerySection = "Opportunity", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = OpportunityFeature_5.Id, DefaultSortName = "EstimatedClosingDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn CancelledOpportunitiesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledOpportunitiesQuery.Id,QueryCode = CancelledOpportunitiesQuery.UniqueCode, IndexOrder = 0, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledOpportunitiesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledOpportunitiesQuery.Id,QueryCode = CancelledOpportunitiesQuery.UniqueCode, IndexOrder = 1, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledOpportunitiesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledOpportunitiesQuery.Id,QueryCode = CancelledOpportunitiesQuery.UniqueCode, IndexOrder = 2, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "EstimatedClosingDate" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "EstimatedClosingDate" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledOpportunitiesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledOpportunitiesQuery.Id,QueryCode = CancelledOpportunitiesQuery.UniqueCode, IndexOrder = 3, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "ValueField" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "ValueField" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledOpportunitiesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledOpportunitiesQuery.Id,QueryCode = CancelledOpportunitiesQuery.UniqueCode, IndexOrder = 4, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "Probability" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "Probability" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledOpportunitiesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledOpportunitiesQuery.Id,QueryCode = CancelledOpportunitiesQuery.UniqueCode, IndexOrder = 5, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "RatingName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "RatingName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledOpportunitiesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledOpportunitiesQuery.Id,QueryCode = CancelledOpportunitiesQuery.UniqueCode, IndexOrder = 6, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledOpportunitiesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledOpportunitiesQuery.Id,QueryCode = CancelledOpportunitiesQuery.UniqueCode, IndexOrder = 7, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "IsClosed" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "IsClosed" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter CancelledOpportunitiesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "CancelledOpportunities" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "CancelledOpportunities" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = CancelledOpportunitiesQuery.Id,QueryCode = CancelledOpportunitiesQuery.UniqueCode, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query OpenByStageQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = OpportunityTextCode_6.Id, NameTextCodeCode = OpportunityTextCode_6.Code, ObjectTableName = "Opportunity", Code = "Open By Stage",  QueryGroupCode = "APPO", IndexOrder = 6, Tenant = 0, ObjectTableId = OpportunityObjectTable.Id, QuerySection = "Opportunity", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = OpportunityFeature_6.Id, DefaultSortName = "StageDueDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn OpenByStageQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenByStageQuery.Id,QueryCode = OpenByStageQuery.UniqueCode, IndexOrder = 0, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenByStageQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenByStageQuery.Id,QueryCode = OpenByStageQuery.UniqueCode, IndexOrder = 1, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "StageDueDate" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "StageDueDate" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenByStageQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenByStageQuery.Id,QueryCode = OpenByStageQuery.UniqueCode, IndexOrder = 2, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenByStageQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenByStageQuery.Id,QueryCode = OpenByStageQuery.UniqueCode, IndexOrder = 3, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "OwnerName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "OwnerName" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenByStageQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenByStageQuery.Id,QueryCode = OpenByStageQuery.UniqueCode, IndexOrder = 4, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "Subject" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenByStageQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenByStageQuery.Id,QueryCode = OpenByStageQuery.UniqueCode, IndexOrder = 5, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "LastStageDate" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "LastStageDate" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenByStageQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenByStageQuery.Id,QueryCode = OpenByStageQuery.UniqueCode, IndexOrder = 6, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "LastCompletedActivityTypeCode" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "LastCompletedActivityTypeCode" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenByStageQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenByStageQuery.Id,QueryCode = OpenByStageQuery.UniqueCode, IndexOrder = 7, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "LastCompletedActivityDate" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "LastCompletedActivityDate" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenByStageQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenByStageQuery.Id,QueryCode = OpenByStageQuery.UniqueCode, IndexOrder = 8, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "NextActivityTypeCode" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "NextActivityTypeCode" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenByStageQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenByStageQuery.Id,QueryCode = OpenByStageQuery.UniqueCode, IndexOrder = 9, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "NextActivityDate" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "NextActivityDate" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter OpenByStageQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "OpenByStageOpp" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "OpenByStageOpp" && d.ObjectTableId == OpportunityObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = OpenByStageQuery.Id,QueryCode = OpenByStageQuery.UniqueCode, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable OpportunityObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Opportunity" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> OpportunityObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Opportunity").ToList();
		       
	      

	         Screen OpportunityHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Opportunity.HeaderScreen", Name = "Header Screen", ObjectTableId = OpportunityObjectTable.Id, NumberOfColumns = 4, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField OpportunityOpportunityHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "Subject").FirstOrDefault().Id, ScreenId = OpportunityHeaderScreenScreen0.Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "Subject").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField OpportunityOpportunityHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "OwnerName").FirstOrDefault().Id, ScreenId = OpportunityHeaderScreenScreen0.Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "OwnerName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField OpportunityOpportunityHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "ValueField").FirstOrDefault().Id, ScreenId = OpportunityHeaderScreenScreen0.Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "ValueField").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField OpportunityOpportunityHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "RatingName").FirstOrDefault().Id, ScreenId = OpportunityHeaderScreenScreen0.Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "RatingName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField OpportunityOpportunityHeaderScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "StageDueDate").FirstOrDefault().Id, ScreenId = OpportunityHeaderScreenScreen0.Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "StageDueDate").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField OpportunityOpportunityHeaderScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 1, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "StageName").FirstOrDefault().Id, ScreenId = OpportunityHeaderScreenScreen0.Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "StageName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField OpportunityOpportunityHeaderScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "ContactName").FirstOrDefault().Id, ScreenId = OpportunityHeaderScreenScreen0.Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "ContactName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField OpportunityOpportunityHeaderScreenScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 1, ObjectFieldId = OpportunityObjectFields.Where(d => d.FieldName == "ContactPhone").FirstOrDefault().Id, ScreenId = OpportunityHeaderScreenScreen0.Id, ObjectFieldCode = OpportunityObjectFields.Where(d => d.FieldName == "ContactPhone").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    OpportunityObjectTable.HeaderScreenId = OpportunityHeaderScreenScreen0.Id;
	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable OpportunityObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Opportunity" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode OpportunityMainTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.TH.Overview", DefaultText = "Main",LocalDefaultText = null, ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature OpportunityMainFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Tab.Overview", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.Overview", NameTextCodeDefaultText = "Main", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode OpportunityGeneralTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature OpportunityGeneralFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Tab.General", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode OpportunityProductsTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.TH.Products", DefaultText = "Products",LocalDefaultText = null, ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature OpportunityProductsFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Tab.Products", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.Products", NameTextCodeDefaultText = "Products", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode OpportunityDocsOutTextCode_TH3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.TH.DocsOut", DefaultText = "Docs Out",LocalDefaultText = null, ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature OpportunityDocsOutFeature_TH3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Tab.DocsOut", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.DocsOut", NameTextCodeDefaultText = "Docs Out", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode OpportunityDocsInTextCode_TH4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.TH.DocsIn", DefaultText = "Docs In",LocalDefaultText = null, ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature OpportunityDocsInFeature_TH4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Tab.DocsIn", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.DocsIn", NameTextCodeDefaultText = "Docs In", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode OpportunityEventsTextCode_TH5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Opportunity.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature OpportunityEventsFeature_TH5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Tab.Events", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "OPOV",HtmlComponentName = "",HtmlComponentUrl = "./CRMModules/CRMOpportunity/Components/EditTabs/OpportunityOverviewTabComponent", FeatureId = OpportunityMainFeature_TH0.Id,FeatureUniqeCode = OpportunityMainFeature_TH0.FeatureUniqeCode, ControlPath = "Logitude.CRM.Views.Tabs.OpportunityTabs.OverviewTabControl", ObjectTableId = OpportunityObjectTable.Id, TabNameTextCodeId = OpportunityMainTextCode_TH0.Id, TabNameTextCodeCode = OpportunityMainTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "OPGN",HtmlComponentName = "",HtmlComponentUrl = "./CRMModules/CRMOpportunity/Components/EditTabs/OpportunityGeneralTabComponent", FeatureId = OpportunityGeneralFeature_TH1.Id,FeatureUniqeCode = OpportunityGeneralFeature_TH1.FeatureUniqeCode, ControlPath = "Logitude.CRM.Views.Tabs.OpportunityTabs.OpportunityGeneralTabControl", ObjectTableId = OpportunityObjectTable.Id, TabNameTextCodeId = OpportunityGeneralTextCode_TH1.Id, TabNameTextCodeCode = OpportunityGeneralTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "OPPR",HtmlComponentName = "",HtmlComponentUrl = "./CRMModules/CRMOpportunity/Components/EditTabs/OpportunityProductsTabComponent", FeatureId = OpportunityProductsFeature_TH2.Id,FeatureUniqeCode = OpportunityProductsFeature_TH2.FeatureUniqeCode, ControlPath = "Logitude.CRM.Views.Tabs.OpportunityTabs.ProductsTabControl", ObjectTableId = OpportunityObjectTable.Id, TabNameTextCodeId = OpportunityProductsTextCode_TH2.Id, TabNameTextCodeCode = OpportunityProductsTextCode_TH2.Code, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "OPDO",HtmlComponentName = "",HtmlComponentUrl = "./CRMModules/CRMOpportunity/Components/EditTabs/OpportunityDocsOutTabComponent", FeatureId = OpportunityDocsOutFeature_TH3.Id,FeatureUniqeCode = OpportunityDocsOutFeature_TH3.FeatureUniqeCode, ControlPath = "Logitude.CRM.Views.Tabs.OpportunityTabs.DocsOutTabControl", ObjectTableId = OpportunityObjectTable.Id, TabNameTextCodeId = OpportunityDocsOutTextCode_TH3.Id, TabNameTextCodeCode = OpportunityDocsOutTextCode_TH3.Code, Tenant = 0, IndexOrder = 3 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "OPDI",HtmlComponentName = "",HtmlComponentUrl = "./CRMModules/CRMOpportunity/Components/EditTabs/OpportunityDocsInTabComponent", FeatureId = OpportunityDocsInFeature_TH4.Id,FeatureUniqeCode = OpportunityDocsInFeature_TH4.FeatureUniqeCode, ControlPath = "Logitude.CRM.Views.Tabs.OpportunityTabs.DocsInTabControl", ObjectTableId = OpportunityObjectTable.Id, TabNameTextCodeId = OpportunityDocsInTextCode_TH4.Id, TabNameTextCodeCode = OpportunityDocsInTextCode_TH4.Code, Tenant = 0, IndexOrder = 4 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "OPEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = OpportunityEventsFeature_TH5.Id,FeatureUniqeCode = OpportunityEventsFeature_TH5.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = OpportunityObjectTable.Id, TabNameTextCodeId = OpportunityEventsTextCode_TH5.Id, TabNameTextCodeCode = OpportunityEventsTextCode_TH5.Code, Tenant = 0, IndexOrder = 5 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable OpportunityObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Opportunity" && d.Tenant == 0).FirstOrDefault(); 

		   Feature OpportunityFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature OpportunityFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature OpportunityFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature OpportunityFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.PackageFeature", NameTextCodeDefaultText = "Opportunity Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 

		   		   //--------------> Additional Features <--------------\\

		   Feature OpportunityFeature_OUTLOOKCONNETION = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OUTLOOKCONNETION", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.OutlookConnection", NameTextCodeDefaultText = @"Outlook Connection" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature OpportunityFeature_Opportunity_Feature_EXCEL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Opportunity.Feature.EXCEL", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.EXCEL", NameTextCodeDefaultText = @"Download to Excel" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature OpportunityFeature_More = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "More", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.More", NameTextCodeDefaultText = @"More" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable OpportunityObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Opportunity" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = OpportunityObjectTable.Id,
				 
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
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "OPMO",
                EnglishName =  "Email out added",
                LocalName =  "Email out added",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "OPMI",
                EnglishName =  "Email in added",
                LocalName =  "Email in added",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPOP",
                EnglishName =  "Opportunity Updated",
                LocalName =  "Opportunity Updated",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CROP",
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
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "ROOP",
                EnglishName =  "Opportunity re-opened",
                LocalName =  "Opportunity re-opened",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "COOP",
                EnglishName =  "Copied",
                LocalName =  "Copied",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CLOP",
                EnglishName =  "Closed As Lost",
                LocalName =  "Closed As Lost",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CCOP",
                EnglishName =  "Closed As Lost To Competitor",
                LocalName =  "Closed As Lost To Competitor",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CWOP",
                EnglishName =  "Closed As Won",
                LocalName =  "Closed As Won",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CAOP",
                EnglishName =  "Cancelled",
                LocalName =  "Cancelled",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "OEMO",
                EnglishName =  "Opportunity email out sent",
                LocalName =  "Email out sent",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "PROP",
                EnglishName =  "Products Updated",
                LocalName =  "Products Updated",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CMOP",
                EnglishName =  "Competitors Updated",
                LocalName =  "Competitors Updated",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "ADOP",
                EnglishName =  "Additional Services Updated",
                LocalName =  "Additional Services Updated",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "QTOP",
                EnglishName =  "Quotes Updated",
                LocalName =  "Quotes Updated",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "OACR",
                EnglishName =  "Activity Created",
                LocalName =  "Activity Created",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "OACM",
                EnglishName =  "Activity Completed",
                LocalName =  "Activity Completed",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "OARP",
                EnglishName =  "Activity Reopened",
                LocalName =  "Activity Reopened",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "OPSG",
                EnglishName =  "Stage Due Date updated",
                LocalName =  "Stage Due Date updated",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "OPFO",
                EnglishName =  "Opportunity Followed",
                LocalName =  "Opportunity Followed",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "OPST",
                EnglishName =  "Stage Changed",
                LocalName =  "Stage Changed",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = OpportunityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   FeatureRepository featureRepository = new FeatureRepository(0); 
		   //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable OpportunityObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Opportunity" && d.Tenant == 0).FirstOrDefault();       
    
			   Feature OpportunityFeature_MB0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CloseAsWon", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.CloseAsWon", NameTextCodeDefaultText = "Close as Won", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature OpportunityFeature_MB1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CloseAsLost", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.CloseAsLost", NameTextCodeDefaultText = "Close as Lost", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature OpportunityFeature_MB2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TenantManagement", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.TenantManagement", NameTextCodeDefaultText = "Tenant Management", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature OpportunityFeature_MB3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Totango", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.Totango", NameTextCodeDefaultText = "Totango", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature OpportunityFeature_MB4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CREATETENANT", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.CreateTenant", NameTextCodeDefaultText = "Create Tenant", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

			   Feature OpportunityFeature_MB50 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Cancel", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.Cancel", NameTextCodeDefaultText = "Cancel", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature OpportunityFeature_MB51 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ReOpen", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.ReOpen", NameTextCodeDefaultText = "Re-open", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature OpportunityFeature_MB52 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Copy", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.Copy", NameTextCodeDefaultText = "Copy", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature OpportunityFeature_MB53 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Edit.Opportunity", ObjectTableId = OpportunityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Opportunity.Features.EditOpportunity", NameTextCodeDefaultText = "Edit Opportunity", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
              

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup OpportunityMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "OpportunityEdit",
					Name = "OpportunityEditButtonsGroup",
					ObjectTableId = OpportunityObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton OpportunityMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CloseAsWon",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "Opportunity.B.CloseAsWon",
						LabelTextCodeDefaultText = "Close as Won",
						Tenant = 0,
						MenuButtonGroupId = OpportunityMenuButtonGroup.Id,
						ObjectTableId = OpportunityObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = OpportunityFeature_MB0.Id,
						Style = "ApproveButtonStyle",
						LocalDefaultText = null,
						FeatureUniqeCode = OpportunityFeature_MB0.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton OpportunityMenuButton1 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CloseAsLost",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "Opportunity.B.CloseAsLost",
						LabelTextCodeDefaultText = "Close as Lost",
						Tenant = 0,
						MenuButtonGroupId = OpportunityMenuButtonGroup.Id,
						ObjectTableId = OpportunityObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = OpportunityFeature_MB1.Id,
						Style = "RedButtonStyle",
						LocalDefaultText = null,
						FeatureUniqeCode = OpportunityFeature_MB1.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton OpportunityMenuButton2 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "OpportunityTenantManagement",
						Index = 2, 
						IsActive = true,
						LabelTextCodeCode = "Opportunity.B.TenantManagement",
						LabelTextCodeDefaultText = "Manage",
						Tenant = 0,
						MenuButtonGroupId = OpportunityMenuButtonGroup.Id,
						ObjectTableId = OpportunityObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = OpportunityFeature_MB2.Id,
						Style = null,
						LocalDefaultText = null,
						FeatureUniqeCode = OpportunityFeature_MB2.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton OpportunityMenuButton3 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "OpportunityTotango",
						Index = 3, 
						IsActive = true,
						LabelTextCodeCode = "Opportunity.B.Totango",
						LabelTextCodeDefaultText = "Totango",
						Tenant = 0,
						MenuButtonGroupId = OpportunityMenuButtonGroup.Id,
						ObjectTableId = OpportunityObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = OpportunityFeature_MB3.Id,
						Style = null,
						LocalDefaultText = null,
						FeatureUniqeCode = OpportunityFeature_MB3.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton OpportunityMenuButton4 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "OpportunityCreateTenant",
						Index = 4, 
						IsActive = true,
						LabelTextCodeCode = "Opportunity.B.CreateTenant",
						LabelTextCodeDefaultText = "Create Tenant",
						Tenant = 0,
						MenuButtonGroupId = OpportunityMenuButtonGroup.Id,
						ObjectTableId = OpportunityObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = OpportunityFeature_MB4.Id,
						Style = null,
						LocalDefaultText = null,
						FeatureUniqeCode = OpportunityFeature_MB4.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton OpportunityMenuButton5 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "More",
						Index = 100, 
						IsActive = false,
						LabelTextCodeCode = "Opportunity.B.More",
						LabelTextCodeDefaultText = "More",
						Tenant = 0,
						MenuButtonGroupId = OpportunityMenuButtonGroup.Id,
						ObjectTableId = OpportunityObjectTable.Id,
						MenuButtonType = "dropdownbutton",
						FeatureId = null,
						Style = null,
						LocalDefaultText = null,
						FeatureUniqeCode = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

			   MenuButton OpportunityMenuButton50 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Cancel",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "Opportunity.B.Cancel",
						LabelTextCodeDefaultText = "Cancel",
						Tenant = 0,
						MenuButtonGroupId = OpportunityMenuButtonGroup.Id,
						ParentMenuButtonId = OpportunityMenuButton5.Id,
						ObjectTableId = OpportunityObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  OpportunityFeature_MB50.Id,
						Style = null,
						LocalDefaultText = null,
						FeatureUniqeCode=  OpportunityFeature_MB50.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton OpportunityMenuButton51 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "ReOpen",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "Opportunity.B.ReOpen",
						LabelTextCodeDefaultText = "Re-open",
						Tenant = 0,
						MenuButtonGroupId = OpportunityMenuButtonGroup.Id,
						ParentMenuButtonId = OpportunityMenuButton5.Id,
						ObjectTableId = OpportunityObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  OpportunityFeature_MB51.Id,
						Style = null,
						LocalDefaultText = null,
						FeatureUniqeCode=  OpportunityFeature_MB51.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton OpportunityMenuButton52 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Copy",
						Index = 2, 
						IsActive = true,
						LabelTextCodeCode = "Opportunity.B.Copy",
						LabelTextCodeDefaultText = "Copy",
						Tenant = 0,
						MenuButtonGroupId = OpportunityMenuButtonGroup.Id,
						ParentMenuButtonId = OpportunityMenuButton5.Id,
						ObjectTableId = OpportunityObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  OpportunityFeature_MB52.Id,
						Style = null,
						LocalDefaultText = null,
						FeatureUniqeCode=  OpportunityFeature_MB52.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton OpportunityMenuButton53 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Edit",
						Index = 3, 
						IsActive = true,
						LabelTextCodeCode = "Opportunity.B.Edit",
						LabelTextCodeDefaultText = "Edit Opportunity",
						Tenant = 0,
						MenuButtonGroupId = OpportunityMenuButtonGroup.Id,
						ParentMenuButtonId = OpportunityMenuButton5.Id,
						ObjectTableId = OpportunityObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  OpportunityFeature_MB53.Id,
						Style = null,
						LocalDefaultText = null,
						FeatureUniqeCode=  OpportunityFeature_MB53.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {     
	    
}

    

   }
    
}
	 