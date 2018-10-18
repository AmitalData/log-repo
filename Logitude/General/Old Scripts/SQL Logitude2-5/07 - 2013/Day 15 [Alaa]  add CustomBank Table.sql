create table [Customs].[PayerTypes] (
    [Code] [varchar](1) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[CustomBanks] (
    [Id] [varchar](15) not null,
    [Tenant] [int] not null,
    [InternalCode] [varchar](15) null,
    [BankCode] [varchar](2) null,
    [BranchCode] [varchar](3) null,
    [AccountNumber] [varchar](11) null,
    [LocalName] [nvarchar](30) null,
    [EnglishName] [varchar](30) null,
    [InActive] [bit] not null,
    [PayerTypeCode] [varchar](1) null,
    [BankAddress] [nvarchar](1024) null,
    primary key ([Id])
);


ALTER TABLE [Customs].[CustomBanks] ADD  CONSTRAINT [UQ_Tenant_InternalCode_CustomBanks] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[InternalCode] ASC
)

alter table [Customs].[CustomBanks] add constraint [CustomBank_Bank] foreign key ([BankCode]) references [Customs].[Banks]([Code]);
alter table [Customs].[CustomBanks] add constraint [CustomBank_Branch] foreign key ([BranchCode]) references [Customs].[Branches]([Code]);
alter table [Customs].[CustomBanks] add constraint [CustomBank_PayerType] foreign key ([PayerTypeCode]) references [Customs].[PayerTypes]([Code]);
