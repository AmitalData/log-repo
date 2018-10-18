  
INSERT INTO DBIdCounters VALUES ('EntityChange', (select LastIdNumber from DBIdCounters where TableName = 'Shipment')+1000);