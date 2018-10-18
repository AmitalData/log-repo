DECLARE @listofContactIds TABLE
(
  id varchar(max)
)

declare @id varchar(15)
declare @tenant int

BEGIN 
		DECLARE primaryContactsCursor CURSOR READ_ONLY
		FOR
		SELECT Id,Tenant
		FROM Cards
		OPEN primaryContactsCursor FETCH NEXT FROM primaryContactsCursor INTO @id,@tenant
		WHILE @@FETCH_STATUS = 0
			BEGIN

		insert into @listofContactIds(id) select Id from Contacts where id	in (select contactid from CardContacts where CardId=@id and Tenant=@tenant) and Tenant=@tenant
		declare @count as int
		set @count=(select count(*) from @listofContactIds)
		if(@count>0)
		begin
			print 'card' + @id
		   declare @contactId varchar(15)
		DECLARE @listofsplittedIds TABLE
        (
           id varchar(max)
        )
		DECLARE splittedIdsCursor CURSOR READ_ONLY
		FOR
		select id from @listofContactIds
		OPEN splittedIdsCursor FETCH NEXT FROM splittedIdsCursor INTO @contactId
		WHILE @@FETCH_STATUS = 0
			BEGIN
			
			Declare @toSplitId varchar(15) = @contactId
            Declare @individual varchar(20) = null
		
			print 'contact' + @contactId + 'for card' +@id

			WHILE LEN(@toSplitId) > 0
            BEGIN
              IF PATINDEX('%-%',@toSplitId) > 0
                 BEGIN
                   SET @individual = SUBSTRING(@toSplitId, 0, PATINDEX('%-%',@toSplitId))
                   SET @toSplitId = SUBSTRING(@toSplitId, LEN(@individual + '-') + 1,LEN(@toSplitId))
	               insert into @listofsplittedIds select @toSplitId
                 END
              ELSE
              BEGIN
                SET @individual = @toSplitId
                SET @toSplitId = NULL
              END
            END

			FETCH NEXT FROM splittedIdsCursor INTO  @contactId
			END
		CLOSE splittedIdsCursor
		DEALLOCATE splittedIdsCursor

		declare @databaseNumber varchar(10) = null
		Declare @toSplitAgainId varchar(15) =(select top(1) id from @listofContactIds)
			WHILE LEN(@toSplitAgainId) > 0
            BEGIN
              IF PATINDEX('%-%',@toSplitAgainId) > 0
                 BEGIN
                   SET @databaseNumber = SUBSTRING(@toSplitAgainId, 0, PATINDEX('%-%',@toSplitAgainId))
                   SET @toSplitAgainId = SUBSTRING(@toSplitAgainId, LEN(@databaseNumber + '-') + 1,LEN(@toSplitAgainId))
                 END
              ELSE
              BEGIN
                SET @toSplitAgainId = NULL
              END
            END

		declare @primaryId varchar(15)=@databaseNumber+'-'+(select top(1) id from @listofsplittedIds order by CONVERT(int,id,101))
		update cards set primarycontactid=@primaryId where id=@id
		delete from @listofsplittedIds
		end
		else
		begin
		update cards set primarycontactid=NULL where id=@id
		end
		delete from @listofContactIds
				FETCH NEXT FROM primaryContactsCursor INTO  @id,@tenant
			END
		CLOSE primaryContactsCursor
		DEALLOCATE primaryContactsCursor
END
     

		