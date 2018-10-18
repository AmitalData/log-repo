create table [Customs].[DeclarationErrorMappings] (
    [Id] [varchar](15) not null,
    [DocumentSectionCode] [varchar](15) null,
    [TagID] [varchar](15) null,
    [Field] [varchar](50) null,
    [Entity] [varchar](50) null,
    [Skip] [bit] not null,
    primary key ([Id])
);