
-- order by			= first
-- order by desc	= last

declare @MemoryTable table
(
  Id varchar(15) not null,  
  UserId varchar(15) null,  
  UserName varchar(16) null
)

declare @MemoryTable2 table
(
  Id varchar(15) not null,  
  UserId varchar(15) null,  
  UserName varchar(16) null
)

insert into @MemoryTable
select Shipments.Id, Shipments.OperationalClosedByUserId, Contacts.EnglishName
from Shipments
left outer join Contacts
on Shipments.OperationalClosedByUserId = Contacts.Id



