declare @Tenant as int
declare @PeriodId as varchar(15)
declare @Year as int
declare @OpenMonth as int
declare @CloseMonth as int
 
BEGIN  
		DECLARE DataCursor1 CURSOR READ_ONLY
		FOR
		SELECT [Year],Tenant,OpenMonth ,ClosedMonth
		FROM AccountingPeriods where PeriodTypeCode ='2'
		OPEN DataCursor1 FETCH NEXT FROM DataCursor1 INTO @Year, @Tenant ,@OpenMonth ,@CloseMonth
		WHILE @@FETCH_STATUS = 0
		BEGIN
		set  @PeriodId =  (select Id from AccountingPeriods where PeriodTypeCode ='3' and Tenant = @Tenant and [Year] =@Year )
        IF  (@PeriodId IS Null)
        begin
        EXECUTE usp_GetNextTableIdValue @PeriodId OUTPUT,'AccountingPeriod'
        Insert into AccountingPeriods (Id,Tenant,[Year],PeriodTypeCode,OpenMonth,ClosedMonth) values (@PeriodId,@Tenant,@Year,'3',@OpenMonth,@CloseMonth)
		end
		FETCH NEXT FROM DataCursor1 INTO @Year, @Tenant,@OpenMonth,@CloseMonth
		END
		CLOSE DataCursor1
		DEALLOCATE DataCursor1
END
 
 