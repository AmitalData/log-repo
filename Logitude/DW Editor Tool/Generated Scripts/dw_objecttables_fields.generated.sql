 
  

-- this script is generated
delete from DWObjectFields
delete from DWObjectTables
------------------------------------------------------------------------------------
declare @DIM_BranchesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed,DefaultFilterBy) Values(@DIM_BranchesNewId,0,'DIM_Branches','DIM_Branches','Dimension','false','[Name]')  
--Fields --
declare @DIM_BranchesId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_BranchesId_NumberNewId,0,'DIM_Branches','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false')  
declare @DIM_BranchesIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_BranchesIdNewId,0,'DIM_Branches','[Id]','Id','Text','true',0,15,'false','false','false','false')  
declare @DIM_BranchesNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_BranchesNameNewId,0,'DIM_Branches','[Name]','Name','Text','true',0,40,'false','false','true','[Code]','false')  
declare @DIM_BranchesLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_BranchesLocalNameNewId,0,'DIM_Branches','[Local Name]','Local Name','nText','false',0,40,'false','false','true','false')  
declare @DIM_BranchesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_BranchesCodeNewId,0,'DIM_Branches','[Code]','Code','Text','false',0,13,'false','false','true','[Name]','false')  
declare @DIM_BranchesSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_BranchesSourceTenantNewId,0,'DIM_Branches','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false')  
declare @DIM_BranchesParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_BranchesParentTenantNewId,0,'DIM_Branches','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_CurrenciesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed,DefaultFilterBy) Values(@DIM_CurrenciesNewId,0,'DIM_Currencies','DIM_Currencies','Dimension','false','[Name]')  
--Fields --
declare @DIM_CurrenciesId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_CurrenciesId_NumberNewId,0,'DIM_Currencies','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false')  
declare @DIM_CurrenciesIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_CurrenciesIdNewId,0,'DIM_Currencies','[Id]','Id','Text','true',0,15,'false','false','false','false')  
declare @DIM_CurrenciesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_CurrenciesCodeNewId,0,'DIM_Currencies','[Code]','Code','Text','true',0,3,'false','false','true','[Name]','false')  
declare @DIM_CurrenciesNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_CurrenciesNameNewId,0,'DIM_Currencies','[Name]','Name','Text','true',0,40,'false','false','true','[Code]','false')  
declare @DIM_CurrenciesLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_CurrenciesLocalNameNewId,0,'DIM_Currencies','[Local Name]','Local Name','nText','false',0,40,'false','false','true','false')  
declare @DIM_CurrenciesCurrencySignNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesCurrencySignNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_CurrenciesCurrencySignNewId,0,'DIM_Currencies','[Currency Sign]','Currency Sign','nText','false',0,3,'false','false','true','false')  
declare @DIM_CurrenciesSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_CurrenciesSourceTenantNewId,0,'DIM_Currencies','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false')  
declare @DIM_CurrenciesParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_CurrenciesParentTenantNewId,0,'DIM_Currencies','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_DatesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed,DefaultFilterBy) Values(@DIM_DatesNewId,0,'DIM_Dates','DIM_Dates','Dimension','false','[Full Date]')  
--Fields --
declare @DIM_DatesDateKeyNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesDateKeyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DatesDateKeyNewId,0,'DIM_Dates','[Date Key]','Date Key','Integer','true',0,0,'true','false','true','false')  
declare @DIM_DatesFullDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesFullDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DatesFullDateNewId,0,'DIM_Dates','[Full Date]','Full Date','DateTime','false',0,0,'false','false','true','false')  
declare @DIM_DatesFullDateUSNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesFullDateUSNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DatesFullDateUSNewId,0,'DIM_Dates','[Full Date US]','Full Date US','Text','false',0,100,'false','false','true','false')  
declare @DIM_DatesDayOfWeekNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesDayOfWeekNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DatesDayOfWeekNewId,0,'DIM_Dates','[Day Of Week]','Day Of Week','Integer','false',0,0,'false','false','true','false')  
declare @DIM_DatesDayNumInMonthNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesDayNumInMonthNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DatesDayNumInMonthNewId,0,'DIM_Dates','[Day Num In Month]','Day Num In Month','Integer','false',0,0,'false','false','true','false')  
declare @DIM_DatesDayNumOverallNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesDayNumOverallNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DatesDayNumOverallNewId,0,'DIM_Dates','[Day Num Overall]','Day Num Overall','Integer','false',0,0,'false','false','true','false')  
declare @DIM_DatesDayNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesDayNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DatesDayNameNewId,0,'DIM_Dates','[Day Name]','Day Name','Text','false',0,13,'false','false','true','false')  
declare @DIM_DatesDayAbbrevNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesDayAbbrevNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DatesDayAbbrevNewId,0,'DIM_Dates','[Day Abbrev]','Day Abbrev','Text','false',0,3,'false','false','true','false')  
declare @DIM_DatesWeekNumInYearNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesWeekNumInYearNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DatesWeekNumInYearNewId,0,'DIM_Dates','[Week Num In Year]','Week Num In Year','Integer','false',0,0,'false','false','true','false')  
declare @DIM_DatesWeekNumOverallNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesWeekNumOverallNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DatesWeekNumOverallNewId,0,'DIM_Dates','[Week Num Overall]','Week Num Overall','Integer','false',0,0,'false','false','true','false')  
declare @DIM_DatesMonthNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesMonthNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DatesMonthNewId,0,'DIM_Dates','[Month]','Month','Integer','false',0,0,'false','false','true','false')  
declare @DIM_DatesMonthNumOverallNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesMonthNumOverallNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DatesMonthNumOverallNewId,0,'DIM_Dates','[Month Num Overall]','Month Num Overall','Integer','false',0,0,'false','false','true','false')  
declare @DIM_DatesMonthNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesMonthNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DatesMonthNameNewId,0,'DIM_Dates','[Month Name]','Month Name','Text','false',0,13,'false','false','true','false')  
declare @DIM_DatesMonthAbbrevNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesMonthAbbrevNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DatesMonthAbbrevNewId,0,'DIM_Dates','[Month Abbrev]','Month Abbrev','Text','false',0,3,'false','false','true','false')  
declare @DIM_DatesQuarterNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesQuarterNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DatesQuarterNewId,0,'DIM_Dates','[Quarter]','Quarter','Integer','false',0,0,'false','false','true','false')  
declare @DIM_DatesYearNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesYearNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DatesYearNewId,0,'DIM_Dates','[Year]','Year','Integer','false',0,0,'false','false','true','false')  
declare @DIM_DatesYearmoNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesYearmoNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DatesYearmoNewId,0,'DIM_Dates','[Yearmo]','Yearmo','Integer','false',0,0,'false','false','true','false')  
declare @DIM_DatesMonthEndFlagNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesMonthEndFlagNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DatesMonthEndFlagNewId,0,'DIM_Dates','[Month End Flag]','Month End Flag','Text','false',0,100,'false','false','true','false')  
------------------------------------------------------------------------------------
declare @DIM_DepartmentsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed,DefaultFilterBy) Values(@DIM_DepartmentsNewId,0,'DIM_Departments','DIM_Departments','Dimension','false','[Name]')  
--Fields --
declare @DIM_DepartmentsId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DepartmentsId_NumberNewId,0,'DIM_Departments','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false')  
declare @DIM_DepartmentsIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DepartmentsIdNewId,0,'DIM_Departments','[Id]','Id','Text','true',0,15,'false','false','false','false')  
declare @DIM_DepartmentsNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DepartmentsNameNewId,0,'DIM_Departments','[Name]','Name','Text','true',0,40,'false','false','true','false')  
declare @DIM_DepartmentsLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DepartmentsLocalNameNewId,0,'DIM_Departments','[Local Name]','Local Name','nText','false',0,40,'false','false','true','false')  
declare @DIM_DepartmentsSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DepartmentsSourceTenantNewId,0,'DIM_Departments','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false')  
declare @DIM_DepartmentsParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_DepartmentsParentTenantNewId,0,'DIM_Departments','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_DirectionsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DirectionsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed,DefaultFilterBy) Values(@DIM_DirectionsNewId,0,'DIM_Directions','DIM_Directions','Dimension','true','[Name]')  
--Fields --
declare @DIM_DirectionsCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DirectionsCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_DirectionsCodeNewId,0,'DIM_Directions','[Code]','Code','Text','true',0,1,'false','false','true','[Name]','false')  
declare @DIM_DirectionsNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DirectionsNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_DirectionsNameNewId,0,'DIM_Directions','[Name]','Name','Text','true',0,40,'true','false','true','[Code]','false')  
------------------------------------------------------------------------------------
declare @DIM_IncotermsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed,DefaultFilterBy) Values(@DIM_IncotermsNewId,0,'DIM_Incoterms','DIM_Incoterms','Dimension','false','[Name]')  
--Fields --
declare @DIM_IncotermsId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_IncotermsId_NumberNewId,0,'DIM_Incoterms','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','[Code],[Name]','false')  
declare @DIM_IncotermsIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_IncotermsIdNewId,0,'DIM_Incoterms','[Id]','Id','Text','true',0,15,'false','false','false','false')  
declare @DIM_IncotermsNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_IncotermsNameNewId,0,'DIM_Incoterms','[Name]','Name','Text','true',0,40,'false','false','true','[Code]','false')  
declare @DIM_IncotermsLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_IncotermsLocalNameNewId,0,'DIM_Incoterms','[Local Name]','Local Name','nText','false',0,40,'false','false','true','false')  
declare @DIM_IncotermsCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_IncotermsCodeNewId,0,'DIM_Incoterms','[Code]','Code','Text','true',0,3,'false','false','true','[Name]','false')  
declare @DIM_IncotermsSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_IncotermsSourceTenantNewId,0,'DIM_Incoterms','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false')  
declare @DIM_IncotermsParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_IncotermsParentTenantNewId,0,'DIM_Incoterms','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_LevelsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_LevelsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed,DefaultFilterBy) Values(@DIM_LevelsNewId,0,'DIM_Levels','DIM_Levels','Dimension','true','[Name]')  
--Fields --
declare @DIM_LevelsCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_LevelsCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_LevelsCodeNewId,0,'DIM_Levels','[Code]','Code','Text','true',0,1,'false','false','true','[Name]','false')  
declare @DIM_LevelsNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_LevelsNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_LevelsNameNewId,0,'DIM_Levels','[Name]','Name','Text','true',0,40,'true','false','true','[Code]','false')  
------------------------------------------------------------------------------------
declare @DIM_MoveTypesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_MoveTypesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed) Values(@DIM_MoveTypesNewId,0,'DIM_MoveTypes','DIM_MoveTypes','Dimension','false')  
--Fields --
declare @DIM_MoveTypesId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_MoveTypesId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_MoveTypesId_NumberNewId,0,'DIM_MoveTypes','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','[Code],[Name]','false')  
declare @DIM_MoveTypesIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_MoveTypesIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_MoveTypesIdNewId,0,'DIM_MoveTypes','[Id]','Id','Text','true',0,15,'false','false','false','false')  
declare @DIM_MoveTypesEnglishNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_MoveTypesEnglishNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_MoveTypesEnglishNameNewId,0,'DIM_MoveTypes','[English Name]','English Name','Text','true',0,40,'false','false','true','false')  
declare @DIM_MoveTypesLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_MoveTypesLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_MoveTypesLocalNameNewId,0,'DIM_MoveTypes','[Local Name]','Local Name','Text','true',0,40,'false','false','true','false')  
declare @DIM_MoveTypesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_MoveTypesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_MoveTypesCodeNewId,0,'DIM_MoveTypes','[Code]','Code','Text','true',0,3,'false','false','true','false')  
declare @DIM_MoveTypesTransportModeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_MoveTypesTransportModeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_MoveTypesTransportModeNewId,0,'DIM_MoveTypes','[Transport Mode]','Transport Mode','Text','false',0,1,'false','false','true','false')  
declare @DIM_MoveTypesSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_MoveTypesSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_MoveTypesSourceTenantNewId,0,'DIM_MoveTypes','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false')  
declare @DIM_MoveTypesParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_MoveTypesParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_MoveTypesParentTenantNewId,0,'DIM_MoveTypes','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_PartnersNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed,DefaultFilterBy) Values(@DIM_PartnersNewId,0,'DIM_Partners','DIM_Partners','Dimension','false','[Name]')  
--Fields --
declare @DIM_PartnersId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PartnersId_NumberNewId,0,'DIM_Partners','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false')  
declare @DIM_PartnersIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PartnersIdNewId,0,'DIM_Partners','[Id]','Id','Text','true',0,15,'false','false','false','false')  
declare @DIM_PartnersNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_PartnersNameNewId,0,'DIM_Partners','[Name]','Name','Text','true',0,70,'false','false','true','[Code],[Partner Type]','false')  
declare @DIM_PartnersLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PartnersLocalNameNewId,0,'DIM_Partners','[Local Name]','Local Name','nText','false',0,100,'false','false','true','false')  
declare @DIM_PartnersCityNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersCityNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PartnersCityNewId,0,'DIM_Partners','[City]','City','nText','false',0,25,'false','false','true','false')  
declare @DIM_PartnersCountryNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersCountryNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PartnersCountryNewId,0,'DIM_Partners','[Country]','Country','Text','false',0,120,'false','false','true','false')  
declare @DIM_PartnersStateNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersStateNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PartnersStateNameNewId,0,'DIM_Partners','[State Name]','State Name','Text','false',0,40,'false','false','true','false')  
declare @DIM_PartnersZipCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersZipCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PartnersZipCodeNewId,0,'DIM_Partners','[Zip Code]','Zip Code','Text','false',0,15,'false','false','true','false')  
declare @DIM_PartnersPrimaryContactNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersPrimaryContactNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PartnersPrimaryContactNewId,0,'DIM_Partners','[Primary Contact]','Primary Contact','Text','false',0,60,'false','false','true','false')  
declare @DIM_PartnersAccountManagerNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersAccountManagerNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PartnersAccountManagerNewId,0,'DIM_Partners','[Account Manager]','Account Manager','Text','false',0,60,'false','false','true','false')  
declare @DIM_PartnersSalesmanNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersSalesmanNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PartnersSalesmanNewId,0,'DIM_Partners','[Salesman]','Salesman','Text','false',0,60,'false','false','true','false')  
declare @DIM_PartnersCustomerRankNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersCustomerRankNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PartnersCustomerRankNewId,0,'DIM_Partners','[Customer Rank]','Customer Rank','Text','false',0,40,'false','false','true','false')  
declare @DIM_PartnersPartnerTypeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersPartnerTypeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PartnersPartnerTypeNewId,0,'DIM_Partners','[Partner Type]','Partner Type','Text','true',0,20,'false','false','true','false')  
declare @DIM_PartnersSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PartnersSourceTenantNewId,0,'DIM_Partners','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false')  
declare @DIM_PartnersParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PartnersParentTenantNewId,0,'DIM_Partners','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false')  
declare @DIM_PartnersCountryCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersCountryCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PartnersCountryCodeNewId,0,'DIM_Partners','[Country Code]','Country Code','Text','false',0,2,'false','false','true','false')  
declare @DIM_PartnersPrimaryContactEmailNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersPrimaryContactEmailNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PartnersPrimaryContactEmailNewId,0,'DIM_Partners','[Primary Contact Email]','Primary Contact Email','Text','false',0,70,'false','false','true','false')  
declare @DIM_PartnersReceivablesAccountingCardNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersReceivablesAccountingCardNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PartnersReceivablesAccountingCardNewId,0,'DIM_Partners','[Receivables Accounting Card]','Receivables Accounting Card','Text','false',0,25,'false','false','true','false')  
declare @DIM_PartnersPayablesAccountingCardNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersPayablesAccountingCardNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PartnersPayablesAccountingCardNewId,0,'DIM_Partners','[Payables Accounting Card]','Payables Accounting Card','Text','false',0,25,'false','false','true','false')  
declare @DIM_PartnersCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_PartnersCodeNewId,0,'DIM_Partners','[Code]','Code','Text','true',0,15,'false','false','true','[Name],[Partner Type]','false')  
------------------------------------------------------------------------------------
declare @DIM_PortsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed,DefaultFilterBy) Values(@DIM_PortsNewId,0,'DIM_Ports','DIM_Ports','Dimension','false','[Name]')  
--Fields --
declare @DIM_PortsId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PortsId_NumberNewId,0,'DIM_Ports','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false')  
declare @DIM_PortsIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PortsIdNewId,0,'DIM_Ports','[Id]','Id','Text','true',0,15,'false','false','false','false')  
declare @DIM_PortsNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_PortsNameNewId,0,'DIM_Ports','[Name]','Name','Text','true',0,40,'false','false','true','[Code]','false')  
declare @DIM_PortsCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_PortsCodeNewId,0,'DIM_Ports','[Code]','Code','Text','true',0,3,'false','false','true','[Name]','false')  
declare @DIM_PortsLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PortsLocalNameNewId,0,'DIM_Ports','[Local Name]','Local Name','nText','false',0,40,'false','false','true','false')  
declare @DIM_PortsUNLocCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsUNLocCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PortsUNLocCodeNewId,0,'DIM_Ports','[UN Loc Code]','UN Loc Code','Text','false',0,30,'false','false','true','false')  
declare @DIM_PortsCountryNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsCountryNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PortsCountryNewId,0,'DIM_Ports','[Country]','Country','Text','true',0,120,'false','false','true','false')  
declare @DIM_PortsStateNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsStateNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PortsStateNameNewId,0,'DIM_Ports','[State Name]','State Name','Text','false',0,40,'false','false','true','false')  
declare @DIM_PortsSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PortsSourceTenantNewId,0,'DIM_Ports','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false')  
declare @DIM_PortsParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_PortsParentTenantNewId,0,'DIM_Ports','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_ShipmentStatusesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed,DefaultFilterBy) Values(@DIM_ShipmentStatusesNewId,0,'DIM_ShipmentStatuses','DIM_ShipmentStatuses','Dimension','false','[Name]')  
--Fields --
declare @DIM_ShipmentStatusesId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_ShipmentStatusesId_NumberNewId,0,'DIM_ShipmentStatuses','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false')  
declare @DIM_ShipmentStatusesIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_ShipmentStatusesIdNewId,0,'DIM_ShipmentStatuses','[Id]','Id','Text','true',0,15,'false','false','false','false')  
declare @DIM_ShipmentStatusesNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_ShipmentStatusesNameNewId,0,'DIM_ShipmentStatuses','[Name]','Name','Text','true',0,40,'false','false','true','[Code]','false')  
declare @DIM_ShipmentStatusesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_ShipmentStatusesCodeNewId,0,'DIM_ShipmentStatuses','[Code]','Code','Text','true',0,4,'false','false','true','[Name]','false')  
declare @DIM_ShipmentStatusesSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_ShipmentStatusesSourceTenantNewId,0,'DIM_ShipmentStatuses','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false')  
declare @DIM_ShipmentStatusesParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_ShipmentStatusesParentTenantNewId,0,'DIM_ShipmentStatuses','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_SpecialServicesTypesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_SpecialServicesTypesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed,DefaultFilterBy) Values(@DIM_SpecialServicesTypesNewId,0,'DIM_SpecialServicesTypes','DIM_SpecialServicesTypes','Dimension','false','[English Name]')  
--Fields --
declare @DIM_SpecialServicesTypesIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_SpecialServicesTypesIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_SpecialServicesTypesIdNewId,0,'DIM_SpecialServicesTypes','[Id]','Id','Text','true',0,15,'false','false','true','false')  
declare @DIM_SpecialServicesTypesId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_SpecialServicesTypesId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_SpecialServicesTypesId_NumberNewId,0,'DIM_SpecialServicesTypes','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false')  
declare @DIM_SpecialServicesTypesEnglishNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_SpecialServicesTypesEnglishNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_SpecialServicesTypesEnglishNameNewId,0,'DIM_SpecialServicesTypes','[English Name]','English Name','Text','true',0,100,'false','false','true','[Code]','false')  
declare @DIM_SpecialServicesTypesLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_SpecialServicesTypesLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_SpecialServicesTypesLocalNameNewId,0,'DIM_SpecialServicesTypes','[Local Name]','Local Name','nText','false',0,100,'false','false','true','false')  
declare @DIM_SpecialServicesTypesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_SpecialServicesTypesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_SpecialServicesTypesCodeNewId,0,'DIM_SpecialServicesTypes','[Code]','Code','Text','true',0,8,'false','false','true','[English Name]','false')  
declare @DIM_SpecialServicesTypesSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_SpecialServicesTypesSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_SpecialServicesTypesSourceTenantNewId,0,'DIM_SpecialServicesTypes','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false')  
declare @DIM_SpecialServicesTypesParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_SpecialServicesTypesParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_SpecialServicesTypesParentTenantNewId,0,'DIM_SpecialServicesTypes','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_TenantsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TenantsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed,DefaultFilterBy) Values(@DIM_TenantsNewId,0,'DIM_Tenants','DIM_Tenants','Dimension','false','[Tenant Name]')  
--Fields --
declare @DIM_TenantsTenantNumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TenantsTenantNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_TenantsTenantNumberNewId,0,'DIM_Tenants','[Tenant Number]','Tenant Number','Integer','true',0,0,'true','false','true','[Tenant Name]','false')  
declare @DIM_TenantsTenantNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TenantsTenantNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_TenantsTenantNameNewId,0,'DIM_Tenants','[Tenant Name]','Tenant Name','Text','true',0,100,'false','false','true','[Tenant Number]','false')  
declare @DIM_TenantsCountryNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TenantsCountryNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_TenantsCountryNewId,0,'DIM_Tenants','[Country]','Country','Text','false',0,120,'false','false','true','false')  
------------------------------------------------------------------------------------
declare @DIM_TransportModesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TransportModesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed,DefaultFilterBy) Values(@DIM_TransportModesNewId,0,'DIM_TransportModes','DIM_TransportModes','Dimension','true','[Name]')  
--Fields --
declare @DIM_TransportModesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TransportModesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_TransportModesCodeNewId,0,'DIM_TransportModes','[Code]','Code','Text','true',0,1,'false','false','true','[Name]','false')  
declare @DIM_TransportModesNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TransportModesNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_TransportModesNameNewId,0,'DIM_TransportModes','[Name]','Name','Text','true',0,13,'true','false','true','[Code]','false')  
------------------------------------------------------------------------------------
declare @DIM_TypesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TypesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed,DefaultFilterBy) Values(@DIM_TypesNewId,0,'DIM_Types','DIM_Types','Dimension','true','[Name]')  
--Fields --
declare @DIM_TypesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TypesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_TypesCodeNewId,0,'DIM_Types','[Code]','Code','Text','true',0,4,'false','false','true','[Name]','false')  
declare @DIM_TypesNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TypesNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_TypesNameNewId,0,'DIM_Types','[Name]','Name','Text','true',0,40,'true','false','true','[Code]','false')  
------------------------------------------------------------------------------------
declare @DIM_UsersNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed,DefaultFilterBy) Values(@DIM_UsersNewId,0,'DIM_Users','DIM_Users','Dimension','false','[Name]')  
--Fields --
declare @DIM_UsersId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_UsersId_NumberNewId,0,'DIM_Users','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false')  
declare @DIM_UsersIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_UsersIdNewId,0,'DIM_Users','[Id]','Id','Text','true',0,15,'false','false','false','false')  
declare @DIM_UsersNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_UsersNameNewId,0,'DIM_Users','[Name]','Name','Text','true',0,60,'false','false','true','[Email]','false')  
declare @DIM_UsersLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_UsersLocalNameNewId,0,'DIM_Users','[Local Name]','Local Name','nText','false',0,100,'false','false','true','false')  
declare @DIM_UsersEmailNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersEmailNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_UsersEmailNewId,0,'DIM_Users','[Email]','Email','Text','false',0,70,'false','false','true','[Name]','false')  
declare @DIM_UsersDepartmentNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersDepartmentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_UsersDepartmentNewId,0,'DIM_Users','[Department]','Department','Text','true',0,40,'false','false','true','false')  
declare @DIM_UsersBranchNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersBranchNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_UsersBranchNewId,0,'DIM_Users','[Branch]','Branch','Text','true',0,40,'false','false','true','false')  
declare @DIM_UsersSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_UsersSourceTenantNewId,0,'DIM_Users','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false')  
declare @DIM_UsersParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_UsersParentTenantNewId,0,'DIM_Users','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false')  
------------------------------------------------------------------------------------
declare @DIM_VesselsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed,DefaultFilterBy) Values(@DIM_VesselsNewId,0,'DIM_Vessels','DIM_Vessels','Dimension','false','[English Name]')  
--Fields --
declare @DIM_VesselsId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_VesselsId_NumberNewId,0,'DIM_Vessels','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false')  
declare @DIM_VesselsIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_VesselsIdNewId,0,'DIM_Vessels','[Id]','Id','Text','true',0,15,'false','false','false','false')  
declare @DIM_VesselsCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_VesselsCodeNewId,0,'DIM_Vessels','[Code]','Code','Text','false',0,5,'false','false','true','[English Name]','false')  
declare @DIM_VesselsEnglishNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsEnglishNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,LOVAdditionalColumns,HideTree) Values(@DIM_VesselsEnglishNameNewId,0,'DIM_Vessels','[English Name]','English Name','Text','true',0,40,'false','false','true','[Code]','false')  
declare @DIM_VesselsLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_VesselsLocalNameNewId,0,'DIM_Vessels','[Local Name]','Local Name','nText','false',0,40,'false','false','true','false')  
declare @DIM_VesselsNotesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsNotesNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_VesselsNotesNewId,0,'DIM_Vessels','[Notes]','Notes','Text','false',0,250,'false','false','true','false')  
declare @DIM_VesselsIMOCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsIMOCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_VesselsIMOCodeNewId,0,'DIM_Vessels','[IMO Code]','IMO Code','Text','false',0,10,'false','false','true','false')  
declare @DIM_VesselsParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_VesselsParentTenantNewId,0,'DIM_Vessels','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false','false','false')  
declare @DIM_VesselsSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_VesselsSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@DIM_VesselsSourceTenantNewId,0,'DIM_Vessels','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false','false','false')  
------------------------------------------------------------------------------------
declare @Fact_ShipmentsNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed) Values(@Fact_ShipmentsNewId,0,'Fact_Shipments','Fact_Shipments','Fact','false')  
--Fields --
declare @Fact_ShipmentsId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@Fact_ShipmentsId_NumberNewId,0,'Fact_Shipments','[Id_Number]','Id_Number','Integer','true',0,0,'true','false','false','false')  
declare @Fact_ShipmentsIdNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,HideTree) Values(@Fact_ShipmentsIdNewId,0,'Fact_Shipments','[Id]','Id','Text','true',0,15,'false','false','false','false')  
declare @Fact_ShipmentsSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsSourceTenantNewId,0,'Fact_Shipments','[Source Tenant]','Source Tenant','Dimension','false',0,0,'DIM_Tenants','false','false','true','General','false')  
declare @Fact_ShipmentsParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsParentTenantNewId,0,'Fact_Shipments','[Parent Tenant]','Parent Tenant','Dimension','false',0,0,'DIM_Tenants','false','false','false','General','false')  
declare @Fact_ShipmentsDirectionNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsDirectionNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsDirectionNewId,0,'Fact_Shipments','[Direction]','Direction','Dimension','true',0,40,'DIM_Directions','false','false','true','General','true')  
declare @Fact_ShipmentsTransportModeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransportModeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsTransportModeNewId,0,'Fact_Shipments','[Transport Mode]','Transport Mode','Dimension','true',0,13,'DIM_TransportModes','false','false','true','General','true')  
declare @Fact_ShipmentsLevelNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsLevelNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsLevelNewId,0,'Fact_Shipments','[Level]','Level','Dimension','true',0,40,'DIM_Levels','false','false','true','General','true')  
declare @Fact_ShipmentsTypeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTypeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsTypeNewId,0,'Fact_Shipments','[Type]','Type','Dimension','true',0,40,'DIM_Types','false','false','true','General','true')  
declare @Fact_ShipmentsDepartmentNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsDepartmentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsDepartmentNewId,0,'Fact_Shipments','[Department]','Department','Dimension','true',0,0,'DIM_Departments','false','false','true','General','false')  
declare @Fact_ShipmentsBranchNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsBranchNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsBranchNewId,0,'Fact_Shipments','[Branch]','Branch','Dimension','true',0,0,'DIM_Branches','false','false','true','General','false')  
declare @Fact_ShipmentsShipmentNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsShipmentNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsShipmentNumberNewId,0,'Fact_Shipments','[Shipment Number]','Shipment Number','Text','true',0,20,'false','false','true','General','References','false')  
declare @Fact_ShipmentsHouseNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsHouseNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsHouseNewId,0,'Fact_Shipments','[House]','House','Text','false',0,20,'false','false','true','General','References','false')  
declare @Fact_ShipmentsMasterNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsMasterNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsMasterNewId,0,'Fact_Shipments','[Master]','Master','Text','false',0,30,'false','false','true','General','References','false')  
declare @Fact_ShipmentsShipperNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsShipperNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsShipperNewId,0,'Fact_Shipments','[Shipper]','Shipper','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false')  
declare @Fact_ShipmentsConsigneeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsConsigneeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsConsigneeNewId,0,'Fact_Shipments','[Consignee]','Consignee','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false')  
declare @Fact_ShipmentsAgentNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAgentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsAgentNewId,0,'Fact_Shipments','[Agent]','Agent','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false')  
declare @Fact_ShipmentsCustomerNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCustomerNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsCustomerNewId,0,'Fact_Shipments','[Customer]','Customer','Dimension','false',0,0,'DIM_Partners','false','false','true','Partners','false')  
declare @Fact_ShipmentsIncotermNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsIncotermNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsIncotermNewId,0,'Fact_Shipments','[Incoterm]','Incoterm','Dimension','false',0,0,'DIM_Incoterms','false','false','true','Packages','General','false')  
declare @Fact_ShipmentsGrossWeightKGNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsGrossWeightKGNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsGrossWeightKGNewId,0,'Fact_Shipments','[Gross Weight (KG)]','Gross Weight (KG)','Decimal','false',0,15,'false','true','SUM','true','Packages','false')  
declare @Fact_ShipmentsChargeableWeightKGNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsChargeableWeightKGNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsChargeableWeightKGNewId,0,'Fact_Shipments','[Chargeable Weight (KG)]','Chargeable Weight (KG)','Decimal','false',0,40,'false','true','SUM','true','Packages','false')  
declare @Fact_ShipmentsTotalVolumeCBMNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTotalVolumeCBMNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsTotalVolumeCBMNewId,0,'Fact_Shipments','[Total Volume (CBM)]','Total Volume (CBM)','Decimal','false',0,0,'false','true','SUM','true','Packages','false')  
declare @Fact_ShipmentsNumberofPackagesNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsNumberofPackagesNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsNumberofPackagesNewId,0,'Fact_Shipments','[Number of Packages]','Number of Packages','Integer','false',0,0,'false','true','SUM','true','Packages','false')  
declare @Fact_ShipmentsNumberofContainersNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsNumberofContainersNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsNumberofContainersNewId,0,'Fact_Shipments','[Number of Containers]','Number of Containers','Integer','false',0,0,'false','true','SUM','true','Packages','false')  
declare @Fact_ShipmentsSalesmanNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsSalesmanNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsSalesmanNewId,0,'Fact_Shipments','[Salesman]','Salesman','Dimension','false',0,0,'DIM_Users','false','false','true','General','Operational','false')  
declare @Fact_ShipmentsAccountManagerNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAccountManagerNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsAccountManagerNewId,0,'Fact_Shipments','[Account Manager]','Account Manager','Dimension','false',0,0,'DIM_Users','false','false','true','General','Operational','false')  
declare @Fact_ShipmentsProfitLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsProfitLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsProfitLocalNewId,0,'Fact_Shipments','[Profit ( Local )]','Profit ( Local )','Decimal','false',0,0,'false','true','SUM','true','Money','false')  
declare @Fact_ShipmentsProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsProfitNewId,0,'Fact_Shipments','[Profit]','Profit','Decimal','false',0,0,'false','true','SUM','true','Money','false')  
declare @Fact_ShipmentsLocalCurrencyNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsLocalCurrencyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsLocalCurrencyNewId,0,'Fact_Shipments','[Local Currency ]','Local Currency ','Dimension','false',0,0,'DIM_Currencies','false','false','true','Money','false')  
declare @Fact_ShipmentsProfitCurrencyNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsProfitCurrencyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsProfitCurrencyNewId,0,'Fact_Shipments','[Profit Currency]','Profit Currency','Dimension','false',0,0,'DIM_Currencies','false','false','true','Money','false')  
declare @Fact_ShipmentsOperationallyClosedNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOperationallyClosedNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsOperationallyClosedNewId,0,'Fact_Shipments','[Operationally Closed]','Operationally Closed','Boolean','false',0,0,'false','false','true','Operational','false')  
declare @Fact_ShipmentsAccountingClosedNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAccountingClosedNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsAccountingClosedNewId,0,'Fact_Shipments','[Accounting Closed]','Accounting Closed','Boolean','false',0,0,'false','false','true','Operational','false')  
declare @Fact_ShipmentsStatusNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsStatusNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsStatusNewId,0,'Fact_Shipments','[Status]','Status','Dimension','false',0,0,'DIM_ShipmentStatuses','false','false','true','Operational','General','false')  
declare @Fact_ShipmentsLocationNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsLocationNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsLocationNewId,0,'Fact_Shipments','[Location]','Location','nText','false',0,40,'false','false','true','Operational','false')  
declare @Fact_ShipmentsOriginNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOriginNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsOriginNewId,0,'Fact_Shipments','[Origin]','Origin','Dimension','false',0,0,'DIM_Ports','false','false','true','Operational','false')  
declare @Fact_ShipmentsFinalDestinationNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFinalDestinationNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsFinalDestinationNewId,0,'Fact_Shipments','[Final Destination]','Final Destination','Dimension','false',0,0,'DIM_Ports','false','false','true','Operational','false')  
declare @Fact_ShipmentsIsDepartedNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsIsDepartedNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsIsDepartedNewId,0,'Fact_Shipments','[Is Departed]','Is Departed','Boolean','false',0,0,'false','false','true','Operational','false')  
declare @Fact_ShipmentsDepartedDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsDepartedDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsDepartedDateNewId,0,'Fact_Shipments','[Departed Date]','Departed Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Dates','Operational','false')  
declare @Fact_ShipmentsIsArrivedNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsIsArrivedNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsIsArrivedNewId,0,'Fact_Shipments','[Is Arrived]','Is Arrived','Boolean','false',0,0,'false','false','true','Operational','false')  
declare @Fact_ShipmentsArrivedDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsArrivedDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsArrivedDateNewId,0,'Fact_Shipments','[Arrived Date]','Arrived Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Dates','Operational','false')  
declare @Fact_ShipmentsIsCustomsClearedNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsIsCustomsClearedNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsIsCustomsClearedNewId,0,'Fact_Shipments','[Is Customs Cleared]','Is Customs Cleared','Boolean','false',0,0,'false','false','true','Operational','false')  
declare @Fact_ShipmentsCustomsClearenceDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCustomsClearenceDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsCustomsClearenceDateNewId,0,'Fact_Shipments','[Customs Clearence Date]','Customs Clearence Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Dates','Operational','false')  
declare @Fact_ShipmentsTotalShipmentsNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTotalShipmentsNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsTotalShipmentsNewId,0,'Fact_Shipments','[Total Shipments]','Total Shipments','Integer','false',0,0,'false','true','COUNT','true','General','false')  
declare @Fact_ShipmentsCreateDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCreateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsCreateDateNewId,0,'Fact_Shipments','[Create Date]','Create Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Dates','false')  
declare @Fact_ShipmentsLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsLastUpdateDateNewId,0,'Fact_Shipments','[Last Update Date]','Last Update Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Dates','Operational','false')  
declare @Fact_ShipmentsOperationalDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOperationalDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsOperationalDateNewId,0,'Fact_Shipments','[Operational Date]','Operational Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Dates','Operational','false')  
declare @Fact_ShipmentsOperationalCloseDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOperationalCloseDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsOperationalCloseDateNewId,0,'Fact_Shipments','[Operational Close Date]','Operational Close Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Dates','Operational','false')  
declare @Fact_ShipmentsAccountingCloseDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAccountingCloseDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsAccountingCloseDateNewId,0,'Fact_Shipments','[Accounting Close Date]','Accounting Close Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Dates','Operational','false')  
declare @Fact_ShipmentsOpenReceivablesLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOpenReceivablesLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsOpenReceivablesLocalNewId,0,'Fact_Shipments','[Open Receivables ( Local )]','Open Receivables ( Local )','Decimal','false',0,0,'false','true','SUM','true','Money','false')  
declare @Fact_ShipmentsOpenReceivablesProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOpenReceivablesProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsOpenReceivablesProfitNewId,0,'Fact_Shipments','[Open Receivables ( Profit )]','Open Receivables ( Profit )','Decimal','false',0,0,'false','true','SUM','true','Money','false')  
declare @Fact_ShipmentsAccountedReceivablesLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAccountedReceivablesLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsAccountedReceivablesLocalNewId,0,'Fact_Shipments','[Accounted Receivables ( Local )]','Accounted Receivables ( Local )','Decimal','false',0,0,'false','true','SUM','true','Money','false')  
declare @Fact_ShipmentsAccountedReceivablesProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAccountedReceivablesProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsAccountedReceivablesProfitNewId,0,'Fact_Shipments','[Accounted Receivables ( Profit )]','Accounted Receivables ( Profit )','Decimal','false',0,0,'false','true','SUM','true','Money','false')  
declare @Fact_ShipmentsOpenPayablesLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOpenPayablesLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsOpenPayablesLocalNewId,0,'Fact_Shipments','[Open Payables ( Local )]','Open Payables ( Local )','Decimal','false',0,0,'false','true','SUM','true','Money','false')  
declare @Fact_ShipmentsOpenPayablesProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOpenPayablesProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsOpenPayablesProfitNewId,0,'Fact_Shipments','[Open Payables ( Profit )]','Open Payables ( Profit )','Decimal','false',0,0,'false','true','SUM','true','Money','false')  
declare @Fact_ShipmentsAccountedPayablesLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAccountedPayablesLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsAccountedPayablesLocalNewId,0,'Fact_Shipments','[Accounted Payables ( Local )]','Accounted Payables ( Local )','Decimal','false',0,0,'false','true','SUM','true','Money','false')  
declare @Fact_ShipmentsAccountedPayablesProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAccountedPayablesProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsAccountedPayablesProfitNewId,0,'Fact_Shipments','[Accounted Payables ( Profit )]','Accounted Payables ( Profit )','Decimal','false',0,0,'false','true','SUM','true','Money','false')  
declare @Fact_ShipmentsAgentRef1NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAgentRef1NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsAgentRef1NewId,0,'Fact_Shipments','[Agent Ref1]','Agent Ref1','Text','false',0,50,'false','false','true','References','false')  
declare @Fact_ShipmentsAgentRef2NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAgentRef2NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsAgentRef2NewId,0,'Fact_Shipments','[Agent Ref2]','Agent Ref2','Text','false',0,50,'false','false','true','References','false')  
declare @Fact_ShipmentsAMSBLNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAMSBLNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsAMSBLNewId,0,'Fact_Shipments','[AMS BL]','AMS BL','Text','false',0,17,'false','false','true','References','false')  
declare @Fact_ShipmentsConsigneeRef1NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsConsigneeRef1NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsConsigneeRef1NewId,0,'Fact_Shipments','[Consignee Ref1]','Consignee Ref1','Text','false',0,50,'false','false','true','References','false')  
declare @Fact_ShipmentsConsigneeRef2NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsConsigneeRef2NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsConsigneeRef2NewId,0,'Fact_Shipments','[Consignee Ref2]','Consignee Ref2','Text','false',0,50,'false','false','true','References','false')  
declare @Fact_ShipmentsCreatedByNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCreatedByNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsCreatedByNewId,0,'Fact_Shipments','[Created By]','Created By','Dimension','false',0,15,'DIM_Users','false','false','true','Operational','false')  
declare @Fact_ShipmentsCustomAgentNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCustomAgentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsCustomAgentNewId,0,'Fact_Shipments','[Custom Agent]','Custom Agent','Dimension','false',0,15,'DIM_Partners','false','false','true','Partners','false')  
declare @Fact_ShipmentsCustomerRef1NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCustomerRef1NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsCustomerRef1NewId,0,'Fact_Shipments','[Customer Ref1]','Customer Ref1','Text','false',0,50,'false','false','true','References','false')  
declare @Fact_ShipmentsCustomerRef2NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCustomerRef2NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category2,HideTree) Values(@Fact_ShipmentsCustomerRef2NewId,0,'Fact_Shipments','[Customer Ref2]','Customer Ref2','Text','false',0,50,'false','false','true','References','false')  
declare @Fact_ShipmentsFirstPickupDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFirstPickupDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsFirstPickupDateNewId,0,'Fact_Shipments','[First Pickup Date]','First Pickup Date','Dimension','false',0,15,'DIM_Dates','false','false','true','Dates','Operational','false')  
declare @Fact_ShipmentsFreightPCNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFreightPCNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsFreightPCNewId,0,'Fact_Shipments','[Freight PC]','Freight PC','Text','true',0,1,'false','false','true','General','false')  
declare @Fact_ShipmentsCarrierDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCarrierDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsCarrierDateNewId,0,'Fact_Shipments','[Carrier Date ]','Carrier Date ','Dimension','false',0,15,'DIM_Dates','false','false','true','Dates','false')  
declare @Fact_ShipmentsCarrierNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCarrierNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsCarrierNewId,0,'Fact_Shipments','[Carrier]','Carrier','Dimension','false',0,15,'DIM_Partners','false','false','true','Partners','false')  
declare @Fact_ShipmentsCarrierNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCarrierNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsCarrierNumberNewId,0,'Fact_Shipments','[Carrier Number]','Carrier Number','Text','false',0,15,'false','false','true','References','false')  
declare @Fact_ShipmentsMainHarmonizeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsMainHarmonizeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsMainHarmonizeNewId,0,'Fact_Shipments','[Main Harmonize]','Main Harmonize','Text','false',0,18,'false','false','true','General','false')  
declare @Fact_ShipmentsOtherChargePCNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOtherChargePCNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsOtherChargePCNewId,0,'Fact_Shipments','[Other Charge PC]','Other Charge PC','Text','true',0,1,'false','false','true','General','false')  
declare @Fact_ShipmentsProject#NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsProject#NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsProject#NewId,0,'Fact_Shipments','[Project#]','Project#','Text','false',0,100,'false','false','true','References','false')  
declare @Fact_ShipmentsShipperRef1NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsShipperRef1NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsShipperRef1NewId,0,'Fact_Shipments','[Shipper Ref1]','Shipper Ref1','Text','false',0,50,'false','false','true','References','false')  
declare @Fact_ShipmentsShipperRef2NewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsShipperRef2NewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsShipperRef2NewId,0,'Fact_Shipments','[Shipper Ref2]','Shipper Ref2','Text','false',0,50,'false','false','true','References','false')  
declare @Fact_ShipmentsTEUNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTEUNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsTEUNewId,0,'Fact_Shipments','[TEU]','TEU','Double','false',0,15,'false','false','true','Packages','false')  
declare @Fact_ShipmentsValueofGoodsNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsValueofGoodsNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsValueofGoodsNewId,0,'Fact_Shipments','[Value of Goods]','Value of Goods','Double','false',0,15,'false','false','true','Money','false')  
declare @Fact_ShipmentsValueofGoodsCurrencyNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsValueofGoodsCurrencyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsValueofGoodsCurrencyNewId,0,'Fact_Shipments','[Value of Goods Currency]','Value of Goods Currency','Dimension','false',0,15,'DIM_Currencies','false','false','true','Money','false')  
declare @Fact_ShipmentsWarehouseTerminalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsWarehouseTerminalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsWarehouseTerminalNewId,0,'Fact_Shipments','[Warehouse Terminal]','Warehouse Terminal','Dimension','false',0,15,'DIM_Partners','false','false','true','Partners','false')  
declare @Fact_ShipmentsForwarderNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsForwarderNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsForwarderNewId,0,'Fact_Shipments','[Forwarder]','Forwarder','Dimension','false',0,15,'DIM_Partners','false','false','true','Partners','false')  
declare @Fact_ShipmentsBookingConfirmationNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsBookingConfirmationNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsBookingConfirmationNumberNewId,0,'Fact_Shipments','[Booking Confirmation Number]','Booking Confirmation Number','Text','false',0,25,'false','false','true','Operational','References ','false')  
declare @Fact_ShipmentsMainCarriageATANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsMainCarriageATANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsMainCarriageATANewId,0,'Fact_Shipments','[Main Carriage ATA]','Main Carriage ATA','Dimension','false',0,0,'DIM_Dates','false','false','true','Operational','Dates','false')  
declare @Fact_ShipmentsMasterDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsMasterDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsMasterDateNewId,0,'Fact_Shipments','[Master Date]','Master Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Operational','Dates','false')  
declare @Fact_ShipmentsStatusDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsStatusDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsStatusDateNewId,0,'Fact_Shipments','[Status Date]','Status Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Operational','Dates','false')  
declare @Fact_ShipmentsCustomsDeclarationNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCustomsDeclarationNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsCustomsDeclarationNumberNewId,0,'Fact_Shipments','[Customs Declaration Number]','Customs Declaration Number','Text','false',0,35,'false','false','true','References ','false')  
declare @Fact_ShipmentsFirstOperationalCloseDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFirstOperationalCloseDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsFirstOperationalCloseDateNewId,0,'Fact_Shipments','[First Operational Close Date]','First Operational Close Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Operational','Dates','false')  
declare @Fact_ShipmentsEstimatedFinalArrivalDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsEstimatedFinalArrivalDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsEstimatedFinalArrivalDateNewId,0,'Fact_Shipments','[Estimated Final Arrival Date]','Estimated Final Arrival Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Operational','Dates','false')  
declare @Fact_ShipmentsActualFinalArrivalDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsActualFinalArrivalDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsActualFinalArrivalDateNewId,0,'Fact_Shipments','[Actual Final Arrival Date]','Actual Final Arrival Date','Dimension','false',0,0,'DIM_Dates','false','false','true','Operational','Dates','false')  
declare @Fact_ShipmentsRoutingNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsRoutingNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsRoutingNewId,0,'Fact_Shipments','[Routing]','Routing','Text','false',0,100,'false','false','true','Operational','false')  
declare @Fact_ShipmentsDescriptionOfGoodsNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsDescriptionOfGoodsNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsDescriptionOfGoodsNewId,0,'Fact_Shipments','[Description Of Goods]','Description Of Goods','Text','false',0,2000,'false','false','true','Operational','false')  
declare @Fact_ShipmentsPreCarriageETDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsPreCarriageETDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsPreCarriageETDNewId,0,'Fact_Shipments','[Pre Carriage ETD]','Pre Carriage ETD','Dimension','false',0,0,'DIM_Dates','false','false','true','Operational','Dates','false')  
declare @Fact_ShipmentsMainCarriageETANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsMainCarriageETANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsMainCarriageETANewId,0,'Fact_Shipments','[Main Carriage ETA]','Main Carriage ETA','Dimension','false',0,0,'DIM_Dates','false','false','true','Operational','Dates','false')  
declare @Fact_ShipmentsMainCarriageETDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsMainCarriageETDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsMainCarriageETDNewId,0,'Fact_Shipments','[Main Carriage ETD]','Main Carriage ETD','Dimension','false',0,0,'DIM_Dates','false','false','true','Operational','Dates','false')  
declare @Fact_ShipmentsMoveTypeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsMoveTypeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsMoveTypeNewId,0,'Fact_Shipments','[Move Type]','Move Type','Dimension','true',0,0,'DIM_MoveTypes','false','false','true','Operational','false')  
declare @Fact_ShipmentsVesselNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsVesselNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsVesselNewId,0,'Fact_Shipments','[Vessel]','Vessel','Dimension','true',0,0,'DIM_Vessels','false','false','true','Operational','false')  
declare @Fact_ShipmentsSpecialServicesNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsSpecialServicesNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,HideTree) Values(@Fact_ShipmentsSpecialServicesNewId,0,'Fact_Shipments','[Special Services]','Special Services','Dimension','true',0,0,'DIM_SpecialServicesTypes','false','false','true','Operational','false')  
declare @Fact_ShipmentsFirstPickupETDNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFirstPickupETDNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsFirstPickupETDNewId,0,'Fact_Shipments','[First Pickup ETD]','First Pickup ETD','Dimension','true',0,0,'DIM_Dates','false','false','true','Operational','Dates','false')  
declare @Fact_ShipmentsFirstPickupETANewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFirstPickupETANewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement,DisplayInQueryBuilder,Category1,Category2,HideTree) Values(@Fact_ShipmentsFirstPickupETANewId,0,'Fact_Shipments','[First Pickup ETA]','First Pickup ETA','Dimension','true',0,0,'DIM_Dates','false','false','true','Operational ','Dates','false')  
