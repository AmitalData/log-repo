
  IF OBJECT_ID ('DIM_InvoiceMainTypes', 'U')  IS   NULL 
  begin

create table DIM_InvoiceMainTypes (
[Code] varchar(2) not null,
[Name] varchar(15) not null, 
primary key (Name));

insert into DIM_InvoiceMainTypes values('AR', 'AR Invoice')
insert into DIM_InvoiceMainTypes values('AP', 'AP Invoice') 

end