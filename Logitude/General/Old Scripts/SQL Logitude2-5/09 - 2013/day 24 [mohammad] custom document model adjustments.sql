--please update customs after executing the script


begin transaction
begin

delete from customs.CustomsDocumentMetaDataValues
delete from customs.CustomsDocuments
drop table customs.CustomsDocumentMetaDataValues
drop table customs.CustomsDocuments
delete from ObjectFields where ObjectTableId=(select id from ObjectTables where name='Customs.CustomsDocument')
delete from RoleFeatures where FeatureId in (select id from Features where ObjectTableId=(select id from ObjectTables where name='Customs.CustomsDocument'))
delete from Features where ObjectTableId=(select id from ObjectTables where name='Customs.CustomsDocument')
delete from TextCodes where ObjectTableId=(select id from ObjectTables where name='Customs.CustomsDocument')
delete from ObjectTables where name='customs.CustomsDocument'

create table [Customs].[CustomsDocuments] (
    [DocumentInId] [varchar](15) not null,
    [Tenant] [int] not null,
    [CustomsDocId] [varchar](15) null,
    [DocumentStatusCode] [varchar](1) null,
    [DocumentRemarks] [varchar](512) null,
    primary key ([DocumentInId])
);

create table [Customs].[CustomsDocumentPointers] (
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
    [CustomDocumentId] [varchar](15) null,
    [RequiredDocID] [varchar](12) null,
    [DocumentTypeCode] [varchar](7) not null,
    primary key ([Id])
);

create table [Customs].[CustomsDocumentMetaDataValues] (
    [CustomsDocumentId] [varchar](15) not null,
    [MetaDataTypeCode] [varchar](6) not null,
    [Tenant] [int] not null,
    [MetaDataValue] [varchar](128) null,
    primary key ([CustomsDocumentId], [MetaDataTypeCode])
);

alter table [Customs].[CustomsDocuments] add constraint [CustomsDocument_CustomsDocumentStatusType] foreign key ([DocumentStatusCode]) references [Customs].[CustomsDocumentStatusTypes]([Code]);
alter table [Customs].[CustomsDocuments] add constraint [CustomsDocument_DocumentIn] foreign key ([DocumentInId]) references [dbo].[DocumentIns]([Id]);
alter table [Customs].[CustomsDocumentMetaDataValues] add constraint [CustomsDocumentMetaDataValue_CustomMetaDataType] foreign key ([MetaDataTypeCode]) references [Customs].[CustomMetaDataTypes]([Code]);
alter table [Customs].[CustomsDocumentMetaDataValues] add constraint [CustomsDocumentMetaDataValue_CustomsDocument] foreign key ([CustomsDocumentId]) references [Customs].[CustomsDocuments]([DocumentInId]);
alter table [Customs].[CustomsDocumentPointers] add constraint [CustomsDocumentPointer_CustomDocumentType] foreign key ([DocumentTypeCode]) references [Customs].[CustomDocumentTypes]([Code]);
alter table [Customs].[CustomsDocumentPointers] add constraint [CustomsDocumentPointer_CustomsDocument] foreign key ([CustomDocumentId]) references [Customs].[CustomsDocuments]([DocumentInId]);

END
commit transaction


