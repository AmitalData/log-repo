


alter table Airlines add AccountNumber varchar(14) null
go

alter table Shipments add AccountNumber varchar(14) null
go

alter table Shipments add AsAgreedFreight bit not null default 0
go

alter table Shipments add AsAgreedOtherCharges bit not null default 0
go

update shipments set AsAgreed = 0 where AsAgreed is null

update Shipments set AsAgreedFreight = AsAgreed
go

update Shipments set AsAgreedOtherCharges = AsAgreed
go

alter table Shipments drop column AsAgreed
go

delete from ObjectFields where FieldName like '%AsAgreed%'
go

delete from TextCodes where Code like '%AsAgreed%'
go
select *  from TextCodes where Code like '%AsAgreed%'
go
delete from ObjectFields where FieldName like '%FinalArrivalDate'
go

delete from TextCodes where Code like '%FinalArrivalDate%'
go

delete from ScreenFields where ScreenId in (select Id from Screens where Code = 'Airline.GeneralTabScreen' Or Code = 'Airline.BillingTabScreen') 
go

delete from ObjectFields where fieldname = 'AccountNumber' and ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
go

delete from ObjectFields where fieldname = 'BankAccountNumber' and ObjectTableId = (select Id from ObjectTables where Name = 'Airline')
go

delete  from TextCodes where Code like '%Airline%AccountNumber%'
go

