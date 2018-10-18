
drop index IX_EntityId on EntityChanges
drop index IX_ObjectTableId on EntityChanges
drop index IX_Tenant on EntityChanges


CREATE NONCLUSTERED INDEX IX_EntityChange_EntityId_ObjectTableId_Tenant
ON [dbo].[EntityChanges] ([EntityId],[ObjectTableId],[Tenant])

