

delete from QueryColumns where QueryId=(select id from Queries where ObjectTableId=(select id from ObjectTables where Name='Customs.Vendor') and Code='CustomsVendors')

delete from Queries where ObjectTableId=(select id from ObjectTables where Name='Customs.Vendor') and Code='CustomsVendors'

select * from QueryColumns where QueryId=(select id from Queries where ObjectTableId=(select id from ObjectTables where Name='Customs.Vendor') and Code='Vendors')