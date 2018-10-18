alter table ReportGroups add  OrderNumber int null 
go

update  ReportGroups set OrderNumber = 4 where code = 'RSTA'
update ReportGroups set OrderNumber = 1 where code= 'ROPR'
update ReportGroups set OrderNumber = 3 where Code = 'RACC'
update ReportGroups set OrderNumber = 2 where Code= 'RQUO'
