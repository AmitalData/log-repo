create table [Customs].[ClientsAddressCommunicationTypes] (
    [ClientId] [varchar](15) not null,
    [AddressId] [varchar](9) not null,
    [Line] [decimal](2, 0) not null,
    [CommunicationTypeCode] [varchar](2) null,
    [CommunicationAddress] [varchar](50) null,
    primary key ([ClientId], [AddressId], [Line])
);

alter table [Customs].[ClientsAddressCommunicationTypes] add constraint [ClientsAddressCommunicationType_ClientAddress] foreign key ([ClientId], [AddressId]) references [Customs].[ClientAddresses]([ClientId], [AddressId]);
alter table [Customs].[ClientsAddressCommunicationTypes] add constraint [ClientsAddressCommunicationType_CommunicationType] foreign key ([CommunicationTypeCode]) references [Customs].[CommunicationTypes]([Code]);
