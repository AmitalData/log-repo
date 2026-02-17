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

namespace JustWebFreight.WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses
{
   public class SupplierInvoiceItemsQuantityUpdateClass
   {
   
	 public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Customs.SupplierInvoiceItemsQuantity",
			      				    DBTableName =  "Customs.SupplierInvoiceItemsQuantities",
			      				    ObjectTableSingular =  "SupplierInvoiceItemsQuantity",
			      				    ObjectTablePlural =  "SupplierInvoiceItemsQuantities",
			      				    IsNewWizard =  false,
			      				    KeyPropertyPath =  "DeclarationId",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  false,
			      				    IsAutoComplete =  false,
			      				    EnableEditFromLOV =  false,
			      				    SortingByObjectField =  "InvoiceItemLineNumber",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  false,
			      				    IsComposition =  true,
			      				    EnableSecurity =  false,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "BR",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "Supplier Invoice Items Quantity",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	{
	
       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "DeclarationId",
			      				    ObjectTableName =  "Customs.SupplierInvoiceItemsQuantity",
			      				    FieldsDataType =  "Text",
			      				    MinLength =  0,
			      				    MaxLength =  15,
			      				    IsRequired =  true,
			      				    DisplayOnLookUp =  false,
			      				    CanFilter =  true,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  false,
			      				    SystemMaxLength =  15,
			      				    DisplayInList =  true,
			      				    IsCustomFilter =  false,
			      				    Operator =  "StartsWith",
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "DeclarationId",
			      				    ListPropertyPath =  "DeclarationId",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.SupplierInvoiceItemsQuantity",
			      				    DisplayInEntityVariables =  false,
			      				    DigitsAfterPoint =  0,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "DeclarationId",
			      				    DefaultText =  "Declaration ID",
			      				    FullLocalDefaultText =  "מספר תיק מכס",
			      				    ListFieldLable =  "DeclarationIdListLable",
			      				    ListLableDefaultText =  "Declaration ID",
			      				    ListLocalDefaultText =  "מספר תיק מכס",
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "InvoiceCounterKey",
			      				    ObjectTableName =  "Customs.SupplierInvoiceItemsQuantity",
			      				    FieldsDataType =  "Integer",
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
			      				    PMPropertyPath =  "InvoiceCounterKey",
			      				    ListPropertyPath =  "InvoiceCounterKey",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.SupplierInvoiceItemsQuantity",
			      				    DisplayInEntityVariables =  false,
			      				    DigitsAfterPoint =  0,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "InvoiceCounterKey",
			      				    DefaultText =  "Invoice Counter Key",
			      				    ListFieldLable =  "InvoiceCounterKeyListLable",
			      				    ListLableDefaultText =  "Invoice Counter Key",
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "InvoiceItemLineNumber",
			      				    ObjectTableName =  "Customs.SupplierInvoiceItemsQuantity",
			      				    FieldsDataType =  "Integer",
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
			      				    PMPropertyPath =  "InvoiceItemLineNumber",
			      				    ListPropertyPath =  "InvoiceItemLineNumber",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.SupplierInvoiceItemsQuantity",
			      				    DisplayInEntityVariables =  false,
			      				    DigitsAfterPoint =  0,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "InvoiceItemLineNumber",
			      				    DefaultText =  "Invoice Item Line Number",
			      				    FullLocalDefaultText =  "מספר קו פריט חשבונית",
			      				    ListFieldLable =  "InvoiceItemLineNumberListLable",
			      				    ListLableDefaultText =  "Invoice Item Line Number",
			      				    ListLocalDefaultText =  "מספר קו פריט חשבונית",
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "LineNumber",
			      				    ObjectTableName =  "Customs.SupplierInvoiceItemsQuantity",
			      				    FieldsDataType =  "Integer",
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
			      				    PMPropertyPath =  "LineNumber",
			      				    ListPropertyPath =  "LineNumber",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.SupplierInvoiceItemsQuantity",
			      				    DisplayInEntityVariables =  false,
			      				    DigitsAfterPoint =  0,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "LineNumber",
			      				    DefaultText =  "Line Number",
			      				    FullLocalDefaultText =  "מספר הקו",
			      				    ListFieldLable =  "LineNumberListLable",
			      				    ListLableDefaultText =  "Line Number",
			      				    ListLocalDefaultText =  "מספר הקו",
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "MeasureQualifierCode",
			      				    ObjectTableName =  "Customs.SupplierInvoiceItemsQuantity",
			      				    FieldsDataType =  "LookUp",
			      				    LookUpTableName =  "Customs.MeasureQualifier",
			      				    MinLength =  0,
			      				    MaxLength =  4,
			      				    IsRequired =  false,
			      				    DisplayOnLookUp =  false,
			      				    CanFilter =  true,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  false,
			      				    SystemMaxLength =  4,
			      				    DisplayInList =  true,
			      				    IsCustomFilter =  false,
			      				    Operator =  "Equals",
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "MeasureQualifierCode",
			      				    ListPropertyPath =  "MeasureQualifierCode",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.SupplierInvoiceItemsQuantity",
			      				    DisplayInEntityVariables =  false,
			      				    DigitsAfterPoint =  0,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "MeasureQualifierCode",
			      				    DefaultText =  "Measure Qualifier",
			      				    FullLocalDefaultText =  "סוג כמות",
			      				    ListFieldLable =  "MeasureQualifierCodeListLable",
			      				    ListLableDefaultText =  "Measure Qualifier",
			      				    ListLocalDefaultText =  "סוג כמות",
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "Quantity",
			      				    ObjectTableName =  "Customs.SupplierInvoiceItemsQuantity",
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
			      				    PMPropertyPath =  "Quantity",
			      				    ListPropertyPath =  "Quantity",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.SupplierInvoiceItemsQuantity",
			      				    DisplayInEntityVariables =  false,
			      				    DigitsAfterPoint =  2,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "Quantity",
			      				    DefaultText =  "Quantity",
			      				    FullLocalDefaultText =  "כמות",
			      				    ListFieldLable =  "QuantityListLable",
			      				    ListLableDefaultText =  "Quantity",
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "MeasureQualifierName",
			      				    ObjectTableName =  "Customs.SupplierInvoiceItemsQuantity",
			      				    FieldsDataType =  "nText",
			      				    MinLength =  0,
			      				    MaxLength =  100,
			      				    IsRequired =  false,
			      				    DisplayOnLookUp =  false,
			      				    CanFilter =  false,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  false,
			      				    SystemMaxLength =  100,
			      				    DisplayInList =  true,
			      				    IsCustomFilter =  false,
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "MeasureQualifierName",
			      				    ListPropertyPath =  "MeasureQualifierName",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.SupplierInvoiceItemsQuantity",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "MeasureQualifierName",
			      				    DefaultText =  "MeasureQualifierName",
			      				    ListFieldLable =  "MeasureQualifierNameListLable",
			      				    ListLableDefaultText =  "MeasureQualifierName",
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	}

   }
    
}
	 