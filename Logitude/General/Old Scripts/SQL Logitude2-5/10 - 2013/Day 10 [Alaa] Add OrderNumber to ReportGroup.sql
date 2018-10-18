
Begin Transaction
begin

alter table ReportGroups Add OrderNumber int not null default 0

end 
commit transaction

