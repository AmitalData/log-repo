

update ARPayments
set SearchFields = SearchFields + ','+ ChequeOrPaymentRef
where ChequeOrPaymentRef is not null
and CHARINDEX(ChequeOrPaymentRef, SearchFields) = 0

update ARPayments
set SearchFields = SearchFields + ',' + InternalNotes
where InternalNotes is not null
and CHARINDEX(InternalNotes, SearchFields) = 0

update APPayments
set SearchFields = SearchFields + ','+ ChequeOrPaymentRef
where ChequeOrPaymentRef is not null
and CHARINDEX(ChequeOrPaymentRef, SearchFields) = 0

update ObjectFields
set DataTypeCode = 'nText', SystemMaxLength = MaxLength
where FieldName = 'SearchFields' and ObjectTableId in (select Id from ObjectTables where Name in ('ARPayment','APPayment'))
go

update TextCodes set DefaultText = 'Search Payment # / Vendor / Reference' where Code = 'APPayment.F.SearchFields'
update TextCodes set DefaultText = 'Search Payment # / Bill to / Reference / Notes' where Code = 'ARPayment.F.SearchFields'

select top 10 Id, ChequeOrPaymentRef, SearchFields from ARPayments where ChequeOrPaymentRef is not null
select top 10 Id, ChequeOrPaymentRef, SearchFields from APPayments where ChequeOrPaymentRef is not null

