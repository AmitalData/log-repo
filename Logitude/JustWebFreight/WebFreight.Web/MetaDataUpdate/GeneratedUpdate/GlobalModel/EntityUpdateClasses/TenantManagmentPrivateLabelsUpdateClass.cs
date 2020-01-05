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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.GlobalModel.EntityUpdateClasses
{
   public class TenantManagmentPrivateLabelsUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "TenantManagmentPrivateLabels",
			      				    IsNew =  false,
			      				    DBTableName =  "TenantManagmentPrivateLabels",
			      				    OldDBTableName =  "TenantManagmentPrivateLabels",
			      				    ObjectTableSingular =  "Tenant Managment Private Labels",
			      				    ObjectTablePlural =  "Tenant Managment Private Labels",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  true,
			      				    LookUp1 =  "Id",
			      				    LookUp2 =  "PrivateLabelName",
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
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "Simplog.Infrastructure.Views.TenantManagmentPrivateLabels.AddEditPrivateLabelsControl",
			      				    DefaultText =  "Tenant Managment Private Labels",
			      				    Code =  "TMPL",
			      				    Name =  "Private Labels",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Infrastructure",
			      				    NewWizardComponentPath =  "./InfrastructureModules/InfrastructureTenantManagement/Components/TenantManagement/AddEditPrivateLabelsComponent",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  true,
			      				    SearchFields =  "TenantManagmentPrivateLabels,TenantManagmentPrivateLabels,Simplog.Infrastructure.Views.TenantManagmentPrivateLabels.AddEditPrivateLabelsControl,Id,",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrivateLabelShortName",
					  						OldFieldName =  "PrivateLabelShortName",
					  						ObjectTableName =  "TenantManagmentPrivateLabels",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  true,
					  						CopyToDW =  false,
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
					  						PMPropertyPath =  "PrivateLabelShortName",
					  						ListPropertyPath =  "PrivateLabelShortName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "TenantManagmentPrivateLabels",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "PrivateLabelShortName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  true,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PrivateLabelShortName",
					  						DefaultText =  "Private Label Short Name",
					  						ListFieldLable =  "PrivateLabelShortNameListLable",
					  						ListLableDefaultText =  "Private Label Short Name",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Private Label Short Name",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrivateLabelName",
					  						OldFieldName =  "PrivateLabelName",
					  						ObjectTableName =  "TenantManagmentPrivateLabels",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  true,
					  						CopyToDW =  false,
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
					  						PMPropertyPath =  "PrivateLabelName",
					  						ListPropertyPath =  "PrivateLabelName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "TenantManagmentPrivateLabels",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "PrivateLabelName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  true,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PrivateLabelName",
					  						DefaultText =  "Private Label Name",
					  						ListFieldLable =  "PrivateLabelNameListLable",
					  						ListLableDefaultText =  "Private Label Name",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Private Label Name",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrivateLabelUrl",
					  						OldFieldName =  "PrivateLabelUrl",
					  						ObjectTableName =  "TenantManagmentPrivateLabels",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  true,
					  						CopyToDW =  false,
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
					  						PMPropertyPath =  "PrivateLabelUrl",
					  						ListPropertyPath =  "PrivateLabelUrl",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "TenantManagmentPrivateLabels",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "PrivateLabelUrl",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PrivateLabelUrl",
					  						DefaultText =  "Private Label Url",
					  						ListFieldLable =  "PrivateLabelUrlListLable",
					  						ListLableDefaultText =  "Private Label Url",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Private Label Url",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ContactUsEmail",
					  						OldFieldName =  "ContactUsEmail",
					  						ObjectTableName =  "TenantManagmentPrivateLabels",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  70,
					  						IsRequired =  true,
					  						CopyToDW =  false,
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
					  						PMPropertyPath =  "ContactUsEmail",
					  						ListPropertyPath =  "ContactUsEmaile",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "TenantManagmentPrivateLabels",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ContactUsEmail",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContactUsEmail",
					  						DefaultText =  "Contact Us Email",
					  						ListFieldLable =  "ContactUsEmailListLable",
					  						ListLableDefaultText =  "Contact Us Email",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Contact Us Email",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ReceiveAllStatuses",
					  						OldFieldName =  "ReceiveAllStatuses",
					  						ObjectTableName =  "TenantManagmentPrivateLabels",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						CopyToDW =  false,
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
					  						PMPropertyPath =  "ReceiveAllStatuses",
					  						ListPropertyPath =  "ReceiveAllStatuses",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "TenantManagmentPrivateLabels",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ReceiveAllStatuses",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ReceiveAllStatuses",
					  						DefaultText =  "Receive All Statuses",
					  						ListFieldLable =  "ReceiveAllStatusesListLable",
					  						ListLableDefaultText =  "Receive All Statuses",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Receive All Statuses",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InActive",
					  						OldFieldName =  "InActive",
					  						ObjectTableName =  "TenantManagmentPrivateLabels",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						CopyToDW =  false,
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
					  						ValidForQuerySection1 =  "TenantManagmentPrivateLabels",
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
					  						HelpTextCode =  "In Active",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						OldFieldName =  "SearchFields",
					  						ObjectTableName =  "TenantManagmentPrivateLabels",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  500,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
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
					  						ValidForQuerySection1 =  "TenantManagmentPrivateLabels",
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
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  "Searching by :\n1: Short Name\n2: Full Name",
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
	        QueryGroup TenantManagmentPrivateLabelsQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "TMPL", Name = "Private Labels" }, queryGroupRepository);
						QueryGroup TenantManagmentPrivateLabelsQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "387b", Name = " Query Group" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable TenantManagmentPrivateLabelsObjectTable = objectContext.ObjectTables.Where(d => d.Name == "TenantManagmentPrivateLabels" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> TenantManagmentPrivateLabelsObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "TenantManagmentPrivateLabels").ToList();   

			   TextCode TenantManagmentPrivateLabelsTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TenantManagmentPrivateLabels.Q.PrivateLabels", DefaultText = @"Private Labels",LocalDefaultText = null, ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature TenantManagmentPrivateLabelsFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PrivateLabelsQ", ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id, Tenant = 0, NameTextCodeCode = "TenantManagmentPrivateLabels.Features.PrivateLabels", NameTextCodeDefaultText = "Private Labels", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query PrivateLabelsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = TenantManagmentPrivateLabelsTextCode_0.Id, NameTextCodeCode = TenantManagmentPrivateLabelsTextCode_0.Code, Code = "PrivateLabels",  EditWizardName = "Simplog.Infrastructure.Views.TenantManagmentPrivateLabels.AddEditPrivateLabelsControl",
			   QueryGroupCode = "TMPL", IndexOrder = 0, Tenant = 0, ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id, QuerySection = "TenantManagmentPrivateLabels", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = TenantManagmentPrivateLabelsFeature_0.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn PrivateLabelsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PrivateLabelsQuery.Id, IndexOrder = 0, ObjectFieldId = TenantManagmentPrivateLabelsObjectFields.Where(d => d.FieldName == "PrivateLabelName" && d.ObjectTableId == TenantManagmentPrivateLabelsObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = TenantManagmentPrivateLabelsObjectFields.Where(d => d.FieldName == "PrivateLabelName" && d.ObjectTableId == TenantManagmentPrivateLabelsObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn PrivateLabelsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PrivateLabelsQuery.Id, IndexOrder = 1, ObjectFieldId = TenantManagmentPrivateLabelsObjectFields.Where(d => d.FieldName == "PrivateLabelShortName" && d.ObjectTableId == TenantManagmentPrivateLabelsObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = TenantManagmentPrivateLabelsObjectFields.Where(d => d.FieldName == "PrivateLabelShortName" && d.ObjectTableId == TenantManagmentPrivateLabelsObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn PrivateLabelsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PrivateLabelsQuery.Id, IndexOrder = 2, ObjectFieldId = TenantManagmentPrivateLabelsObjectFields.Where(d => d.FieldName == "PrivateLabelUrl" && d.ObjectTableId == TenantManagmentPrivateLabelsObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = TenantManagmentPrivateLabelsObjectFields.Where(d => d.FieldName == "PrivateLabelUrl" && d.ObjectTableId == TenantManagmentPrivateLabelsObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn PrivateLabelsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PrivateLabelsQuery.Id, IndexOrder = 3, ObjectFieldId = TenantManagmentPrivateLabelsObjectFields.Where(d => d.FieldName == "ContactUsEmail" && d.ObjectTableId == TenantManagmentPrivateLabelsObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = TenantManagmentPrivateLabelsObjectFields.Where(d => d.FieldName == "ContactUsEmail" && d.ObjectTableId == TenantManagmentPrivateLabelsObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn PrivateLabelsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PrivateLabelsQuery.Id, IndexOrder = 4, ObjectFieldId = TenantManagmentPrivateLabelsObjectFields.Where(d => d.FieldName == "ReceiveAllStatuses" && d.ObjectTableId == TenantManagmentPrivateLabelsObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = TenantManagmentPrivateLabelsObjectFields.Where(d => d.FieldName == "ReceiveAllStatuses" && d.ObjectTableId == TenantManagmentPrivateLabelsObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 70 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn PrivateLabelsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PrivateLabelsQuery.Id, IndexOrder = 5, ObjectFieldId = TenantManagmentPrivateLabelsObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == TenantManagmentPrivateLabelsObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = TenantManagmentPrivateLabelsObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == TenantManagmentPrivateLabelsObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 70 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {    

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {      
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable TenantManagmentPrivateLabelsObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "TenantManagmentPrivateLabels" && d.Tenant == 0).FirstOrDefault(); 

		   Feature TenantManagmentPrivateLabelsFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id, Tenant = 0, NameTextCodeCode = "TenantManagmentPrivateLabels.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature TenantManagmentPrivateLabelsFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id, Tenant = 0, NameTextCodeCode = "TenantManagmentPrivateLabels.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature TenantManagmentPrivateLabelsFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id, Tenant = 0, NameTextCodeCode = "TenantManagmentPrivateLabels.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature TenantManagmentPrivateLabelsFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id, Tenant = 0, NameTextCodeCode = "TenantManagmentPrivateLabels.Features.PackageFeature", NameTextCodeDefaultText = "TenantManagmentPrivateLabels Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable TenantManagmentPrivateLabelsObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "TenantManagmentPrivateLabels" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id,
				 
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
                ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id,
				 
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
	 