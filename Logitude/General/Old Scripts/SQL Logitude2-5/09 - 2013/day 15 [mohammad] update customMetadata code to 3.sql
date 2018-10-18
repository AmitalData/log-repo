
begin transaction
begin

alter table customs.CustomDocumentTypeMetaData drop constraint [CustomDocumentTypeMetaData_CustomMetaDataType]

alter table customs.CustomsDocumentMetaDataValues drop constraint CustomsDocumentMetaDataValue_CustomMetaDataType

alter table customs.CustomMetadatatypes alter column Code varchar(6) not null

alter table customs.CustomDocumentTypeMetaData alter column [MetaDataTypeCode] varchar(6) not null

alter table [Customs].[CustomsDocumentMetaDataValues] alter column [MetaDataTypeCode] varchar(6) not null

alter table [Customs].[CustomDocumentTypeMetaData] add constraint [CustomDocumentTypeMetaData_CustomMetaDataType] foreign key ([MetaDataTypeCode]) references [Customs].[CustomMetaDataTypes]([Code]);

alter table [Customs].[CustomsDocumentMetaDataValues] add constraint [CustomsDocumentMetaDataValue_CustomMetaDataType] foreign key ([MetaDataTypeCode]) references [Customs].[CustomMetaDataTypes]([Code]);

END
commit transaction