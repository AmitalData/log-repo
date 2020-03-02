--MetaData All Scripts: Never Apply these scripts
--drop procedure [dbo].[usp_ObjectTableMetadata]
create PROCEDURE [dbo].[usp_DeleteObjectTableMetadata]
(
    @pTableName    varchar(50)
)
AS

 
 
 Declare @ObjectTableId As varchar(15)

 Begin
 
 SELECT TOP 1 @ObjectTableId = Id FROM ObjectTables where name = @pTableName
--*--Delete--*--
--ObjectFields
delete from objectfields where tenant = 0 and ObjectTableId = @ObjectTableId
delete from querycolumns where tenant = 0 and (ObjectFieldCode not like 'customs.%' or ObjectFieldCode is null) and userid is null  and QueryCode in (select UniqueCode from Queries where ObjectTableId = @ObjectTableId)
delete from AdvancedQueryFilters where tenant = 0 and (ObjectFieldCode not like 'customs.%' or ObjectFieldCode is null) and userid is null and QueryCode in  (select UniqueCode from Queries where ObjectTableId = @ObjectTableId)

delete from RuleConditionFields where tenant = 0  and (ObjectFieldCode not like 'customs.%' or ObjectFieldCode is null) and ObjectTableRuleId not in (select id from ObjectTableRules where SystemLevel=0 )  and ObjectTableRuleId in (select id from ObjectTableRules  where ObjectTableId = @ObjectTableId)
delete from ObjectTableRuleFields where tenant = 0 and (ObjectFieldCode not like 'customs.%' or ObjectFieldCode is null) and systemlevel = 1 and ObjectTableRuleId in (select id from ObjectTableRules  where ObjectTableId = @ObjectTableId)
delete from ObjectTableRules where tenant = 0 and (TriggerFieldCode not like 'customs.%' or TriggerFieldCode is null) and systemlevel = 1 and Id in (select id from ObjectTableRules  where ObjectTableId = @ObjectTableId)

--Screens
delete from ScreenFields where tenant = 0 and (ObjectFieldCode not like 'customs.%' or ObjectFieldCode is null) and ScreenCode in (select code from Screens where ObjectTableId = @ObjectTableId)
delete from screens where tenant = 0 and (Code not like 'customs.%' or Code is null) and ObjectTableId = @ObjectTableId

--Queries
delete from Queries where tenant = 0 and (Code not like 'customs.%' or Code is null) and userid is null and systemlevel = 1 and ObjectTableId = @ObjectTableId

--TextCodes
delete from MenuButtons where tenant = 0 and (LabelTextCodeCode not like 'customs.%' or LabelTextCodeCode is null) and MenuButtonGroupId in (select id from MenuButtonGroups where ObjectTableId = @ObjectTableId)
delete from ObjectTableTabs where tenant = 0 and (TabNameTextCodeCode not like 'customs.%' or TabNameTextCodeCode is null) and ObjectTableId = @ObjectTableId
delete from textcodes where tenant = 0 and (code not like 'customs.%' or code is null) and code not in (select NameTextCodeCode from queries where tenant=0 and userid is not null and SystemLevel=0 and NameTextCodeCode is not null) and code not in (select ShortTextCodeCode from tips)  and ObjectTableId = @ObjectTableId

--Features
delete from Features where tenant = 0 and (NameTextCodeCode not like 'customs.%' or NameTextCodeCode is null) and ObjectTableId = @ObjectTableId
 
-----------
End