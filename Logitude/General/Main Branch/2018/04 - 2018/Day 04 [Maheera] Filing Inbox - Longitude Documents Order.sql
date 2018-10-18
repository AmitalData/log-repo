--> Please run this script at Logitude Env. ONLY

update DocumentTypes set [OrderBy] = 10
update DocumentTypes set [OrderBy] = 1 where Code in ('380','CQ')
update DocumentTypes set [OrderBy] = 2 where Code in ('721','QUOTE')
update DocumentTypes set [OrderBy] = 3 where Code in ('341','TQ')
update DocumentTypes set [OrderBy] = 4 where Code in ('770','PQ')

select [OrderBy], * from DocumentTypes where tenant = 0 and Code in ('380', '721', '341', '770')
select [OrderBy], * from DocumentTypes where tenant = 0 and Code in ('CQ', 'QUOTE', 'TQ', 'PQ')