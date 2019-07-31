
-- USAGE: fill searchfields of reconciliation by: [recoLines.searchfields]

declare @recoId varchar(15)
DECLARE db_cursor CURSOR FOR 

	select Id from Reconciliations

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @recoId
WHILE @@FETCH_STATUS = 0  
BEGIN
	-- fetch lines searchfileds
	declare @recoSearchFeilds varchar(max) = ''
	
	--****************************** nested cursor
				declare @recoLineSearchField varchar(max)
				DECLARE db_cursor2 CURSOR FOR 

					select SearchFields from ReconciliationLines where ReconciliationId = @recoId

				OPEN db_cursor2 
				FETCH NEXT FROM db_cursor2 INTO @recoLineSearchField
				WHILE @@FETCH_STATUS = 0  
				BEGIN
					
					set @recoSearchFeilds += @recoLineSearchField + ',' 
					      
					FETCH NEXT FROM db_cursor2 INTO @recoLineSearchField 
				END 

				CLOSE db_cursor2
				DEALLOCATE db_cursor2 
					
	
	--*******************************
	
	-- update reconciliation
	update Reconciliations set SearchFields = @recoSearchFeilds where Id = @recoId  
	      
    FETCH NEXT FROM db_cursor INTO @recoId 
END 

CLOSE db_cursor  
DEALLOCATE db_cursor 