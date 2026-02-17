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
   public class OpportunityProductCompetitorUpdateClass
   {
   
	 public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "OpportunityProductCompetitor",
			      				    DBTableName =  "OpportunityProductCompetitors",
			      				    ObjectTableSingular =  "Opportunity Product Competitor",
			      				    ObjectTablePlural =  "Opportunity Product Competitors",
			      				    IsNewWizard =  false,
			      				    KeyPropertyPath =  "OpportunityId",
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
			      				    SortingByObjectField =  "OpportunityId",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  false,
			      				    IsComposition =  true,
			      				    EnableSecurity =  false,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "Opportunity Product Competitor",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	{
	
       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "OpportunityId",
			      				    ObjectTableName =  "OpportunityProductCompetitor",
			      				    FieldsDataType =  "Text",
			      				    MinLength =  0,
			      				    MaxLength =  15,
			      				    IsRequired =  true,
			      				    DisplayOnLookUp =  false,
			      				    CanFilter =  false,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  true,
			      				    SystemMaxLength =  15,
			      				    DisplayInList =  true,
			      				    IsCustomFilter =  false,
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "OpportunityId",
			      				    ListPropertyPath =  "OpportunityId",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "OpportunityProductCompetitor",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "OpportunityId",
			      				    DefaultText =  "Opportunity Id",
			      				    ListFieldLable =  "OpportunityIdListLable",
			      				    ListLableDefaultText =  "OpportunityId",
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "OpportunityProductTypeCode",
			      				    ObjectTableName =  "OpportunityProductCompetitor",
			      				    FieldsDataType =  "Text",
			      				    MinLength =  0,
			      				    MaxLength =  2,
			      				    IsRequired =  true,
			      				    DisplayOnLookUp =  false,
			      				    CanFilter =  true,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  true,
			      				    SystemMaxLength =  2,
			      				    DisplayInList =  true,
			      				    IsCustomFilter =  false,
			      				    Operator =  "StartsWith",
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "OpportunityProductTypeCode",
			      				    ListPropertyPath =  "OpportunityProductTypeCode",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "OpportunityProductCompetitor",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "OpportunityProductTypeCode",
			      				    DefaultText =  "Opportunity Product Type",
			      				    ListFieldLable =  "OpportunityProductTypeCodeListLable",
			      				    ListLableDefaultText =  "Opportunity Product Type",
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "CompetitorId",
			      				    ObjectTableName =  "OpportunityProductCompetitor",
			      				    FieldsDataType =  "Text",
			      				    MinLength =  0,
			      				    MaxLength =  15,
			      				    IsRequired =  true,
			      				    DisplayOnLookUp =  false,
			      				    CanFilter =  true,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  true,
			      				    SystemMaxLength =  15,
			      				    DisplayInList =  true,
			      				    IsCustomFilter =  false,
			      				    Operator =  "StartsWith",
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "CompetitorId",
			      				    ListPropertyPath =  "CompetitorId",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "OpportunityProductCompetitor",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "CompetitorId",
			      				    DefaultText =  "Competitor",
			      				    ListFieldLable =  "CompetitorIdListLable",
			      				    ListLableDefaultText =  "Competitor",
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "Notes",
			      				    ObjectTableName =  "OpportunityProductCompetitor",
			      				    FieldsDataType =  "nText",
			      				    MinLength =  0,
			      				    MaxLength =  250,
			      				    IsRequired =  false,
			      				    DisplayOnLookUp =  false,
			      				    CanFilter =  false,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  false,
			      				    SystemMaxLength =  250,
			      				    DisplayInList =  true,
			      				    IsCustomFilter =  false,
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
			      				    ValidForQuerySection1 =  "OpportunityProductCompetitor",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "Notes",
			      				    DefaultText =  "Notes",
			      				    ListFieldLable =  "NotesListLable",
			      				    ListLableDefaultText =  "Notes",
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "ProductPeriodCode",
			      				    ObjectTableName =  "OpportunityProductCompetitor",
			      				    FieldsDataType =  "Text",
			      				    MinLength =  0,
			      				    MaxLength =  2,
			      				    IsRequired =  false,
			      				    DisplayOnLookUp =  false,
			      				    CanFilter =  false,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  false,
			      				    SystemMaxLength =  2,
			      				    DisplayInList =  true,
			      				    IsCustomFilter =  false,
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "ProductPeriodCode",
			      				    ListPropertyPath =  "ProductPeriodCode",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "OpportunityProductCompetitor",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "ProductPeriodCode",
			      				    DefaultText =  "Period",
			      				    ListFieldLable =  "ProductPeriodCodeListLable",
			      				    ListLableDefaultText =  "Period",
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "CompetitorName",
			      				    ObjectTableName =  "OpportunityProductCompetitor",
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
			      				    PMPropertyPath =  "CompetitorName",
			      				    ListPropertyPath =  "CompetitorName",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "OpportunityProductCompetitor",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "CompetitorName",
			      				    DefaultText =  "Competitor",
			      				    ListFieldLable =  "CompetitorNameListLable",
			      				    ListLableDefaultText =  "Competitor",
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	}

   }
    
}
	 