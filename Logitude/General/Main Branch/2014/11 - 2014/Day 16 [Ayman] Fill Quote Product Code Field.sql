

update Quotes set ProductCode = 'CI' where DirectionId = 'C'
go

update Quotes set ProductCode = TransportModeId + DirectionId where DirectionId != 'C'
go
