update Queries 
set EditWizardName = 'Logitude.BookingLib.Views.BookingWizard.BookingWizardEditControl' 
where ObjectTableId = (select Id from ObjectTables where Name = 'Booking')



declare @OriginalQueryId as varchar(15)
declare @MyQueryId as varchar(15)
declare @WizardName as varchar(100)

BEGIN
		DECLARE QueriesCursor CURSOR READ_ONLY
		FOR
		SELECT OriginalQueryId
		FROM Queries
		OPEN QueriesCursor FETCH NEXT FROM QueriesCursor INTO @OriginalQueryId
		WHILE @@FETCH_STATUS = 0
		BEGIN

			set @WizardName = (select EditWizardName from Queries where Id = @OriginalQueryId)
			
			update Queries set EditWizardName = @WizardName where OriginalQueryId = @OriginalQueryId

		FETCH NEXT FROM QueriesCursor INTO @OriginalQueryId

		END				
		CLOSE QueriesCursor
		DEALLOCATE QueriesCursor
END