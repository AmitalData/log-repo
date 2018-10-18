
delete from ObjectFields where FieldName = 'VatUniqueCountryCode'
delete from ObjectFields where FieldName = 'VatMandatoryCountryCode'
delete from TextCodes where Code like '%VatUniqueCountryCode%'
delete from TextCodes where Code like '%VatMandatoryCountryCode%'