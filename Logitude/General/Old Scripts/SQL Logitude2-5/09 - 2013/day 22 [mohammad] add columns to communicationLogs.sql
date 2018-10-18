
begin transaction
begin

alter table communicationlogs add Logs nvarchar(max) null
alter table communicationlogs add CorrelationID varchar(64) null

END
commit transaction