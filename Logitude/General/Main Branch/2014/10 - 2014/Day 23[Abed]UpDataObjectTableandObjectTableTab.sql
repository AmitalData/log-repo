

--Main DB
-- Run this after update database migration
-- Please Run this then update 

update ObjectTables set HasDocuments ='true'
FROM ObjectTables 
WHERE Name ='Shipment' or   Name ='Quote'  or  Name ='Master'  or   Name ='Invoice'   or   Name ='Customer'   or   Name ='Opportunity' or  Name ='ARPayment'  or   Name ='APInvoice'   or   Name ='APPayment'   or   Name ='ARInvoice' 


update ObjectTables set IsOperational ='true'
FROM ObjectTables 
WHERE Name ='Shipment' or   Name ='Quote' 



Delete From ObjectTableTabs WHERE Code = 'DTRT' or   Code = 'DTHT'
 