insert into BookingLevels(Code, Name, SearchFields) values ('D', 'Direct', 'D,Direct' )
insert into BookingLevels(Code, Name, SearchFields) values ('C', 'Consol', 'C,Consol' )
go

update Bookings set BookingLevelCode = 'D' where BookingLevelCode = 'DI'
update Bookings set BookingLevelCode = 'C' where BookingLevelCode = 'MS'
go

delete from BookingLevels where Code = 'DI'
delete from BookingLevels where Code = 'MS'
go
