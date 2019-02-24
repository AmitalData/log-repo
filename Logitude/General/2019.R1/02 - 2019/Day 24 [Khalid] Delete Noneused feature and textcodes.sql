delete from advancedqueryfilters where objectfieldId In ( select Id from ObjectFields where FieldName like '%qbofailed%')
delete from ObjectFields where FieldName like '%qbofailed%'
delete from QueryColumns where QueryId In ( select Id from queries where FeatureId In ( select Id  from Features where NameTextCodeId In ( Select Id from TextCodes where Code like '%QBOfailedtransmission%')))
delete from queries where FeatureId In ( select Id  from Features where NameTextCodeId In ( Select Id from TextCodes where Code like '%QBOfailedtransmission%'))
delete from Features where NameTextCodeId In ( Select Id from TextCodes where Code like '%QBOfailedtransmission%')
delete from TextCodes where Code like '%QBOfailedtransmission%'
delete from Features where Code like '%QBOfailedtransmission%'