

-- change tenant!

--declare @tenant int = 999999

-- Delete report copies
declare @reportCode varchar(4) = 'AGER'
update Reports set DefaultMessageTemplateId = null , DefaultTemplateId = null where Id = ( select id from Reports where Code = @reportCode and Tenant = @tenant)
delete from ReportsTemplatesVersions where ReportId = ( select id from Reports where Code = @reportCode and Tenant = @tenant)
delete from ReportsTemplates where ReportId = ( select id from Reports where Code = @reportCode and Tenant = @tenant)
delete from Reports where Id = ( select id from Reports where Code = @reportCode and Tenant = @tenant)

-- Delete report copies
declare @reportCode varchar(4) = 'TRBR'
update Reports set DefaultMessageTemplateId = null , DefaultTemplateId = null where Id = ( select id from Reports where Code = @reportCode and Tenant = @tenant)
delete from ReportsTemplatesVersions where ReportId = ( select id from Reports where Code = @reportCode and Tenant = @tenant)
delete from ReportsTemplates where ReportId = ( select id from Reports where Code = @reportCode and Tenant = @tenant)
delete from Reports where Id = ( select id from Reports where Code = @reportCode and Tenant = @tenant)



-- Delete report copies
declare @reportCode varchar(4) = 'REXR'
update Reports set DefaultMessageTemplateId = null , DefaultTemplateId = null where Id = ( select id from Reports where Code = @reportCode and Tenant = @tenant)
delete from ReportsTemplatesVersions where ReportId = ( select id from Reports where Code = @reportCode and Tenant = @tenant)
delete from ReportsTemplates where ReportId = ( select id from Reports where Code = @reportCode and Tenant = @tenant)
delete from Reports where Id = ( select id from Reports where Code = @reportCode and Tenant = @tenant)













--
-- After delete report copies, you have to:
--   1- LogitudeUpdate > Copy Reports
--   2- upload report templates for each deleted reports
-- 

