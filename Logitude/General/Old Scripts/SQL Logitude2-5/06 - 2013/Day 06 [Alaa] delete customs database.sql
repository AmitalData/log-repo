delete from QueryColumns where QueryId in (select id from Queries where Tenant=0 and  ObjectTableId in ( select Id from ObjectTables where name like '%customs.%'))
GO

delete  from AdvancedQueryFilters where QueryId in (select id from Queries where Tenant=0 and ObjectTableId in ( select Id from ObjectTables where name like '%customs.%'))
GO

delete from Queries where ObjectTableId in (select id from ObjectTables where tenant=0 and name like'%customs.%')
GO
delete  from ScreenFields where ScreenId  in (select id from Screens where Tenant=0  and ObjectTableId in ( select Id from ObjectTables where name like '%customs.%'))
GO


delete from ObjectFields where ObjectTableId in  (select id from ObjectTables where tenant=0 and name like'%customs.%')
GO

--select * from ObjectFields where ObjectTableId in  (select id from ObjectTables where tenant=0 and name like'%customs.%')

delete from RoleFeatures where FeatureId in ( select Id from Features where Tenant=0 and ObjectTableId in  (select id from ObjectTables where  name like'%customs.%'))
GO

delete FROM ObjectTableTabs where FeatureId in ( select Id from Features where Tenant=0 and ObjectTableId in  (select id from ObjectTables where  name like'%customs.%'))
GO

delete from MenuButtons where EventCode = 'SendDeclaration'
go

delete from MenuButtons where EventCode = 'More'
go

delete from Features where ObjectTableId in  (select id from ObjectTables where tenant=0 and name like'%customs.%')
GO

delete from TextCodes where ObjectTableId in  (select id from ObjectTables where tenant=0 and name like'%customs.%')
GO

delete from EntityLastActivities where Tenant=0 and  ObjectTableId in ( select id from ObjectTables where Name like '%customs.%')
GO

delete  from EntityLastActivities where ObjectTableId in  (select id from ObjectTables where tenant=0 and name like'%customs.%')
go

delete from MenuButtonGroups where ObjectTableId  in  (select id from ObjectTables where tenant=0 and name like'%customs.%')
go

delete from Screens where ObjectTableId  in  (select id from ObjectTables where tenant=0 and name like'%customs.%')

--ALTER TABLE objecttables drop  CONSTRAINT FK_ScreenObjectTable
--go
--alter table screens drop constraint FK_ObjectTableScreen
--go
--alter table communicationlogs drop constraint FK_CommunicationLogObjectTable
--go

delete from ObjectTables where tenant=0 and name like'%customs.%'

---------------------------
--ALTER TABLE [dbo].[ObjectTables]  WITH CHECK ADD  CONSTRAINT [FK_ScreenObjectTable] FOREIGN KEY([HeaderScreenId])
--REFERENCES [dbo].[Screens] ([Id])
--GO

--ALTER TABLE [dbo].[ObjectTables] CHECK CONSTRAINT [FK_ScreenObjectTable]
--GO


--ALTER TABLE [dbo].[Screens]  WITH CHECK ADD  CONSTRAINT [FK_ObjectTableScreen] FOREIGN KEY([ObjectTableId])
--REFERENCES [dbo].[ObjectTables] ([Id])
--GO

--ALTER TABLE [dbo].[Screens] CHECK CONSTRAINT [FK_ObjectTableScreen]
--GO

--ALTER TABLE [dbo].[CommunicationLogs]  WITH CHECK ADD  CONSTRAINT [FK_CommunicationLogObjectTable] FOREIGN KEY([ObjectTableId])
--REFERENCES [dbo].[ObjectTables] ([Id])
--GO

--ALTER TABLE [dbo].[CommunicationLogs] CHECK CONSTRAINT [FK_CommunicationLogObjectTable]
--GO
