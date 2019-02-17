delete   ObjectFields where HelpTextCodeId not in (select id from TextCodes)
 
delete ScreenFields WHERE ObjectFieldId = (select id from ObjectFields WHERE FieldName = 'CreateDate' and ObjectTableId = (select id from ObjectTables where Name = 'User'))
delete QueryColumns WHERE ObjectFieldId = (select id from ObjectFields WHERE FieldName = 'CreateDate' and ObjectTableId = (select id from ObjectTables where Name = 'User'))
--delete ObjectFields WHERE FieldName = 'CreateDate' and ObjectTableId = (select id from ObjectTables where Name = 'User')
--delete TextCodes where Id =(select FullNameTextCodeId from ObjectFields WHERE FieldName = 'CreateDate' and ObjectTableId = (select id from ObjectTables where Name = 'User'))



delete ScreenFields WHERE ObjectFieldId = (select id from ObjectFields WHERE FieldName = 'ShipperContactId' and ObjectTableId = (select id from ObjectTables where Name = 'Quote'))
delete QueryColumns WHERE ObjectFieldId = (select id from ObjectFields WHERE FieldName = 'ShipperContactId' and ObjectTableId = (select id from ObjectTables where Name = 'Quote'))
delete ObjectFields WHERE FieldName = 'ShipperContactId' and ObjectTableId = (select id from ObjectTables where Name = 'Quote')
delete TextCodes where Id =(select FullNameTextCodeId from ObjectFields WHERE FieldName = 'ShipperContactId' and ObjectTableId = (select id from ObjectTables where Name = 'Quote'))


delete ScreenFields WHERE ObjectFieldId = (select id from ObjectFields WHERE FieldName = 'FromPort' and ObjectTableId = (select id from ObjectTables where Name = 'Quote'))
delete QueryColumns WHERE ObjectFieldId = (select id from ObjectFields WHERE FieldName = 'FromPort' and ObjectTableId = (select id from ObjectTables where Name = 'Quote'))
delete ObjectFields WHERE FieldName = 'FromPort' and ObjectTableId = (select id from ObjectTables where Name = 'Quote')
delete TextCodes where Id =(select FullNameTextCodeId from ObjectFields WHERE FieldName = 'FromPort' and ObjectTableId = (select id from ObjectTables where Name = 'Quote'))

delete ScreenFields WHERE ObjectFieldId = (select id from ObjectFields WHERE FieldName = 'ToPort' and ObjectTableId = (select id from ObjectTables where Name = 'Quote'))
delete QueryColumns WHERE ObjectFieldId = (select id from ObjectFields WHERE FieldName = 'ToPort' and ObjectTableId = (select id from ObjectTables where Name = 'Quote'))
delete ObjectFields WHERE FieldName = 'ToPort' and ObjectTableId = (select id from ObjectTables where Name = 'Quote')
delete TextCodes where Id =(select FullNameTextCodeId from ObjectFields WHERE FieldName = 'ToPort' and ObjectTableId = (select id from ObjectTables where Name = 'Quote'))

delete ScreenFields WHERE ObjectFieldId = (select id from ObjectFields WHERE FieldName = 'FollowUpDate' and ObjectTableId = (select id from ObjectTables where Name = 'Quote'))
delete QueryColumns WHERE ObjectFieldId = (select id from ObjectFields WHERE FieldName = 'FollowUpDate' and ObjectTableId = (select id from ObjectTables where Name = 'Quote'))
delete ObjectFields WHERE FieldName = 'FollowUpDate' and ObjectTableId = (select id from ObjectTables where Name = 'Quote')
delete TextCodes where Id =(select FullNameTextCodeId from ObjectFields WHERE FieldName = 'FollowUpDate' and ObjectTableId = (select id from ObjectTables where Name = 'Quote'))


delete ScreenFields WHERE ObjectFieldId = (select id from ObjectFields WHERE FieldName = 'FollowUpType' and ObjectTableId = (select id from ObjectTables where Name = 'Quote'))
delete QueryColumns WHERE ObjectFieldId = (select id from ObjectFields WHERE FieldName = 'FollowUpType' and ObjectTableId = (select id from ObjectTables where Name = 'Quote'))
delete ObjectFields WHERE FieldName = 'FollowUpType' and ObjectTableId = (select id from ObjectTables where Name = 'Quote')
delete TextCodes where Id =(select FullNameTextCodeId from ObjectFields WHERE FieldName = 'FollowUpType' and ObjectTableId = (select id from ObjectTables where Name = 'Quote'))


delete ScreenFields WHERE ObjectFieldId = (select id from ObjectFields WHERE FieldName = 'FollowUpTypeId' and ObjectTableId = (select id from ObjectTables where Name = 'Quote'))
delete QueryColumns WHERE ObjectFieldId = (select id from ObjectFields WHERE FieldName = 'FollowUpTypeId' and ObjectTableId = (select id from ObjectTables where Name = 'Quote'))
delete ObjectFields WHERE FieldName = 'FollowUpTypeId' and ObjectTableId = (select id from ObjectTables where Name = 'Quote')
delete TextCodes where Id =(select FullNameTextCodeId from ObjectFields WHERE FieldName = 'FollowUpTypeId' and ObjectTableId = (select id from ObjectTables where Name = 'Quote'))


delete ScreenFields WHERE ObjectFieldId = (select id from ObjectFields WHERE FieldName = 'FollowUpNotes' and ObjectTableId = (select id from ObjectTables where Name = 'Quote'))
delete QueryColumns WHERE ObjectFieldId = (select id from ObjectFields WHERE FieldName = 'FollowUpNotes' and ObjectTableId = (select id from ObjectTables where Name = 'Quote'))
delete ObjectFields WHERE FieldName = 'FollowUpNotes' and ObjectTableId = (select id from ObjectTables where Name = 'Quote')
delete TextCodes where Id =(select FullNameTextCodeId from ObjectFields WHERE FieldName = 'FollowUpNotes' and ObjectTableId = (select id from ObjectTables where Name = 'Quote'))
delete TextCodes where Code = 'QuoteTemplate.S.ShowTotalinLocalCurrency'
delete  TextCodes where Code = 'Quote.F.ToPort.Short'
delete  TextCodes where Code = 'Quote.F.FromPort.Short'
delete  TextCodes where Code = 'Quote.ShipperContactHelpText'
--delete  TextCodes where Code = 'Quote.F.ShipperContactId.Short'
delete  TextCodes where Code = 'Quote.FromPortCodeHelpText'
delete  TextCodes where Code = 'Quote.ToPortCodeHelpText'
delete  TextCodes where Code = 'Quote.FUDateHelpText'
delete  TextCodes where Code = 'Quote.FUTypeHelpText'
delete  TextCodes where Code = 'Quote.FUTypeIdHelpText'
delete  TextCodes where Code = 'Quote.FUNotesHelpText'
delete  TextCodes where Code = 'Quote.FollowUpTypeIdHelpText'
--delete  TextCodes where Code = 'Quote.CH.FollowUpDateListLable'
--delete  TextCodes where Code = 'Quote.CH.FollowUpTypeListLable'
--delete  TextCodes where Code = 'Quote.CH.FollowUpNotesListLable'
--delete  TextCodes where Code = 'User.Create DateHelpText'
--delete  TextCodes where Code = 'User.CH.CreateDateListLable'











