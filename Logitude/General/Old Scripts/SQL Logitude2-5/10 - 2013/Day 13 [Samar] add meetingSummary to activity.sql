begin transaction
begin

alter table Activities add[MeetingSummary] nvarchar(250) null

END
commit transaction
