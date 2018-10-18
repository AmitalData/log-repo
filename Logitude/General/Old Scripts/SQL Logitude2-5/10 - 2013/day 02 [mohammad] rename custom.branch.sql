UPDATE dbo.ObjectTables SET Name='Customs.CustomsBranch' WHERE Name='Customs.Branch'

EXEC sp_rename '[Customs].[Branches]', 'CustomsBranches';