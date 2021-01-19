
declare  @CardId varchar(15)

    DECLARE CardCursor CURSOR READ_ONLY
    FOR
    SELECT Id 
    From Cards  where Id not in (select CardId from CardSearches )
    OPEN CardCursor FETCH NEXT FROM CardCursor INTO  @CardId
    WHILE @@FETCH_STATUS = 0
    BEGIN
	execute usp_UpdateCardSearchFunction  @CardId
	
    FETCH NEXT FROM CardCursor INTO @CardId
    END
    CLOSE CardCursor
    DEALLOCATE CardCursor

 
