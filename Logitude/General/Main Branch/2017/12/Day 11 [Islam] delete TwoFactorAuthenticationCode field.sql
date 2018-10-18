 
 delete from ObjectFields where FieldName = 'TwoFactorAuthenticationCode' and ObjectTableId = (select Id from ObjectTables where name = 'TenantLoginPolicy')
delete from TextCodes where Code = 'TenantLoginPolicy.F.TwoFactorAuthenticationCode'
delete from TextCodes where Code = 'TenantLoginPolicy.TwoFactorAuthenticationCodeHelpText'
delete from TextCodes where Code = 'TenantLoginPolicy.CH.TwoFactorAuthenticationCodeListLable'