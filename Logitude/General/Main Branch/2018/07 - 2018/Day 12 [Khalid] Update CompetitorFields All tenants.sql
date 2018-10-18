

declare @CompetitorId varchar(15)
 declare @CompetitorFields varchar(1000)
 declare @CustomerCompetitor AS varchar(15)


DECLARE CustomerCompetitorCursor CURSOR READ_ONLY
	FOR	
	SELECT distinct customerid
	FROM CustomerCompetitors 
	OPEN CustomerCompetitorCursor FETCH NEXT FROM CustomerCompetitorCursor INTO  @CustomerCompetitor 
	WHILE @@FETCH_STATUS = 0
	BEGIN
	 

DECLARE @Names VARCHAR(8000) 
set @Names='!';
SELECT @Names = COALESCE(@Names + ',', '') + CompetitorId 
FROM CustomerCompetitors where CustomerId = @CustomerCompetitor


set @Names =REPLACE(@Names,'!,','');  
update Customers set COmpetitorFields=@Names where Id=@CustomerCompetitor
print @Names

FETCH NEXT FROM CustomerCompetitorCursor INTO @CustomerCompetitor
		END

CLOSE CustomerCompetitorCursor	
DEALLOCATE CustomerCompetitorCursor	

	