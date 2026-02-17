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
   public class MyEntityUpdateClass
   {
   
	 public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "MyEntity",
			      				    DBTableName =  "MyEntities",
			      				    ObjectTableSingular =  "MyEntity",
			      				    ObjectTablePlural =  "MyEntities",
			      				    DescriptionDefaultText =  "My Entitiy",
			      				    IsNewWizard =  false,
			      				    KeyPropertyPath =  "Id",
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
			      				    SortingByObjectField =  "Name",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  false,
			      				    IsComposition =  false,
			      				    EnableSecurity =  false,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "My Entitiy",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	{
	
       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "Name",
			      				    ObjectTableName =  "MyEntity",
			      				    FieldsDataType =  "Text",
			      				    MinLength =  0,
			      				    MaxLength =  60,
			      				    IsRequired =  true,
			      				    DisplayOnLookUp =  false,
			      				    CanFilter =  false,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  true,
			      				    SystemMaxLength =  60,
			      				    DisplayInList =  true,
			      				    IsCustomFilter =  false,
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "Name",
			      				    ListPropertyPath =  "Name",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    DependencyFilter1IsList =  false,
			      				    DependencyFilter2IsList =  false,
			      				    ValidForQuerySection1 =  "MyEntities",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "Name",
			      				    DefaultText =  "Name",
			      				    ListFieldLable =  "NameListLable",
			      				    ListLableDefaultText =  "Name",
			      				    ListLocalDefaultText =  "Name",
			      				    IsMaxLength =  false,
			      				    GenerateInList =  true,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	}

   }
    
}
	 