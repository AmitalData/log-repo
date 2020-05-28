delete  ScreenFields where ObjectFieldId in (select id from ObjectFields where ObjectTableId  = (select id from ObjectTables where Name = 'WarehouseEntry' and (FieldName = 'EntryReference' or FieldName = 'MasterNumber' or FieldName = 'HouseNumber' or FieldName = 'ActualEntryDate' )))





