--delete from EntityLastActivities where ActivityDate < DATEADD(month,-12,getdate())

DECLARE @groups TABLE (UserId varchar(15), EntityId varchar(15), ObjectTableId varchar(15),Tenant int,CountOfRecords int)
INSERT INTO @groups
SELECT UserId, EntityId, ObjectTableId,Tenant,count(*) as CountOfRecords
FROM EntityLastActivities group by UserId,EntityId,ObjectTableId,Tenant
select * from @groups
--DECLARE @UserId AS VARCHAR(15)
--DECLARE @EntityId AS VARCHAR(15)
--DECLARE @ObjectTableId AS VARCHAR(15)
--DECLARE @Tenant AS INT

--BEGIN;

--	DECLARE RecentShipmentsRemoverCursor CURSOR READ_ONLY
--	FOR	
--	SELECT UserId, EntityId,ObjectTableId,Tenant
--	FROM @groups	 
--	OPEN RecentShipmentsRemoverCursor FETCH NEXT FROM RecentShipmentsRemoverCursor INTO @UserId, @EntityId,@ObjectTableId,@Tenant
--	WHILE @@FETCH_STATUS = 0
--		BEGIN
		
--		delete from EntityLastActivities WHERE UserId=@UserId and EntityId=@EntityId and ObjectTableId=@ObjectTableId and Tenant=@Tenant and Id NOT IN (SELECT TOP 10 Id FROM EntityLastActivities where UserId=@UserId and EntityId=@EntityId and ObjectTableId=@ObjectTableId and Tenant=@Tenant order by ActivityDate desc)

--	FETCH NEXT FROM RecentShipmentsRemoverCursor INTO @UserId, @EntityId,@ObjectTableId,@Tenant
--	END
--	CLOSE RecentShipmentsRemoverCursor
--	DEALLOCATE RecentShipmentsRemoverCursor	
--END
--go


--select UserId,ObjecttableId,ActivityDate,EntityId from EntityLastActivities order by UserId desc,ObjecttableId desc,ActivityDate desc,EntityId desc

declare @id as varchar(15)
DECLARE @UserId AS VARCHAR(15)
DECLARE @EntityId AS VARCHAR(15)
DECLARE @ObjectTableId AS VARCHAR(15)
DECLARE @Tenant AS INT
Declare @currentUserId as varchar(15)
Declare @currentEntityId as varchar(15)
declare @currentObjectTableId as varchar(15)
declare @counter as int
declare @entityCounter as int

set @currentUserId =''
set @currentEntityId =''
set @currentObjectTableId=''
set @counter =0
set @entityCounter =0

BEGIN;

	DECLARE RecentShipmentsRemoverCursor CURSOR READ_ONLY
	FOR	
	SELECT UserId, EntityId,ObjectTableId,Tenant,Id
	FROM EntityLastActivities order by UserId desc,ObjecttableId desc,ActivityDate desc,EntityId desc 
	OPEN RecentShipmentsRemoverCursor FETCH NEXT FROM RecentShipmentsRemoverCursor INTO @UserId, @EntityId,@ObjectTableId,@Tenant,@id
	WHILE @@FETCH_STATUS = 0
		BEGIN
		
		if(@counter=10 and @currentUserId=@UserId and @currentObjectTableId=@ObjectTableId)
		begin
		delete from EntityLastActivities where id=@id
		end

		--if(@entityCounter>15 and @currentUserId=@UserId and @currentObjectTableId=@ObjectTableId)
		--begin
		--delete from EntityLastActivities where id=@id
		--end

		if(@currentUserId <> @UserId)
		Begin
		print @currentUserId
		print @currentObjectTableId
		print @counter
		set @counter=0
		set @currentUserId =@UserId
		set @currentObjectTableId=@ObjectTableId
		set @currentEntityId=@EntityId
		End

		Else
		Begin

		if(@currentObjectTableId<>@ObjectTableId)
		begin
		print @currentUserId
		print @currentObjectTableId
		print @counter
		set @counter=0
		set @currentObjectTableId=@ObjectTableId
		set @currentEntityId=@EntityId
		end

		else
		begin

		if(@currentEntityId<>@EntityId)
		begin
		set @counter=@counter+1
		set @currentEntityId=@EntityId
		set @entityCounter=0
		end

		else if(@currentEntityId=@EntityId)
		begin

		set @entityCounter=@entityCounter+1

		end

		

		end
		End

		

	FETCH NEXT FROM RecentShipmentsRemoverCursor INTO @UserId,@EntityId,@ObjectTableId,@Tenant,@id
	END
	CLOSE RecentShipmentsRemoverCursor
	DEALLOCATE RecentShipmentsRemoverCursor	
END
go


       