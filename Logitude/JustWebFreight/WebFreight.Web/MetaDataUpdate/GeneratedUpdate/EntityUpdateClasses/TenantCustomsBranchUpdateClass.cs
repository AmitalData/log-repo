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
   public class TenantCustomsBranchUpdateClass
   {
   
	 public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Customs.TenantCustomsBranch",
			      				    DBTableName =  "Customs.TenantCustomsBranches",
			      				    ObjectTableSingular =  "TenantCustomsBranch",
			      				    ObjectTablePlural =  "TenantCustomsBranches",
			      				    IsNewWizard =  false,
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  true,
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
			      				    ObjectTableTypeCode =  "BR",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "Tenant Customs Branch",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	{
	
       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "Code",
			      				    ObjectTableName =  "Customs.TenantCustomsBranch",
			      				    FieldsDataType =  "Text",
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
			      				    Operator =  "StartsWith",
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "Code",
			      				    ListPropertyPath =  "Code",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.TenantCustomsBranch",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "Code",
			      				    DefaultText =  "Code",
			      				    FullLocalDefaultText =  "קוד  תחנה",
			      				    ListFieldLable =  "CodeListLable",
			      				    ListLableDefaultText =  "Code",
			      				    ListLocalDefaultText =  "קוד  תחנה",
			      				    IsMaxLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "TransportModeId",
			      				    ObjectTableName =  "Customs.TenantCustomsBranch",
			      				    FieldsDataType =  "LookUp",
			      				    LookUpTableName =  "Customs.CustomsTransportMode",
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
			      				    Operator =  "Equals",
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "TransportModeId",
			      				    ListPropertyPath =  "TransportModeId",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.TenantCustomsBranch",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "TransportModeId",
			      				    DefaultText =  "Transport Mode",
			      				    FullLocalDefaultText =  "סוג הובלה",
			      				    ListFieldLable =  "TransportModeIdListLable",
			      				    ListLableDefaultText =  "Transport Mode",
			      				    ListLocalDefaultText =  "סוג הובלה",
			      				    IsMaxLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "UnloadPortCode",
			      				    ObjectTableName =  "Customs.TenantCustomsBranch",
			      				    FieldsDataType =  "LookUp",
			      				    LookUpTableName =  "Customs.SiteLookup",
			      				    MinLength =  0,
			      				    MaxLength =  17,
			      				    IsRequired =  false,
			      				    DisplayOnLookUp =  false,
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
			      				    PMPropertyPath =  "UnloadPortCode",
			      				    ListPropertyPath =  "UnloadPortCode",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.TenantCustomsBranch",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "UnloadPortCode",
			      				    DefaultText =  "Unload Port",
			      				    FullLocalDefaultText =  "קוד אתר פריקה",
			      				    ListFieldLable =  "UnloadPortCodeListLable",
			      				    ListLableDefaultText =  "Unload Port",
			      				    ListLocalDefaultText =  "קוד אתר פריקה",
			      				    IsMaxLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "TransportModeName",
			      				    ObjectTableName =  "Customs.TenantCustomsBranch",
			      				    FieldsDataType =  "nText",
			      				    MinLength =  0,
			      				    MaxLength =  10,
			      				    IsRequired =  false,
			      				    DisplayOnLookUp =  false,
			      				    CanFilter =  false,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  false,
			      				    SystemMaxLength =  10,
			      				    DisplayInList =  true,
			      				    IsCustomFilter =  false,
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "TransportModeName",
			      				    ListPropertyPath =  "TransportModeName",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.TenantCustomsBranch",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "TransportModeName",
			      				    DefaultText =  "Transport Mode Name",
			      				    FullLocalDefaultText =  "סוג הובלה",
			      				    ListFieldLable =  "TransportModeNameListLable",
			      				    ListLableDefaultText =  "Transport Mode Name",
			      				    ListLocalDefaultText =  "סוג הובלה",
			      				    IsMaxLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "UnloadPortName",
			      				    ObjectTableName =  "Customs.TenantCustomsBranch",
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
			      				    PMPropertyPath =  "UnloadPortName",
			      				    ListPropertyPath =  "UnloadPortName",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.TenantCustomsBranch",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "UnloadPortName",
			      				    DefaultText =  "Unload Port Name",
			      				    FullLocalDefaultText =  "קוד אתר פריקה",
			      				    ListFieldLable =  "UnloadPortNameListLable",
			      				    ListLableDefaultText =  "Unload Port Name",
			      				    ListLocalDefaultText =  "קוד אתר פריקה",
			      				    IsMaxLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	}

   }
    
}
	 