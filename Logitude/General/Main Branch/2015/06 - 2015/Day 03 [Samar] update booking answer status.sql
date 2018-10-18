
delete from BookingAnswerStatus
go

insert into BookingAnswerStatus(Code, Name, SearchFields) values ('ACC', 'Accepted', 'ACC,Accepted')
insert into BookingAnswerStatus(Code, Name, SearchFields) values ('WAT', 'Waiting for Acceptance', 'WAT,Waiting for Acceptance')
insert into BookingAnswerStatus(Code, Name, SearchFields) values ('DEC', 'Declined', 'DEC,Declined')