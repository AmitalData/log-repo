 
  

-- this script is generated
delete from DWObjectFields
delete from DWObjectTables
------------------------------------------------------------------------------------
declare @DIM_BranchesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_BranchesNewId,0,'','DIM_Branches','DIM_Branches','Dimension','false','[Name]','false')  
--Fields --
declare @DIM_BranchesId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_BranchesId_NumberNewId,0,'DIM_Branches','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false','false','false','false','false')  
declare @DIM_BranchesIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_BranchesIdNewId,0,'DIM_Branches','[Id]','Id','Text','true',0,15,'false','false','false','false','false','false','true','false')  
declare @DIM_BranchesNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_BranchesNameNewId,0,'DIM_Branches','[Name]','Name','Text','true',0,40,'false','false','true','[Code]','false','true','false','false','false')  
declare @DIM_BranchesLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_BranchesLocalNameNewId,0,'DIM_Branches','[Local Name]','Local Name','nText','false',0,40,'false','false','true','false','true','false','false','false')  
declare @DIM_BranchesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_BranchesCodeNewId,0,'DIM_Branches','[Code]','Code','Text','false',0,13,'false','false','true','[Name],[Local Name]','false','false','false','true','false')  
declare @DIM_BranchesSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@DIM_BranchesSourceTenantNewId,0,'DIM_Branches','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false','false','false','Tenant','false','false')  
declare @DIM_BranchesParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_BranchesParentTenantNewId,0,'DIM_Branches','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false','false','false','true','false')  
declare @DIM_BranchesAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_BranchesAutomaticLastUpdateDateNewId,0,'DIM_Branches','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','true','false','false','false','false','false')  
declare @DIM_BranchesInActiveNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesInActiveNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_BranchesInActiveNewId,0,'DIM_Branches','[InActive]','InActive','Boolean','false',0,0,'false','false','true','false','false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_ChargesTypesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ChargesTypesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_ChargesTypesNewId,0,'','DIM_ChargesTypes','DIM_ChargesTypes','Dimension','false','[English Name]','false')  
--Fields --
declare @DIM_ChargesTypesId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ChargesTypesId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ChargesTypesId_NumberNewId,0,'DIM_ChargesTypes','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false','false','false','false','false')  
declare @DIM_ChargesTypesIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ChargesTypesIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ChargesTypesIdNewId,0,'DIM_ChargesTypes','[Id]','Id','Text','true',0,15,'false','false','false','false','false','false','true','false')  
declare @DIM_ChargesTypesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ChargesTypesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ChargesTypesCodeNewId,0,'DIM_ChargesTypes','[Code]','Code','Text','false',0,15,'false','false','true','[English Name],[Local Name]','false','false','false','false','false')  
declare @DIM_ChargesTypesEnglishNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ChargesTypesEnglishNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ChargesTypesEnglishNameNewId,0,'DIM_ChargesTypes','[English Name]','English Name','Text','true',0,40,'false','false','true','[Code]','false','true','false','false','false')  
declare @DIM_ChargesTypesLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ChargesTypesLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ChargesTypesLocalNameNewId,0,'DIM_ChargesTypes','[Local Name]','Local Name','nText','false',0,40,'false','false','true','false','true','false','false','false')  
declare @DIM_ChargesTypesChargeGroupCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ChargesTypesChargeGroupCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ChargesTypesChargeGroupCodeNewId,0,'DIM_ChargesTypes','[Charge Group Code]','Charge Group Code','Text','false',0,5,'false','false','true','false','false','false','false','false')  
declare @DIM_ChargesTypesParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ChargesTypesParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ChargesTypesParentTenantNewId,0,'DIM_ChargesTypes','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false','false','false','true','false')  
declare @DIM_ChargesTypesSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ChargesTypesSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@DIM_ChargesTypesSourceTenantNewId,0,'DIM_ChargesTypes','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false','false','false','Tenant','false','false')  
declare @DIM_ChargesTypesIsExpenseNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ChargesTypesIsExpenseNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@DIM_ChargesTypesIsExpenseNewId,0,'DIM_ChargesTypes','[Is Expense]','Is Expense','Boolean','false',0,0,'false','false','true','Charges','false','false','false','ChargesType.IsExpense','false','false')  
declare @DIM_ChargesTypesAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ChargesTypesAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ChargesTypesAutomaticLastUpdateDateNewId,0,'DIM_ChargesTypes','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','true','false','false','false','false','false')  
declare @DIM_ChargesTypesInActiveNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ChargesTypesInActiveNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ChargesTypesInActiveNewId,0,'DIM_ChargesTypes','[InActive]','InActive','Boolean','false',0,0,'false','false','true','false','false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_CountriesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CountriesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_CountriesNewId,0,'','DIM_Countries','DIM_Countries','Dimension','false','[Name]','false')  
--Fields --
declare @DIM_CountriesId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CountriesId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_CountriesId_NumberNewId,0,'DIM_Countries','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false','false','false','false','false')  
declare @DIM_CountriesIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CountriesIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_CountriesIdNewId,0,'DIM_Countries','[Id]','Id','Text','true',0,15,'false','false','false','false','false','false','true','false')  
declare @DIM_CountriesNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CountriesNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_CountriesNameNewId,0,'DIM_Countries','[Name]','Name','nText','true',0,120,'false','false','true','[Code]','false','true','false','false','false')  
declare @DIM_CountriesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CountriesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_CountriesCodeNewId,0,'DIM_Countries','[Code]','Code','Text','true',0,2,'false','false','true','[Name]','false','false','false','false','false')  
declare @DIM_CountriesSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CountriesSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@DIM_CountriesSourceTenantNewId,0,'DIM_Countries','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false','false','false','Tenant','false','false')  
declare @DIM_CountriesAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CountriesAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_CountriesAutomaticLastUpdateDateNewId,0,'DIM_Countries','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','true','false','false','false','false','false')  
declare @DIM_CountriesParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CountriesParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_CountriesParentTenantNewId,0,'DIM_Countries','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false','false','false','true','false')  
------------------------------------------------------------------------------------
declare @DIM_CurrenciesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_CurrenciesNewId,0,'','DIM_Currencies','DIM_Currencies','Dimension','false','[Name]','false')  
--Fields --
declare @DIM_CurrenciesId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_CurrenciesId_NumberNewId,0,'DIM_Currencies','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false','false','false','false','false')  
declare @DIM_CurrenciesIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_CurrenciesIdNewId,0,'DIM_Currencies','[Id]','Id','Text','true',0,15,'false','false','false','false','false','false','true','false')  
declare @DIM_CurrenciesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_CurrenciesCodeNewId,0,'DIM_Currencies','[Code]','Code','Text','true',0,3,'false','false','true','[Name],[Local Name]','false','false','false','false','false')  
declare @DIM_CurrenciesNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_CurrenciesNameNewId,0,'DIM_Currencies','[Name]','Name','Text','true',0,40,'false','false','true','[Code]','false','true','false','false','false')  
declare @DIM_CurrenciesLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_CurrenciesLocalNameNewId,0,'DIM_Currencies','[Local Name]','Local Name','nText','false',0,40,'false','false','true','false','true','false','false','false')  
declare @DIM_CurrenciesCurrencySignNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesCurrencySignNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_CurrenciesCurrencySignNewId,0,'DIM_Currencies','[Currency Sign]','Currency Sign','nText','false',0,3,'false','false','true','false','false','false','false','false')  
declare @DIM_CurrenciesSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@DIM_CurrenciesSourceTenantNewId,0,'DIM_Currencies','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false','false','false','Tenant','false','false')  
declare @DIM_CurrenciesParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_CurrenciesParentTenantNewId,0,'DIM_Currencies','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false','false','false','true','false')  
declare @DIM_CurrenciesAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_CurrenciesAutomaticLastUpdateDateNewId,0,'DIM_Currencies','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','true','false','false','false','false','false')  
declare @DIM_CurrenciesInActiveNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesInActiveNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_CurrenciesInActiveNewId,0,'DIM_Currencies','[InActive]','InActive','Boolean','false',0,0,'false','false','true','false','false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_CustomPickListsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CustomPickListsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_CustomPickListsNewId,0,'','DIM_CustomPickLists','DIM_CustomPickLists','Dimension','false','[Value]','false')  
--Fields --
declare @DIM_CustomPickListsIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CustomPickListsIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_CustomPickListsIdNewId,0,'DIM_CustomPickLists','[Id]','Id','Text','true',0,15,'true','false','false','false','false','false','false','false')  
declare @DIM_CustomPickListsCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CustomPickListsCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_CustomPickListsCodeNewId,0,'DIM_CustomPickLists','[Code]','Code','nText','true',0,100,'false','false','true','false','false','false','true','false')  
declare @DIM_CustomPickListsValueNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CustomPickListsValueNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_CustomPickListsValueNewId,0,'DIM_CustomPickLists','[Value]','Value','nText','true',0,1000,'false','false','true','false','false','false','false','false')  
declare @DIM_CustomPickListsIsMultipleChoiceNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CustomPickListsIsMultipleChoiceNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_CustomPickListsIsMultipleChoiceNewId,0,'DIM_CustomPickLists','[Is Multiple Choice]','Is Multiple Choice','Boolean','false',0,0,'false','false','true','false','false','false','true','false')  
declare @DIM_CustomPickListsSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CustomPickListsSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@DIM_CustomPickListsSourceTenantNewId,0,'DIM_CustomPickLists','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false','false','false','Tenant','false','false')  
declare @DIM_CustomPickListsParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CustomPickListsParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_CustomPickListsParentTenantNewId,0,'DIM_CustomPickLists','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false','false','false','true','false')  
declare @DIM_CustomPickListsAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CustomPickListsAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_CustomPickListsAutomaticLastUpdateDateNewId,0,'DIM_CustomPickLists','[Automatic Last Update Date]','Automatic Last Update Date','DateTime','false',0,0,'false','false','true','false','false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_DatesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy) Values(@DIM_DatesNewId,0,'','DIM_Dates','DIM_Dates','Dimension','false','[Full Date]')  
--Fields --
declare @DIM_DatesDateKeyNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesDateKeyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom) Values(@DIM_DatesDateKeyNewId,0,'DIM_Dates','[Date Key]','Date Key','DateTime','true',0,0,'true','false','true','false','false','false')  
declare @DIM_DatesFullDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesFullDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom) Values(@DIM_DatesFullDateNewId,0,'DIM_Dates','[Full Date]','Full Date','DateTime','false',0,0,'false','false','true','false','false','false')  
declare @DIM_DatesFullDateUSNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesFullDateUSNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom) Values(@DIM_DatesFullDateUSNewId,0,'DIM_Dates','[Full Date US]','Full Date US','Text','false',0,100,'false','false','true','false','false','false')  
declare @DIM_DatesDayOfWeekNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesDayOfWeekNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom) Values(@DIM_DatesDayOfWeekNewId,0,'DIM_Dates','[Day Of Week]','Day Of Week','Integer','false',0,0,'false','false','true','false','false','false')  
declare @DIM_DatesDayNumInMonthNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesDayNumInMonthNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom) Values(@DIM_DatesDayNumInMonthNewId,0,'DIM_Dates','[Day Num In Month]','Day Num In Month','Integer','false',0,0,'false','false','true','false','false','false')  
declare @DIM_DatesDayNumOverallNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesDayNumOverallNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom) Values(@DIM_DatesDayNumOverallNewId,0,'DIM_Dates','[Day Num Overall]','Day Num Overall','Integer','false',0,0,'false','false','true','false','false','false')  
declare @DIM_DatesDayNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesDayNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom) Values(@DIM_DatesDayNameNewId,0,'DIM_Dates','[Day Name]','Day Name','Text','false',0,13,'false','false','true','false','false','false')  
declare @DIM_DatesDayAbbrevNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesDayAbbrevNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom) Values(@DIM_DatesDayAbbrevNewId,0,'DIM_Dates','[Day Abbrev]','Day Abbrev','Text','false',0,3,'false','false','true','false','false','false')  
declare @DIM_DatesWeekNumInYearNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesWeekNumInYearNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom) Values(@DIM_DatesWeekNumInYearNewId,0,'DIM_Dates','[Week Num In Year]','Week Num In Year','Integer','false',0,0,'false','false','true','false','false','false')  
declare @DIM_DatesWeekNumOverallNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesWeekNumOverallNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom) Values(@DIM_DatesWeekNumOverallNewId,0,'DIM_Dates','[Week Num Overall]','Week Num Overall','Integer','false',0,0,'false','false','true','false','false','false')  
declare @DIM_DatesMonthNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesMonthNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom) Values(@DIM_DatesMonthNewId,0,'DIM_Dates','[Month]','Month','Integer','false',0,0,'false','false','true','false','false','false')  
declare @DIM_DatesMonthNumOverallNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesMonthNumOverallNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom) Values(@DIM_DatesMonthNumOverallNewId,0,'DIM_Dates','[Month Num Overall]','Month Num Overall','Integer','false',0,0,'false','false','true','false','false','false')  
declare @DIM_DatesMonthNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesMonthNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom) Values(@DIM_DatesMonthNameNewId,0,'DIM_Dates','[Month Name]','Month Name','Text','false',0,13,'false','false','true','false','false','false')  
declare @DIM_DatesMonthAbbrevNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesMonthAbbrevNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom) Values(@DIM_DatesMonthAbbrevNewId,0,'DIM_Dates','[Month Abbrev]','Month Abbrev','Text','false',0,3,'false','false','true','false','false','false')  
declare @DIM_DatesQuarterNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesQuarterNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom) Values(@DIM_DatesQuarterNewId,0,'DIM_Dates','[Quarter]','Quarter','Integer','false',0,0,'false','false','true','false','false','false')  
declare @DIM_DatesYearNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesYearNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom) Values(@DIM_DatesYearNewId,0,'DIM_Dates','[Year]','Year','Integer','false',0,0,'false','false','true','false','false','false')  
declare @DIM_DatesYearmoNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesYearmoNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom) Values(@DIM_DatesYearmoNewId,0,'DIM_Dates','[Yearmo]','Yearmo','Integer','false',0,0,'false','false','true','false','false','false')  
declare @DIM_DatesMonthEndFlagNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesMonthEndFlagNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom) Values(@DIM_DatesMonthEndFlagNewId,0,'DIM_Dates','[Month End Flag]','Month End Flag','Text','false',0,100,'false','false','true','false','false','false')  
declare @DIM_DatesDateNumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesDateNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom) Values(@DIM_DatesDateNumberNewId,0,'DIM_Dates','[Date Number]','Date Number','Integer','true',0,0,'false','false','true','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_DepartmentsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_DepartmentsNewId,0,'','DIM_Departments','DIM_Departments','Dimension','false','[Name]','false')  
--Fields --
declare @DIM_DepartmentsId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_DepartmentsId_NumberNewId,0,'DIM_Departments','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false','false','false','false','false')  
declare @DIM_DepartmentsIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_DepartmentsIdNewId,0,'DIM_Departments','[Id]','Id','Text','true',0,15,'false','false','false','false','false','false','true','false')  
declare @DIM_DepartmentsNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_DepartmentsNameNewId,0,'DIM_Departments','[Name]','Name','Text','true',0,40,'false','false','true','false','false','false','false','false')  
declare @DIM_DepartmentsLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_DepartmentsLocalNameNewId,0,'DIM_Departments','[Local Name]','Local Name','nText','false',0,40,'false','false','true','false','false','false','false','false')  
declare @DIM_DepartmentsSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@DIM_DepartmentsSourceTenantNewId,0,'DIM_Departments','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false','false','false','Tenant','false','false')  
declare @DIM_DepartmentsParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_DepartmentsParentTenantNewId,0,'DIM_Departments','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false','false','false','true','false')  
declare @DIM_DepartmentsAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_DepartmentsAutomaticLastUpdateDateNewId,0,'DIM_Departments','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','true','false','false','false','false','false')  
declare @DIM_DepartmentsInActiveNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsInActiveNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_DepartmentsInActiveNewId,0,'DIM_Departments','[InActive]','InActive','Boolean','false',0,0,'false','false','true','false','false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_DirectionsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DirectionsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_DirectionsNewId,0,'','DIM_Directions','DIM_Directions','Dimension','true','[Name]','false')  
--Fields --
declare @DIM_DirectionsCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DirectionsCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_DirectionsCodeNewId,0,'DIM_Directions','[Code]','Code','Text','true',0,1,'false','false','true','[Name]','false','false','false','true','false')  
declare @DIM_DirectionsNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DirectionsNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_DirectionsNameNewId,0,'DIM_Directions','[Name]','Name','Text','true',0,40,'true','false','true','[Code]','false','false','false','false','false')  
declare @DIM_DirectionsAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DirectionsAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_DirectionsAutomaticLastUpdateDateNewId,0,'DIM_Directions','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','false','false','false','false','true','false')  
------------------------------------------------------------------------------------
declare @DIM_IncotermsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_IncotermsNewId,0,'','DIM_Incoterms','DIM_Incoterms','Dimension','false','[Name]','false')  
--Fields --
declare @DIM_IncotermsId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_IncotermsId_NumberNewId,0,'DIM_Incoterms','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','[Code],[Name]','false','false','false','false','false')  
declare @DIM_IncotermsIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_IncotermsIdNewId,0,'DIM_Incoterms','[Id]','Id','Text','true',0,15,'false','false','false','false','false','false','true','false')  
declare @DIM_IncotermsNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_IncotermsNameNewId,0,'DIM_Incoterms','[Name]','Name','Text','true',0,40,'false','false','true','[Code]','false','true','false','false','false')  
declare @DIM_IncotermsLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_IncotermsLocalNameNewId,0,'DIM_Incoterms','[Local Name]','Local Name','nText','false',0,40,'false','false','true','false','true','false','false','false')  
declare @DIM_IncotermsCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_IncotermsCodeNewId,0,'DIM_Incoterms','[Code]','Code','Text','true',0,3,'false','false','true','[Name],[Local Name]','false','false','false','false','false')  
declare @DIM_IncotermsSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@DIM_IncotermsSourceTenantNewId,0,'DIM_Incoterms','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false','false','false','Tenant','false','false')  
declare @DIM_IncotermsParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_IncotermsParentTenantNewId,0,'DIM_Incoterms','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false','false','false','true','false')  
declare @DIM_IncotermsAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_IncotermsAutomaticLastUpdateDateNewId,0,'DIM_Incoterms','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','true','false','false','false','false','false')  
declare @DIM_IncotermsInActiveNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsInActiveNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_IncotermsInActiveNewId,0,'DIM_Incoterms','[InActive]','InActive','Boolean','false',0,0,'false','false','true','false','false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_InvoiceTypesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_InvoiceTypesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn,HasCustomFields,MaxNumberOfCustomFields) Values(@DIM_InvoiceTypesNewId,0,'','DIM_InvoiceTypes','DIM_InvoiceTypes','Dimension','false','[Name]','false','false',0)  
--Fields --
declare @DIM_InvoiceTypesId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_InvoiceTypesId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_InvoiceTypesId_NumberNewId,0,'DIM_InvoiceTypes','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false','false','false','false','false')  
declare @DIM_InvoiceTypesNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_InvoiceTypesNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_InvoiceTypesNameNewId,0,'DIM_InvoiceTypes','[Name]','Name','Text','true',0,20,'false','false','true','false','true','false','false','false')  
declare @DIM_InvoiceTypesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_InvoiceTypesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_InvoiceTypesCodeNewId,0,'DIM_InvoiceTypes','[Code]','Code','Text','true',0,2,'false','false','true','[Name]','false','false','false','false','false')  
declare @DIM_InvoiceTypesSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_InvoiceTypesSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@DIM_InvoiceTypesSourceTenantNewId,0,'DIM_InvoiceTypes','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','true','false','false','false','Tenant','false','false')  
declare @DIM_InvoiceTypesParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_InvoiceTypesParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_InvoiceTypesParentTenantNewId,0,'DIM_InvoiceTypes','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false','false','false','true','false')  
declare @DIM_InvoiceTypesLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_InvoiceTypesLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_InvoiceTypesLastUpdateDateNewId,0,'DIM_InvoiceTypes','[Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','true','false','false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_LevelsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_LevelsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_LevelsNewId,0,'','DIM_Levels','DIM_Levels','Dimension','true','[Name]','false')  
--Fields --
declare @DIM_LevelsCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_LevelsCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_LevelsCodeNewId,0,'DIM_Levels','[Code]','Code','Text','true',0,1,'false','false','true','[Name]','false','false','false','true','false')  
declare @DIM_LevelsNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_LevelsNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_LevelsNameNewId,0,'DIM_Levels','[Name]','Name','Text','true',0,40,'true','false','true','[Code]','false','false','false','false','false')  
declare @DIM_LevelsAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_LevelsAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_LevelsAutomaticLastUpdateDateNewId,0,'DIM_Levels','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','false','false','false','false','true','false')  
------------------------------------------------------------------------------------
declare @DIM_MoveTypesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_MoveTypesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_MoveTypesNewId,0,'','DIM_MoveTypes','DIM_MoveTypes','Dimension','false','[English Name]','false')  
--Fields --
declare @DIM_MoveTypesId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_MoveTypesId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_MoveTypesId_NumberNewId,0,'DIM_MoveTypes','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','[Code],[Name]','false','false','false','false','false')  
declare @DIM_MoveTypesIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_MoveTypesIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_MoveTypesIdNewId,0,'DIM_MoveTypes','[Id]','Id','Text','true',0,15,'false','false','false','false','false','false','true','false')  
declare @DIM_MoveTypesEnglishNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_MoveTypesEnglishNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_MoveTypesEnglishNameNewId,0,'DIM_MoveTypes','[English Name]','English Name','Text','true',0,40,'false','false','true','false','true','false','false','false')  
declare @DIM_MoveTypesLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_MoveTypesLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_MoveTypesLocalNameNewId,0,'DIM_MoveTypes','[Local Name]','Local Name','Text','true',0,40,'false','false','true','false','true','false','false','false')  
declare @DIM_MoveTypesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_MoveTypesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_MoveTypesCodeNewId,0,'DIM_MoveTypes','[Code]','Code','Text','true',0,3,'false','false','true','[English Name],[Local Name]','false','false','false','false','false')  
declare @DIM_MoveTypesTransportModeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_MoveTypesTransportModeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_MoveTypesTransportModeNewId,0,'DIM_MoveTypes','[Transport Mode]','Transport Mode','Text','false',0,1,'false','false','true','false','false','false','true','false')  
declare @DIM_MoveTypesSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_MoveTypesSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@DIM_MoveTypesSourceTenantNewId,0,'DIM_MoveTypes','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false','false','false','Tenant','false','false')  
declare @DIM_MoveTypesParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_MoveTypesParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_MoveTypesParentTenantNewId,0,'DIM_MoveTypes','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false','false','false','true','false')  
declare @DIM_MoveTypesAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_MoveTypesAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_MoveTypesAutomaticLastUpdateDateNewId,0,'DIM_MoveTypes','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','true','false','false','false','false','false')  
declare @DIM_MoveTypesInActiveNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_MoveTypesInActiveNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_MoveTypesInActiveNewId,0,'DIM_MoveTypes','[InActive]','InActive','Boolean','false',0,0,'false','false','true','false','false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_OBLTypesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_OBLTypesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_OBLTypesNewId,0,'','DIM_OBLTypes','DIM_OBLTypes','Dimension','true','[Name]','false')  
--Fields --
declare @DIM_OBLTypesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_OBLTypesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_OBLTypesCodeNewId,0,'DIM_OBLTypes','[Code]','Code','Text','true',0,4,'false','false','true','[Name]','false','false','false','true','false')  
declare @DIM_OBLTypesNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_OBLTypesNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_OBLTypesNameNewId,0,'DIM_OBLTypes','[Name]','Name','Text','true',0,25,'true','false','true','[Code]','false','false','false','false','false')  
declare @DIM_OBLTypesAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_OBLTypesAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_OBLTypesAutomaticLastUpdateDateNewId,0,'DIM_OBLTypes','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','false','false','false','false','true','false')  
------------------------------------------------------------------------------------
declare @DIM_PartnersNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn,HasCustomFields,MaxNumberOfCustomFields) Values(@DIM_PartnersNewId,0,'<ArrayOfIndexItem xmlns:i="http://www.w3.org/2001/XMLSchema-instance"><IndexItem> <Columns>[InActive],[Source Tenant]</Columns></IndexItem></ArrayOfIndexItem>','DIM_Partners','DIM_Partners','Dimension','false','[Name]','false','false',0)  
--Fields --
declare @DIM_PartnersId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersId_NumberNewId,0,'DIM_Partners','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false','false','false','false','false')  
declare @DIM_PartnersIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersIdNewId,0,'DIM_Partners','[Id]','Id','Text','true',0,15,'false','false','false','false','false','false','true','false')  
declare @DIM_PartnersNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersNameNewId,0,'DIM_Partners','[Name]','Name','Text','true',0,70,'false','false','true','[Code],[Partner Type]','false','true','false','false','false')  
declare @DIM_PartnersLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersLocalNameNewId,0,'DIM_Partners','[Local Name]','Local Name','nText','false',0,100,'false','false','true','false','true','false','false','false')  
declare @DIM_PartnersCityNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersCityNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersCityNewId,0,'DIM_Partners','[City]','City','nText','false',0,25,'false','false','true','false','false','false','false','false')  
declare @DIM_PartnersCountryNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersCountryNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersCountryNewId,0,'DIM_Partners','[Country]','Country','Text','false',0,120,'false','false','true','false','false','false','false','false')  
declare @DIM_PartnersStateNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersStateNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersStateNameNewId,0,'DIM_Partners','[State Name]','State Name','Text','false',0,40,'false','false','true','false','false','false','false','false')  
declare @DIM_PartnersZipCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersZipCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersZipCodeNewId,0,'DIM_Partners','[Zip Code]','Zip Code','Text','false',0,15,'false','false','true','false','false','false','false','false')  
declare @DIM_PartnersPrimaryContactNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersPrimaryContactNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersPrimaryContactNewId,0,'DIM_Partners','[Primary Contact]','Primary Contact','Text','false',0,60,'false','false','true','false','false','false','false','false')  
declare @DIM_PartnersAccountManagerNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersAccountManagerNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersAccountManagerNewId,0,'DIM_Partners','[Account Manager]','Account Manager','Text','false',0,60,'false','false','true','false','false','false','false','false')  
declare @DIM_PartnersSalesmanNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersSalesmanNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection,RecordType) Values(@DIM_PartnersSalesmanNewId,0,'DIM_Partners','[Salesman]','Salesman','Text','false',0,60,'false','false','true','false','false','false','false','false','Customer')  
declare @DIM_PartnersCustomerRankNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersCustomerRankNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection,RecordType) Values(@DIM_PartnersCustomerRankNewId,0,'DIM_Partners','[Customer Rank]','Rank','Text','false',0,40,'false','false','true','false','false','false','false','false','Customer')  
declare @DIM_PartnersPartnerTypeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersPartnerTypeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersPartnerTypeNewId,0,'DIM_Partners','[Partner Type]','Partner Type','Text','true',0,20,'false','false','true','false','false','false','false','false')  
declare @DIM_PartnersSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersSourceTenantNewId,0,'DIM_Partners','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false','false','false','Tenant','false','false')  
declare @DIM_PartnersParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersParentTenantNewId,0,'DIM_Partners','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false','false','false','true','false')  
declare @DIM_PartnersCountryCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersCountryCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersCountryCodeNewId,0,'DIM_Partners','[Country Code]','Country Code','Text','false',0,2,'false','false','true','false','false','false','false','false')  
declare @DIM_PartnersPrimaryContactEmailNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersPrimaryContactEmailNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersPrimaryContactEmailNewId,0,'DIM_Partners','[Primary Contact Email]','Primary Contact Email','Text','false',0,70,'false','false','true','false','false','false','false','false')  
declare @DIM_PartnersReceivablesAccountingCardNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersReceivablesAccountingCardNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersReceivablesAccountingCardNewId,0,'DIM_Partners','[Receivables Accounting Card]','Receivables Accounting Card','Text','false',0,25,'false','false','true','false','false','false','false','false')  
declare @DIM_PartnersCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersCodeNewId,0,'DIM_Partners','[Code]','Code','Text','true',0,15,'false','false','true','[Code],[Local Name],[Partner Type]','false','false','false','false','false')  
declare @DIM_PartnersAddress1NewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersAddress1NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersAddress1NewId,0,'DIM_Partners','[Address1]','Address1','Text','false',0,65,'false','false','true','Operational ','false','false','false','false','false')  
declare @DIM_PartnersAddress2NewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersAddress2NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersAddress2NewId,0,'DIM_Partners','[Address2]','Address2','Text','false',0,65,'false','false','true','Operational ','false','false','false','false','false')  
declare @DIM_PartnersPhoneNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersPhoneNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersPhoneNewId,0,'DIM_Partners','[Phone]','Phone','Text','false',0,40,'false','false','true','Partners','false','false','false','false','false')  
declare @DIM_PartnersIndustryNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersIndustryNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection,RecordType) Values(@DIM_PartnersIndustryNewId,0,'DIM_Partners','[Industry]','Industry','nText','false',0,60,'false','false','true','false','false','false','false','false','Customer')  
declare @DIM_PartnersCustomerSizeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersCustomerSizeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection,RecordType) Values(@DIM_PartnersCustomerSizeNewId,0,'DIM_Partners','[Customer Size]','Size','nText','false',0,60,'false','false','true','false','false','false','false','false','Customer')  
declare @DIM_PartnersRegionNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersRegionNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection,RecordType) Values(@DIM_PartnersRegionNewId,0,'DIM_Partners','[Region]','Region','Text','false',0,100,'false','false','true','false','false','false','false','false','Customer')  
declare @DIM_PartnersVATNumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersVATNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersVATNumberNewId,0,'DIM_Partners','[VAT Number ]','VAT Number ','Text','false',0,20,'false','false','true','false','false','false','false','false')  
declare @DIM_PartnersCreditLimitAmountLocalNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersCreditLimitAmountLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection,RecordType) Values(@DIM_PartnersCreditLimitAmountLocalNewId,0,'DIM_Partners','[Credit Limit Amount (Local)]','Credit Limit Amount (Local)','Decimal','false',0,0,'false','false','true','false','false','false','false','false','Customer')  
declare @DIM_PartnersOpenBalanceLocalNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersOpenBalanceLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection,RecordType) Values(@DIM_PartnersOpenBalanceLocalNewId,0,'DIM_Partners','[Open Balance (Local)]','Open Balance (Local)','Decimal','false',0,0,'false','false','true','false','false','false','false','false','Customer')  
declare @DIM_PartnersLeadSourceNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersLeadSourceNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection,RecordType) Values(@DIM_PartnersLeadSourceNewId,0,'DIM_Partners','[Lead Source]','Lead Source','Text','false',0,60,'false','false','true','false','false','false','false','false','Customer')  
declare @DIM_PartnersAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersAutomaticLastUpdateDateNewId,0,'DIM_Partners','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','true','false','false','false','false','false')  
declare @DIM_PartnersInActiveNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersInActiveNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersInActiveNewId,0,'DIM_Partners','[InActive]','InActive','Boolean','false',0,0,'false','false','true','false','false','false','false','false')  
declare @DIM_PartnersCustomerFirstShipmentDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersCustomerFirstShipmentDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection,RecordType) Values(@DIM_PartnersCustomerFirstShipmentDateNewId,0,'DIM_Partners','[Customer First Shipment Date]','First Shipment Date','DateTime','false',0,0,'false','false','true','false','false','false','false','false','Customer')  
declare @DIM_PartnersCustomerLastShipmentDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersCustomerLastShipmentDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection,RecordType) Values(@DIM_PartnersCustomerLastShipmentDateNewId,0,'DIM_Partners','[Customer Last Shipment Date]','Last Shipment Date','DateTime','false',0,0,'false','false','true','false','false','false','false','false','Customer')  
declare @DIM_PartnersBillToNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersBillToNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PartnersBillToNewId,0,'DIM_Partners','[Bill To]','Bill To','Text','false',0,70,'false','false','true','false','false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_PaymentTermsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PaymentTermsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn,HasCustomFields,MaxNumberOfCustomFields) Values(@DIM_PaymentTermsNewId,0,'','DIM_PaymentTerms','DIM_PaymentTerms','Dimension','false','[Name]','false','false',0)  
--Fields --
declare @DIM_PaymentTermsId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PaymentTermsId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PaymentTermsId_NumberNewId,0,'DIM_PaymentTerms','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false','false','false','false','false')  
declare @DIM_PaymentTermsIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PaymentTermsIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PaymentTermsIdNewId,0,'DIM_PaymentTerms','[Id]','Id','Text','true',0,15,'false','false','false','false','false','false','true','false')  
declare @DIM_PaymentTermsEnglishNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PaymentTermsEnglishNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PaymentTermsEnglishNameNewId,0,'DIM_PaymentTerms','[English Name]','English Name','Text','true',0,40,'false','false','true','false','true','false','false','false')  
declare @DIM_PaymentTermsLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PaymentTermsLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PaymentTermsLocalNameNewId,0,'DIM_PaymentTerms','[Local Name]','Local Name','Text','false',0,40,'false','false','true','false','true','false','false','false')  
declare @DIM_PaymentTermsCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PaymentTermsCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PaymentTermsCodeNewId,0,'DIM_PaymentTerms','[Code]','Code','Text','true',0,4,'false','false','true','[Name]','false','false','false','false','false')  
declare @DIM_PaymentTermsSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PaymentTermsSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@DIM_PaymentTermsSourceTenantNewId,0,'DIM_PaymentTerms','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false','false','false','Tenant','false','false')  
declare @DIM_PaymentTermsParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PaymentTermsParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PaymentTermsParentTenantNewId,0,'DIM_PaymentTerms','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false','false','false','true','false')  
declare @DIM_PaymentTermsAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PaymentTermsAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PaymentTermsAutomaticLastUpdateDateNewId,0,'DIM_PaymentTerms','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','true','false','false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_PortsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_PortsNewId,0,'','DIM_Ports','DIM_Ports','Dimension','false','[Name]','false')  
--Fields --
declare @DIM_PortsId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PortsId_NumberNewId,0,'DIM_Ports','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false','false','false','false','false')  
declare @DIM_PortsIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PortsIdNewId,0,'DIM_Ports','[Id]','Id','Text','true',0,15,'false','false','false','false','false','false','true','false')  
declare @DIM_PortsNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PortsNameNewId,0,'DIM_Ports','[Name]','Name','Text','true',0,40,'false','false','true','[Code]','false','true','false','false','false')  
declare @DIM_PortsCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PortsCodeNewId,0,'DIM_Ports','[Code]','Code','Text','true',0,3,'false','false','true','[Name],[Local Name]','false','false','false','false','false')  
declare @DIM_PortsLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PortsLocalNameNewId,0,'DIM_Ports','[Local Name]','Local Name','nText','false',0,40,'false','false','true','false','true','false','false','false')  
declare @DIM_PortsUNLocCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsUNLocCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PortsUNLocCodeNewId,0,'DIM_Ports','[UN Loc Code]','UN Loc Code','Text','false',0,30,'false','false','true','false','false','false','false','false')  
declare @DIM_PortsCountryNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsCountryNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PortsCountryNewId,0,'DIM_Ports','[Country]','Country','Text','true',0,120,'false','false','true','false','false','false','false','false')  
declare @DIM_PortsStateNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsStateNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PortsStateNameNewId,0,'DIM_Ports','[State Name]','State Name','Text','false',0,40,'false','false','true','false','false','false','false','false')  
declare @DIM_PortsSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@DIM_PortsSourceTenantNewId,0,'DIM_Ports','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false','false','false','Tenant','false','false')  
declare @DIM_PortsParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PortsParentTenantNewId,0,'DIM_Ports','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false','false','false','true','false')  
declare @DIM_PortsAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PortsAutomaticLastUpdateDateNewId,0,'DIM_Ports','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','true','false','false','false','false','false')  
declare @DIM_PortsInActiveNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsInActiveNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_PortsInActiveNewId,0,'DIM_Ports','[InActive]','InActive','Boolean','false',0,0,'false','false','true','false','false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_QuoteClosingReasonsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_QuoteClosingReasonsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_QuoteClosingReasonsNewId,0,'','DIM_QuoteClosingReasons','DIM_QuoteClosingReasons','Dimension','false','[Name]','false')  
--Fields --
declare @DIM_QuoteClosingReasonsId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_QuoteClosingReasonsId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_QuoteClosingReasonsId_NumberNewId,0,'DIM_QuoteClosingReasons','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false','false','false','false','false')  
declare @DIM_QuoteClosingReasonsIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_QuoteClosingReasonsIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_QuoteClosingReasonsIdNewId,0,'DIM_QuoteClosingReasons','[Id]','Id','Text','true',0,15,'false','false','false','false','false','false','true','false')  
declare @DIM_QuoteClosingReasonsNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_QuoteClosingReasonsNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_QuoteClosingReasonsNameNewId,0,'DIM_QuoteClosingReasons','[Name]','Name','Text','true',0,60,'false','false','true','[Code]','false','true','false','false','false')  
declare @DIM_QuoteClosingReasonsCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_QuoteClosingReasonsCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_QuoteClosingReasonsCodeNewId,0,'DIM_QuoteClosingReasons','[Code]','Code','Text','true',0,2,'false','false','true','[Name]','false','false','false','false','false')  
declare @DIM_QuoteClosingReasonsSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_QuoteClosingReasonsSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@DIM_QuoteClosingReasonsSourceTenantNewId,0,'DIM_QuoteClosingReasons','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false','false','false','Tenant','false','false')  
declare @DIM_QuoteClosingReasonsAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_QuoteClosingReasonsAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_QuoteClosingReasonsAutomaticLastUpdateDateNewId,0,'DIM_QuoteClosingReasons','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','true','false','false','false','false','false')  
declare @DIM_QuoteClosingReasonsParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_QuoteClosingReasonsParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_QuoteClosingReasonsParentTenantNewId,0,'DIM_QuoteClosingReasons','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false','false','false','true','false')  
------------------------------------------------------------------------------------
declare @DIM_QuoteStagesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_QuoteStagesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_QuoteStagesNewId,0,'','DIM_QuoteStages','DIM_QuoteStages','Dimension','false','[Name]','false')  
--Fields --
declare @DIM_QuoteStagesId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_QuoteStagesId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_QuoteStagesId_NumberNewId,0,'DIM_QuoteStages','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false','false','false','false','false')  
declare @DIM_QuoteStagesIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_QuoteStagesIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_QuoteStagesIdNewId,0,'DIM_QuoteStages','[Id]','Id','Text','true',0,15,'false','false','false','false','false','false','true','false')  
declare @DIM_QuoteStagesNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_QuoteStagesNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_QuoteStagesNameNewId,0,'DIM_QuoteStages','[Name]','Name','Text','true',0,40,'false','false','true','[Code]','false','true','false','false','false')  
declare @DIM_QuoteStagesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_QuoteStagesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_QuoteStagesCodeNewId,0,'DIM_QuoteStages','[Code]','Code','Text','true',0,4,'false','false','true','[Name]','false','false','false','false','false')  
declare @DIM_QuoteStagesSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_QuoteStagesSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@DIM_QuoteStagesSourceTenantNewId,0,'DIM_QuoteStages','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false','false','false','Tenant','false','false')  
declare @DIM_QuoteStagesAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_QuoteStagesAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_QuoteStagesAutomaticLastUpdateDateNewId,0,'DIM_QuoteStages','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','true','false','false','false','false','false')  
declare @DIM_QuoteStagesParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_QuoteStagesParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_QuoteStagesParentTenantNewId,0,'DIM_QuoteStages','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false','false','false','true','false')  
------------------------------------------------------------------------------------
declare @DIM_ShipmentPayableStatusesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentPayableStatusesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_ShipmentPayableStatusesNewId,0,'','DIM_ShipmentPayableStatuses','DIM_ShipmentPayableStatuses','Dimension','true','[Name]','false')  
--Fields --
declare @DIM_ShipmentPayableStatusesNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentPayableStatusesNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ShipmentPayableStatusesNameNewId,0,'DIM_ShipmentPayableStatuses','[Name]','Name','Text','true',0,40,'true','false','true','[Code]','false','false','false','false','false')  
declare @DIM_ShipmentPayableStatusesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentPayableStatusesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ShipmentPayableStatusesCodeNewId,0,'DIM_ShipmentPayableStatuses','[Code]','Code','Text','true',0,4,'false','false','true','[Name]','false','false','false','true','false')  
declare @DIM_ShipmentPayableStatusesAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentPayableStatusesAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ShipmentPayableStatusesAutomaticLastUpdateDateNewId,0,'DIM_ShipmentPayableStatuses','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','false','false','false','false','true','false')  
------------------------------------------------------------------------------------
declare @DIM_ShipmentReceivableStatusesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentReceivableStatusesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_ShipmentReceivableStatusesNewId,0,'','DIM_ShipmentReceivableStatuses','DIM_ShipmentReceivableStatuses','Dimension','true','[Name]','false')  
--Fields --
declare @DIM_ShipmentReceivableStatusesNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentReceivableStatusesNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ShipmentReceivableStatusesNameNewId,0,'DIM_ShipmentReceivableStatuses','[Name]','Name','Text','true',0,40,'true','false','true','[Code]','false','false','false','false','false')  
declare @DIM_ShipmentReceivableStatusesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentReceivableStatusesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ShipmentReceivableStatusesCodeNewId,0,'DIM_ShipmentReceivableStatuses','[Code]','Code','Text','true',0,4,'false','false','true','[Name]','false','false','false','true','false')  
declare @DIM_ShipmentReceivableStatusesAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentReceivableStatusesAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ShipmentReceivableStatusesAutomaticLastUpdateDateNewId,0,'DIM_ShipmentReceivableStatuses','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','false','false','false','false','true','false')  
------------------------------------------------------------------------------------
declare @DIM_ShipmentStatusesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_ShipmentStatusesNewId,0,'','DIM_ShipmentStatuses','DIM_ShipmentStatuses','Dimension','false','[Name]','false')  
--Fields --
declare @DIM_ShipmentStatusesId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ShipmentStatusesId_NumberNewId,0,'DIM_ShipmentStatuses','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false','false','false','false','false')  
declare @DIM_ShipmentStatusesIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ShipmentStatusesIdNewId,0,'DIM_ShipmentStatuses','[Id]','Id','Text','true',0,15,'false','false','false','false','false','false','true','false')  
declare @DIM_ShipmentStatusesNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ShipmentStatusesNameNewId,0,'DIM_ShipmentStatuses','[Name]','Name','Text','true',0,40,'false','false','true','[Code]','false','true','false','false','false')  
declare @DIM_ShipmentStatusesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ShipmentStatusesCodeNewId,0,'DIM_ShipmentStatuses','[Code]','Code','Text','true',0,4,'false','false','true','[Name]','false','false','false','false','false')  
declare @DIM_ShipmentStatusesSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@DIM_ShipmentStatusesSourceTenantNewId,0,'DIM_ShipmentStatuses','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false','false','false','Tenant','false','false')  
declare @DIM_ShipmentStatusesParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ShipmentStatusesParentTenantNewId,0,'DIM_ShipmentStatuses','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false','false','false','true','false')  
declare @DIM_ShipmentStatusesStatusWeightNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesStatusWeightNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ShipmentStatusesStatusWeightNewId,0,'DIM_ShipmentStatuses','[Status Weight]','Status Weight','Integer','false',0,0,'false','false','false','false','false','false','false','false')  
declare @DIM_ShipmentStatusesAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_ShipmentStatusesAutomaticLastUpdateDateNewId,0,'DIM_ShipmentStatuses','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','true','false','false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_SpecialServicesTypesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_SpecialServicesTypesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_SpecialServicesTypesNewId,0,'','DIM_SpecialServicesTypes','DIM_SpecialServicesTypes','Dimension','false','[English Name]','false')  
--Fields --
declare @DIM_SpecialServicesTypesIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_SpecialServicesTypesIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_SpecialServicesTypesIdNewId,0,'DIM_SpecialServicesTypes','[Id]','Id','Text','true',0,15,'false','false','true','false','false','false','true','false')  
declare @DIM_SpecialServicesTypesId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_SpecialServicesTypesId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_SpecialServicesTypesId_NumberNewId,0,'DIM_SpecialServicesTypes','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false','false','false','false','false')  
declare @DIM_SpecialServicesTypesEnglishNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_SpecialServicesTypesEnglishNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_SpecialServicesTypesEnglishNameNewId,0,'DIM_SpecialServicesTypes','[English Name]','English Name','nText','true',0,100,'false','false','true','[Code]','false','true','false','false','false')  
declare @DIM_SpecialServicesTypesLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_SpecialServicesTypesLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_SpecialServicesTypesLocalNameNewId,0,'DIM_SpecialServicesTypes','[Local Name]','Local Name','nText','false',0,100,'false','false','true','false','true','false','false','false')  
declare @DIM_SpecialServicesTypesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_SpecialServicesTypesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_SpecialServicesTypesCodeNewId,0,'DIM_SpecialServicesTypes','[Code]','Code','Text','true',0,8,'false','false','true','[English Name],[Local Name]','false','false','false','false','false')  
declare @DIM_SpecialServicesTypesSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_SpecialServicesTypesSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@DIM_SpecialServicesTypesSourceTenantNewId,0,'DIM_SpecialServicesTypes','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false','false','false','Tenant','false','false')  
declare @DIM_SpecialServicesTypesParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_SpecialServicesTypesParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_SpecialServicesTypesParentTenantNewId,0,'DIM_SpecialServicesTypes','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false','false','false','true','false')  
declare @DIM_SpecialServicesTypesAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_SpecialServicesTypesAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_SpecialServicesTypesAutomaticLastUpdateDateNewId,0,'DIM_SpecialServicesTypes','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','true','false','false','false','false','false')  
declare @DIM_SpecialServicesTypesInActiveNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_SpecialServicesTypesInActiveNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_SpecialServicesTypesInActiveNewId,0,'DIM_SpecialServicesTypes','[InActive]','InActive','Boolean','false',0,0,'false','false','true','false','false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_TenantsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TenantsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_TenantsNewId,0,'','DIM_Tenants','DIM_Tenants','Dimension','false','[Tenant Name]','false')  
--Fields --
declare @DIM_TenantsTenantNumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TenantsTenantNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_TenantsTenantNumberNewId,0,'DIM_Tenants','[Tenant Number]','Tenant Number','Integer','true',0,0,'true','false','true','[Tenant Name]','false','false','false','false','false')  
declare @DIM_TenantsTenantNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TenantsTenantNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_TenantsTenantNameNewId,0,'DIM_Tenants','[Tenant Name]','Tenant Name','Text','true',0,100,'false','false','true','[Tenant Number]','false','false','false','false','false')  
declare @DIM_TenantsCountryNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TenantsCountryNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_TenantsCountryNewId,0,'DIM_Tenants','[Country]','Country','Text','false',0,120,'false','false','true','false','false','false','false','false')  
declare @DIM_TenantsAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TenantsAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_TenantsAutomaticLastUpdateDateNewId,0,'DIM_Tenants','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','true','false','false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_TransportModesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TransportModesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_TransportModesNewId,0,'','DIM_TransportModes','DIM_TransportModes','Dimension','true','[Name]','false')  
--Fields --
declare @DIM_TransportModesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TransportModesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_TransportModesCodeNewId,0,'DIM_TransportModes','[Code]','Code','Text','true',0,1,'false','false','true','[Name]','false','false','false','true','false')  
declare @DIM_TransportModesNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TransportModesNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_TransportModesNameNewId,0,'DIM_TransportModes','[Name]','Name','Text','true',0,13,'true','false','true','[Code]','false','false','false','false','false')  
declare @DIM_TransportModesAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TransportModesAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_TransportModesAutomaticLastUpdateDateNewId,0,'DIM_TransportModes','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','false','false','false','false','true','false')  
------------------------------------------------------------------------------------
declare @DIM_TypesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TypesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_TypesNewId,0,'','DIM_Types','DIM_Types','Dimension','true','[Name]','false')  
--Fields --
declare @DIM_TypesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TypesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_TypesCodeNewId,0,'DIM_Types','[Code]','Code','Text','true',0,4,'false','false','true','[Name]','false','false','false','true','false')  
declare @DIM_TypesNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TypesNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_TypesNameNewId,0,'DIM_Types','[Name]','Name','Text','true',0,40,'true','false','true','[Code]','false','false','false','false','false')  
declare @DIM_TypesAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TypesAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_TypesAutomaticLastUpdateDateNewId,0,'DIM_Types','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','false','false','false','false','true','false')  
------------------------------------------------------------------------------------
declare @DIM_UsersNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_UsersNewId,0,'','DIM_Users','DIM_Users','Dimension','false','[Name]','false')  
--Fields --
declare @DIM_UsersId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_UsersId_NumberNewId,0,'DIM_Users','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false','false','false','false','false')  
declare @DIM_UsersIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_UsersIdNewId,0,'DIM_Users','[Id]','Id','Text','true',0,15,'false','false','false','false','false','false','true','false')  
declare @DIM_UsersNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_UsersNameNewId,0,'DIM_Users','[Name]','Name','Text','true',0,60,'false','false','true','[Email]','false','false','false','false','false')  
declare @DIM_UsersLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_UsersLocalNameNewId,0,'DIM_Users','[Local Name]','Local Name','nText','false',0,100,'false','false','true','false','false','false','false','false')  
declare @DIM_UsersEmailNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersEmailNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_UsersEmailNewId,0,'DIM_Users','[Email]','Email','Text','false',0,70,'false','false','true','[Name]','false','false','false','false','false')  
declare @DIM_UsersDepartmentNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersDepartmentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_UsersDepartmentNewId,0,'DIM_Users','[Department]','Department','Text','true',0,40,'false','false','true','false','false','false','false','false')  
declare @DIM_UsersBranchNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersBranchNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_UsersBranchNewId,0,'DIM_Users','[Branch]','Branch','Text','true',0,40,'false','false','true','false','false','false','false','false')  
declare @DIM_UsersSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@DIM_UsersSourceTenantNewId,0,'DIM_Users','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false','false','false','Tenant','false','false')  
declare @DIM_UsersParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_UsersParentTenantNewId,0,'DIM_Users','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false','false','false','true','false')  
declare @DIM_UsersAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_UsersAutomaticLastUpdateDateNewId,0,'DIM_Users','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','true','false','false','false','false','false')  
declare @DIM_UsersInActiveNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersInActiveNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_UsersInActiveNewId,0,'DIM_Users','[InActive]','InActive','Boolean','false',0,0,'false','false','true','false','false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_VesselsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DefaultFilterBy,HasPivotColumn) Values(@DIM_VesselsNewId,0,'','DIM_Vessels','DIM_Vessels','Dimension','false','[English Name]','false')  
--Fields --
declare @DIM_VesselsId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_VesselsId_NumberNewId,0,'DIM_Vessels','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false','false','false','false','false')  
declare @DIM_VesselsIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_VesselsIdNewId,0,'DIM_Vessels','[Id]','Id','Text','true',0,15,'false','false','false','false','false','false','true','false')  
declare @DIM_VesselsCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_VesselsCodeNewId,0,'DIM_Vessels','[Code]','Code','Text','false',0,5,'false','false','true','[English Name],[Local Name]','false','false','false','false','false')  
declare @DIM_VesselsEnglishNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsEnglishNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_VesselsEnglishNameNewId,0,'DIM_Vessels','[English Name]','English Name','Text','true',0,40,'false','false','true','[Code]','false','true','false','false','false')  
declare @DIM_VesselsLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_VesselsLocalNameNewId,0,'DIM_Vessels','[Local Name]','Local Name','nText','false',0,40,'false','false','true','false','true','false','false','false')  
declare @DIM_VesselsNotesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsNotesNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_VesselsNotesNewId,0,'DIM_Vessels','[Notes]','Notes','Text','false',0,250,'false','false','true','false','false','false','false','false')  
declare @DIM_VesselsIMOCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsIMOCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_VesselsIMOCodeNewId,0,'DIM_Vessels','[IMO Code]','IMO Code','Text','false',0,10,'false','false','true','false','false','false','false','false')  
declare @DIM_VesselsParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_VesselsParentTenantNewId,0,'DIM_Vessels','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false','false','false','true','false')  
declare @DIM_VesselsSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@DIM_VesselsSourceTenantNewId,0,'DIM_Vessels','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false','false','false','Tenant','false','false')  
declare @DIM_VesselsAutomaticLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsAutomaticLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_VesselsAutomaticLastUpdateDateNewId,0,'DIM_Vessels','[Automatic Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false','true','false','false','false','false','false')  
declare @DIM_VesselsInActiveNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsInActiveNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@DIM_VesselsInActiveNewId,0,'DIM_Vessels','[InActive]','InActive','Boolean','false',0,0,'false','false','true','false','false','false','false','false')  
------------------------------------------------------------------------------------
declare @Fact_ChargesNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DataViewName,HasPivotColumn,PivotFieldCode,AdditionalFactCode,AdditionalFactForeignKey,RecordType,DisplayName,ObjectTableName,HasCustomFields,MaxNumberOfCustomFields) Values(@Fact_ChargesNewId,0,'<ArrayOfIndexItem xmlns:i="http://www.w3.org/2001/XMLSchema-instance"><IndexItem> <Columns>[Source Tenant],[Shipment Create Date]</Columns></IndexItem><IndexItem> <Columns>[Source Tenant],[Shipment Create Date Time]</Columns></IndexItem><IndexItem> <Columns>[Source Tenant],[Invoice Date]</Columns></IndexItem><IndexItem> <Columns>[Source Tenant],[DirectHouse]</Columns></IndexItem></ArrayOfIndexItem>','Fact_Charges','Fact_Charges','Fact','false','factCharges','true','DIM_ChargesTypes','Fact_Shipments','[Shipment Id]','Shipment','Shipment Charges','Shipment','false',0)  
--Fields --
declare @Fact_ChargesId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesId_NumberNewId,0,'Fact_Charges','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false','false','false','true','false')  
declare @Fact_ChargesShipmentIdNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesShipmentIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesShipmentIdNewId,0,'Fact_Charges','[Shipment Id]','Shipment Id','Text','true',0,15,'false','false','false','false','false','false','ShipmentPayable.ShipmentId','true','false')  
declare @Fact_ChargesSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesSourceTenantNewId,0,'Fact_Charges','[Source Tenant]','Source Tenant','Dimension','false',0,0,'DIM_Tenants','false','false','true','General','false','false','false','Tenant','false','false')  
declare @Fact_ChargesParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesParentTenantNewId,0,'Fact_Charges','[Parent Tenant]','Parent Tenant','Dimension','false',0,0,'DIM_Tenants','false','false','false','General','false','false','false','DWHSetting.ParentTenant','true','false')  
declare @Fact_ChargesDirectionNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesDirectionNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesDirectionNewId,0,'Fact_Charges','[Direction]','Direction','Dimension','true',0,40,'DIM_Directions','false','false','true','General','true','false','false','Shipment.DirectionId','false','false')  
declare @Fact_ChargesTransportModeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesTransportModeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesTransportModeNewId,0,'Fact_Charges','[Transport Mode]','Transport Mode','Dimension','true',0,13,'DIM_TransportModes','false','false','true','General','true','false','false','Shipment.TransportModeId','false','false')  
declare @Fact_ChargesDirectHouseNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesDirectHouseNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,DimensionDataViewName,RecordType) Values(@Fact_ChargesDirectHouseNewId,0,'Fact_Charges','[DirectHouse]','Direct / House','Dimension','true',0,40,'DIM_Levels','false','false','true','General','true','false','false','Shipment.ShipmentLevelName','false','false','dimDirectHouse','Shipment')  
declare @Fact_ChargesTypeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesTypeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesTypeNewId,0,'Fact_Charges','[Type]','Type','Dimension','true',0,40,'DIM_Types','false','false','true','General','true','false','false','Shipment.ShipmentTypeId','false','false')  
declare @Fact_ChargesDepartmentNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesDepartmentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesDepartmentNewId,0,'Fact_Charges','[Department]','Department','Dimension','true',0,0,'DIM_Departments','false','false','true','General','false','false','false','Shipment.DepartmentId','false','false')  
declare @Fact_ChargesBranchNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesBranchNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesBranchNewId,0,'Fact_Charges','[Branch]','Branch','Dimension','true',0,0,'DIM_Branches','false','false','true','General','false','false','false','Shipment.BranchId','false','false')  
declare @Fact_ChargesShipmentNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesShipmentNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,HelpText,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ChargesShipmentNumberNewId,0,'Fact_Charges','[Shipment Number]','Shipment Number','Text','true',0,20,'false','false','true','General','References','false','false','test help','false','Shipment.ShipmentNumber','false','false','Shipment')  
declare @Fact_ChargesHouseNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesHouseNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ChargesHouseNewId,0,'Fact_Charges','[House]','House','Text','false',0,20,'false','false','true','General','References','false','false','false','Shipment.House','false','false','Shipment')  
declare @Fact_ChargesMasterNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesMasterNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesMasterNewId,0,'Fact_Charges','[Master]','Master','Text','false',0,30,'false','false','true','General','References','false','false','false','Shipment.MasterShipmentDataId','false','false')  
declare @Fact_ChargesAgentNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesAgentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesAgentNewId,0,'Fact_Charges','[Agent]','Agent','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false','false','false','Shipment.ReleasingAgentId','false','false')  
declare @Fact_ChargesCustomerNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesCustomerNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesCustomerNewId,0,'Fact_Charges','[Customer]','Customer','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false','false','false','Shipment.CustomerId','false','false')  
declare @Fact_ChargesSalesmanNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesSalesmanNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,DimensionDataViewName) Values(@Fact_ChargesSalesmanNewId,0,'Fact_Charges','[Salesman]','Salesman','Dimension','false',0,0,'DIM_Users','false','false','true','General','Operational','false','false','false','Shipment.SalesmanUserId','false','false','dimSalesmen')  
declare @Fact_ChargesAccountManagerNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesAccountManagerNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesAccountManagerNewId,0,'Fact_Charges','[Account Manager]','Account Manager','Dimension','false',0,0,'DIM_Users','false','false','true','General','Operational','false','false','false','Shipment.AccountManagerUserId','false','false')  
declare @Fact_ChargesStatusNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesStatusNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesStatusNewId,0,'Fact_Charges','[Status]','Status','Dimension','false',0,0,'DIM_ShipmentStatuses','false','false','true','Operational','General','false','false','false','Shipment.StatusId','false','false')  
declare @Fact_ChargesMainCarriageFromPortNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesMainCarriageFromPortNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesMainCarriageFromPortNewId,0,'Fact_Charges','[MainCarriage From Port]','MainCarriage From Port','Dimension','false',0,0,'DIM_Ports','false','false','true','Operational','false','false','false','Master.MainCarriageFromPortId','false','false')  
declare @Fact_ChargesMainCarriageToPortNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesMainCarriageToPortNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesMainCarriageToPortNewId,0,'Fact_Charges','[MainCarriage To Port]','MainCarriage To Port','Dimension','false',0,0,'DIM_Ports','false','false','true','Operational','false','false','false','Master.MainCarriageToPortId','false','false')  
declare @Fact_ChargesShipmentCreateDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesShipmentCreateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesShipmentCreateDateNewId,0,'Fact_Charges','[Shipment Create Date]','Shipment Create Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Dates','Operational','false','false','false','Shipment.CreateDateTime','false','false')  
declare @Fact_ChargesAgentRef1NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesAgentRef1NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesAgentRef1NewId,0,'Fact_Charges','[Agent Ref1]','Agent Ref1','Text','false',0,50,'false','false','true','References','false','false','false','Shipment.AgentReference1','false','false')  
declare @Fact_ChargesAgentRef2NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesAgentRef2NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesAgentRef2NewId,0,'Fact_Charges','[Agent Ref2]','Agent Ref2','Text','false',0,50,'false','false','true','References','false','false','false','Shipment.AgentReference1','false','false')  
declare @Fact_ChargesShipmentCreatedByNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesShipmentCreatedByNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,DimensionDataViewName) Values(@Fact_ChargesShipmentCreatedByNewId,0,'Fact_Charges','[Shipment Created By]','Shipment Created By','Dimension','false',0,15,'DIM_Users','false','false','true','Operational','false','false','false','Shipment.CreatedByUserId','false','false','dimShipmentCreatedBy')  
declare @Fact_ChargesCustomerRef1NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesCustomerRef1NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ChargesCustomerRef1NewId,0,'Fact_Charges','[Customer Ref1]','Customer Ref1','Text','false',0,50,'false','false','true','References','false','false','false','Shipment.CustomerReference1','false','false','Shipment')  
declare @Fact_ChargesCustomerRef2NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesCustomerRef2NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ChargesCustomerRef2NewId,0,'Fact_Charges','[Customer Ref2]','Customer Ref2','Text','false',0,50,'false','false','true','References','false','false','false','Shipment.CustomerReference1','false','false','Shipment')  
declare @Fact_ChargesCarrierNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesCarrierNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesCarrierNewId,0,'Fact_Charges','[Carrier]','Carrier','Dimension','false',0,15,'DIM_Partners','false','false','true','Partners','false','false','false','Master.Transshipment1CarrierId','false','false')  
declare @Fact_ChargesFirstOperationalCloseDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesFirstOperationalCloseDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesFirstOperationalCloseDateNewId,0,'Fact_Charges','[First Operational Close Date]','First Operational Close Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Operational','Dates','false','false','false','Shipment.FirstOperationalCloseDate','false','false')  
declare @Fact_ChargesSpecialServicesNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesSpecialServicesNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,DimensionDataViewName) Values(@Fact_ChargesSpecialServicesNewId,0,'Fact_Charges','[Special Services]','Special Services','Dimension','true',0,0,'DIM_SpecialServicesTypes','false','false','true','Operational','false','false','false','Shipment.SpecialServicesTypeId','false','false','dimSpecialServices')  
declare @Fact_ChargesMasterShipmentNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesMasterShipmentNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesMasterShipmentNumberNewId,0,'Fact_Charges','[Master Shipment Number]','Master Shipment Number','Text','false',0,20,'false','false','true','Operational ','false','false','false','Shipment.MasterShipmentNumber','false','false')  
declare @Fact_ChargesChargesTypeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesChargesTypeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesChargesTypeNewId,0,'Fact_Charges','[Charges Type]','Charges Type','Dimension','true',0,0,'DIM_ChargesTypes','false','false','true','Charges','false','false','false','ShipmentPayable.ChargesTypeId','false','false')  
declare @Fact_ChargesOpenReceivablesinLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesOpenReceivablesinLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesOpenReceivablesinLocalNewId,0,'Fact_Charges','[Open Receivables in Local]','Open Receivables (Local)','Decimal','false',0,0,'false','true','SUM','true','Charges','false','false','false','Shipment.OpenReceivablesInLocalCurrency','false','true')  
declare @Fact_ChargesAccountedReceivablesinLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesAccountedReceivablesinLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesAccountedReceivablesinLocalNewId,0,'Fact_Charges','[Accounted Receivables in Local]','Accounted Receivables (Local)','Decimal','false',0,0,'false','true','SUM','true','Charges','false','false','false','Shipment.AccountedReceivablesInLocalCurrency','false','true')  
declare @Fact_ChargesOpenPayablesinLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesOpenPayablesinLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesOpenPayablesinLocalNewId,0,'Fact_Charges','[Open Payables in Local]','Open Payables (Local)','Decimal','false',0,0,'false','true','SUM','true','Charges','false','false','false','Shipment.OpenPayablesInLocalCurrency','false','true')  
declare @Fact_ChargesAccountedPayablesinLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesAccountedPayablesinLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesAccountedPayablesinLocalNewId,0,'Fact_Charges','[Accounted Payables in Local]','Accounted Payables (Local)','Decimal','false',0,0,'false','true','SUM','true','Charges','false','false','false','Shipment.AccountedPayablesInLocalCurrency','false','true')  
declare @Fact_ChargesOpenReceivablesinProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesOpenReceivablesinProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesOpenReceivablesinProfitNewId,0,'Fact_Charges','[Open Receivables in Profit]','Open Receivables (Profit)','Decimal','false',0,0,'false','true','SUM','true','Charges','false','false','false','Shipment.OpenReceivablesInProfitCurrency','false','true')  
declare @Fact_ChargesAccountedReceivablesinProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesAccountedReceivablesinProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesAccountedReceivablesinProfitNewId,0,'Fact_Charges','[Accounted Receivables in Profit]','Accounted Receivables (Profit)','Decimal','false',0,0,'false','true','SUM','true','Charges','false','false','false','Shipment.AccountedReceivablesInProfitCurrency','false','true')  
declare @Fact_ChargesOpenPayablesinProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesOpenPayablesinProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesOpenPayablesinProfitNewId,0,'Fact_Charges','[Open Payables in Profit]','Open Payables (Profit)','Decimal','false',0,0,'false','true','SUM','true','Charges','false','false','false','Shipment.OpenPayablesInProfitCurrency','false','true')  
declare @Fact_ChargesAccountedPayablesinProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesAccountedPayablesinProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesAccountedPayablesinProfitNewId,0,'Fact_Charges','[Accounted Payables in Profit]','Accounted Payables (Profit)','Decimal','false',0,0,'false','true','SUM','true','Charges','false','false','false','Shipment.AccountedPayablesInProfitCurrency','false','true')  
declare @Fact_ChargesInvoiceExchangeRateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesInvoiceExchangeRateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesInvoiceExchangeRateNewId,0,'Fact_Charges','[Invoice Exchange Rate]','Invoice Exchange Rate','Decimal','false',0,0,'false','true','SUM','true','Charges','false','false','false','APInvoice.InvoiceCurrencyExchangeRate','false','false')  
declare @Fact_ChargesIsOpenReceivableNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesIsOpenReceivableNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesIsOpenReceivableNewId,0,'Fact_Charges','[Is Open Receivable]','Open Receivable','Boolean','false',0,0,'false','false','true','Charges','false','false','false','false','false')  
declare @Fact_ChargesIsOpenPayableNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesIsOpenPayableNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesIsOpenPayableNewId,0,'Fact_Charges','[Is Open Payable]','Open Payable','Boolean','false',0,0,'false','false','true','Charges','false','false','false','false','false')  
declare @Fact_ChargesInvoiceCurrencyNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesInvoiceCurrencyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesInvoiceCurrencyNewId,0,'Fact_Charges','[Invoice Currency]','Invoice Currency','Dimension','false',0,0,'DIM_Currencies','false','false','true','Charges','false','false','false','APInvoice.InvoiceCurrencyId','false','false')  
declare @Fact_ChargesShipmentCreateDateTimeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesShipmentCreateDateTimeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesShipmentCreateDateTimeNewId,0,'Fact_Charges','[Shipment Create Date Time]','Shipment Create Date Time','DateTime','false',0,0,'false','false','true','Dates','false','false','false','Shipment.CreateDateTime','false','false')  
declare @Fact_ChargesInvoiceNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesInvoiceNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesInvoiceNumberNewId,0,'Fact_Charges','[Invoice Number]','Invoice Number','Text','false',0,20,'false','false','true','Charges','false','false','false','APInvoice.InvoiceNumber','false','false')  
declare @Fact_ChargesVATamountinInvoiceCurrencyNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesVATamountinInvoiceCurrencyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesVATamountinInvoiceCurrencyNewId,0,'Fact_Charges','[VAT amount in Invoice Currency]','VAT amount in Invoice Currency','Decimal','false',0,0,'false','false','false','General','false','false','false','APInvoice.AmountInInvoiceCurrency','false','false')  
declare @Fact_ChargesPayableIdNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesPayableIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesPayableIdNewId,0,'Fact_Charges','[Payable Id]','Payable Id','Text','false',0,15,'false','false','false','General','false','false','false','Shipment.OpenPayablesInLocalCurrency','false','false')  
declare @Fact_ChargesReceivableIdNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesReceivableIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesReceivableIdNewId,0,'Fact_Charges','[Receivable Id]','Receivable Id','Text','false',0,15,'false','false','false','General','false','false','false','Shipment.OpenReceivablesInLocalCurrency','false','false')  
declare @Fact_ChargesBillToNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesBillToNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,DimensionDataViewName) Values(@Fact_ChargesBillToNewId,0,'Fact_Charges','[Bill To]','Bill To','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false','false','false','ARInvoice.BillToId','false','false','dimBillTo')  
declare @Fact_ChargesVendorNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesVendorNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesVendorNewId,0,'Fact_Charges','[Vendor]','Vendor','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false','false','false','ShipmentPayable.VendorId','false','false')  
declare @Fact_ChargesInvoiceIdNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesInvoiceIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesInvoiceIdNewId,0,'Fact_Charges','[Invoice Id]','InvoiceId','Text','false',0,15,'false','false','false','General','false','false','false','Shipment.ARInvoices','false','false')  
declare @Fact_ChargesMainCarriageATDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesMainCarriageATDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesMainCarriageATDNewId,0,'Fact_Charges','[Main Carriage ATD]','Main Carriage ATD','DateTime','false',0,0,'false','false','true','Dates','Operational','false','false','false','Master.MainCarriageATD','false','false')  
declare @Fact_ChargesMainCarriageATANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesMainCarriageATANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesMainCarriageATANewId,0,'Fact_Charges','[Main Carriage ATA]','Main Carriage ATA','DateTime','false',0,0,'false','false','true','Dates','Operational','false','false','false','Master.MainCarriageATA','false','false')  
declare @Fact_ChargesShipmentOperationalDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesShipmentOperationalDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesShipmentOperationalDateNewId,0,'Fact_Charges','[Shipment Operational Date]','Shipment Operational Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Dates','Operational','false','false','false','Shipment.OperationalDate','false','false')  
declare @Fact_ChargesShipmentOperationallyClosedNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesShipmentOperationallyClosedNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesShipmentOperationallyClosedNewId,0,'Fact_Charges','[Shipment Operationally Closed]','Shipment Operationally Closed','Boolean','false',0,0,'false','false','true','Operational','false','false','false','Shipment.IsOperationalClosed','false','false')  
declare @Fact_ChargesShipmentAccountingClosedNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesShipmentAccountingClosedNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesShipmentAccountingClosedNewId,0,'Fact_Charges','[Shipment Accounting Closed]','Shipment Accounting Closed','Boolean','false',0,0,'false','false','true','Operational','false','false','false','Shipment.IsAccountingClosed','false','false')  
declare @Fact_ChargesOperationalCloseDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesOperationalCloseDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesOperationalCloseDateNewId,0,'Fact_Charges','[Operational Close Date]','Operational Close Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Dates','false','false','false','Shipment.OperationalCloseDate','false','false')  
declare @Fact_ChargesAccountingCloseDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesAccountingCloseDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesAccountingCloseDateNewId,0,'Fact_Charges','[Accounting Close Date]','Accounting Close Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Dates','false','false','false','Shipment.AccountingCloseDate','false','false')  
declare @Fact_ChargesProject#NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesProject#NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesProject#NewId,0,'Fact_Charges','[Project#]','Project Number','Text','false',0,100,'false','false','true','References','false','false','false','Shipment.ProjectNumber','false','false')  
declare @Fact_ChargesShipperNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesShipperNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesShipperNewId,0,'Fact_Charges','[Shipper]','Shipper','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false','false','false','Shipment.ShipperId','false','false')  
declare @Fact_ChargesConsigneeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesConsigneeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesConsigneeNewId,0,'Fact_Charges','[Consignee]','Consignee','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false','false','false','Shipment.ConsigneeId','false','false')  
declare @Fact_ChargesRoutingNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesRoutingNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesRoutingNewId,0,'Fact_Charges','[Routing]','Routing','Text','false',0,100,'false','false','true','Operational','false','false','false','Shipment.Routing','false','false')  
declare @Fact_ChargesIncotermNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesIncotermNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesIncotermNewId,0,'Fact_Charges','[Incoterm]','Incoterm','Dimension','false',0,0,'DIM_Incoterms','false','false','true','General','false','false','false','Shipment.IncotermId','false','false')  
declare @Fact_ChargesRegistryDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesRegistryDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesRegistryDateNewId,0,'Fact_Charges','[Registry Date]','Registry Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Dates','false','false','false','Shipment.RegistryDate','false','false')  
declare @Fact_ChargesInvoiceDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesInvoiceDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ChargesInvoiceDateNewId,0,'Fact_Charges','[Invoice Date]','Invoice Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Charges','false','false','false','APInvoice.InvoiceDate','true','false')  
declare @Fact_ChargesHousesOpenReceivablesInLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesHousesOpenReceivablesInLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ChargesHousesOpenReceivablesInLocalNewId,0,'Fact_Charges','[Houses Open Receivables In Local]','Open Receivables from houses only (Local)','Decimal','false',0,0,'false','false','true','Charges','false','false','false','Shipment.HousesOpenReceivablesInLocal','false','true','Master')  
declare @Fact_ChargesHousesACCTReceivablesInLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesHousesACCTReceivablesInLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ChargesHousesACCTReceivablesInLocalNewId,0,'Fact_Charges','[Houses ACCT Receivables In Local]','Accounted Receivables from houses only (Local)','Decimal','false',0,0,'false','false','true','Charges','false','false','false','Shipment.HousesACCTReceivablesInLocal','false','true','Master')  
declare @Fact_ChargesHousesOpenPayablesInLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesHousesOpenPayablesInLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ChargesHousesOpenPayablesInLocalNewId,0,'Fact_Charges','[Houses Open Payables In Local]','Open Payables from houses only (Local)','Decimal','false',0,0,'false','false','true','Charges','false','false','false','Shipment.HousesOpenPayablesInLocal','false','true','Master')  
declare @Fact_ChargesHousesACCTPayablesInLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesHousesACCTPayablesInLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ChargesHousesACCTPayablesInLocalNewId,0,'Fact_Charges','[Houses ACCT Payables In Local]','Accounted Payables from houses only (Local)','Decimal','false',0,0,'false','false','true','Charges','false','false','false','Shipment.HousesACCTPayablesInLocal','false','true','Master')  
declare @Fact_ChargesHousesOpenReceivablesInProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesHousesOpenReceivablesInProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ChargesHousesOpenReceivablesInProfitNewId,0,'Fact_Charges','[Houses Open Receivables In Profit]','Open Receivables from houses only (Profit)','Decimal','false',0,0,'false','false','true','Charges','false','false','false','Shipment.HousesOpenReceivablesInProfit','false','true','Master')  
declare @Fact_ChargesHousesACCTReceivablesInProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesHousesACCTReceivablesInProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ChargesHousesACCTReceivablesInProfitNewId,0,'Fact_Charges','[Houses ACCT Receivables In Profit]','Accounted Receivables from houses only (Profit)','Decimal','false',0,0,'false','false','true','Charges','false','false','false','Shipment.HousesACCTReceivablesInProfit','false','true','Master')  
declare @Fact_ChargesHousesOpenPayablesInProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesHousesOpenPayablesInProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ChargesHousesOpenPayablesInProfitNewId,0,'Fact_Charges','[Houses Open Payables In Profit]','Open Payables from houses only (Profit)','Decimal','false',0,0,'false','false','true','Charges','false','false','false','Shipment.HousesOpenPayablesInProfit','false','true','Master')  
declare @Fact_ChargesHousesACCTPayablesInProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ChargesHousesACCTPayablesInProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ChargesHousesACCTPayablesInProfitNewId,0,'Fact_Charges','[Houses ACCT Payables In Profit]','Accounted Payables from houses only (Profit)','Decimal','false',0,0,'false','false','true','Charges','false','false','false','Shipment.HousesACCTPayablesInProfit','false','true','Master')  
------------------------------------------------------------------------------------
declare @Fact_MasterChargesNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_MasterChargesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DataViewName,HasPivotColumn,PivotFieldCode,AdditionalFactCode,AdditionalFactForeignKey,ParentFactCode,RecordType,DisplayName,ObjectTableName,HasCustomFields,MaxNumberOfCustomFields) Values(@Fact_MasterChargesNewId,0,'','Fact_MasterCharges','Fact_MasterCharges','Fact','false','factMasterCharges','true','DIM_ChargesTypes','Fact_Shipments','[Shipment Id]','Fact_Charges','Master','Master Charges','Shipment','false',0)  
--Fields --
------------------------------------------------------------------------------------
declare @Fact_MastersNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_MastersNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DataViewName,HasPivotColumn,ParentFactCode,RecordType,DisplayName,ObjectTableName,HasCustomFields,MaxNumberOfCustomFields) Values(@Fact_MastersNewId,0,'','Fact_Masters','Fact_Masters','Fact','false','factMasters','false','Fact_Shipments','Master','Masters','Shipment','false',0)  
--Fields --
------------------------------------------------------------------------------------
declare @Fact_QuotesNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DataViewName,HasPivotColumn,RecordType,DisplayName,ObjectTableName,HasCustomFields,MaxNumberOfCustomFields) Values(@Fact_QuotesNewId,0,'<ArrayOfIndexItem xmlns:i="http://www.w3.org/2001/XMLSchema-instance"><IndexItem> <Columns>[Source Tenant]</Columns></IndexItem><IndexItem> <Columns>[Parent Tenant]</Columns></IndexItem><IndexItem> <Columns>[Id]</Columns></IndexItem><IndexItem> <Columns>[Source Tenant],[Open Date]</Columns></IndexItem></ArrayOfIndexItem>','Fact_Quotes','Fact_Quotes','Fact','false','factQuotes','false','Quotes','Quotes','Quote','true',20)  
--Fields --
declare @Fact_QuotesId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesId_NumberNewId,0,'Fact_Quotes','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false','false','false','true','false')  
declare @Fact_QuotesIdNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesIdNewId,0,'Fact_Quotes','[Id]','Id','Text','true',0,15,'false','false','false','false','false','false','true','false')  
declare @Fact_QuotesSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesSourceTenantNewId,0,'Fact_Quotes','[Source Tenant]','Source Tenant','Dimension','false',0,0,'DIM_Tenants','false','false','true','General','false','false','false','Tenant','false','false')  
declare @Fact_QuotesParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesParentTenantNewId,0,'Fact_Quotes','[Parent Tenant]','Parent Tenant','Dimension','false',0,0,'DIM_Tenants','false','false','true','General','false','false','false','DWHSetting.ParentTenant','true','false')  
declare @Fact_QuotesDirectionNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesDirectionNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesDirectionNewId,0,'Fact_Quotes','[Direction]','Direction','Dimension','true',0,1,'DIM_Directions','false','false','true','General','false','false','false','Quote.DirectionId','false','false')  
declare @Fact_QuotesTransportModeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesTransportModeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesTransportModeNewId,0,'Fact_Quotes','[Transport Mode]','Transport Mode','Dimension','true',0,1,'DIM_TransportModes','false','false','true','General','false','false','false','Quote.TransportModeId','false','false')  
declare @Fact_QuotesTypeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesTypeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesTypeNewId,0,'Fact_Quotes','[Type]','Type','Dimension','false',0,4,'DIM_Types','false','false','true','General','false','false','false','Quote.ShipmentTypeId','false','false')  
declare @Fact_QuotesDepartmentNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesDepartmentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesDepartmentNewId,0,'Fact_Quotes','[Department]','Department ','Dimension','false',0,15,'DIM_Departments','false','false','true','General','false','false','false','Quote.DepartmentId','false','false')  
declare @Fact_QuotesBranchNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesBranchNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesBranchNewId,0,'Fact_Quotes','[Branch]','Branch','Dimension','false',0,15,'DIM_Branches','false','false','true','General','false','false','false','Quote.BranchId','false','false')  
declare @Fact_QuotesQuoteNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesQuoteNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesQuoteNumberNewId,0,'Fact_Quotes','[Quote Number]','Quote Number','nText','false',0,20,'false','false','true','General','false','false','false','Quote.QuoteNumber','false','false')  
declare @Fact_QuotesShipperNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesShipperNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesShipperNewId,0,'Fact_Quotes','[Shipper]','Shipper','Dimension','false',0,15,'DIM_Partners','false','false','true','Partners','false','false','false','Quote.ShipperId','false','false')  
declare @Fact_QuotesConsigneeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesConsigneeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesConsigneeNewId,0,'Fact_Quotes','[Consignee ]','Consignee ','Dimension','false',0,15,'DIM_Partners','false','false','true','Partners','false','false','false','Quote.ConsigneeId','false','false')  
declare @Fact_QuotesCustomerNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesCustomerNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesCustomerNewId,0,'Fact_Quotes','[Customer]','Customer','Dimension','false',0,15,'DIM_Partners','false','false','true','Partners','false','false','false','Quote.CustomerId','false','false')  
declare @Fact_QuotesAgentNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesAgentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesAgentNewId,0,'Fact_Quotes','[Agent]','Agent','Dimension','false',0,15,'DIM_Partners','false','false','true','Partners','false','false','false','Quote.AgentId','false','false')  
declare @Fact_QuotesIncotermsNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesIncotermsNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesIncotermsNewId,0,'Fact_Quotes','[Incoterms]','Incoterms','Dimension','false',0,15,'DIM_Incoterms','false','false','true','General','false','false','false','Quote.IncotermId','false','false')  
declare @Fact_QuotesOpenedbyNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesOpenedbyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesOpenedbyNewId,0,'Fact_Quotes','[Opened by]','Opened by','Dimension','true',0,15,'DIM_Users','false','false','true','Partners','false','false','false','Quote.CreatedByUserId','false','false')  
declare @Fact_QuotesOpenDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesOpenDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesOpenDateNewId,0,'Fact_Quotes','[Open Date]','Open Date','Dimension','true',0,0,'DIM_Dates','false','false','true','Dates','false','false','false','Quote.OpenDate','false','false')  
declare @Fact_QuotesSentDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesSentDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesSentDateNewId,0,'Fact_Quotes','[Sent Date]','Sent Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Dates','false','false','false','Quote.SentDate','false','false')  
declare @Fact_QuotesAcceptedDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesAcceptedDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesAcceptedDateNewId,0,'Fact_Quotes','[Accepted Date]','Accepted Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Dates','false','false','false','Quote.AcceptedDate','false','false')  
declare @Fact_QuotesDeclinedDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesDeclinedDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesDeclinedDateNewId,0,'Fact_Quotes','[Declined Date]','Declined Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Dates','false','false','false','Quote.DeclinedDate','false','false')  
declare @Fact_QuotesStartDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesStartDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesStartDateNewId,0,'Fact_Quotes','[Start Date]','Start Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Dates','false','false','false','Quote.StartDate','false','false')  
declare @Fact_QuotesLastActivityDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesLastActivityDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesLastActivityDateNewId,0,'Fact_Quotes','[Last Activity Date]','Last Activity Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Dates','false','false','false','Quote.LastActivityDate','false','false')  
declare @Fact_QuotesSalesmanNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesSalesmanNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesSalesmanNewId,0,'Fact_Quotes','[Salesman]','Salesman','Dimension','false',0,15,'DIM_Users','false','false','true','General','false','false','false','Quote.SalesmanUserId','false','false')  
declare @Fact_QuotesStageNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesStageNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesStageNewId,0,'Fact_Quotes','[Stage]','Stage','Dimension','false',0,15,'DIM_QuoteStages','false','false','true','General','false','false','false','Quote.StageId','false','false')  
declare @Fact_QuotesNotesNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesNotesNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesNotesNewId,0,'Fact_Quotes','[Notes]','Notes','nText','false',0,500,'false','false','true','General','false','false','false','Quote.Notes','false','false')  
declare @Fact_QuotesClosingReasonNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesClosingReasonNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesClosingReasonNewId,0,'Fact_Quotes','[Closing Reason]','Closing Reason','Dimension','false',0,15,'DIM_QuoteClosingReasons','false','false','true','General','false','false','false','Quote.QuoteClosingReasonId','false','false')  
declare @Fact_QuotesEstimatedProfitinLocalCurrencyNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesEstimatedProfitinLocalCurrencyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesEstimatedProfitinLocalCurrencyNewId,0,'Fact_Quotes','[Estimated Profit in Local Currency]','Estimated Profit in Local Currency','Decimal','false',0,0,'false','false','true','Money','false','false','false','Quote.EstimatedProfitInLocal','false','false')  
declare @Fact_QuotesSalesCurrencyNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesSalesCurrencyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesSalesCurrencyNewId,0,'Fact_Quotes','[Sales Currency]','Sales Currency','Dimension','true',0,15,'DIM_Currencies','false','false','true','Money','false','false','false','Quote.SaleCurrencyId','false','false')  
declare @Fact_QuotesSubjectNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesSubjectNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesSubjectNewId,0,'Fact_Quotes','[Subject]','Subject','nText','false',0,200,'false','false','true','Routings','false','false','false','Quote.Subject','false','false')  
declare @Fact_QuotesGrossWeightInKgNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesGrossWeightInKgNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesGrossWeightInKgNewId,0,'Fact_Quotes','[Gross Weight In Kg]','Gross Weight In Kg','Decimal','false',0,0,'false','false','true','Packages','false','false','false','Quote.GrossWeightInKG','false','false')  
declare @Fact_QuotesChargeableWeightinKgNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesChargeableWeightinKgNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesChargeableWeightinKgNewId,0,'Fact_Quotes','[Chargeable Weight in Kg]','Chargeable Weight in Kg','Decimal','false',0,0,'false','false','true','Packages','false','false','false','Quote.ChargeableWeightInKG','false','false')  
declare @Fact_QuotesVolumeinCBMNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesVolumeinCBMNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesVolumeinCBMNewId,0,'Fact_Quotes','[Volume in CBM]','Volume in CBM','Decimal','false',0,0,'false','false','true','Packages','false','false','false','Quote.VolumeInCBM','false','false')  
declare @Fact_QuotesNumberofPackagesNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesNumberofPackagesNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesNumberofPackagesNewId,0,'Fact_Quotes','[Number of Packages]','Number of Packages','Integer','false',0,0,'false','false','true','Packages','false','false','false','Quote.NumberOfPackages','false','false')  
declare @Fact_QuotesNumberofContainersNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesNumberofContainersNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesNumberofContainersNewId,0,'Fact_Quotes','[Number of Containers]','Number of Containers','Integer','false',0,0,'false','false','true','Packages','false','false','false','Quote.NumberOfContainers','false','false')  
declare @Fact_QuotesExpirationDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesExpirationDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesExpirationDateNewId,0,'Fact_Quotes','[Expiration Date]','Expiration Date ','DateTime','false',0,0,'false','false','true','Dates','false','false','false','Quote.ExpirationDate','false','false')  
declare @Fact_QuotesCloseAutobySystemNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesCloseAutobySystemNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesCloseAutobySystemNewId,0,'Fact_Quotes','[Close Auto by System]','Close Auto by System','Boolean','false',0,0,'false','false','true','General','false','false','false','Quote.IsAutomaticallyClosed','false','false')  
declare @Fact_QuotesIncludePickupNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesIncludePickupNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesIncludePickupNewId,0,'Fact_Quotes','[Include Pickup]','Include Pickup','Boolean','false',0,0,'false','false','true','Routings','false','false','false','Quote.IncludePickUp','false','false')  
declare @Fact_QuotesIncludeDeliveryNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesIncludeDeliveryNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesIncludeDeliveryNewId,0,'Fact_Quotes','[Include Delivery]','Include Delivery','Boolean','false',0,0,'false','false','true','Routings','false','false','false','Quote.IncludeDelivery','false','false')  
declare @Fact_QuotesStageDueDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesStageDueDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesStageDueDateNewId,0,'Fact_Quotes','[Stage Due Date]','Stage Due Date','DateTime','false',0,0,'false','false','true','Dates','false','false','false','Quote.StageDueDate','false','false')  
declare @Fact_QuotesIsPotentialShipperNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesIsPotentialShipperNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesIsPotentialShipperNewId,0,'Fact_Quotes','[Is Potential Shipper]','Is Potential Shipper','Boolean','false',0,0,'false','false','true','General','false','false','false','Quote.IsPotentialShipper','false','false')  
declare @Fact_QuotesIsQuoteDataExternalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesIsQuoteDataExternalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesIsQuoteDataExternalNewId,0,'Fact_Quotes','[Is Quote Data External]','Is Quote Data External','Boolean','false',0,0,'false','false','true','General','false','false','false','Quote.IsQuoteDataExternal','false','false')  
declare @Fact_QuotesIsQuoteDocumentExternalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesIsQuoteDocumentExternalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesIsQuoteDocumentExternalNewId,0,'Fact_Quotes','[Is Quote Document External]','Is Quote Document External','Boolean','false',0,0,'false','false','true','General','false','false','false','Quote.IsQuoteDocumentExternal','false','false')  
declare @Fact_QuotesLocalCurrencyNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesLocalCurrencyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesLocalCurrencyNewId,0,'Fact_Quotes','[Local Currency]','Local Currency','Dimension','false',0,0,'DIM_Currencies','false','false','true','Money','false','false','false','false','false')  
declare @Fact_QuotesPickupFromNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesPickupFromNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesPickupFromNewId,0,'Fact_Quotes','[Pickup From]','Pickup From','Text','false',0,500,'false','false','true','Routings','false','false','false','QuoteComputedField.PickupFrom','false','false')  
declare @Fact_QuotesConnectedtoTicketNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesConnectedtoTicketNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesConnectedtoTicketNewId,0,'Fact_Quotes','[Connected to Ticket]','Connected to Ticket','Boolean','false',0,0,'false','false','true','General','false','false','false','QuoteComputedField.ConnectedToTicket','false','false')  
declare @Fact_QuotesConnectedtoShipmentNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesConnectedtoShipmentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesConnectedtoShipmentNewId,0,'Fact_Quotes','[Connected to Shipment]','Connected to Shipment ','Boolean','false',0,0,'false','false','true','General','false','false','false','QuoteComputedField.ConnectedToShipment','false','false')  
declare @Fact_QuotesFromNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesFromNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesFromNewId,0,'Fact_Quotes','[From]','From','Text','false',0,100,'false','false','false','Routings','false','false','false','Quote.FromLocation','[From]','true','false')  
declare @Fact_QuotesToNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesToNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesToNewId,0,'Fact_Quotes','[To]','To','Text','false',0,100,'false','false','false','Routings','false','false','false','Quote.ToLocation','[To]','true','false')  
declare @Fact_QuotesFromPortCountryNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesFromPortCountryNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesFromPortCountryNewId,0,'Fact_Quotes','[From Port Country]','From Port Country','Dimension','false',0,120,'DIM_Countries','true','false','true','Routings','false','false','false','Quote.FromPortCountry','false','false')  
declare @Fact_QuotesToPortCountryNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesToPortCountryNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesToPortCountryNewId,0,'Fact_Quotes','[To Port Country]','To Port Country','Dimension','false',0,120,'DIM_Countries','false','false','true','Routings','false','false','false','Quote.ToPortCountry','false','false')  
declare @Fact_QuotesEstimatedPayablesinSalesCurrencyNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesEstimatedPayablesinSalesCurrencyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesEstimatedPayablesinSalesCurrencyNewId,0,'Fact_Quotes','[Estimated Payables in Sales Currency]','Estimated Payables in Sales Currency','Decimal','false',0,0,'false','false','true','Money','false','false','false','QuoteComputedField.EstimatedPayablesInSales','false','false')  
declare @Fact_QuotesEstimatedReceivablesinLocalCurrencyNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesEstimatedReceivablesinLocalCurrencyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesEstimatedReceivablesinLocalCurrencyNewId,0,'Fact_Quotes','[Estimated Receivables in Local Currency]','Estimated Receivables in Local Currency','Decimal','false',0,0,'false','false','true','Money','false','false','false','QuoteComputedField.EstimatedReceivablesInLocal','false','false')  
declare @Fact_QuotesEstimatedReceivablesinSalesCurrencyNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesEstimatedReceivablesinSalesCurrencyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesEstimatedReceivablesinSalesCurrencyNewId,0,'Fact_Quotes','[Estimated Receivables in Sales Currency]','Estimated Receivables in Sales Currency','Decimal','false',0,0,'false','false','true','Money','false','false','false','QuoteComputedField.EstimatedReceivablesInSales','false','false')  
declare @Fact_QuotesEstimatedProfitinSalesCurrencyNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesEstimatedProfitinSalesCurrencyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesEstimatedProfitinSalesCurrencyNewId,0,'Fact_Quotes','[Estimated Profit in Sales Currency]','Estimated Profit in Sales Currency','Decimal','false',0,0,'false','false','true','Money','false','false','false','false','false')  
declare @Fact_QuotesEstimatedPayablesinLocalCurrencyNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesEstimatedPayablesinLocalCurrencyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesEstimatedPayablesinLocalCurrencyNewId,0,'Fact_Quotes','[Estimated Payables in Local Currency]','Estimated Payables in Local Currency','Decimal','false',0,0,'false','false','true','Money','false','false','false','QuoteComputedField.EstimatedPayablesInLocal','false','false')  
declare @Fact_QuotesCloseDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesCloseDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesCloseDateNewId,0,'Fact_Quotes','[Close Date]','Close Date','DateTime','false',0,0,'false','false','true','Dates','false','false','false','Quote.AutomaticallyCloseDate','false','false')  
declare @Fact_QuotesIsPotentialConsigneeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesIsPotentialConsigneeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesIsPotentialConsigneeNewId,0,'Fact_Quotes','[Is Potential Consignee]','Is Potential Consignee','Boolean','false',0,0,'false','false','true','General','false','false','false','false','false')  
declare @Fact_QuotesIsPotentialCustomerNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesIsPotentialCustomerNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesIsPotentialCustomerNewId,0,'Fact_Quotes','[Is Potential Customer]','Is Potential Customer','Boolean','false',0,0,'false','false','true','General','false','false','false','false','false')  
declare @Fact_QuotesConnectedtoOpportunityNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesConnectedtoOpportunityNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesConnectedtoOpportunityNewId,0,'Fact_Quotes','[Connected to Opportunity]','Connected to Opportunity','Boolean','false',0,0,'false','false','true','General','false','false','false','false','false')  
declare @Fact_QuotesField1NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesField1NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesField1NewId,0,'Fact_Quotes','[Field1]','Field1','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_QuotesField2NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesField2NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesField2NewId,0,'Fact_Quotes','[Field2]','Field2','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_QuotesField3NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesField3NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesField3NewId,0,'Fact_Quotes','[Field3]','Field3','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_QuotesField4NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesField4NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesField4NewId,0,'Fact_Quotes','[Field4]','Field4','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_QuotesField5NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesField5NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesField5NewId,0,'Fact_Quotes','[Field5]','Field5','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_QuotesField6NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesField6NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesField6NewId,0,'Fact_Quotes','[Field6]','Field6','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_QuotesField7NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesField7NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesField7NewId,0,'Fact_Quotes','[Field7]','Field7','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_QuotesField8NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesField8NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesField8NewId,0,'Fact_Quotes','[Field8]','Field8','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_QuotesField9NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesField9NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesField9NewId,0,'Fact_Quotes','[Field9]','Field9','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_QuotesField10NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesField10NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesField10NewId,0,'Fact_Quotes','[Field10]','Field10','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_QuotesField11NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesField11NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesField11NewId,0,'Fact_Quotes','[Field11]','Field11','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_QuotesField12NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesField12NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesField12NewId,0,'Fact_Quotes','[Field12]','Field12','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_QuotesField13NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesField13NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesField13NewId,0,'Fact_Quotes','[Field13]','Field13','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_QuotesField14NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesField14NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesField14NewId,0,'Fact_Quotes','[Field14]','Field14','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_QuotesField15NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesField15NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesField15NewId,0,'Fact_Quotes','[Field15]','Field15','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_QuotesField16NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesField16NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesField16NewId,0,'Fact_Quotes','[Field16]','Field16','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_QuotesField17NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesField17NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesField17NewId,0,'Fact_Quotes','[Field17]','Field17','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_QuotesField18NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesField18NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesField18NewId,0,'Fact_Quotes','[Field18]','Field18','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_QuotesField19NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesField19NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesField19NewId,0,'Fact_Quotes','[Field19]','Field19','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_QuotesField20NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesField20NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesField20NewId,0,'Fact_Quotes','[Field20]','Field20','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_QuotesToLocationNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesToLocationNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesToLocationNewId,0,'Fact_Quotes','[To Location]','To Location','Text','false',0,500,'false','false','true','Routings','false','false','false','QuoteComputedField.ToLocation','false','false')  
declare @Fact_QuotesFromLocationNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesFromLocationNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesFromLocationNewId,0,'Fact_Quotes','[From Location]','From Location','Text','false',0,500,'false','false','true','Routings','false','false','false','QuoteComputedField.FromLocation','false','false')  
declare @Fact_QuotesDeliveryToNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_QuotesDeliveryToNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_QuotesDeliveryToNewId,0,'Fact_Quotes','[Delivery To]','Delivery To','Text','false',0,500,'false','false','true','Routings','false','false','false','QuoteComputedField.DeliveryTo','false','false')  
------------------------------------------------------------------------------------
declare @Fact_ShipmentsNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,IndexesXml,Code,Name,TypeCode,IsClosed,DataViewName,HasPivotColumn,RecordType,DisplayName,ObjectTableName,HasCustomFields,MaxNumberOfCustomFields) Values(@Fact_ShipmentsNewId,0,'<ArrayOfIndexItem xmlns:i="http://www.w3.org/2001/XMLSchema-instance"><IndexItem> <Columns>[Source Tenant],[Create Date]</Columns></IndexItem><IndexItem> <Columns>[Source Tenant],[Create Date Time]</Columns></IndexItem><IndexItem> <Columns>[Source Tenant],[DirectHouse]</Columns></IndexItem></ArrayOfIndexItem>','Fact_Shipments','Fact_Shipments','Fact','false','factShipments','false','Shipment','Shipments','Shipment','true',40)  
--Fields --
declare @Fact_ShipmentsId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsId_NumberNewId,0,'Fact_Shipments','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false','false','false','true','false')  
declare @Fact_ShipmentsIdNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsIdNewId,0,'Fact_Shipments','[Id]','Id','Text','true',0,15,'false','false','false','false','false','false','true','false')  
declare @Fact_ShipmentsSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsSourceTenantNewId,0,'Fact_Shipments','[Source Tenant]','Source Tenant','Dimension','false',0,0,'DIM_Tenants','false','false','true','General','false','false','false','Tenant','false','false')  
declare @Fact_ShipmentsParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsParentTenantNewId,0,'Fact_Shipments','[Parent Tenant]','Parent Tenant','Dimension','false',0,0,'DIM_Tenants','false','false','false','General','false','false','false','DWHSetting.ParentTenant','true','false')  
declare @Fact_ShipmentsDirectionNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsDirectionNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsDirectionNewId,0,'Fact_Shipments','[Direction]','Direction','Dimension','true',0,40,'DIM_Directions','false','false','true','General','true','false','false','Shipment.DirectionId','false','false')  
declare @Fact_ShipmentsTransportModeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransportModeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransportModeNewId,0,'Fact_Shipments','[Transport Mode]','Transport Mode','Dimension','true',0,13,'DIM_TransportModes','false','false','true','General','true','false','false','Shipment.TransportModeId','false','false')  
declare @Fact_ShipmentsDirectHouseNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsDirectHouseNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,DimensionDataViewName,RecordType) Values(@Fact_ShipmentsDirectHouseNewId,0,'Fact_Shipments','[DirectHouse]','Direct / House','Dimension','true',0,40,'DIM_Levels','false','false','true','General','true','false','false','Shipment.ShipmentLevelName','false','false','dimDirectHouse','Shipment')  
declare @Fact_ShipmentsTypeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTypeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTypeNewId,0,'Fact_Shipments','[Type]','Type','Dimension','true',0,40,'DIM_Types','false','false','true','General','true','false','false','Shipment.ShipmentTypeId','false','false')  
declare @Fact_ShipmentsDepartmentNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsDepartmentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsDepartmentNewId,0,'Fact_Shipments','[Department]','Department','Dimension','true',0,0,'DIM_Departments','false','false','true','General','false','false','false','Shipment.DepartmentId','false','false')  
declare @Fact_ShipmentsBranchNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsBranchNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsBranchNewId,0,'Fact_Shipments','[Branch]','Branch','Dimension','true',0,0,'DIM_Branches','false','false','true','General','false','false','false','Shipment.BranchId','false','false')  
declare @Fact_ShipmentsShipmentNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsShipmentNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,HelpText,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsShipmentNumberNewId,0,'Fact_Shipments','[Shipment Number]','Shipment Number','Text','true',0,20,'false','false','true','General','References','false','false','test help','false','Shipment.ShipmentNumber','false','false','Shipment')  
declare @Fact_ShipmentsHouseNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsHouseNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsHouseNewId,0,'Fact_Shipments','[House]','House','Text','false',0,20,'false','false','true','General','References','false','false','false','Shipment.House','false','false','Shipment')  
declare @Fact_ShipmentsMasterNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsMasterNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsMasterNewId,0,'Fact_Shipments','[Master]','Master','Text','false',0,30,'false','false','true','References','References','false','false','false','Shipment.MasterShipmentDataId','false','false')  
declare @Fact_ShipmentsShipperNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsShipperNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsShipperNewId,0,'Fact_Shipments','[Shipper]','Shipper','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false','false','false','Shipment.ShipperId','false','false')  
declare @Fact_ShipmentsConsigneeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsConsigneeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsConsigneeNewId,0,'Fact_Shipments','[Consignee]','Consignee','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false','false','false','Shipment.ConsigneeId','false','false')  
declare @Fact_ShipmentsAgentNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAgentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsAgentNewId,0,'Fact_Shipments','[Agent]','Agent','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false','false','false','Shipment.ReleasingAgentId','false','false')  
declare @Fact_ShipmentsCustomerNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCustomerNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsCustomerNewId,0,'Fact_Shipments','[Customer]','Customer','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false','false','false','Shipment.CustomerId','false','false')  
declare @Fact_ShipmentsIncotermNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsIncotermNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsIncotermNewId,0,'Fact_Shipments','[Incoterm]','Incoterm','Dimension','false',0,0,'DIM_Incoterms','false','false','true','Operational','General','false','false','false','Shipment.IncotermId','false','false')  
declare @Fact_ShipmentsGrossWeightKGNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsGrossWeightKGNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsGrossWeightKGNewId,0,'Fact_Shipments','[Gross Weight (KG)]','Gross Weight (KG)','Decimal','false',0,15,'false','true','SUM','true','Packages','false','false','false','Shipment.GrossWeightInKG','false','false')  
declare @Fact_ShipmentsChargeableWeightKGNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsChargeableWeightKGNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsChargeableWeightKGNewId,0,'Fact_Shipments','[Chargeable Weight (KG)]','Chargeable Weight (KG)','Decimal','false',0,40,'false','true','SUM','true','Packages','false','false','false','Shipment.ChargeableWeightUnitCode','false','false')  
declare @Fact_ShipmentsTotalVolumeCBMNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTotalVolumeCBMNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTotalVolumeCBMNewId,0,'Fact_Shipments','[Total Volume (CBM)]','Total Volume (CBM)','Decimal','false',0,0,'false','true','SUM','true','Packages','false','false','false','Shipment.VolumeInCBM','false','false')  
declare @Fact_ShipmentsNumberofPackagesNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsNumberofPackagesNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsNumberofPackagesNewId,0,'Fact_Shipments','[Number of Packages]','Number of Packages','Integer','false',0,0,'false','true','SUM','true','Packages','false','false','false','Shipment.NumberOfPackages','false','false')  
declare @Fact_ShipmentsNumberofContainersNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsNumberofContainersNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsNumberofContainersNewId,0,'Fact_Shipments','[Number of Containers]','Number of Containers','Integer','false',0,0,'false','true','SUM','true','Packages','false','false','false','Shipment.NumberOfContainers','false','false')  
declare @Fact_ShipmentsSalesmanNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsSalesmanNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,DimensionDataViewName) Values(@Fact_ShipmentsSalesmanNewId,0,'Fact_Shipments','[Salesman]','Salesman','Dimension','false',0,0,'DIM_Users','false','false','true','Operational','Operational','false','false','false','Shipment.SalesmanUserId','false','false','dimSalesmen')  
declare @Fact_ShipmentsAccountManagerNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAccountManagerNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsAccountManagerNewId,0,'Fact_Shipments','[Account Manager]','Account Manager','Dimension','false',0,0,'DIM_Users','false','false','true','Operational','false','false','false','Shipment.AccountManagerUserId','false','false')  
declare @Fact_ShipmentsProfitLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsProfitLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsProfitLocalNewId,0,'Fact_Shipments','[Profit ( Local )]','Profit ( Local )','Decimal','false',0,0,'false','true','SUM','true','Money','false','false','false','Shipment.ProfitCurrencyId','false','false')  
declare @Fact_ShipmentsProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsProfitNewId,0,'Fact_Shipments','[Profit]','Profit','Decimal','false',0,0,'false','true','SUM','true','Money','false','false','false','Shipment.ProfitCurrencyId','false','false')  
declare @Fact_ShipmentsLocalCurrencyNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsLocalCurrencyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsLocalCurrencyNewId,0,'Fact_Shipments','[Local Currency ]','Local Currency ','Dimension','false',0,0,'DIM_Currencies','false','false','true','Money','false','false','false','Shipment.EstimateProfitInLocalCurrency','false','false')  
declare @Fact_ShipmentsProfitCurrencyNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsProfitCurrencyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsProfitCurrencyNewId,0,'Fact_Shipments','[Profit Currency]','Profit Currency','Dimension','false',0,0,'DIM_Currencies','false','false','true','Money','false','false','false','Shipment.ProfitCurrencyId','false','false')  
declare @Fact_ShipmentsOperationallyClosedNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOperationallyClosedNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsOperationallyClosedNewId,0,'Fact_Shipments','[Operationally Closed]','Operationally Closed','Boolean','false',0,0,'false','false','true','Operational','false','false','false','Shipment.IsOperationalClosed','false','false')  
declare @Fact_ShipmentsAccountingClosedNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAccountingClosedNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsAccountingClosedNewId,0,'Fact_Shipments','[Accounting Closed]','Accounting Closed','Boolean','false',0,0,'false','false','true','Operational','false','false','false','Shipment.IsAccountingClosed','false','false')  
declare @Fact_ShipmentsStatusNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsStatusNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsStatusNewId,0,'Fact_Shipments','[Status]','Status','Dimension','false',0,0,'DIM_ShipmentStatuses','false','false','true','Operational','Operational','false','false','false','Shipment.StatusId','false','false')  
declare @Fact_ShipmentsLocationNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsLocationNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsLocationNewId,0,'Fact_Shipments','[Location]','Location','nText','false',0,40,'false','false','true','Routings','false','false','false','Shipment.StatusLocation','false','false')  
declare @Fact_ShipmentsMainCarriageFromPortNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsMainCarriageFromPortNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsMainCarriageFromPortNewId,0,'Fact_Shipments','[MainCarriage From Port]','MainCarriage From Port','Dimension','false',0,0,'DIM_Ports','false','false','true','Routings','false','false','false','Master.MainCarriageFromPortId','false','false')  
declare @Fact_ShipmentsMainCarriageToPortNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsMainCarriageToPortNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsMainCarriageToPortNewId,0,'Fact_Shipments','[MainCarriage To Port]','MainCarriage To Port','Dimension','false',0,0,'DIM_Ports','false','false','true','Routings','false','false','false','Master.MainCarriageToPortId','false','false')  
declare @Fact_ShipmentsIsDepartedNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsIsDepartedNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsIsDepartedNewId,0,'Fact_Shipments','[Is Departed]','Departed','Boolean','false',0,0,'false','false','true','Operational','false','false','false','false','false')  
declare @Fact_ShipmentsMainCarriageATDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsMainCarriageATDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsMainCarriageATDNewId,0,'Fact_Shipments','[MainCarriage ATD]','Main Carriage ATD','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Master.MainCarriageATD','false','false')  
declare @Fact_ShipmentsIsArrivedNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsIsArrivedNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsIsArrivedNewId,0,'Fact_Shipments','[Is Arrived]','Arrived','Boolean','false',0,0,'false','false','true','Operational','false','false','false','false','false')  
declare @Fact_ShipmentsArrivedDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsArrivedDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsArrivedDateNewId,0,'Fact_Shipments','[Arrived Date]','Arrived Date','DateTime','false',0,0,'false','false','true','Dates','Routings','false','false','false','false','false')  
declare @Fact_ShipmentsIsCustomsClearedNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsIsCustomsClearedNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsIsCustomsClearedNewId,0,'Fact_Shipments','[Is Customs Cleared]','Customs Cleared','Boolean','false',0,0,'false','false','true','Operational','false','false','false','false','false','Shipment')  
declare @Fact_ShipmentsTotalShipmentsNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTotalShipmentsNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTotalShipmentsNewId,0,'Fact_Shipments','[Total Shipments]','Total Shipments','Integer','false',0,0,'false','true','COUNT','true','General','false','false','false','Shipment.ReleasingAgentReference1','false','false')  
declare @Fact_ShipmentsCreateDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCreateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsCreateDateNewId,0,'Fact_Shipments','[Create Date]','Create Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Dates','false','false','false','Shipment.CreateDateTime','false','false')  
declare @Fact_ShipmentsLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsLastUpdateDateNewId,0,'Fact_Shipments','[Last Update Date]','Last Update Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Operational','Dates','false','false','false','Shipment.LastUpdateDate','false','false')  
declare @Fact_ShipmentsOperationalDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOperationalDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsOperationalDateNewId,0,'Fact_Shipments','[Operational Date]','Operational Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Operational','Dates','false','false','false','Shipment.OperationalDate','false','false')  
declare @Fact_ShipmentsOperationalCloseDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOperationalCloseDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsOperationalCloseDateNewId,0,'Fact_Shipments','[Operational Close Date]','Operational Close Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Operational','Dates','false','false','false','Shipment.OperationalCloseDate','false','false')  
declare @Fact_ShipmentsAccountingCloseDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAccountingCloseDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsAccountingCloseDateNewId,0,'Fact_Shipments','[Accounting Close Date]','Accounting Close Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Dates','Operational','false','false','false','Shipment.AccountingCloseDate','false','false')  
declare @Fact_ShipmentsOpenReceivablesLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOpenReceivablesLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsOpenReceivablesLocalNewId,0,'Fact_Shipments','[Open Receivables ( Local )]','Open Receivables ( Local )','Decimal','false',0,0,'false','true','SUM','true','Money','false','false','false','Shipment.OpenReceivablesInLocalCurrency','false','false')  
declare @Fact_ShipmentsOpenReceivablesProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOpenReceivablesProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsOpenReceivablesProfitNewId,0,'Fact_Shipments','[Open Receivables ( Profit )]','Open Receivables ( Profit )','Decimal','false',0,0,'false','true','SUM','true','Money','false','false','false','Shipment.OpenReceivablesInLocalCurrency','false','false')  
declare @Fact_ShipmentsAccountedReceivablesLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAccountedReceivablesLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsAccountedReceivablesLocalNewId,0,'Fact_Shipments','[Accounted Receivables ( Local )]','Accounted Receivables ( Local )','Decimal','false',0,0,'false','true','SUM','true','Money','false','false','false','Shipment.AccountedReceivablesInLocalCurrency','false','false')  
declare @Fact_ShipmentsAccountedReceivablesProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAccountedReceivablesProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsAccountedReceivablesProfitNewId,0,'Fact_Shipments','[Accounted Receivables ( Profit )]','Accounted Receivables ( Profit )','Decimal','false',0,0,'false','true','SUM','true','Money','false','false','false','Shipment.AccountedReceivablesInLocalCurrency','false','false')  
declare @Fact_ShipmentsOpenPayablesLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOpenPayablesLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsOpenPayablesLocalNewId,0,'Fact_Shipments','[Open Payables ( Local )]','Open Payables ( Local )','Decimal','false',0,0,'false','true','SUM','true','Money','false','false','false','Shipment.OpenPayablesInLocalCurrency','false','false')  
declare @Fact_ShipmentsOpenPayablesProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOpenPayablesProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsOpenPayablesProfitNewId,0,'Fact_Shipments','[Open Payables ( Profit )]','Open Payables ( Profit )','Decimal','false',0,0,'false','true','SUM','true','Money','false','false','false','Shipment.OpenPayablesInLocalCurrency','false','false')  
declare @Fact_ShipmentsAccountedPayablesLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAccountedPayablesLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsAccountedPayablesLocalNewId,0,'Fact_Shipments','[Accounted Payables ( Local )]','Accounted Payables ( Local )','Decimal','false',0,0,'false','true','SUM','true','Money','false','false','false','Shipment.AccountedPayablesInLocalCurrency','false','false')  
declare @Fact_ShipmentsAccountedPayablesProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAccountedPayablesProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsAccountedPayablesProfitNewId,0,'Fact_Shipments','[Accounted Payables ( Profit )]','Accounted Payables ( Profit )','Decimal','false',0,0,'false','true','SUM','true','Money','false','false','false','Shipment.AccountedPayablesInLocalCurrency','false','false')  
declare @Fact_ShipmentsAgentRef1NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAgentRef1NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsAgentRef1NewId,0,'Fact_Shipments','[Agent Ref1]','Agent Ref1','Text','false',0,50,'false','false','true','References','false','false','false','Shipment.AgentReference1','false','false')  
declare @Fact_ShipmentsAgentRef2NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAgentRef2NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsAgentRef2NewId,0,'Fact_Shipments','[Agent Ref2]','Agent Ref2','Text','false',0,50,'false','false','true','References','false','false','false','Shipment.AgentReference1','false','false')  
declare @Fact_ShipmentsAMSBLNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAMSBLNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsAMSBLNewId,0,'Fact_Shipments','[AMS BL]','AMS BL','Text','false',0,17,'false','false','true','References','false','false','false','Shipment.AMSBL','false','false')  
declare @Fact_ShipmentsConsigneeRef1NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsConsigneeRef1NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsConsigneeRef1NewId,0,'Fact_Shipments','[Consignee Ref1]','Consignee Ref1','Text','false',0,50,'false','false','true','References','false','false','false','Shipment.ConsigneeReference2','false','false','Shipment')  
declare @Fact_ShipmentsConsigneeRef2NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsConsigneeRef2NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsConsigneeRef2NewId,0,'Fact_Shipments','[Consignee Ref2]','Consignee Ref2','Text','false',0,50,'false','false','true','References','false','false','false','Shipment.ConsigneeReference2','false','false','Shipment')  
declare @Fact_ShipmentsCreatedByNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCreatedByNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,DimensionDataViewName) Values(@Fact_ShipmentsCreatedByNewId,0,'Fact_Shipments','[Created By]','Created By','Dimension','false',0,15,'DIM_Users','false','false','true','Operational','false','false','false','Shipment.CreatedByUserId','false','false','dimCreatedBy')  
declare @Fact_ShipmentsCustomAgentNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCustomAgentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsCustomAgentNewId,0,'Fact_Shipments','[Custom Agent]','Custom Agent','Dimension','false',0,15,'DIM_Partners','false','false','true','Partners','false','false','false','Shipment.CustomAgentExportId','false','false','Shipment')  
declare @Fact_ShipmentsCustomerRef1NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCustomerRef1NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsCustomerRef1NewId,0,'Fact_Shipments','[Customer Ref1]','Customer Ref1','Text','false',0,50,'false','false','true','References','false','false','false','Shipment.CustomerReference1','false','false','Shipment')  
declare @Fact_ShipmentsCustomerRef2NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCustomerRef2NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsCustomerRef2NewId,0,'Fact_Shipments','[Customer Ref2]','Customer Ref2','Text','false',0,50,'false','false','true','References','false','false','false','Shipment.CustomerReference1','false','false','Shipment')  
declare @Fact_ShipmentsFirstPickupDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFirstPickupDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsFirstPickupDateNewId,0,'Fact_Shipments','[First Pickup Date]','First Pickup Date','Dimension','false',0,15,'DIM_Dates','false','false','true','Dates','false','false','false','false','false')  
declare @Fact_ShipmentsFreightPCNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFreightPCNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsFreightPCNewId,0,'Fact_Shipments','[Freight PC]','Freight PC','Text','true',0,1,'false','false','true','Operational','false','false','false','Shipment.FreightPrepaidCollectId','false','false')  
declare @Fact_ShipmentsCarrierDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCarrierDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsCarrierDateNewId,0,'Fact_Shipments','[Carrier Date ]','Carrier Date ','Dimension','false',0,15,'DIM_Dates','false','false','true','Dates','false','false','false','false','false')  
declare @Fact_ShipmentsCarrierNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCarrierNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsCarrierNewId,0,'Fact_Shipments','[Carrier]','Carrier','Dimension','false',0,15,'DIM_Partners','false','false','true','Partners','false','false','false','Master.Transshipment1CarrierId','false','false')  
declare @Fact_ShipmentsCarrierNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCarrierNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsCarrierNumberNewId,0,'Fact_Shipments','[Carrier Number]','Flight/Trucker/Voyage','Text','false',0,15,'false','false','true','References','Routings','false','false','false','Master.MainCarriageCarrierNumber','false','false')  
declare @Fact_ShipmentsMainHarmonizeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsMainHarmonizeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsMainHarmonizeNewId,0,'Fact_Shipments','[Main Harmonize]','Main Harmonize','Text','false',0,18,'false','false','true','Operational','false','false','false','Shipment.MainHarmonize','false','false')  
declare @Fact_ShipmentsOtherChargePCNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOtherChargePCNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsOtherChargePCNewId,0,'Fact_Shipments','[Other Charge PC]','Other Charge PC','Text','true',0,1,'false','false','true','Operational','false','false','false','Shipment.OtherPrepaidCollectId','false','false')  
declare @Fact_ShipmentsProject#NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsProject#NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsProject#NewId,0,'Fact_Shipments','[Project#]','Project#','Text','false',0,100,'false','false','true','References','false','false','false','Shipment.ProjectNumber','false','false')  
declare @Fact_ShipmentsShipperRef1NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsShipperRef1NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsShipperRef1NewId,0,'Fact_Shipments','[Shipper Ref1]','Shipper Ref1','Text','false',0,50,'false','false','true','References','false','false','false','Shipment.ShipperReference1','false','false','Shipment')  
declare @Fact_ShipmentsShipperRef2NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsShipperRef2NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsShipperRef2NewId,0,'Fact_Shipments','[Shipper Ref2]','Shipper Ref2','Text','false',0,50,'false','false','true','References','false','false','false','Shipment.ShipperReference1','false','false','Shipment')  
declare @Fact_ShipmentsTEUNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTEUNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTEUNewId,0,'Fact_Shipments','[TEU]','TEU','Double','false',0,15,'false','false','true','Packages','false','false','false','Shipment.TEU','false','false')  
declare @Fact_ShipmentsValueofGoodsNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsValueofGoodsNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsValueofGoodsNewId,0,'Fact_Shipments','[Value of Goods]','Value of Goods','Double','false',0,15,'false','false','true','Money','false','false','false','Shipment.ValueOfGoods','false','false')  
declare @Fact_ShipmentsValueofGoodsCurrencyNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsValueofGoodsCurrencyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsValueofGoodsCurrencyNewId,0,'Fact_Shipments','[Value of Goods Currency]','Value of Goods Currency','Dimension','false',0,15,'DIM_Currencies','false','false','true','Money','false','false','false','Shipment.ValueOfGoodsCurrencyId','false','false')  
declare @Fact_ShipmentsWarehouseTerminalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsWarehouseTerminalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsWarehouseTerminalNewId,0,'Fact_Shipments','[Warehouse Terminal]','Warehouse Terminal','Dimension','false',0,15,'DIM_Partners','false','false','true','Partners','false','false','false','Shipment.WarehouseLegExpectedEntryDate','false','false','Shipment')  
declare @Fact_ShipmentsFreightForwarderNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFreightForwarderNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsFreightForwarderNewId,0,'Fact_Shipments','[Freight Forwarder]','Freight Forwarder','Dimension','false',0,15,'DIM_Partners','false','false','true','Partners','false','false','false','Shipment.FreightForwarderId','false','false','Shipment')  
declare @Fact_ShipmentsBookingConfirmationNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsBookingConfirmationNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsBookingConfirmationNumberNewId,0,'Fact_Shipments','[Booking Confirmation Number]','Booking Confirmation Number','Text','false',0,25,'false','false','true','Operational','References ','false','false','false','Master.BookingConfirmationNumber','false','false')  
declare @Fact_ShipmentsMainCarriageATANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsMainCarriageATANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsMainCarriageATANewId,0,'Fact_Shipments','[Main Carriage ATA]','Main Carriage ATA','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Master.MainCarriageATA','false','false')  
declare @Fact_ShipmentsMasterDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsMasterDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsMasterDateNewId,0,'Fact_Shipments','[Master Date]','Master Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Operational','Dates','false','false','false','Master.MAWBOBLDate','false','false')  
declare @Fact_ShipmentsStatusDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsStatusDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsStatusDateNewId,0,'Fact_Shipments','[Status Date]','Status Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Operational','Dates','false','false','false','Shipment.StatusDate','false','false')  
declare @Fact_ShipmentsCustomsDeclarationNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCustomsDeclarationNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsCustomsDeclarationNumberNewId,0,'Fact_Shipments','[Customs Declaration Number]','Customs Declaration Number','Text','false',0,35,'false','false','true','References ','false','false','false','Shipment.CustomsDeclarationNumber','false','false','Shipment')  
declare @Fact_ShipmentsFirstOperationalCloseDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFirstOperationalCloseDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsFirstOperationalCloseDateNewId,0,'Fact_Shipments','[First Operational Close Date]','First Operational Close Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Operational','Dates','false','false','false','Shipment.FirstOperationalCloseDate','false','false')  
declare @Fact_ShipmentsEstimatedFinalArrivalDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsEstimatedFinalArrivalDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsEstimatedFinalArrivalDateNewId,0,'Fact_Shipments','[Estimated Final Arrival Date]','Estimated Final Arrival Date','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Shipment.EstimatedFinalArrivalDate','false','false')  
declare @Fact_ShipmentsActualFinalArrivalDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsActualFinalArrivalDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsActualFinalArrivalDateNewId,0,'Fact_Shipments','[Actual Final Arrival Date]','Actual Final Arrival Date','DateTime','false',0,0,'false','false','true','Dates','Routings','false','false','false','Shipment.ActualFinalArrivalDate','false','false')  
declare @Fact_ShipmentsRoutingNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsRoutingNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsRoutingNewId,0,'Fact_Shipments','[Routing]','Routing','Text','false',0,100,'false','false','true','Routings','false','false','false','Shipment.Routing','false','false')  
declare @Fact_ShipmentsDescriptionOfGoodsNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsDescriptionOfGoodsNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsDescriptionOfGoodsNewId,0,'Fact_Shipments','[Description Of Goods]','Description Of Goods','nText','false',0,2000,'false','false','true','Operational','false','false','false','Shipment.DescriptionOfGoods','false','false')  
declare @Fact_ShipmentsPreCarriageETDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsPreCarriageETDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsPreCarriageETDNewId,0,'Fact_Shipments','[Pre Carriage ETD]','Pre Carriage ETD','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Shipment.PreCarriageETD','false','false')  
declare @Fact_ShipmentsMainCarriageETANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsMainCarriageETANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsMainCarriageETANewId,0,'Fact_Shipments','[Main Carriage ETA]','Main Carriage ETA','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Master.MainCarriageETA','false','false')  
declare @Fact_ShipmentsMainCarriageETDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsMainCarriageETDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsMainCarriageETDNewId,0,'Fact_Shipments','[Main Carriage ETD]','Main Carriage ETD','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Master.MainCarriageETD','false','false')  
declare @Fact_ShipmentsMoveTypeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsMoveTypeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsMoveTypeNewId,0,'Fact_Shipments','[Move Type]','Move Type','Dimension','true',0,0,'DIM_MoveTypes','false','false','true','Operational','false','false','false','Shipment.MoveTypeId','false','false')  
declare @Fact_ShipmentsVesselNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsVesselNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsVesselNewId,0,'Fact_Shipments','[Vessel]','Vessel','Dimension','true',0,0,'DIM_Vessels','false','false','true','Routings','false','false','false','Master.MainCarriageVesselId','false','false')  
declare @Fact_ShipmentsSpecialServicesNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsSpecialServicesNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,DimensionDataViewName,RecordType) Values(@Fact_ShipmentsSpecialServicesNewId,0,'Fact_Shipments','[Special Services]','Special Services','Dimension','true',0,0,'DIM_SpecialServicesTypes','false','false','true','Operational','false','false','false','Shipment.SpecialServicesTypeId','false','false','dimSpecialServices','Shipment')  
declare @Fact_ShipmentsFirstPickupETDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFirstPickupETDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsFirstPickupETDNewId,0,'Fact_Shipments','[First Pickup ETD]','First Pickup ETD','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Shipment.FirstPickupETD','false','false','Shipment')  
declare @Fact_ShipmentsFirstPickupETANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFirstPickupETANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsFirstPickupETANewId,0,'Fact_Shipments','[First Pickup ETA]','First Pickup ETA','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Shipment.FirstPickupETA','false','false','Shipment')  
declare @Fact_ShipmentsARInvoicesNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsARInvoicesNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsARInvoicesNewId,0,'Fact_Shipments','[AR Invoices]','AR Invoices','Text','false',0,1000,'false','false','true','Operational ','false','false','false','Shipment.ARInvoices','false','false')  
declare @Fact_ShipmentsMasterShipmentNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsMasterShipmentNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsMasterShipmentNumberNewId,0,'Fact_Shipments','[Master Shipment Number]','Master Shipment Number','Text','false',0,20,'false','false','true','References','false','false','false','Shipment.MasterShipmentNumber','false','false')  
declare @Fact_ShipmentsField1NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField1NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField1NewId,0,'Fact_Shipments','[Field1]','Field1','SqlVariant','false',0,2000,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField2NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField2NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField2NewId,0,'Fact_Shipments','[Field2]','Field2','SqlVariant','false',0,2000,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField3NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField3NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField3NewId,0,'Fact_Shipments','[Field3]','Field3','SqlVariant','false',0,2000,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField4NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField4NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField4NewId,0,'Fact_Shipments','[Field4]','Field4','SqlVariant','false',0,2000,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField5NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField5NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField5NewId,0,'Fact_Shipments','[Field5]','Field5','SqlVariant','false',0,2000,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField6NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField6NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField6NewId,0,'Fact_Shipments','[Field6]','Field6','SqlVariant','false',0,2000,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField7NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField7NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField7NewId,0,'Fact_Shipments','[Field7]','Field7','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField8NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField8NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField8NewId,0,'Fact_Shipments','[Field8]','Field8','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField9NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField9NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField9NewId,0,'Fact_Shipments','[Field9]','Field9','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField10NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField10NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField10NewId,0,'Fact_Shipments','[Field10]','Field10','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField11NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField11NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField11NewId,0,'Fact_Shipments','[Field11]','Field11','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField12NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField12NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField12NewId,0,'Fact_Shipments','[Field12]','Field12','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField13NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField13NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField13NewId,0,'Fact_Shipments','[Field13]','Field13','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField14NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField14NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField14NewId,0,'Fact_Shipments','[Field14]','Field14','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField15NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField15NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField15NewId,0,'Fact_Shipments','[Field15]','Field15','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField16NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField16NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField16NewId,0,'Fact_Shipments','[Field16]','Field16','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField17NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField17NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField17NewId,0,'Fact_Shipments','[Field17]','Field17','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField18NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField18NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField18NewId,0,'Fact_Shipments','[Field18]','Field18','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField19NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField19NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField19NewId,0,'Fact_Shipments','[Field19]','Field19','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField20NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField20NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField20NewId,0,'Fact_Shipments','[Field20]','Field20','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField21NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField21NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField21NewId,0,'Fact_Shipments','[Field21]','Field21','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField22NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField22NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField22NewId,0,'Fact_Shipments','[Field22]','Field22','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField23NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField23NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField23NewId,0,'Fact_Shipments','[Field23]','Field23','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField24NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField24NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField24NewId,0,'Fact_Shipments','[Field24]','Field24','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField25NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField25NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField25NewId,0,'Fact_Shipments','[Field25]','Field25','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField26NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField26NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField26NewId,0,'Fact_Shipments','[Field26]','Field26','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField27NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField27NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField27NewId,0,'Fact_Shipments','[Field27]','Field27','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField28NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField28NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField28NewId,0,'Fact_Shipments','[Field28]','Field28','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField29NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField29NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField29NewId,0,'Fact_Shipments','[Field29]','Field29','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField30NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField30NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField30NewId,0,'Fact_Shipments','[Field30]','Field30','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField31NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField31NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField31NewId,0,'Fact_Shipments','[Field31]','Field31','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField32NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField32NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField32NewId,0,'Fact_Shipments','[Field32]','Field32','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField33NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField33NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField33NewId,0,'Fact_Shipments','[Field33]','Field33','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField34NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField34NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField34NewId,0,'Fact_Shipments','[Field34]','Field34','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField35NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField35NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField35NewId,0,'Fact_Shipments','[Field35]','Field35','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField36NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField36NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField36NewId,0,'Fact_Shipments','[Field36]','Field36','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField37NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField37NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField37NewId,0,'Fact_Shipments','[Field37]','Field37','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField38NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField38NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField38NewId,0,'Fact_Shipments','[Field38]','Field38','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField39NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField39NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField39NewId,0,'Fact_Shipments','[Field39]','Field39','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsField40NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsField40NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsField40NewId,0,'Fact_Shipments','[Field40]','Field40','SqlVariant','false',0,0,'false','false','false','CustomFields','false','false','true','false','false')  
declare @Fact_ShipmentsOperationalDateTimeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOperationalDateTimeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsOperationalDateTimeNewId,0,'Fact_Shipments','[Operational Date Time]','Operational Date Time','DateTime','false',0,0,'false','false','true','Dates','Operational','false','false','false','Shipment.OperationalDate','false','false')  
declare @Fact_ShipmentsCreateDateTimeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCreateDateTimeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsCreateDateTimeNewId,0,'Fact_Shipments','[Create Date Time]','Create Date Time','DateTime','false',0,0,'false','false','true','Dates','false','false','false','Shipment.CreateDateTime','false','false')  
declare @Fact_ShipmentsUpdateDateTimeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsUpdateDateTimeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsUpdateDateTimeNewId,0,'Fact_Shipments','[Update Date Time]','Update Date Time','DateTime','false',0,0,'false','false','true','Operational','Operational','false','false','false','Shipment.LastUpdateDate','false','false')  
declare @Fact_ShipmentsCutoffDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCutoffDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsCutoffDateNewId,0,'Fact_Shipments','[Cutoff Date]','Cutoff Date','DateTime','false',0,0,'false','false','true','Operational','Dates','false','false','false','Master.CutoffDate','false','false')  
declare @Fact_ShipmentsConsolidatorNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsConsolidatorNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsConsolidatorNewId,0,'Fact_Shipments','[Consolidator]','Consolidator','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false','false','false','Shipment.ConsolidatorId','false','false')  
declare @Fact_ShipmentsConsolidatorRef1NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsConsolidatorRef1NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsConsolidatorRef1NewId,0,'Fact_Shipments','[Consolidator Ref1]','Consolidator Ref1','Text','false',0,50,'false','false','true','References ','false','false','false','Shipment.ConsolidatorReference','false','false')  
declare @Fact_ShipmentsShipmentNotesNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsShipmentNotesNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsShipmentNotesNewId,0,'Fact_Shipments','[Shipment Notes]','Shipment Notes','nText','false',0,1000,'false','false','true','Operational','General ','false','false','false','Shipment.Notes','false','false')  
declare @Fact_ShipmentsNotify1NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsNotify1NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection,DimensionDataViewName,RecordType) Values(@Fact_ShipmentsNotify1NewId,0,'Fact_Shipments','[Notify 1]','Notify 1','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false','false','false','Shipment.Notify1Id','NotifyOne','false','false','dimNotifyOne','Shipment')  
declare @Fact_ShipmentsNotify1Ref1NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsNotify1Ref1NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsNotify1Ref1NewId,0,'Fact_Shipments','[Notify 1 Ref1]','Notify 1 Ref1','Text','false',0,50,'false','false','true','References ','false','false','false','Shipment.Notify1Reference','false','false','Shipment')  
declare @Fact_ShipmentsNotify2NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsNotify2NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,ViewFieldDisplayName,DontDisplayInView,IsMultipleSelection,DimensionDataViewName,RecordType) Values(@Fact_ShipmentsNotify2NewId,0,'Fact_Shipments','[Notify 2]','Notify 2','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false','false','false','Shipment.Notify2Id','NotifyTwo','false','false','dimNotifyTwo','Shipment')  
declare @Fact_ShipmentsNotify2Ref1NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsNotify2Ref1NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsNotify2Ref1NewId,0,'Fact_Shipments','[Notify 2 Ref1]','Notify 2 Ref1','Text','false',0,50,'false','false','true','References ','false','false','false','Shipment.Notify2Reference','false','false','Shipment')  
declare @Fact_ShipmentsColoaderNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsColoaderNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsColoaderNewId,0,'Fact_Shipments','[Coloader]','Coloader','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false','false','false','Shipment.ColoaderId','false','false','Shipment')  
declare @Fact_ShipmentsColoaderRef1NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsColoaderRef1NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsColoaderRef1NewId,0,'Fact_Shipments','[Coloader Ref1]','Coloader Ref1','Text','false',0,50,'false','false','true','References ','false','false','false','Shipment.ColoaderReference1','false','false','Shipment')  
declare @Fact_ShipmentsShipperNotExporterNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsShipperNotExporterNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,DimensionDataViewName,RecordType) Values(@Fact_ShipmentsShipperNotExporterNewId,0,'Fact_Shipments','[Shipper Not Exporter]','Shipper Not Exporter','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false','false','false','Shipment.ShipperNotExporterId','false','false','dimShippersNotExporters','Shipment')  
declare @Fact_ShipmentsShipperNotExporterRef1NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsShipperNotExporterRef1NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsShipperNotExporterRef1NewId,0,'Fact_Shipments','[Shipper Not Exporter Ref1]','Shipper Not Exporter Ref1','Text','false',0,50,'false','false','true','References','false','false','false','Shipment.ShipperNotExporterReference','false','false','Shipment')  
declare @Fact_ShipmentsReleasingAgentNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsReleasingAgentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsReleasingAgentNewId,0,'Fact_Shipments','[Releasing Agent]','Releasing Agent','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false','false','false','Shipment.ReleasingAgentId','false','false')  
declare @Fact_ShipmentsReleasingAgentRef1NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsReleasingAgentRef1NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsReleasingAgentRef1NewId,0,'Fact_Shipments','[Releasing Agent Ref1]','Releasing Agent Ref1','Text','false',0,50,'false','false','true','References','false','false','false','Shipment.ReleasingAgentReference1','false','false')  
declare @Fact_ShipmentsIncludesCustomsNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsIncludesCustomsNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsIncludesCustomsNewId,0,'Fact_Shipments','[Includes Customs]','Includes Customs','Boolean','false',0,0,'false','false','true','Operational','false','false','false','Shipment.IncludesCustoms','false','false','Shipment')  
declare @Fact_ShipmentsDeclarationNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsDeclarationNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsDeclarationNumberNewId,0,'Fact_Shipments','[Declaration Number]','Declaration Number','Text','false',0,40,'false','false','true','References ','false','false','false','Shipment.DeclarationNumber','false','false','Shipment')  
declare @Fact_ShipmentsDeclarationDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsDeclarationDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsDeclarationDateNewId,0,'Fact_Shipments','[Declaration Date]','Declaration Date','DateTime','false',0,0,'false','false','true','Dates','false','false','false','Shipment.DeclarationDate','false','false','Shipment')  
declare @Fact_ShipmentsCustomsClearanceDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCustomsClearanceDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsCustomsClearanceDateNewId,0,'Fact_Shipments','[Customs Clearance Date]','Customs Clearance Date','DateTime','false',0,0,'false','false','true','Dates','false','false','false','Shipment.CustomsClearanceDate','false','false','Shipment')  
declare @Fact_ShipmentsTerminalAvailableNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTerminalAvailableNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsTerminalAvailableNewId,0,'Fact_Shipments','[Terminal Available]','Terminal Available','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Shipment.TerminalAvailable','false','false','Shipment')  
declare @Fact_ShipmentsWarehouseLastfreeDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsWarehouseLastfreeDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsWarehouseLastfreeDateNewId,0,'Fact_Shipments','[Warehouse Last free Date]','Warehouse Last free Date','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Shipment.WarehouseLegLastFreeDate','false','false','Shipment')  
declare @Fact_ShipmentsWarehouseEntryDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsWarehouseEntryDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsWarehouseEntryDateNewId,0,'Fact_Shipments','[Warehouse Entry Date]','Warehouse Entry Date','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','false','false','Shipment')  
declare @Fact_ShipmentsWarehouseReleaseDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsWarehouseReleaseDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsWarehouseReleaseDateNewId,0,'Fact_Shipments','[Warehouse Release Date]','Warehouse Release Date','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','false','false','Shipment')  
declare @Fact_ShipmentsVolumetricWeightNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsVolumetricWeightNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsVolumetricWeightNewId,0,'Fact_Shipments','[Volumetric Weight]','Volumetric Weight','Text','false',0,40,'false','false','true','Packages','false','false','false','Shipment.VolumetricWeight','false','false')  
declare @Fact_ShipmentsRatioNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsRatioNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsRatioNewId,0,'Fact_Shipments','[Ratio]','Ratio','Text','false',0,20,'false','false','true','Packages','false','false','false','Shipment.Ratio','false','false')  
declare @Fact_ShipmentsFirstPickupATDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFirstPickupATDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsFirstPickupATDNewId,0,'Fact_Shipments','[First Pickup ATD]','First Pickup ATD','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','ShipmentComputedFields.FirstPickupATD','false','false','Shipment')  
declare @Fact_ShipmentsFirstPickupATANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFirstPickupATANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsFirstPickupATANewId,0,'Fact_Shipments','[First Pickup ATA]','First Pickup ATA','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','ShipmentComputedFields.FirstPickupATA','false','false','Shipment')  
declare @Fact_ShipmentsFinalDeliveryETDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFinalDeliveryETDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsFinalDeliveryETDNewId,0,'Fact_Shipments','[Final Delivery ETD]','Final Delivery ETD','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','ShipmentComputedFields.FinalDeliveryETD','false','false','Shipment')  
declare @Fact_ShipmentsFinalDeliveryETANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFinalDeliveryETANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsFinalDeliveryETANewId,0,'Fact_Shipments','[Final Delivery ETA]','Final Delivery ETA','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','ShipmentComputedFields.FinalDeliveryETA','false','false','Shipment')  
declare @Fact_ShipmentsFinalDeliveryATDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFinalDeliveryATDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsFinalDeliveryATDNewId,0,'Fact_Shipments','[Final Delivery ATD]','Final Delivery ATD','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','ShipmentComputedFields.FinalDeliveryATD','false','false','Shipment')  
declare @Fact_ShipmentsFinalDeliveryATANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFinalDeliveryATANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsFinalDeliveryATANewId,0,'Fact_Shipments','[Final Delivery ATA]','Final Delivery ATA','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','ShipmentComputedFields.FinalDeliveryATA','false','false')  
declare @Fact_ShipmentsTransshipment1ETANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment1ETANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment1ETANewId,0,'Fact_Shipments','[Transshipment 1 ETA]','Transshipment 1 ETA','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Master.Transshipment1ETA','false','false')  
declare @Fact_ShipmentsTransshipment1ETDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment1ETDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment1ETDNewId,0,'Fact_Shipments','[Transshipment 1 ETD]','Transshipment 1 ETD','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Master.Transshipment1ETD','false','false')  
declare @Fact_ShipmentsTransshipment1ATANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment1ATANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment1ATANewId,0,'Fact_Shipments','[Transshipment 1 ATA]','Transshipment 1 ATA','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Master.Transshipment1ATA','false','false')  
declare @Fact_ShipmentsTransshipment1ATDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment1ATDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment1ATDNewId,0,'Fact_Shipments','[Transshipment 1 ATD]','Transshipment 1 ATD','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Master.Transshipment1ATD','false','false')  
declare @Fact_ShipmentsTransshipment1VesselNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment1VesselNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment1VesselNewId,0,'Fact_Shipments','[Transshipment 1 Vessel]','Transshipment 1 Vessel','Dimension','false',0,0,'DIM_Vessels','false','false','true','Routings','false','false','false','Master.Transshipment1VesselId','false','false')  
declare @Fact_ShipmentsTransshipment1CarrierNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment1CarrierNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment1CarrierNewId,0,'Fact_Shipments','[Transshipment 1 Carrier]','Transshipment 1 Carrier','Dimension','false',0,0,'DIM_Partners','false','false','true','Routings','false','false','false','Master.Transshipment1CarrierId','false','false')  
declare @Fact_ShipmentsTransshipment1MasterDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment1MasterDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment1MasterDateNewId,0,'Fact_Shipments','[Transshipment 1 Master Date]','Transshipment 1 Master Date','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Master.MAWBOBLDate','false','false')  
declare @Fact_ShipmentsTransshipment1MasterNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment1MasterNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment1MasterNewId,0,'Fact_Shipments','[Transshipment 1 Master]','Transshipment 1 Master','Text','false',0,20,'false','false','true','Routings','References','false','false','false','Master.Transshipment1AdditionalMAWBOBLBL','false','false')  
declare @Fact_ShipmentsContainersNumbersArrayNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsContainersNumbersArrayNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsContainersNumbersArrayNewId,0,'Fact_Shipments','[ContainersNumbers Array]','ContainersNumbers Array','nText','false',0,1000,'false','false','true','Operational','false','false','false','ShipmentComputedFields.ContainersNumbers','false','false')  
declare @Fact_ShipmentsOrderGrossWeightNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOrderGrossWeightNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsOrderGrossWeightNewId,0,'Fact_Shipments','[Order Gross Weight]','Order Gross Weight','Text','false',0,40,'false','false','true','Packages','false','false','false','Shipment.OrderGrossWeight','false','false','Shipment')  
declare @Fact_ShipmentsOrderNumberofPackagesNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOrderNumberofPackagesNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsOrderNumberofPackagesNewId,0,'Fact_Shipments','[Order Number of Packages]','Order Number of Packages','Text','false',0,15,'false','false','true','Packages','false','false','false','Shipment.BookingNumberOfPackages','false','false','Shipment')  
declare @Fact_ShipmentsOrderChargeableWeightNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOrderChargeableWeightNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsOrderChargeableWeightNewId,0,'Fact_Shipments','[Order Chargeable Weight]','Order Chargeable Weight','Decimal','false',0,0,'false','true','SUM','true','Packages','false','false','false','Shipment.OrderChargeableWeight','false','false','Shipment')  
declare @Fact_ShipmentsOrderVolumeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOrderVolumeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsOrderVolumeNewId,0,'Fact_Shipments','[Order Volume]','Order Volume','Text','false',0,40,'false','false','true','Packages','false','false','false','Shipment.BookingVolume','false','false','Shipment')  
declare @Fact_ShipmentsEstimatedProfitProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsEstimatedProfitProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsEstimatedProfitProfitNewId,0,'Fact_Shipments','[Estimated Profit (Profit)]','Estimated Profit (Profit)','Decimal','false',0,0,'false','true','SUM','true','Money','false','false','false','Shipment.EstimateProfitInProfitCurrency','false','false')  
declare @Fact_ShipmentsEstimatedProfitLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsEstimatedProfitLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsEstimatedProfitLocalNewId,0,'Fact_Shipments','[Estimated Profit (Local)]','Estimated Profit (Local)','Decimal','false',0,0,'false','true','SUM','true','Money','false','false','false','Shipment.EstimateProfitInLocalCurrency','false','false')  
declare @Fact_ShipmentsConsigneeNotImporterNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsConsigneeNotImporterNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,DimensionDataViewName,RecordType) Values(@Fact_ShipmentsConsigneeNotImporterNewId,0,'Fact_Shipments','[Consignee Not Importer]','Consignee Not Importer','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false','false','false','Shipment.ConsigneeNotImporterId','false','false','dimConsigneesNotImportes','Shipment')  
declare @Fact_ShipmentsIssuingCarrierAgentNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsIssuingCarrierAgentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsIssuingCarrierAgentNewId,0,'Fact_Shipments','[Issuing Carrier Agent]','Issuing Carrier Agent','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false','false','false','Shipment.IssuingCarrierAgentId','false','false')  
declare @Fact_ShipmentsOrderConfirmationNotesNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOrderConfirmationNotesNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsOrderConfirmationNotesNewId,0,'Fact_Shipments','[Order Confirmation Notes]','Order Confirmation Notes','nText','false',0,250,'false','false','true','Operational','false','false','false','Master.BookingConfirmationNotes','false','false','Shipment')  
declare @Fact_ShipmentsOrderConfirmedByNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOrderConfirmedByNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsOrderConfirmedByNewId,0,'Fact_Shipments','[Order Confirmed By]','Order Confirmed By','Text','false',0,40,'false','false','true','Operational','false','false','false','Master.BookingConfirmedBy','false','false','Shipment')  
declare @Fact_ShipmentsOnCarriageTransportModeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOnCarriageTransportModeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsOnCarriageTransportModeNewId,0,'Fact_Shipments','[On Carriage Transport Mode]','On Carriage Transport Mode','Dimension','false',0,13,'DIM_TransportModes','false','false','true','Routings','Operational','true','false','false','Shipment.OnCarriageTransportModeId','false','false','Shipment')  
declare @Fact_ShipmentsFirstARInvoiceApprovalDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFirstARInvoiceApprovalDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsFirstARInvoiceApprovalDateNewId,0,'Fact_Shipments','[First AR Invoice Approval Date]','First AR Invoice Approval Date','DateTime','false',0,0,'false','false','true','Operational','Dates','false','false','false','Shipment.FirstARInvoiceApprovalDate','false','false')  
declare @Fact_ShipmentsNumberofDeliveriesNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsNumberofDeliveriesNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsNumberofDeliveriesNewId,0,'Fact_Shipments','[Number of Deliveries]','Number of Deliveries','Integer','false',0,100,'false','false','true','Operational','false','false','false','ShipmentComputedFields.NumberOfDeliveries','false','false','Shipment')  
declare @Fact_ShipmentsOperationalClosedByNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOperationalClosedByNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,DimensionDataViewName) Values(@Fact_ShipmentsOperationalClosedByNewId,0,'Fact_Shipments','[Operational Closed By]','Operational Closed By','Dimension','false',0,0,'DIM_Users','false','false','true','Operational','false','false','false','ShipmentComputedFields.OperationallyClosedByUserId','false','false','dimOperationalClosedBy')  
declare @Fact_ShipmentsLastPickupATANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsLastPickupATANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsLastPickupATANewId,0,'Fact_Shipments','[Last Pickup ATA]','Last Pickup ATA','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','ShipmentComputedFields.LastPickupATA','false','false','Shipment')  
declare @Fact_ShipmentsLastPickupETANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsLastPickupETANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsLastPickupETANewId,0,'Fact_Shipments','[Last Pickup ETA]','Last Pickup ETA','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','ShipmentComputedFields.LastPickupETA','false','false','Shipment')  
declare @Fact_ShipmentsDeliveryToPortNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsDeliveryToPortNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsDeliveryToPortNewId,0,'Fact_Shipments','[Delivery To Port]','Delivery To Port','Dimension','false',0,0,'DIM_Ports','false','false','true','Routings','false','false','false','ShipmentComputedFields.DeliveryToPortId','false','false','Shipment')  
declare @Fact_ShipmentsLastPickupETDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsLastPickupETDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsLastPickupETDNewId,0,'Fact_Shipments','[Last Pickup ETD]','Last Pickup ETD','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','ShipmentComputedFields.LastPickupETD','false','false','Shipment')  
declare @Fact_ShipmentsLastPickupATDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsLastPickupATDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsLastPickupATDNewId,0,'Fact_Shipments','[Last Pickup ATD]','Last Pickup ATD','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','ShipmentComputedFields.LastPickupATD','false','false','Shipment')  
declare @Fact_ShipmentsDeliveryFromNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsDeliveryFromNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsDeliveryFromNewId,0,'Fact_Shipments','[Delivery From]','Delivery From','Text','false',0,40,'false','false','true','Routings','false','false','false','ShipmentComputedFields.DeliveryFrom','false','false','Shipment')  
declare @Fact_ShipmentsDeliveryToNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsDeliveryToNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsDeliveryToNewId,0,'Fact_Shipments','[Delivery To]','Delivery To','Text','false',0,40,'false','false','true','Routings','false','false','false','ShipmentComputedFields.DeliveryTo','false','false','Shipment')  
declare @Fact_ShipmentsPickupFromNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsPickupFromNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsPickupFromNewId,0,'Fact_Shipments','[Pickup From]','Pickup From','Text','false',0,40,'false','false','true','Routings','false','false','false','ShipmentComputedFields.PickupFrom','false','false','Shipment')  
declare @Fact_ShipmentsPickupToNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsPickupToNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsPickupToNewId,0,'Fact_Shipments','[Pickup To]','Pickup To','Text','false',0,40,'false','false','true','Routings','false','false','false','ShipmentComputedFields.PickupTo','false','false','Shipment')  
declare @Fact_ShipmentsFreightReleaseNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFreightReleaseNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsFreightReleaseNewId,0,'Fact_Shipments','[Freight Release]','Freight Release','Date','false',0,100,'false','false','true','Operational','Dates','false','false','false','Shipment.FreightRelease','false','false')  
declare @Fact_ShipmentsDangerousGoodsNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsDangerousGoodsNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsDangerousGoodsNewId,0,'Fact_Shipments','[Dangerous Goods]','Dangerous Goods','Boolean','false',0,0,'false','false','true','Packages','Packages','false','false','false','Shipment.IsDangerous','false','false')  
declare @Fact_ShipmentsNextETANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsNextETANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsNextETANewId,0,'Fact_Shipments','[Next ETA]','Next ETA','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Shipment.NextETA','false','false','Shipment')  
declare @Fact_ShipmentsNextETDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsNextETDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsNextETDNewId,0,'Fact_Shipments','[Next ETD]','Next ETD','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Shipment.NextETD','false','false','Shipment')  
declare @Fact_ShipmentsLastFinalDestinationNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsLastFinalDestinationNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsLastFinalDestinationNewId,0,'Fact_Shipments','[Last Final Destination]','Final Destination','Text','false',0,150,'false','false','true','Routings','false','false','false','Shipment.LastFinalDestination','false','false','Shipment')  
declare @Fact_ShipmentsGrossWeightPerTonNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsGrossWeightPerTonNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsGrossWeightPerTonNewId,0,'Fact_Shipments','[Gross Weight Per Ton]','Gross Weight Per Ton','Decimal','false',0,0,'false','false','true','Packages','false','false','false','Shipment.GrossWeightPerTon','false','false','Shipment')  
declare @Fact_ShipmentsRegistryDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsRegistryDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsRegistryDateNewId,0,'Fact_Shipments','[Registry Date]','Registry Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Operational','Dates','false','false','false','Shipment.RegistryDate','false','false')  
declare @Fact_ShipmentsAWBPrintNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAWBPrintNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsAWBPrintNewId,0,'Fact_Shipments','[AWB Print]','Is AWB Printed','Boolean','false',0,0,'false','false','true','Operational','false','false','false','Shipment.AWBPrint','false','false','Shipment')  
declare @Fact_ShipmentsCarrierLastStatusDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCarrierLastStatusDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsCarrierLastStatusDateNewId,0,'Fact_Shipments','[Carrier Last Status Date]','Carrier Last Status Date','DateTime','false',0,0,'false','false','true','Operational','false','false','false','Shipment.CarrierLastStatusDate','false','false','Shipment')  
declare @Fact_ShipmentsPayableStatusNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsPayableStatusNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsPayableStatusNewId,0,'Fact_Shipments','[Payable Status]','Payable Status','Dimension','false',0,40,'DIM_ShipmentPayableStatuses','false','false','true','Money','true','false','false','Shipment.ShipmentPayableStatusCode','false','false')  
declare @Fact_ShipmentsReceivableStatusNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsReceivableStatusNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsReceivableStatusNewId,0,'Fact_Shipments','[Receivable Status]','Receivable Status','Dimension','false',0,40,'DIM_ShipmentReceivableStatuses','false','false','true','Money','true','false','false','Shipment.ShipmentReceivableStatusCode','false','false')  
declare @Fact_ShipmentsExceptionDescriptionNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsExceptionDescriptionNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsExceptionDescriptionNewId,0,'Fact_Shipments','[Exception Description]','Exception Description','nText','false',0,500,'false','false','true','KPI','false','false','false','Shipment.ExceptionDescription','false','false','Shipment')  
declare @Fact_ShipmentsHasExceptionNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsHasExceptionNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsHasExceptionNewId,0,'Fact_Shipments','[Has Exception]','Has Exception','Boolean','false',0,0,'false','false','true','KPI','false','false','false','Shipment.HasException','false','false','Shipment')  
declare @Fact_ShipmentsExceptionResolvedDescriptionNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsExceptionResolvedDescriptionNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsExceptionResolvedDescriptionNewId,0,'Fact_Shipments','[Exception Resolved Description]','Exception Resolved Description','nText','false',0,500,'false','false','true','KPI','false','false','false','Shipment.ExceptionResolvedDescription','false','false','Shipment')  
declare @Fact_ShipmentsLastExceptionDescriptionNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsLastExceptionDescriptionNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsLastExceptionDescriptionNewId,0,'Fact_Shipments','[Last Exception Description]','Last Exception Description','nText','false',0,2000,'false','false','true','KPI','false','false','false','Shipment.LastExceptionDescription','false','false','Shipment')  
declare @Fact_ShipmentsCommodityNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCommodityNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsCommodityNewId,0,'Fact_Shipments','[Commodity]','Commodity','nText','false',0,15,'false','false','true','Packages','false','false','false','ShipmentComputedFields.Commodity','false','false')  
declare @Fact_ShipmentsFirstPickupLocationNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFirstPickupLocationNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsFirstPickupLocationNewId,0,'Fact_Shipments','[First Pickup Location]','First Pickup Location','nText','false',0,100,'false','false','true','Routings','false','false','false','ShipmentComputedFields.FirstPickupLocation','false','false','Shipment')  
declare @Fact_ShipmentsTrailerNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTrailerNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTrailerNumberNewId,0,'Fact_Shipments','[Trailer Number]','Trailer Number','Text','false',0,15,'false','false','true','Routings','References','false','false','false','Master.TrailerNumber','false','false')  
declare @Fact_ShipmentsFromLocationNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFromLocationNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsFromLocationNewId,0,'Fact_Shipments','[From Location]','From Location','Text','false',0,100,'false','false','false','Routings','false','false','false','Master.FromLocation','false','false')  
declare @Fact_ShipmentsToLocationNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsToLocationNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsToLocationNewId,0,'Fact_Shipments','[To Location]','To Location','Text','false',0,100,'false','false','false','Routings','false','false','false','Master.ToLocation','false','false')  
declare @Fact_ShipmentsFinalDeliveryTruckerNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFinalDeliveryTruckerNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsFinalDeliveryTruckerNewId,0,'Fact_Shipments','[Final Delivery Trucker]','Final Delivery Trucker','Dimension','false',0,15,'DIM_Partners','false','false','true','Routings','false','false','false','ShipmentComputedFields.DeliveryTruckerId','false','false','Shipment')  
declare @Fact_ShipmentsFinalDeliveryTruckerNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFinalDeliveryTruckerNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsFinalDeliveryTruckerNumberNewId,0,'Fact_Shipments','[Final Delivery Trucker Number]','Final Delivery Trucker Number','Text','false',0,15,'false','false','true','Routings','false','false','false','ShipmentComputedFields.DeliveryTruckerNumber','false','false','Shipment')  
declare @Fact_ShipmentsFinalDeliveryDriverNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFinalDeliveryDriverNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsFinalDeliveryDriverNewId,0,'Fact_Shipments','[Final Delivery Driver]','Final Delivery Driver','Text','false',0,40,'false','false','true','Routings','false','false','false','ShipmentComputedFields.DeliveryDriver','false','false','Shipment')  
declare @Fact_ShipmentsFinalDeliveryTrailerNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFinalDeliveryTrailerNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsFinalDeliveryTrailerNumberNewId,0,'Fact_Shipments','[Final Delivery Trailer Number]','Final Delivery Trailer Number','Text','false',0,15,'false','false','true','Routings','false','false','false','ShipmentComputedFields.DeliveryTrailerNumber','false','false','Shipment')  
declare @Fact_ShipmentsFinalDeliveryNotesNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFinalDeliveryNotesNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsFinalDeliveryNotesNewId,0,'Fact_Shipments','[Final Delivery Notes]','Final Delivery Notes','nText','false',0,2000,'false','false','true','Routings','false','false','false','ShipmentComputedFields.DeliveryNotes','false','false','Shipment')  
declare @Fact_ShipmentsFirstPickupTruckerNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFirstPickupTruckerNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsFirstPickupTruckerNewId,0,'Fact_Shipments','[First Pickup Trucker]','First Pickup Trucker','Dimension','false',0,15,'DIM_Partners','false','false','true','Routings','false','false','false','ShipmentComputedFields.PickupTruckerId','false','false','Shipment')  
declare @Fact_ShipmentsFirstPickupTruckerNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFirstPickupTruckerNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsFirstPickupTruckerNumberNewId,0,'Fact_Shipments','[First Pickup Trucker Number]','First Pickup Trucker Number','Text','false',0,15,'false','false','true','Routings','false','false','false','ShipmentComputedFields.PickupTruckerNumber','false','false','Shipment')  
declare @Fact_ShipmentsFirstPickupDriverNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFirstPickupDriverNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsFirstPickupDriverNewId,0,'Fact_Shipments','[First Pickup Driver]','First Pickup Driver','Text','false',0,40,'false','false','true','Routings','false','false','false','ShipmentComputedFields.PickupDriver','false','false','Shipment')  
declare @Fact_ShipmentsFirstPickupTrailerNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFirstPickupTrailerNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsFirstPickupTrailerNumberNewId,0,'Fact_Shipments','[First Pickup Trailer Number]','First Pickup Trailer Number','Text','false',0,15,'false','false','true','Routings','false','false','false','ShipmentComputedFields.PickupTrailerNumber','false','false','Shipment')  
declare @Fact_ShipmentsFirstPickupNotesNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFirstPickupNotesNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsFirstPickupNotesNewId,0,'Fact_Shipments','[First Pickup Notes]','First Pickup Notes','nText','false',0,2000,'false','false','true','Routings','false','false','false','ShipmentComputedFields.PickupNotes','false','false','Shipment')  
declare @Fact_ShipmentsOrderGrossWeightinTonNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOrderGrossWeightinTonNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsOrderGrossWeightinTonNewId,0,'Fact_Shipments','[Order Gross Weight in Ton]','Order Gross Weight in Ton','Decimal','false',0,15,'false','true','SUM','true','Packages','false','false','false','Shipment.GrossWeightPerTon','false','false','Shipment')  
declare @Fact_ShipmentsDocumentClosingDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsDocumentClosingDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsDocumentClosingDateNewId,0,'Fact_Shipments','[Document Closing Date]','Document Closing Date','DateTime','false',0,15,'false','false','true','Routings','Dates','false','false','false','Master.DocumentsClosingDate','false','false','Shipment')  
declare @Fact_ShipmentsDeliveryDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsDeliveryDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,HelpText,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsDeliveryDateNewId,0,'Fact_Shipments','[Delivery Date]','Delivery Date','DateTime','false',0,0,'false','false','true','KPI','Dates','false','false','ATD of first Delivery','false','ShipmentComputedFields.DeliveryDate','false','false','Shipment')  
declare @Fact_ShipmentsOnHandDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOnHandDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,HelpText,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsOnHandDateNewId,0,'Fact_Shipments','[On Hand Date]','On-Hand Date','DateTime','false',0,0,'false','false','true','KPI','Dates','false','false','ATA of first Pickup','false','ShipmentComputedFields.OnHandDate','false','false','Shipment')  
declare @Fact_ShipmentsPODDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsPODDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,HelpText,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsPODDateNewId,0,'Fact_Shipments','[POD Date]','POD Date','DateTime','false',0,0,'false','false','true','KPI','Dates','false','false','ATA of final Delivery','false','ShipmentComputedFields.PODDate','false','false','Shipment')  
declare @Fact_ShipmentsInWarehouseDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsInWarehouseDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,HelpText,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsInWarehouseDateNewId,0,'Fact_Shipments','[In Warehouse Date]','In Warehouse Date','DateTime','false',0,0,'false','false','true','KPI','Dates','false','false','Actual Entry Date from Warehouse/Terminal leg in import shipments','false','Shipment.WarehouseLegActualEntryDate','false','false','Shipment')  
declare @Fact_ShipmentsBookingConfirmationSentNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsBookingConfirmationSentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsBookingConfirmationSentNewId,0,'Fact_Shipments','[Booking Confirmation Sent]','Booking Confirmation Sent Date','DateTime','false',0,0,'false','false','true','KPI','false','false','false','ShipmentComputedFields.BookingConfirmationSent','false','false','Shipment')  
declare @Fact_ShipmentsPreAlertSentNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsPreAlertSentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsPreAlertSentNewId,0,'Fact_Shipments','[Pre Alert Sent]','Pre-Alert Sent Date','DateTime','false',0,0,'false','false','true','KPI','false','false','false','ShipmentComputedFields.PreAlertSent','false','false','Shipment')  
declare @Fact_ShipmentsDeliveryNoticeSentNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsDeliveryNoticeSentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsDeliveryNoticeSentNewId,0,'Fact_Shipments','[Delivery Notice Sent]','Delivery Notice Sent Date','DateTime','false',0,0,'false','false','true','KPI','false','false','false','ShipmentComputedFields.DeliveryNoticeSent','false','false','Shipment')  
declare @Fact_ShipmentsExpectedArrivalNoticeSentNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsExpectedArrivalNoticeSentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsExpectedArrivalNoticeSentNewId,0,'Fact_Shipments','[Expected Arrival Notice Sent]','Expected Arrival Notice Sent Date','DateTime','false',0,0,'false','false','true','KPI','false','false','false','ShipmentComputedFields.ExpectedArrivalNoticeSent','false','false','Shipment')  
declare @Fact_ShipmentsT1ReceivedNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsT1ReceivedNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsT1ReceivedNewId,0,'Fact_Shipments','[T1 Received]','T1 Received Date','DateTime','false',0,0,'false','false','true','KPI','false','false','false','ShipmentComputedFields.T1Received','false','false','Shipment')  
declare @Fact_ShipmentsArrivalNoticeSentNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsArrivalNoticeSentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection,RecordType) Values(@Fact_ShipmentsArrivalNoticeSentNewId,0,'Fact_Shipments','[Arrival Notice Sent]','Arrival Notice Sent Date','DateTime','false',0,0,'false','false','true','KPI','false','false','false','ShipmentComputedFields.ArrivalNoticeSent','false','false','Shipment')  
declare @Fact_ShipmentsOBLTypeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOBLTypeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsOBLTypeNewId,0,'Fact_Shipments','[OBL Type]','OBL Type','Dimension','false',0,0,'DIM_OBLTypes','false','false','true','Routings','Operational','false','false','false','Master.OBLTypeCode','false','false')  
declare @Fact_ShipmentsUnNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsUnNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsUnNumberNewId,0,'Fact_Shipments','[Un Number]','UN Number','Text','false',0,4,'false','false','true','packages','false','false','false','Shipment.DangerousUnNumber','false','false')  
declare @Fact_ShipmentsContainersNumbersandTypesArrayNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsContainersNumbersandTypesArrayNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsContainersNumbersandTypesArrayNewId,0,'Fact_Shipments','[Containers Numbers and Types Array]','Containers Numbers and Types Array','nText','false',2000,2000,'false','false','true','packages','false','false','false','ShipmentComputedFields.ContainersNumbersandTypesArray','false','false')  
declare @Fact_ShipmentsPreCarriageCarrierNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsPreCarriageCarrierNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsPreCarriageCarrierNumberNewId,0,'Fact_Shipments','[Pre Carriage Carrier Number]','Pre Carriage Carrier Number','Text','false',0,15,'false','false','true','Routings','false','false','false','Shipment.PreCarriageCarrierNumber','false','false')  
declare @Fact_ShipmentsOnCarriageCarrierNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOnCarriageCarrierNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsOnCarriageCarrierNumberNewId,0,'Fact_Shipments','[On Carriage Carrier Number]','On Carriage Carrier Number','Text','false',0,15,'false','false','true','Routings','false','false','false','Shipment.OnCarriageCarrierNumber','false','false')  
declare @Fact_ShipmentsPreCarriageETANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsPreCarriageETANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsPreCarriageETANewId,0,'Fact_Shipments','[Pre Carriage ETA]','Pre Carriage ETA','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Shipment.PreCarriageETA','false','false')  
declare @Fact_ShipmentsPreCarriageATDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsPreCarriageATDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsPreCarriageATDNewId,0,'Fact_Shipments','[Pre Carriage ATD]','Pre Carriage ATD','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Shipment.PreCarriageATD','false','false')  
declare @Fact_ShipmentsPreCarriageATANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsPreCarriageATANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsPreCarriageATANewId,0,'Fact_Shipments','[Pre Carriage ATA]','Pre Carriage ATA','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Shipment.PreCarriageATA','false','false')  
declare @Fact_ShipmentsOnCarriageETDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOnCarriageETDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsOnCarriageETDNewId,0,'Fact_Shipments','[On Carriage ETD]','On Carriage ETD','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Shipment.OnCarriageETD','false','false')  
declare @Fact_ShipmentsOnCarriageETANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOnCarriageETANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsOnCarriageETANewId,0,'Fact_Shipments','[On Carriage ETA]','On Carriage ETA','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Shipment.OnCarriageETA','false','false')  
declare @Fact_ShipmentsOnCarriageATDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOnCarriageATDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsOnCarriageATDNewId,0,'Fact_Shipments','[On Carriage ATD]','On Carriage ATD','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Shipment.OnCarriageATD','false','false')  
declare @Fact_ShipmentsOnCarriageATANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOnCarriageATANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsOnCarriageATANewId,0,'Fact_Shipments','[On Carriage ATA]','On Carriage ATA','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Shipment.OnCarriageATA','false','false')  
declare @Fact_ShipmentsTransshipment2ATANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment2ATANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment2ATANewId,0,'Fact_Shipments','[Transshipment 2 ATA]','Transshipment 2 ATA','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Master.Transshipment2ATA','false','false')  
declare @Fact_ShipmentsTransshipment3ATANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment3ATANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment3ATANewId,0,'Fact_Shipments','[Transshipment 3 ATA]','Transshipment 3 ATA','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Master.Transshipment3ATA','false','false')  
declare @Fact_ShipmentsTransshipment2ETANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment2ETANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment2ETANewId,0,'Fact_Shipments','[Transshipment 2 ETA]','Transshipment 2 ETA','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Master.Transshipment2ETA','false','false')  
declare @Fact_ShipmentsTransshipment3ETANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment3ETANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment3ETANewId,0,'Fact_Shipments','[Transshipment 3 ETA]','Transshipment 3 ETA','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Master.Transshipment3ETA','false','false')  
declare @Fact_ShipmentsTransshipment2ATDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment2ATDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment2ATDNewId,0,'Fact_Shipments','[Transshipment 2 ATD]','Transshipment 2 ATD','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Master.Transshipment2ATD','false','false')  
declare @Fact_ShipmentsTransshipment3ATDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment3ATDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment3ATDNewId,0,'Fact_Shipments','[Transshipment 3 ATD]','Transshipment 3 ATD','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Master.Transshipment3ATD','false','false')  
declare @Fact_ShipmentsTransshipment2ETDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment2ETDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment2ETDNewId,0,'Fact_Shipments','[Transshipment 2 ETD]','Transshipment 2 ETD','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Master.Transshipment2ETD','false','false')  
declare @Fact_ShipmentsTransshipment3ETDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment3ETDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment3ETDNewId,0,'Fact_Shipments','[Transshipment 3 ETD]','Transshipment 3 ETD','DateTime','false',0,0,'false','false','true','Routings','Dates','false','false','false','Master.Transshipment3ETD','false','false')  
declare @Fact_ShipmentsTransshipment2CarrierNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment2CarrierNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment2CarrierNewId,0,'Fact_Shipments','[Transshipment 2 Carrier]','Transshipment 2 Carrier','Dimension','false',0,0,'DIM_Partners','false','false','true','Routings','false','false','false','Master.Transshipment2CarrierId','false','false')  
declare @Fact_ShipmentsTransshipment3CarrierNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment3CarrierNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment3CarrierNewId,0,'Fact_Shipments','[Transshipment 3 Carrier]','Transshipment 3 Carrier','Dimension','false',0,0,'DIM_Partners','false','false','true','Routings','false','false','false','Master.Transshipment3CarrierId','false','false')  
declare @Fact_ShipmentsTransshipment2MasterNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment2MasterNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment2MasterNewId,0,'Fact_Shipments','[Transshipment 2 Master]','Transshipment 2 Master','Text','false',0,20,'false','false','true','Routings','false','false','false','Master.Transshipment2AdditionalMAWBOBLBL','false','false')  
declare @Fact_ShipmentsTransshipment3MasterNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment3MasterNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment3MasterNewId,0,'Fact_Shipments','[Transshipment 3 Master]','Transshipment 3 Master','Text','false',0,20,'false','false','true','Routings','false','false','false','Master.Transshipment3AdditionalMAWBOBLBL','false','false')  
declare @Fact_ShipmentsPreCarriageTransportModeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsPreCarriageTransportModeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsPreCarriageTransportModeNewId,0,'Fact_Shipments','[Pre Carriage Transport Mode]','Pre Carriage Transport Mode','Dimension','false',0,13,'DIM_TransportModes','false','false','true','Routings','true','false','false','Shipment.PreCarriageTransportModeId','false','false')  
declare @Fact_ShipmentsPreCarriageFromPortNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsPreCarriageFromPortNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsPreCarriageFromPortNewId,0,'Fact_Shipments','[Pre Carriage From Port]','Pre Carriage From Port','Dimension','false',0,15,'DIM_Ports','false','false','true','Routings','false','false','false','Shipments.PreCarriageFromPortId','false','false')  
declare @Fact_ShipmentsPreCarriageToPortNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsPreCarriageToPortNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsPreCarriageToPortNewId,0,'Fact_Shipments','[Pre Carriage To Port]','Pre Carriage To Port','Dimension','false',0,15,'DIM_Ports','false','false','true','Routings','false','false','false','Shipments.PreCarriageToPortId','false','false')  
declare @Fact_ShipmentsOnCarriageToPortNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOnCarriageToPortNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsOnCarriageToPortNewId,0,'Fact_Shipments','[On Carriage To Port]','On Carriage To Port','Dimension','false',0,15,'DIM_Ports','false','false','true','Routings','false','false','false','Shipments.OnCarriageToPortId','false','false')  
declare @Fact_ShipmentsOnCarriageFromPortNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOnCarriageFromPortNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsOnCarriageFromPortNewId,0,'Fact_Shipments','[On Carriage From Port]','On Carriage From Port','Dimension','false',0,15,'DIM_Ports','false','false','true','Routings','false','false','false','Shipments.OnCarriageFromPortId','false','false')  
declare @Fact_ShipmentsTransshipment1FromPortNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment1FromPortNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment1FromPortNewId,0,'Fact_Shipments','[Transshipment 1 From Port]','Transshipment 1 From Port','Dimension','false',0,15,'DIM_Ports','false','false','true','Routings','false','false','false','Master.Transshipment1FromPortId','false','false')  
declare @Fact_ShipmentsTransshipment2FromPortNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment2FromPortNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment2FromPortNewId,0,'Fact_Shipments','[Transshipment 2 From Port]','Transshipment 2 From Port','Dimension','false',0,15,'DIM_Ports','false','false','true','Routings','false','false','false','Master.Transshipment2FromPortId','false','false')  
declare @Fact_ShipmentsTransshipment3FromPortNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment3FromPortNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment3FromPortNewId,0,'Fact_Shipments','[Transshipment 3 From Port]','Transshipment 3 From Port','Dimension','false',0,15,'DIM_Ports','false','false','true','Routings','false','false','false','Master.Transshipment3FromPortId','false','false')  
declare @Fact_ShipmentsTransshipment1ToPortNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment1ToPortNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment1ToPortNewId,0,'Fact_Shipments','[Transshipment 1 To Port]','Transshipment 1 To Port','Dimension','false',0,15,'DIM_Ports','false','false','true','Routings','false','false','false','Master.Transshipment1ToPortd','false','false')  
declare @Fact_ShipmentsTransshipment2ToPortNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment2ToPortNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment2ToPortNewId,0,'Fact_Shipments','[Transshipment 2 To Port]','Transshipment 2 To Port','Dimension','false',0,15,'DIM_Ports','false','false','true','Routings','false','false','false','Master.Transshipment2ToPortd','false','false')  
declare @Fact_ShipmentsTransshipment3ToPortNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransshipment3ToPortNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsTransshipment3ToPortNewId,0,'Fact_Shipments','[Transshipment 3 To Port]','Transshipment 3 To Port','Dimension','false',0,15,'DIM_Ports','false','false','true','Routings','false','false','false','Master.Transshipment3ToPortd','false','false')  
declare @Fact_ShipmentsPreCarriageCarrierNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsPreCarriageCarrierNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsPreCarriageCarrierNewId,0,'Fact_Shipments','[Pre Carriage Carrier]','Pre Carriage Carrier','Dimension','false',0,15,'DIM_Partners','false','false','true','Routings','false','false','false','Shipments.PreCarriageCarrierId','false','false')  
declare @Fact_ShipmentsOnCarriageCarrierNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOnCarriageCarrierNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsOnCarriageCarrierNewId,0,'Fact_Shipments','[On Carriage Carrier]','On Carriage Carrier','Dimension','false',0,15,'DIM_Partners','false','false','true','Routings','false','false','false','Shipments.OnCarriageCarrierId','false','false')  
declare @Fact_ShipmentsConnectedQuoteNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsConnectedQuoteNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree,CannotFilter,IsCustom,OriginalObjectFieldCode,DontDisplayInView,IsMultipleSelection) Values(@Fact_ShipmentsConnectedQuoteNewId,0,'Fact_Shipments','[Connected Quote]','Connected Quote','Text','false',0,20,'false','false','true','General','false','false','false','Shipment.QuoteNumber','false','false')  
