delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'ProductionStartDate')
delete from ObjectFields where FieldName = 'ProductionStartDate'
delete from TextCodes where Code = 'TenantManagement.F.ProductionStartDate'
delete from TextCodes where Code = 'TenantManagement.ProductionStartDateHelpText'
delete from TextCodes where Code = 'TenantManagement.CH.ProductionStartDateListLable'
delete from TextCodes where Code = 'TenantManagement.ProductionEndDateHelpText'
delete from TextCodes where Code = 'TenantManagement.CH.ProductionEndDateListLable'