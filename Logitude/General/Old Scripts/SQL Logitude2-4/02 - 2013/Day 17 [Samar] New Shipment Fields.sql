
--MoveType
alter table MoveTypes add [Code] varchar(3) NOT NULL
go

alter table MoveTypes add [SearchFields] nvarchar(1000)  NULL
go

--Shipment
alter table Shipments add [MoveTypeId] varchar(15)  NULL
go

alter table Shipments add [AMSBL] nvarchar(17)  NULL
go

alter table Shipments add [ColoaderReference1] nvarchar(50)  NULL
go

alter table Shipments add [ColoaderContactId] nvarchar(15)  NULL
go

alter table Shipments add [ColoaderAddressId] nvarchar(15)  NULL
go

alter table Shipments add [ColoaderId] nvarchar(15)  NULL
go

alter table Shipments add [CustomClearancePointReference1] nvarchar(50)  NULL
go

alter table Shipments add [CustomClearancePointContactId] nvarchar(15)  NULL
go

alter table Shipments add [CustomClearancePointAddressId] nvarchar(15)  NULL
go

alter table Shipments add [CustomClearancePointId] nvarchar(15)  NULL
go

--Tenant
alter table Tenants add [IsSharedLogisticsActivated] bit NULL default 0
go

--Card
alter table Cards add [InternetAccess] bit NULL default 0
go

-----
update Tenants set IsSharedLogisticsActivated = '0'
update Cards set InternetAccess = '0'

delete from ObjectTableTabs where code = 'CLSL'
delete from TextCodes where code = 'Customer.TH.SharedLogistic'
delete from RoleFeatures where FeatureId = (select Id from Features where code = 'SHAREDLOGISTIC' and ObjectTableId = (select Id from ObjectTables where name = 'Customer'))
delete from Features where code = 'SHAREDLOGISTIC' and ObjectTableId = (select Id from ObjectTables where name = 'Customer')