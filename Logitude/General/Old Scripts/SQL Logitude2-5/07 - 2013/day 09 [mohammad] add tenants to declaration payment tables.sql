alter table Customs.DeclarationPayments add Tenant int not null 
go

alter table Customs.DeclarationPaymentMethods add Tenant int not null 
go

alter table Customs.DeclarationPaymentProtests add Tenant int not null 
go