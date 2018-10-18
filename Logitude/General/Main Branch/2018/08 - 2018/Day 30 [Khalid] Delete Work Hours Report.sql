select * from Reports where DefaultTemplateId in (select id from ReportsTemplates where  reportId In (Select Id from Reports where Code='WPTS'))
update  Reports set DefaultTemplateId = null where DefaultTemplateId in (select id from ReportsTemplates where  reportId In (Select Id from Reports where Code='WPTS'))
select * from ReportsTemplates where  reportId In (Select Id from Reports where Code='WPTS')

select * from ReportsTemplatesVersions where TemplateId in (select id from ReportsTemplates where  reportId In (Select Id from Reports where Code='WPTS'))
delete from ReportsTemplatesVersions where TemplateId in (select id from ReportsTemplates where  reportId In (Select Id from Reports where Code='WPTS'))

select * from ReportsTemplates where  reportId In (Select Id from Reports where Code='WPTS')
delete from ReportsTemplates where  reportId In (Select Id from Reports where Code='WPTS')

select * from Reports where Code='WPTS'
delete from Reports where Code='WPTS'