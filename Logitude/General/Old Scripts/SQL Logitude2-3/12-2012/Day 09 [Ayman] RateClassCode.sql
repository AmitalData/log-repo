

update Shipments set RateClassCode = 'Q' where RateClassCode = 'A' OR RateClassCode = 'W' OR RateClassCode is null
go

delete from RateClasses where Code = 'A' Or Code = 'W'
go