
--ARPayment
alter table ARPayments add RegisterDate DateTime
go

update ARPayments set RegisterDate=PaymentDate
go

alter table ARPayments alter column  RegisterDate DateTime Not Null
go

alter table ARPayments drop Column PaymentDate
go

delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'PaymentDate' and ObjectTableId = (select Id from ObjectTables where Name = 'ARPayment'))
delete from ObjectFields where FieldName = 'PaymentDate' and ObjectTableId = (select Id from ObjectTables where Name = 'ARPayment')
delete from TextCodes where Code = 'ARPayment.f.PaymentDate'
delete from TextCodes where Code = 'ARPayment.ch.PaymentDateListLable'


--APPayment
alter table APPayments add RegisterDate DateTime 
go

update APPayments set RegisterDate=PaymentDate
go

alter table APPayments alter column  RegisterDate DateTime Not Null
go

alter table APPayments drop Column PaymentDate
go

delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'PaymentDate' and ObjectTableId = (select Id from ObjectTables where Name = 'APPayment'))
delete from ObjectFields where FieldName = 'PaymentDate' and ObjectTableId = (select Id from ObjectTables where Name = 'APPayment')
delete from TextCodes where Code = 'APPayment.f.PaymentDate'
delete from TextCodes where Code = 'APPayment.ch.PaymentDateListLable'