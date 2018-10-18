 
--delete  from querycolumns where queryid in (select  id from queries where  code = 'Invoices Failed to Open in SAT') and tenant <> 0
delete from querycolumns where queryid in (select id from queries where code = 'SAT Failed Invoices')
delete from advancedqueryfilters where queryid in (select id from queries where code = 'SAT Failed Invoices')
delete from queries where code = 'SAT Failed Invoices'

--delete  from querycolumns where queryid in (select  id from queries where  code = 'Payments Failed to Open in SAT') and tenant <> 0
delete from querycolumns where queryid in (select id from queries where code = 'SAT Failed Payments')
delete from advancedqueryfilters where queryid in (select id from queries where code = 'SAT Failed Payments')
delete from queries where code = 'SAT Failed Payments'