drop table [Customs].[VendorCommunications]
go

create table [Customs].[VendorCommunications] (
    [VendorId] [varchar](15) not null,
    [LineNumber] [int] not null,
    [Tenant] [int] not null,
    [CommunicationTypeCode] [varchar](2) not null,
    [CommunicationAddress] [varchar](2) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([VendorId], [LineNumber])
);

go