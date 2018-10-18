
declare @CommunicationLogTypeCode as varchar(4)

set @CommunicationLogTypeCode = (select Code from CommunicationLogTypes where Code = 'DCBK')
if(@CommunicationLogTypeCode is null)
Begin 
insert into CommunicationLogTypes values ('DCBK','Document Backup','DCBK,Document Backup') 

end



