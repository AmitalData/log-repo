
update Airlines
set IsDescriptionOfGoodsFromList = 1
where Id = (select Id from Cards where Code = 'LH' and PartnerTypeId = 'AL' and Tenant = 0)