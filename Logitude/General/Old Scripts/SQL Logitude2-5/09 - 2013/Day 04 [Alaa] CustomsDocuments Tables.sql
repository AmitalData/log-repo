create table [Customs].[CustomsDocuments] (
    [Id] [varchar](15) not null,
    [Tenant] [int] not null,
    [ParentEntityCode] [varchar](40) null,
    [ParentEntityId] [varchar](15) null,
    [Child1EntityCode] [varchar](40) null,
    [Child1EntityId] [varchar](15) null,
    [Child2EntityCode] [varchar](40) null,
    [Child2EntityId] [varchar](15) null,
    [Child3EntityCode] [varchar](40) null,
    [Child3EntityId] [varchar](15) null,
    [DocumentInId] [varchar](15) null,
    [RequiredDocID] [varchar](12) null,
    [DocumentStatusCode] [varchar](1) null,
    [DocumentRemarks] [varchar](512) null,
    [DocumentTypeCode] [varchar](7) null,
    primary key ([Id])
);
create table [Customs].[CustomsDocumentMetaDataValues] (
    [CustomsDocumentId] [varchar](15) not null,
    [MetaDataTypeCode] [varchar](2) not null,
    [Tenant] [int] not null,
    [MetaDataValue] [varchar](128) null,
    primary key ([CustomsDocumentId], [MetaDataTypeCode])
);
create table [Customs].[CustomsDocumentStatusTypes] (
    [Code] [varchar](1) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[CustomDocumentTypes] (
    [Code] [varchar](7) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

create table [Customs].[CustomDocumentTypeMetaData] (
    [MetaDataTypeCode] [varchar](2) not null,
    [DocumentTypeCode] [varchar](7) null,
    [Mandatory] [bit] not null,
    [Format] [varchar](20) null,
    [ValuesTable] [varchar](100) null,
    primary key ([MetaDataTypeCode])
);



create table [Customs].[CustomMetaDataTypes] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);



alter table [Customs].[CustomsDocuments] add constraint [CustomsDocument_CustomDocumentType] foreign key ([DocumentTypeCode]) references [Customs].[CustomDocumentTypes]([Code]);
alter table [Customs].[CustomsDocuments] add constraint [CustomsDocument_CustomsDocumentStatusType] foreign key ([DocumentStatusCode]) references [Customs].[CustomsDocumentStatusTypes]([Code]);
alter table [Customs].[CustomsDocumentMetaDataValues] add constraint [CustomsDocumentMetaDataValue_CustomMetaDataType] foreign key ([MetaDataTypeCode]) references [Customs].[CustomMetaDataTypes]([Code]);
alter table [Customs].[CustomsDocumentMetaDataValues] add constraint [CustomsDocumentMetaDataValue_CustomsDocument] foreign key ([CustomsDocumentId]) references [Customs].[CustomsDocuments]([Id]);
alter table [Customs].[CustomDocumentTypeMetaData] add constraint [CustomDocumentTypeMetaData_CustomDocumentType] foreign key ([DocumentTypeCode]) references [Customs].[CustomDocumentTypes]([Code]);
alter table [Customs].[CustomDocumentTypeMetaData] add constraint [CustomDocumentTypeMetaData_CustomMetaDataType] foreign key ([MetaDataTypeCode]) references [Customs].[CustomMetaDataTypes]([Code]);
