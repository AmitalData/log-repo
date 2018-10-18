-- main db

declare @Command  nvarchar(1000)
declare @table_name nvarchar(256)

set @table_name = N'airlines'

select @Command = 'ALTER TABLE ' + @table_name + ' drop constraint ' + (SELECT name  
FROM sys.default_constraints
WHERE  name like '%DF__Airlines__IsRegi%' and OBJECT_NAME(parent_object_id) = @table_name)

execute (@Command)

if exists(select * from sys.columns 
            where Name = N'IsRegistered' and Object_ID = Object_ID(N'airlines'))    
begin
    alter table airlines drop column IsRegistered;
	print 'drop completed successfully.'
end



delete from ObjectFields where FieldName = 'IsRegistered' and ObjectTableId = (select Id from ObjectTables where name = 'Airline')
go

delete from TextCodes where code like '%IsRegistered%' and ObjectTableId = (select Id from ObjectTables where name = 'Airline')
go

