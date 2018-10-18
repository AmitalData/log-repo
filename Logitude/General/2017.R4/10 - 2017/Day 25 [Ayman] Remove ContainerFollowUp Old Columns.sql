


delete from QueryColumns where ObjectFieldId in (select Id from ObjectFields where FieldName = 'DeliveryOn' and ObjectTableId = (select Id from ObjectTables where Name = 'ContainerFollowUp'))
delete from ObjectFields where FieldName = 'DeliveryOn' and ObjectTableId = (select Id from ObjectTables where Name = 'ContainerFollowUp')
delete from TextCodes where Code like '%ContainerFollowUp%DeliveryOn%'

delete from QueryColumns where ObjectFieldId in (select Id from ObjectFields where FieldName = 'ReturnOn' and ObjectTableId = (select Id from ObjectTables where Name = 'ContainerFollowUp'))
delete from ObjectFields where FieldName = 'ReturnOn' and ObjectTableId = (select Id from ObjectTables where Name = 'ContainerFollowUp')
delete from TextCodes where Code like '%ContainerFollowUp%ReturnOn%'