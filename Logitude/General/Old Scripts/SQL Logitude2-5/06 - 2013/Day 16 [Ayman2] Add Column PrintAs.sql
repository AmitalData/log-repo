-- do not run online - already exist

alter table PackageTypes add PrintAs varchar(5)
go

update PackageTypes set PrintAs = Code
go

update PackageTypes
set PrintAs = LEFT(Code,2) + CHAR(39) + REPLACE(Code,LEFT(Code,2),'')
where LEN(Code) > 2 AND ISNUMERIC(LEFT(Code,2)) = 1
go

-- For Apdating the General Screen Tab fields
update Screens set NumberOfRows = 8, NumberOfColumns = 2
where Code = 'PackageType.GeneralTabScreen'
go

delete from ScreenFields where ScreenId = (Select Id from Screens where Code = 'PackageType.GeneralTabScreen')
go

--Update Tenant 0