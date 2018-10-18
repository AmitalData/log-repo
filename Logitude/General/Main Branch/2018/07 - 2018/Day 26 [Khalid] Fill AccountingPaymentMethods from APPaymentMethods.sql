declare @TBID as int
set @TBID = (select Id from  DBIdCounters where TableName='AccountingPaymentMethod' )
	if (@TBID is null)
		begin
			update  DBIdCounters set TableName='AccountingPaymentMethod'  where TableName='ARPAymentMethod'
		end

declare @Tenant as int
declare @Code as varchar(15)
declare @ExternalId as varchar(15)
declare @Id as varchar(15)
declare @Name as varchar(40)
declare @SearchFields as varchar(1000)
declare @AddedManually as bit
declare @InActive as bit
declare @NewId as varchar(15)
--APPaymentMethods
BEGIN 
	DECLARE APPaymentMEthodCursor CURSOR READ_ONLY
	FOR
	SELECT Code, Tenant,AccountingExternalId,Name,SearchFields,AddedManually,InActive
	FROM APPAymentMethods
	OPEN APPaymentMEthodCursor FETCH NEXT FROM APPaymentMEthodCursor INTO @Code, @Tenant ,@ExternalId   ,@Name,@SearchFields,@AddedManually,@InActive
	WHILE @@FETCH_STATUS = 0
	BEGIN
	set @Id = (Select Id from AccountingPaymentMethods where Code=@Code and Tenant=@Tenant)
		if (@Id is not null)
		begin
			update AccountingPaymentMethods set IsAP = 1,APExternalId=@ExternalId where Id = @Id and Tenant = @Tenant
		end
		else 
		begin
	EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'AccountingPaymentMethod'
	print(@NewId)
	insert into AccountingPaymentMethods(Id,Code, Name, SearchFields,Tenant,AddedManually,InActive,APExternalId,IsAP,IsAR) values (@NewId,@Code, @Name, @SearchFields,@Tenant,@AddedManually,@InActive ,@ExternalId,1,0)
          end
		FETCH NEXT FROM APPaymentMEthodCursor INTO  @Code, @Tenant ,@ExternalId   ,@Name,@SearchFields    ,@AddedManually,@InActive
    END
    CLOSE APPaymentMEthodCursor
    DEALLOCATE APPaymentMEthodCursor
END



--APPayments
BEGIN 
	DECLARE APPaymentCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant,PaymentMEthodId
	FROM APPAyments
	OPEN APPaymentCursor FETCH NEXT FROM APPaymentCursor INTO @Id, @Tenant ,@ExternalId
	WHILE @@FETCH_STATUS = 0
	BEGIN
		if (@ExternalId is not null)
		begin
	set @Code = (Select Code from APPaymentMethods where Id=@ExternalId and Tenant=@Tenant)
	set @NewId=(Select Id from AccountingPaymentMEthods where Code=@Code and Tenant=@Tenant)
	update APPayments set AccountingPaymentMethodId=@NewId where Id=@Id and Tenant=@Tenant		
	       end
		FETCH NEXT FROM APPaymentCursor INTO  @Id, @Tenant ,@ExternalId
    END
    CLOSE APPaymentCursor
    DEALLOCATE APPaymentCursor
END


delete from PackageFeatures where FeatureId=(Select Id from Features where Code ='APPAYMENTMETHODS')
delete from MenusTables where FeatureId=(Select Id from Features where Code ='APPAYMENTMETHODS')
delete from RoleFeatures where FeatureId=(Select Id from Features where Code ='APPAYMENTMETHODS')
delete from Features where Code='APPAYMENTMETHODS'

   If Exists 
(
    Select * 
    From   sys.indexes 
    Where  name = 'IX_AccountingPaymentMethodId' 
    And    Object_Id = Object_Id('dbo.APPayments')
)
begin
Drop Index IX_AccountingPaymentMethodId On dbo.APPayments;
end


ALTER TABLE APPayments
ALTER COLUMN AccountingPaymentMethodId varchar(15) NOT NULL
