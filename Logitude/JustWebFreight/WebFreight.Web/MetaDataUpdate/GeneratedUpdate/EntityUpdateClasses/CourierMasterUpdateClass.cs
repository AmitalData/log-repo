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
   public class CourierMasterUpdateClass
   {  		


		public const string HashString = "acaa15ec76ac581759c61c704a95449f";

	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Customs.CourierMaster",
			      				    IsNew =  false,
			      				    DBTableName =  "Customs.CourierMasters",
			      				    ObjectTableSingular =  "CourierMaster",
			      				    ObjectTablePlural =  "CourierMasters",
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
			      				    SortingByObjectField =  "CreateDateTime",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "NewCourierComponent",
			      				    LocalDefaultText =  "×‘×œ×“×¨ ×¨×�×©×™",
			      				    DefaultText =  "Courier Master",
			      				    Code =  "dcc9",
			      				    Name =  "Customs.CourierMaster Query Group",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Customs",
			      				    ServerModuleName =  "Customs",
			      				    NewWizardComponentPath =  "./CustomsModules/CustomsCourier/Components/NewEntity/NewCourierComponent",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    HashString =  CourierMasterUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreateDateTime",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreateDateTime",
					  						DefaultText =  "Create Date",
					  						FullLocalDefaultText =  "×ª×�×¨×™×š ×™×¦×™×¨×”",
					  						ListFieldLable =  "CreateDateTimeListLable",
					  						ListLableDefaultText =  "Create Date",
					  						ListLocalDefaultText =  "×ª×�×¨×™×š ×™×¦×™×¨×”",
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
					 
					 						FieldName =  "CreatedByUserId",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreatedByUserId",
					  						DefaultText =  "Created By Id",
					  						FullLocalDefaultText =  "×ž×©×ª×ž×© ×¤×•×ª×— ×‘×œ×“×¨ ×¨×�×©×™",
					  						ListFieldLable =  "CreatedByUserIdListLable",
					  						ListLableDefaultText =  "CreatedByUserId",
					  						ListLocalDefaultText =  "×ž×©×ª×ž×© ×¤×•×ª×— ×‘×œ×“×¨ ×¨×�×©×™",
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
					 
					 						FieldName =  "UpdateDateTime",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdateDateTime",
					  						DefaultText =  "Update Date",
					  						FullLocalDefaultText =  "×ª×�×¨×™×š ×¢×“×›×•×Ÿ",
					  						ListFieldLable =  "UpdateDateTimeListLable",
					  						ListLableDefaultText =  "Update Date",
					  						ListLocalDefaultText =  "×ª×�×¨×™×š ×¢×“×›×•×Ÿ",
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
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search ...",
					  						FullLocalDefaultText =  "×©×˜×¨ ×ž×˜×¢×Ÿ ×¨×�×©×™/×©×˜×¨ ×ž×˜×¢×Ÿ ×¤× ×™×ž×™/×§×™×“×•×ž×ª ×—×‘×¨×ª ×ª×¢×•×¤×”",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "Search ...",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  "Searching by :",
					  						HelpLocalDefaultText =  "×—×™×¤×•×© ×¢×œ ×™×“×™: ",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
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
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
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
					  						FullLocalDefaultText =  "×ž×©×ª×ž×© ×¤×•×ª×— ×‘×œ×“×¨ ×¨×�×©×™",
					  						ListFieldLable =  "CreatedByUserNameListLable",
					  						ListLableDefaultText =  "Created By",
					  						ListLocalDefaultText =  "×ž×©×ª×ž×© ×¤×•×ª×— ×‘×œ×“×¨ ×¨×�×©×™",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AirlineId",
					  						ObjectTableName =  "Customs.CourierMaster",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Airline",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
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
					  						PMPropertyPath =  "AirlineId",
					  						ListPropertyPath =  "AirlineId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AirlineId",
					  						DefaultText =  "Airline ",
					  						FullLocalDefaultText =  "×§×•×“ ×—×‘×¨×ª ×ª×¢×•×¤×”",
					  						ListFieldLable =  "AirlineIdListLable",
					  						ListLableDefaultText =  "Airline Code",
					  						ListLocalDefaultText =  "×§×•×“ ×—×‘×¨×ª ×ª×¢×•×¤×”",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AirlineName",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "AirlineName",
					  						ListPropertyPath =  "AirlineName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AirlineName",
					  						DefaultText =  "Airline Name",
					  						FullLocalDefaultText =  "×©×� ×—×‘×¨×ª ×ª×¢×•×¤×”",
					  						ListFieldLable =  "AirlineNameListLable",
					  						ListLableDefaultText =  "Airline Name",
					  						ListLocalDefaultText =  "×©×� ×—×‘×¨×ª ×ª×¢×•×¤×”",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
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
					  						ObjectTableName =  "Customs.CourierMaster",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  35,
					  						IsRequired =  true,
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
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
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
					  						FullLocalDefaultText =  "×©×˜×¨ ×ž×˜×¢×Ÿ ×¨×�×©×™",
					  						ListFieldLable =  "MAWBListLable",
					  						ListLableDefaultText =  "MAWB",
					  						ListLocalDefaultText =  "×©×˜×¨ ×ž×˜×¢×Ÿ ×¨×�×©×™",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MAWBTypeName",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "MAWBTypeName",
					  						ListPropertyPath =  "MAWBTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "MAWBTypeName",
					  						DefaultText =  "MAWB Type Name",
					  						FullLocalDefaultText =  "×¡×•×’ ×©×˜×¨ ×ž×˜×¢×Ÿ",
					  						ListFieldLable =  "MAWBTypeNameListLable",
					  						ListLableDefaultText =  "MAWB Type Name",
					  						ListLocalDefaultText =  "×¡×•×’ ×©×˜×¨ ×ž×˜×¢×Ÿ",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "HAWB",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "HAWB",
					  						ListPropertyPath =  "HAWB",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "HAWB",
					  						DefaultText =  "HAWB",
					  						FullLocalDefaultText =  "×©×˜×¨ ×ž×˜×¢×Ÿ ×¤× ×™×ž×™",
					  						ListFieldLable =  "HAWBListLable",
					  						ListLableDefaultText =  "HAWB",
					  						ListLocalDefaultText =  "×©×˜×¨ ×ž×˜×¢×Ÿ ×¤× ×™×ž×™",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EstimatedArrivalDate",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "EstimatedArrivalDate",
					  						ListPropertyPath =  "EstimatedArrivalDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EstimatedArrivalDate",
					  						DefaultText =  "Estimated Arrivel Date",
					  						FullLocalDefaultText =  "×ª×�×¨×™×š ×”×’×¢×” ×ž×©×•×¢×¨",
					  						ListFieldLable =  "EstimatedArrivalDateListLable",
					  						ListLableDefaultText =  "Estimated Arrivel Date",
					  						ListLocalDefaultText =  "×ª×�×¨×™×š ×”×’×¢×” ×ž×©×•×¢×¨",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "GatewayPortCode",
					  						ObjectTableName =  "Customs.CourierMaster",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.InternationalSite",
					  						MinLength =  0,
					  						MaxLength =  17,
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "GatewayPortCode",
					  						ListPropertyPath =  "GatewayPortCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "GatewayPortCode",
					  						DefaultText =  "Gateway Port Code",
					  						FullLocalDefaultText =  "× ×ž×œ ×˜×¢×™× ×”",
					  						ListFieldLable =  "GatewayPortCodeListLable",
					  						ListLableDefaultText =  "Gateway Port Code",
					  						ListLocalDefaultText =  "× ×ž×œ ×˜×¢×™× ×”",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "GatewayPortName",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "GatewayPortName",
					  						ListPropertyPath =  "GatewayPortName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "GatewayPortName",
					  						DefaultText =  "Gateway Port",
					  						FullLocalDefaultText =  "× ×ž×œ ×˜×¢×™× ×”",
					  						ListFieldLable =  "GatewayPortNameListLable",
					  						ListLableDefaultText =  "Gateway Port Name",
					  						ListLocalDefaultText =  "× ×ž×œ ×˜×¢×™× ×”",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OriginPortCode",
					  						ObjectTableName =  "Customs.CourierMaster",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.InternationalSite",
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
					  						PMPropertyPath =  "OriginPortCode",
					  						ListPropertyPath =  "OriginPortCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OriginPortCode",
					  						DefaultText =  "Origin Port Code",
					  						FullLocalDefaultText =  "× ×ž×œ ×ž×•×¦×�",
					  						ListFieldLable =  "OriginPortCodeListLable",
					  						ListLableDefaultText =  "Origin Port Code",
					  						ListLocalDefaultText =  "× ×ž×œ ×ž×•×¦×�",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OriginPortName",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "OriginPortName",
					  						ListPropertyPath =  "OriginPortName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OriginPortName",
					  						DefaultText =  "Origin Port",
					  						FullLocalDefaultText =  "× ×ž×œ ×ž×•×¦×�",
					  						ListFieldLable =  "OriginPortNameListLable",
					  						ListLableDefaultText =  "Origin Port Name",
					  						ListLocalDefaultText =  "× ×ž×œ ×ž×•×¦×�",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsOpen",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "IsOpen",
					  						ListPropertyPath =  "IsOpen",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsOpen",
					  						DefaultText =  "Is Open",
					  						FullLocalDefaultText =  "×¤×ª×•×—",
					  						ListFieldLable =  "IsOpenListLable",
					  						ListLableDefaultText =  "Is Open",
					  						ListLocalDefaultText =  "×¤×ª×•×—",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
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
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
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
					  						FullLocalDefaultText =  "×ž×‘×•×˜×œ",
					  						ListFieldLable =  "IsCancelledListLable",
					  						ListLableDefaultText =  "Is Cancelled",
					  						ListLocalDefaultText =  "×ž×‘×•×˜×œ",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByUserId",
					  						ObjectTableName =  "Customs.CourierMaster",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
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
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
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
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByUserName",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "UpdatedByUserName",
					  						ListPropertyPath =  "UpdatedByUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdatedByUserName",
					  						DefaultText =  "Updated By",
					  						ListFieldLable =  "UpdatedByUserNameListLable",
					  						ListLableDefaultText =  "Updated By",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AirlinePrefix",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "AirlinePrefix",
					  						ListPropertyPath =  "AirlinePrefix",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AirlinePrefix",
					  						DefaultText =  "Airline Code",
					  						FullLocalDefaultText =  "×§×•×“ ×—×‘×¨×ª ×ª×¢×•×¤×”",
					  						ListFieldLable =  "AirlinePrefixListLable",
					  						ListLableDefaultText =  "Airline Code",
					  						ListLocalDefaultText =  "×§×•×“ ×—×‘×¨×ª ×ª×¢×•×¤×”",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SelectedDeclarationChanged",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "SelectedDeclarationChanged",
					  						ListPropertyPath =  "SelectedDeclarationChanged",
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
					  						FullFieldLable =  "SelectedDeclarationChanged",
					  						DefaultText =  "SelectedDeclarationChanged",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
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
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						ObjectTableName =  "Customs.CourierMaster",
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
					 
					 						FieldName =  "MAWBTypeCode",
					  						ObjectTableName =  "Customs.CourierMaster",
					  						FieldsDataType =  "Text",
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
					  						PMPropertyPath =  "MAWBTypeCode",
					  						ListPropertyPath =  "MAWBTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "MAWBTypeCode",
					  						DefaultText =  "MAWB Type",
					  						FullLocalDefaultText =  "×¡×•×’ ×©×˜×¨ ×ž×˜×¢×Ÿ",
					  						ListFieldLable =  "MAWBTypeCodeListLable",
					  						ListLableDefaultText =  "MAWB Type",
					  						ListLocalDefaultText =  "×¡×•×’ ×©×˜×¨ ×ž×˜×¢×Ÿ",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
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
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ManifestNumber",
					  						DefaultText =  "Manifest Number",
					  						FullLocalDefaultText =  "×ž×¡×¤×¨ ×ž×¦×”×¨",
					  						ListFieldLable =  "ManifestNumberListLable",
					  						ListLableDefaultText =  "Manifest Number",
					  						ListLocalDefaultText =  "×ž×¡×¤×¨ ×ž×¦×”×¨",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PackageQuantity",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "PackageQuantity",
					  						ListPropertyPath =  "PackageQuantity",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PackageQuantity",
					  						DefaultText =  "Package Quantity",
					  						FullLocalDefaultText =  "×›×ž×•×ª ×�×¨×™×–×•×ª",
					  						ListFieldLable =  "PackageQuantityListLable",
					  						ListLableDefaultText =  "Package Quantity",
					  						ListLocalDefaultText =  "×›×ž×•×ª ×�×¨×™×–×•×ª",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "GrossMassMeasure",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "GrossMassMeasure",
					  						ListPropertyPath =  "GrossMassMeasure",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
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
					  						FullFieldLable =  "GrossMassMeasure",
					  						DefaultText =  "Gross Weight",
					  						FullLocalDefaultText =  "×ž×©×§×œ ×�×¨×™×–×•×ª",
					  						ListFieldLable =  "GrossMassMeasureListLable",
					  						ListLableDefaultText =  "Gross Weight",
					  						ListLocalDefaultText =  "×ž×©×§×œ ×�×¨×™×–×•×ª",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ShortHAWB",
					  						ObjectTableName =  "Customs.CourierMaster",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  8,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  8,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ShortHAWB",
					  						ListPropertyPath =  "ShortHAWB",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ShortHAWB",
					  						DefaultText =  "Short HAWB",
					  						FullLocalDefaultText =  "×©×˜×¨ ×ž×˜×¢×Ÿ ×¤× ×™×ž×™ ×ž×§×•×¦×¨",
					  						ListFieldLable =  "ShortHAWBListLable",
					  						ListLableDefaultText =  "Short HAWB",
					  						ListLocalDefaultText =  "×©×˜×¨ ×ž×˜×¢×Ÿ ×¤× ×™×ž×™ ×ž×§×•×¦×¨",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FlightNumber",
					  						ObjectTableName =  "Customs.CourierMaster",
					  						FieldsDataType =  "nText",
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
					  						PMPropertyPath =  "FlightNumber",
					  						ListPropertyPath =  "FlightNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FlightNumber",
					  						DefaultText =  "Flight Number",
					  						FullLocalDefaultText =  "×ž×¡×¤×¨ ×˜×™×¡×”",
					  						ListFieldLable =  "FlightNumberListLable",
					  						ListLableDefaultText =  "Flight Number",
					  						ListLocalDefaultText =  "×ž×¡×¤×¨ ×˜×™×¡×”",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DepartureDate",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "DepartureDate",
					  						ListPropertyPath =  "DepartureDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DepartureDate",
					  						DefaultText =  "Departure Date",
					  						FullLocalDefaultText =  "×ª×�×¨×™×š ×”×ž×¨×�×”",
					  						ListFieldLable =  "DepartureDateListLable",
					  						ListLableDefaultText =  "Departure Date",
					  						ListLocalDefaultText =  "×ª×�×¨×™×š ×”×ž×¨×�×”",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EstimatedArrivalDateOnly",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "EstimatedArrivalDateOnly",
					  						ListPropertyPath =  "EstimatedArrivalDateOnly",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EstimatedArrivalDateOnly",
					  						DefaultText =  "Estimated Arrival Date",
					  						FullLocalDefaultText =  "×ª×�×¨×™×š ×”×’×¢×” ×ž×©×•×¢×¨",
					  						ListFieldLable =  "EstimatedArrivalDateOnlyListLable",
					  						ListLableDefaultText =  "Estimated Arrival Date",
					  						ListLocalDefaultText =  "×ª×�×¨×™×š ×”×’×¢×” ×ž×©×•×¢×¨",
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
					 
					 						FieldName =  "EstimatedArrivalTimeOnly",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "EstimatedArrivalTimeOnly",
					  						ListPropertyPath =  "EstimatedArrivalTimeOnly",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EstimatedArrivalTimeOnly",
					  						DefaultText =  "Estimated Arrival Time",
					  						FullLocalDefaultText =  "×©×¢×ª ×”×’×¢×” ×ž×©×•×¢×¨×ª",
					  						ListFieldLable =  "EstimatedArrivalTimeOnlyListLable",
					  						ListLableDefaultText =  "Estimated Arrival Time",
					  						ListLocalDefaultText =  "×©×¢×ª ×”×’×¢×” ×ž×©×•×¢×¨×ª",
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
					 
					 						FieldName =  "WeightValueCode",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "WeightValueCode",
					  						ListPropertyPath =  "WeightValueCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "WeightValueCode",
					  						DefaultText =  "Weight Value Code",
					  						FullLocalDefaultText =  "×¡×•×’ ×ª×©×œ×•×�",
					  						ListFieldLable =  "WeightValueCodeListLable",
					  						ListLableDefaultText =  "Weight Value Code",
					  						ListLocalDefaultText =  "×¡×•×’ ×ª×©×œ×•×�",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
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
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "WeightValueName",
					  						DefaultText =  "Weight Value",
					  						FullLocalDefaultText =  "×¡×•×’ ×ª×©×œ×•×�",
					  						ListFieldLable =  "WeightValueNameListLable",
					  						ListLableDefaultText =  "Weight Value Name",
					  						ListLocalDefaultText =  "×¡×•×’ ×ª×©×œ×•×�",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StorageSiteCode",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
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
					  						FullLocalDefaultText =  "×�×ª×¨ ×�×—×¡×•×Ÿ",
					  						ListFieldLable =  "StorageSiteCodeListLable",
					  						ListLableDefaultText =  "Storage Site Code",
					  						ListLocalDefaultText =  "×�×ª×¨ ×�×—×¡×•×Ÿ",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
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
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
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
					  						FullLocalDefaultText =  "×©×� ×�×ª×¨ ×�×—×¡×•×Ÿ",
					  						ListFieldLable =  "StorageSiteNameListLable",
					  						ListLableDefaultText =  "Storage Site Name",
					  						ListLocalDefaultText =  "×©×� ×�×ª×¨ ×�×—×¡×•×Ÿ",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TruckerId",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "TruckerId",
					  						ListPropertyPath =  "TruckerId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TruckerId",
					  						DefaultText =  "Trucker Id",
					  						FullLocalDefaultText =  "×§×•×“ ×ž×•×‘×™×œ",
					  						ListFieldLable =  "TruckerIdListLable",
					  						ListLableDefaultText =  "Trucker Id",
					  						ListLocalDefaultText =  "×§×•×“ ×ž×•×‘×™×œ",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IntegratorCode",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IntegratorCode",
					  						ListPropertyPath =  "IntegratorCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IntegratorCode",
					  						DefaultText =  "Integrator",
					  						FullLocalDefaultText =  "×�×™× ×˜×’×¨×˜×•×¨",
					  						ListFieldLable =  "IntegratorCodeListLable",
					  						ListLableDefaultText =  "Integrator",
					  						ListLocalDefaultText =  "×�×™× ×˜×’×¨×˜×•×¨",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IntegratorName",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "IntegratorName",
					  						ListPropertyPath =  "IntegratorName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IntegratorName",
					  						DefaultText =  "Integrator",
					  						FullLocalDefaultText =  "×�×™× ×˜×’×¨×˜×•×¨",
					  						ListFieldLable =  "IntegratorNameListLable",
					  						ListLableDefaultText =  "Integrator",
					  						ListLocalDefaultText =  "×�×™× ×˜×’×¨×˜×•×¨",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IntegratorNumber",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "IntegratorNumber",
					  						ListPropertyPath =  "IntegratorNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IntegratorNumber",
					  						DefaultText =  "Integrator",
					  						FullLocalDefaultText =  "×�×™× ×˜×’×¨×˜×•×¨",
					  						ListFieldLable =  "IntegratorNumberListLable",
					  						ListLableDefaultText =  "Integrator",
					  						ListLocalDefaultText =  "×�×™× ×˜×’×¨×˜×•×¨",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsReadyForInvoice",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "IsReadyForInvoice",
					  						ListPropertyPath =  "IsReadyForInvoice",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsReadyForInvoice",
					  						DefaultText =  "is Ready For Invoice",
					  						FullLocalDefaultText =  "×˜×™×¡×” ×›×ž×•×›× ×” ×œ×”×¤×§×ª ×—×©×‘×•× ×™×ª",
					  						ListFieldLable =  "IsReadyForInvoiceListLable",
					  						ListLableDefaultText =  "is Ready For Invoice",
					  						ListLocalDefaultText =  "×˜×™×¡×” ×›×ž×•×›× ×” ×œ×”×¤×§×ª ×—×©×‘×•× ×™×ª",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsAllDecClosedForFollowUp",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "IsAllDecClosedForFollowUp",
					  						ListPropertyPath =  "IsAllDecClosedForFollowUp",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsAllDecClosedForFollowUp",
					  						DefaultText =  "Is All Declaration Closed For Follow Up",
					  						FullLocalDefaultText =  "×©×˜×¨×™ ×ž×˜×¢×Ÿ ×‘×œ×“×¨ ×¤×ª×•×—×™×�",
					  						ListFieldLable =  "IsAllDecClosedForFollowUpListLable",
					  						ListLableDefaultText =  "Is All Declaration Closed For Follow Up",
					  						ListLocalDefaultText =  "×©×˜×¨×™ ×ž×˜×¢×Ÿ ×‘×œ×“×¨ ×¤×ª×•×—×™×�",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CalcClosedForFollowUp",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "CalcClosedForFollowUp",
					  						ListPropertyPath =  "CalcClosedForFollowUp",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CalcClosedForFollowUp",
					  						DefaultText =  "Calc All Closed For Follow Up",
					  						FullLocalDefaultText =  "×›×ž×•×ª ×”×¦×”×¨×•×ª ×¤×ª×•×—×•×ª",
					  						ListFieldLable =  "CalcClosedForFollowUpListLable",
					  						ListLableDefaultText =  "Calc All Closed For Follow Up",
					  						ListLocalDefaultText =  "×›×ž×•×ª ×”×¦×”×¨×•×ª ×¤×ª×•×—×•×ª",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CalcMissingClassification",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "CalcMissingClassification",
					  						ListPropertyPath =  "CalcMissingClassification",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CalcMissingClassification",
					  						DefaultText =  "Calc Courier Missing Classification",
					  						FullLocalDefaultText =  "×›×ž×•×ª ×”×¦×”×¨×•×ª ×œ×œ×� ×¡×™×•×•×’",
					  						ListFieldLable =  "CalcMissingClassificationListLable",
					  						ListLableDefaultText =  "Calc Courier Missing Classification",
					  						ListLocalDefaultText =  "×›×ž×•×ª ×”×¦×”×¨×•×ª ×œ×œ×� ×¡×™×•×•×’",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CalcMissingImporterId",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "CalcMissingImporterId",
					  						ListPropertyPath =  "CalcMissingImporterId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CalcMissingImporterId",
					  						DefaultText =  "Calc Missing ImporterId",
					  						FullLocalDefaultText =  "×›×ž×•×ª ×”×¦×”×¨×•×ª ×œ×œ×� ×ª×–",
					  						ListFieldLable =  "CalcMissingImporterIdListLable",
					  						ListLableDefaultText =  "Calc Missing ImporterId",
					  						ListLocalDefaultText =  "×›×ž×•×ª ×”×¦×”×¨×•×ª ×œ×œ×� ×ª×–",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CalcPendingCustoms",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "CalcPendingCustoms",
					  						ListPropertyPath =  "CalcPendingCustoms",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CalcPendingCustoms",
					  						DefaultText =  "Calc Pending Customs",
					  						FullLocalDefaultText =  "×›×ž×•×ª ×”×¦×”×¨×•×ª ×ž×¢×•×›×‘ ×ž×›×¡",
					  						ListFieldLable =  "CalcPendingCustomsListLable",
					  						ListLableDefaultText =  "Calc Pending Customs",
					  						ListLocalDefaultText =  "×›×ž×•×ª ×”×¦×”×¨×•×ª ×ž×¢×•×›×‘ ×ž×›×¡",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CalcPending900",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "CalcPending900",
					  						ListPropertyPath =  "CalcPending900",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CalcPending900",
					  						DefaultText =  "Calc Pending900",
					  						FullLocalDefaultText =  "×›×ž×•×ª ×”×¦×”×¨×•×ª ×‘×ª×”×œ×™×š ×’×‘×™×”",
					  						ListFieldLable =  "CalcPending900ListLable",
					  						ListLableDefaultText =  "Calc Pending900",
					  						ListLocalDefaultText =  "×›×ž×•×ª ×”×¦×”×¨×•×ª ×‘×ª×”×œ×™×š ×’×‘×™×”",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CalcSuspendedDeclarations",
					  						ObjectTableName =  "Customs.CourierMaster",
					  						FieldsDataType =  "Integer",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CalcSuspendedDeclarations",
					  						ListPropertyPath =  "CalcSuspendedDeclarations",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CalcSuspendedDeclarations",
					  						DefaultText =  "Calc Suspended Declarations",
					  						FullLocalDefaultText =  "×›×ž×•×ª ×”×¦×”×¨×•×ª ×ž×¢×•×›×‘ ×ž×›×¡",
					  						ListFieldLable =  "CalcSuspendedDeclarationsListLable",
					  						ListLableDefaultText =  "Calc Suspended Declarations",
					  						ListLocalDefaultText =  "×›×ž×•×ª ×”×¦×”×¨×•×ª ×ž×¢×•×›×‘ ×ž×›×¡",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "NoOfCourierHawb",
					  						ObjectTableName =  "Customs.CourierMaster",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  5,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  5,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "NoOfCourierHawb",
					  						ListPropertyPath =  "NoOfCourierHawb",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "NoOfCourierHawb",
					  						DefaultText =  "No. Of Courier Hawb",
					  						FullLocalDefaultText =  "×›×ž×•×ª ×©×˜×¨×™ ×ž×˜×¢×Ÿ ×‘×œ×“×¨",
					  						ListFieldLable =  "NoOfCourierHawbListLable",
					  						ListLableDefaultText =  "No. Of Courier Hawb",
					  						ListLocalDefaultText =  "×›×ž×•×ª ×©×˜×¨×™ ×ž×˜×¢×Ÿ ×‘×œ×“×¨",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsAutomaticManifestSent",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "IsAutomaticManifestSent",
					  						ListPropertyPath =  "IsAutomaticManifestSent",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsAutomaticManifestSent",
					  						DefaultText =  "Is Automatic Manifest Sent",
					  						FullLocalDefaultText =  "×”×�×� ×©×•×“×¨ ×ž×¦×”×¨ ×�×•×˜×•×ž×˜×™",
					  						ListFieldLable =  "IsAutomaticManifestSentListLable",
					  						ListLableDefaultText =  "Is Automatic Manifest Sent",
					  						ListLocalDefaultText =  "×”×�×� ×©×•×“×¨ ×ž×¦×”×¨ ×�×•×˜×•×ž×˜×™",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsEstimatedArrivalToDay",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "IsEstimatedArrivalToDay",
					  						ListPropertyPath =  "IsEstimatedArrivalToDay",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsEstimatedArrivalToDay",
					  						DefaultText =  "Is Estimated Arrival today",
					  						FullLocalDefaultText =  "Is Estimated Arrival today",
					  						ListFieldLable =  "IsEstimatedArrivalToDayListLable",
					  						ListLableDefaultText =  "Is Estimated Arrival today",
					  						ListLocalDefaultText =  "Is Estimated Arrival today",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EstimatedArrivalColor",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "EstimatedArrivalColor",
					  						ListPropertyPath =  "EstimatedArrivalColor",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EstimatedArrivalColor",
					  						DefaultText =  "Estimated Arrival Color",
					  						FullLocalDefaultText =  "Estimated Arrival Color",
					  						ListFieldLable =  "EstimatedArrivalColorListLable",
					  						ListLableDefaultText =  "Estimated Arrival Color",
					  						ListLocalDefaultText =  "Estimated Arrival Color",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PackageQuantityInMAWB",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "PackageQuantityInMAWB",
					  						ListPropertyPath =  "PackageQuantityInMAWB",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PackageQuantityInMAWB",
					  						DefaultText =  "PackageQuantityInMAWB",
					  						FullLocalDefaultText =  "×›×ž×•×ª ×§×¨×˜×•× ×™×� ×‘×©.×ž.×¨",
					  						ListFieldLable =  "PackageQuantityInMAWBListLable",
					  						ListLableDefaultText =  "PackageQuantityInMAWB",
					  						ListLocalDefaultText =  "×›×ž×•×ª ×§×¨×˜×•× ×™×� ×‘×©.×ž.×¨",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LandingDate",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "LandingDate",
					  						ListPropertyPath =  "LandingDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LandingDate",
					  						DefaultText =  "Landing Date",
					  						FullLocalDefaultText =  "×ª×�×¨×™×š × ×—×™×ª×”",
					  						ListFieldLable =  "LandingDateListLable",
					  						ListLableDefaultText =  "Landing Date",
					  						ListLocalDefaultText =  "×ª×�×¨×™×š × ×—×™×ª×”",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UnifreightLeadingFile",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UnifreightLeadingFile",
					  						ListPropertyPath =  "UnifreightLeadingFile",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UnifreightLeadingFile",
					  						DefaultText =  "Unifreight Leading File",
					  						FullLocalDefaultText =  "×ª×™×§ ×¢×ž×™×œ×•×ª ×ž×•×‘×™×œ",
					  						ListFieldLable =  "UnifreightLeadingFileListLable",
					  						ListLableDefaultText =  "Unifreight Leading File",
					  						ListLocalDefaultText =  "×ª×™×§ ×¢×ž×™×œ×•×ª ×ž×•×‘×™×œ",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LandingDateDateOnly",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "LandingDateDateOnly",
					  						ListPropertyPath =  "LandingDateDateOnly",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LandingDateDateOnly",
					  						DefaultText =  "Landing Date",
					  						FullLocalDefaultText =  "×ª×�×¨×™×š × ×—×™×ª×”",
					  						ListFieldLable =  "LandingDateDateOnlyListLable",
					  						ListLableDefaultText =  "Landing Date",
					  						ListLocalDefaultText =  "×ª×�×¨×™×š × ×—×™×ª×”",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LandingDateTimeOnly",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "LandingDateTimeOnly",
					  						ListPropertyPath =  "LandingDateTimeOnly",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LandingDateTimeOnly",
					  						DefaultText =  "Landing Date Time ",
					  						FullLocalDefaultText =  "×©×¢×ª × ×—×™×ª×”",
					  						ListFieldLable =  "LandingDateTimeOnlyListLable",
					  						ListLableDefaultText =  "Landing Date Time ",
					  						ListLocalDefaultText =  "×©×¢×ª × ×—×™×ª×”",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CourierMasterRemarks",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CourierMasterRemarks",
					  						ListPropertyPath =  "CourierMasterRemarks",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CourierMasterRemarks",
					  						DefaultText =  "Remarks",
					  						FullLocalDefaultText =  "×”×¢×¨×•×ª",
					  						ListFieldLable =  "CourierMasterRemarksListLable",
					  						ListLableDefaultText =  "Remarks",
					  						ListLocalDefaultText =  "×”×¢×¨×•×ª",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OpenDeclarations",
					  						ObjectTableName =  "Customs.CourierMaster",
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
					  						PMPropertyPath =  "OpenDeclarations",
					  						ListPropertyPath =  "OpenDeclarations",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierMaster",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OpenDeclarations",
					  						DefaultText =  "Open Declarations",
					  						FullLocalDefaultText =  "×”×¦×”×¨×•×ª ×¤×ª×•×—×•×ª",
					  						ListFieldLable =  "OpenDeclarationsListLable",
					  						ListLableDefaultText =  "Open Declarations",
					  						ListLocalDefaultText =  "×”×¦×”×¨×•×ª ×¤×ª×•×—×•×ª",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
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
	        QueryGroup CourierMasterQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "dcc9", Name = "Customs.CourierMaster Query Group" }, queryGroupRepository,tenantQueryGroups);
						QueryGroup CourierMasterQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "4e4d", Name = " Query Group" }, queryGroupRepository,tenantQueryGroups);
				        queryGroupRepository.SubmitChanges();
	        ObjectTable CourierMasterObjectTable = objectTables.ContainsKey("Customs.CourierMaster") ? objectTables["Customs.CourierMaster"] : null;
            if (CourierMasterObjectTable == null)
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(0);  

                CourierMasterObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Customs.CourierMaster" && d.Tenant == 0).FirstOrDefault();
            }

	         
			List<Feature> addedFeatures = new List<Feature>();
			List<TextCode> addedTextCodes = new List<TextCode>();
			List<Query> addedQueries = new List<Query>();
			List<QueryColumn> addedQueryColumns = new List<QueryColumn>();
			List<AdvancedQueryFilter> addedQueryFilters = new List<AdvancedQueryFilter>();
   

			   TextCode CourierMasterTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CourierMaster.Q.OPENCOURIERMASTERS", DefaultText = @"Open Courier Masters",LocalDefaultText = "×˜×™×¡×•×ª ×¤×ª×•×—×•×ª", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature CourierMasterFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CourierMaster.Q.OPENCOURIERMASTERS", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, NameTextCodeCode = "CourierMaster.Features.OPENCOURIERMASTERS", NameTextCodeDefaultText = "OPENCOURIERMASTERS", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,CourierMasterObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode CourierMasterTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CourierMaster.Q.CLOSECOURIERMASTERS", DefaultText = @"Close Courier Masters",LocalDefaultText = "×˜×™×¡×•×ª ×¡×’×•×¨×•×ª", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature CourierMasterFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CourierMaster.Q.CLOSECOURIERMASTERS", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, NameTextCodeCode = "CourierMaster.Features.CLOSECOURIERMASTERS", NameTextCodeDefaultText = "CLOSECOURIERMASTERS", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,CourierMasterObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode CourierMasterTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CourierMaster.Q.ALLCOURIERS", DefaultText = @"All Courier Masters",LocalDefaultText = "×›×œ ×”×˜×™×¡×•×ª", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature CourierMasterFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CourierMaster.Q.ALLCOURIERS", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, NameTextCodeCode = "CourierMaster.Features.ALLCOURIERS", NameTextCodeDefaultText = "ALLCOURIERS", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,CourierMasterObjectTable, addedFeatures, addedTextCodes);

	        //TextCodeRepository.SubmitChanges();
	        //FeaturesRepository.SubmitChanges();    
	      

			  Query OPENCOURIERMASTERSQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CourierMasterTextCode_0.Id, NameTextCodeCode = CourierMasterTextCode_0.Code, ObjectTableName = "Customs.CourierMaster", Code = "OPENCOURIERMASTERS",  QueryGroupCode = "dcc9", IndexOrder = 0, Tenant = 0, ObjectTableId = CourierMasterObjectTable.Id, QuerySection = "Customs.CourierMaster", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CourierMasterFeature_0.Id,FeatureUniqeCode= CourierMasterFeature_0.FeatureUniqeCode, DefaultSortName = "CreateDateTime", DefaultSortDirection = "Desending", Perspective = null }, addedQueries);
	
			 QueryColumn OPENCOURIERMASTERSQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OPENCOURIERMASTERSQuery.Id,QueryCode = OPENCOURIERMASTERSQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.CourierMaster.AirlineName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn OPENCOURIERMASTERSQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OPENCOURIERMASTERSQuery.Id,QueryCode = OPENCOURIERMASTERSQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.CourierMaster.OpenDeclarations" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn OPENCOURIERMASTERSQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OPENCOURIERMASTERSQuery.Id,QueryCode = OPENCOURIERMASTERSQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.CourierMaster.IntegratorName" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn OPENCOURIERMASTERSQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OPENCOURIERMASTERSQuery.Id,QueryCode = OPENCOURIERMASTERSQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.CourierMaster.MAWB" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn OPENCOURIERMASTERSQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OPENCOURIERMASTERSQuery.Id,QueryCode = OPENCOURIERMASTERSQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.CourierMaster.HAWB" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn OPENCOURIERMASTERSQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OPENCOURIERMASTERSQuery.Id,QueryCode = OPENCOURIERMASTERSQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.CourierMaster.GatewayPortName" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn OPENCOURIERMASTERSQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OPENCOURIERMASTERSQuery.Id,QueryCode = OPENCOURIERMASTERSQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.CourierMaster.CreateDateTime" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn OPENCOURIERMASTERSQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OPENCOURIERMASTERSQuery.Id,QueryCode = OPENCOURIERMASTERSQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.CourierMaster.EstimatedArrivalDate" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn OPENCOURIERMASTERSQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OPENCOURIERMASTERSQuery.Id,QueryCode = OPENCOURIERMASTERSQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "Customs.CourierMaster.IsOpen" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn OPENCOURIERMASTERSQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OPENCOURIERMASTERSQuery.Id,QueryCode = OPENCOURIERMASTERSQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "Customs.CourierMaster.IsCancelled" , ColumnWidth = 100 }, addedQueryColumns);

             AdvancedQueryFilter OPENCOURIERMASTERSQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.CourierMaster.IsOpen", PredefinedValue = "true",PredefinedValue2 = null, QueryId = OPENCOURIERMASTERSQuery.Id,QueryCode = OPENCOURIERMASTERSQuery.UniqueCode, Tenant = 0}, addedQueryFilters);


             AdvancedQueryFilter OPENCOURIERMASTERSQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.CourierMaster.IsCancelled", PredefinedValue = "0",PredefinedValue2 = null, QueryId = OPENCOURIERMASTERSQuery.Id,QueryCode = OPENCOURIERMASTERSQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query CLOSECOURIERMASTERSQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CourierMasterTextCode_1.Id, NameTextCodeCode = CourierMasterTextCode_1.Code, ObjectTableName = "Customs.CourierMaster", Code = "CLOSECOURIERMASTERS",  QueryGroupCode = "dcc9", IndexOrder = 1, Tenant = 0, ObjectTableId = CourierMasterObjectTable.Id, QuerySection = "Customs.CourierMaster", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CourierMasterFeature_1.Id,FeatureUniqeCode= CourierMasterFeature_1.FeatureUniqeCode, DefaultSortName = "CreateDateTime", DefaultSortDirection = "Desending", Perspective = null }, addedQueries);
	
			 QueryColumn CLOSECOURIERMASTERSQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CLOSECOURIERMASTERSQuery.Id,QueryCode = CLOSECOURIERMASTERSQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.CourierMaster.AirlineName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn CLOSECOURIERMASTERSQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CLOSECOURIERMASTERSQuery.Id,QueryCode = CLOSECOURIERMASTERSQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.CourierMaster.OpenDeclarations" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn CLOSECOURIERMASTERSQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CLOSECOURIERMASTERSQuery.Id,QueryCode = CLOSECOURIERMASTERSQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.CourierMaster.IntegratorName" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn CLOSECOURIERMASTERSQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CLOSECOURIERMASTERSQuery.Id,QueryCode = CLOSECOURIERMASTERSQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.CourierMaster.MAWB" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn CLOSECOURIERMASTERSQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CLOSECOURIERMASTERSQuery.Id,QueryCode = CLOSECOURIERMASTERSQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.CourierMaster.HAWB" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn CLOSECOURIERMASTERSQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CLOSECOURIERMASTERSQuery.Id,QueryCode = CLOSECOURIERMASTERSQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.CourierMaster.GatewayPortName" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn CLOSECOURIERMASTERSQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CLOSECOURIERMASTERSQuery.Id,QueryCode = CLOSECOURIERMASTERSQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.CourierMaster.CreateDateTime" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn CLOSECOURIERMASTERSQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CLOSECOURIERMASTERSQuery.Id,QueryCode = CLOSECOURIERMASTERSQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.CourierMaster.EstimatedArrivalDate" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn CLOSECOURIERMASTERSQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CLOSECOURIERMASTERSQuery.Id,QueryCode = CLOSECOURIERMASTERSQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "Customs.CourierMaster.IsOpen" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn CLOSECOURIERMASTERSQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CLOSECOURIERMASTERSQuery.Id,QueryCode = CLOSECOURIERMASTERSQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "Customs.CourierMaster.IsCancelled" , ColumnWidth = 100 }, addedQueryColumns);

             AdvancedQueryFilter CLOSECOURIERMASTERSQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.CourierMaster.IsOpen", PredefinedValue = "false",PredefinedValue2 = null, QueryId = CLOSECOURIERMASTERSQuery.Id,QueryCode = CLOSECOURIERMASTERSQuery.UniqueCode, Tenant = 0}, addedQueryFilters);


             AdvancedQueryFilter CLOSECOURIERMASTERSQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.CourierMaster.IsCancelled", PredefinedValue = "0",PredefinedValue2 = null, QueryId = CLOSECOURIERMASTERSQuery.Id,QueryCode = CLOSECOURIERMASTERSQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query ALLCOURIERSQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CourierMasterTextCode_2.Id, NameTextCodeCode = CourierMasterTextCode_2.Code, ObjectTableName = "Customs.CourierMaster", Code = "ALLCOURIERS",  QueryGroupCode = "dcc9", IndexOrder = 2, Tenant = 0, ObjectTableId = CourierMasterObjectTable.Id, QuerySection = "Customs.CourierMaster", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CourierMasterFeature_2.Id,FeatureUniqeCode= CourierMasterFeature_2.FeatureUniqeCode, DefaultSortName = "CreateDateTime", DefaultSortDirection = "Desending", Perspective = null }, addedQueries);
	
			 QueryColumn ALLCOURIERSQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ALLCOURIERSQuery.Id,QueryCode = ALLCOURIERSQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.CourierMaster.AirlineName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn ALLCOURIERSQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ALLCOURIERSQuery.Id,QueryCode = ALLCOURIERSQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.CourierMaster.OpenDeclarations" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ALLCOURIERSQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ALLCOURIERSQuery.Id,QueryCode = ALLCOURIERSQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.CourierMaster.IntegratorName" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn ALLCOURIERSQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ALLCOURIERSQuery.Id,QueryCode = ALLCOURIERSQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.CourierMaster.MAWB" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn ALLCOURIERSQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ALLCOURIERSQuery.Id,QueryCode = ALLCOURIERSQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.CourierMaster.HAWB" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn ALLCOURIERSQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ALLCOURIERSQuery.Id,QueryCode = ALLCOURIERSQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.CourierMaster.GatewayPortName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn ALLCOURIERSQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ALLCOURIERSQuery.Id,QueryCode = ALLCOURIERSQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.CourierMaster.CreateDateTime" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ALLCOURIERSQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ALLCOURIERSQuery.Id,QueryCode = ALLCOURIERSQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.CourierMaster.EstimatedArrivalDate" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn ALLCOURIERSQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ALLCOURIERSQuery.Id,QueryCode = ALLCOURIERSQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "Customs.CourierMaster.IsOpen" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn ALLCOURIERSQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ALLCOURIERSQuery.Id,QueryCode = ALLCOURIERSQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "Customs.CourierMaster.IsCancelled" , ColumnWidth = 100 }, addedQueryColumns);
			SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
			SqlBulkInsert.BulkInsert("Features", addedFeatures);
			SqlBulkInsert.BulkInsert("Queries", addedQueries);
			SqlBulkInsert.BulkInsert("QueryColumns", addedQueryColumns);
			SqlBulkInsert.BulkInsert("AdvancedQueryFilters", addedQueryFilters);	 
  
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable CourierMasterObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CourierMaster" && d.Tenant == 0).FirstOrDefault();
		   //List<ObjectField> CourierMasterObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.CourierMaster").ToList();
		       
	      

	         Screen CourierMasterCustomsCourierMasterHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CourierMaster.HeaderScreen", Name = "Customs.CourierMasterHeaderScreen", ObjectTableId = CourierMasterObjectTable.Id, NumberOfColumns = 4, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
             ScreenField CustomsCourierMasterCustomsCourierMasterHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ScreenId = CourierMasterCustomsCourierMasterHeaderScreenScreen0.Id,ScreenCode = CourierMasterCustomsCourierMasterHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.CourierMaster.AirlineName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsCourierMasterCustomsCourierMasterHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ScreenId = CourierMasterCustomsCourierMasterHeaderScreenScreen0.Id,ScreenCode = CourierMasterCustomsCourierMasterHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.CourierMaster.EstimatedArrivalDate", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsCourierMasterCustomsCourierMasterHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ScreenId = CourierMasterCustomsCourierMasterHeaderScreenScreen0.Id,ScreenCode = CourierMasterCustomsCourierMasterHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.CourierMaster.MAWB", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsCourierMasterCustomsCourierMasterHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ScreenId = CourierMasterCustomsCourierMasterHeaderScreenScreen0.Id,ScreenCode = CourierMasterCustomsCourierMasterHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.CourierMaster.HAWB", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsCourierMasterCustomsCourierMasterHeaderScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ScreenId = CourierMasterCustomsCourierMasterHeaderScreenScreen0.Id,ScreenCode = CourierMasterCustomsCourierMasterHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.CourierMaster.GatewayPortName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsCourierMasterCustomsCourierMasterHeaderScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 1, ScreenId = CourierMasterCustomsCourierMasterHeaderScreenScreen0.Id,ScreenCode = CourierMasterCustomsCourierMasterHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.CourierMaster.CreateDateTime", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsCourierMasterCustomsCourierMasterHeaderScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ScreenId = CourierMasterCustomsCourierMasterHeaderScreenScreen0.Id,ScreenCode = CourierMasterCustomsCourierMasterHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.CourierMaster.CreatedByUserName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          	
		    CourierMasterObjectTable.HeaderScreenId = CourierMasterCustomsCourierMasterHeaderScreenScreen0.Id;
		    CourierMasterObjectTable.HeaderScreenCode = CourierMasterCustomsCourierMasterHeaderScreenScreen0.Code;

	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable CourierMasterObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CourierMaster" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode CourierMasterGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.TH.General", DefaultText = "General",LocalDefaultText = "×›×œ×œ×™", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CourierMasterGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CourierMaster.Tab.General", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, NameTextCodeCode = "CourierMaster.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,CourierMasterObjectTable);
 
                 
			   TextCode CourierMasterConnectedDeclarationTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.TH.ConnectedDeclaration", DefaultText = "Connected Declaration",LocalDefaultText = "×”×¦×”×¨×•×ª ×ž×§×•×©×¨×•×ª", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CourierMasterConnectedDeclarationFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CourierMaster.Tab.ConnectedDeclaration", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, NameTextCodeCode = "CourierMaster.Features.ConnectedDeclaration", NameTextCodeDefaultText = "Connected Declaration", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,CourierMasterObjectTable);
 
                 
			   TextCode CourierMasterEventsTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.TH.Events", DefaultText = "Events",LocalDefaultText = "×�×™×¨×•×¢×™×�", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CourierMasterEventsFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CourierMaster.Tab.Events", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, NameTextCodeCode = "CourierMaster.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,CourierMasterObjectTable);
 
                 
			   TextCode CourierMasterCommunicationsTextCode_TH3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.TH.Communications", DefaultText = "Communications",LocalDefaultText = "×ª×§×©×•×¨×•×ª", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CourierMasterCommunicationsFeature_TH3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CourierMaster.Tab.Communications", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, NameTextCodeCode = "CourierMaster.Features.Communications", NameTextCodeDefaultText = "Communications", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,CourierMasterObjectTable);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "COGN",HtmlComponentName = "CourierMasterGeneralTabComponent",HtmlComponentUrl = "./CustomsModules/CustomsCourier/Components/EditTabs/CourierMasterGeneralTabComponent", FeatureId = CourierMasterGeneralFeature_TH0.Id,FeatureUniqeCode = CourierMasterGeneralFeature_TH0.FeatureUniqeCode, ControlPath = "CourierMasterGeneralTabComponent", ObjectTableId = CourierMasterObjectTable.Id, TabNameTextCodeId = CourierMasterGeneralTextCode_TH0.Id, TabNameTextCodeCode = CourierMasterGeneralTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "COCD",HtmlComponentName = "CMConnectedDeclarationTabComponent",HtmlComponentUrl = "./CustomsModules/CustomsCourier/Components/EditTabs/CMConnectedDeclarationTabComponent", FeatureId = CourierMasterConnectedDeclarationFeature_TH1.Id,FeatureUniqeCode = CourierMasterConnectedDeclarationFeature_TH1.FeatureUniqeCode, ControlPath = "ConnectedDeclarationTabComponent", ObjectTableId = CourierMasterObjectTable.Id, TabNameTextCodeId = CourierMasterConnectedDeclarationTextCode_TH1.Id, TabNameTextCodeCode = CourierMasterConnectedDeclarationTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "COME",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = CourierMasterEventsFeature_TH2.Id,FeatureUniqeCode = CourierMasterEventsFeature_TH2.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = CourierMasterObjectTable.Id, TabNameTextCodeId = CourierMasterEventsTextCode_TH2.Id, TabNameTextCodeCode = CourierMasterEventsTextCode_TH2.Code, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "COCM",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = CourierMasterCommunicationsFeature_TH3.Id,FeatureUniqeCode = CourierMasterCommunicationsFeature_TH3.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Communications.CommunicationsControl", ObjectTableId = CourierMasterObjectTable.Id, TabNameTextCodeId = CourierMasterCommunicationsTextCode_TH3.Id, TabNameTextCodeCode = CourierMasterCommunicationsTextCode_TH3.Code, Tenant = 0, IndexOrder = 3 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable CourierMasterObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CourierMaster" && d.Tenant == 0).FirstOrDefault(); 

		   Feature CourierMasterFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, NameTextCodeCode = "CourierMaster.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,CourierMasterObjectTable);
		   Feature CourierMasterFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, NameTextCodeCode = "CourierMaster.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,CourierMasterObjectTable);
		   Feature CourierMasterFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, NameTextCodeCode = "CourierMaster.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,CourierMasterObjectTable);
		   Feature CourierMasterFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = false, ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, NameTextCodeCode = "CourierMaster.Features.PackageFeature", NameTextCodeDefaultText = "CourierMaster Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,CourierMasterObjectTable); 

		   		   //--------------> Additional Features <--------------\\

		   Feature CourierMasterFeature_AllowAccounting = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AllowAccounting", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.CourierMaster.Features.Allow Accounting", NameTextCodeDefaultText = @"Allow Accounting" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,CourierMasterObjectTable);

		   Feature CourierMasterFeature_SendManifest = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SendManifest", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.CourierMaster.Features.SendManifest", NameTextCodeDefaultText = @"SendAutoManifest" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,CourierMasterObjectTable);

		   Feature CourierMasterFeature_SendDeclaration = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SendDeclaration", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.CourierMaster.Features.SendDeclaration", NameTextCodeDefaultText = @"SendAutoDeclaration" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,CourierMasterObjectTable);

		   Feature CourierMasterFeature_SendDeclaration902 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SendDeclaration902", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.CourierMaster.Features.SendDeclaration902", NameTextCodeDefaultText = @"Send Declaration After Deleting Pending 902" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,CourierMasterObjectTable);

		   Feature CourierMasterFeature_StatusDeclarationOldVersion = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "StatusDeclarationOldVersion", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.CourierMaster.Features.StatusDeclarationOldVersion", NameTextCodeDefaultText = @"Status Declaration Old Version" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,CourierMasterObjectTable);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable CourierMasterObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CourierMaster" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = CourierMasterObjectTable.Id,
				 
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
                ObjectTableId = CourierMasterObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "VGR",
                EnglishName =  "Gatepass Movement Recived",
                LocalName =  "×‘×§×©×” ×œ×’×™×™×˜×¤×¡ ×”×¢×‘×¨×•×ª ×ž×ž×ª×™× ×” ×œ×�×™×©×•×¨",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CourierMasterObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "VGA",
                EnglishName =  "Gatepass Movement Approved",
                LocalName =  "×‘×§×©×ª ×’×™×™×˜×¤×¡ ×”×¢×‘×¨×•×ª ×�×•×©×¨×”",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CourierMasterObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "VGE",
                EnglishName =  "Gatepass Movement Error",
                LocalName =  "×‘×§×©×ª ×’×™×™×˜×¤×¡ ×”×¢×‘×¨×•×ª ×©×’×•×™×”",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CourierMasterObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "VGD",
                EnglishName =  "Gatepass Movement Reject",
                LocalName =  "×‘×§×©×ª ×’×™×™×˜×¤×¡ ×”×¢×‘×¨×•×ª × ×“×—×ª×”",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CourierMasterObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable CourierMasterObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CourierMaster" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOFilter = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.Filter", DefaultText = "Filter",LocalDefaultText = @"×¡×™× ×•×Ÿ", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOFlight = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.Flight", DefaultText = "Flight",LocalDefaultText = @"×˜×™×¡×”", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOTotalFilter = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.TotalFilter", DefaultText = "Total",LocalDefaultText = @"×¡×”×› ×©.×ž.×‘", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOValidation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.Validation", DefaultText = "Validation",LocalDefaultText = @"×‘×¢×™×” ×‘×¨×ž×ª ×”×˜×™×¡×”", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOClean = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.Clean", DefaultText = "Clean",LocalDefaultText = @"× ×§×”", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOAvailable = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.Available", DefaultText = "Available",LocalDefaultText = @"×–×ž×™× ×•×ª", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOStatus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.Status", DefaultText = "Status",LocalDefaultText = @"×¡×˜×˜×•×¡", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOCourierBOL = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.CourierBOL", DefaultText = "Courier BOL",LocalDefaultText = @"×¢×¨×š ×©.×ž.×‘", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterCourierBOLHigh = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.CourierBOL.High", DefaultText = "High",LocalDefaultText = @"High", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterCourierBOLLow = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.CourierBOL.Low", DefaultText = "Low",LocalDefaultText = @"Low", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterCourierBOLHighLow = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.CourierBOL.HighLow", DefaultText = "High/Low value",LocalDefaultText = @"High/Low value", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterAvailableAvailable = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.Available.Available", DefaultText = "Available",LocalDefaultText = @"×–×ž×™×Ÿ", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterAvailableNotAvailable = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.Available.NotAvailable", DefaultText = "Not Available",LocalDefaultText = @"×œ×� ×–×ž×™×Ÿ", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterAvailableAdditional = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.Available.Additional", DefaultText = "Additional",LocalDefaultText = @"×–×ž×™×Ÿ ×—×œ×§×™×ª", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterStatusOpen = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.Status.Open", DefaultText = "Open",LocalDefaultText = @"×¤×ª×•×—", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterStatusClose = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.Status.Close", DefaultText = "Close",LocalDefaultText = @"×¡×’×•×¨", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterHighLowValueHigh = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.HighLowValue.High", DefaultText = "High",LocalDefaultText = @"×ž×”×™×¨", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterHighLowValueLow = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.HighLowValue.Low", DefaultText = "Low",LocalDefaultText = @"×¤×¨×˜× ×™", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterMNFCompleteMissing = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.MNF.CompleteMissing", DefaultText = "Complete Missing Data",LocalDefaultText = @"×”×©×œ×� × ×ª×•× ×™×� ×—×¡×¨×™×�", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterCourierCustomStatusNoValue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.CourierCustomStatus.NoValue", DefaultText = "No Value",LocalDefaultText = @"×œ×œ×� ×¢×¨×š", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterMNFHandleWrong = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.MNF.HandleWrong", DefaultText = "Handle Wrong Feedback",LocalDefaultText = @"×˜×¤×œ ×‘×ž×©×•×‘×™×� ×©×’×•×™×™×�", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterACCWrong = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.ACC.Wrong", DefaultText = "Wrong",LocalDefaultText = @"×©.×ž.×‘ ×©×’×•×™×™×�", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterACCWrongSpecial = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.ACC.WrongSpecial", DefaultText = "Wrong Special",LocalDefaultText = @"×ž×™×•×—×“×•×ª ×©×’×•×™×™×�", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterSVGClassification = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.SVG.Classification", DefaultText = "Classification",LocalDefaultText = @"×¢×‘×•×¨ ×œ×¡×™×•×•×’", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterDOCDocumentCorrection = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.DOC.DocumentCorrection", DefaultText = "Document Correction",LocalDefaultText = @"×—×¡×¨×™×�", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterDOCDocumentCorrectionUploaded = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.DOC.DocumentCorrectionUploaded", DefaultText = "Document Correction Uploaded",LocalDefaultText = @"×©×’×™×�×•×ª", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOValidationErrors = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.ValidationErrors", DefaultText = "Corrections",LocalDefaultText = @"×‘×¢×™×” ×‘×¨×ž×ª ×”×˜×™×¡×”", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOMoreActions = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.MoreActions", DefaultText = "More Actions",LocalDefaultText = @"×¤×¢×•×œ×•×ª × ×•×¡×¤×•×ª", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOCompleteDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.CompleteDetails", DefaultText = "Complete Details",LocalDefaultText = @"×”×©×œ×ž×ª ×¤×¨×˜×™×�", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOReadyMNFToSend = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.ReadyMNFToSend", DefaultText = "Ready To Send",LocalDefaultText = @"×©×“×¨ ×ž×¦×”×¨ ×ž×¡×•×ž× ×™×�", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOReadyDECToSend = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.ReadyDECToSend", DefaultText = "Ready To Send",LocalDefaultText = @"×©×“×¨ ×”×¦×”×¨×” ×ž×¡×•×ž× ×™×�", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOReadyMNFToSendR = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.ReadyMNFToSendR", DefaultText = "Ready To Send",LocalDefaultText = @"×©×“×¨ ×ž×¦×”×¨ ×ž×•×›× ×™×�", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOReadyMNFToSendRV = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.ReadyMNFToSendRV", DefaultText = "Ready To Send",LocalDefaultText = @"×©×“×¨ ×ž×¦×”×¨ ×ª×§×™× ×™×�", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOReadyDECToSendR = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.ReadyDECToSendR", DefaultText = "Ready To Send",LocalDefaultText = @"×©×“×¨ ×”×¦×”×¨×” ×ž×•×›× ×™×�", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOReadyDECToSendRV = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.ReadyDECToSendRV", DefaultText = "Ready To Send",LocalDefaultText = @"×©×“×¨ ×”×¦×”×¨×” ×ª×§×™× ×™×�", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOReadyDECToSendX = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.ReadyDECToSendX", DefaultText = "Ready To Send",LocalDefaultText = @"×©×“×¨ ×”×¦×”×¨×” ×©×’×•×™×™×�", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOMarkPending = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.MarkPending", DefaultText = "Ready To Send",LocalDefaultText = @"×¡×™×ž×•×Ÿ ×‘ Pending", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOPendingReason = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.PendingReason", DefaultText = "Pending Reason",LocalDefaultText = @"×”×¡×™×‘×” ×œ-Pending", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOPendingMarkReason = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.PendingMarkReason", DefaultText = "Pending Mark Reason",LocalDefaultText = @"×”×¡×™×‘×” ×œ×¡×™×ž×•×Ÿ ×‘ Pending", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOPendingReasonApprove = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.PendingReasonApprove", DefaultText = "Pending Reason",LocalDefaultText = @"×�×™×©×•×¨ - × ×“×¨×© ×�×™×©×•×¨ ×©×œ ×¨×©×•×ª ×ž×•×¡×ž×›×ª", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterONoResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.NoResults", DefaultText = "No Results",LocalDefaultText = @"×�×™×Ÿ × ×ª×•× ×™×� ×œ×©×œ×™×—×”", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOSendMamanSpecialAction = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.SendMamanSpecialAction", DefaultText = "Send Maman Special Action",LocalDefaultText = @"×©×œ×— ×ž×¡×¨ ×¤×¢×•×œ×•×ª ×ž×™×•×—×“×•×ª", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOStickerDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.StickerDetails", DefaultText = "Sticker Details",LocalDefaultText = @"×¤×¨×˜×™ ×ž×“×‘×§×”", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterMPaymentPendingHold = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.M.PaymentPendingHold", DefaultText = "Payment can not be made when there is Pending with a hold-off type.",LocalDefaultText = @"×œ×� × ×™×ª×Ÿ ×œ×‘×¦×¢ ×”×’×©×ª ×ª×©×œ×•×� ×›×�×©×¨ ×™×© ×”×©×”×™×™×” ×ž×¡×•×’ ×¢×¦×™×¨×ª ×ª×©×œ×•×�. ", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOGatepassRequest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.GatepassRequest", DefaultText = "Gatepass Request",LocalDefaultText = @"×’×™×™×˜×¤×¡ ×”×¢×‘×¨×•×ª", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOChangeStorageSite = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.ChangeStorageSite", DefaultText = "Change Storage Site",LocalDefaultText = @"×©×™× ×•×™ ×�×ª×¨ ×�×—×¡×•×Ÿ", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterCourierCustomStatusSuspended = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.CourierCustomStatus.Suspended", DefaultText = "Suspended",LocalDefaultText = @"×ž×¢×•×›×‘", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterCourierCustomStatusHatara = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.CourierCustomStatus.Hatara", DefaultText = "Hatara",LocalDefaultText = @"×”×ª×¨×”", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOCorrect = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.Correct", DefaultText = "Correct",LocalDefaultText = @"×ª×§×™× ×™×�", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOInCorrect = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.InCorrect", DefaultText = "Incorrect",LocalDefaultText = @"×©×’×•×™×™×�", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOReadyToSend = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.ReadyToSend", DefaultText = "Ready To Send",LocalDefaultText = @"×ž×•×›× ×™×� ×œ×©×œ×™×—×”", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOInProgress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.InProgress", DefaultText = "In Progress",LocalDefaultText = @"×‘×ª×”×œ×™×š ×©×œ×™×—×”", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOOpenFlight = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.OpenFlight", DefaultText = "Open Flight",LocalDefaultText = @"×¤×ª×™×—×ª ×‘×™×˜×•×œ ×˜×™×¡×”", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOCancelFlight = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.CancelFlight", DefaultText = "Cancel Flight",LocalDefaultText = @"×‘×™×˜×•×œ ×˜×™×¡×”", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterONotValidDecInProccess = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.NotValidDecInProccess", DefaultText = "There are declarations in progress.",LocalDefaultText = @"×™×© ×‘×§×©×•×ª ×‘×ª×”×œ×™×š", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterONotValidDecWithPayment = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.NotValidDecWithPayment", DefaultText = "There are paid declarations.",LocalDefaultText = @"×™×© ×”×¦×”×¨×•×ª ×©×©×•×œ×ž×•.", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOCantCancelFlight = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.CantCancelFlight", DefaultText = "The flight cannot be canceled.",LocalDefaultText = @"×�×™×Ÿ ×�×¤×©×¨×•×ª ×œ×‘×˜×œ ×�×ª ×”×˜×™×¡×” - ", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOSureToCancel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.SureToCancel", DefaultText = "Are you sure you want to cancel?",LocalDefaultText = @"× ×� ×�×©×¨ ×‘×™×˜×•×œ ×˜×™×¡×”.", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOSureToOpenCancel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.SureToOpenCancel", DefaultText = "Please confirm opening flight cancellation.",LocalDefaultText = @"× ×� ×�×©×¨ ×¤×ª×™×—×ª ×‘×™×˜×•×œ ×˜×™×¡×”.", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOAll = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.All", DefaultText = "All",LocalDefaultText = @"×‘×—×¨ ×”×›×œ", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterONone = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.None", DefaultText = "None",LocalDefaultText = @"×‘×˜×œ ×‘×—×™×¨×”", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOCourierMasterQueries = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.CourierMasterQueries", DefaultText = "Courier Master Queries",LocalDefaultText = @"×©×�×™×œ×ª×•×ª ×œ×©×˜×¨×™ ×ž×˜×¢×Ÿ ×‘×œ×“×¨", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CourierMasterTextCode_CustomsCourierMasterOFlightOpenDeclarations = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierMaster.O.FlightOpenDeclarations", DefaultText = "Flight Open Declarations",LocalDefaultText = @"×”×¦×”×¨×•×ª ×¤×ª×•×—×•×ª ×œ×¤×™ ×˜×™×¡×”", ObjectTableId = CourierMasterObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 