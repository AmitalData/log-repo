
  IF OBJECT_ID ('DIM_InvoiceFilters', 'U')  IS   NULL 
  begin

create table DIM_InvoiceFilters (
[Code] varchar(2) not null,
[Name] varchar(15) not null, 
primary key (Name));

insert into DIM_InvoiceFilters values('AR', 'AR Invoice')
insert into DIM_InvoiceFilters values('AP', 'AP Invoice') 

end