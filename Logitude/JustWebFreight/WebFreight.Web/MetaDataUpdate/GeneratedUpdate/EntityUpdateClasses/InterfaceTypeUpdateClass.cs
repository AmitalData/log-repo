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
   public class InterfaceTypeUpdateClass
   {
   
	 public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Customs.InterfaceType",
			      				    DBTableName =  "Customs.InterfaceTypes",
			      				    ObjectTableSingular =  "Interface Type",
			      				    ObjectTablePlural =  "Interface Types",
			      				    IsNewWizard =  false,
			      				    LookUp1 =  "Code",
			      				    LookUp2 =  "DcaPrefixName",
			      				    KeyPropertyPath =  "Code",
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
			      				    SortingByObjectField =  "Code",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  false,
			      				    IsComposition =  false,
			      				    EnableSecurity =  false,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    LocalDefaultText =  "סוג הממשק",
			      				    DefaultText =  "Interface Type",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	{
	
       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "Code",
			      				    ObjectTableName =  "Customs.InterfaceType",
			      				    FieldsDataType =  "Text",
			      				    MinLength =  0,
			      				    MaxLength =  32,
			      				    IsRequired =  true,
			      				    DisplayOnLookUp =  true,
			      				    CanFilter =  true,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  false,
			      				    SystemMaxLength =  32,
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
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.InterfaceType",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "Code",
			      				    DefaultText =  "Code",
			      				    FullLocalDefaultText =  "מספר מסר",
			      				    ListFieldLable =  "CodeListLable",
			      				    ListLableDefaultText =  "Code",
			      				    ListLocalDefaultText =  "מספר מסר",
			      				    IsMaxLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "DcaPrefixName",
			      				    ObjectTableName =  "Customs.InterfaceType",
			      				    FieldsDataType =  "Text",
			      				    MinLength =  0,
			      				    MaxLength =  256,
			      				    IsRequired =  false,
			      				    DisplayOnLookUp =  true,
			      				    CanFilter =  true,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  false,
			      				    SystemMaxLength =  256,
			      				    DisplayInList =  true,
			      				    IsCustomFilter =  false,
			      				    Operator =  "StartsWith",
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  true,
			      				    PMPropertyPath =  "DcaPrefixName",
			      				    ListPropertyPath =  "DcaPrefixName",
			      				    DisplayInLookUpIndex =  1,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  1,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.InterfaceType",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "DcaPrefixName",
			      				    DefaultText =  "DcaPrefixName",
			      				    FullLocalDefaultText =  "שם קובץ בכספת",
			      				    ListFieldLable =  "DcaPrefixNameListLable",
			      				    ListLableDefaultText =  "DCA Prefix Name",
			      				    ListLocalDefaultText =  "שם קובץ בכספת",
			      				    IsMaxLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "Description",
			      				    ObjectTableName =  "Customs.InterfaceType",
			      				    FieldsDataType =  "Text",
			      				    MinLength =  0,
			      				    MaxLength =  256,
			      				    IsRequired =  false,
			      				    DisplayOnLookUp =  false,
			      				    CanFilter =  true,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  false,
			      				    SystemMaxLength =  256,
			      				    DisplayInList =  true,
			      				    IsCustomFilter =  false,
			      				    Operator =  "StartsWith",
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "Description",
			      				    ListPropertyPath =  "Description",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.InterfaceType",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "Description",
			      				    DefaultText =  "Description",
			      				    FullLocalDefaultText =  "תיאור מסר",
			      				    ListFieldLable =  "DescriptionListLable",
			      				    ListLableDefaultText =  "Description",
			      				    ListLocalDefaultText =  "תיאור מסר",
			      				    IsMaxLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "InOut",
			      				    ObjectTableName =  "Customs.InterfaceType",
			      				    FieldsDataType =  "Text",
			      				    MinLength =  0,
			      				    MaxLength =  1,
			      				    IsRequired =  false,
			      				    DisplayOnLookUp =  false,
			      				    CanFilter =  true,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  false,
			      				    SystemMaxLength =  1,
			      				    DisplayInList =  true,
			      				    IsCustomFilter =  false,
			      				    Operator =  "StartsWith",
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "InOut",
			      				    ListPropertyPath =  "InOut",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.InterfaceType",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "InOut",
			      				    DefaultText =  "InOut",
			      				    FullLocalDefaultText =  "נכנס / יוצא",
			      				    ListFieldLable =  "InOutListLable",
			      				    ListLableDefaultText =  "InOut",
			      				    ListLocalDefaultText =  "נכנס / יוצא",
			      				    IsMaxLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "SendOptionsCode",
			      				    ObjectTableName =  "Customs.InterfaceType",
			      				    FieldsDataType =  "LookUp",
			      				    MinLength =  0,
			      				    MaxLength =  3,
			      				    IsRequired =  false,
			      				    DisplayOnLookUp =  false,
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
			      				    PMPropertyPath =  "SendOptionsCode",
			      				    ListPropertyPath =  "SendOptionsCode",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.InterfaceType",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "SendOptionsCode",
			      				    DefaultText =  "Send Options ",
			      				    FullLocalDefaultText =  "אפשרויות שליחה",
			      				    ListFieldLable =  "SendOptionsCodeListLable",
			      				    ListLableDefaultText =  "Send Options ",
			      				    ListLocalDefaultText =  "אפשרויות שליחה",
			      				    IsMaxLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "Prioirity",
			      				    ObjectTableName =  "Customs.InterfaceType",
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
			      				    PMPropertyPath =  "Prioirity",
			      				    ListPropertyPath =  "Prioirity",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.InterfaceType",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "Prioirity",
			      				    DefaultText =  "Prioirity",
			      				    FullLocalDefaultText =  "עדיפות שליחה / ניתוח",
			      				    ListFieldLable =  "PrioirityListLable",
			      				    ListLableDefaultText =  "Prioirity",
			      				    ListLocalDefaultText =  "עדיפות שליחה / ניתוח",
			      				    IsMaxLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "AnalyzeClass",
			      				    ObjectTableName =  "Customs.InterfaceType",
			      				    FieldsDataType =  "Text",
			      				    MinLength =  0,
			      				    MaxLength =  60,
			      				    IsRequired =  false,
			      				    DisplayOnLookUp =  false,
			      				    CanFilter =  false,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  false,
			      				    SystemMaxLength =  60,
			      				    DisplayInList =  false,
			      				    IsCustomFilter =  false,
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "AnalyzeClass",
			      				    ListPropertyPath =  "AnalyzeClass",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "AnalyzeClass",
			      				    DefaultText =  "Analyze Class",
			      				    ListFieldLable =  "AnalyzeClassListLable",
			      				    ListLableDefaultText =  "Analyze Class",
			      				    ListLocalDefaultText =  "Customs.InterfaceType",
			      				    IsMaxLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	}

   }
    
}
	 