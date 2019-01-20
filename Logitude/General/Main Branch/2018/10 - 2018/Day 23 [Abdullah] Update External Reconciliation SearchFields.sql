--
-- USAGE: Update search values of external reconciliations depending on another tables
--

--select * from ExternalReconciliations

BEGIN
	DROP TABLE #TempRecos
	Select * Into   #TempRecos
	From   ExternalReconciliations

	Declare @updatedCount int = 0
	WHILE EXISTS(SELECT * FROM #TempRecos)
	Begin
		Declare @RecoSearchFields nvarchar(max) = ''
		Declare @RecoId varchar(15)
		
		Declare @recoNumber varchar(30)
		
		
		Select Top 1 * into #reco From #TempRecos
		set @RecoId = (Select Top 1 Id From #TempRecos)
		
		
		
		
		--add reco searchfields
		set @RecoSearchFields += (select CONVERT(varchar(30), #reco.ReconciliationNumber) from #reco)
		set @RecoSearchFields += ','
	
		--get GLAccount
		set @RecoSearchFields += (select GL.DisplayNumber from GLAccounts GL where Id in (select GLAccountId from #reco))
		set @RecoSearchFields += ','
	
		--get reco lines
		select ERT.LedgerTransactionId,ERT.ExternalPageLineId into #RecoLines  from ExternalReconciliationLines ERT where ERT.ReconciliationId = @RecoId 

		
		----get transactions
		select * into #RecoTransactions from LedgerTransactions LT where LT.Id in (select LedgerTransactionId from #RecoLines where #RecoLines.LedgerTransactionId is not null)
		select * into #RecoPageLines from ReconcileExternalPageLines REPL where REPL.Id in (select #RecoLines.ExternalPageLineId from #RecoLines where #RecoLines.ExternalPageLineId is not null)
	
	
		--loop transactions
		Declare @transId varchar(15)
		WHILE EXISTS(SELECT * FROM #RecoTransactions)
		begin
			set @transId = (Select Top 1 Id From #RecoTransactions)
			Select Top 1 * into #transaction From #RecoTransactions
			set @RecoSearchFields += (select ISNULL(CONVERT(nvarchar(50),#transaction.Reference1),'')  from #transaction)
			set @RecoSearchFields += ','
			set @RecoSearchFields += (select ISNULL(CONVERT(nvarchar(50),#transaction.Reference2),'') from #transaction)
			set @RecoSearchFields += ','
			set @RecoSearchFields += (select ISNULL(CONVERT(nvarchar(50),#transaction.Reference3),'') from #transaction)
			set @RecoSearchFields += ','
			
			delete #RecoTransactions Where Id = @transId
			DROP TABLE #transaction
		end
		
		--loop page lines
		Declare @plineId varchar(15)
		WHILE EXISTS(SELECT * FROM #RecoPageLines)
		begin
			Select Top 1 @plineId = Id From #RecoPageLines
			Select Top 1 * into #pageLine From #RecoPageLines
			set @RecoSearchFields += (select ISNULL(CONVERT(nvarchar(50),#pageLine.Reference),'') from #pageLine)
			set @RecoSearchFields += ','
			
			delete #RecoPageLines Where Id = @plineId
			DROP TABLE #pageLine
		end
		
		
		DROP TABLE #RecoPageLines
		DROP TABLE #RecoTransactions
		DROP TABLE #RecoLines
		DROP TABLE #reco
		Delete #TempRecos Where Id = @RecoId
		
		
		update ExternalReconciliations 
		set SearchFields = @RecoSearchFields 
		where ExternalReconciliations.Id = @RecoId
		
		set @updatedCount += 1
		
	End
	
	select @updatedCount as 'Rows updated'
	
	
END

--DROP TABLE #reco