 
  

-- this script is generated
delete from DWObjectFields
delete from DWObjectTables
------------------------------------------------------------------------------------
declare @DIM_BranchesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed) Values(@DIM_BranchesNewId,0,'DIM_Branches','DIM_Branches','Dimension','false')  
--Fields --
declare @DIM_BranchesId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_BranchesId_NumberNewId,0,'DIM_Branches','[Id_Number]','Id_Number','Integer','true',0,0,'true','false')  
declare @DIM_BranchesIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_BranchesIdNewId,0,'DIM_Branches','[Id]','Id','Text','true',0,15,'false','false')  
declare @DIM_BranchesNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_BranchesNameNewId,0,'DIM_Branches','[Name]','Name','Text','true',0,40,'false','false')  
declare @DIM_BranchesLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_BranchesLocalNameNewId,0,'DIM_Branches','[Local Name]','Local Name','nText','false',0,40,'false','false')  
declare @DIM_BranchesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_BranchesCodeNewId,0,'DIM_Branches','[Code]','Code','Text','true',0,10,'true','false')  
declare @DIM_BranchesSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_BranchesSourceTenantNewId,0,'DIM_Branches','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false')  
declare @DIM_BranchesParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_BranchesParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_BranchesParentTenantNewId,0,'DIM_Branches','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false')  
------------------------------------------------------------------------------------
declare @DIM_CurrenciesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed) Values(@DIM_CurrenciesNewId,0,'DIM_Currencies','DIM_Currencies','Dimension','false')  
--Fields --
declare @DIM_CurrenciesId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_CurrenciesId_NumberNewId,0,'DIM_Currencies','[Id_Number]','Id_Number','Integer','true',0,0,'true','false')  
declare @DIM_CurrenciesIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_CurrenciesIdNewId,0,'DIM_Currencies','[Id]','Id','Text','true',0,15,'false','false')  
declare @DIM_CurrenciesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_CurrenciesCodeNewId,0,'DIM_Currencies','[Code]','Code','Text','true',0,3,'false','false')  
declare @DIM_CurrenciesNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_CurrenciesNameNewId,0,'DIM_Currencies','[Name]','Name','Text','true',0,40,'false','false')  
declare @DIM_CurrenciesLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_CurrenciesLocalNameNewId,0,'DIM_Currencies','[LocalName]','LocalName','nText','false',0,40,'false','false')  
declare @DIM_CurrenciesCurrencySignNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesCurrencySignNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_CurrenciesCurrencySignNewId,0,'DIM_Currencies','[Currency Sign]','Currency Sign','nText','true',0,3,'false','false')  
declare @DIM_CurrenciesSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_CurrenciesSourceTenantNewId,0,'DIM_Currencies','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false')  
declare @DIM_CurrenciesParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_CurrenciesParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_CurrenciesParentTenantNewId,0,'DIM_Currencies','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false')  
------------------------------------------------------------------------------------
declare @DIM_DatesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed) Values(@DIM_DatesNewId,0,'DIM_Dates','DIM_Dates','Dimension','false')  
--Fields --
declare @DIM_DatesDateKeyNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesDateKeyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DatesDateKeyNewId,0,'DIM_Dates','[Date Key]','Date Key','Integer','true',0,0,'false','false')  
declare @DIM_DatesFullDateNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesFullDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DatesFullDateNewId,0,'DIM_Dates','[Full Date]','Full Date','DateTime','false',0,0,'false','false')  
declare @DIM_DatesFullDateUSNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesFullDateUSNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DatesFullDateUSNewId,0,'DIM_Dates','[Full Date US]','Full Date US','Text','false',0,100,'false','false')  
declare @DIM_DatesDayOfWeekNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesDayOfWeekNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DatesDayOfWeekNewId,0,'DIM_Dates','[Day Of Week]','Day Of Week','Integer','false',0,0,'false','false')  
declare @DIM_DatesDayNumInMonthNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesDayNumInMonthNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DatesDayNumInMonthNewId,0,'DIM_Dates','[Day Num In Month]','Day Num In Month','Integer','false',0,0,'false','false')  
declare @DIM_DatesDayNumOverallNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesDayNumOverallNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DatesDayNumOverallNewId,0,'DIM_Dates','[Day Num Overall]','Day Num Overall','Integer','false',0,0,'false','false')  
declare @DIM_DatesDayNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesDayNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DatesDayNameNewId,0,'DIM_Dates','[Day Name]','Day Name','Text','false',0,13,'false','false')  
declare @DIM_DatesDayAbbrevNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesDayAbbrevNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DatesDayAbbrevNewId,0,'DIM_Dates','[Day Abbrev]','Day Abbrev','Text','false',0,3,'false','false')  
declare @DIM_DatesWeekNumInYearNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesWeekNumInYearNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DatesWeekNumInYearNewId,0,'DIM_Dates','[Week Num In Year]','Week Num In Year','Integer','false',0,0,'false','false')  
declare @DIM_DatesWeekNumOverallNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesWeekNumOverallNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DatesWeekNumOverallNewId,0,'DIM_Dates','[Week Num Overall]','Week Num Overall','Integer','false',0,0,'false','false')  
declare @DIM_DatesMonthNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesMonthNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DatesMonthNewId,0,'DIM_Dates','[Month]','Month','Integer','false',0,0,'false','false')  
declare @DIM_DatesMonthNumOverallNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesMonthNumOverallNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DatesMonthNumOverallNewId,0,'DIM_Dates','[Month Num Overall]','Month Num Overall','Integer','false',0,0,'false','false')  
declare @DIM_DatesMonthNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesMonthNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DatesMonthNameNewId,0,'DIM_Dates','[Month Name]','Month Name','Text','false',0,13,'false','false')  
declare @DIM_DatesMonthAbbrevNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesMonthAbbrevNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DatesMonthAbbrevNewId,0,'DIM_Dates','[Month Abbrev]','Month Abbrev','Text','false',0,3,'false','false')  
declare @DIM_DatesQuarterNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesQuarterNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DatesQuarterNewId,0,'DIM_Dates','[Quarter]','Quarter','Integer','false',0,0,'false','false')  
declare @DIM_DatesYearNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesYearNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DatesYearNewId,0,'DIM_Dates','[Year]','Year','Integer','false',0,0,'false','false')  
declare @DIM_DatesYearmoNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesYearmoNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DatesYearmoNewId,0,'DIM_Dates','[Yearmo]','Yearmo','Integer','false',0,0,'false','false')  
declare @DIM_DatesMonthEndFlagNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DatesMonthEndFlagNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DatesMonthEndFlagNewId,0,'DIM_Dates','[Month End Flag]','Month End Flag','Text','false',0,100,'false','false')  
------------------------------------------------------------------------------------
declare @DIM_DepartmentsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed) Values(@DIM_DepartmentsNewId,0,'DIM_Departments','DIM_Departments','Dimension','false')  
--Fields --
declare @DIM_DepartmentsId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DepartmentsId_NumberNewId,0,'DIM_Departments','[Id_Number]','Id_Number','Integer','true',0,0,'true','false')  
declare @DIM_DepartmentsIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DepartmentsIdNewId,0,'DIM_Departments','[Id]','Id','Text','true',0,15,'false','false')  
declare @DIM_DepartmentsNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DepartmentsNameNewId,0,'DIM_Departments','[Name]','Name','Text','true',0,40,'false','false')  
declare @DIM_DepartmentsLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DepartmentsLocalNameNewId,0,'DIM_Departments','[Local Name]','Local Name','nText','false',0,40,'false','false')  
declare @DIM_DepartmentsSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DepartmentsSourceTenantNewId,0,'DIM_Departments','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false')  
declare @DIM_DepartmentsParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DepartmentsParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DepartmentsParentTenantNewId,0,'DIM_Departments','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false')  
------------------------------------------------------------------------------------
declare @DIM_DirectionsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DirectionsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed) Values(@DIM_DirectionsNewId,0,'DIM_Directions','DIM_Directions','Dimension','true')  
--Fields --
declare @DIM_DirectionsCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DirectionsCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DirectionsCodeNewId,0,'DIM_Directions','[Code]','Code','Text','true',0,1,'true','false')  
declare @DIM_DirectionsNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_DirectionsNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_DirectionsNameNewId,0,'DIM_Directions','[Name]','Name','Text','true',0,40,'false','false')  
------------------------------------------------------------------------------------
declare @DIM_IncotermsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed) Values(@DIM_IncotermsNewId,0,'DIM_Incoterms','DIM_Incoterms','Dimension','false')  
--Fields --
declare @DIM_IncotermsId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_IncotermsId_NumberNewId,0,'DIM_Incoterms','[Id_Number]','Id_Number','Integer','true',0,0,'true','false')  
declare @DIM_IncotermsIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_IncotermsIdNewId,0,'DIM_Incoterms','[Id]','Id','Text','true',0,15,'false','false')  
declare @DIM_IncotermsNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_IncotermsNameNewId,0,'DIM_Incoterms','[Name]','Name','Text','true',0,40,'false','false')  
declare @DIM_IncotermsLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_IncotermsLocalNameNewId,0,'DIM_Incoterms','[LocalName]','LocalName','nText','false',0,40,'false','false')  
declare @DIM_IncotermsCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_IncotermsCodeNewId,0,'DIM_Incoterms','[Code]','Code','Text','true',0,3,'false','false')  
declare @DIM_IncotermsSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_IncotermsSourceTenantNewId,0,'DIM_Incoterms','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false')  
declare @DIM_IncotermsParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_IncotermsParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_IncotermsParentTenantNewId,0,'DIM_Incoterms','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false')  
------------------------------------------------------------------------------------
declare @DIM_LevelsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_LevelsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed) Values(@DIM_LevelsNewId,0,'DIM_Levels','DIM_Levels','Dimension','true')  
--Fields --
declare @DIM_LevelsCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_LevelsCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_LevelsCodeNewId,0,'DIM_Levels','[Code]','Code','Text','true',0,1,'true','false')  
declare @DIM_LevelsNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_LevelsNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_LevelsNameNewId,0,'DIM_Levels','[Name]','Name','Text','true',0,40,'false','false')  
------------------------------------------------------------------------------------
declare @DIM_PartnersNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed) Values(@DIM_PartnersNewId,0,'DIM_Partners','DIM_Partners','Dimension','false')  
--Fields --
declare @DIM_PartnersId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PartnersId_NumberNewId,0,'DIM_Partners','[Id_Number]','Id_Number','Integer','true',0,0,'true','false')  
declare @DIM_PartnersIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PartnersIdNewId,0,'DIM_Partners','[Id]','Id','Text','true',0,15,'false','false')  
declare @DIM_PartnersNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PartnersNameNewId,0,'DIM_Partners','[Name]','Name','Text','true',0,40,'false','false')  
declare @DIM_PartnersLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PartnersLocalNameNewId,0,'DIM_Partners','[Local Name]','Local Name','nText','false',0,100,'false','false')  
declare @DIM_PartnersCityNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersCityNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PartnersCityNewId,0,'DIM_Partners','[City]','City','nText','false',0,25,'false','false')  
declare @DIM_PartnersCountryNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersCountryNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PartnersCountryNewId,0,'DIM_Partners','[Country]','Country','Text','false',0,120,'false','false')  
declare @DIM_PartnersStateNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersStateNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PartnersStateNameNewId,0,'DIM_Partners','[State Name]','State Name','Text','false',0,40,'false','false')  
declare @DIM_PartnersZipCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersZipCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PartnersZipCodeNewId,0,'DIM_Partners','[Zip Code]','Zip Code','Text','false',0,15,'false','false')  
declare @DIM_PartnersPrimaryContactNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersPrimaryContactNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PartnersPrimaryContactNewId,0,'DIM_Partners','[Primary Contact]','Primary Contact','Text','false',0,60,'false','false')  
declare @DIM_PartnersAccountManagerNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersAccountManagerNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PartnersAccountManagerNewId,0,'DIM_Partners','[Account Manager]','Account Manager','Text','false',0,60,'false','false')  
declare @DIM_PartnersSalesmanNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersSalesmanNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PartnersSalesmanNewId,0,'DIM_Partners','[Salesman]','Salesman','Text','false',0,60,'false','false')  
declare @DIM_PartnersCustomerRankNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersCustomerRankNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PartnersCustomerRankNewId,0,'DIM_Partners','[Customer Rank]','Customer Rank','Text','false',0,40,'false','false')  
declare @DIM_PartnersPartnerTypeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersPartnerTypeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PartnersPartnerTypeNewId,0,'DIM_Partners','[Partner Type]','Partner Type','Text','true',0,20,'false','false')  
declare @DIM_PartnersSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PartnersSourceTenantNewId,0,'DIM_Partners','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false')  
declare @DIM_PartnersParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PartnersParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PartnersParentTenantNewId,0,'DIM_Partners','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false')  
------------------------------------------------------------------------------------
declare @DIM_PortsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed) Values(@DIM_PortsNewId,0,'DIM_Ports','DIM_Ports','Dimension','false')  
--Fields --
declare @DIM_PortsId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PortsId_NumberNewId,0,'DIM_Ports','[Id_Number]','Id_Number','Integer','true',0,0,'true','false')  
declare @DIM_PortsIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PortsIdNewId,0,'DIM_Ports','[Id]','Id','Text','true',0,15,'false','false')  
declare @DIM_PortsNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PortsNameNewId,0,'DIM_Ports','[Name]','Name','Text','true',0,40,'false','false')  
declare @DIM_PortsCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PortsCodeNewId,0,'DIM_Ports','[Code]','Code','Text','true',0,3,'false','false')  
declare @DIM_PortsLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PortsLocalNameNewId,0,'DIM_Ports','[Local Name]','Local Name','nText','false',0,40,'false','false')  
declare @DIM_PortsUNLocCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsUNLocCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PortsUNLocCodeNewId,0,'DIM_Ports','[UN Loc Code]','UN Loc Code','Text','false',0,30,'false','false')  
declare @DIM_PortsCountryNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsCountryNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PortsCountryNewId,0,'DIM_Ports','[Country]','Country','Text','true',0,40,'false','false')  
declare @DIM_PortsStateNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsStateNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PortsStateNameNewId,0,'DIM_Ports','[State Name]','State Name','Text','false',0,40,'false','false')  
declare @DIM_PortsSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PortsSourceTenantNewId,0,'DIM_Ports','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false')  
declare @DIM_PortsParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_PortsParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_PortsParentTenantNewId,0,'DIM_Ports','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false')  
------------------------------------------------------------------------------------
declare @DIM_ShipmentStatusesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed) Values(@DIM_ShipmentStatusesNewId,0,'DIM_ShipmentStatuses','DIM_ShipmentStatuses','Dimension','false')  
--Fields --
declare @DIM_ShipmentStatusesId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_ShipmentStatusesId_NumberNewId,0,'DIM_ShipmentStatuses','[Id_Number]','Id_Number','Integer','true',0,0,'true','false')  
declare @DIM_ShipmentStatusesIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_ShipmentStatusesIdNewId,0,'DIM_ShipmentStatuses','[Id]','Id','Text','true',0,15,'false','false')  
declare @DIM_ShipmentStatusesNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_ShipmentStatusesNameNewId,0,'DIM_ShipmentStatuses','[Name]','Name','Text','true',0,40,'false','false')  
declare @DIM_ShipmentStatusesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_ShipmentStatusesCodeNewId,0,'DIM_ShipmentStatuses','[Code]','Code','Text','true',0,4,'false','false')  
declare @DIM_ShipmentStatusesSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_ShipmentStatusesSourceTenantNewId,0,'DIM_ShipmentStatuses','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false')  
declare @DIM_ShipmentStatusesParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_ShipmentStatusesParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_ShipmentStatusesParentTenantNewId,0,'DIM_ShipmentStatuses','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false')  
------------------------------------------------------------------------------------
declare @DIM_TenantsNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TenantsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed) Values(@DIM_TenantsNewId,0,'DIM_Tenants','DIM_Tenants','Dimension','false')  
--Fields --
declare @DIM_TenantsTenantNumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TenantsTenantNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_TenantsTenantNumberNewId,0,'DIM_Tenants','[TenantNumber]','TenantNumber','Integer','true',0,0,'true','false')  
declare @DIM_TenantsTenantNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TenantsTenantNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_TenantsTenantNameNewId,0,'DIM_Tenants','[TenantName]','TenantName','Text','true',0,100,'false','false')  
declare @DIM_TenantsCountryNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TenantsCountryNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_TenantsCountryNewId,0,'DIM_Tenants','[Country]','Country','Text','false',0,120,'false','false')  
------------------------------------------------------------------------------------
declare @DIM_TransportModesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TransportModesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed) Values(@DIM_TransportModesNewId,0,'DIM_TransportModes','DIM_TransportModes','Dimension','true')  
--Fields --
declare @DIM_TransportModesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TransportModesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_TransportModesCodeNewId,0,'DIM_TransportModes','[Code]','Code','Text','true',0,1,'true','false')  
declare @DIM_TransportModesNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TransportModesNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_TransportModesNameNewId,0,'DIM_TransportModes','[Name]','Name','Text','true',0,10,'false','false')  
------------------------------------------------------------------------------------
declare @DIM_TypesNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TypesNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed) Values(@DIM_TypesNewId,0,'DIM_Types','DIM_Types','Dimension','true')  
--Fields --
declare @DIM_TypesCodeNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TypesCodeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_TypesCodeNewId,0,'DIM_Types','[Code]','Code','Text','true',0,4,'true','false')  
declare @DIM_TypesNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_TypesNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_TypesNameNewId,0,'DIM_Types','[Name]','Name','Text','true',0,40,'false','false')  
------------------------------------------------------------------------------------
declare @DIM_UsersNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed) Values(@DIM_UsersNewId,0,'DIM_Users','DIM_Users','Dimension','false')  
--Fields --
declare @DIM_UsersId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_UsersId_NumberNewId,0,'DIM_Users','[Id_Number]','Id_Number','Integer','true',0,0,'true','false')  
declare @DIM_UsersIdNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_UsersIdNewId,0,'DIM_Users','[Id]','Id','Text','true',0,15,'false','false')  
declare @DIM_UsersNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_UsersNameNewId,0,'DIM_Users','[Name]','Name','Text','true',0,60,'false','false')  
declare @DIM_UsersLocalNameNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersLocalNameNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_UsersLocalNameNewId,0,'DIM_Users','[LocalName]','LocalName','nText','false',0,100,'false','false')  
declare @DIM_UsersEmailNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersEmailNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_UsersEmailNewId,0,'DIM_Users','[Email]','Email','Text','false',0,70,'false','false')  
declare @DIM_UsersDepartmentNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersDepartmentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_UsersDepartmentNewId,0,'DIM_Users','[Department]','Department','Text','true',0,40,'false','false')  
declare @DIM_UsersBranchNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersBranchNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_UsersBranchNewId,0,'DIM_Users','[Branch]','Branch','Text','true',0,40,'false','false')  
declare @DIM_UsersSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_UsersSourceTenantNewId,0,'DIM_Users','[Source Tenant]','Source Tenant','Integer','true',0,0,'false','false')  
declare @DIM_UsersParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @DIM_UsersParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@DIM_UsersParentTenantNewId,0,'DIM_Users','[Parent Tenant]','Parent Tenant','Integer','true',0,0,'false','false')  
------------------------------------------------------------------------------------
declare @Fact_ShipmentsNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsNewId OUTPUT,'DWObjectTable' 
insert into DWObjectTables(Id,Tenant,Code,Name,TypeCode,IsClosed) Values(@Fact_ShipmentsNewId,0,'Fact_Shipments','Fact_Shipments','Fact','false')  
--Fields --
declare @Fact_ShipmentsId_NumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsId_NumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsId_NumberNewId,0,'Fact_Shipments','[Id_Number]','Id_Number','Integer','true',0,0,'true','false')  
declare @Fact_ShipmentsIdNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsIdNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsIdNewId,0,'Fact_Shipments','[Id]','Id','Text','true',0,15,'false','false')  
declare @Fact_ShipmentsSourceTenantNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsSourceTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsSourceTenantNewId,0,'Fact_Shipments','[Source Tenant]','Source Tenant','Dimension','false',0,0,'DIM_Tenants','false','false')  
declare @Fact_ShipmentsParentTenantNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsParentTenantNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsParentTenantNewId,0,'Fact_Shipments','[Parent Tenant]','Parent Tenant','Dimension','false',0,0,'DIM_Tenants','false','false')  
declare @Fact_ShipmentsDirectionNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsDirectionNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsDirectionNewId,0,'Fact_Shipments','[Direction]','Direction','Dimension','true',0,1,'DIM_Directions','false','false')  
declare @Fact_ShipmentsTransportModeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTransportModeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsTransportModeNewId,0,'Fact_Shipments','[Transport Mode]','Transport Mode','Dimension','true',0,1,'DIM_TransportModes','false','false')  
declare @Fact_ShipmentsLevelNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsLevelNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsLevelNewId,0,'Fact_Shipments','[Level]','Level','Dimension','true',0,1,'DIM_Levels','false','false')  
declare @Fact_ShipmentsTypeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTypeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsTypeNewId,0,'Fact_Shipments','[Type]','Type','Dimension','true',0,4,'DIM_Types','false','false')  
declare @Fact_ShipmentsDepartmentNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsDepartmentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsDepartmentNewId,0,'Fact_Shipments','[Department]','Department','Dimension','true',0,0,'DIM_Departments','false','false')  
declare @Fact_ShipmentsBranchNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsBranchNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsBranchNewId,0,'Fact_Shipments','[Branch]','Branch','Dimension','true',0,0,'DIM_Branches','false','false')  
declare @Fact_ShipmentsShipmentNumberNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsShipmentNumberNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsShipmentNumberNewId,0,'Fact_Shipments','[Shipment Number]','Shipment Number','Text','true',0,15,'false','false')  
declare @Fact_ShipmentsHouseNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsHouseNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsHouseNewId,0,'Fact_Shipments','[House]','House','Text','false',0,20,'false','false')  
declare @Fact_ShipmentsMasterNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsMasterNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsMasterNewId,0,'Fact_Shipments','[Master]','Master','Text','false',0,30,'false','false')  
declare @Fact_ShipmentsShipperNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsShipperNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsShipperNewId,0,'Fact_Shipments','[Shipper]','Shipper','Dimension','false',0,0,'DIM_Partners','false','false')  
declare @Fact_ShipmentsConsigneeNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsConsigneeNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsConsigneeNewId,0,'Fact_Shipments','[Consignee]','Consignee','Dimension','false',0,0,'DIM_Partners','false','false')  
declare @Fact_ShipmentsAgentNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAgentNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsAgentNewId,0,'Fact_Shipments','[Agent]','Agent','Dimension','false',0,0,'DIM_Partners','false','false')  
declare @Fact_ShipmentsCustomerNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCustomerNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsCustomerNewId,0,'Fact_Shipments','[Customer]','Customer','Dimension','false',0,0,'DIM_Partners','false','false')  
declare @Fact_ShipmentsIncotermNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsIncotermNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsIncotermNewId,0,'Fact_Shipments','[Incoterm]','Incoterm','Dimension','false',0,0,'DIM_Incoterms','false','false')  
declare @Fact_ShipmentsGrossWeightKGNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsGrossWeightKGNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode) Values(@Fact_ShipmentsGrossWeightKGNewId,0,'Fact_Shipments','[Gross Weight (KG)]','Gross Weight (KG)','Decimal','false',0,0,'false','true','SUM')  
declare @Fact_ShipmentsChargeableWeightKGNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsChargeableWeightKGNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode) Values(@Fact_ShipmentsChargeableWeightKGNewId,0,'Fact_Shipments','[Chargeable Weight (KG)]','Chargeable Weight (KG)','Decimal','false',0,0,'false','true','SUM')  
declare @Fact_ShipmentsTotalVolumeCBMNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTotalVolumeCBMNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode) Values(@Fact_ShipmentsTotalVolumeCBMNewId,0,'Fact_Shipments','[Total Volume (CBM)]','Total Volume (CBM)','Decimal','false',0,0,'false','true','SUM')  
declare @Fact_ShipmentsNumberofPackagesNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsNumberofPackagesNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode) Values(@Fact_ShipmentsNumberofPackagesNewId,0,'Fact_Shipments','[Number of Packages]','Number of Packages','Integer','false',0,0,'false','true','SUM')  
declare @Fact_ShipmentsNumberofContainersNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsNumberofContainersNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode) Values(@Fact_ShipmentsNumberofContainersNewId,0,'Fact_Shipments','[Number of Containers]','Number of Containers','Integer','false',0,0,'false','true','SUM')  
declare @Fact_ShipmentsSalesmanNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsSalesmanNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsSalesmanNewId,0,'Fact_Shipments','[Salesman]','Salesman','Dimension','false',0,0,'DIM_Users','false','false')  
declare @Fact_ShipmentsAccountManagerNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAccountManagerNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsAccountManagerNewId,0,'Fact_Shipments','[Account Manager]','Account Manager','Dimension','false',0,0,'DIM_Users','false','false')  
declare @Fact_ShipmentsReceivablesLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsReceivablesLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode) Values(@Fact_ShipmentsReceivablesLocalNewId,0,'Fact_Shipments','[Receivables ( Local )]','Receivables ( Local )','Decimal','false',0,0,'false','true','COUNT')  
declare @Fact_ShipmentsPayablesLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsPayablesLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode) Values(@Fact_ShipmentsPayablesLocalNewId,0,'Fact_Shipments','[Payables ( Local )]','Payables ( Local )','Decimal','false',0,0,'false','true','COUNT')  
declare @Fact_ShipmentsProfitLocalNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsProfitLocalNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode) Values(@Fact_ShipmentsProfitLocalNewId,0,'Fact_Shipments','[Profit ( Local )]','Profit ( Local )','Decimal','false',0,0,'false','true','COUNT')  
declare @Fact_ShipmentsReceivablesProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsReceivablesProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode) Values(@Fact_ShipmentsReceivablesProfitNewId,0,'Fact_Shipments','[Receivables (Profit)]','Receivables (Profit)','Decimal','false',0,0,'false','true','COUNT')  
declare @Fact_ShipmentsPayablesProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsPayablesProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode) Values(@Fact_ShipmentsPayablesProfitNewId,0,'Fact_Shipments','[Payables (Profit)]','Payables (Profit)','Decimal','false',0,0,'false','true','COUNT')  
declare @Fact_ShipmentsProfitNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsProfitNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode) Values(@Fact_ShipmentsProfitNewId,0,'Fact_Shipments','[Profit]','Profit','Decimal','false',0,0,'false','true','COUNT')  
declare @Fact_ShipmentsLocalCurrencyNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsLocalCurrencyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsLocalCurrencyNewId,0,'Fact_Shipments','[Local Currency ]','Local Currency ','Dimension','false',0,0,'DIM_Currencies','false','false')  
declare @Fact_ShipmentsProfitCurrencyNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsProfitCurrencyNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsProfitCurrencyNewId,0,'Fact_Shipments','[Profit Currency]','Profit Currency','Dimension','false',0,0,'DIM_Currencies','false','false')  
declare @Fact_ShipmentsNumberofInvoicesNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsNumberofInvoicesNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement,AggregationTypeCode) Values(@Fact_ShipmentsNumberofInvoicesNewId,0,'Fact_Shipments','[Number of Invoices]','Number of Invoices','Integer','false',0,0,'false','true','SUM')  
declare @Fact_ShipmentsOperationallyClosedNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOperationallyClosedNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsOperationallyClosedNewId,0,'Fact_Shipments','[Operationally Closed]','Operationally Closed','Boolean','false',0,0,'false','false')  
declare @Fact_ShipmentsAccountingClosedNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAccountingClosedNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsAccountingClosedNewId,0,'Fact_Shipments','[Accounting Closed]','Accounting Closed','Boolean','false',0,0,'false','false')  
declare @Fact_ShipmentsStatusNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsStatusNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsStatusNewId,0,'Fact_Shipments','[Status]','Status','Dimension','false',0,0,'DIM_ShipmentStatuses','false','false')  
declare @Fact_ShipmentsLocationNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsLocationNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsLocationNewId,0,'Fact_Shipments','[Location]','Location','Text','false',0,40,'false','false')  
declare @Fact_ShipmentsOriginNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOriginNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsOriginNewId,0,'Fact_Shipments','[Origin]','Origin','Dimension','false',0,0,'DIM_Ports','false','false')  
declare @Fact_ShipmentsFinalDestinationNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsFinalDestinationNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsFinalDestinationNewId,0,'Fact_Shipments','[Final Destination]','Final Destination','Dimension','false',0,0,'DIM_Ports','false','false')  
declare @Fact_ShipmentsIsDepartedNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsIsDepartedNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsIsDepartedNewId,0,'Fact_Shipments','[Is Departed]','Is Departed','Boolean','false',0,0,'false','false')  
declare @Fact_ShipmentsDepartedDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsDepartedDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsDepartedDateNewId,0,'Fact_Shipments','[Departed Date]','Departed Date','DateTime','false',0,0,'false','false')  
declare @Fact_ShipmentsIsArrivedNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsIsArrivedNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsIsArrivedNewId,0,'Fact_Shipments','[Is Arrived]','Is Arrived','Boolean','false',0,0,'false','false')  
declare @Fact_ShipmentsArrivedDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsArrivedDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsArrivedDateNewId,0,'Fact_Shipments','[Arrived Date]','Arrived Date','DateTime','false',0,0,'false','false')  
declare @Fact_ShipmentsIsCustomsClearedNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsIsCustomsClearedNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsIsCustomsClearedNewId,0,'Fact_Shipments','[Is Customs Cleared]','Is Customs Cleared','Boolean','false',0,0,'false','false')  
declare @Fact_ShipmentsCustomsClearenceDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCustomsClearenceDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsCustomsClearenceDateNewId,0,'Fact_Shipments','[Customs Clearence Date]','Customs Clearence Date','Dimension','false',0,0,'DIM_Dates','false','false')  
declare @Fact_ShipmentsTotalShipmentsNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsTotalShipmentsNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsTotalShipmentsNewId,0,'Fact_Shipments','[Total Shipments]','Total Shipments','Integer','false',0,0,'false','false')  
declare @Fact_ShipmentsCreateDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsCreateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsCreateDateNewId,0,'Fact_Shipments','[Create Date]','Create Date','DateTime','false',0,0,'false','false')  
declare @Fact_ShipmentsLastUpdateDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsLastUpdateDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsLastUpdateDateNewId,0,'Fact_Shipments','[Last Update Date]','Last Update Date','DateTime','false',0,0,'false','false')  
declare @Fact_ShipmentsOperationalDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOperationalDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsOperationalDateNewId,0,'Fact_Shipments','[Operational Date]','Operational Date','Dimension','false',0,0,'DIM_Dates','false','false')  
declare @Fact_ShipmentsOperationalCloseDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsOperationalCloseDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsOperationalCloseDateNewId,0,'Fact_Shipments','[Operational Close Date]','Operational Close Date','Dimension','false',0,0,'DIM_Dates','false','false')  
declare @Fact_ShipmentsAccountingCloseDateNewId varchar(15)
execute usp_GetNextTableIdValue @Fact_ShipmentsAccountingCloseDateNewId OUTPUT,'DWObjectField' 
insert into DWObjectFields(Id,Tenant,DWObjectTableCode,Code,Name,DataTypeCode,IsRequired,MinLength,MaxLength,DimensionTableCode,IsPrimaryKey,IsMeasurement) Values(@Fact_ShipmentsAccountingCloseDateNewId,0,'Fact_Shipments','[Accounting Close Date]','Accounting Close Date','Dimension','false',0,0,'DIM_Dates','false','false')  
