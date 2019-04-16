delete from AdvancedQueryFilters where QueryId=(Select Id from Queries where Code='AFTA')
delete from QueryColumns where QueryId=(Select Id from Queries where Code='AFTA')
delete from Queries where Code ='AFTA'

