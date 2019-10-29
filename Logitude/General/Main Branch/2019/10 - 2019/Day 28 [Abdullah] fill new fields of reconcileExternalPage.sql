
-- [Usage] fill the new fields of ReconcileExternalPage from bankAccountId
--          UNCOMMENT SCRIPT -> EXCUTE


--DECLARE @bankAccountTableId varchar(15) = (select Id from ObjectTables where Name = 'BankAccount') 
--select @bankAccountTableId

--update	r 
--set		r.ObjectTableId = @bankAccountTableId , r.EntityId = r.BankAccountId
--from	ReconcileExternalPages r


--select r.ObjectTableId,r.EntityId, r.BankAccountId from	ReconcileExternalPages r
