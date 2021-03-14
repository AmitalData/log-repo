CREATE NONCLUSTERED INDEX [IX_dw_Addresses_AddressTypeId_CardId] ON[dbo].[dw_Addresses]([AddressTypeId],[CardId])
CREATE NONCLUSTERED INDEX [IX_dw_Partners_SalesmanUserId_AutomaticLastUpdateDate] ON[dbo].[dw_Partners]([SalesmanUserId],[AutomaticLastUpdateDate])
