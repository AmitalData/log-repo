
update Airlines
set IsManagingProduct = 1, IsProductMandatory = 1
where Id = (select Id from Cards where Code = 'AR' and PartnerTypeId = 'AL' and Tenant = 0)