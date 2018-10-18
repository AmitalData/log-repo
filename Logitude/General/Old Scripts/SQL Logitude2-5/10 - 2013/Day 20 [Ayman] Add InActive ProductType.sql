
select * from ProductTypes

alter table ProductTypes add InActive bit not null default 0
go
update ProductTypes set InActive = 1 where Code = 'AD'
update ProductTypes set InActive = 1 where Code = 'OD'
update ProductTypes set InActive = 1 where Code = 'ID'