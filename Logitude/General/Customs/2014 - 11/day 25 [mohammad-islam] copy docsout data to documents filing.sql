
BEGIN;
declare @Id as varchar(15)
declare @Tenant as int
declare @ChildEntityId as varchar(15)
declare @ChildEntityReference as varchar(40)
declare @EntityId as varchar(15)
declare @IssuedByUserId as varchar(15)
declare @IssuedDate as datetime
declare @DocumentTypeId as varchar(15)
declare @Notes as varchar(250)
declare @ObjectTableId as varchar(15)
declare @Issued as bit
	DECLARE documentCursor CURSOR READ_ONLY
	FOR
	SELECT Id,Tenant,ChildEntityId,ChildEntityReference,EntityId,DocumentTypeId,Note,ObjectTableId
	From DocumentOuts
	OPEN documentCursor FETCH NEXT FROM documentCursor INTO @Id,@Tenant,@ChildEntityId,@ChildEntityReference,@EntityId,@DocumentTypeId,@Notes,@ObjectTableId
	WHILE @@FETCH_STATUS = 0
	BEGIN

	declare @codeCounter varchar(15)
	
	EXECUTE usp_GetNextTableCodeValue @codeCounter OUTPUT,'DocumentsFiling',@Tenant
	

   declare @email as varchar(60)
   declare @tenantString as varchar(50)
   set @tenantString=CONVERT(varchar(50), @Tenant)
   set @email='system@tenant'+@tenantString+'.com'
  declare @createdDate as datetime
  set @createdDate =GETDATE()
  declare @createdBy as varchar(15)
  set @createdBy=(select id from Contacts where Email=@email and Tenant=@Tenant)
  
   declare @existedId as varchar(15)
   set @existedId=NULL
   set @existedId =(select id from DocumentsFilings where Id=@Id)
   if(@existedId is null)
   begin
   insert into DocumentsFilings (Id,Tenant,ChildEntityId,ChildEntityReference,EntityId,UpdatedByUserId,UpdateDate,DocumentTypeId,Notes,ObjectTableId,CreateDate,CreatedByUserId,DirectionCode,OwnerId,HasCopies,Code) 
   values (@Id,@Tenant,@ChildEntityId,@ChildEntityReference,@EntityId,@createdBy,@createdDate,@DocumentTypeId,@Notes,@ObjectTableId,@createdDate,@createdBy,'O',@createdBy,0,@codeCounter)
   print @email
   end
   
	FETCH NEXT FROM documentCursor INTO @Id,@Tenant,@ChildEntityId,@ChildEntityReference,@EntityId,@DocumentTypeId,@Notes,@ObjectTableId
	END
	CLOSE documentCursor
	DEALLOCATE documentCursor
END

--update TenantManagements set IsDistributorSupportEnabled=1,IsSystemSupportEnabled=1
--delete from followups where externaldocumentid not in (select id from DocumentsFilings)
--delete from documentoutcopies where documentoutid in (select id from DocumentOuts where id not in (select id from DocumentsFilings))
--delete from communicationattachments where communicationlogid in (select id from communicationlogs where DocumentOutId in (select id from DocumentOuts where id not in (select id from DocumentsFilings)))
--delete from communicationlogs where documentoutid in (select id from DocumentOuts where id not in (select id from DocumentsFilings))

--delete from DocumentOuts where id not in (select id from DocumentsFilings)

--select * from Contacts where Email like '%system@tenant%' and tenant=9
--select * from communicationlogs where documentoutid is not null