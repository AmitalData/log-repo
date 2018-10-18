alter table DocumentsMetaDataTypes add tempField varchar(6);
/

update DocumentsMetaDataTypes set tempField = CUSTOMSMETADATACODE;
/

alter table DocumentsMetaDataTypes drop column CUSTOMSMETADATACODE;
/

alter table DocumentsMetaDataTypes rename column tempField to CUSTOMSMETADATACODE;
/
