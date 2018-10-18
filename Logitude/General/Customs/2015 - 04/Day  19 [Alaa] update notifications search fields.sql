Declare @objectTableId varchar(15)
Declare @notifications table (id varchar(15), entityId varchar(15), customerName nvarchar(100))
Declare @customerIds table (id varchar(15), declarationId varchar(15))
declare @customers table (id varchar(15) , localName nvarchar(100))

 set @objectTableId =  (select id from ObjectTables where name ='customs.declaration')
 insert into @notifications (id, entityId)   (select id, EntityId from [Customs].Notifications where  ObjectTableId = @objectTableId)


   select * from @notifications

 insert into @customerIds (id, declarationId)  select  CustomerId , Id from [Customs].Declarations where id in( select entityId from @notifications) 

  select * from @customerIds

  insert into @customers (id, localName) select id, localName from Cards where id in ( select id from @customerIds)
  select * from @customers
 
Update @notifications
set customerName = 

isnull(c.localName,'') + ','
from @notifications as n,@customerIds as cd , @customers as c
Where n.entityId = cd.declarationId and cd.id = c.id

  select * from @notifications

  update Customs.Notifications
  set SearchFields = 
  isnull(nt.Reference1Number,'') + ',' +
 isnull(d.LocalName,'') + ',' +
 isnull(n.customerName,'') + ',' +
 ISNull(nt.Description, '') + ','

  FROM Customs.Notifications as nt  ,[Customs].NotificationDefinitions as d, @notifications as n
  Where  nt.NotificationDefinitionCode = d.Code and nt.Id = n.id