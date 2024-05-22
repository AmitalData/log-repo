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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.ShipmentsModel.EntityUpdateClasses
{
   public class ShipmentAWBPrintOnlyUpdateClass
   {  		
		public const string HashString = "a3e68b0220e20c8a56c2030e2f7895c6";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "ShipmentAWBPrintOnly",
			      				    DBTableName =  "ShipmentAWBPrintOnlies",
			      				    ObjectTableSingular =  "Shipment AWB Print Only",
			      				    ObjectTablePlural =  "ShipmentAWBPrintOnlies",
			      				    DefaultText =  "Shipment AWB Print Only",
			      				    Name =  "ShipmentAWBPrintOnly",
			      				    IsNewWizard =  false,
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
			      				    SearchFields =  "ShipmentAWBPrintOnly,ShipmentAWBPrintOnlies,,Id,",
			      				    IsSaveButtonVisible =  true,
			      				    EnableSecurity =  true,
			      				    ObjectTableTypeCode =  "BR",
			      				    IsComposition =  true,
			      				    MaxNumberOfCustomFields =  0,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsEditable =  true,
			      				    HashString =  ShipmentAWBPrintOnlyUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Id",
					  						ObjectTableName =  "ShipmentAWBPrintOnly",
					  						FieldsDataType =  "Text",
					  						PMPropertyPath =  "Id",
					  						ListPropertyPath =  "Id",
					  						FullFieldLable =  "Id",
					  						DefaultText =  "Id",
					  						FullLocalDefaultText =  "Id",
					  						ListFieldLable =  "IdListLable",
					  						ListLableDefaultText =  "Id",
					  						ListLocalDefaultText =  "Id",
					  						IsRequired =  false,
					  						DisplayInList =  false,
					  						NoMetaDataField =  true,
					  						MaxLength =  15,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Tenant",
					  						ObjectTableName =  "ShipmentAWBPrintOnly",
					  						FieldsDataType =  "Integer",
					  						PMPropertyPath =  "Tenant",
					  						ListPropertyPath =  "Tenant",
					  						FullFieldLable =  "Tenant",
					  						DefaultText =  "Tenant",
					  						FullLocalDefaultText =  "Tenant",
					  						ListFieldLable =  "TenantListLable",
					  						ListLableDefaultText =  "Tenant",
					  						ListLocalDefaultText =  "Tenant",
					  						IsRequired =  false,
					  						DisplayInList =  false,
					  						NoMetaDataField =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ExchangeRate",
					  						ObjectTableName =  "ShipmentAWBPrintOnly",
					  						FieldsDataType =  "Double",
					  						DataTypeCode =  "Double",
					  						MaxLength =  0,
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
					  						PMPropertyPath =  "ExchangeRate",
					  						ListPropertyPath =  "ExchangeRate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFilterInx =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						FullFieldLable =  "ExchangeRate",
					  						DefaultText =  "Exchange Rate",
					  						HelpTextCode =  "ExchangeRate",
					  						ShortFieldLable =  "ExchangeRate",
					  						ShortFieldLableDefaultText =  "Ex. Rate",
					  						Code =  "ExchangeRate",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						NoMetaDataField =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MeasurementId",
					  						ObjectTableName =  "ShipmentAWBPrintOnly",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Measurement",
					  						DataTypeCode =  "LookUp",
					  						MaxLength =  15,
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
					  						PMPropertyPath =  "MeasurementId",
					  						ListPropertyPath =  "MeasurementId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFilterInx =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						FullFieldLable =  "MeasurementId",
					  						DefaultText =  "Measurement",
					  						HelpTextCode =  "MeasurementId",
					  						Code =  "MeasurementId",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						NoMetaDataField =  false,
					  						IsForeignKey =  true,
					  						ForeignEntity =  "Measurement",
					  						NavigationPropertyName =  "Measurement",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IATACodeId",
					  						ObjectTableName =  "ShipmentAWBPrintOnly",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "IATACode",
					  						DataTypeCode =  "LookUp",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "IATACodeId",
					  						ListPropertyPath =  "IATACodeId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFilterInx =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						FullFieldLable =  "IATACodeId",
					  						DefaultText =  "IATA Code",
					  						HelpTextCode =  "IATACodeId",
					  						Code =  "IATACodeId",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						NoMetaDataField =  false,
					  						IsForeignKey =  true,
					  						ForeignEntity =  "IATACode",
					  						NavigationPropertyName =  "IATACode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CurrencyId",
					  						ObjectTableName =  "ShipmentAWBPrintOnly",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Currency",
					  						DataTypeCode =  "LookUp",
					  						MaxLength =  15,
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
					  						PMPropertyPath =  "CurrencyId",
					  						ListPropertyPath =  "CurrencyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFilterInx =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						FullFieldLable =  "Currency",
					  						DefaultText =  "Currency",
					  						HelpTextCode =  "Currency",
					  						Code =  "CurrencyId",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  true,
					  						NoMetaDataField =  false,
					  						IsForeignKey =  true,
					  						ForeignEntity =  "Currency",
					  						NavigationPropertyName =  "Currency",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DueTypeCode",
					  						ObjectTableName =  "ShipmentAWBPrintOnly",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "DueType",
					  						DataTypeCode =  "LookUp",
					  						MaxLength =  2,
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
					  						PMPropertyPath =  "DueTypeCode",
					  						ListPropertyPath =  "DueTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFilterInx =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						FullFieldLable =  "DueTypeCode",
					  						DefaultText =  "Due Type",
					  						HelpTextCode =  "DueTypeCode",
					  						ShortFieldLable =  "DueTypeCode",
					  						ShortFieldLableDefaultText =  "Due",
					  						Code =  "DueTypeCode",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  true,
					  						NoMetaDataField =  false,
					  						IsForeignKey =  true,
					  						ForeignEntity =  "DueType",
					  						NavigationPropertyName =  "DueType",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrepaidCollectId",
					  						ObjectTableName =  "ShipmentAWBPrintOnly",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "PrepaidCollect",
					  						DataTypeCode =  "LookUp",
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
					  						PMPropertyPath =  "PrepaidCollectId",
					  						ListPropertyPath =  "PrepaidCollectId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFilterInx =  0,
					  						IsMulti =  false,
					  						DependencyFilter1Value =  "True",
					  						DependencyFilter1Type =  "Constant",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						FullFieldLable =  "PrepaidCollectId",
					  						DefaultText =  "Prepaid/Collect",
					  						HelpTextCode =  "PrepaidCollectId",
					  						ShortFieldLable =  "PrepaidCollectId",
					  						ShortFieldLableDefaultText =  "P/C",
					  						Code =  "PrepaidCollectId",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						NoMetaDataField =  false,
					  						IsForeignKey =  true,
					  						ForeignEntity =  "PrepaidCollect",
					  						NavigationPropertyName =  "PrepaidCollect",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ShipmentId",
					  						ObjectTableName =  "ShipmentAWBPrintOnly",
					  						FieldsDataType =  "Text",
					  						LookUpTableName =  "Shipment",
					  						DataTypeCode =  "LookUp",
					  						MaxLength =  15,
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
					  						PMPropertyPath =  "ShipmentId",
					  						ListPropertyPath =  "ShipmentId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFilterInx =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						FullFieldLable =  "ShipmentId",
					  						DefaultText =  "Shipment ID",
					  						HelpTextCode =  "ShipmentId",
					  						Code =  "ShipmentId",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						NoMetaDataField =  false,
					  						IsForeignKey =  true,
					  						ForeignEntity =  "Shipment",
					  						NavigationPropertyName =  "Shipment",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Quantity",
					  						ObjectTableName =  "ShipmentAWBPrintOnly",
					  						FieldsDataType =  "Double",
					  						DataTypeCode =  "Double",
					  						MaxLength =  0,
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
					  						PMPropertyPath =  "Quantity",
					  						ListPropertyPath =  "Quantity",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFilterInx =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						FullFieldLable =  "Quantity",
					  						DefaultText =  "Quantity",
					  						HelpTextCode =  "Quantity",
					  						Code =  "Quantity",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						NoMetaDataField =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UnitPrice",
					  						ObjectTableName =  "ShipmentAWBPrintOnly",
					  						FieldsDataType =  "Double",
					  						DataTypeCode =  "Double",
					  						MaxLength =  0,
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
					  						PMPropertyPath =  "UnitPrice",
					  						ListPropertyPath =  "UnitPrice",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFilterInx =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						FullFieldLable =  "UnitPrice",
					  						DefaultText =  "Unit Price",
					  						HelpTextCode =  "UnitPrice",
					  						Code =  "UnitPrice",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						NoMetaDataField =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Amount",
					  						ObjectTableName =  "ShipmentAWBPrintOnly",
					  						FieldsDataType =  "Double",
					  						DataTypeCode =  "Double",
					  						MaxLength =  0,
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
					  						PMPropertyPath =  "Amount",
					  						ListPropertyPath =  "Amount",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFilterInx =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						FullFieldLable =  "Amount",
					  						DefaultText =  "Amount",
					  						HelpTextCode =  "Amount",
					  						Code =  "Amount",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						NoMetaDataField =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldsDataType =  "Text",
					  						FieldName =  "CurrencyCode",
					  						PMPropertyPath =  "CurrencyCode",
					  						ListPropertyPath =  "CurrencyCode",
					  						FullFieldLable =  "CurrencyCode",
					  						DefaultText =  "CurrencyCode",
					  						FullLocalDefaultText =  "CurrencyCode",
					  						ListFieldLable =  "CurrencyCodeListLable",
					  						ListLableDefaultText =  "CurrencyCode",
					  						ListLocalDefaultText =  "CurrencyCode",
					  						ObjectTableName =  "ShipmentAWBPrintOnly",
					  						NoMetaDataField =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldsDataType =  "Text",
					  						FieldName =  "DueTypeName",
					  						PMPropertyPath =  "DueTypeName",
					  						ListPropertyPath =  "DueTypeName",
					  						FullFieldLable =  "DueTypeName",
					  						DefaultText =  "DueTypeName",
					  						FullLocalDefaultText =  "DueTypeName",
					  						ListFieldLable =  "DueTypeNameListLable",
					  						ListLableDefaultText =  "DueTypeName",
					  						ListLocalDefaultText =  "DueTypeName",
					  						ObjectTableName =  "ShipmentAWBPrintOnly",
					  						NoMetaDataField =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldsDataType =  "Text",
					  						FieldName =  "MeasurementCode",
					  						PMPropertyPath =  "MeasurementCode",
					  						ListPropertyPath =  "MeasurementCode",
					  						FullFieldLable =  "MeasurementCode",
					  						DefaultText =  "MeasurementCode",
					  						FullLocalDefaultText =  "MeasurementCode",
					  						ListFieldLable =  "MeasurementCodeListLable",
					  						ListLableDefaultText =  "MeasurementCode",
					  						ListLocalDefaultText =  "MeasurementCode",
					  						ObjectTableName =  "ShipmentAWBPrintOnly",
					  						NoMetaDataField =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldsDataType =  "Text",
					  						FieldName =  "IATACodeName",
					  						PMPropertyPath =  "IATACodeName",
					  						ListPropertyPath =  "IATACodeName",
					  						FullFieldLable =  "IATACodeName",
					  						DefaultText =  "IATACodeName",
					  						FullLocalDefaultText =  "IATACodeName",
					  						ListFieldLable =  "IATACodeNameListLable",
					  						ListLableDefaultText =  "IATACodeName",
					  						ListLocalDefaultText =  "IATACodeName",
					  						ObjectTableName =  "ShipmentAWBPrintOnly",
					  						NoMetaDataField =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldsDataType =  "Text",
					  						FieldName =  "ChangeSetOp",
					  						PMPropertyPath =  "ChangeSetOp",
					  						ListPropertyPath =  "ChangeSetOp",
					  						FullFieldLable =  "ChangeSetOp",
					  						DefaultText =  "ChangeSetOp",
					  						FullLocalDefaultText =  "ChangeSetOp",
					  						ListFieldLable =  "ChangeOpListLable",
					  						ListLableDefaultText =  "ChangeSetOp",
					  						ListLocalDefaultText =  "ChangeSetOp",
					  						ObjectTableName =  "ShipmentAWBPrintOnly",
					  						NoMetaDataField =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters,Dictionary<string, QueryGroup> tenantQueryGroups )
	    {    
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {    

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {      
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {     
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable ShipmentAWBPrintOnlyObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ShipmentAWBPrintOnly" && d.Tenant == 0).FirstOrDefault(); 
	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {     
	    
}

    

   }
    
}
	 