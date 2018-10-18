
insert into CommunicationStatusTypes (Code, Name, SearchFields)
values ('E', 'Demo', 'E,Demo')
go

update CommunicationStatusTypes set SearchFields = CommunicationStatusTypes.Code + ',' + CommunicationStatusTypes.Name