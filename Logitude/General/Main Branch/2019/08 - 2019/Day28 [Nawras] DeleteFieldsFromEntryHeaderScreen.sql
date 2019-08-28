delete  ScreenFields where ObjectFieldId in (select id from ObjectFields where ObjectTableId  = (select id from ObjectTables where Name = 'WarehouseEntry') and ( FieldName = 'HouseNumber' or FieldName ='EntryReferencesAndDate' ))



