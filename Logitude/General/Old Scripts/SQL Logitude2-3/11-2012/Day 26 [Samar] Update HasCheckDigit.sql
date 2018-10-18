
alter table airlines add CheckDigit bit
go

update airlines set CheckDigit=HasCheckDigit
go

alter table airlines drop HasCheckDigit_airlines_Default_True
go

alter table airlines drop HasCheckDigit_NOTNULL
go

alter table airlines drop Column HasCheckDigit
go

delete from ScreenFields where ObjectFieldId = (select Id from ObjectFields where FieldName = 'HasCheckDigit' and ObjectTableId = (select Id from ObjectTables where Name = 'Airline'))
delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'HasCheckDigit' and ObjectTableId = (select Id from ObjectTables where Name = 'Airline'))
delete from ObjectFields where FieldName = 'HasCheckDigit' and ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
delete from TextCodes where Code = 'airline.f.HasCheckDigit'
delete from TextCodes where Code = 'airline.ch.HasCheckDigitListLable'


