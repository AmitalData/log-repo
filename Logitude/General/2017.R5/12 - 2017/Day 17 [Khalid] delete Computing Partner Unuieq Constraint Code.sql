declare @table_name nvarchar(256)  
declare @col_name nvarchar(256)  
declare @Command  nvarchar(1000)  

set @table_name = N'ComputingPartners'
set @col_name = N'Code'

select @Command = 'ALTER TABLE ' + @table_name + ' drop constraint ' + d.name
    from sys.tables t 
    join sys.indexes d on d.object_id = t.object_id  and d.type=2 and d.is_unique=1
    join sys.index_columns ic on d.index_id=ic.index_id and ic.object_id=t.object_id
    join sys.columns c on ic.column_id = c.column_id  and c.object_id=t.object_id
    where t.name = @table_name and c.name=@col_name;

	execute(@Command)
	print(@Command)


