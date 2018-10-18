-- This code  delete menu table for employee group & business hour (if any of them exist)

delete from MenusTables where code = 'MTEG' and ObjectTableId = (select id from ObjectTables where name = 'EmployeeGroup')

delete from MenusTables where code = 'BUHR' and ObjectTableId = (select id from ObjectTables where name = 'BusinessHour')
