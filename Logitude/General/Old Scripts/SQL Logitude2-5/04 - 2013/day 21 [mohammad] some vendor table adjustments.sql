alter table customs.vendors alter column VendorName varchar(55) null
go
alter table customs.vendors alter column VendorTypeCode varchar(2) null
go

alter table customs.vendors alter column CountryCode varchar(2) null
go
alter table customs.vendors alter column SubCountryCode varchar(6) null
go
alter table customs.vendors alter column VendorNumber varchar(9) null
go

alter table customs.VendorCommunications alter column CommunicationAddress varchar(50) null
go