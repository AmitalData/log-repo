delete from QueryColumns where QueryId in (select Id  from Queries where Code = 'Document Filings')
delete from Queries where Code = 'Document Filings'