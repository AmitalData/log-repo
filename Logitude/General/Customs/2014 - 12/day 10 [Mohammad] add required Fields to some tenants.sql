
BEGIN;
declare @Tenant as int
declare @Id as varchar(15)
declare @ObjectFieldId as varchar(15)
declare @ObjectTableId as varchar(15)

	DECLARE myCursor CURSOR READ_ONLY
	FOR
	SELECT Id,Tenant,ObjectfieldId,ObjectTableId
	From Customs.CustomsRequiredFields
	where Tenant=1
	OPEN myCursor FETCH NEXT FROM myCursor INTO @Id,@Tenant,@ObjectFieldId,@ObjectTableId
	WHILE @@FETCH_STATUS = 0
	BEGIN
	declare @newId1 varchar(15)
	EXECUTE usp_GetNextTableIdValue @newId1 OUTPUT,'Customs.CustomsRequiredField'
	declare @newId2 varchar(15)
	EXECUTE usp_GetNextTableIdValue @newId2 OUTPUT,'Customs.CustomsRequiredField'
	declare @newId3 varchar(15)
	EXECUTE usp_GetNextTableIdValue @newId3 OUTPUT,'Customs.CustomsRequiredField'

	insert into customs.CustomsRequiredFields (Id,Tenant,ObjectfieldId,ObjectTableId) values (@newId1,84,@ObjectFieldId,@ObjectTableId)--84
	insert into customs.CustomsRequiredFields (Id,Tenant,ObjectfieldId,ObjectTableId) values (@newId2,85,@ObjectFieldId,@ObjectTableId)--85
	insert into customs.CustomsRequiredFields (Id,Tenant,ObjectfieldId,ObjectTableId) values (@newId3,86,@ObjectFieldId,@ObjectTableId)--86
	--delete from Customs.CustomsRequiredFields where ObjectTableId=@ObjectTableId and ObjectfieldId=@ObjectFieldId and Tenant=84
   
	FETCH NEXT FROM myCursor INTO  @Id,@Tenant,@ObjectFieldId,@ObjectTableId
	END
	CLOSE myCursor
	DEALLOCATE myCursor
END

--delete from customs.CustomsRequiredFields where tenant=86 

select * from customs.CustomsRequiredFields where tenant=84 order by ObjectTableId, ObjectfieldId
select * from customs.CustomsRequiredFields where tenant=1 order by ObjectTableId, ObjectfieldId
select * from customs.CustomsRequiredFields where tenant=0 order by ObjectTableId, ObjectfieldId

--delete from   customs.CustomsRequiredFields where tenant <>0 and tenant <>81
--select * from tenants order by id desc