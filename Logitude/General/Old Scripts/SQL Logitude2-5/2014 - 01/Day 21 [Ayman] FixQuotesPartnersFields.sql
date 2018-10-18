
update Quotes
set FromPartnerId = null
where FromPartnerId is not null and FromPartnerAddressId is null
go

update Quotes
set ToPartnerId = null
where ToPartnerId is not null and ToPartnerAddressId is null
go