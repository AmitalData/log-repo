
update Reports 
set ReportGroupId = (select Id from ReportGroups where Code = 'RQUO' and Tenant = 0)
where Code = 'SPQS'