

Create  PROCEDURE DeleteOldRecentShipmensRecords
(

@Tenant int                     --Input parameter ,  tenant to delete

)
AS
BEGIN
	
	
	
	PRINT @Tenant

		
		--users cursor
		DECLARE @UserId AS varchar(15)

BEGIN;
	DECLARE UsersCursor CURSOR READ_ONLY
	FOR	
	SELECT Id
	FROM Users 
	OPEN UsersCursor FETCH NEXT FROM UsersCursor INTO @UserId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		PRINT @UserId
DECLARE @recentList AS TABLE
(
ObjectTableId VARCHAR(15) NOT NULL

)

    INSERT INTO @recentList
    SELECT ObjectTableId
    FROM dbo.EntityLastActivities
    WHERE Tenant=1 AND UserId=@UserId
	GROUP BY ObjectTableId
	ORDER BY ObjectTableId

	SELECT * FROM @recentList

	DECLARE @ObjectTableId varchar(15)
	BEGIN;
	DECLARE ObjectTablesCursor CURSOR READ_ONLY
	FOR	
	SELECT ObjectTableId
	FROM @recentList 
	OPEN ObjectTablesCursor FETCH NEXT FROM ObjectTablesCursor INTO @ObjectTableId
	WHILE @@FETCH_STATUS = 0
	BEGIN

	PRINT @ObjectTableId
	DELETE FROM dbo.EntityLastActivities
	WHERE id NOT IN(SELECT TOP 50 id from dbo.EntityLastActivities
	 where ObjectTableId=@ObjectTableId AND Tenant=@Tenant AND UserId=@UserId
	  ORDER BY ActivityDate desc) and ObjectTableId=@ObjectTableId AND Tenant=@Tenant AND UserId=@UserId



	FETCH NEXT FROM ObjectTablesCursor INTO @ObjectTableId
	END
	CLOSE ObjectTablesCursor
	DEALLOCATE ObjectTablesCursor
END


	FETCH NEXT FROM UsersCursor INTO @UserId
	END
	CLOSE UsersCursor
	DEALLOCATE UsersCursor
END


END