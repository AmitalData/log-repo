using System;
using System.Collections.Generic;
using System.Linq;
 
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
using Simplog.Global.Data.GlobalModel.Repositories;

namespace JustWebFreight.WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses
{
   public class CertificateExemptionUpdateClass
   {
   
	 public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Customs.CertificateExemption",
			      				    DBTableName =  "Customs.CertificateExemptions",
			      				    ObjectTableSingular =  "Customs.CertificateExemption",
			      				    ObjectTablePlural =  "Customs.CertificateExemptions",
			      				    IsNewWizard =  false,
			      				    LookUp1 =  "Code",
			      				    LookUp2 =  "LocalName",
			      				    KeyPropertyPath =  "Code",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  true,
			      				    CacheOnClient =  true,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  false,
			      				    IsAutoComplete =  true,
			      				    EnableEditFromLOV =  false,
			      				    SortingByObjectField =  "Code",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  false,
			      				    IsComposition =  false,
			      				    EnableSecurity =  false,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "BR",
			      				    MaxNumberOfCustomFields =  0,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	{
	
       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "Code",
			      				    ObjectTableName =  "Customs.CertificateExemption",
			      				    FieldsDataType =  "Text",
			      				    MinLength =  0,
			      				    MaxLength =  3,
			      				    IsRequired =  true,
			      				    DisplayOnLookUp =  true,
			      				    CanFilter =  true,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  false,
			      				    SystemMaxLength =  3,
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
			      				    ValidForQuerySection1 =  "Customs.CertificateExemption",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "Code",
			      				    DefaultText =  "Code",
			      				    ListFieldLable =  "CodeListLable",
			      				    ListLableDefaultText =  "Code",
			      				    ShortFieldLable =  "Code",
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "EnglishName",
			      				    ObjectTableName =  "Customs.CertificateExemption",
			      				    FieldsDataType =  "Text",
			      				    MinLength =  0,
			      				    MaxLength =  40,
			      				    IsRequired =  false,
			      				    DisplayOnLookUp =  false,
			      				    CanFilter =  true,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  false,
			      				    SystemMaxLength =  40,
			      				    DisplayInList =  true,
			      				    IsCustomFilter =  false,
			      				    Operator =  "StartsWith",
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "EnglishName",
			      				    ListPropertyPath =  "EnglishName",
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.CertificateExemption",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "EnglishName",
			      				    DefaultText =  "English Name",
			      				    ListFieldLable =  "EnglishNameListLable",
			      				    ListLableDefaultText =  "English Name",
			      				    ShortFieldLable =  "EnglishName",
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "LocalName",
			      				    ObjectTableName =  "Customs.CertificateExemption",
			      				    FieldsDataType =  "nText",
			      				    MinLength =  0,
			      				    MaxLength =  100,
			      				    IsRequired =  false,
			      				    DisplayOnLookUp =  true,
			      				    CanFilter =  true,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  false,
			      				    SystemMaxLength =  100,
			      				    DisplayInList =  true,
			      				    IsCustomFilter =  false,
			      				    Operator =  "StartsWith",
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  true,
			      				    PMPropertyPath =  "LocalName",
			      				    ListPropertyPath =  "LocalName",
			      				    DisplayInLookUpIndex =  1,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  1,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.CertificateExemption",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "LocalName",
			      				    DefaultText =  "Local Name",
			      				    ListFieldLable =  "LocalNameListLable",
			      				    ListLableDefaultText =  "Local Name",
			      				    ShortFieldLable =  "LocalName",
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "SearchFields",
			      				    ObjectTableName =  "Customs.CertificateExemption",
			      				    FieldsDataType =  "nText",
			      				    MinLength =  0,
			      				    MaxLength =  1000,
			      				    IsRequired =  false,
			      				    DisplayOnLookUp =  false,
			      				    CanFilter =  false,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  false,
			      				    SystemMaxLength =  1000,
			      				    DisplayInList =  true,
			      				    IsCustomFilter =  false,
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "SearchFields",
			      				    ListPropertyPath =  "SearchFields",
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.CertificateExemption",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "SearchFields",
			      				    DefaultText =  "Search Fields",
			      				    ListFieldLable =  "SearchFieldsListLable",
			      				    ListLableDefaultText =  "SearchFields",
			      				    ShortFieldLable =  "SearchFields",
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	}

   }
    
}
	 