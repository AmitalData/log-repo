
--Begin Transaction 
--Begin


--alter table [customs].[consignments] drop Consignment_StorageSite
alter table [Customs].[Consignments] alter column StorageSiteCode varchar(20)
go
--alter table [customs].[Consignments] drop Consignment_ReceiverWarehouse
alter table [Customs].[Consignments] alter column ReceiverWarehouseCode varchar(20)
go






create table [Customs].[DeliverySiteTypes] (
    [Code] [varchar](20) not null,
    [LocalName] [nvarchar](100) null,
    [EnglishName] [varchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
go

create table [Customs].[InternalBorderSiteTypes] (
    [Code] [varchar](20) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
go

create table [Customs].[RegisteredWarehouseSiteTypes] (
    [Code] [varchar](20) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
go


update [Customs].[ConsignmentInternalTransitions] set sitecode = null;
go

alter table [Customs].[ConsignmentInternalTransitions] alter column SiteCode varchar(20) null
go

alter table [Customs].[ConsignmentInternalTransitions] add constraint [ConsignmentInternalTransition_InternalBorderSiteType] foreign key ([SiteCode]) references [Customs].[InternalBorderSiteTypes]([Code]);
go

update customs.consignments set StorageSiteCode = null;
go

alter table [Customs].[Consignments] add constraint [Consignment_DeliverySiteType] foreign key ([StorageSiteCode]) references [Customs].[DeliverySiteTypes]([Code]);
go

update customs.consignments set ReceiverWarehouseCode = null;
go

alter table [Customs].[Consignments] add constraint [Consignment_RegisteredWarehouseSiteType] foreign key ([ReceiverWarehouseCode]) references [Customs].[RegisteredWarehouseSiteTypes]([Code]);
go
--end
--commit Transaction