declare @createdCount int = 0

declare @bankAccountObjectTableId varchar(15) = (select id from ObjectTables where Name = 'BankAccount')

declare @id varchar(15)
declare @Tenant int
declare @LastPageNumber varchar(15)
declare @LastPageEndDate datetime
declare @LastPageCloseBalance decimal

DECLARE db_cursor CURSOR FOR 

	select Id,Tenant,LastPageNumber,LastPageEndDate,LastPageCloseBalance 
	from BankAccounts 
	where LastPageNumber is not null

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @id,@tenant,@LastPageNumber,@LastPageEndDate,@LastPageCloseBalance
WHILE @@FETCH_STATUS = 0  
BEGIN
	--
	
	BEGIN TRANSACTION [Tran1]

	  BEGIN TRY
		--print 'Create for Bank: ' + @id;
		
		
		INSERT INTO ExternalPageAdditionalDatas
		   (ObjectTableId
		   ,EntityId
		   ,Tenant
		   ,LastPageNumber
		   ,LastPageEndDate
		   ,LastPageCloseBalance)
		VALUES
		   (@bankAccountObjectTableId
		   ,@id
		   ,@tenant
		   ,@LastPageNumber
		   ,@LastPageEndDate
		   ,@LastPageCloseBalance)
		   
		   update BankAccounts set @LastPageNumber = null where id = null
		   
		---- select created additional data
		--select * from ExternalPageAdditionalDatas where entityid = @id and objecttableid=@bankAccountObjectTableId

		--print '-- fields: ' + @bankAccountObjectTableId + ',' + @id+ ',' + @LastPageNumber
	
	
	
		set @createdCount = @createdCount + 1;
		 

			print 'Created for Bank: ' + @id

		  COMMIT TRANSACTION [Tran1]

	  END TRY

	  BEGIN CATCH
			print 'ERROR Catched for Bank: '+ @id
		  ROLLBACK TRANSACTION [Tran1]

	  END CATCH  
	  
	 
	--
	      
    FETCH NEXT FROM db_cursor INTO @id,@tenant,@LastPageNumber,@LastPageEndDate,@LastPageCloseBalance 
END 

CLOSE db_cursor  
DEALLOCATE db_cursor 

print ''
print '-----------------------------------'
print '- Created: ' + str(@createdCount)
print '-----------------------------------'


