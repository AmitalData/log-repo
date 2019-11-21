
insert into CommunicationStatusTypes (Code, Name, SearchFields)
values ('T', 'Time out', 'T,Time out')
go

update CommunicationStatusTypes set SearchFields = CommunicationStatusTypes.Code + ',' + CommunicationStatusTypes.Name