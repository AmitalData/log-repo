
declare @CommunicationLogTypeCode as varchar(1)

set @CommunicationLogTypeCode = (select Code from CommunicationLogTypes where Code = 'A')
if(@CommunicationLogTypeCode is null)
Begin 
insert into CommunicationLogTypes values ('A','API','A,API') 

end



